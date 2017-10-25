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
    public class INVOICESetToLive : IINVOICEASetToLive
    {
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();

        string fileName = "INVOICE_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public INVOICESetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void setInvoice()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.invoicerecord != null)
            {
                foreach (var invfromLive in STI_PUSH_ENTITY.invoicerecord)
                {
                    var ExistData = CloudDB.Invoice_Creation.Where(x => x.Invoice_Number == invfromLive.Invoice_Number).FirstOrDefault();
                    if (ExistData == null)
                    {
                        var movedINV = new Invoice_Creation();
                        using (var scope1 = new TransactionScope())
                        {
                            movedINV.Invoice_Number = invfromLive.Invoice_Number;
                            movedINV.Dispatch_id = invfromLive.Dispatch_id;
                            movedINV.Customer_Dispatch_Number = invfromLive.Customer_Dispatch_Number;
                            movedINV.Invoice_Date = invfromLive.Invoice_Date;
                            movedINV.Invoice_Type = invfromLive.Invoice_Type;
                            movedINV.Customer_Id = invfromLive.Customer_Id;
                            movedINV.Customer_code = invfromLive.Customer_code;
                            movedINV.Customer_Name = invfromLive.Customer_Name;
                            movedINV.DC_ID = invfromLive.DC_LID;
                            movedINV.DC_Code = invfromLive.DC_LCode;
                            movedINV.Term_of_Payment = invfromLive.Term_of_Payment;
                            movedINV.Supplier_Ref = invfromLive.Supplier_Ref;
                            movedINV.Buyer_Order_No = invfromLive.Buyer_Order_No;
                            movedINV.Order_Date = invfromLive.Order_Date;
                            movedINV.is_Deleted = false;
                            movedINV.is_Syunc = true;
                            movedINV.SKU_Type_Id = invfromLive.SKU_Type_Id;
                            movedINV.SKU_Type = invfromLive.SKU_Type;
                            movedINV.Remark = invfromLive.Remark;
                            movedINV.is_Invoice_Approved = null;
                            movedINV.is_Invoice_approved_user_id = invfromLive.is_Invoice_approved_user_id;
                            movedINV.is_Invoice_approved_by = invfromLive.is_Invoice_approved_by;
                            movedINV.is_Invoice_approved_date = invfromLive.is_Invoice_approved_date;
                            movedINV.CreatedDate = DateTime.Now;
                            movedINV.CreateBy = invfromLive.CreateBy;
                            movedINV.UpdateBy = invfromLive.UpdateBy;
                            movedINV.UpdateDate = invfromLive.UpdateDate;

                            CloudDB.Invoice_Creation.Add(movedINV);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        int inv_ID = movedINV.invoice_Id;
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(movedINV.Invoice_Number + " Inserted into Invoice. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        foreach (var pSub in invfromLive.InvoiceLineItems)
                        {
                            var model = new Invoice_Line_item();
                            using (var scope3 = new TransactionScope())
                            {
                                model.Invoice_Number = pSub.Invoice_Number;
                                model.Invoice_Id = inv_ID;
                                model.Dispatch_Line_id = pSub.Dispatch_Line_id;
                                model.SKU_ID = pSub.SKU_ID;
                                model.SKU_Code = pSub.SKU_Code;
                                model.SKU_Name = pSub.SKU_Name;
                                model.Pack_Type_Id = pSub.Pack_Type_Id;
                                model.Pack_Type = pSub.Pack_Type;
                                model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                                model.SKU_SubType = pSub.SKU_SubType;
                                model.Pack_Size = pSub.Pack_Size;
                                model.HSN_Code = pSub.HSN_Code;
                                model.CGST = pSub.CGST;
                                model.SGST = pSub.SGST;
                                model.Total_GST = pSub.Total_GST;
                                model.Grade = pSub.Grade;
                                model.UOM = pSub.UOM;
                                model.Invoice_Qty = pSub.Invoice_Qty;
                                model.Rate = pSub.Rate;
                                model.Discount = pSub.Discount;
                                model.Invoice_Amount = pSub.Invoice_Qty * pSub.Rate;
                                model.is_Deleted = false;
                                model.CreatedDate = DateTime.Now;
                                model.CreateBy = pSub.CreateBy;
                                model.Dispatch_Qty = pSub.Dispatch_Qty;
                                model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                                CloudDB.Invoice_Line_item.Add(model);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }
                        }
                        lcount++;
                    }
                    else
                    {
                        INVOICEGetFromLocal csAnother = new INVOICEGetFromLocal();
                        csAnother.Invoice_Single_Record_Update(invfromLive.Invoice_Number);
                    }
                }
                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into Invoice. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }

            if (STI_PUSH_ENTITY.InvoiceNumClass != null)
            {
                int alcount = 0; 
                foreach (var autoNums in STI_PUSH_ENTITY.InvoiceNumClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.Invoice_NUM_Generation.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.Invoice_Last_Number = autoNums.Invoice_Last_Number;

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
                STI_PUSH_ENTITY.InvoiceNumClass = null;
            }
        }
    }
}
