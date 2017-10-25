using System;
using System.Collections.Generic;
using System.Linq;
//using DataModelLocal;
using DataModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class WastageGetFromLocal : IWasatageGetFromLocal
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        //LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();
        string fileName = "Wastage_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public WastageGetFromLocal()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getWastage()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.wastagerecord = (from x in CloudDB.Wastage_Creation
                                                 where x.is_Deleted == false && x.is_Syunc == false && x.Wastage_Approval_Flag == true
                                                 select new WastageEntity
                                                 {
                                                     Wastage_Id = x.Wastage_Id,
                                                     Wastage_Number = x.Wastage_Number,
                                                     DC_Id = x.DC_Id,
                                                     DC_Name = x.DC_Name,
                                                     DC_Code = x.DC_Code,
                                                     Wastage_Rls_Date = x.Wastage_Rls_Date,
                                                     Wastage_Type = x.Wastage_Type,
                                                     Wastage_raisedBy = x.Wastage_raisedBy,
                                                     Wastage_Approval_Flag = x.Wastage_Approval_Flag,
                                                     Wastage_approved_by = x.Wastage_approved_by,
                                                     Wastage_approved_user_id = x.Wastage_approved_user_id,
                                                     Wastage_approved_date = x.Wastage_approved_date,
                                                     Ref_Id = x.Ref_Id,
                                                     Ref_Number = x.Ref_Number,
                                                     Remark = x.Remark,
                                                     is_Deleted = x.is_Deleted,
                                                     Reject_Reason = x.Reject_Reason,
                                                     CreatedBy = x.CreatedBy,
                                                     CreatedDate = x.CreatedDate,
                                                     UpdatedBy = x.UpdatedBy,
                                                     UpdatedDate = x.UpdatedDate,
                                                     WastageLineDetails = (from a in CloudDB.Wastage_Line_item
                                                                            where a.Wastage_Id == x.Wastage_Id
                                                                           select new WastageLineItemEntity
                                                                           {
                                                                               Wastage_Id = a.Wastage_Id,
                                                                               Wastage_Number = a.Wastage_Number,
                                                                               SKU_ID = a.SKU_ID,
                                                                               SKU_Code = a.SKU_Code,
                                                                               SKU_Name = a.SKU_Name,
                                                                               SKU_Type_Id = a.SKU_Type_Id,
                                                                               SKU_Type = a.SKU_Type,
                                                                               Ref_Id = a.Ref_Id,
                                                                               Ref_Line_Id = a.Ref_Line_Id,
                                                                               UOM = a.UOM,
                                                                               Grade = a.Grade,
                                                                               Wastage_Qty = a.Wastage_Qty,
                                                                               Wasted_Qty_Price = a.Wasted_Qty_Price,
                                                                               Reason = a.Reason,
                                                                               Stock_Reduce_Flag = a.Stock_Reduce_Flag,
                                                                               is_Stk_Update = a.is_Stk_Update,
                                                                               CreatedBy = a.CreatedBy,
                                                                               CreatedDate = a.CreatedDate,
                                                                               UpdatedBy = a.UpdatedBy,
                                                                               UpdatedDate = a.UpdatedDate
                                                                           }).ToList()
                                                 }).ToList();

                STI_PUSH_ENTITY.WastageNumGenClass = (from c in CloudDB.Wastage_Auto_Num_Gen
                                                 where c.DC_Code == dc_Code
                                                 select new WastageNumGen
                                                 {
                                                     Id = c.Id,
                                                     DC_Code = c.DC_Code,
                                                     Wastage_Last_Number = c.Wastage_Last_Number
                                                 }).ToList();

                int count = STI_PUSH_ENTITY.wastagerecord.Count;
                int alcount = STI_PUSH_ENTITY.WastageNumGenClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from Wastage and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }
        }

        public void WSTG_Single_Record_Update(string wstg_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Wastage_Creation dispUpdate = CloudDB.Wastage_Creation.Where(x => x.Wastage_Number == wstg_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(wstg_number + " Sync Field Updated in Wastage. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void Wastage_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.wastagerecord)
            {
                using (var scope3 = new TransactionScope())
                {
                    Wastage_Creation dispUpdate = CloudDB.Wastage_Creation.Where(x => x.Wastage_Id == disp.Wastage_Id).FirstOrDefault();

                    dispUpdate.is_Syunc = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                lcount++;
            }
            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Updated in Wastage. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            STI_PUSH_ENTITY.wastagerecord = null;
        }

    }
}
