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
    public class RI_Sync : IRI_Sync
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();



        string fileName = "RI_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public RI_Sync()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getRI()
        {
            using (var scope = new TransactionScope())
            {
                var qry = (from x in CloudDB.Rate_Template
                           where x.Is_Deleted == false 
                           select new RateIndentEntity
                           {
                               Template_ID = x.Template_ID,
                               Template_Name = x.Template_Name,
                               DC_Id = x.DC_Id,
                               Is_BNG_Synced=x.Is_BNG_Synced,
                               Is_JDM_Synced=x.Is_JDM_Synced,
                               DC_Code = x.DC_Code,
                               Location_Id = x.Location_Id,
                               Location_Code = x.Location_Code,
                               Region_Id = x.Region_Id,
                               Region = x.Region,
                               Reason = x.Reason,
                             
                               //DC_Location = x.DC_Location,
                               SKU_Type_Id = x.SKU_Type_Id,
                               Category_Id = x.Category_Id,
                               SKU_Type_Code = x.SKU_Type_Code,
                               Region_Code = x.Region_Code,
                               Customer_Category_Code = x.Customer_Category_Code,
                               SKU_Type = x.SKU_Type,
                               Customer_Category = x.Customer_Category,
                               Valitity_upto = x.Valitity_upto,
                               CreatedDate = x.CreatedDate,
                               UpdateDate = x.UpdateDate,
                               CreateBy = x.CreateBy,
                               Template_Code = x.Template_Code,
                               UpdateBy = x.UpdateBy,
                               Is_Deleted = x.Is_Deleted,
                               LineItems = (from a in CloudDB.Rate_Template_Line_item
                                            where a.RT_id == x.Template_ID
                                            select new RateTemplateLineitem
                                            {
                                                RT_Line_Id = a.RT_Line_Id,
                                                RT_id = a.RT_id,
                                                SKU_Id = a.SKU_Id,
                                                SKU_Code = a.SKU_Code,
                                                HSN_Code = a.HSN_Code,
                                                CGST = a.CGST,
                                                SGST = a.SGST,
                                                Total_GST = a.Total_GST,
                                                SKU_Name = a.SKU_Name,
                                                SKU_SubType_Id = a.SKU_SubType_Id,
                                                SKU_SubType = a.SKU_SubType,
                                                Pack_Type_Id = a.Pack_Type_Id,
                                                Pack_Type = a.Pack_Type,
                                                Pack_Size = a.Pack_Size,
                                                Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
                                                Pack_Weight_Type = a.Pack_Weight_Type,
                                                UOM = a.UOM,
                                                Grade = a.Grade,
                                                Selling_price = a.Selling_price,
                                                MRP = a.MRP,
                                                Material_Code = a.Material_Code,
                                                //  Price = a.Price,
                                                CreatedDate = a.CreatedDate,
                                                UpdateDate = a.UpdateDate,
                                                CreateBy = a.CreateBy,
                                                UpdateBy = a.UpdateBy,
                                                Rate_Template_Code = a.Rate_Template_Code,
                                                Material_Auto_Num_Code = a.Material_Auto_Num_Code,
                                            }).ToList(),

                           }).ToList();


                if(dc_Code=="JDM")
                {
                    qry = qry.Where(a => a.Is_JDM_Synced == false).ToList();
                }
                else if (dc_Code == "BNG")
                {
                    qry = qry.Where(a => a.Is_BNG_Synced == false).ToList();
                }
                else
                {
                    qry = null;
                }
                STI_PUSH_ENTITY.riRecords = qry; 

                
                STI_PUSH_ENTITY.RINumGenClass = (from c in CloudDB.Rate_Template_Num_Gen
                                                 where c.DC_Code == dc_Code
                                                 select new RINumGen
                                                 {
                                                     Rate_Template_Num_Gen_Id = c.Rate_Template_Num_Gen_Id,
                                                     DC_Code = c.DC_Code,
                                                     Financial_Year = c.Financial_Year,
                                                     Rate_Template_Last_Number = c.Rate_Template_Last_Number
                                                 }).ToList();


                int count = STI_PUSH_ENTITY.riRecords.Count;
                int alcount = STI_PUSH_ENTITY.RINumGenClass.Count;

                    using (var tw = new StreamWriter(Path, true))
                    {
                        tw.WriteLine(count + " Records Fetched from Rate Template and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                        tw.Close();
                    }

                scope.Complete();
            }
        }


        public void RISet()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.riRecords != null)
            {
                foreach (var list in STI_PUSH_ENTITY.riRecords)
                {
                    var RIDetails = CloudDB.Rate_Template.Where(x => x.Template_Code == list.Template_Code).FirstOrDefault();

                    if (RIDetails != null)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            RIDetails.Template_Name = list.Template_Name;
                            RIDetails.DC_Id = list.DC_Id;
                            RIDetails.DC_Code = list.DC_Code;
                            RIDetails.Location_Id = list.Location_Id;
                            RIDetails.Location_Code = list.Location_Code;
                            RIDetails.Region_Id = list.Region_Id;
                            RIDetails.Region = list.Region;
                            RIDetails.Region_Code = list.Region_Code;
                            RIDetails.Category_Id = list.Category_Id;
                            RIDetails.SKU_Type_Code = list.SKU_Type_Code;
                            RIDetails.Customer_Category_Code = list.Customer_Category_Code;
                            // p.DC_Location = rateEntity.DC_Location;
                            RIDetails.SKU_Type_Id = list.SKU_Type_Id;
                            RIDetails.SKU_Type = list.SKU_Type;
                            RIDetails.Customer_Category = list.Customer_Category;
                            RIDetails.Valitity_upto = list.Valitity_upto;
                            RIDetails.UpdateDate = list.UpdateDate;
                            RIDetails.UpdateBy = list.UpdateBy;
                            RIDetails.CreatedDate = list.CreatedDate;
                            RIDetails.CreateBy = list.CreateBy;
                            if (dc_Code == "JDM")
                            {
                                RIDetails.Is_JDM_Synced = true;
                                RIDetails.Is_BNG_Synced = false;
                            }
                            else if (dc_Code == "BNG")
                            {
                                RIDetails.Is_BNG_Synced = true;
                                RIDetails.Is_JDM_Synced = false;
                            }
                         //   RIDetails.Is_Sync = true;
                            RIDetails.Is_Deleted = list.Is_Deleted;
                            RIDetails.Template_Code = list.Template_Code;

                            CloudDB.Entry(RIDetails).State = EntityState.Modified;
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(RIDetails.Template_Code + " Updated into RI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        int csiId = RIDetails.Template_ID;

                        var model = CloudDB.Rate_Template_Line_item.Where(x => x.RT_id == csiId).ToList();
                        foreach (var ditem in model)
                        {
                            using (var scope3 = new TransactionScope())
                            {
                                var item = CloudDB.Rate_Template_Line_item.Where(x => x.RT_Line_Id == ditem.RT_Line_Id).FirstOrDefault();

                                CloudDB.Rate_Template_Line_item.Remove(item);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }
                        }

                        foreach (var lineItem in list.LineItems)
                        {
                            
                                var model1 = new Rate_Template_Line_item();
                                using (var scope3 = new TransactionScope())
                                {
                                    model1.RT_id = RIDetails.Template_ID;
                                    model1.Material_Code = lineItem.Material_Code;
                                    model1.SKU_Id = lineItem.SKU_Id;
                                    model1.SKU_Code = lineItem.SKU_Code;
                                    model1.SKU_Name = lineItem.SKU_Name;
                                    model1.SKU_SubType_Id = lineItem.SKU_SubType_Id;
                                    model1.SKU_SubType = lineItem.SKU_SubType;
                                    model1.Pack_Type_Id = lineItem.Pack_Type_Id;
                                    model1.Pack_Type = lineItem.Pack_Type;

                                    model1.HSN_Code = lineItem.HSN_Code;
                                    model1.CGST = lineItem.CGST;
                                    model1.SGST = lineItem.SGST;
                                    model1.Total_GST = lineItem.Total_GST;
                                    model1.Pack_Size = lineItem.Pack_Size;
                                    model1.Pack_Weight_Type_Id = lineItem.Pack_Weight_Type_Id;
                                    model1.Pack_Weight_Type = lineItem.Pack_Weight_Type;
                                    model1.UOM = lineItem.UOM;
                                    model1.Grade = lineItem.Grade;
                                    // model.Price = pSub.Price;
                                    model1.Selling_price = lineItem.Selling_price;
                                    model1.MRP = lineItem.MRP;
                                    model1.UpdateDate = lineItem.UpdateDate;
                                    model1.CreatedDate = lineItem.CreatedDate;
                                    model1.CreateBy = lineItem.CreateBy;
                                    model1.UpdateBy = lineItem.UpdateBy;
                                    model1.Material_Auto_Num_Code = lineItem.Material_Auto_Num_Code;
                                    model1.Rate_Template_Code = lineItem.Rate_Template_Code;

                                    CloudDB.Rate_Template_Line_item.Add(model1);
                                    CloudDB.SaveChanges();

                                    scope3.Complete();
                                }
                            

                        }

                    }
                    else
                    {
                        var moveCSI = new Rate_Template();

                        using (var scope1 = new TransactionScope())
                        {

                            moveCSI.Template_Name = list.Template_Name;
                            moveCSI.DC_Id = list.DC_Id;
                            moveCSI.DC_Code = list.DC_Code;
                            moveCSI.Location_Id = list.Location_Id;
                            moveCSI.Location_Code = list.Location_Code;
                            moveCSI.Region_Id = list.Region_Id;
                            moveCSI.Region = list.Region;
                            moveCSI.Region_Code = list.Region_Code;
                            moveCSI.Category_Id = list.Category_Id;
                            moveCSI.SKU_Type_Code = list.SKU_Type_Code;
                            moveCSI.Customer_Category_Code = list.Customer_Category_Code;
                            // p.DC_Location = rateEntity.DC_Location;
                            moveCSI.SKU_Type_Id = list.SKU_Type_Id;
                            moveCSI.SKU_Type = list.SKU_Type;
                            moveCSI.Customer_Category = list.Customer_Category;
                            moveCSI.Valitity_upto = list.Valitity_upto;
                            moveCSI.UpdateDate = list.UpdateDate;
                            moveCSI.UpdateBy = list.UpdateBy;
                            moveCSI.CreatedDate = list.CreatedDate;
                            moveCSI.CreateBy = list.CreateBy;
                            if (dc_Code == "JDM")
                            {
                                moveCSI.Is_JDM_Synced = true;
                                moveCSI.Is_BNG_Synced = false;
                            }
                            else if (dc_Code == "BNG")
                            {
                                moveCSI.Is_BNG_Synced = true;
                                moveCSI.Is_JDM_Synced = false;
                            } 
                          //  moveCSI.Is_Sync = true;
                            moveCSI.Is_Deleted = list.Is_Deleted;
                            moveCSI.Template_Code = list.Template_Code;

                            CloudDB.Rate_Template.Add(moveCSI);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(moveCSI.Template_Code + " Inserted into RI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        int csiId = moveCSI.Template_ID;

                        foreach (var lineItem in list.LineItems)
                        {
                            var model = new Rate_Template_Line_item();

                            using (var scope3 = new TransactionScope())
                            {
                                model.RT_id = csiId;
                                model.Material_Code = lineItem.Material_Code;
                                model.SKU_Id = lineItem.SKU_Id;
                                model.SKU_Code = lineItem.SKU_Code;
                                model.SKU_Name = lineItem.SKU_Name;
                                model.SKU_SubType_Id = lineItem.SKU_SubType_Id;
                                model.SKU_SubType = lineItem.SKU_SubType;
                                model.Pack_Type_Id = lineItem.Pack_Type_Id;
                                model.Pack_Type = lineItem.Pack_Type;
                                model.Pack_Size = lineItem.Pack_Size;
                                model.HSN_Code = lineItem.HSN_Code;
                                model.CGST = lineItem.CGST;
                                model.SGST = lineItem.SGST;
                                model.Total_GST = lineItem.Total_GST;
                                model.Pack_Weight_Type_Id = lineItem.Pack_Weight_Type_Id;
                                model.Pack_Weight_Type = lineItem.Pack_Weight_Type;
                                model.UOM = lineItem.UOM;
                                model.Grade = lineItem.Grade;
                                // model.Price = pSub.Price;
                                model.Selling_price = lineItem.Selling_price;
                                model.MRP = lineItem.MRP;
                                model.UpdateDate = lineItem.UpdateDate;
                                model.CreatedDate = lineItem.CreatedDate;
                                model.CreateBy = lineItem.CreateBy;
                                model.UpdateBy = lineItem.UpdateBy;
                                model.Material_Auto_Num_Code = lineItem.Material_Auto_Num_Code;
                                model.Rate_Template_Code = lineItem.Rate_Template_Code;

                                CloudDB.Rate_Template_Line_item.Add(model);
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
                    tw.WriteLine(count + " Records Inserted/Updated into RI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }

            }

            if (STI_PUSH_ENTITY.RINumGenClass != null)
            {
                int alcount = 0;
                foreach (var autoNums in STI_PUSH_ENTITY.RINumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.Rate_Template_Num_Gen.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.Rate_Template_Last_Number = autoNums.Rate_Template_Last_Number;

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

                STI_PUSH_ENTITY.RINumGenClass = null;
            }
        }


        public void RI_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.riRecords)
            {
                using (var scope3 = new TransactionScope())
                {
                    Rate_Template dispUpdate = CloudDB.Rate_Template.Where(x => x.Template_ID == disp.Template_ID).FirstOrDefault();
                    if (dc_Code == "JDM")
                    {
                        dispUpdate.Is_JDM_Synced = true;
                    }
                    else if (dc_Code == "BNG")
                    {
                        dispUpdate.Is_BNG_Synced = true;
                    }
                   // dispUpdate.Is_Sync = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                lcount++;
            }

            int count = lcount;
           
                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Updated in Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            
            STI_PUSH_ENTITY.riRecords = null;
        }
    }
}