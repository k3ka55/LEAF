using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
//using DataModel;
using DataModelLocal;
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
    public class STI_Push : ISTISET_STNGET
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();

        string fileName = "STI_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public STI_Push()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void STIGet()
        {

            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.localStis = (from x in CloudDB.Stock_Transfer_Indent
                                             where x.Material_Source == dc_Code && x.is_Deleted == false && x.is_Syunc == false && x.STI_Approval_Flag == true
                                             select new StockTransferIntentEntity
                                             {
                                                 STI_id = x.STI_id,
                                                 STI_Number = x.STI_Number,
                                                 Indent_Raised_by_DC_Id = x.Indent_Raised_by_DC_Id,
                                                 Indent_Raised_by_DC_Code = x.Indent_Raised_by_DC_Code,
                                                 STI_RLS_date = x.STI_RLS_date,
                                                 SKU_Type = x.SKU_Type,
                                                 SKU_Type_Id = x.SKU_Type_Id,
                                                 Material_Source_id = x.Material_Source_id,
                                                 Material_Source = x.Material_Source,
                                                 Intermediate_DC_Code = x.Intermediate_DC_Code,
                                                 Delivery_DC_id = x.Delivery_DC_id,
                                                 Delivery_DC_Code = x.Delivery_DC_Code,
                                                 STI_Type = x.STI_Type,
                                                 STI_Delivery_cycle = x.STI_Delivery_cycle,
                                                 DC_Delivery_Date = x.DC_Delivery_Date,
                                                 STI_raise_by = x.STI_raise_by,
                                                 STI_Approve_by = x.STI_Approve_by,
                                                 STI_Approval_Flag = x.STI_Approval_Flag,
                                                 STI_Approved_date = x.STI_Approved_date,
                                                 Status = x.Status,
                                                 Reason = x.Reason,
                                                 DeleteReason = x.DeleteReason,
                                                 is_Deleted = x.is_Deleted,
                                                 CreatedDate = x.CreatedDate,
                                                 CreatedBy = x.CreatedBy,
                                                 UpdatedDate = x.UpdatedDate,
                                                 UpdatedBy = x.UpdatedBy,
                                                 STIDetails = (from c in CloudDB.STI_Line_item
                                                               where c.STI_id == x.STI_id
                                                               select new SIT_LineItems
                                                               {
                                                                   STI_id = c.STI_id,
                                                                   STI_Number = c.STI_Number,
                                                                   SKU_ID = c.SKU_ID,
                                                                   SKU_Code = c.SKU_Code,
                                                                   SKU_Name = c.SKU_Name,
                                                                   SKU_SubType_Id = c.SKU_SubType_Id,
                                                                   SKU_SubType = c.SKU_SubType,
                                                                   Pack_Type_Id = c.Pack_Type_Id,
                                                                   Pack_Type = c.Pack_Type,
                                                                   Pack_Size = c.Pack_Size,
                                                                   Pack_Weight_Type = c.Pack_Weight_Type,
                                                                   Pack_Weight_Type_Id = c.Pack_Weight_Type_Id,
                                                                   UOM = c.UOM,
                                                                   Qty = c.Qty,
                                                                   Grade = c.Grade,
                                                                   Total_Qty = c.Total_Qty,
                                                                   Remark = c.Remark,
                                                                   Status = c.Status,
                                                                   CreatedBy = c.CreatedBy,
                                                                   CreatedDate = c.CreatedDate,
                                                                   UpdatedBy = c.UpdatedBy,
                                                                   UpdatedDate = c.UpdatedDate
                                                               }).ToList()
                                             }).ToList();


                //STI_PUSH_ENTITY.STINumGenClass = (from c in CloudDB.ST_NUM_Generation
                //                                 where c.DC_Code == dc_Code
                //                                 select new STINumGen
                //                                 {
                //                                     ST_Num_Gen_Id = c.ST_Num_Gen_Id,
                //                                     DC_Code = c.DC_Code,
                //                                     Financial_Year = c.Financial_Year,
                //                                     ST_Last_Number = c.ST_Last_Number
                //                                 }).ToList();

                int count = STI_PUSH_ENTITY.localStis.Count;
                //int alcount = STI_PUSH_ENTITY.PONumGenClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from STI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }

        }

        public void STISet()
        {
            int lcount = 0;

            if (STI_PUSH_ENTITY.localStis != null)
            foreach (var stiFromLocal in STI_PUSH_ENTITY.localStis)
            {
                var ExistData = CloudDB.Stock_Transfer_Indent.Where(x => x.STI_Number == stiFromLocal.STI_Number).FirstOrDefault();
                    if (ExistData == null)
                    {
                var movedSTI = new Stock_Transfer_Indent();
                using (var scope1 = new TransactionScope())
                {
                    movedSTI.STI_Number = stiFromLocal.STI_Number;
                    movedSTI.Indent_Raised_by_DC_Id = stiFromLocal.Indent_Raised_by_DC_Id;
                    movedSTI.Indent_Raised_by_DC_Code = stiFromLocal.Indent_Raised_by_DC_Code;
                    movedSTI.STI_RLS_date = stiFromLocal.STI_RLS_date;
                    movedSTI.SKU_Type = stiFromLocal.SKU_Type;
                    movedSTI.SKU_Type_Id = stiFromLocal.SKU_Type_Id;
                    movedSTI.Material_Source_id = stiFromLocal.Material_Source_id;
                    movedSTI.Material_Source = stiFromLocal.Material_Source;
                    movedSTI.Intermediate_DC_Code = stiFromLocal.Intermediate_DC_Code;
                    movedSTI.Delivery_DC_id = stiFromLocal.Delivery_DC_id;
                    movedSTI.Delivery_DC_Code = stiFromLocal.Delivery_DC_Code;
                    movedSTI.STI_Type = stiFromLocal.STI_Type;
                    movedSTI.STI_Delivery_cycle = stiFromLocal.STI_Delivery_cycle;
                    movedSTI.DC_Delivery_Date = stiFromLocal.DC_Delivery_Date;
                    movedSTI.STI_raise_by = stiFromLocal.STI_raise_by;
                    movedSTI.STI_Approve_by = stiFromLocal.STI_Approve_by;
                    movedSTI.STI_Approval_Flag = stiFromLocal.STI_Approval_Flag;
                    movedSTI.STI_Approved_date = stiFromLocal.STI_Approved_date;
                    movedSTI.Status = stiFromLocal.Status;
                    movedSTI.Reason = stiFromLocal.Reason;
                    movedSTI.DeleteReason = stiFromLocal.DeleteReason;
                    movedSTI.is_Deleted = stiFromLocal.is_Deleted;
                    movedSTI.CreatedDate = stiFromLocal.CreatedDate;
                    movedSTI.CreatedBy = stiFromLocal.CreatedBy;
                    movedSTI.UpdatedDate = stiFromLocal.UpdatedDate;
                    movedSTI.UpdatedBy = stiFromLocal.UpdatedBy;
                    movedSTI.is_Syunc = false;
                    //using (var scopec = new TransactionScope())
                    //{
                    CloudDB.Stock_Transfer_Indent.Add(movedSTI);
                    CloudDB.SaveChanges();
                    //scopec.Complete();
                    //}
                    scope1.Complete();
                }


                int sti_id_forCloud = movedSTI.STI_id;
                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(movedSTI.STI_Number + " Inserted into STI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }

                    foreach (var stilinefromlocal in stiFromLocal.STIDetails)
                    {
                        var movedSTILine = new STI_Line_item();
                        using (var scope3 = new TransactionScope())
                        {
                        movedSTILine.STI_id = sti_id_forCloud;
                        movedSTILine.STI_Number = stilinefromlocal.STI_Number;
                        movedSTILine.SKU_ID = stilinefromlocal.SKU_ID;
                        movedSTILine.SKU_Code = stilinefromlocal.SKU_Code;
                        movedSTILine.SKU_Name = stilinefromlocal.SKU_Name;
                        movedSTILine.SKU_SubType_Id = stilinefromlocal.SKU_SubType_Id;
                        movedSTILine.SKU_SubType = stilinefromlocal.SKU_SubType;
                        movedSTILine.Pack_Type_Id = stilinefromlocal.Pack_Type_Id;
                        movedSTILine.Pack_Type = stilinefromlocal.Pack_Type;
                        movedSTILine.Pack_Size = stilinefromlocal.Pack_Size;
                        movedSTILine.Pack_Weight_Type = stilinefromlocal.Pack_Weight_Type;
                        movedSTILine.Pack_Weight_Type_Id = stilinefromlocal.Pack_Weight_Type_Id;
                        movedSTILine.UOM = stilinefromlocal.UOM;
                        movedSTILine.Qty = stilinefromlocal.Qty;
                        movedSTILine.Grade = stilinefromlocal.Grade;
                        movedSTILine.Total_Qty = stilinefromlocal.Total_Qty;
                        movedSTILine.Remark = stilinefromlocal.Remark;
                        movedSTILine.Status = stilinefromlocal.Status;
                        movedSTILine.CreatedBy = stilinefromlocal.CreatedBy;
                        movedSTILine.CreatedDate = stilinefromlocal.CreatedDate;
                        movedSTILine.UpdatedBy = stilinefromlocal.UpdatedBy;
                        movedSTILine.UpdatedDate = stilinefromlocal.UpdatedDate;
                        
                        CloudDB.STI_Line_item.Add(movedSTILine);
                        CloudDB.SaveChanges();
                        scope3.Complete();
                    }

                    
                }

                    lcount++;
                    }
                    else
                    {
                        DataSyuncServices csAnother = new DataSyuncServices();
                        csAnother.STI_Single_Record_Update(stiFromLocal.STI_Number);
                    }
            }

            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Inserted into STI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            if (STI_PUSH_ENTITY.STINumGenClass != null)
            {
                int alcount = 0; 
                foreach (var autoNums in STI_PUSH_ENTITY.STINumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.ST_NUM_Generation.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.ST_Last_Number = autoNums.ST_Last_Number;

                            CloudDB.Entry(list).State = EntityState.Modified;
                            CloudDB.SaveChanges();
                        }

                        scope3.Complete();
                    }
                    alcount++;
                }
                int scount = alcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(scount + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                STI_PUSH_ENTITY.STINumGenClass = null;
            }
        }


        public void STNGet()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.stnRecordes = (from x in CloudDB.Dispatch_Creation
                                               where x.Dispatch_Type == "DC Stock Transfer" && x.Delivery_Location_Code == dc_Code && x.Stock_Xfer_Dispatch_Number != null && x.is_Syunc == false && x.is_Deleted == false
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
                                                   Invoice_Flag = x.Invoice_Flag,
                                                   Price_Template_ID = x.Price_Template_ID,
                                                   Price_Template_Name = x.Price_Template_Name,
                                                   Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                                                   CreatedDate = x.CreatedDate,
                                                   CreateBy = x.CreateBy,
                                                   UpdateBy = x.UpdateBy,
                                                   UpdateDate = x.UpdateDate,
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
                                                                     Indent_Qty = c.Indent_Qty,
                                                                     Dispatch_Qty = c.Dispatch_Qty,
                                                                     Grade = c.Grade,
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

                int count = STI_PUSH_ENTITY.stnRecordes.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from STN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                scope.Complete();
            }
        }

        public void STNSet()
        {
            int lcount = 0;

            if (STI_PUSH_ENTITY.stnRecordes != null)
                foreach (var disp in STI_PUSH_ENTITY.stnRecordes)
                {
                    var ExistData = CloudDB.Dispatch_Creation.Where(x => x.Stock_Xfer_Dispatch_Number == disp.Stock_Xfer_Dispatch_Number).FirstOrDefault();
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
                            pullDisp.CSI_Number = disp.CSI_Number;
                            pullDisp.Indent_Rls_Date = disp.Indent_Rls_Date;
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
                            pullDisp.is_Syunc = false;
                            pullDisp.Invoice_Flag = disp.Invoice_Flag;
                            pullDisp.Price_Template_ID = disp.Price_Template_ID;
                            pullDisp.Price_Template_Name = disp.Price_Template_Name;
                            pullDisp.Price_Template_Valitity_upto = disp.Price_Template_Valitity_upto;
                            pullDisp.Remark = disp.Remark;
                            pullDisp.Status = disp.Status;
                            pullDisp.CreatedDate = disp.CreatedDate;
                            pullDisp.CreateBy = disp.CreateBy;
                            pullDisp.UpdateBy = disp.UpdateBy;
                            pullDisp.UpdateDate = disp.UpdateDate;
                            pullDisp.Price_Template_Code = disp.Price_Template_Code;

                            CloudDB.Dispatch_Creation.Add(pullDisp);
                            CloudDB.SaveChanges();
                            scope1.Complete();
                        }

                        int pulledDipsID = pullDisp.Dispatch_Id;
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(pullDisp.Stock_Xfer_Dispatch_Number + " Inserted into STN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

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
                        DataSyuncServices csAnother = new DataSyuncServices();
                        csAnother.STN_Single_Record_Update(disp.Stock_Xfer_Dispatch_Number);
                    }
                }
            int count = lcount;

            using (var scope7 = new TransactionScope())
            {
                CloudDB.dispatch_proc();
                CloudDB.SaveChanges();
                scope7.Complete();
            }

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Inserted into STN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }

            if (STI_PUSH_ENTITY.STNNumGenClass != null)
            {
                int alcount = 0;
                foreach (var autoNums in STI_PUSH_ENTITY.STNNumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.Stock_Xfer_Dispatch_Num_Gen.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.Stock_Xfer_Dispatch_Last_Number = autoNums.Stock_Xfer_Dispatch_Last_Number;

                            CloudDB.Entry(list).State = EntityState.Modified;
                            CloudDB.SaveChanges();
                        }

                        scope3.Complete();
                    }
                    alcount++;
                }
                int scount = alcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(scount + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                STI_PUSH_ENTITY.STNNumGenClass = null;
            }

        }

        public void STI_Single_Record_Update(string sti_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Stock_Transfer_Indent dispUpdate = CloudDB.Stock_Transfer_Indent.Where(x => x.STI_Number == sti_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(sti_number + " Sync Field Updated in STI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void STN_Single_Record_Update(string stn_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Dispatch_Creation dispUpdate = CloudDB.Dispatch_Creation.Where(x => x.Stock_Xfer_Dispatch_Number == stn_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(stn_number + " Sync Field Updated in STN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void STN_Update()
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
                tw.WriteLine(count + " Records Updated in STN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }
            STI_PUSH_ENTITY.stnRecordes = null;
        }

        public void STI_Update_Local()
        {
            int lcount = 0;
            foreach (var stiFromLocal in STI_PUSH_ENTITY.localStis)
            {
                using (var scope4 = new TransactionScope())
                {

                    Stock_Transfer_Indent updatelocalsti = CloudDB.Stock_Transfer_Indent.Where(t => t.STI_id == stiFromLocal.STI_id).FirstOrDefault();
                    updatelocalsti.is_Syunc = true;

                    CloudDB.Entry(updatelocalsti).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope4.Complete();
                }
                lcount++;
            }
            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Updated in STI. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }
            STI_PUSH_ENTITY.localStis = null;
        }
    }
}
