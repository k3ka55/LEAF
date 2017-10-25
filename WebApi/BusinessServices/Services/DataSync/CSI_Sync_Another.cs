using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel;
//using DataModelLocal;
//using DataModel;
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
    public class CSI_Sync_Another : ICSI_Sync_Another
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        //LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
         string fileName = "CSI_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public CSI_Sync_Another()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getCSI()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.csiRecords = (from x in CloudDB.Customer_Sale_Indent
                                              where x.is_Syunc == false && x.is_Deleted == false && x.CSI_Approved_Flag == true
                                              select new SaleIndentEntity
                                              {
                                                  CSI_id = x.CSI_id,
                                                  CSI_Number = x.CSI_Number,
                                                  DC_Code = x.DC_Code,
                                                  CSI_Raised_date = x.CSI_Raised_date,
                                                  CSI_Timestamp = x.CSI_Timestamp,
                                                  SKU_Type_Id = x.SKU_Type_Id,
                                                  SKU_Type = x.SKU_Type,
                                                  Customer_Id = x.Customer_Id,
                                                  Customer_Code = x.Customer_Code,
                                                  Customer_Name = x.Customer_Name,
                                                  Delivery_Location_ID = x.Delivery_Location_ID,
                                                  Delivery_Location_Code = x.Delivery_Location_Code,
                                                  Delivery_cycle = x.Delivery_cycle,
                                                  Delivery_Expected_Date = x.Delivery_Expected_Date,
                                                  Delievery_Type = x.Delievery_Type,
                                                  Delivery_Date = x.Delivery_Date,
                                                  CSI_raised_by = x.CSI_raised_by,
                                                  CSI_Approved_by = x.CSI_Approved_by,
                                                  CSI_Approved_Flag = x.CSI_Approved_Flag,
                                                  CSI_Approved_date = x.CSI_Approved_date,
                                                  Status = x.Status,
                                                  Reason = x.Reason,
                                                  is_Deleted = x.is_Deleted,
                                                  CSI_Create_by = x.CSI_Create_by,
                                                  CSI_type = x.CSI_type,
                                                  CreateBy = x.CreateBy,
                                                  CreatedDate = x.CreatedDate,
                                                  UpdateBy = x.UpdateBy,
                                                  UpdateDate = x.UpdateDate,
                                                  Indent_Id = x.Indent_Id,
                                                  Indent_Name = x.Indent_Name,
                                                  Expected_Delivering_Sales_Person_Id = x.Expected_Delivering_Sales_Person_Id,
                                                  Expected_Delivering_Sales_Person_Name = x.Expected_Delivering_Sales_Person_Name,
                                                  Expected_Route_Alias_Name = x.Expected_Route_Alias_Name,
                                                  Expected_Route_Code = x.Expected_Route_Code,
                                                  Expected_Route_Id = x.Expected_Route_Id,
                                                  Rate_Template_Id = x.Rate_Template_Id,
                                                  Rate_Template_Name = x.Rate_Template_Name,
                                                  User_Location_Id = x.User_Location_Id,
                                                  User_Location_Code = x.User_Location_Code,
                                                  User_Location = x.User_Location,
                                                  User_Type = x.User_Type,
                                                  Rate_Template_Code = x.Rate_Template_Code,
                                                  Indent_Code = x.Indent_Code,
                                                  LineItems = (from y in CloudDB.CSI_Line_item
                                                               where x.CSI_id == y.CSI_id
                                                               select new CSI_LineItems_Entity
                                                               {
                                                                   CSI_Number = y.CSI_Number,
                                                                   SKU_ID = y.SKU_ID,
                                                                   SKU_Code = y.SKU_Code,
                                                                   SKU_Name = y.SKU_Name,
                                                                   SKU_SubType_Id = y.SKU_SubType_Id,
                                                                   SKU_SubType = y.SKU_SubType,
                                                                   Pack_Type_Id = y.Pack_Type_Id,
                                                                   HSN_Code = y.HSN_Code,
                                                                     CGST = y.CGST,
                                                                     SGST = y.SGST,
                                                                     Total_GST = y.Total_GST,
                                                                   Pack_Type = y.Pack_Type,
                                                                   Pack_Size = y.Pack_Size,
                                                                   Pack_Weight_Type_Id = y.Pack_Weight_Type_Id,
                                                                   Pack_Weight_Type = y.Pack_Weight_Type,
                                                                   UOM = y.UOM,
                                                                   Grade = y.Grade,
                                                                   Indent_Qty = y.Indent_Qty,
                                                                   Remark = y.Remark,
                                                                   CreatedBy = y.CreatedBy,
                                                                   CreatedDate = y.CreatedDate,
                                                                   UpdatedBy = y.UpdatedBy,
                                                                   UpdatedDate = y.UpdatedDate,
                                                                   Status = y.Status,
                                                                   price = y.price
                                                               }).ToList(),
                                              }).ToList();
                int count = STI_PUSH_ENTITY.csiRecords.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from CSI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }
        }


        public void CSISet()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.csiRecords != null)
            {
                foreach (var list in STI_PUSH_ENTITY.csiRecords)
                {
                    var ExistData = CloudDB.Customer_Sale_Indent.Where(x => x.CSI_Number == list.CSI_Number).FirstOrDefault();
                    if(ExistData == null)
                    {
                    
                    var moveCSI = new Customer_Sale_Indent();

                    using (var scope1 = new TransactionScope())
                    {
                        moveCSI.CSI_Number = list.CSI_Number;
                        moveCSI.DC_Code = list.DC_Code;
                        moveCSI.CSI_Raised_date = list.CSI_Raised_date;
                        moveCSI.CSI_Timestamp = list.CSI_Timestamp;
                        moveCSI.SKU_Type_Id = list.SKU_Type_Id;
                        moveCSI.SKU_Type = list.SKU_Type;
                        moveCSI.Customer_Id = list.Customer_Id;
                        moveCSI.Customer_Code = list.Customer_Code;
                        moveCSI.Customer_Name = list.Customer_Name;
                        moveCSI.Delivery_Location_ID = list.Delivery_Location_ID;
                        moveCSI.Delivery_Location_Code = list.Delivery_Location_Code;
                        moveCSI.Delivery_cycle = list.Delivery_cycle;
                        moveCSI.Delivery_Expected_Date = list.Delivery_Expected_Date;
                        moveCSI.Delievery_Type = list.Delievery_Type;
                        moveCSI.Delivery_Date = list.Delivery_Date;
                        moveCSI.CSI_raised_by = list.CSI_raised_by;
                        moveCSI.CSI_Approved_by = list.CSI_Approved_by;
                        moveCSI.CSI_Approved_Flag = list.CSI_Approved_Flag;
                        moveCSI.CSI_Approved_date = list.CSI_Approved_date;
                        moveCSI.Status = list.Status;
                        moveCSI.Expected_Delivering_Sales_Person_Id = list.Expected_Delivering_Sales_Person_Id;
                        moveCSI.Expected_Delivering_Sales_Person_Name = list.Expected_Delivering_Sales_Person_Name;
                        moveCSI.Expected_Route_Alias_Name = list.Expected_Route_Alias_Name;
                        moveCSI.Expected_Route_Code = list.Expected_Route_Code;
                        moveCSI.Expected_Route_Id = list.Expected_Route_Id;
                        moveCSI.Reason = list.Reason;
                        moveCSI.is_Deleted = list.is_Deleted;
                        moveCSI.CSI_Create_by = list.CSI_Create_by;
                        moveCSI.CSI_type = list.CSI_type;
                        moveCSI.CreateBy = list.CreateBy;
                        moveCSI.CreatedDate = list.CreatedDate;
                        moveCSI.UpdateBy = list.UpdateBy;
                        moveCSI.UpdateDate = list.UpdateDate;
                        moveCSI.Indent_Id = list.Indent_Id;
                        moveCSI.Indent_Name = list.Indent_Name;
                        moveCSI.Rate_Template_Id = list.Rate_Template_Id;
                        moveCSI.Rate_Template_Name = list.Rate_Template_Name;
                        moveCSI.User_Location_Id = list.User_Location_Id;
                        moveCSI.User_Location_Code = list.User_Location_Code;
                        moveCSI.User_Location = list.User_Location;
                        moveCSI.User_Type = list.User_Type;
                        moveCSI.is_Syunc = true;
                        moveCSI.Rate_Template_Code = list.Rate_Template_Code;
                        moveCSI.Indent_Code = list.Indent_Code;

                        CloudDB.Customer_Sale_Indent.Add(moveCSI);
                        CloudDB.SaveChanges();

                        scope1.Complete();
                    }

                    int csiId = moveCSI.CSI_id;
                    using (var tw = new StreamWriter(Path, true))
                    {
                        tw.WriteLine(moveCSI.CSI_Number + " Inserted into CSI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                        tw.Close();
                    }

                    foreach (var lineItem in list.LineItems)
                    {
                        var lineItems = new CSI_Line_item();

                        using (var scope3 = new TransactionScope())
                        {
                            lineItems.CSI_id = csiId;
                            lineItems.CSI_Number = lineItem.CSI_Number;
                            lineItems.SKU_ID = lineItem.SKU_ID.Value;
                            lineItems.SKU_Code = lineItem.SKU_Code;
                            lineItems.SKU_Name = lineItem.SKU_Name;
                            lineItems.SKU_SubType_Id = lineItem.SKU_SubType_Id;
                            lineItems.SKU_SubType = lineItem.SKU_SubType;
                            lineItems.Pack_Type_Id = lineItem.Pack_Type_Id;
                            lineItems.Pack_Type = lineItem.Pack_Type;
                            lineItems.HSN_Code = lineItem.HSN_Code;
                            lineItems.CGST = lineItem.CGST;
                            lineItems.SGST = lineItem.SGST;
                            lineItems.Total_GST = lineItem.Total_GST;
                            lineItems.Pack_Size = lineItem.Pack_Size;
                            lineItems.Pack_Weight_Type_Id = lineItem.Pack_Weight_Type_Id;
                            lineItems.Pack_Weight_Type = lineItem.Pack_Weight_Type;
                            lineItems.UOM = lineItem.UOM;
                            lineItems.Grade = lineItem.Grade;
                            lineItems.Indent_Qty = lineItem.Indent_Qty;
                            lineItems.Remark = lineItem.Remark;
                            lineItems.CreatedBy = lineItem.CreatedBy;
                            lineItems.CreatedDate = lineItem.CreatedDate;
                            lineItems.UpdatedBy = lineItem.UpdatedBy;
                            lineItems.UpdatedDate = lineItem.UpdatedDate;
                            lineItems.Status = lineItem.Status;
                            lineItems.price = lineItem.price;

                            CloudDB.CSI_Line_item.Add(lineItems);
                            CloudDB.SaveChanges();

                            scope3.Complete();
                        }
                    }
                    lcount++;
                    }
                    else
                    {
                        CSI_Syunc csAnother = new CSI_Syunc();
                        csAnother.CSI_Single_Record_Update(list.CSI_Number);
                    }
                }
                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into CSI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
            if (STI_PUSH_ENTITY.CSINumGenClass != null)
            {
                int alcount = 0;
                foreach (var autoNums in STI_PUSH_ENTITY.CSINumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.CSI_NUM_Generation.Where(x => x.CSI_Num_Gen_Id == autoNums.CSI_Num_Gen_Id).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.CSI_Last_Number = autoNums.CSI_Last_Number;

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
                STI_PUSH_ENTITY.CSINumGenClass = null;
            }
        }

        public void CSI_Single_Record_Update(string csi_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Customer_Sale_Indent dispUpdate = CloudDB.Customer_Sale_Indent.Where(x => x.CSI_Number == csi_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(csi_number + " Sync Field Updated in CSI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void CSI_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.csiRecords)
            {
                using (var scope3 = new TransactionScope())
                {
                    Customer_Sale_Indent dispUpdate = CloudDB.Customer_Sale_Indent.Where(x => x.CSI_id == disp.CSI_id).FirstOrDefault();

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
                tw.WriteLine(count + " Records Updated in CSI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            STI_PUSH_ENTITY.csiRecords = null;
        }
    }
}
