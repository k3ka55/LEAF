using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Globalization;
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
    public class PurchaseOrderServices : IPurchaseOrderServices
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public PurchaseOrderServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        //--------------------------------CREATE----------------------------------------
        public string CreatePurchaseOrder(PurchaseOrderEntity purchaseEntity)
        {
            string prefix, locationId, poNumber;
            int? incNumber;

            using (var scopef = new TransactionScope())
            {
                string locationID = purchaseEntity.DC_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                prefix = rm.GetString("POT");
                PO_NUM_Generation autoIncNumber = GetAutoIncrement(locationID);
                locationId = autoIncNumber.DC_Code;
                incNumber = autoIncNumber.PO_Last_Number;
                int? incrementedValue = incNumber + 1;
                var POincrement = DB.PO_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                POincrement.PO_Last_Number = incrementedValue;
                _unitOfWork.AutoIncrementRepository.Update(POincrement);
                _unitOfWork.Save();

                poNumber = prefix + "/" + locationId + "/" + String.Format("{0:00000}", incNumber);
                scopef.Complete();
            }

            using (var scope = new TransactionScope())
            {
                var purchaseorder = new Purchase_Order
                {
                    PO_Number = poNumber,
                    DC_Code = purchaseEntity.DC_Code,
                    PO_RLS_date = purchaseEntity.PO_RLS_date,
                    Material_Source = purchaseEntity.Material_Source,
                    Material_Source_id = purchaseEntity.Material_Source_id,
                    Supplier_Id = purchaseEntity.Supplier_Id,
                    Supplier_Code = purchaseEntity.Supplier_Code,
                    Supplier_name = purchaseEntity.Supplier_name,
                    PO_Type = purchaseEntity.PO_Type,
                    SKU_Type_Id = purchaseEntity.SKU_Type_Id,
                    SKU_Type = purchaseEntity.SKU_Type,
                    PO_Requisitioned_by = purchaseEntity.PO_Requisitioned_by,
                    Payment_cycle = purchaseEntity.Payment_cycle,
                    Payment_type = purchaseEntity.Payment_type,
                    Delivery_Date = purchaseEntity.Delivery_Date,
                    PO_raise_by = purchaseEntity.PO_raise_by,
                    PO_Approve_by = purchaseEntity.PO_Approve_by,
                    CreatedDate = DateTime.Now,
                    CreatedBy = purchaseEntity.CreatedBy,
                    Status = "Open",
                    PO_Approval_Flag = null,
                    is_Deleted = false,
                    is_Syunc = false,
                };

                _unitOfWork.PurchaseOrderRepository.Insert(purchaseorder);
                _unitOfWork.Save();

                int? poId = purchaseorder.Po_id;

                var model = new Purchase_Order_Line_item();
                foreach (PurchaseSubEntity pSub in purchaseEntity.PurchaseDetails)
                {
                    model.PO_Number = poNumber;
                    model.Po_id = poId;
                    model.SKU_ID = pSub.SKU_ID;
                    model.SKU_Code = pSub.SKU_Code;
                    model.SKU_Name = pSub.SKU_Name;
                    model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                    model.SKU_SubType = pSub.SKU_SubType;
                    model.UOM = pSub.UOM;
                    model.A_Grade_Qty = pSub.A_Grade_Qty;
                    model.B_Grade_Qty = pSub.B_Grade_Qty;
                    model.Qty = pSub.Qty;
                    model.A_Grade_Price = pSub.A_Grade_Price;
                    model.B_Grade_Price = pSub.B_Grade_Price;
                    model.Total_Qty = pSub.Qty * pSub.A_Grade_Price;
                    //this is not toatal qty it is povalue
                    model.Remark = pSub.Remark;
                    model.Status = "Open";
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = pSub.CreatedBy;

                    _unitOfWork.PurchaseSubRepository.Insert(model);
                    _unitOfWork.Save();

                }
                scope.Complete();
                return purchaseorder.PO_Number;
            }
        }
        //-----------------------------------------SEARCH-------------------------------------------

          public List<PurchaseOrderEntity> GetPOApprovalAND(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string ULocation, string Url)
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

            var qu = (from a in DB.Purchase_Order
                      where ((a.Delivery_Date.Value >= startDate.Value) && (a.Delivery_Date.Value <= endDate.Value)) && a.is_Deleted == false && a.Supplier_name == supplierName && a.PO_Approval_Flag == null && a.DC_Code == ULocation && a.Reason == null && a.Status == "Open"
                      select new PurchaseOrderEntity
                      {
                          Po_id = a.Po_id,
                          PO_Number = a.PO_Number,
                          DC_Code = a.DC_Code,
                          PO_RLS_date = a.PO_RLS_date,
                          Material_Source = a.Material_Source,
                          Material_Source_id = a.Material_Source_id,
                          Supplier_Id = a.Supplier_Id,
                          Supplier_Code = a.Supplier_Code,
                          Supplier_name = a.Supplier_name,
                          PO_Type = a.PO_Type,
                          SKU_Type_Id = a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          PO_Requisitioned_by = a.PO_Requisitioned_by,
                          Payment_cycle = a.Payment_cycle,
                          Payment_type = a.Payment_type,
                          Delivery_Date = a.Delivery_Date,
                          PO_raise_by = a.PO_raise_by,
                          PO_Approve_by = a.PO_Approve_by,
                          Status = a.Status,
                          Reason = a.Reason,
                          PO_Approval_Flag = a.PO_Approval_Flag,
                          PO_Approved_date = a.PO_Approved_date,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.Purchase_Order_Line_item
                                      where p.Po_id == a.Po_id && (p.Status == "Open" || p.Status == "Partial")
                                      select new
                                      {
                                          PO_id = p.Po_id
                                      }).Count()
                      }).ToList();
          
            return qu;
        }


        public List<PurchaseOrderEntity> GetPOApprovalOR(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string ULocation, string Url)
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

            var qu = (from a in DB.Purchase_Order
                      where ((a.Delivery_Date.Value >= startDate.Value) && (a.Delivery_Date.Value <= endDate.Value) || a.Supplier_name == supplierName) && a.is_Deleted == false && a.PO_Approval_Flag == null && a.DC_Code == ULocation && a.Reason == null && a.Status == "Open"
                     select new PurchaseOrderEntity
                      {
                          Po_id = a.Po_id,
                          PO_Number = a.PO_Number,
                          DC_Code = a.DC_Code,
                          PO_RLS_date = a.PO_RLS_date,
                          Material_Source = a.Material_Source,
                          Material_Source_id = a.Material_Source_id,
                          Supplier_Id = a.Supplier_Id,
                          Supplier_Code = a.Supplier_Code,
                          Supplier_name = a.Supplier_name,
                          PO_Type = a.PO_Type,
                          SKU_Type_Id = a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          PO_Requisitioned_by = a.PO_Requisitioned_by,
                          Payment_cycle = a.Payment_cycle,
                          Payment_type = a.Payment_type,
                          Delivery_Date = a.Delivery_Date,
                          PO_raise_by = a.PO_raise_by,
                          PO_Approve_by = a.PO_Approve_by,
                          Status = a.Status,
                          Reason = a.Reason,
                          PO_Approval_Flag = a.PO_Approval_Flag,
                          PO_Approved_date = a.PO_Approved_date,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.Purchase_Order_Line_item
                                      where p.Po_id == a.Po_id && (p.Status == "Open" || p.Status == "Partial")
                                      select new
                                      {
                                          PO_id = p.Po_id
                                      }).Count()
                      }).ToList();
            return qu;
        }

       public List<PurchaseOrderEntity> SearchPO(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string Ulocation, string Url)
        {

            List<PurchaseOrderEntity> POSearchLIST = new List<PurchaseOrderEntity>();

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

            var POLIST = (from a in DB.Purchase_Order
                          where a.is_Deleted == false && a.PO_Approval_Flag == true && a.DC_Code == Ulocation
                          select new PurchaseOrderEntity
                          {
                              Po_id = a.Po_id,
                              PO_Number = a.PO_Number,
                              DC_Code = a.DC_Code,
                              PO_RLS_date = a.PO_RLS_date,
                              Material_Source = a.Material_Source,
                              Material_Source_id = a.Material_Source_id,
                              Supplier_Id = a.Supplier_Id,
                              Supplier_Code = a.Supplier_Code,
                              Supplier_name = a.Supplier_name,
                              PO_Type = a.PO_Type,
                              SKU_Type_Id = a.SKU_Type_Id,
                              SKU_Type = a.SKU_Type,
                              PO_Requisitioned_by = a.PO_Requisitioned_by,
                              Payment_cycle = a.Payment_cycle,
                              Payment_type = a.Payment_type,
                              Delivery_Date = a.Delivery_Date,
                              PO_raise_by = a.PO_raise_by,
                              PO_Approve_by = a.PO_Approve_by,
                              Status = a.Status,
                              PO_Approval_Flag = a.PO_Approval_Flag,
                              PO_Approved_date = a.PO_Approved_date,
                              is_Create = iCrt,
                              is_Delete = isDel,
                              is_Edit = isEdt,
                              is_Approval = isApp,
                              is_View = isViw,
                              Counting = (from p in DB.Purchase_Order_Line_item
                                          where p.Po_id == a.Po_id && (p.Status == "Open" || p.Status == "Partial")
                                          select new
                                          {
                                              PO_id = p.Po_id
                                          }).Count(),

                              PurchaseDetails = (from m in DB.Purchase_Order_Line_item
                                                 where m.Po_id == a.Po_id
                                                 orderby m.SKU_Name
                                                 select new PurchaseSubEntity
                                                 {
                                                     PO_Line_Id = m.PO_Line_Id,
                                                     PO_Number = m.PO_Number,
                                                     SKU_ID = m.SKU_ID,
                                                     SKU_Code = m.SKU_Code,
                                                     SKU_Name = m.SKU_Name,
                                                     SKU_SubType_Id = m.SKU_SubType_Id,
                                                     SKU_SubType = m.SKU_SubType,
                                                     UOM = m.UOM,
                                                     Qty = m.Qty,
                                                     A_Grade_Qty = m.A_Grade_Qty,
                                                     B_Grade_Qty = m.B_Grade_Qty,
                                                     A_Grade_Price = m.A_Grade_Price,
                                                     B_Grade_Price = m.B_Grade_Price,
                                                     Total_Qty = m.Total_Qty.Value,
                                                     Remark = m.Remark,
                                                     Supplier_name = a.Supplier_name,
                                                     Delivery_Date = a.Delivery_Date,
                                                     DC_Code = a.DC_Code,
                                                     Status = a.Status,
                                                     SKU_Type = a.SKU_Type,
                                                     CreatedDate = a.CreatedDate,
                                                     CreatedBy = a.CreatedBy,
                                                     UpdatedBy = a.UpdatedBy,
                                                     UpdatedDate = a.UpdatedDate,
                                                     PO_RLS_date = a.PO_RLS_date,
                                                     Material_Source = a.Material_Source,
                                                     PO_Requisitioned_by = a.PO_Requisitioned_by,
                                                     PO_Type = a.PO_Type,
                                                     Payment_cycle = a.Payment_cycle,
                                                     Payment_type = a.Payment_type,
                                                     PO_raise_by = a.PO_raise_by,
                                                     PO_Approved_date = a.PO_Approved_date,
                                                     PO_Approve_by = a.PO_Approve_by
                                                 }).ToList(),
                          });


            if (startDate.Value != null && endDate.Value != null)
            {
                POLIST = POLIST.Where(a => a.Delivery_Date >= startDate && a.Delivery_Date <= endDate);
            }
            if (supplierName != "null")
            {
                POLIST = POLIST.Where(a => a.Supplier_name == supplierName);
            }
            if (status != "null")
            {
                POLIST = POLIST.Where(a => a.Status == status);
            }

            foreach (var po in POLIST)
            {
                POSearchLIST.Add(po);
            }
         
            return POSearchLIST;
        }
        public List<POWithLineItemEntity> GetPOforEditALL(int? roleId, string ULocation, string Url, string poNumber)
        {
            DateTime yesterDay = DateTime.UtcNow.Date.AddDays(-1);
            DateTime now = DateTime.UtcNow;
            var qu = (from a in DB.Purchase_Order
                      join rma in DB.Role_Menu_Access on roleId equals rma.Role_Id
                      join pol in DB.Purchase_Order_Line_item on a.Po_id equals pol.Po_id
                      join mm in DB.Menu_Master on rma.Menu_Id equals mm.Menu_Id
                      join grn in DB.GRN_Creation on a.PO_Number equals grn.PO_Number
                      join grnL in DB.GRN_Line_item on grn.GRN_Number equals grnL.GRN_Number
                      where grn.GRN_Rls_Date >= yesterDay && grn.GRN_Rls_Date <= now && a.PO_Number == poNumber && a.is_Deleted == false && a.PO_Approval_Flag == true && a.DC_Code == ULocation && a.Status == "Closed"
                      where ((rma.Role_Id == roleId) && mm.Url == Url)

                      select new POWithLineItemEntity
                      {
                          Po_id = a.Po_id,
                          PO_Number = a.PO_Number,
                          DC_Code = a.DC_Code,
                          PO_RLS_date = a.PO_RLS_date,
                          Delivery_Date = a.Delivery_Date,
                          PO_Line_Id = pol.PO_Line_Id,
                          Supplier_Id = a.Supplier_Id,
                          Supplier_Code = a.Supplier_Code,
                          Supplier_name = a.Supplier_name,
                          SKU_Type_Id = a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          UOM = pol.UOM,
                          Status = a.Status,
                          SKU_ID = pol.SKU_ID,
                          SKU_Code = pol.SKU_Code,
                          SKU_Name = pol.SKU_Name,
                          SKU_SubType_Id = pol.SKU_SubType_Id,
                          SKU_SubType = pol.SKU_SubType,
                          A_Grade_Qty = pol.A_Grade_Qty,
                          B_Grade_Qty = pol.B_Grade_Qty,
                          A_Grade_Price = pol.A_Grade_Price,
                          B_Grade_Price = pol.B_Grade_Price,
                          Qty = pol.Qty,
                          Total_Qty = pol.Total_Qty,
                          Menu_Id = mm.Menu_Id,
                          Menu_Name = mm.Menu_Name,
                          Counting = (from p in DB.Purchase_Order_Line_item
                                      where p.Po_id == a.Po_id && (p.Status != "Open")
                                      select new
                                      {
                                          PO_id = p.Po_id
                                      }).Count()
                      }).Distinct().ToList();
            return qu;
        }
      
        
        public List<POWithLineItemEntity> GetPOforEditOR(int? roleId, string ULocation, string Url, string poNumber)
        {
            DateTime yesterDay = DateTime.UtcNow.Date.AddDays(-1);
            DateTime now = DateTime.UtcNow;

            var qu = (from a in DB.Purchase_Order
                      join rma in DB.Role_Menu_Access on roleId equals rma.Role_Id
                      join mm in DB.Menu_Master on rma.Menu_Id equals mm.Menu_Id
                      join pol in DB.Purchase_Order_Line_item on a.Po_id equals pol.Po_id
                      join grn in DB.GRN_Creation on a.PO_Number equals grn.PO_Number
                      join grnL in DB.GRN_Line_item on grn.GRN_Number equals grnL.GRN_Number
                      where grn.GRN_Rls_Date >= yesterDay && grn.GRN_Rls_Date <= now && a.is_Deleted == false && a.PO_Approval_Flag == true && a.DC_Code == ULocation && a.Status == "Closed"
                      where rma.Role_Id == roleId && mm.Url == Url
                      select new POWithLineItemEntity
                      {
                          Po_id = a.Po_id,
                          PO_Number = a.PO_Number,
                          DC_Code = a.DC_Code,
                          PO_RLS_date = a.PO_RLS_date,
                          Delivery_Date = a.Delivery_Date,
                          PO_Line_Id = pol.PO_Line_Id,
                          Supplier_Id = a.Supplier_Id,
                          Supplier_Code = a.Supplier_Code,
                          Supplier_name = a.Supplier_name,
                          SKU_Type_Id = a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          SKU_ID = pol.SKU_ID,
                          Status = a.Status,
                          UOM = pol.UOM,
                          SKU_Code = pol.SKU_Code,
                          SKU_Name = pol.SKU_Name,
                          SKU_SubType_Id = pol.SKU_SubType_Id,
                          SKU_SubType = pol.SKU_SubType,
                          A_Grade_Qty = pol.A_Grade_Qty,
                          B_Grade_Qty = pol.B_Grade_Qty,
                          A_Grade_Price = pol.A_Grade_Price,
                          B_Grade_Price = pol.B_Grade_Price,
                          Qty = pol.Qty,
                          Total_Qty = pol.Total_Qty,
                          Counting = (from p in DB.Purchase_Order_Line_item
                                      where p.Po_id == a.Po_id && (p.Status != "Open")
                                      select new
                                      {
                                          PO_id = p.Po_id
                                      }).Count()
                      }).Distinct().ToList();
            return qu;
        }

        //------------------------------------------CREATE----------------------------------------
        public PO_NUM_Generation GetAutoIncrement(string locationId)
        {
            var autoinc = DB.PO_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new PO_NUM_Generation
            {
                PO_Num_Gen_Id = autoinc.PO_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                PO_Last_Number = autoinc.PO_Last_Number
            };

            return model;
        }

        //---------------------------------------------GET----------------------------------------
        public List<PurchaseOrderEntity> GetPurchaseLineItemforPO(string id)
        {
           
            var list = (from x in DB.Purchase_Order
                        where x.PO_Number == id
                        select  new PurchaseOrderEntity
                        {
                            Po_id = x.Po_id,
                            PO_Number = x.PO_Number,
                            DC_Code = x.DC_Code,
                            PO_RLS_date = x.PO_RLS_date,
                            Material_Source = x.Material_Source,
                            Material_Source_id = x.Material_Source_id,
                            Supplier_Id = x.Supplier_Id,
                            Supplier_Code = x.Supplier_Code,
                            Supplier_name = x.Supplier_name,
                            PO_Requisitioned_by = x.PO_Requisitioned_by,
                            PO_Type = x.PO_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Payment_cycle = x.Payment_cycle,
                            Payment_type = x.Payment_type,
                            PO_raise_by = x.PO_raise_by,
                            PO_Approve_by = x.PO_Approve_by,
                            PO_Approved_date = x.PO_Approved_date,
                            Delivery_Date = x.Delivery_Date,
                            Status = x.Status,
                            PurchaseDetails = (from a in DB.Purchase_Order_Line_item
                                               where a.Po_id == x.Po_id
                                               select new PurchaseSubEntity
                                               {
                                                   PO_Line_Id = a.PO_Line_Id,
                                                   PO_Number = a.PO_Number,
                                                   SKU_ID = a.SKU_ID,
                                                   SKU_Code = a.SKU_Code,
                                                   SKU_Name = a.SKU_Name,
                                                   SKU_SubType_Id = a.SKU_SubType_Id,
                                                   SKU_SubType = a.SKU_SubType,
                                                   UOM = a.UOM,
                                                   Qty = a.Qty,
                                                   A_Grade_Qty = a.A_Grade_Qty,
                                                   B_Grade_Qty = a.B_Grade_Qty,
                                                   A_Grade_Price = a.A_Grade_Price,
                                                   B_Grade_Price = a.B_Grade_Price,
                                                   Total_Qty = a.Total_Qty,
                                                   Remark = a.Remark,
                                                   Status = a.Status
                                               }).ToList(),
                            PO_Qty_Sum = (from p in DB.Purchase_Order_Line_item
                                          where p.PO_Number == id
                                          group p by p.PO_Number into g
                                          select new PO_Qty_SumEntity
                                          {
                                              Total_Qty_Sum = g.Sum(z => z.Total_Qty)

                                          })
                        }).ToList();

            return list;
        }

        public List<PurchaseOrderEntity> GetPurchaseLineItemforGRN(string id)
        {

            var list = (from x in DB.Purchase_Order
                        where x.PO_Number == id
                        select new PurchaseOrderEntity
                        {
                            Po_id = x.Po_id,
                            PO_Number = x.PO_Number,
                            DC_Code = x.DC_Code,
                            PO_RLS_date = x.PO_RLS_date,
                            Material_Source = x.Material_Source,
                            Material_Source_id = x.Material_Source_id,
                            Supplier_Id = x.Supplier_Id,
                            Supplier_Code = x.Supplier_Code,
                            Supplier_name = x.Supplier_name,
                            PO_Requisitioned_by = x.PO_Requisitioned_by,
                            PO_Type = x.PO_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Payment_cycle = x.Payment_cycle,
                            Payment_type = x.Payment_type,
                            PO_raise_by = x.PO_raise_by,
                            PO_Approve_by = x.PO_Approve_by,
                            PO_Approved_date = x.PO_Approved_date,
                            Delivery_Date = x.Delivery_Date,
                            Status = x.Status,
                            PurchaseDetails = (from a in DB.Purchase_Order_Line_item
                                               where a.Po_id == x.Po_id && (a.Status == "Open" || a.Status == "Partial")
                                               orderby a.SKU_Name
                                               select new PurchaseSubEntity
                                               {
                                                   PO_Line_Id = a.PO_Line_Id,
                                                   PO_Number = a.PO_Number,
                                                   SKU_ID = a.SKU_ID,
                                                   SKU_Code = a.SKU_Code,
                                                   SKU_Name = a.SKU_Name,
                                                   SKU_SubType_Id = a.SKU_SubType_Id,
                                                   SKU_SubType = a.SKU_SubType,
                                                   UOM = a.UOM,
                                                   Qty = a.Qty,
                                                   A_Grade_Qty = a.A_Grade_Qty,
                                                   B_Grade_Qty = a.B_Grade_Qty,
                                                   A_Grade_Price = a.A_Grade_Price,
                                                   B_Grade_Price = a.B_Grade_Price,
                                                   Total_Qty = a.Total_Qty,
                                                   Remark = a.Remark,
                                                   Status = a.Status
                                               }).ToList(),
                            PO_Qty_Sum = (from p in DB.Purchase_Order_Line_item
                                          where p.PO_Number == id
                                          group p by p.PO_Number into g
                                          select new PO_Qty_SumEntity
                                          {
                                              Total_Qty_Sum = g.Sum(z => z.Total_Qty)

                                          })
                        }).ToList();

            return list;
        }

        public List<PurchaseOrderEntity> GetPOApprovalList(string Ulocation)
        {
            var list = (from x in DB.Purchase_Order
                        where x.is_Deleted != true && x.PO_Approval_Flag == null && x.DC_Code == Ulocation
                        select new PurchaseOrderEntity
                        {
                            Po_id = x.Po_id,
                            PO_Number = x.PO_Number,
                            DC_Code = x.DC_Code,
                            PO_RLS_date = x.PO_RLS_date,
                            Material_Source = x.Material_Source,
                            Material_Source_id = x.Material_Source_id,
                            Supplier_Id = x.Supplier_Id,
                            Supplier_Code = x.Supplier_Code,
                            Supplier_name = x.Supplier_name,
                            PO_Type = x.PO_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            PO_Requisitioned_by = x.PO_Requisitioned_by,
                            Payment_cycle = x.Payment_cycle,
                            Payment_type = x.Payment_type,
                            Delivery_Date = x.Delivery_Date,
                            PO_raise_by = x.PO_raise_by,
                            PO_Approve_by = x.PO_Approve_by,
                            Status = x.Status,
                            Reason = x.Reason,
                            PO_Approval_Flag = x.PO_Approval_Flag,
                            PO_Approved_date = x.PO_Approved_date,
                        }).ToList();

            return list;
        }

        public List<Tuple<string>> getStatuses()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.Status", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("OpenStatus")));
            list.Add(new Tuple<string>(rm.GetString("CloseStatus")));
            return list;
        }

        public bool poApproval(PurchaseOrderEntity poEntity)
        {
            var success = false;
            if (poEntity.Po_id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var pOrderEntity = _unitOfWork.PurchaseOrderRepository.GetByID(poEntity.Po_id);
                    if (pOrderEntity != null)
                    {
                        pOrderEntity.Reason = poEntity.Reason;
                        pOrderEntity.PO_Approval_Flag = poEntity.PO_Approval_Flag;
                        pOrderEntity.PO_Approved_date = DateTime.Now;
                        pOrderEntity.PO_Approve_by = poEntity.PO_Approve_by;

                        _unitOfWork.PurchaseOrderRepository.Update(pOrderEntity);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        //------------------POBulk Approval-------------------------------

        public bool poBulkApproval(bulkApprovalEntity bulkEntity)
        {
            var success = false;
            foreach (bulkIdsEntity id in bulkEntity.bulkid)
            {
                if (id.poId > 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        var pOrderEntity = _unitOfWork.PurchaseOrderRepository.GetByID(id.poId);
                        if (pOrderEntity != null)
                        {
                            pOrderEntity.Reason = id.Reason;
                            pOrderEntity.PO_Approval_Flag = id.PO_Approval_Flag;
                            pOrderEntity.PO_Approved_date = DateTime.Now;
                            pOrderEntity.PO_Approve_by = id.PO_Approve_by;

                            _unitOfWork.PurchaseOrderRepository.Update(pOrderEntity);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        //--------------------------UpdatePurchaseOrder(int poId, PurchaseOrderEntity POEntity)

        public string UpdatePurchaseOrder(int poId, BusinessEntities.PurchaseOrderEntity POEntity)
        {
            string poNumber = "";

            if (POEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.PurchaseOrderRepository.GetByID(poId);
                    
                    if (p != null)
                    {
                        poNumber = POEntity.PO_Number;
                        p.PO_Number = POEntity.PO_Number;
                        p.DC_Code = POEntity.DC_Code;
                        p.PO_RLS_date = POEntity.PO_RLS_date;
                        p.Material_Source_id = POEntity.Material_Source_id;
                        p.Material_Source = POEntity.Material_Source;
                        p.Supplier_Id = POEntity.Supplier_Id;
                        p.Supplier_Code = POEntity.Supplier_Code;
                        p.Supplier_name = POEntity.Supplier_name;
                        p.PO_Requisitioned_by = POEntity.PO_Requisitioned_by;
                        p.PO_Type = POEntity.PO_Type;
                        p.SKU_Type_Id = POEntity.SKU_Type_Id;
                        p.SKU_Type = POEntity.SKU_Type;
                        p.Payment_cycle = POEntity.Payment_cycle;
                        p.Payment_type = POEntity.Payment_type;
                        p.Delivery_Date = POEntity.Delivery_Date;
                        p.UpdatedDate = DateTime.Now;
                        p.UpdatedBy = POEntity.UpdatedBy;
                        p.is_Syunc = false;
                        _unitOfWork.PurchaseOrderRepository.Update(p);
                        _unitOfWork.Save();
                    }

                    var lineItemList = DB.Purchase_Order_Line_item.Where(x => x.Po_id == poId).ToList();
                    
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var list = _unitOfWork.PurchaseSubRepository.GetByID(li.PO_Line_Id);

                            if (list != null)
                            {
                                _unitOfWork.PurchaseSubRepository.Delete(list);
                                _unitOfWork.Save();
                            }

                            scope1.Complete();
                        }
                    }


                    foreach (PurchaseSubEntity pSub in POEntity.PurchaseDetails)
                    {
                        // var line = _unitOfWork.PurchaseSubRepository.GetByID(pSub.PO_Line_Id);
                        var model = new Purchase_Order_Line_item();

                        //if (line != null)
                        //{
                        //    line.SKU_ID = pSub.SKU_ID;
                        //    line.SKU_Code = pSub.SKU_Code;
                        //    line.SKU_Name = pSub.SKU_Name;
                        //    line.SKU_SubType_Id=pSub.SKU_SubType_Id;
                        //    line.SKU_SubType = pSub.SKU_SubType;
                        //   // line.SKU_Type = pSub.SKU_Type;
                        //    line.UOM = pSub.UOM;
                        //    line.Qty = pSub.Qty;
                        //    line.A_Grade_Qty = pSub.A_Grade_Qty;
                        //    line.B_Grade_Qty = pSub.B_Grade_Qty;
                        //    line.A_Grade_Price = pSub.A_Grade_Price;
                        //    line.B_Grade_Price = pSub.B_Grade_Price;
                        //   //line.Total_Qty = pSub.Total_Qty;
                        //    line.Remark = pSub.Remark;
                        //    line.Status = "Open";
                        //    line.UpdatedDate = DateTime.Now;
                        //    line.UpdatedBy = pSub.UpdatedBy;
                        //    line.Total_Qty = pSub.Qty * pSub.A_Grade_Price;
                        //    //this is not toatal qty it is povalue
                        //    _unitOfWork.PurchaseSubRepository.Update(line);
                        //    _unitOfWork.Save();
                        //}
                        //else
                        //{
                        model.PO_Number = poNumber;
                        model.Po_id = poId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.UOM = pSub.UOM;
                        model.Qty = pSub.Qty;
                        model.A_Grade_Qty = pSub.A_Grade_Qty;
                        model.B_Grade_Qty = pSub.B_Grade_Qty;
                        model.A_Grade_Price = pSub.A_Grade_Price;
                        model.B_Grade_Price = pSub.B_Grade_Price;
                        model.Total_Qty = pSub.Qty * pSub.A_Grade_Price;
                        model.Remark = pSub.Remark;
                        model.Status = "Open";
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = pSub.CreatedBy;

                        _unitOfWork.PurchaseSubRepository.Insert(model);
                        _unitOfWork.Save();
                        //}
                    }
                    scope.Complete();
                }
            }
            return poNumber;
        }

        //------------------------DELETEPURCHASEORDER----------

        public bool DeletePurchaseOrder(int poId, string deleteReason)
        {
            var success = false;
            if (poId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.PurchaseOrderRepository.GetByID(poId);
                    if (p != null)
                    {
                        p.is_Deleted = true;
                        p.is_Syunc = false;
                        p.UpdatedDate = DateTime.Now;
                        p.Delete_Reason = deleteReason;
                        _unitOfWork.PurchaseOrderRepository.Update(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        //---------------------------DELETEPURCHASEORDERLINEITEM----------------------------


        public bool UpdateLineItemPORate(int Id, string poNumber, double? ARate, double? BRate, double? CRate)
        {
            var success = false;
            using (var scopes = new TransactionScope())
            {
                //var s = DB.Purchase_Order.Where(x => x.PO_Number == poNumber).Select(x => x.is_Syunc).FirstOrDefault();
                var s = DB.Purchase_Order.Single(x => x.PO_Number == poNumber);//.Select(x => x.is_Syunc).FirstOrDefault();
                s.is_Syunc = false;

                DB.Entry(s).State = EntityState.Modified;
                DB.SaveChanges();
                scopes.Complete();
            }
            using (var scope = new TransactionScope())
            {
                var a = (from PO in DB.Purchase_Order
                         join pol in DB.Purchase_Order_Line_item on PO.PO_Number equals pol.PO_Number
                         where PO.PO_Number == poNumber
                         orderby pol.SKU_Name
                         select new PO_Rate_Change_Audit
                         {
                             Po_id = PO.Po_id,
                             PO_Number = PO.PO_Number,
                             DC_Code = PO.DC_Code,
                             PO_Type = PO.PO_Type,
                             PO_Line_Id = pol.PO_Line_Id,
                             PO_RLS_date = PO.PO_RLS_date,
                             Supplier_Id = PO.Supplier_Id,
                             Supplier_Code = PO.Supplier_Code,
                             Supplier_Name = PO.Supplier_name,
                             SKU_Type_Id = PO.SKU_Type_Id,
                             SKU_Type = PO.SKU_Type,
                             SKU_ID = pol.SKU_ID,
                             SKU_SubType_Id = pol.SKU_SubType_Id,
                             SKU_SubType = pol.SKU_SubType,
                             UOM = pol.UOM,
                             SKU_Code = pol.SKU_Code,
                             SKU_Name = pol.SKU_Name,
                             A_Grade_Qty = pol.A_Grade_Qty,
                             B_Grade_Qty = pol.B_Grade_Qty,
                             A_Grade_Price = pol.A_Grade_Price,
                             B_Grade_Price = pol.B_Grade_Price,
                             Qty = pol.Qty,
                             CreatedDate = DateTime.Now,
                             CreateBy = PO.CreatedBy
                         });

                if (Id > 0)
                {
                    var p = _unitOfWork.PurchaseSubRepository.GetByID(Id);
                    if (p != null)
                    {
                        p.A_Grade_Price = ARate;
                        p.B_Grade_Price = BRate;
                        _unitOfWork.PurchaseSubRepository.Update(p);
                        _unitOfWork.Save();
                    }
                }
                var e = (from grn in DB.GRN_Creation
                         join grnL in DB.GRN_Line_item on grn.GRN_Number equals grnL.GRN_Number
                         join poL in DB.Purchase_Order_Line_item on poNumber equals poL.PO_Number
                         where grn.PO_Number == poNumber && poL.PO_Line_Id == Id && poL.SKU_ID == grnL.SKU_ID
                         select new { grn, grnL }).FirstOrDefault();
                e.grnL.A_Accepted_Price = ARate;
                e.grnL.B_Accepted_Price = BRate;
                e.grnL.C_Accepted_Price = CRate;
                             
                DB.SaveChanges();
                scope.Complete();
                success = true;
            }
            return success;
        }

        public bool DeletePurchaseOrderLineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.PurchaseSubRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.PurchaseSubRepository.Delete(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

    }
}
