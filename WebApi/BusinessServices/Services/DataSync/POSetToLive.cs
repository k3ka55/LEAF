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
    public class POSetToLive : IPOSetToLive
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();

        string fileName = "PO_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        POGetFromLocal csData = new POGetFromLocal();

        public POSetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        //public void setPO()
        //{
        //    int lcount = 0;

        //    if (STI_PUSH_ENTITY.porecord != null)
        //    {
        //        foreach (var pofromLive in STI_PUSH_ENTITY.porecord)
        //        {
        //            var ExistData = CloudDB.Purchase_Order.Where(x => x.PO_Number == pofromLive.PO_Number).FirstOrDefault();
        //            if (ExistData == null)
        //            { 
        //            var movedPO = new Purchase_Order();
        //            using (var scope1 = new TransactionScope())
        //            {
        //                movedPO.PO_Number = pofromLive.PO_Number;
        //                movedPO.DC_Code = pofromLive.DC_Code;
        //                movedPO.PO_RLS_date = pofromLive.PO_RLS_date;
        //                movedPO.Material_Source = pofromLive.Material_Source;
        //                movedPO.Material_Source_id = pofromLive.Material_Source_id;
        //                movedPO.Supplier_Id = pofromLive.Supplier_Id;
        //                movedPO.Supplier_Code = pofromLive.Supplier_Code;
        //                movedPO.Supplier_name = pofromLive.Supplier_name;
        //                movedPO.PO_Requisitioned_by = pofromLive.PO_Requisitioned_by;
        //                movedPO.PO_Type = pofromLive.PO_Type;
        //                movedPO.Payment_cycle = pofromLive.Payment_cycle;
        //                movedPO.Payment_type = pofromLive.Payment_type;
        //                movedPO.SKU_Type_Id = pofromLive.SKU_Type_Id;
        //                movedPO.SKU_Type = pofromLive.SKU_Type;
        //                movedPO.Delivery_Date = pofromLive.Delivery_Date;
        //                movedPO.PO_raise_by = pofromLive.PO_raise_by;
        //                movedPO.PO_Approve_by = pofromLive.PO_Approve_by;
        //                movedPO.PO_Approval_Flag = pofromLive.PO_Approval_Flag;
        //                movedPO.PO_Approved_date = pofromLive.PO_Approved_date;
        //                movedPO.Status = pofromLive.Status;
        //                movedPO.is_Deleted = pofromLive.is_Deleted;
        //                movedPO.Reason = pofromLive.Reason;
        //                movedPO.CreatedDate = pofromLive.CreatedDate;
        //                movedPO.CreatedBy = pofromLive.CreatedBy;
        //                movedPO.UpdatedBy = pofromLive.UpdatedBy;
        //                movedPO.UpdatedDate = pofromLive.UpdatedDate;

        //                CloudDB.Purchase_Order.Add(movedPO);
        //                CloudDB.SaveChanges();

        //                scope1.Complete();
        //            }

        //            int po_ID = movedPO.Po_id;

                    
        //                using (var tw = new StreamWriter(Path, true))
        //                {
        //                    tw.WriteLine(movedPO.PO_Number + " Inserted into Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
        //                    tw.Close();
        //                }
                    
        //            foreach (var lieitem in pofromLive.PurchaseDetails)
        //            {
        //                var movedLineItem = new Purchase_Order_Line_item();
        //                using (var scope3 = new TransactionScope())
        //                {
        //                    movedLineItem.Po_id = po_ID;
        //                    movedLineItem.PO_Number = lieitem.PO_Number;
        //                    movedLineItem.SKU_ID = lieitem.SKU_ID;
        //                    movedLineItem.SKU_Code = lieitem.SKU_Code;
        //                    movedLineItem.SKU_Name = lieitem.SKU_Name;
        //                    movedLineItem.SKU_SubType_Id = lieitem.SKU_SubType_Id;
        //                    movedLineItem.SKU_SubType = lieitem.SKU_SubType;
        //                    movedLineItem.UOM = lieitem.UOM;
        //                    movedLineItem.Qty = lieitem.Qty;
        //                    movedLineItem.A_Grade_Price = lieitem.A_Grade_Price;
        //                    movedLineItem.A_Grade_Qty = lieitem.A_Grade_Qty;
        //                    movedLineItem.B_Grade_Price = lieitem.B_Grade_Price;
        //                    movedLineItem.B_Grade_Qty = lieitem.B_Grade_Qty;
        //                    movedLineItem.Total_Qty = lieitem.Total_Qty;
        //                    movedLineItem.Remark = lieitem.Remark;
        //                    movedLineItem.Status = lieitem.Status;
        //                    movedLineItem.CreatedBy = lieitem.CreatedBy;
        //                    movedLineItem.CreatedDate = lieitem.CreatedDate;
        //                    movedLineItem.UpdatedBy = lieitem.UpdatedBy;
        //                    movedLineItem.UpdatedDate = lieitem.UpdatedDate;

        //                    CloudDB.Purchase_Order_Line_item.Add(movedLineItem);
        //                    CloudDB.SaveChanges();

        //                    scope3.Complete();
        //                }
        //            }
        //            lcount++;
        //         }
        //            else
        //            {
        //                POGetFromLocal poAnother = new POGetFromLocal();
        //                poAnother.PO_Single_Record_Update(pofromLive.PO_Number);
        //            }
        //        }

        //        int count = lcount;
                
        //            using (var tw = new StreamWriter(Path, true))
        //            {
        //                tw.WriteLine(count + " Records Inserted into Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
        //                tw.Close();
        //            }
                
        //    }

        //    if(STI_PUSH_ENTITY.PONumGenClass != null)
        //    {
        //        int alcount = 0; 
        //        foreach(var autoNums in STI_PUSH_ENTITY.PONumGenClass)
        //        {
        //            using (var scope3 = new TransactionScope())
        //            {
        //                var list = CloudDB.PO_NUM_Generation.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

        //                if (list != null)
        //                {
        //                    list.Financial_Year = autoNums.Financial_Year;
        //                    list.PO_Last_Number = autoNums.PO_Last_Number;

        //                    CloudDB.Entry(list).State = EntityState.Modified;
        //                    CloudDB.SaveChanges();
        //                }

        //                scope3.Complete();
        //            }
        //            alcount++;
        //        }
        //        int count = alcount;
                
        //            using (var tw = new StreamWriter(Path, true))
        //            {
        //                tw.WriteLine(count + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
        //                tw.Close();
        //            }
              
        //        STI_PUSH_ENTITY.PONumGenClass = null;
        //    }
        //}

        public void setPO()
        {
            int lcount = 0;

            if (STI_PUSH_ENTITY.porecord != null)
            {
                foreach (var pofromLive in STI_PUSH_ENTITY.porecord)
                {
                    var ExistData = CloudDB.Purchase_Order.Where(x => x.PO_Number == pofromLive.PO_Number).FirstOrDefault();
                    if (ExistData == null)
                    {
                        var movedPO = new Purchase_Order();
                        using (var scope1 = new TransactionScope())
                        {
                            movedPO.PO_Number = pofromLive.PO_Number;
                            movedPO.DC_Code = pofromLive.DC_Code;
                            movedPO.PO_RLS_date = pofromLive.PO_RLS_date;
                            movedPO.Material_Source = pofromLive.Material_Source;
                            movedPO.Material_Source_id = pofromLive.Material_Source_id;
                            movedPO.Supplier_Id = pofromLive.Supplier_Id;
                            movedPO.Supplier_Code = pofromLive.Supplier_Code;
                            movedPO.Supplier_name = pofromLive.Supplier_name;
                            movedPO.PO_Requisitioned_by = pofromLive.PO_Requisitioned_by;
                            movedPO.PO_Type = pofromLive.PO_Type;
                            movedPO.Payment_cycle = pofromLive.Payment_cycle;
                            movedPO.Payment_type = pofromLive.Payment_type;
                            movedPO.SKU_Type_Id = pofromLive.SKU_Type_Id;
                            movedPO.SKU_Type = pofromLive.SKU_Type;
                            movedPO.Delivery_Date = pofromLive.Delivery_Date;
                            movedPO.PO_raise_by = pofromLive.PO_raise_by;
                            movedPO.PO_Approve_by = pofromLive.PO_Approve_by;
                            movedPO.PO_Approval_Flag = pofromLive.PO_Approval_Flag;
                            movedPO.PO_Approved_date = pofromLive.PO_Approved_date;
                            movedPO.Status = pofromLive.Status;
                            movedPO.is_Deleted = pofromLive.is_Deleted;
                            movedPO.Reason = pofromLive.Reason;
                            movedPO.CreatedDate = pofromLive.CreatedDate;
                            movedPO.CreatedBy = pofromLive.CreatedBy;
                            movedPO.UpdatedBy = pofromLive.UpdatedBy;
                            movedPO.UpdatedDate = pofromLive.UpdatedDate;


                            CloudDB.Purchase_Order.Add(movedPO);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        int po_ID = movedPO.Po_id;


                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(movedPO.PO_Number + " Inserted into Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        foreach (var lieitem in pofromLive.PurchaseDetails)
                        {
                            var movedLineItem = new Purchase_Order_Line_item();
                            using (var scope3 = new TransactionScope())
                            {
                                movedLineItem.Po_id = po_ID;
                                movedLineItem.PO_Number = lieitem.PO_Number;
                                movedLineItem.SKU_ID = lieitem.SKU_ID;
                                movedLineItem.SKU_Code = lieitem.SKU_Code;
                                movedLineItem.SKU_Name = lieitem.SKU_Name;
                                movedLineItem.SKU_SubType_Id = lieitem.SKU_SubType_Id;
                                movedLineItem.SKU_SubType = lieitem.SKU_SubType;
                                movedLineItem.UOM = lieitem.UOM;
                                movedLineItem.Qty = lieitem.Qty;
                                movedLineItem.A_Grade_Price = lieitem.A_Grade_Price;
                                movedLineItem.A_Grade_Qty = lieitem.A_Grade_Qty;
                                movedLineItem.B_Grade_Price = lieitem.B_Grade_Price;
                                movedLineItem.B_Grade_Qty = lieitem.B_Grade_Qty;
                                movedLineItem.Total_Qty = lieitem.Total_Qty;
                                movedLineItem.Remark = lieitem.Remark;
                                movedLineItem.Status = lieitem.Status;
                                movedLineItem.CreatedBy = lieitem.CreatedBy;
                                movedLineItem.CreatedDate = lieitem.CreatedDate;
                                movedLineItem.UpdatedBy = lieitem.UpdatedBy;
                                movedLineItem.UpdatedDate = lieitem.UpdatedDate;


                                CloudDB.Purchase_Order_Line_item.Add(movedLineItem);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }


                            csData.poMasterIDUpdate(movedLineItem.PO_Number, movedLineItem.SKU_ID, movedLineItem.SKU_SubType_Id, movedLineItem.UOM, movedLineItem.PO_Line_Id);

                        }
                        lcount++;

                    }
                    else
                    {
                        var movedLineItem = new Purchase_Order_Line_item();
                        using (var scope1 = new TransactionScope())
                        {
                            ExistData.PO_Number = pofromLive.PO_Number;
                            ExistData.DC_Code = pofromLive.DC_Code;
                            ExistData.PO_RLS_date = pofromLive.PO_RLS_date;
                            ExistData.Material_Source = pofromLive.Material_Source;
                            ExistData.Material_Source_id = pofromLive.Material_Source_id;
                            ExistData.Supplier_Id = pofromLive.Supplier_Id;
                            ExistData.Supplier_Code = pofromLive.Supplier_Code;
                            ExistData.Supplier_name = pofromLive.Supplier_name;
                            ExistData.PO_Requisitioned_by = pofromLive.PO_Requisitioned_by;
                            ExistData.PO_Type = pofromLive.PO_Type;
                            ExistData.Payment_cycle = pofromLive.Payment_cycle;
                            ExistData.Payment_type = pofromLive.Payment_type;
                            ExistData.SKU_Type_Id = pofromLive.SKU_Type_Id;
                            ExistData.SKU_Type = pofromLive.SKU_Type;
                            ExistData.Delivery_Date = pofromLive.Delivery_Date;
                            ExistData.PO_raise_by = pofromLive.PO_raise_by;
                            ExistData.PO_Approve_by = pofromLive.PO_Approve_by;
                            ExistData.PO_Approval_Flag = pofromLive.PO_Approval_Flag;
                            ExistData.PO_Approved_date = pofromLive.PO_Approved_date;
                            ExistData.Status = pofromLive.Status;
                            ExistData.is_Deleted = pofromLive.is_Deleted;
                            ExistData.Reason = pofromLive.Reason;
                            ExistData.CreatedDate = pofromLive.CreatedDate;
                            ExistData.CreatedBy = pofromLive.CreatedBy;
                            ExistData.UpdatedBy = pofromLive.UpdatedBy;
                            ExistData.UpdatedDate = pofromLive.UpdatedDate;

                            CloudDB.Entry(ExistData).State = EntityState.Modified;
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }


                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(ExistData.PO_Number + " Updated into Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }



                        int po_ID = ExistData.Po_id;




                        foreach (var lineitem in pofromLive.PurchaseDetails)
                        {

                            var item = CloudDB.Purchase_Order_Line_item.Where(x => x.PO_Number == lineitem.PO_Number && x.SKU_ID == lineitem.SKU_ID && x.UOM == lineitem.UOM).FirstOrDefault();
                            if (item != null)
                            {
                                using (var scope3 = new TransactionScope())
                                {

                                    item.Po_id = po_ID;
                                    item.PO_Number = lineitem.PO_Number;
                                    item.SKU_ID = lineitem.SKU_ID;
                                    item.SKU_Code = lineitem.SKU_Code;
                                    item.SKU_Name = lineitem.SKU_Name;
                                    item.SKU_SubType_Id = lineitem.SKU_SubType_Id;
                                    item.SKU_SubType = lineitem.SKU_SubType;
                                    item.UOM = lineitem.UOM;
                                    item.Qty = lineitem.Qty;
                                    item.A_Grade_Price = lineitem.A_Grade_Price;
                                    item.A_Grade_Qty = lineitem.A_Grade_Qty;
                                    item.B_Grade_Price = lineitem.B_Grade_Price;
                                    item.B_Grade_Qty = lineitem.B_Grade_Qty;
                                    item.Total_Qty = lineitem.Total_Qty;
                                    item.Remark = lineitem.Remark;
                                    item.Status = lineitem.Status;
                                    item.CreatedBy = lineitem.CreatedBy;
                                    item.CreatedDate = lineitem.CreatedDate;
                                    item.UpdatedBy = lineitem.UpdatedBy;
                                    item.UpdatedDate = lineitem.UpdatedDate;


                                    CloudDB.Entry(item).State = EntityState.Modified;
                                    CloudDB.SaveChanges();

                                    scope3.Complete();
                                }
                            }

                            else
                            {
                                using (var scope3 = new TransactionScope())
                                {
                                    movedLineItem.Po_id = po_ID;
                                    movedLineItem.PO_Number = lineitem.PO_Number;
                                    movedLineItem.SKU_ID = lineitem.SKU_ID;
                                    movedLineItem.SKU_Code = lineitem.SKU_Code;
                                    movedLineItem.SKU_Name = lineitem.SKU_Name;
                                    movedLineItem.SKU_SubType_Id = lineitem.SKU_SubType_Id;
                                    movedLineItem.SKU_SubType = lineitem.SKU_SubType;
                                    movedLineItem.UOM = lineitem.UOM;
                                    movedLineItem.Qty = lineitem.Qty;
                                    movedLineItem.A_Grade_Price = lineitem.A_Grade_Price;
                                    movedLineItem.A_Grade_Qty = lineitem.A_Grade_Qty;
                                    movedLineItem.B_Grade_Price = lineitem.B_Grade_Price;
                                    movedLineItem.B_Grade_Qty = lineitem.B_Grade_Qty;
                                    movedLineItem.Total_Qty = lineitem.Total_Qty;
                                    movedLineItem.Remark = lineitem.Remark;
                                    movedLineItem.Status = lineitem.Status;
                                    movedLineItem.CreatedBy = lineitem.CreatedBy;
                                    movedLineItem.CreatedDate = lineitem.CreatedDate;
                                    movedLineItem.UpdatedBy = lineitem.UpdatedBy;
                                    movedLineItem.UpdatedDate = lineitem.UpdatedDate;

                                    CloudDB.Purchase_Order_Line_item.Add(movedLineItem);
                                    CloudDB.SaveChanges();

                                    scope3.Complete();
                                }

                                csData.poMasterIDUpdate(movedLineItem.PO_Number, movedLineItem.SKU_ID, movedLineItem.SKU_SubType_Id, movedLineItem.UOM, movedLineItem.PO_Line_Id);
                            }


                        }

                    }


                }


            }



            if (STI_PUSH_ENTITY.PONumGenClass != null)
            {
                int alcount = 0;
                foreach (var autoNums in STI_PUSH_ENTITY.PONumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.PO_NUM_Generation.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.PO_Last_Number = autoNums.PO_Last_Number;

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

                STI_PUSH_ENTITY.PONumGenClass = null;
            }

        }
    }
}
