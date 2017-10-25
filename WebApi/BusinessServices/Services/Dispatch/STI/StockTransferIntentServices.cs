using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace BusinessServices
{
    public class StockTransferIntentServices : IStockTransferIntentServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public StockTransferIntentServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public bool STIUpdateById(STIUpdateEntity stiEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                foreach (STILIDs lst in stiEntity.StiLIds)
                {
                    STI_Line_item lineItem = _unitOfWork.StockSubRepository.GetByID(lst.sti_LineId);

                    if (lineItem != null)
                    {
                        if (lst.statusflag == 0)
                        {
                            lineItem.Status = "Closed";
                            _unitOfWork.StockSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 1)
                        {
                            lineItem.Status = "Partial";
                            _unitOfWork.StockSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 2)
                        {
                            lineItem.Status = "Exceed";
                            _unitOfWork.StockSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }

                    }
                }

                scope.Complete();
            }
            success = true;
            using (var fscope = new TransactionScope())
            {
                success = false;

                Stock_Transfer_Indent st = DB.Stock_Transfer_Indent.Where(x => x.STI_Number == stiEntity.id).FirstOrDefault();
                            if (st != null)
                            {
                                st.Status = "Closed";
                                DB.Entry(st).State = EntityState.Modified;
                                DB.SaveChanges();
                    }
                fscope.Complete();
                success = true;
            }

            return success;
        }

      
               
        //----------------------------------------CREATESTOCKTRANSFER------------------------

        public string CreateStockTransfer(StockTransferIntentEntity stockEntity)
        {

            string stiNumber, locationId, STI_prefix;
            int? incNumber;

            using (var iscope = new TransactionScope())
            {
                string locationID = stockEntity.Indent_Raised_by_DC_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                STI_prefix = rm.GetString("STIT");
                ST_NUM_Generation autoIncNumber = GetAutoIncrement(locationID);
                locationId = autoIncNumber.DC_Code;
                incNumber = autoIncNumber.ST_Last_Number;
                int? incrementedValue = incNumber + 1;
                var STincrement = DB.ST_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                STincrement.ST_Last_Number = incrementedValue;
                _unitOfWork.ST_NUMRepository.Update(STincrement);
                _unitOfWork.Save();
                stiNumber = STI_prefix + "/" + locationId + "/" + String.Format("{0:00000}", incNumber);

                iscope.Complete();
            }

            using (var scope = new TransactionScope())
            {

                var stocktransfer = new Stock_Transfer_Indent
                {
        
                    STI_Number = stiNumber,
                    Indent_Raised_by_DC_Id=stockEntity.Indent_Raised_by_DC_Id,
                    Indent_Raised_by_DC_Code = stockEntity.Indent_Raised_by_DC_Code,
                    STI_RLS_date = stockEntity.STI_RLS_date,
                    Material_Source_id = stockEntity.Material_Source_id,
                    Material_Source = stockEntity.Material_Source,
                    Delivery_DC_id=stockEntity.Delivery_DC_id,
                    Delivery_DC_Code=stockEntity.Delivery_DC_Code,
                    Intermediate_DC_Code = stockEntity.Intermediate_DC_Code,
                    STI_Type = stockEntity.STI_Type,
                    SKU_Type_Id=stockEntity.SKU_Type_Id,
                    SKU_Type=stockEntity.SKU_Type,
                    STI_Delivery_cycle = stockEntity.STI_Delivery_cycle,
                    DC_Delivery_Date = stockEntity.DC_Delivery_Date,
                    STI_raise_by = stockEntity.STI_raise_by,
                    STI_Approve_by = stockEntity.STI_Approve_by,
                    STI_Approval_Flag = null,
                    STI_Approved_date = stockEntity.STI_Approved_date,
                    Status = "Open",
                    Reason = stockEntity.Reason,
                    is_Deleted = false,
                    is_Syunc = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = stockEntity.CreatedBy
                };
                //

                _unitOfWork.StockTransferRepository.Insert(stocktransfer);
                _unitOfWork.Save();

                int? STIId = stocktransfer.STI_id;

                var model = new STI_Line_item();
                foreach (var sSub in stockEntity.STIDetails)
                {
                    model.STI_id = STIId;
                    model.STI_Number = stiNumber;
                    model.SKU_ID = sSub.SKU_ID;
                    model.SKU_Code = sSub.SKU_Code;
                    model.SKU_Name = sSub.SKU_Name;
                    model.Pack_Size = sSub.Pack_Size;
                    model.Pack_Type_Id = sSub.Pack_Type_Id;
                    model.Pack_Type = sSub.Pack_Type;
                    model.Pack_Weight_Type_Id = sSub.Pack_Weight_Type_Id;
                    model.Pack_Weight_Type = sSub.Pack_Weight_Type;
                    model.SKU_SubType_Id = sSub.SKU_SubType_Id;
                    model.SKU_SubType = sSub.SKU_SubType;
                    model.UOM = sSub.UOM;
                    model.Qty = sSub.Qty;
                    model.Grade = sSub.Grade;
                    model.Total_Qty = sSub.Total_Qty;
                    model.Remark = sSub.Remark;
                    model.Status = "Open";
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = sSub.CreatedBy;

                    _unitOfWork.StockSubRepository.Insert(model);
                    _unitOfWork.Save();

                }
                scope.Complete();
                return stocktransfer.STI_Number;
            }
        }
        //-------------------------------------GETST------------------------------------
                
        public List<StockTransferIntentEntity> SearchStockTransfer(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ProcessedBy, string ULocation, string Url)
        {
         var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges
           ).FirstOrDefault();
            //
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);

       var qu = (from a in DB.Stock_Transfer_Indent
                  where a.is_Deleted == false && a.STI_Approval_Flag == true && (a.Indent_Raised_by_DC_Code == ULocation || a.Material_Source == ProcessedBy)
                  select  new StockTransferIntentEntity
                  {

                      STI_id = a.STI_id,
                      STI_Number = a.STI_Number,
                      Indent_Raised_by_DC_Id = a.Indent_Raised_by_DC_Id,
                      Indent_Raised_by_DC_Code = a.Indent_Raised_by_DC_Code,
                      Delivery_DC_id = a.Delivery_DC_id,
                      Delivery_DC_Code = a.Delivery_DC_Code,
                      STI_RLS_date = a.STI_RLS_date,
                      Material_Source_id = a.Material_Source_id,
                      Material_Source = a.Material_Source,
                      Intermediate_DC_Code = a.Intermediate_DC_Code,
                      STI_Type = a.STI_Type,
                      SKU_Type_Id = a.SKU_Type_Id,
                      SKU_Type = a.SKU_Type,
                      STI_Delivery_cycle = a.STI_Delivery_cycle,
                      DC_Delivery_Date = a.DC_Delivery_Date,
                      STI_raise_by = a.STI_raise_by,
                      STI_Approve_by = a.STI_Approve_by,
                      STI_Approval_Flag = a.STI_Approval_Flag,
                      STI_Approved_date = a.STI_Approved_date,
                      Status = a.Status,
                      Reason = a.Reason,
                      is_Create = iCrt,
                      is_Delete = isDel,
                      is_Edit = isEdt,
                      is_Approval = isApp,
                      is_View = isViw,
                      Counting = (from p in DB.STI_Line_item
                                  where p.STI_id == a.STI_id
                                  select new
                                  {
                                      STI_id = p.STI_id
                                  }).Count(),
                      STIDetails = (from s in DB.STI_Line_item
                                    where s.STI_id == a.STI_id
                                    orderby s.SKU_Name
                                    select new SIT_LineItems
                                    {
                                        STI_Line_Id = s.STI_Line_Id,
                                        STI_id = s.STI_id,
                                        STI_Number = s.STI_Number,
                                        SKU_ID = s.SKU_ID,
                                        SKU_Code = s.SKU_Code,
                                        SKU_Name = s.SKU_Name,
                                        SKU_SubType_Id = s.SKU_SubType_Id,
                                        SKU_SubType = s.SKU_SubType,
                                        Pack_Size = s.Pack_Size,
                                        Pack_Type_Id = s.Pack_Type_Id,
                                        Pack_Type = s.Pack_Type,
                                        Pack_Weight_Type_Id = s.Pack_Weight_Type_Id,
                                        Pack_Weight_Type = s.Pack_Weight_Type,
                                        UOM = s.UOM,
                                        Grade = s.Grade,
                                        Total_Qty = s.Total_Qty,
                                        Remark = s.Remark,
                                        Qty = s.Qty,
                                        Indent_Raised_by_DC_Code = a.Indent_Raised_by_DC_Code,
                                        STI_RLS_date = a.STI_RLS_date,
                                        Material_Source = a.Material_Source,
                                        Delivery_DC_Code = a.Delivery_DC_Code,
                                        STI_Type = a.STI_Type,
                                        DC_Delivery_Date = a.DC_Delivery_Date,
                                        STI_raise_by = a.STI_raise_by,
                                        STI_Approve_by = a.STI_Approve_by,
                                        STI_Approved_date = a.STI_Approved_date,
                                        Status = a.Status,
                                        CreatedDate = a.CreatedDate,
                                        UpdatedDate = a.UpdatedDate,
                                        CreatedBy = a.CreatedBy,
                                        UpdatedBy = a.UpdatedBy
                                    }).ToList()
                  }).ToList();
     
            if (startDate.Value != null && endDate.Value != null)
            {
                qu = qu.Where(a => a.STI_RLS_date >= startDate.Value && a.STI_RLS_date.Value <= endDate.Value).ToList();
            }
            if (status != "null")
            {
                qu = qu.Where(a => a.Status == status).ToList();
            }
          
            return qu;
        }
        //-------------------------------------GETSTA------------------------------------



        public List<StockTransferIntentEntity> GetSTAAND(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url)
        {
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges
           ).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);

            var qu = (from a in DB.Stock_Transfer_Indent
                      where (a.STI_RLS_date.Value >= startDate.Value) && (a.STI_RLS_date.Value <= endDate.Value) && a.Status == status && a.is_Deleted == false && a.STI_Approval_Flag == null && a.Indent_Raised_by_DC_Code == ULocation
                      select new StockTransferIntentEntity
                      {
                          STI_id = a.STI_id,
                          STI_Number = a.STI_Number,
                          Indent_Raised_by_DC_Id = a.Indent_Raised_by_DC_Id,
                          Indent_Raised_by_DC_Code = a.Indent_Raised_by_DC_Code,
                          Delivery_DC_id = a.Delivery_DC_id,
                          Delivery_DC_Code = a.Delivery_DC_Code,
                          STI_RLS_date = a.STI_RLS_date,
                          Material_Source_id = a.Material_Source_id,
                          Material_Source = a.Material_Source,
                          Intermediate_DC_Code = a.Intermediate_DC_Code,
                          STI_Type = a.STI_Type,
                          SKU_Type_Id = a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          STI_Delivery_cycle = a.STI_Delivery_cycle,
                          DC_Delivery_Date = a.DC_Delivery_Date,
                          STI_raise_by = a.STI_raise_by,
                          STI_Approve_by = a.STI_Approve_by,
                          STI_Approval_Flag = a.STI_Approval_Flag,
                          STI_Approved_date = a.STI_Approved_date,
                          Status = a.Status,
                          Reason = a.Reason,
                          is_Deleted = a.is_Deleted,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.STI_Line_item
                                      where p.STI_id == a.STI_id
                                      select new
                                      {
                                          STI_id = p.STI_id
                                      }).Count(),
                      }).ToList();
          
            return qu;
        }
        public List<StockTransferIntentEntity> GetSTAOR(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url)
        {
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges
             ).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);
            var qu = (from a in DB.Stock_Transfer_Indent
                      where ((a.STI_RLS_date.Value >= startDate.Value) && (a.STI_RLS_date.Value <= endDate.Value) || a.Status == status) && a.is_Deleted == false && a.STI_Approval_Flag == null && a.Indent_Raised_by_DC_Code == ULocation
                      select new StockTransferIntentEntity
                      {
                          STI_id = a.STI_id,
                          STI_Number = a.STI_Number,
                          Indent_Raised_by_DC_Id = a.Indent_Raised_by_DC_Id,
                          Indent_Raised_by_DC_Code = a.Indent_Raised_by_DC_Code,
                          Delivery_DC_id = a.Delivery_DC_id,
                          Delivery_DC_Code = a.Delivery_DC_Code,
                          STI_RLS_date = a.STI_RLS_date,
                          Material_Source_id = a.Material_Source_id,
                          Material_Source = a.Material_Source,
                          Intermediate_DC_Code = a.Intermediate_DC_Code,
                          STI_Type = a.STI_Type,
                          SKU_Type_Id = a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          STI_Delivery_cycle = a.STI_Delivery_cycle,
                          DC_Delivery_Date = a.DC_Delivery_Date,
                          STI_raise_by = a.STI_raise_by,
                          STI_Approve_by = a.STI_Approve_by,
                          STI_Approval_Flag = a.STI_Approval_Flag,
                          STI_Approved_date = a.STI_Approved_date,
                          Status = a.Status,
                          Reason = a.Reason,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          is_Deleted = a.is_Deleted,
                          Counting = (from p in DB.STI_Line_item
                                      where p.STI_id == a.STI_id
                                      select new
                                      {
                                          STI_id = p.STI_id
                                      }).Count()
                      }).ToList();
              return qu;
        }

        //---------------------------------GETAUTOINCREMENT-----------------------

        public ST_NUM_Generation GetAutoIncrement(string locationId)
        {
            var autoinc = DB.ST_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();

            var model = new ST_NUM_Generation
            {
                ST_Num_Gen_Id = autoinc.ST_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                ST_Last_Number = autoinc.ST_Last_Number
            };

            return model;
        }

        //---------------------------------------GETSTOCKLINEITEM--------------------


        public List<StockTransferIntentEntity> GetStockLineItemForModel(string id)
        {
            var list = (from a in DB.Stock_Transfer_Indent
                        where a.STI_Number == id && a.is_Deleted == false
                        select new StockTransferIntentEntity
                        {

                            STI_id = a.STI_id,
                            STI_Number = a.STI_Number,
                            Indent_Raised_by_DC_Code = a.Indent_Raised_by_DC_Code,
                            Indent_Raised_by_DC_Id = a.Indent_Raised_by_DC_Id,
                            Delivery_DC_Code = a.Delivery_DC_Code,
                            Delivery_DC_id = a.Delivery_DC_id,
                            STI_RLS_date = a.STI_RLS_date,
                            Material_Source_id = a.Material_Source_id,
                            Material_Source = a.Material_Source,
                            Intermediate_DC_Code = a.Intermediate_DC_Code,
                            STI_Type = a.STI_Type,
                            SKU_Type_Id = a.SKU_Type_Id,
                            SKU_Type = a.SKU_Type,
                            STI_Delivery_cycle = a.STI_Delivery_cycle,
                            DC_Delivery_Date = a.DC_Delivery_Date,
                            STI_raise_by = a.STI_raise_by,
                            STI_Approve_by = a.STI_Approve_by,
                            STI_Approval_Flag = a.STI_Approval_Flag,
                            STI_Approved_date = a.STI_Approved_date,
                            Status = a.Status,
                            Reason = a.Reason,
                            is_Deleted = a.is_Deleted,
                            Counting = (from p in DB.STI_Line_item
                                        where p.STI_id == a.STI_id
                                        select new
                                        {
                                            STI_id = p.STI_id
                                        }).Count(),

                            ST_Qty_Sum = (from p in DB.STI_Line_item
                                          where p.STI_Number == id
                                          group p by p.STI_Number into g
                                          select new ST_Qty_SumEntity
                                          {
                                              Total_Qty_Sum = g.Sum(z => z.Qty)
                                          }),
                            STIDetails = (from s in DB.STI_Line_item
                                          where s.STI_id == a.STI_id
                                          orderby s.SKU_Name
                                          select new SIT_LineItems
                                          {
                                              STI_Line_Id = s.STI_Line_Id,
                                              STI_id = s.STI_id,
                                              STI_Number = s.STI_Number,
                                              SKU_ID = s.SKU_ID,
                                              SKU_Code = s.SKU_Code,
                                              SKU_Name = s.SKU_Name,
                                              SKU_SubType_Id = s.SKU_SubType_Id,
                                              SKU_SubType = s.SKU_SubType,
                                              Pack_Size = s.Pack_Size,
                                              Pack_Type_Id = s.Pack_Type_Id,
                                              Pack_Type = s.Pack_Type,
                                              Pack_Weight_Type_Id = s.Pack_Weight_Type_Id,
                                              Pack_Weight_Type = s.Pack_Weight_Type,
                                              UOM = s.UOM,
                                              Qty = s.Qty,
                                              Grade = s.Grade,
                                              Total_Qty = s.Total_Qty,
                                              Remark = s.Remark,
                                              Status = s.Status,
                                 }).ToList()
                        }).ToList();
            return list;
        }

        public List<StockTransferIntentEntity> GetStockLineItem(string id)
        {
            var list = (from a in DB.Stock_Transfer_Indent
                        where a.STI_Number == id && a.is_Deleted == false
                        select new StockTransferIntentEntity
                        {

                            STI_id = a.STI_id,
                            STI_Number = a.STI_Number,
                            Indent_Raised_by_DC_Code=a.Indent_Raised_by_DC_Code,
                            Indent_Raised_by_DC_Id=a.Indent_Raised_by_DC_Id,
                            Delivery_DC_Code=a.Delivery_DC_Code,
                            Delivery_DC_id=a.Delivery_DC_id,
                            STI_RLS_date = a.STI_RLS_date,
                            Material_Source_id = a.Material_Source_id,
                            Material_Source = a.Material_Source,
                            Intermediate_DC_Code = a.Intermediate_DC_Code,
                            STI_Type = a.STI_Type,
                            SKU_Type_Id = a.SKU_Type_Id,
                            SKU_Type = a.SKU_Type,
                            STI_Delivery_cycle = a.STI_Delivery_cycle,
                            DC_Delivery_Date = a.DC_Delivery_Date,
                            STI_raise_by = a.STI_raise_by,
                            STI_Approve_by = a.STI_Approve_by,
                            STI_Approval_Flag = a.STI_Approval_Flag,
                            STI_Approved_date = a.STI_Approved_date,
                            Status = a.Status,
                            Reason = a.Reason,
                            is_Deleted = a.is_Deleted,
                            Counting = (from p in DB.STI_Line_item
                                        where p.STI_id == a.STI_id
                                        select new
                                        {
                                            STI_id = p.STI_id
                                        }).Count(),

                            ST_Qty_Sum = (from p in DB.STI_Line_item
                                          where p.STI_Number == id
                                          group p by p.STI_Number into g
                                          select new ST_Qty_SumEntity
                                          {
                                              Total_Qty_Sum = g.Sum(z => z.Qty)
                                          }),
                            STIDetails = (from s in DB.STI_Line_item
                                          where s.STI_id == a.STI_id && s.Status == "Open"
                                          orderby s.SKU_Name
                                          select new SIT_LineItems
                                          {
                                              STI_Line_Id = s.STI_Line_Id,
                                              STI_id = s.STI_id,
                                              STI_Number = s.STI_Number,
                                              SKU_ID = s.SKU_ID,
                                              SKU_Code = s.SKU_Code,
                                              SKU_Name = s.SKU_Name,
                                              SKU_SubType_Id=s.SKU_SubType_Id,
                                              SKU_SubType=s.SKU_SubType,
                                              Pack_Size=s.Pack_Size,
                                              Pack_Type_Id=s.Pack_Type_Id,
                                              Pack_Type=s.Pack_Type,
                                              Pack_Weight_Type_Id=s.Pack_Weight_Type_Id,
                                              Pack_Weight_Type=s.Pack_Weight_Type,
                                              UOM = s.UOM,
                                              Qty = ((s.Qty!=null)?s.Qty:0.0) - ((from d in DB.Dispatch_Creation
                                                              join c in DB.Dispatch_Line_item on d.Dispatch_Id equals c.Dispatch_Id
                                                                                  where d.STI_Number == s.STI_Number && d.is_Deleted == false && d.Status == "Open" && c.SKU_Name == s.SKU_Name && c.SKU_SubType == s.SKU_SubType && c.Pack_Size == s.Pack_Size && c.Pack_Type == s.Pack_Type && c.Pack_Weight_Type == s.Pack_Weight_Type && c.UOM == s.UOM
                                                              select c.Dispatch_Qty
                                                             ).Sum() != null ? (from d in DB.Dispatch_Creation
                                                                                join c in DB.Dispatch_Line_item on d.Dispatch_Id equals c.Dispatch_Id
                                                                                where d.STI_Number == s.STI_Number && d.Status == "Open" && c.SKU_Name == s.SKU_Name && c.SKU_SubType == s.SKU_SubType && c.Pack_Size == s.Pack_Size && c.Pack_Type == s.Pack_Type && c.Pack_Weight_Type == s.Pack_Weight_Type && c.UOM == s.UOM && c.is_Deleted != true
                                                                                select c.Dispatch_Qty
                                                             ).Sum() : 0),
                                              Grade = s.Grade,
                                              Total_Qty = s.Total_Qty,
                                              Remark = s.Remark,
                                              Status = s.Status,
                                    }).ToList()
                        }).ToList();
            return list;
        }
        //-------------------------------------GETSTAPPROVALLIST-------------------------

        public List<StockTransferIntentEntity> GetSTApprovalList(string ULocation)
        {
            var list = (from a in DB.Stock_Transfer_Indent
                        where a.is_Deleted == false && a.Indent_Raised_by_DC_Code == ULocation
                        select new StockTransferIntentEntity
                        {
                            STI_id = a.STI_id,
                            STI_Number = a.STI_Number,
                            Indent_Raised_by_DC_Code = a.Indent_Raised_by_DC_Code,
                            Indent_Raised_by_DC_Id = a.Indent_Raised_by_DC_Id,
                            Delivery_DC_Code = a.Delivery_DC_Code,
                            Delivery_DC_id = a.Delivery_DC_id,
                            STI_RLS_date = a.STI_RLS_date,
                            Material_Source_id = a.Material_Source_id,
                            Material_Source = a.Material_Source,
                            Intermediate_DC_Code = a.Intermediate_DC_Code,
                            STI_Type = a.STI_Type,
                            SKU_Type_Id = a.SKU_Type_Id,
                            SKU_Type = a.SKU_Type,
                            STI_Delivery_cycle = a.STI_Delivery_cycle,
                            DC_Delivery_Date = a.DC_Delivery_Date,
                            STI_raise_by = a.STI_raise_by,
                            STI_Approve_by = a.STI_Approve_by,
                            STI_Approval_Flag = a.STI_Approval_Flag,
                            STI_Approved_date = a.STI_Approved_date,
                            Status = a.Status,
                            Reason = a.Reason,
                            is_Deleted = a.is_Deleted,
                            Counting = (from p in DB.STI_Line_item
                                        where p.STI_id == a.STI_id
                                        select new
                                        {
                                            STI_id = p.STI_id
                                        }).Count()
                        }).ToList();

            return list;
        }

       

        //--------------------------------------STAPPROVAL-------------------------------

        public bool stApproval(StockTransferIntentEntity stEntity)
        {
            var success = false;
            if (stEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var stockTEntity = _unitOfWork.StockTransferRepository.GetByID(stEntity.STI_id);
                    if (stockTEntity != null)
                    {

                        stockTEntity.STI_Approval_Flag = stEntity.STI_Approval_Flag;
                        stockTEntity.STI_Approved_date = DateTime.Now;
                        stockTEntity.STI_Approve_by = stEntity.STI_Approve_by;
                        stockTEntity.Reason = stEntity.Reason;

                        _unitOfWork.StockTransferRepository.Update(stockTEntity);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        //--------------------------UPDATESTOCKTRANSFER------------------------------

        public string UpdateStockTransfer(int stiId, BusinessEntities.StockTransferIntentEntity stockEntity)
        {
            string stiNumber = "";

            if (stockEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.StockTransferRepository.GetByID(stiId);
                    if (p != null)
                    {
        
                        stiNumber = stockEntity.STI_Number;
                        p.STI_Number = stockEntity.STI_Number;
                        p.Indent_Raised_by_DC_Id= stockEntity.Indent_Raised_by_DC_Id;
                        p.Indent_Raised_by_DC_Code = stockEntity.Indent_Raised_by_DC_Code;
                        p.STI_RLS_date = stockEntity.STI_RLS_date;
                        p.Material_Source_id = stockEntity.Material_Source_id;
                        p.Material_Source = stockEntity.Material_Source;
                        p.Intermediate_DC_Code = stockEntity.Intermediate_DC_Code;
                        p.Delivery_DC_id = stockEntity.Delivery_DC_id;
                        p.Delivery_DC_Code = stockEntity.Delivery_DC_Code;                 
                        p.STI_Type = stockEntity.STI_Type;
                        p.SKU_Type_Id = stockEntity.SKU_Type_Id;
                        p.SKU_Type = stockEntity.SKU_Type;
                        p.STI_Delivery_cycle = stockEntity.STI_Delivery_cycle;
                        p.DC_Delivery_Date = stockEntity.DC_Delivery_Date;
                        p.is_Deleted = false;
                        p.UpdatedDate = DateTime.Now;
                        p.UpdatedBy = stockEntity.UpdatedBy;

                        _unitOfWork.StockTransferRepository.Update(p);
                        _unitOfWork.Save();

                    }

                    var lineItemList = DB.STI_Line_item.Where(x => x.STI_id == stiId).ToList();
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var list = _unitOfWork.StockSubRepository.GetByID(li.STI_Line_Id);

                            if (list != null)
                            {
                                _unitOfWork.StockSubRepository.Delete(list);
                                _unitOfWork.Save();
                            }

                            scope1.Complete();
                        }
                    }

                    foreach (SIT_LineItems sitSub in stockEntity.STIDetails)
                    {
                       // var line = _unitOfWork.StockSubRepository.GetByID(sitSub.STI_Line_Id);
                        var model = new STI_Line_item();

                        //if (line != null)
                        //{
                        //    line.STI_id = stiId;
                        //    line.STI_Number = stiNumber;
                        //    line.SKU_ID = sitSub.SKU_ID;
                        //    line.SKU_Code = sitSub.SKU_Code;
                        //    line.SKU_Name = sitSub.SKU_Name;
                        //    line.Pack_Size = sitSub.Pack_Size;
                        //    line.Pack_Type_Id = sitSub.Pack_Type_Id;
                        //    line.Pack_Type = sitSub.Pack_Type;
                        //    line.Pack_Weight_Type_Id = sitSub.Pack_Weight_Type_Id;
                        //    line.Pack_Weight_Type = sitSub.Pack_Weight_Type;
                        //    line.SKU_SubType_Id = sitSub.SKU_SubType_Id;
                        //    line.SKU_SubType = sitSub.SKU_SubType;
                        //    line.UOM = sitSub.UOM;
                        //    line.Qty = sitSub.Qty;
                        //    line.Grade = sitSub.Grade;
                        //    line.Total_Qty = sitSub.Total_Qty;
                        //    line.Remark = sitSub.Remark;
                        //    line.Status = "Open";
                        //    line.UpdatedDate = DateTime.Now;
                        //    line.UpdatedBy = sitSub.UpdatedBy;

                        //    _unitOfWork.StockSubRepository.Update(line);
                        //    _unitOfWork.Save();
                        //}
                        //else
                        //{
                            model.STI_id = stiId;
                            model.STI_Number = stiNumber;
                            model.SKU_ID = sitSub.SKU_ID;
                            model.SKU_Code = sitSub.SKU_Code;
                            model.SKU_Name = sitSub.SKU_Name;
                            model.Pack_Size = sitSub.Pack_Size;
                            model.Pack_Type_Id = sitSub.Pack_Type_Id;
                            model.Pack_Type = sitSub.Pack_Type;
                            model.Pack_Weight_Type_Id = sitSub.Pack_Weight_Type_Id;
                            model.Pack_Weight_Type = sitSub.Pack_Weight_Type;
                            model.SKU_SubType_Id = sitSub.SKU_SubType_Id;
                            model.SKU_SubType = sitSub.SKU_SubType;
                            model.UOM = sitSub.UOM;
                            model.Qty = sitSub.Qty;
                            model.Grade = sitSub.Grade;
                            model.Total_Qty = sitSub.Total_Qty;
                            model.Remark = sitSub.Remark;
                            model.Status = "Open";
                            model.CreatedDate = DateTime.Now;
                            model.CreatedBy = sitSub.UpdatedBy;

                            _unitOfWork.StockSubRepository.Insert(model);
                            _unitOfWork.Save();
                       // }

                    }
                    scope.Complete();
                }
            }
            return stiNumber;
        }
        //--------------------------------DELETESTOCKTRANSFER-------------------------

        public bool DeleteStockTransfer(int stiId)
        {
            var success = false;
            if (stiId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var g = _unitOfWork.StockTransferRepository.GetByID(stiId);
                    if (g != null)
                    {
                        g.is_Deleted = true;
                        _unitOfWork.StockTransferRepository.Update(g);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        //---------------------------DELETESTILINEITEM----------------------------
        public bool DeleteSTIOrderLineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.StockSubRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.StockSubRepository.Delete(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool stiBulkApproval(STIbulkApprovalEntity bulkEntity)
        {
            var success = false;
            foreach (STIbulkIdsEntity id in bulkEntity.bulkid)
            {
                if (id.stiId > 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        var stiEntity = _unitOfWork.StockTransferRepository.GetByID(id.stiId);
                        if (stiEntity != null)
                        {
                            stiEntity.Reason = id.Reason;
                            stiEntity.STI_Approval_Flag = id.STI_Approval_Flag;
                            stiEntity.STI_Approved_date = DateTime.Now;
                            stiEntity.STI_Approve_by = id.STI_Approve_by;

                            _unitOfWork.StockTransferRepository.Update(stiEntity);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            return success;
        }
        //---------------------------------------GETSTATUSES----------------------------

        public List<Tuple<string>> getStatuses()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.Status", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("OpenStatus")));
            list.Add(new Tuple<string>(rm.GetString("CloseStatus")));
            return list;
        }

        //--------------------------------------------------excel import for sti---------------------------

        public List<STIExcelFields> ExcelImportForCI(fileImportSTI fileDetail)
        {
            List<STIExcelFields> stilist = new List<STIExcelFields>();
            STIExcelFields stiDetail = new STIExcelFields();

            string Profilepicname = "STI_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/STI";
            string dirCreatePath = "";
            try
            {
                string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                string ext = ".xlsx";
                dirCreatePath = RootPath;

                if (!Directory.Exists(dirCreatePath))
                {
                    Directory.CreateDirectory(RootPath);
                }
                sPath = RootPath;
                name = Profilepicname + ext;
                vPath = sPath + "\\" + name;

                if (File.Exists(vPath))
                {
                    File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
                }
                else
                {
                    File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
                }

                FileStream stream = File.Open(vPath, FileMode.Open, FileAccess.Read);
                stream.Position = 0;
                IExcelDataReader excelReader = null;

                if (ext == ".xls")
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (ext == ".xlsx")
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                int columnCount = result1.Tables[0].Columns.Count;

                if (columnCount > 8)
                {
                    stiDetail.status = false;
                    stiDetail.Message = "Extra Column's Present in Given Excel";
                    stilist.Add(stiDetail);
                    return stilist;
                }

                //--------------------validating Excel Columns
                int lineCount = 1;

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        STI_Line_item ci = new STI_Line_item();
                        object[] A = { };

                        ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        ci.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        ci.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        ci.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        ci.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ci.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        ci.Qty = d["Qty"] != null && d["Qty"].ToString() != "" ? double.Parse(d["Qty"].ToString()) : 0;

                        using (var iscope = new TransactionScope())
                        {
                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == ci.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            if (subtypedetail != null)
                            {
                                ci.SKU_SubType = subtypedetail.SKU_SubType_Name;
                                ci.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "SKU_SubType_Name";
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            if (skuDetail != null)
                            {
                                ci.SKU_Name = skuDetail.SKU_Name;
                                ci.SKU_ID = skuDetail.SKU_Id;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "SKU_Name";
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
                            if (uomDetail != null)
                            {
                                ci.UOM = uomDetail.Unit_Name;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "UOM";
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ci.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            if (packtypedetail != null)
                            {
                                ci.Pack_Type = packtypedetail.Pack_Type_Name;
                                ci.Pack_Type_Id = packtypedetail.Pack_Type_Id;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "Pack_Type_Name";
                                stilist.Add(stiDetail);
                                return stilist;
                            }
                           
                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            if (packweightType != null)
                            {
                                ci.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                                ci.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "Pack_Weight_Type";
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ci.Grade.ToLower().Trim()).FirstOrDefault();
                            if (gradeDetail != null)
                            {
                                ci.Grade = gradeDetail.GradeType_Name;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "Grade";
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ci.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            if (packsizedetail != null)
                            {
                                ci.Pack_Size = packsizedetail.Pack_Size_Value;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "Pack_Size_Value";
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            iscope.Complete();
                        }

                        lineCount += 1;
                    }

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {

                        stiDetail.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        stiDetail.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        stiDetail.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        stiDetail.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        stiDetail.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        stiDetail.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        stiDetail.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        stiDetail.Qty = d["Qty"] != null && d["Qty"].ToString() != "" ? double.Parse(d["Qty"].ToString()) : 0;

                        using (var iscope = new TransactionScope())
                        {

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == stiDetail.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            stiDetail.SKU_Name = skuDetail.SKU_Name;
                            stiDetail.SKU_ID = skuDetail.SKU_Id;

                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == stiDetail.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            stiDetail.SKU_SubType = subtypedetail.SKU_SubType_Name;
                            stiDetail.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == stiDetail.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            stiDetail.Pack_Type = packtypedetail.Pack_Type_Name;
                            stiDetail.Pack_Type_Id = packtypedetail.Pack_Type_Id;


                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == stiDetail.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            stiDetail.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                            stiDetail.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == stiDetail.Grade.ToLower().Trim()).FirstOrDefault();
                            stiDetail.Grade = gradeDetail.GradeType_Name;

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == stiDetail.UOM.ToLower().Trim()).FirstOrDefault();
                            stiDetail.UOM = uomDetail.Unit_Name;

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == stiDetail.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            stiDetail.Pack_Size = packsizedetail.Pack_Size_Value;

                            stilist.Add(stiDetail);
                            iscope.Complete();
                        }

                        lineCount += 1;

                    }

                excelReader.Close();
            }
            catch (Exception e)
            {
                stilist = new List<STIExcelFields>();
                stiDetail.status = false;
                stiDetail.Message = e.Message.ToString();
                stilist.Add(stiDetail);
                return stilist;
            }
            stilist[0].status = true;
            stilist[0].Message = "Success";
            
            return stilist;
        }
        
    }
}
