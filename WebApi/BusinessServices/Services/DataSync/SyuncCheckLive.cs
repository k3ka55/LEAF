
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModelLocal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices.Services.DataSync
{
    public class SyuncCheckLive : ISyuncCheckLive
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();
        string fileName = "SCL_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public SyuncCheckLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getSyncDetails()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.SyncChecking = (from x in CloudDB.Sync_Checking
                                                where x.is_Syunc == false
                                                select new SyncCheckingEntity
                                                {
                                                    Sync_id = x.Sync_id,
                                                    Sync_Number = x.Sync_Number,
                                                    DC_Code = x.DC_Code,
                                                    Created_By = x.Created_By,
                                                    Created_Date = x.Created_Date,
                                                    Updated_By = x.Updated_By,
                                                    Updated_Date = x.Updated_Date,
                                                    is_Syunc = x.is_Syunc,
                                                    LineItems = (from a in CloudDB.Sync_Checking_Line_item
                                                                 where a.Sync_id == x.Sync_id
                                                                 select new SyncCheckingLineItem
                                                                 {
                                                                     SC_Line_Id = a.SC_Line_Id,
                                                                     Sync_id = a.Sync_id,
                                                                     Sync_Number = a.Sync_Number,
                                                                     SKU_ID = a.SKU_ID,
                                                                     SKU_Name = a.SKU_Name,
                                                                     UOM = a.UOM,
                                                                     Qty = a.Qty,
                                                                     Grade = a.Grade,
                                                                     CreatedBy = a.CreatedBy,
                                                                     CreatedDate = a.CreatedDate,
                                                                     UpdatedBy = a.UpdatedBy,
                                                                     UpdatedDate = a.UpdatedDate
                                                                 }).ToList()
                                                }).ToList();

                int count = STI_PUSH_ENTITY.grnrecord.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from SynchChecking --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }
        }

        public void setSyncDetails()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.SyncChecking != null)
            {
                foreach (var SyncfromLive in STI_PUSH_ENTITY.SyncChecking)
                {
                    var movedGRN = new Sync_Checking();
                    using (var scope1 = new TransactionScope())
                    {
                        movedGRN.Sync_Number = SyncfromLive.Sync_Number;
                        movedGRN.DC_Code = SyncfromLive.DC_Code;
                        movedGRN.Created_By = SyncfromLive.Created_By;
                        movedGRN.Created_Date = SyncfromLive.Created_Date;
                        movedGRN.Updated_By = SyncfromLive.Updated_By;
                        movedGRN.Updated_Date = SyncfromLive.Updated_Date;
                        movedGRN.is_Syunc = true;

                        CloudDB.Sync_Checking.Add(movedGRN);
                        CloudDB.SaveChanges();

                        scope1.Complete();
                    }

                    int grnID = movedGRN.Sync_id;
                    using (var tw = new StreamWriter(Path, true))
                    {
                        tw.WriteLine(movedGRN.Sync_Number + " Inserted into SyncChecking. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                        tw.Close();
                    }

                    foreach (var lieitem in SyncfromLive.LineItems)
                    {
                        var movedLineItem = new Sync_Checking_Line_item();
                        using (var scope3 = new TransactionScope())
                        {
                            movedLineItem.Sync_id = grnID;
                            movedLineItem.Sync_Number = lieitem.Sync_Number;
                            movedLineItem.SKU_ID = lieitem.SKU_ID;
                            movedLineItem.SKU_Name = lieitem.SKU_Name;
                            movedLineItem.UOM = lieitem.UOM;
                            movedLineItem.Qty = lieitem.Qty;
                            movedLineItem.Grade = lieitem.Grade;
                            movedLineItem.CreatedBy = lieitem.CreatedBy;
                            movedLineItem.CreatedDate = lieitem.CreatedDate;
                            movedLineItem.UpdatedBy = lieitem.UpdatedBy;
                            movedLineItem.UpdatedDate = lieitem.UpdatedDate;

                            CloudDB.Sync_Checking_Line_item.Add(movedLineItem);
                            CloudDB.SaveChanges();

                            scope3.Complete();
                        }
                    }
                    lcount++;
                }

                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into GRN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void Sync_Update()
        {
            int alcount = 0;

            foreach (var disp in STI_PUSH_ENTITY.SyncChecking)
            {
                using (var scope3 = new TransactionScope())
                {
                    Sync_Checking dispUpdate = CloudDB.Sync_Checking.Where(x => x.Sync_id == disp.Sync_id).FirstOrDefault();

                    dispUpdate.is_Syunc = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                alcount++;
            }
            int count = alcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Sync Filelds Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            STI_PUSH_ENTITY.SyncChecking = null;
        }
    }
}
