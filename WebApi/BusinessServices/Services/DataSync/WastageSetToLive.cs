using System;
using System.Collections.Generic;
using System.Linq;
using DataModelLocal;
//using DataModel;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entity;
using System.Transactions;
using BusinessServices.Interfaces;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class WastageSetToLive : IWastageSetToLive
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string fileName = "Wastage_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public WastageSetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void setWastage()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.wastagerecord != null)
            {
                foreach (var wstgfromLive in STI_PUSH_ENTITY.wastagerecord)
                {
                    var ExistData = CloudDB.Wastage_Creation.Where(x => x.Wastage_Number == wstgfromLive.Wastage_Number).FirstOrDefault();
                    if (ExistData == null)
                    {
                        
                        var movedWSTG = new Wastage_Creation();

                        using (var scope1 = new TransactionScope())
                        {
                            movedWSTG.Wastage_Number = wstgfromLive.Wastage_Number;
                            movedWSTG.DC_Id = wstgfromLive.DC_Id;
                            movedWSTG.DC_Name = wstgfromLive.DC_Name;
                            movedWSTG.DC_Code = wstgfromLive.DC_Code;
                            movedWSTG.Wastage_Rls_Date = wstgfromLive.Wastage_Rls_Date;
                            movedWSTG.Wastage_Type = wstgfromLive.Wastage_Type;
                            movedWSTG.Wastage_raisedBy = wstgfromLive.Wastage_raisedBy;
                            movedWSTG.Wastage_Approval_Flag = wstgfromLive.Wastage_Approval_Flag;
                            movedWSTG.Wastage_approved_by = wstgfromLive.Wastage_approved_by;
                            movedWSTG.Wastage_approved_user_id = wstgfromLive.Wastage_approved_user_id;
                            movedWSTG.Wastage_approved_date = wstgfromLive.Wastage_approved_date;
                            movedWSTG.Ref_Id = wstgfromLive.Ref_Id;
                            movedWSTG.Ref_Number = wstgfromLive.Ref_Number;
                            movedWSTG.Remark = wstgfromLive.Remark;
                            movedWSTG.is_Deleted = wstgfromLive.is_Deleted;
                            movedWSTG.Reject_Reason = wstgfromLive.Reject_Reason;
                            movedWSTG.CreatedBy = wstgfromLive.CreatedBy;
                            movedWSTG.CreatedDate = wstgfromLive.CreatedDate;
                            movedWSTG.UpdatedBy = wstgfromLive.UpdatedBy;
                            movedWSTG.UpdatedDate = wstgfromLive.UpdatedDate;

                            CloudDB.Wastage_Creation.Add(movedWSTG);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        int WSTGID = movedWSTG.Wastage_Id;
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(movedWSTG.Wastage_Number + " Inserted into Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        foreach (var lieitem in wstgfromLive.WastageLineDetails)
                        {
                            var movedLineItem = new Wastage_Line_item();
                            using (var scope3 = new TransactionScope())
                            {
                                movedLineItem.Wastage_Id = WSTGID;
                                movedLineItem.Wastage_Number = lieitem.Wastage_Number;
                                movedLineItem.SKU_ID = lieitem.SKU_ID;
                                movedLineItem.SKU_Code = lieitem.SKU_Code;
                                movedLineItem.SKU_Name = lieitem.SKU_Name;
                                movedLineItem.SKU_Type_Id = lieitem.SKU_Type_Id;
                                movedLineItem.SKU_Type = lieitem.SKU_Type;
                                movedLineItem.Ref_Id = lieitem.Ref_Id;
                                movedLineItem.Ref_Line_Id = lieitem.Ref_Line_Id;
                                movedLineItem.UOM = lieitem.UOM;
                                movedLineItem.Grade = lieitem.Grade;
                                movedLineItem.Wastage_Qty = lieitem.Wastage_Qty;
                                movedLineItem.Wasted_Qty_Price = lieitem.Wasted_Qty_Price;
                                movedLineItem.Reason = lieitem.Reason;
                                movedLineItem.Stock_Reduce_Flag = lieitem.Stock_Reduce_Flag;
                                if (wstgfromLive.Wastage_Type == "Process" || wstgfromLive.Wastage_Type == "Floor")
                                {
                                    movedLineItem.is_Stk_Update = false;
                                }
                                else
                                {
                                    movedLineItem.is_Stk_Update = true;
                                }
                                movedLineItem.CreatedBy = lieitem.CreatedBy;
                                movedLineItem.CreatedDate = lieitem.CreatedDate;
                                movedLineItem.UpdatedBy = lieitem.UpdatedBy;
                                movedLineItem.UpdatedDate = lieitem.UpdatedDate;

                                CloudDB.Wastage_Line_item.Add(movedLineItem);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }
                        }
                        lcount++;
                    }
                    else
                    {
                        WastageGetFromLocal csAnother = new WastageGetFromLocal();
                        csAnother.WSTG_Single_Record_Update(wstgfromLive.Wastage_Number);
                    }
                }
                int count = lcount;

                using (var scopeE = new TransactionScope())
                {
                    CloudDB.Wastage_proc();
                    CloudDB.SaveChanges();
                    scopeE.Complete();
                }

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into Wastage. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }

            if (STI_PUSH_ENTITY.WastageNumGenClass != null)
            {
                int alcount = 0; 
                foreach (var autoNums in STI_PUSH_ENTITY.WastageNumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.Wastage_Auto_Num_Gen.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Wastage_Last_Number = autoNums.Wastage_Last_Number;

                            CloudDB.Entry(list).State = EntityState.Modified;
                            CloudDB.SaveChanges();
                        }

                        scope3.Complete();
                    }
                    alcount++;
                }
                int count = alcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
              
                STI_PUSH_ENTITY.WastageNumGenClass = null;
            }
        }


    }
}
