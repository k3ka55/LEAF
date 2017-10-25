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
using System.Configuration;
using System.Data.Entity;
using BusinessServices.Interfaces;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class INVOICEGetFromLocal : IINVOICEGetFromLocal
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        // LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();

        string fileName = "INVOICE_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public INVOICEGetFromLocal()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getInvoice()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.invoicerecord = (from x in CloudDB.Invoice_Creation
                                                 where x.is_Deleted == false && x.is_Syunc == false && x.is_Invoice_Approved == true
                                                 select new InvoiceEntity
                                                 {
                                                     invoice_Id = x.invoice_Id,
                                                     Invoice_Number = x.Invoice_Number,
                                                     Dispatch_id = x.Dispatch_id,
                                                     Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                     Invoice_Type = x.Invoice_Type,
                                                     SKU_Type_Id = x.SKU_Type_Id,
                                                     SKU_Type = x.SKU_Type,
                                                     Invoice_Date = x.Invoice_Date,
                                                     Customer_Id = x.Customer_Id,
                                                     Customer_code = x.Customer_code,
                                                     Customer_Name = x.Customer_Name,
                                                     DC_LID = x.DC_ID,
                                                     DC_LCode = x.DC_Code,
                                                     Term_of_Payment = x.Term_of_Payment,
                                                     Supplier_Ref = x.Supplier_Ref,
                                                     Buyer_Order_No = x.Buyer_Order_No,
                                                     Order_Date = x.Order_Date,
                                                     Remark = x.Remark,
                                                     is_Invoice_Approved = x.is_Invoice_Approved,
                                                     is_Invoice_approved_by = x.is_Invoice_approved_by,
                                                     is_Invoice_approved_date = x.is_Invoice_approved_date,
                                                     is_Invoice_approved_user_id = x.is_Invoice_approved_user_id,
                                                     CreateBy = x.CreateBy,
                                                     CreatedDate = x.CreatedDate,
                                                     UpdateBy = x.UpdateBy,
                                                     UpdateDate = x.UpdateDate,
                                                     InvoiceLineItems = (from y in CloudDB.Invoice_Line_item
                                                                         where y.Invoice_Id == x.invoice_Id
                                                                         select new InvoiceLineItemEntity
                                                                         {
                                                                             Invoice_Line_Id = y.Invoice_Line_Id,
                                                                             Invoice_Id = y.Invoice_Id,
                                                                             Invoice_Number = y.Invoice_Number,
                                                                             Dispatch_Line_id = y.Dispatch_Line_id,
                                                                             SKU_ID = y.SKU_ID,
                                                                             SKU_Code = y.SKU_Code,
                                                                             SKU_Name = y.SKU_Name,
                                                                             Pack_Type_Id = y.Pack_Type_Id,
                                                                             Pack_Type = y.Pack_Type,
                                                                             SKU_SubType_Id = y.SKU_SubType_Id,
                                                                             SKU_SubType = y.SKU_SubType,
                                                                             Pack_Size = y.Pack_Size,
                                                                             HSN_Code = y.HSN_Code,
                                                                             CGST = y.CGST,
                                                                             SGST = y.SGST,
                                                                             Total_GST = y.Total_GST,
                                                                             Return_Qty = y.Return_Qty,
                                                                             UOM = y.UOM,
                                                                             Grade = y.Grade,
                                                                             Invoice_Qty = y.Invoice_Qty,
                                                                             Rate = y.Rate,
                                                                             Discount = y.Discount,
                                                                             Invoice_Amount = y.Invoice_Amount,
                                                                             is_Deleted = y.is_Deleted,
                                                                             Converted_Unit_Value = y.Converted_Unit_Value,
                                                                             CreatedDate = x.CreatedDate,
                                                                             Dispatch_Qty = y.Dispatch_Qty,
                                                                             CreateBy = x.CreateBy,
                                                                             UpdateBy = x.UpdateBy,
                                                                             UpdateDate = x.UpdateDate,
                                                                         }).ToList(),
                                                 }).ToList();
                STI_PUSH_ENTITY.InvoiceNumClass = (from c in CloudDB.Invoice_NUM_Generation
                                                 where c.DC_Code == dc_Code
                                                 select new InvoiceNumGen
                                                 {
                                                     Invoice_Num_Gen_Id = c.Invoice_Num_Gen_Id,
                                                     DC_Code = c.DC_Code,
                                                     Financial_Year = c.Financial_Year,
                                                     Invoice_Last_Number = c.Invoice_Last_Number
                                                 }).ToList();

                int count = STI_PUSH_ENTITY.invoicerecord.Count;
                int alcount = STI_PUSH_ENTITY.InvoiceNumClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from Invoice and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }

                scope.Complete();
            }
        }

        public void Invoice_Single_Record_Update(string invoice_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Invoice_Creation dispUpdate = CloudDB.Invoice_Creation.Where(x => x.Invoice_Number == invoice_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(invoice_number + " Sync Field Updated in Invoice. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void Invoice_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.invoicerecord)
            {
                using (var scope3 = new TransactionScope())
                {
                    Invoice_Creation dispUpdate = CloudDB.Invoice_Creation.Where(x => x.invoice_Id == disp.invoice_Id).FirstOrDefault();

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
                tw.WriteLine(count + " Records Updated in Invoice. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            STI_PUSH_ENTITY.invoicerecord = null;
        }
    }   
}
