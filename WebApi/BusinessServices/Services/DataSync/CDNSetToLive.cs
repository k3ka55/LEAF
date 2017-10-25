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
using BusinessEntities;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class CDNSetToLive : ICDNSetToLive
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();

        string fileName = "CDN_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public CDNSetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void CDNSet()
        {
            int lcount = 0;

            if (STI_PUSH_ENTITY.stnRecordes != null)
                foreach (var disp in STI_PUSH_ENTITY.stnRecordes)
                {
                    var ExistData = CloudDB.Dispatch_Creation.Where(x => x.Customer_Dispatch_Number == disp.Customer_Dispatch_Number).FirstOrDefault();
                    if (ExistData == null)
                    {
                        var pullDisp = new Dispatch_Creation();
                        using (var scope1 = new TransactionScope())
                        {
                            pullDisp.Dispatched_Location_ID = disp.Dispatched_Location_ID;
                            pullDisp.Dispatch_Location_Code = disp.Dispatch_Location_Code;
                            pullDisp.Customer_Id = disp.Customer_Id;
                            pullDisp.Customer_code = disp.Customer_code;
                            pullDisp.Customer_Name = disp.Customer_Name;
                            pullDisp.STI_Id = disp.STI_Id;
                            pullDisp.STI_Number = disp.STI_Number;
                            pullDisp.CSI_Id = disp.CSI_Id;
                            pullDisp.Sales_Person_Id = disp.Sales_Person_Id;
                            pullDisp.Sales_Person_Name = disp.Sales_Person_Name;
                            pullDisp.Route_Id = disp.Route_Id;
                            pullDisp.Route = disp.Route;
                            pullDisp.Route_Code = disp.Route_Code;
                            pullDisp.Order_Reference = disp.Order_Reference;
                            pullDisp.Vehicle_No = disp.Vehicle_No;
                            pullDisp.CSI_Number = disp.CSI_Number;
                            pullDisp.Indent_Rls_Date = disp.Indent_Rls_Date;
                            pullDisp.Sales_Person_Id = disp.Sales_Person_Id;
                            pullDisp.SKU_Type_Id = disp.SKU_Type_Id;
                            pullDisp.SKU_Type = disp.SKU_Type;
                            pullDisp.Delievery_Type = disp.Delievery_Type;
                            pullDisp.Customer_Dispatch_Number = disp.Customer_Dispatch_Number;
                            pullDisp.Stock_Xfer_Dispatch_Number = disp.Stock_Xfer_Dispatch_Number;
                            pullDisp.Delivery_Date = disp.Delivery_Date;
                            pullDisp.Dispatch_Type = disp.Dispatch_Type;
                            pullDisp.Delivery_done_by = disp.Delivery_done_by;
                            pullDisp.Dispatch_time_stamp = disp.Dispatch_time_stamp;
                            pullDisp.Delivery_Location_ID = disp.Delivery_Location_ID;
                            pullDisp.Delivery_Location_Code = disp.Delivery_Location_Code;
                            pullDisp.Expected_Delivery_date = disp.Expected_Delivery_date;
                            pullDisp.Expected_Delivery_time = disp.Expected_Delivery_time;
                            pullDisp.is_Deleted = disp.is_Deleted;
                            pullDisp.is_Syunc = true;
                            pullDisp.Remark = disp.Remark;
                            pullDisp.Status = disp.Status;
                            pullDisp.CreatedDate = disp.CreatedDate;
                            pullDisp.CreateBy = disp.CreateBy;
                            pullDisp.UpdateBy = disp.UpdateBy;
                            pullDisp.UpdateDate = disp.UpdateDate;
                            pullDisp.Invoice_Flag = disp.Invoice_Flag;
                            pullDisp.Price_Template_ID = disp.Price_Template_ID;
                            pullDisp.Price_Template_Name = disp.Price_Template_Name;
                            pullDisp.Price_Template_Valitity_upto = disp.Price_Template_Valitity_upto;
                            pullDisp.Price_Template_Code = disp.Price_Template_Code;

                            CloudDB.Dispatch_Creation.Add(pullDisp);
                            CloudDB.SaveChanges();
                            scope1.Complete();
                        }

                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(pullDisp.Customer_Dispatch_Number + " Inserted into CDN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        int pulledDipsID = pullDisp.Dispatch_Id;

                        foreach (var displine in disp.Line_Items)
                        {
                            var pullDispLine = new Dispatch_Line_item();
                            using (var scope2 = new TransactionScope())
                            {
                                pullDispLine.Dispatch_Id = pulledDipsID;
                                pullDispLine.SKU_ID = displine.SKU_ID;
                                pullDispLine.SKU_Code = displine.SKU_Code;
                                pullDispLine.SKU_Name = displine.SKU_Name;
                                pullDispLine.Supplier_Id = displine.Supplier_Id;
                                pullDispLine.Supplier_Code = displine.Supplier_Code;
                                pullDispLine.Supplier_Name = displine.Supplier_Name;
                                pullDispLine.SKU_SubType_Id = displine.SKU_SubType_Id;
                                pullDispLine.SKU_SubType = displine.SKU_SubType;
                                pullDispLine.Pack_Type_Id = displine.Pack_Type_Id;
                                pullDispLine.Pack_Type = displine.Pack_Type;
                                pullDispLine.HSN_Code = displine.HSN_Code;
                                pullDispLine.CGST = displine.CGST;
                                pullDispLine.SGST = displine.SGST;
                                pullDispLine.Total_GST = displine.Total_GST;
                                pullDispLine.Pack_Size = displine.Pack_Size;
                                pullDispLine.Pack_Weight_Type_Id = displine.Pack_Weight_Type_Id;
                                pullDispLine.Pack_Weight_Type = displine.Pack_Weight_Type;
                                pullDispLine.UOM = displine.UOM;
                                pullDispLine.Indent_Qty = displine.Indent_Qty;
                                pullDispLine.Dispatch_Qty = displine.Dispatch_Qty;
                                pullDispLine.Grade = displine.Grade;
                                pullDispLine.Return_Qty = displine.Return_Qty;
                                pullDispLine.Accepted_Qty = displine.Accepted_Qty;
                                pullDispLine.Received_Qty = displine.Received_Qty;
                                pullDispLine.Unit_Rate = displine.Unit_Rate;
                                pullDispLine.Dispatch_Value = displine.Dispatch_Value;
                                pullDispLine.No_of_Packed_Item = displine.No_of_Packed_Item;
                                pullDispLine.Dispatch_Pack_Type = displine.Dispatch_Pack_Type;
                                pullDispLine.Remark = displine.Remark;
                                pullDispLine.Tally_Status = displine.Tally_Status;
                                pullDispLine.Status = displine.Status;
                                pullDispLine.is_Deleted = displine.is_Deleted;
                                pullDispLine.CreateBy = displine.CreateBy;
                                pullDispLine.CreatedDate = displine.CreatedDate;
                                pullDispLine.UpdateBy = displine.UpdateBy;
                                pullDispLine.UpdateDate = displine.UpdateDate;
                                pullDispLine.is_Stk_Update = false;

                                CloudDB.Dispatch_Line_item.Add(pullDispLine);
                                CloudDB.SaveChanges();

                                scope2.Complete();
                            }

                        }

                        lcount++;
                    }
                    else
                    {
                        CDNGetFromLocal csAnother = new CDNGetFromLocal();
                        csAnother.CDN_Single_Record_Update(disp.Customer_Dispatch_Number);
                    }
                }

            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Inserted into CDN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            if (STI_PUSH_ENTITY.CDNNumGenClass != null)
            {
                int alcount = 0;
                foreach (var autoNums in STI_PUSH_ENTITY.CDNNumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.Customer_Dispatch_Num_Gen.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.Customer_Dispatch_Last_Number = autoNums.Customer_Dispatch_Last_Number;

                            CloudDB.Entry(list).State = EntityState.Modified;
                            CloudDB.SaveChanges();
                        }

                        scope3.Complete();
                    }
                    alcount++;
                }

                using (var scope7 = new TransactionScope())
                {
                    CloudDB.dispatch_proc();
                    CloudDB.SaveChanges();
                    scope7.Complete();
                }


                int count1 = alcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count1 + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }

                STI_PUSH_ENTITY.CDNNumGenClass = null;
            }


        }

        public void CDNGet()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.stnRecordes = (from x in CloudDB.Dispatch_Creation
                                               where x.Dispatch_Type == "Customer" && x.Dispatch_Location_Code == dc_Code && x.Customer_Dispatch_Number != null && x.is_Syunc == false && x.is_Deleted == false
                                               select new DispatchEntity
                                               {
                                                   Dispatch_Id = x.Dispatch_Id,
                                                   Dispatched_Location_ID = x.Dispatched_Location_ID,
                                                   Dispatch_Location_Code = x.Dispatch_Location_Code,
                                                   Customer_Id = x.Customer_Id,
                                                   Customer_code = x.Customer_code,
                                                   Customer_Name = x.Customer_Name,
                                                   STI_Id = x.STI_Id,
                                                   STI_Number = x.STI_Number,
                                                   CSI_Id = x.CSI_Id,
                                                   CSI_Number = x.CSI_Number,
                                                   Indent_Rls_Date = x.Indent_Rls_Date,
                                                   SKU_Type_Id = x.SKU_Type_Id,
                                                   SKU_Type = x.SKU_Type,
                                                   Delievery_Type = x.Delievery_Type,
                                                   Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                   Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                                                   Delivery_Date = x.Delivery_Date,
                                                   Dispatch_Type = x.Dispatch_Type,
                                                   Delivery_done_by = x.Delivery_done_by,
                                                   Dispatch_time_stamp = x.Dispatch_time_stamp,
                                                   Delivery_Location_ID = x.Delivery_Location_ID,
                                                   Delivery_Location_Code = x.Delivery_Location_Code,
                                                   Expected_Delivery_date = x.Expected_Delivery_date,
                                                   Expected_Delivery_time = x.Expected_Delivery_time,
                                                   is_Deleted = x.is_Deleted,
                                                   is_Syunc = x.is_Syunc,
                                                   Remark = x.Remark,
                                                   Status = x.Status,
                                                   Sales_Person_Id = x.Sales_Person_Id,
                                                   Sales_Person_Name = x.Sales_Person_Name,
                                                   Route_Id = x.Route_Id,
                                                   Route = x.Route,
                                                   Route_Code = x.Route_Code,
                                                   Order_Reference = x.Order_Reference,
                                                   Vehicle_No = x.Vehicle_No,
                                                   CreatedDate = x.CreatedDate,
                                                   CreateBy = x.CreateBy,
                                                   UpdateBy = x.UpdateBy,
                                                   UpdateDate = x.UpdateDate,
                                                   Invoice_Flag = x.Invoice_Flag,
                                                   Price_Template_ID = x.Price_Template_ID,
                                                   Price_Template_Name = x.Price_Template_Name,
                                                   Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                                                   Price_Template_Code = x.Price_Template_Code,
                                                   Line_Items = (from c in CloudDB.Dispatch_Line_item
                                                                 where c.Dispatch_Id == x.Dispatch_Id
                                                                 select new DispatchLineItemsEntity
                                                                 {
                                                                     Dispatch_Id = c.Dispatch_Id,
                                                                     SKU_ID = c.SKU_ID,
                                                                     SKU_Code = c.SKU_Code,
                                                                     SKU_Name = c.SKU_Name,
                                                                     Supplier_Id = c.Supplier_Id,
                                                                     Supplier_Code = c.Supplier_Code,
                                                                     Supplier_Name = c.Supplier_Name,
                                                                     SKU_SubType_Id = c.SKU_SubType_Id,
                                                                     SKU_SubType = c.SKU_SubType,
                                                                     Pack_Type_Id = c.Pack_Type_Id,
                                                                     Pack_Type = c.Pack_Type,
                                                                     Pack_Size = c.Pack_Size,
                                                                     Pack_Weight_Type_Id = c.Pack_Weight_Type_Id,
                                                                     Pack_Weight_Type = c.Pack_Weight_Type,
                                                                     UOM = c.UOM,
                                                                     HSN_Code = c.HSN_Code,
                                                                     CGST = c.CGST,
                                                                     SGST = c.SGST,
                                                                     Total_GST = c.Total_GST,
                                                                     Indent_Qty = c.Indent_Qty,
                                                                     Dispatch_Qty = c.Dispatch_Qty,
                                                                     Grade = c.Grade,
                                                                     Tally_Status=c.Tally_Status,
                                                                     Return_Qty = c.Return_Qty,
                                                                     Accepted_Qty = c.Accepted_Qty,
                                                                     Received_Qty = c.Received_Qty,
                                                                     Unit_Rate = c.Unit_Rate,
                                                                     Dispatch_Value = c.Dispatch_Value,
                                                                     No_of_Packed_Item = c.No_of_Packed_Item,
                                                                     Dispatch_Pack_Type = c.Dispatch_Pack_Type,
                                                                     Remark = c.Remark,
                                                                     Status = c.Status,
                                                                     is_Deleted = c.is_Deleted,
                                                                     CreateBy = c.CreateBy,
                                                                     CreatedDate = c.CreatedDate,
                                                                     UpdateBy = c.UpdateBy,
                                                                     UpdateDate = c.UpdateDate
                                                                 }).ToList()
                                               }).ToList();

                //STI_PUSH_ENTITY.CDNNumGenClass = (from c in CloudDB.Customer_Dispatch_Num_Gen
                //                                  where c.DC_Code == dc_Code
                //                                  select new CDNNumGen
                //                                  {
                //                                      Dispatch_Num_Gen_Id = c.Dispatch_Num_Gen_Id,
                //                                      DC_Code = c.DC_Code,
                //                                      Financial_Year = c.Financial_Year,
                //                                      Customer_Dispatch_Last_Number = c.Customer_Dispatch_Last_Number
                //                                  }).ToList();
                int count = STI_PUSH_ENTITY.stnRecordes.Count;
                //int alcount = STI_PUSH_ENTITY.CDNNumGenClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from CDN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }
        }

        public void CDN_Single_Record_Update(string cdn_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Dispatch_Creation dispUpdate = CloudDB.Dispatch_Creation.Where(x => x.Customer_Dispatch_Number == cdn_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(cdn_number + " Sync Field Updated in CDN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void CDN_Update()
        {
            int lcount = 0;

            foreach (var disp in STI_PUSH_ENTITY.stnRecordes)
            {
                using (var scope3 = new TransactionScope())
                {
                    Dispatch_Creation dispUpdate = CloudDB.Dispatch_Creation.Where(x => x.Dispatch_Id == disp.Dispatch_Id).FirstOrDefault();

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
                tw.WriteLine(count + " Records Updated in CDN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            STI_PUSH_ENTITY.stnRecordes = null;
        }

    }
}
