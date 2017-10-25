using System;
using System.Collections.Generic;
using System.Linq;
using DataModelLocal;
//using DataModel;
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
    public class CI_Sync : ICI_Sync
    {
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();
        string fileName = "CI_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public CI_Sync()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getCI()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.ciRecord = (from x in CloudDB.Customer_Indent
                                            where x.Is_Deleted == false && x.Is_Sync == false
                                            select new CIEntity
                                            {
                                                Indent_ID = x.Indent_ID,
                                                Indent_Name = x.Indent_Name,
                                                DC_Id = x.DC_Id,
                                                DC_Code = x.DC_Code,
                                                Location_Id = x.Location_Id,
                                                Location_Code = x.Location_Code,
                                                Region_Id = x.Region_Id,
                                                Region = x.Region,
                                                Region_Code = x.Region_Code,
                                                Indent_Type = x.Indent_Type,
                                                Customer_Id = x.Customer_Id,
                                                Customer_Code = x.Customer_Code,
                                                Customer_Name = x.Customer_Name,
                                                Customer_Delivery_Address = x.Customer_Delivery_Address,
                                                Dispatch_DC_Code = x.Dispatch_DC_Code,
                                                Delivery_cycle = x.Delivery_cycle,
                                                Delivery_Expected_Date = x.Delivery_Expected_Date,
                                                Delivery_Type = x.Delivery_Type,
                                                SKU_Type_Id = x.SKU_Type_Id,
                                                SKU_Type = x.SKU_Type,
                                                Price_Template_ID = x.Price_Template_ID,
                                                Price_Template_Name = x.Price_Template_Name,
                                                Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                                                CreatedDate = x.CreatedDate,
                                                UpdateDate = x.UpdateDate,
                                                CreateBy = x.CreateBy,
                                                UpdateBy = x.UpdateBy,
                                                Indent_Code = x.Indent_Code,
                                                Is_Deleted = x.Is_Deleted,
                                                Price_Template_Code = x.Price_Template_Code,
                                               
                                                LineItems = (from a in CloudDB.Customer_Indent_Line_item
                                                             where a.Indent_ID == x.Indent_ID
                                                             select new CustomerIndentLineItemEntity
                                                             {
                                                                 CI_Line_Id = a.CI_Line_Id,
                                                                 Indent_ID = a.Indent_ID,
                                                                 SKU_Id = a.SKU_Id,
                                                                 SKU_Name = a.SKU_Name,
                                                                 SKU_SubType_Id = a.SKU_SubType_Id,
                                                                 SKU_SubType = a.SKU_SubType,
                                                                 Pack_Type_Id = a.Pack_Type_Id,
                                                                 HSN_Code = a.HSN_Code,
                                                                 CGST = a.CGST,
                                                                 SGST = a.SGST,
                                                                 Total_GST = a.Total_GST,
                                                                 Pack_Type = a.Pack_Type,
                                                                 Pack_Size = a.Pack_Size,
                                                                 Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
                                                                 Pack_Weight_Type = a.Pack_Weight_Type,
                                                                 UOM = a.UOM,
                                                                 Grade = a.Grade,
                                                                 Price = a.Price,
                                                                 Dispatch_Qty = a.Dispatch_Qty,
                                                                 CreatedDate = a.CreatedDate,
                                                                 UpdateDate = a.UpdateDate,
                                                                 CreateBy = a.CreateBy,
                                                                 UpdateBy = a.UpdateBy,
                                                                 Indent_Code = a.Indent_Code,
                                                             }).ToList(),

                                            }).ToList();

                STI_PUSH_ENTITY.CINumGenClass = (from c in CloudDB.Customer_Indent_Num_Gen
                                                  select new CINumGen
                                                  {
                                                      Customer_Indent_Num_Gen_Id = c.Customer_Indent_Num_Gen_Id,
                                                      Financial_Year = c.Financial_Year,
                                                      DC_Code = c.DC_Code,
                                                      Customer_Indent_Last_Number = c.Customer_Indent_Last_Number
                                                  }).ToList();

                int count = STI_PUSH_ENTITY.ciRecord.Count;
                int alcount = STI_PUSH_ENTITY.CINumGenClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from CI and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                

                scope.Complete();
            }
        }
        public void CISet()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.ciRecord != null)
            {
                foreach (var list in STI_PUSH_ENTITY.ciRecord)
                {
                    var moveCI1 = CloudDB.Customer_Indent.Where(x => x.Indent_Code == list.Indent_Code).FirstOrDefault();

                    if (moveCI1 != null)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            moveCI1.Indent_Name = list.Indent_Name;
                            moveCI1.DC_Id = list.DC_Id;
                            moveCI1.DC_Code = list.DC_Code;
                            moveCI1.Location_Code = list.Location_Code;
                            moveCI1.Location_Id = list.Location_Id;
                            moveCI1.Region_Id = list.Region_Id;
                            moveCI1.Region = list.Region;
                            moveCI1.Region_Code = list.Region_Code;
                            moveCI1.Indent_Type = list.Indent_Type;
                            moveCI1.Customer_Code = list.Customer_Code;
                            moveCI1.Customer_Id = list.Customer_Id;
                            moveCI1.Customer_Name = list.Customer_Name;
                            moveCI1.Customer_Delivery_Address = list.Customer_Delivery_Address;
                            moveCI1.Dispatch_DC_Code = list.Dispatch_DC_Code;
                            moveCI1.Delivery_cycle = list.Delivery_cycle;
                            moveCI1.Delivery_Expected_Date = list.Delivery_Expected_Date;
                            moveCI1.Delivery_Type = list.Delivery_Type;
                            //    DC_Location = ciEntity.DC_Location,
                            moveCI1.SKU_Type_Id = list.SKU_Type_Id;
                            moveCI1.SKU_Type = list.SKU_Type;
                            moveCI1.Price_Template_ID = list.Price_Template_ID;
                            moveCI1.Price_Template_Name = list.Price_Template_Name;
                            moveCI1.Price_Template_Valitity_upto = list.Price_Template_Valitity_upto;
                            moveCI1.Is_Deleted = list.Is_Deleted;
                            moveCI1.Is_Sync = true;
                            moveCI1.CreateBy = list.CreateBy;
                            moveCI1.CreatedDate = list.CreatedDate;
                            moveCI1.UpdateBy = list.UpdateBy;
                            moveCI1.UpdateDate = list.UpdateDate;
                            moveCI1.Indent_Code = list.Indent_Code;
                            moveCI1.Price_Template_Code = list.Price_Template_Code;

                            CloudDB.Entry(moveCI1).State = EntityState.Modified;
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }
                        int csiId = moveCI1.Indent_ID;

                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(moveCI1.Indent_Code + " Updated into CI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        var model = CloudDB.Customer_Indent_Line_item.Where(x => x.Indent_ID == csiId).ToList();
                        foreach (var ditem in model)
                        {
                            using (var scope3 = new TransactionScope())
                            {
                                var item = CloudDB.Customer_Indent_Line_item.Where(x => x.CI_Line_Id == ditem.CI_Line_Id).FirstOrDefault();

                                CloudDB.Customer_Indent_Line_item.Remove(item);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }
                        }

                        foreach (var lineItem in list.LineItems)
                        {
                            
                                var lineItems = new Customer_Indent_Line_item();

                                using (var scope3 = new TransactionScope())
                                {
                                    lineItems.Indent_ID = csiId;
                                    lineItems.SKU_Id = lineItem.SKU_Id;
                                    lineItems.SKU_Name = lineItem.SKU_Name;
                                    lineItems.SKU_SubType_Id = lineItem.SKU_SubType_Id;
                                    lineItems.SKU_SubType = lineItem.SKU_SubType;
                                    lineItems.Pack_Type_Id = lineItem.Pack_Type_Id;
                                    lineItems.HSN_Code = lineItem.HSN_Code;
                                    lineItems.CGST = lineItem.CGST;
                                    lineItems.SGST = lineItem.SGST;
                                    lineItems.Total_GST = lineItem.Total_GST;
                                    lineItems.Pack_Type = lineItem.Pack_Type;
                                    lineItems.UOM = lineItem.UOM;
                                    lineItems.Pack_Size = lineItem.Pack_Size;
                                    lineItems.Pack_Weight_Type_Id = lineItem.Pack_Weight_Type_Id;
                                    lineItems.Pack_Weight_Type = lineItem.Pack_Weight_Type;
                                    lineItems.Grade = lineItem.Grade;
                                    lineItems.Price = lineItem.Price;
                                    lineItems.Dispatch_Qty = lineItem.Dispatch_Qty;
                                    lineItems.CreatedDate = lineItem.CreatedDate;
                                    lineItems.CreateBy = lineItem.CreateBy;
                                    lineItems.UpdateDate = lineItem.UpdateDate;
                                    lineItems.UpdateBy = lineItem.UpdateBy;
                                    lineItems.Indent_Code = lineItems.Indent_Code;


                                    CloudDB.Customer_Indent_Line_item.Add(lineItems);
                                    CloudDB.SaveChanges();

                                    scope3.Complete();
                                }                           
                            
                        }
                    }
                    else
                    {
                        var moveCI = new Customer_Indent();

                        using (var scope1 = new TransactionScope())
                        {

                            moveCI.Indent_Name = list.Indent_Name;
                            moveCI.DC_Id = list.DC_Id;
                            moveCI.DC_Code = list.DC_Code;
                            moveCI.Location_Code = list.Location_Code;
                            moveCI.Location_Id = list.Location_Id;
                            moveCI.Region_Id = list.Region_Id;
                            moveCI.Region = list.Region;
                            moveCI.Region_Code = list.Region_Code;
                            moveCI.Indent_Type = list.Indent_Type;
                           
                            moveCI.Customer_Code = list.Customer_Code;
                            moveCI.Customer_Id = list.Customer_Id;
                            moveCI.Customer_Name = list.Customer_Name;
                            moveCI.Customer_Delivery_Address = list.Customer_Delivery_Address;
                            moveCI.Dispatch_DC_Code = list.Dispatch_DC_Code;
                            moveCI.Delivery_cycle = list.Delivery_cycle;
                            moveCI.Delivery_Expected_Date = list.Delivery_Expected_Date;
                            moveCI.Delivery_Type = list.Delivery_Type;
                            moveCI.SKU_Type_Id = list.SKU_Type_Id;
                            moveCI.SKU_Type = list.SKU_Type;
                            moveCI.Price_Template_ID = list.Price_Template_ID;
                            moveCI.Price_Template_Name = list.Price_Template_Name;
                            moveCI.Price_Template_Valitity_upto = list.Price_Template_Valitity_upto;
                            moveCI.CreateBy = list.CreateBy;
                            moveCI.CreatedDate = list.CreatedDate;
                            moveCI.UpdateBy = list.UpdateBy;
                            moveCI.UpdateDate = list.UpdateDate;
                            moveCI.Is_Sync = true;
                            moveCI.Is_Deleted = list.Is_Deleted;
                            moveCI.Indent_Code = list.Indent_Code;
                            moveCI.Price_Template_Code = list.Price_Template_Code;

                            CloudDB.Customer_Indent.Add(moveCI);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        int csiId = moveCI.Indent_ID; 
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(moveCI.Indent_Code + " Inserted into CI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        foreach (var lineItem in list.LineItems)
                        {
                            var lineItems = new Customer_Indent_Line_item();

                            using (var scope3 = new TransactionScope())
                            {
                                lineItems.Indent_ID = csiId;
                                lineItems.SKU_Id = lineItem.SKU_Id;
                                lineItems.SKU_Name = lineItem.SKU_Name;
                                lineItems.SKU_SubType_Id = lineItem.SKU_SubType_Id;
                                lineItems.SKU_SubType = lineItem.SKU_SubType;
                                lineItems.Pack_Type_Id = lineItem.Pack_Type_Id;
                                lineItems.Pack_Type = lineItem.Pack_Type;
                                lineItems.UOM = lineItem.UOM;
                                lineItems.HSN_Code = lineItem.HSN_Code;
                                lineItems.CGST = lineItem.CGST;
                                lineItems.SGST = lineItem.SGST;
                                lineItems.Total_GST = lineItem.Total_GST;
                                lineItems.Pack_Size = lineItem.Pack_Size;
                                lineItems.Pack_Weight_Type_Id = lineItem.Pack_Weight_Type_Id;
                                lineItems.Pack_Weight_Type = lineItem.Pack_Weight_Type;
                                lineItems.Grade = lineItem.Grade;
                                lineItems.Price = lineItem.Price;
                                lineItems.Dispatch_Qty = lineItem.Dispatch_Qty;
                                lineItems.CreatedDate = lineItem.CreatedDate;
                                lineItems.CreateBy = lineItem.CreateBy;
                                lineItems.UpdateDate = lineItem.UpdateDate;
                                lineItems.UpdateBy = lineItem.UpdateBy;
                                lineItems.Indent_Code = lineItems.Indent_Code;

                                CloudDB.Customer_Indent_Line_item.Add(lineItems);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }
                        }
                    }
                    lcount++;
                }
                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted/Updated into CI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }


        public void CI_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.ciRecord)
            {
                using (var scope3 = new TransactionScope())
                {
                    Customer_Indent dispUpdate = CloudDB.Customer_Indent.Where(x => x.Indent_ID == disp.Indent_ID).FirstOrDefault();

                    dispUpdate.Is_Sync = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                lcount++;
            }
            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Updated in CI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }
            STI_PUSH_ENTITY.ciRecord = null;
        }
    }
}
