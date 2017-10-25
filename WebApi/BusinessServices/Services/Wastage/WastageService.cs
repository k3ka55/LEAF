using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
    public class WastageService : IWastageService
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public WastageService()
        {
            _unitOfWork = new UnitOfWork();
        }
        //--------------------------------CREATE----------------------------------------
        public List<GrnEntity> GetCDNwastage(string cdnNumber)
        {
            var query = (from x in DB.GRN_Creation
                         join z in DB.GRN_Line_item on x.GRN_Number equals z.GRN_Number
                         where x.CDN_Number == cdnNumber && x.is_Deleted == false
                         select new GrnEntity
                         {
                             INW_Id = x.INW_Id,
                             GRN_Number = x.GRN_Number,
                             PO_Number = x.PO_Number,
                             GRN_Rls_Date = x.GRN_Rls_Date,
                             Voucher_Type = x.Voucher_Type,
                             SKU_Type = x.SKU_Type,
                             STN_DC_Id = x.STN_DC_Id,
                             STN_DC_Code = x.STN_DC_Code,
                             STN_DC_Name = x.STN_DC_Name,
                             Supplier_Id = x.Supplier_Id,
                             Supplier_code = x.Supplier_code,
                             Supplier_Name = x.Supplier_Name,
                             DC_Code = x.DC_Code,
                             CreatedBy = x.CreatedBy,
                              GrnDetails = (from a in DB.GRN_Line_item
                                           where a.INW_id == x.INW_Id
                                            orderby a.SKU_Name
                                           select new GrnLineItemsEntity
                                           {
                                               GRN_Line_Id = a.GRN_Line_Id,
                                               INW_id = a.INW_id,
                                               GRN_Number = a.GRN_Number,
                                               SKU_ID = a.SKU_ID,
                                               SKU_Code = a.SKU_Code,
                                               SKU_Name = a.SKU_Name,
                                               SKU_SubType = a.SKU_SubType,
                                               UOM = a.UOM,
                                               PO_QTY = a.PO_QTY,
                                               C_Accepted_Qty = a.C_Accepted_Qty,
                                               C_Accepted_Price = a.C_Accepted_Price,
                                               Billed_Qty = a.Billed_Qty,
                                               Price_Book_Id = a.Price_Book_Id,
                                               Price = a.Price,
                                               Remark = a.Remark,
                                               Total_Accepted_Qty = (a.A_Accepted_Qty != null ? a.A_Accepted_Qty : 0) + (a.B_Accepted_Qty != null ? a.B_Accepted_Qty : 0)
                                           }).ToList(),
                         }).ToList();
            return query;
        }
        public List<cdnNumber> GetCdnNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.GRN_Creation
                         join z in DB.GRN_Line_item on x.GRN_Number equals z.GRN_Number
                         join y in DB.Dispatch_Creation on x.CDN_Number equals y.Customer_Dispatch_Number
                         where x.GRN_Rls_Date.Value.Year == date.Value.Year && x.GRN_Rls_Date.Value.Month == date.Value.Month && x.GRN_Rls_Date.Value.Day == date.Value.Day && x.is_Deleted == false && x.DC_Code == Ulocation && z.C_Accepted_Qty != null
                         select new cdnNumber
                         {
                             CDN_Number = x.CDN_Number
                         }).ToList();
            return query;
        }
        public string CreateWastage(WastageEntity wsEntity)
        {
            string prefix, locationId, wsNumber;
            int? incNumber;

            using (var scopef = new TransactionScope())
            {
                string locationID = wsEntity.DC_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                prefix = rm.GetString("WST");
                Wastage_Auto_Num_Gen autoIncNumber = GetWastAutoIncrement(locationID);
                locationId = autoIncNumber.DC_Code;
                incNumber = autoIncNumber.Wastage_Last_Number;
                int? incrementedValue = incNumber + 1;
                var WSincrement = DB.Wastage_Auto_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                WSincrement.Wastage_Last_Number = incrementedValue;
                _unitOfWork.WastageNumIncrementRepository.Update(WSincrement);
                _unitOfWork.Save();
                wsNumber = prefix + "/" + locationId + "/" + String.Format("{0:00000}", incNumber);
                scopef.Complete();
            }

            using (var scope = new TransactionScope())
            {
                var Wastage = new Wastage_Creation
                {
                    Wastage_Number = wsNumber,
                    DC_Id = wsEntity.DC_Id,
                    DC_Code = wsEntity.DC_Code,
                    DC_Name = wsEntity.DC_Name,
                    Ref_Id = wsEntity.Ref_Id,
                    Ref_Number = wsEntity.Ref_Number,
                    Wastage_Type = wsEntity.Wastage_Type,
                    Wastage_raisedBy = wsEntity.Wastage_raisedBy,
                    Wastage_Rls_Date = wsEntity.Wastage_Rls_Date,
                    is_Deleted = false,
                    Remark = wsEntity.Remark,
                    CreatedDate = DateTime.Now,
                    CreatedBy = wsEntity.CreatedBy,
                    Wastage_Approval_Flag = null,
                    is_Syunc = false,
                };

                _unitOfWork.WastageRepository.Insert(Wastage);
                _unitOfWork.Save();

                DateTime Today = DateTime.Now;
                var check = (from ee in DB.Stirnkage_Summary
                             where ee.DC_Code == wsEntity.DC_Code
                             && ee.CreatedDate.Value.Year == Today.Year
                              && ee.CreatedDate.Value.Month == Today.Month
                               && ee.CreatedDate.Value.Day == Today.Day
                               && ee.Adjustment_Freeze == false
                             select ee).FirstOrDefault();
                if (check != null)
                {
                    check.Adjustment_Freeze = true;
                    DB.Entry(check).State = EntityState.Modified;
                    DB.SaveChanges();
                }

                int? wsId = Wastage.Wastage_Id;
                int? RefId = 0;
                string RefNum = "";
                if (wsEntity.Wastage_Type == "Process" || wsEntity.Wastage_Type == "Floor")
                {
                    RefNum = null;
                }
                else if (wsEntity.Wastage_Type == "Customer Return")
                {
                    RefId = Wastage.Ref_Id;
                    RefNum = Wastage.Ref_Number;
                }
                string wastageType = Wastage.Wastage_Type;
                if (wsEntity.Wastage_Type == "Process" || wsEntity.Wastage_Type == "Floor")
                {
                    var model = new Wastage_Line_item();
                    foreach (WastageLineItemEntity pSub in wsEntity.WastageLineDetails)
                    {
                        model.Wastage_Number = wsNumber;
                        model.Wastage_Id = wsId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.SKU_Type_Id = pSub.SKU_Type_Id;
                        model.SKU_Type = pSub.SKU_Type;
                        model.UOM = pSub.UOM;
                        model.Grade = pSub.Grade;
                        model.Ref_Id = pSub.Ref_Id;
                        model.Ref_Line_Id = pSub.Ref_Line_Id;
                        model.Wastage_Qty = pSub.Wastage_Qty;
                        model.Wasted_Qty_Price = pSub.Wasted_Qty_Price;
                        model.is_Stk_Update = false;
                        model.Reason = pSub.Reason;
                        model.Stock_Reduce_Flag = false;

                        _unitOfWork.WastageSubRepository.Insert(model);
                        _unitOfWork.Save();

                    }
                }
                else if (wsEntity.Wastage_Type == "Customer Return")
                {
                    var model = new Wastage_Line_item();
                    foreach (WastageLineItemEntity pSub in wsEntity.WastageLineDetails)
                    {

                        model.Wastage_Number = wsNumber;
                        model.Wastage_Id = wsId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.SKU_Type_Id = pSub.SKU_Type_Id;
                        model.SKU_Type = pSub.SKU_Type;
                        model.UOM = pSub.UOM;
                        model.Grade = pSub.Grade;
                        model.Ref_Id = pSub.Ref_Id;
                        model.Ref_Line_Id = pSub.Ref_Line_Id;
                        model.Wastage_Qty = pSub.Wastage_Qty;
                        model.Wasted_Qty_Price = pSub.Wasted_Qty_Price;
                        model.is_Stk_Update = true;
                        model.Reason = pSub.Reason;
                        model.Stock_Reduce_Flag = false;


                        _unitOfWork.WastageSubRepository.Insert(model);
                        _unitOfWork.Save();

                    }
                }

                scope.Complete();
                return Wastage.Wastage_Number;
            }
        }

        //------------------------------------------CREATE----------------------------------------
        public Wastage_Auto_Num_Gen GetWastAutoIncrement(string locationId)
        {
            var autoinc = DB.Wastage_Auto_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new Wastage_Auto_Num_Gen
            {
                Id = autoinc.Id,
                Wastage_Last_Number = autoinc.Wastage_Last_Number,
                DC_Code = autoinc.DC_Code
            };

            return model;
        }

        public IEnumerable<WS_QtySumEntityModel> GetDCWastages(DateTime? fdate,DateTime? tdate, string DCCode)
        {
            var dddd = (from a in DB.Wastage_Creation
                        join b in DB.Wastage_Line_item on new { Wastage_Id = a.Wastage_Id } equals new { Wastage_Id = (int)b.Wastage_Id }
                        where a.Wastage_Rls_Date >= fdate && a.Wastage_Rls_Date <= tdate && a.DC_Code == DCCode
                        group new { b, a } by new
                        {
                            b.SKU_Name,
                            b.SKU_Type,
                            b.Grade,
                            a.Wastage_Type,
                            b.UOM,
                            a.DC_Code
                        } into g
                        select new
                        {
                            g.Key.SKU_Name,
                            g.Key.SKU_Type,
                            g.Key.Grade,
                            g.Key.Wastage_Type,
                            g.Key.UOM,
                            g.Key.DC_Code,
                            Wastage_Qty = (double?)g.Sum(p => p.b.Wastage_Qty)
                        }).Distinct();
           
            
            var query = (dddd.GroupBy(c => new { c.SKU_Name, c.Grade, c.SKU_Type, c.DC_Code, c.UOM })
  .Select(g => new WS_QtySumEntity
  {

      Unit = g.Key.UOM,
      DC_Code = g.Key.DC_Code,
      SkuName = g.Key.SKU_Name,
      SKU_Type = g.Key.SKU_Type,
      Grade = g.Key.Grade,
      Process = Math.Round(g.Where(c => c.Wastage_Type == "Process").Sum(c => c.Wastage_Qty.Value), 2),
      Floor = Math.Round(g.Where(c => c.Wastage_Type == "Floor").Sum(c => c.Wastage_Qty.Value), 2),

  })).ToList();

            List<WS_QtySumEntity> sumry = new List<WS_QtySumEntity>();
            foreach (var y in query)
            {
                var hh = new WS_QtySumEntity();
                hh.SkuName = y.SkuName;
                hh.SKU_Type = y.SKU_Type;
                hh.Grade = y.Grade;
                //var t = (((y.Process != null) ? Math.Round(y.Process.Value, 3) : 0.00)).ToString("0.00");Convert.ToDouble(t)
                hh.Process = (y.Process != null ? Math.Round(y.Process.Value, 3) : 0);
                hh.Floor = ((y.Floor != null) ? Math.Round(y.Floor.Value, 3) : 0);
                hh.DC_Code = y.DC_Code;
                hh.Unit = y.Unit;
                hh.Total_Qty_Sum = Math.Round((((hh.Process != null) ? hh.Process.Value : 0) + ((hh.Floor != null) ? hh.Floor.Value : 0)));
                sumry.Add(hh);
            }
            List<WS_QtySumEntityModel> output = new List<WS_QtySumEntityModel>();

            foreach (var t in sumry)
            {
                var hh = new WS_QtySumEntityModel();
                hh.SkuName = t.SkuName;
                hh.SKU_Type = t.SKU_Type;
                hh.Grade = t.Grade;
                hh.Process = (((t.Process != null) ? Math.Round(t.Process.Value, 3) : 0));
                //((y.Process != null) ? Math.Round(y.Process.Value, 2) : 0.00);
                hh.Floor = (((t.Floor != null) ? Math.Round(t.Floor.Value, 3) : 0));
                hh.DC_Code = t.DC_Code;
                hh.Unit = t.Unit;
                hh.Total_Qty_Sum = (Math.Round((((t.Process != null) ? t.Process.Value : 0) + ((t.Floor != null) ? t.Floor.Value : 0))));
                output.Add(hh);
            }

            return output;

        }
      
        public List<WastageEntity> GetWastageItem(string id)
        {
            var list = (from x in DB.Wastage_Creation
                        where x.Wastage_Number == id
                        select new WastageEntity
                        {
                            Wastage_Id = x.Wastage_Id,
                            Wastage_Number = x.Wastage_Number,
                            DC_Id = x.DC_Id,
                            DC_Name = x.DC_Name,
                            DC_Code = x.DC_Code,
                            Wastage_Rls_Date = x.Wastage_Rls_Date,
                            Wastage_Type = x.Wastage_Type,
                            Wastage_raisedBy = x.Wastage_raisedBy,
                            Wastage_approved_by = x.Wastage_approved_by,
                            Wastage_approved_user_id = x.Wastage_approved_user_id,
                            Ref_Id = x.Ref_Id,
                            Ref_Number = x.Ref_Number,
                            Remark = x.Remark,
                            WastageLineDetails = (from a in DB.Wastage_Line_item
                                                  where a.Wastage_Id == x.Wastage_Id
                                                  orderby a.SKU_Name
                                                  select new WastageLineItemEntity
                                                  {
                                                      Wastage_Line_Id = a.Wastage_Line_Id,
                                                      Wastage_Id = a.Wastage_Id,
                                                      Wastage_Number = a.Wastage_Number,
                                                      SKU_ID = a.SKU_ID,
                                                      SKU_Code = a.SKU_Code,
                                                      SKU_Name = a.SKU_Name,
                                                      SKU_Type_Id = a.SKU_Type_Id,
                                                      SKU_Type = a.SKU_Type,
                                                      Ref_Id = a.Ref_Id,
                                                      Ref_Line_Id = a.Ref_Line_Id,
                                                      UOM = a.UOM,
                                                      Grade = a.Grade,
                                                      Wastage_Qty = a.Wastage_Qty,
                                                      Wasted_Qty_Price = a.Wasted_Qty_Price,
                                                      Reason = a.Reason,

                                                  }).ToList(),
                            WS_Qty_Sum = (from p in DB.Wastage_Line_item
                                          where p.Wastage_Number == id
                                          group p by p.Wastage_Number into g
                                          select new WS_QtySumEntity
                                          {
                                              Total_Qty_Sum = g.Sum(z => z.Wastage_Qty)

                                          })
                        }).ToList();

            return list;
        }

        //------------------POBulk Approval-------------------------------

        public bool wsBulkApproval(bulkWastApprovalEntity bulkwasEntity)
        {
            var success = false;
          //  double? Grn_Qty_Sum = 0.0;
            foreach (bulkWastIdsEntity id in bulkwasEntity.bulkWastid)
            {
                if (id.Wastage_Id > 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        var wsEntity = _unitOfWork.WastageRepository.GetByID(id.Wastage_Id);
                        if (wsEntity != null)
                        {
                            wsEntity.Reject_Reason = id.Reason;
                            wsEntity.Wastage_Approval_Flag = id.Wastage_Approval_Flag;
                            wsEntity.Wastage_approved_date = DateTime.Now;
                            wsEntity.Wastage_approved_by = id.Wastage_approved_by;
                            wsEntity.Wastage_approved_user_id = id.Wastage_approved_user_id;

                            _unitOfWork.WastageRepository.Update(wsEntity);
                            _unitOfWork.Save();

                            if (wsEntity.Wastage_Approval_Flag == true)
                            {
                                var lineItems = DB.Wastage_Line_item.Where(x => x.Wastage_Id == id.Wastage_Id).ToList();

                                foreach (Wastage_Line_item lineitem in lineItems)
                                {
                                    
                                        List<Stock> line = (from w in DB.Stocks
                                                            where w.SKU_Name == lineitem.SKU_Name && w.Grade == lineitem.Grade && w.SKU_Type == lineitem.SKU_Type && w.DC_Code == wsEntity.DC_Code
                                                            select w).ToList();

                                        foreach (Stock m in line)
                                        {                                        
                                                var wastageSupplier = new Wastage_Supplier_Info
                                                {
                                                    Wastage_Number = lineitem.Wastage_Number,
                                                    Wastage_Line_Id = lineitem.Wastage_Line_Id,
                                                    SKU_Code = m.SKU_Code,
                                                    SKU_Name = m.SKU_Name,
                                                    DC_Code = m.DC_Code,
                                                    Grade = m.Grade
                                                };

                                                _unitOfWork.WastageSupplierRepository.Insert(wastageSupplier);
                                                _unitOfWork.Save();
                                    }
                                    success = true;
                                }                              
                            }
                        }
                        scope.Complete();
                    }
                   
                }
            }
            
            using (var scope = new TransactionScope())
            {
                DB.Wastage_proc();
                DB.SaveChanges();
                scope.Complete();
            }
            return success;
        }
        //--------------------------UpdatePurchaseOrder(int poId, PurchaseOrderEntity POEntity)

        public string UpdateWastage(int wsId, WastageEntity wsEntity)
        {
            string wsNumber = "";

            if (wsEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.WastageRepository.GetByID(wsId);
                    if (p != null)
                    {

                        wsNumber = wsEntity.Wastage_Number;
                        p.DC_Id = wsEntity.DC_Id;
                        p.DC_Code = wsEntity.DC_Code;
                        p.DC_Name = wsEntity.DC_Name;
                        p.Ref_Id = wsEntity.Ref_Id;
                        p.Ref_Number = wsEntity.Ref_Number;
                        p.Wastage_Type = wsEntity.Wastage_Type;
                        p.Wastage_raisedBy = wsEntity.Wastage_raisedBy;
                        p.Wastage_Rls_Date = wsEntity.Wastage_Rls_Date;
                        p.Remark = wsEntity.Remark;
                        p.UpdatedDate = DateTime.Now;
                        p.UpdatedBy = wsEntity.UpdatedBy;
                        _unitOfWork.WastageRepository.Update(p);
                        _unitOfWork.Save();
                    }

                    DateTime Today = DateTime.Now;
                    var check = (from ee in DB.Stirnkage_Summary
                                 where ee.DC_Code == wsEntity.DC_Code
                                 && ee.CreatedDate.Value.Year == Today.Year
                                  && ee.CreatedDate.Value.Month == Today.Month
                                   && ee.CreatedDate.Value.Day == Today.Day
                                   && ee.Adjustment_Freeze == false
                                 select ee).FirstOrDefault();
                    if (check != null)
                    {
                        check.Adjustment_Freeze = true;
                        DB.Entry(check).State = EntityState.Modified;
                        DB.SaveChanges();
                    }

                    var lineItemList = DB.Wastage_Line_item.Where(x => x.Wastage_Id == wsId).ToList();
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var list = _unitOfWork.WastageSubRepository.GetByID(li.Wastage_Line_Id);

                            if (list != null)
                            {
                                _unitOfWork.WastageSubRepository.Delete(list);
                                _unitOfWork.Save();
                            }

                            scope1.Complete();
                        }
                    }

                    foreach (WastageLineItemEntity pSub in wsEntity.WastageLineDetails)
                    {
                      //  var line = _unitOfWork.WastageSubRepository.GetByID(pSub.Wastage_Line_Id);
                        var model = new Wastage_Line_item();

                        //if (line != null)
                        //{
                        //    line.SKU_ID = pSub.SKU_ID;
                        //    line.SKU_Code = pSub.SKU_Code;
                        //    line.SKU_Name = pSub.SKU_Name;
                        //    line.SKU_Type_Id = pSub.SKU_Type_Id;
                        //    line.SKU_Type = pSub.SKU_Type;
                        //    line.UOM = pSub.UOM;
                        //    line.Grade = pSub.Grade;
                        //    line.Ref_Id = pSub.Ref_Id;
                        //    line.Ref_Line_Id = pSub.Ref_Line_Id;
                        //    line.Wastage_Qty = pSub.Wastage_Qty;
                        //    line.Reason = pSub.Reason;
                        //    line.Wasted_Qty_Price = pSub.Wasted_Qty_Price;

                        //    _unitOfWork.WastageSubRepository.Update(line);
                        //    _unitOfWork.Save();
                        //}
                        //else
                        //{
                            model.Wastage_Number = wsNumber;
                            model.Wastage_Id = wsId;
                            model.SKU_ID = pSub.SKU_ID;
                            model.SKU_Code = pSub.SKU_Code;
                            model.SKU_Name = pSub.SKU_Name;
                            model.SKU_Type_Id = pSub.SKU_Type_Id;
                            model.SKU_Type = pSub.SKU_Type;
                            model.UOM = pSub.UOM;
                            model.Grade = pSub.Grade;
                            model.Ref_Id = pSub.Ref_Id;
                            model.Ref_Line_Id = pSub.Ref_Line_Id;
                            model.Wastage_Qty = pSub.Wastage_Qty;
                            model.Reason = pSub.Reason;
                            model.Wasted_Qty_Price = pSub.Wasted_Qty_Price;
                            if (wsEntity.Wastage_Type == "Process" || wsEntity.Wastage_Type == "Floor")
                            {
                                model.is_Stk_Update = false;
                            }
                            else
                            {
                                model.is_Stk_Update = true;
                            }

                            _unitOfWork.WastageSubRepository.Insert(model);
                            _unitOfWork.Save();
                        //}
                    }
                    scope.Complete();
                }
            }
            using (var scope = new TransactionScope())
            {
                DB.Wastage_proc();
                DB.SaveChanges();
                scope.Complete();
            }
            return wsNumber;
        }

        //------------------------DELETEPURCHASEORDER----------

        public bool DeleteWastage(int wsId)
        {
            var success = false;
            if (wsId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.WastageRepository.GetByID(wsId);
                    if (p != null)
                    {
                        p.is_Deleted = true;

                        DateTime Today = DateTime.Now;
                        var check = (from ee in DB.Stirnkage_Summary
                                     where ee.DC_Code == p.DC_Code
                                     && ee.CreatedDate.Value.Year == Today.Year
                                      && ee.CreatedDate.Value.Month == Today.Month
                                       && ee.CreatedDate.Value.Day == Today.Day
                                       && ee.Adjustment_Freeze == false
                                     select ee).FirstOrDefault();
                        if (check != null)
                        {
                            check.Adjustment_Freeze = true;
                            DB.Entry(check).State = EntityState.Modified;
                            DB.SaveChanges();
                        }

                        p.UpdatedDate = DateTime.Now;
                        //p.Delete_Reason = deleteReason;
                        _unitOfWork.WastageRepository.Update(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                    using (var scope1 = new TransactionScope())
                    {
                        DB.Wastage_add(wsId);
                        DB.SaveChanges();
                        scope1.Complete();
                    }
                }
            }
            return success;
        }


        public bool DeleteWastageLineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.WastageSubRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.WastageSubRepository.Delete(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }


        //-----------------------------------------SEARCH-------------------------------------------

        public List<WastageEntity> SearchWastage(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType, string Ulocation, string Url)
        {
            List<WastageEntity> Result = new List<WastageEntity>();
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
        var  qu = (from a in DB.Wastage_Creation
                   where a.is_Deleted == false && a.Wastage_Approval_Flag == true && a.DC_Code == Ulocation
                   select new WastageEntity
                  {
                      Wastage_Id = a.Wastage_Id,
                      Wastage_Number = a.Wastage_Number,
                      DC_Id = a.DC_Id,
                      DC_Code = a.DC_Code,
                      DC_Name = a.DC_Name,
                      Ref_Id = a.Ref_Id,
                      Ref_Number = a.Ref_Number,
                      Wastage_Type = a.Wastage_Type,
                      Wastage_raisedBy = a.Wastage_raisedBy,
                      Wastage_Rls_Date = a.Wastage_Rls_Date,
                      Remark = a.Remark,
                      Reject_Reason = a.Reject_Reason,
                      Wastage_approved_user_id = a.Wastage_approved_user_id,
                      Wastage_approved_by = a.Wastage_approved_by,
                      Wastage_approved_date = a.Wastage_approved_date,
                      CreatedBy = a.CreatedBy,
                      UpdatedBy = a.UpdatedBy,
                      is_Create = iCrt,
                      is_Delete = isDel,
                      is_Edit = isEdt,
                      is_Approval = isApp,
                      is_View = isViw,
                      Counting = (from p in DB.Wastage_Line_item
                                  where p.Wastage_Id == a.Wastage_Id
                                  select new
                                  {
                                      Wastage_Id = p.Wastage_Id
                                  }).Count(),
                      WastageLineDetails = (from g in DB.Wastage_Line_item
                                            where g.Wastage_Id == a.Wastage_Id
                                            orderby g.SKU_Name
                                            select new WastageLineItemEntity
                                            {
                                                Wastage_Line_Id = g.Wastage_Line_Id,
                                                Wastage_Id = g.Wastage_Id,
                                                Wastage_Number = g.Wastage_Number,
                                                SKU_ID = g.SKU_ID,
                                                SKU_Code = g.SKU_Code,
                                                SKU_Name = g.SKU_Name,
                                                SKU_Type_Id = g.SKU_Type_Id,
                                                SKU_Type = g.SKU_Type,
                                                Ref_Id = g.Ref_Id,
                                                Ref_Line_Id = g.Ref_Line_Id,
                                                UOM = g.UOM,
                                                Grade = g.Grade,
                                                Wastage_Qty = g.Wastage_Qty,
                                                Wasted_Qty_Price = g.Wasted_Qty_Price,
                                                Reason = g.Reason,

                                                DC_Name = a.DC_Name,
                                                Ref_Number = a.Ref_Number,
                                                Wastage_Type = a.Wastage_Type,
                                                Wastage_raisedBy = a.Wastage_raisedBy,
                                                Wastage_Rls_Date = a.Wastage_Rls_Date,
                                                Remark = a.Remark,
                                                Wastage_approved_by = a.Wastage_approved_by,
                                                Wastage_approved_date = a.Wastage_approved_date,

                                            }).ToList(),
                  });
      
            if (startDate.Value != null && endDate.Value != null)
            {
                qu = qu.Where(a => a.Wastage_Rls_Date.Value >= startDate.Value && a.Wastage_Rls_Date.Value <= endDate.Value);
            }
            if (wastageType != "null")
            {
                qu = qu.Where(a => a.Wastage_Type == wastageType);
            }
            Result = qu.ToList();
           return Result;
        }

        public List<WastageEntity> GetWSApprovalAND(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType, string ULocation, string Url)
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
            var qu = (from a in DB.Wastage_Creation
                     where ((a.Wastage_Rls_Date.Value >= startDate.Value) && (a.Wastage_Rls_Date.Value <= endDate.Value)) && a.is_Deleted == false && a.Wastage_Type == wastageType && a.Wastage_Approval_Flag == null && a.DC_Code == ULocation && a.Reject_Reason == null
                     select new WastageEntity
                      {
                          Wastage_Id = a.Wastage_Id,
                          Wastage_Number = a.Wastage_Number,
                          DC_Id = a.DC_Id,
                          DC_Code = a.DC_Code,
                          DC_Name = a.DC_Name,
                          Ref_Id = a.Ref_Id,
                          Ref_Number = a.Ref_Number,
                          Wastage_Type = a.Wastage_Type,
                          Wastage_raisedBy = a.Wastage_raisedBy,
                          Wastage_Rls_Date = a.Wastage_Rls_Date,
                          Remark = a.Remark,
                          Reject_Reason = a.Reject_Reason,
                          Wastage_approved_user_id = a.Wastage_approved_user_id,
                          Wastage_approved_by = a.Wastage_approved_by,
                          Wastage_approved_date = a.Wastage_approved_date,
                          CreatedBy = a.CreatedBy,
                          UpdatedBy = a.UpdatedBy,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.Wastage_Line_item
                                      where p.Wastage_Id == a.Wastage_Id
                                      select new
                                      {
                                          Wastage_Id = p.Wastage_Id
                                      }).Count()
                      }).ToList();
              return qu;
        }


        public List<WastageEntity> GetWSApprovalOR(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType, string ULocation, string Url)
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
            var qu = (from a in DB.Wastage_Creation
                      where ((a.Wastage_Rls_Date.Value >= startDate.Value) && (a.Wastage_Rls_Date.Value <= endDate.Value) || a.Wastage_Type == wastageType) && a.is_Deleted == false && a.Wastage_Approval_Flag == null && a.DC_Code == ULocation && a.Reject_Reason == null
                      select new WastageEntity
                      {
                          Wastage_Id = a.Wastage_Id,
                          Wastage_Number = a.Wastage_Number,
                          DC_Id = a.DC_Id,
                          DC_Code = a.DC_Code,
                          DC_Name = a.DC_Name,
                          Ref_Id = a.Ref_Id,
                          Ref_Number = a.Ref_Number,
                          Wastage_Type = a.Wastage_Type,
                          Wastage_raisedBy = a.Wastage_raisedBy,
                          Wastage_Rls_Date = a.Wastage_Rls_Date,
                          Remark = a.Remark,
                          Reject_Reason = a.Reject_Reason,
                          Wastage_approved_user_id = a.Wastage_approved_user_id,
                          Wastage_approved_by = a.Wastage_approved_by,
                          Wastage_approved_date = a.Wastage_approved_date,
                          CreatedBy = a.CreatedBy,
                          UpdatedBy = a.UpdatedBy,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.Wastage_Line_item
                                      where p.Wastage_Id == a.Wastage_Id
                                      select new
                                      {
                                          Wastage_Id = p.Wastage_Id
                                      }).Count()
                      }).ToList();
          
            return qu;
        }

     
    }
}
