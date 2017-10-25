using System;
using System.Collections.Generic;
using System.Linq;
//using DataModelLocal;
using DataModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BusinessEntities.Entity;
using BusinessEntities;
using BusinessServices.Interfaces;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class GRNGetFromLocal : IGRNGetFromLocal
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        //LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();
        string fileName = "GRN_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public GRNGetFromLocal()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getGRN()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.grnrecord = (from x in CloudDB.GRN_Creation
                                             where x.is_Deleted == false && x.is_Syunc == false
                                             select new GrnEntity
                                             {
                                                 INW_Id = x.INW_Id,
                                                 GRN_Number = x.GRN_Number,
                                                 PO_Number = x.PO_Number,
                                                 GRN_Rls_Date = x.GRN_Rls_Date,
                                                 CDN_Number = x.CDN_Number,
                                                 Voucher_Type = x.Voucher_Type,
                                                 Customer_Id=x.Customer_Id,
                                                 Customer_Name=x.Customer_Name,
                                                 Customer_code=x.Customer_code,
                                                 Route=x.Route,
                                                 Route_Code=x.Route_Code,
                                                 Route_Id=x.Route_Id,
                                                 Sales_Person_Id=x.Sales_Person_Id,
                                                 Sales_Person_Name=x.Sales_Person_Name,
                                                 Vehicle_No=x.Vehicle_No,
                                                 SKU_Type_Id = x.SKU_Type_Id,
                                                 SKU_Type = x.SKU_Type,
                                                 STN_DC_Id = x.STN_DC_Id,
                                                 STN_DC_Code = x.STN_DC_Code,
                                                 STN_DC_Name = x.STN_DC_Name,
                                                 Supplier_Id = x.Supplier_Id,
                                                 Supplier_code = x.Supplier_code,
                                                 Supplier_Name = x.Supplier_Name,
                                                 DC_Code = x.DC_Code,
                                                 CreatedBy = x.CreatedBy,
                                                 CreatedDate = x.CreatedDate,
                                                 UpdatedBy = x.UpdatedBy,
                                                 UpdatedDate = x.UpdatedDate,
                                                 is_Deleted = x.is_Deleted,
                                                 GrnDetails = (from a in CloudDB.GRN_Line_item
                                                                where a.INW_id == x.INW_Id
                                                               select new GrnLineItemsEntity
                                                               {
                                                                   INW_id = a.INW_id,
                                                                   GRN_Number = a.GRN_Number,
                                                                   SKU_ID = a.SKU_ID,
                                                                   SKU_Code = a.SKU_Code,
                                                                   SKU_Name = a.SKU_Name,
                                                                   SKU_SubType_Id = a.SKU_SubType_Id,
                                                                   SKU_SubType = a.SKU_SubType,
                                                                   UOM = a.UOM,
                                                                   Strinkage_Qty=a.Strinkage_Qty,
                                                                   PO_QTY = a.PO_QTY,
                                                                   A_Accepted_Qty = a.A_Accepted_Qty,
                                                                   B_Accepted_Qty = a.B_Accepted_Qty,
                                                                   C_Accepted_Qty = a.C_Accepted_Qty,
                                                                   A_Accepted_Price = a.A_Accepted_Price,
                                                                   B_Accepted_Price = a.B_Accepted_Price,
                                                                   C_Accepted_Price = a.C_Accepted_Price,
                                                                   A_Converted_Qty = a.A_Converted_Qty,
                                                                   B_Converted_Qty = a.B_Converted_Qty,
                                                                   C_Converted_Qty = a.C_Converted_Qty,
                                                                   Billed_Qty = a.Billed_Qty,
                                                                   Price_Book_Id = a.Price_Book_Id,
                                                                   Price = a.Price,
                                                                   Remark = a.Remark,
                                                                   Tally_Status=a.Tally_Status,
                                                                   CreatedBy = a.CreatedBy,
                                                                   CreatedDate = a.CreatedDate,
                                                                   UpdatedBy = a.UpdatedBy,
                                                                   UpdatedDate = a.UpdatedDate
                                                               }).ToList()
                                             }).ToList();

                STI_PUSH_ENTITY.GRNNumGenClass = (from c in CloudDB.GRN_NUM_Generation
                                                 where c.DC_Code == dc_Code
                                                 select new GRNNumGen
                                                 {
                                                     GRN_Num_Gen_Id = c.GRN_Num_Gen_Id,
                                                     DC_Code = c.DC_Code,
                                                     Financial_Year = c.Financial_Year,
                                                     GRN_Last_Number = c.GRN_Last_Number
                                                 }).ToList();

                int count = STI_PUSH_ENTITY.grnrecord.Count;
                int alcount = STI_PUSH_ENTITY.GRNNumGenClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from GRN and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }
        }

        public void GRN_Single_Record_Update(string grn_number)
        {
            using (var scope3 = new TransactionScope())
            {
                GRN_Creation dispUpdate = CloudDB.GRN_Creation.Where(x => x.GRN_Number == grn_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(grn_number + " Sync Field Updated in GRN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void GRN_Update()
        {
            int alcount = 0;

            foreach (var disp in STI_PUSH_ENTITY.grnrecord)
            {
                using (var scope3 = new TransactionScope())
                {
                    GRN_Creation dispUpdate = CloudDB.GRN_Creation.Where(x => x.INW_Id == disp.INW_Id).FirstOrDefault();

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
                tw.WriteLine(count + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            STI_PUSH_ENTITY.grnrecord = null;
        }
    }
}
