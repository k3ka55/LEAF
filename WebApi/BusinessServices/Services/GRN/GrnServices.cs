using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace BusinessServices
{
    public class GrnServices : IGrnServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public GrnServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<cdnNumber> GetCdnNumbersforGRN(string Ulocation, string CustomerCode)
        //
        {
            DateTime yesterDay = DateTime.UtcNow.Date.AddDays(-1);
            DateTime now = DateTime.UtcNow;
            var query = (from x in DB.Dispatch_Creation
                         where x.Indent_Rls_Date >= yesterDay && x.Indent_Rls_Date <= now && x.is_Deleted == false && x.Dispatch_Location_Code == Ulocation && x.Invoice_Flag == null && x.Customer_code == CustomerCode
                         //&& x.Customer_code==CustomerCode
                         select new cdnNumber
                         {
                             CDN_Number = x.Customer_Dispatch_Number
                         }).ToList();
            return query;
        }

        public bool POUpdateById(POBULKAPPROVAL grnEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {

                foreach (approvalUpdateList lst in grnEntity.Line_Ids)
                {
                    Purchase_Order_Line_item lineItem = _unitOfWork.PurchaseSubRepository.GetByID(lst.po_LineId);

                    if (lineItem != null)
                    {
                        if (lst.statusflag == 0)
                        {
                            lineItem.Status = "Closed";
                            _unitOfWork.PurchaseSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 1)
                        {
                            lineItem.Status = "Partial";
                            _unitOfWork.PurchaseSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 2)
                        {
                            lineItem.Status = "Exceed";
                            _unitOfWork.PurchaseSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }

                    }
                }

                Purchase_Order po = DB.Purchase_Order.Where(x => x.PO_Number == grnEntity.id).FirstOrDefault();

                if (po != null)
                {
                    po.Status = "Closed";
                    po.is_Syunc = false;
                    DB.Entry(po).State = EntityState.Modified;
                    DB.SaveChanges();
                }

                scope.Complete();
                success = true;
            }
            return success;
        }
        //--------------------------UpdateGRN-----------------------

        public string UpdateGRN(int grnId, BusinessEntities.GrnEntity grnEntity)
        {
            string grnNumber = "";
            string wsNumber, prefix, locationId;
            int? inNumber;
            double? C_Qty = 0, B_Qty = 0, A_Qty = 0;
            if (grnEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var g = _unitOfWork.GrnRepository.GetByID(grnId);
                    if (g != null)
                    {
                        grnNumber = grnEntity.GRN_Number;
                        g.GRN_Number = grnEntity.GRN_Number;
                        g.PO_Number = grnEntity.PO_Number;
                        g.CDN_Number = grnEntity.CDN_Number;
                        g.GRN_Rls_Date = grnEntity.GRN_Rls_Date;
                        g.Voucher_Type = grnEntity.Voucher_Type;
                        g.Vehicle_No = grnEntity.Vehicle_No;
                        g.Sales_Person_Id = grnEntity.Sales_Person_Id;
                        g.Sales_Person_Name = grnEntity.Sales_Person_Name;
                        g.Route_Id = grnEntity.Route_Id;
                        g.Route_Code = grnEntity.Route_Code;
                        g.Route = grnEntity.Route;
                        g.SKU_Type_Id = grnEntity.SKU_Type_Id;
                        g.SKU_Type = grnEntity.SKU_Type;
                        g.STN_DC_Id = grnEntity.STN_DC_Id;
                        g.STN_DC_Code = grnEntity.STN_DC_Code;
                        g.STN_DC_Name = grnEntity.STN_DC_Name;
                        g.Customer_Id = grnEntity.Customer_Id;
                        g.Customer_code = grnEntity.Customer_code;
                        g.Customer_Name = grnEntity.Customer_Name;
                        g.Supplier_Id = grnEntity.Supplier_Id;
                        g.Supplier_code = grnEntity.Supplier_code;
                        g.Supplier_Name = grnEntity.Supplier_Name;
                        g.DC_Code = grnEntity.DC_Code;
                        g.UpdatedDate = DateTime.Now;
                        g.UpdatedBy = grnEntity.UpdatedBy;


                        _unitOfWork.GrnRepository.Update(g);
                        _unitOfWork.Save();

                        DB.Grn_Detection_Service(grnId);
                        DB.SaveChanges();

                    }


                    DateTime Today = DateTime.Now;
                    var check = (from ee in DB.Stirnkage_Summary
                                 where ee.DC_Code == grnEntity.DC_Code
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


                    var lineItemList = DB.GRN_Line_item.Where(x => x.INW_id == grnId).ToList();
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            //var list = _unitOfWork.GrnSubRepository.GetByID(li.GRN_Line_Id);

                            //if (list != null)
                            //{
                            //    _unitOfWork.GrnSubRepository.Delete(list);
                            //    _unitOfWork.Save();
                            //}



                            var grnLineItems = DB.GRN_SKU_Line_Items.Where(f => f.GRN_Line_Id == li.GRN_Line_Id).ToList();
                            foreach (var litem in grnLineItems)
                            {
                                DB.GRN_SKU_Line_Items.Remove(litem);
                                DB.SaveChanges();
                            }
                            var grnLineItemConsumables = DB.GRN_Consumables.Where(f => f.GRN_Line_Id_FK == li.GRN_Line_Id).ToList();
                            foreach (var litemC in grnLineItemConsumables)
                            {
                                DB.GRN_Consumables.Remove(litemC);
                                DB.SaveChanges();
                            }

                            scope1.Complete();
                        }
                    }
                    foreach (GrnLineItemsEntity pSub in grnEntity.GrnDetails)
                    {
                        //  var line = _unitOfWork.GrnSubRepository.GetByID(pSub.GRN_Line_Id);
                        // var wantToDelete=lineItemList.Select(a=>a.GRN_Line_Id!=pSub.GRN_Line_Id)

                        //  if (line != null)
                        // {
                        var model = _unitOfWork.GrnSubRepository.GetByID(pSub.GRN_Line_Id);
                        model.GRN_Number = grnNumber;
                        model.INW_id = grnId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.UOM = pSub.UOM;
                        model.PO_QTY = pSub.PO_QTY;
                        model.Strinkage_Qty = pSub.Strinkage_Qty;
                        model.A_Accepted_Qty = pSub.A_Accepted_Qty;
                        A_Qty = pSub.A_Accepted_Qty;
                        model.B_Accepted_Qty = pSub.B_Accepted_Qty;
                        B_Qty = pSub.B_Accepted_Qty;
                        model.C_Accepted_Qty = pSub.C_Accepted_Qty;
                        C_Qty = pSub.C_Accepted_Qty;
                        model.A_Accepted_Price = pSub.A_Accepted_Price;
                        model.B_Accepted_Price = pSub.B_Accepted_Price;
                        model.C_Accepted_Price = pSub.C_Accepted_Price;
                        model.A_Converted_Qty = pSub.A_Converted_Qty;
                        model.B_Converted_Qty = pSub.B_Converted_Qty;
                        model.C_Converted_Qty = pSub.C_Converted_Qty;
                        model.Grade = pSub.Grade;
                        model.Billed_Qty = pSub.Billed_Qty;
                        model.Price_Book_Id = pSub.Price_Book_Id;
                        model.Price = pSub.Price;
                        model.Remark = pSub.Remark;
                        model.moved = false;
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = grnEntity.UpdatedBy;

                        _unitOfWork.GrnSubRepository.Update(model);
                        _unitOfWork.Save();
                        //  }


                        int grnLineId = pSub.GRN_Line_Id;

                        foreach (var skuSub in pSub.GRNSKULineItems)
                        {
                            var skuLSub = new GRN_SKU_Line_Items();
                            skuLSub.GRN_Line_Id = grnLineId;
                            skuLSub.SKU_Id = skuSub.SKU_Id;
                            skuLSub.A_Qty = skuSub.A_Qty;
                            skuLSub.B_Qty = skuSub.B_Qty;
                            skuLSub.C_Qty = skuSub.C_Qty;
                            skuLSub.Barcode = skuSub.Barcode;
                            skuLSub.Batch_Number = skuSub.Batch_Number;
                            skuLSub.UOM = skuSub.UOM;
                            skuLSub.CreatedDate = DateTime.Now;
                            skuLSub.CreatedBy = grnEntity.UpdatedBy;
                            skuLSub.is_Deleted = false;

                            _unitOfWork.GrnSKULineItemRepository.Insert(skuLSub);
                            _unitOfWork.Save();

                        }
                        if (pSub.GRNSKULineItemsConsumables != null)
                        {
                            foreach (var t in pSub.GRNSKULineItemsConsumables)
                            {
                                var skuCSub = new GRN_Consumables();
                                skuCSub.GRN_Line_Id_FK = grnLineId;
                                skuCSub.SKU_Id = t.SKU_Id.Value;
                                skuCSub.Grade = t.Grade;
                                skuCSub.Consumable_Qty = t.Consumable_Qty;
                                skuCSub.UOM = t.UOM;
                                skuCSub.CreatedDate = DateTime.Now;
                                skuCSub.CreatedBy = grnEntity.UpdatedBy;
                                skuCSub.is_Deleted = false;

                                _unitOfWork.GrnConsumablesRepository.Insert(skuCSub);
                                _unitOfWork.Save();
                            }
                        }

                        if (grnEntity.CDN_Number != null && grnEntity.Voucher_Type == "Customer Return")
                        {
                            //var grnToInsert = (from a in DB.GRN_Line_item
                            //                   where a.INW_id.Value == grnId
                            //                   select a).ToList();

                            //foreach(var grnLine in grnToInsert)
                            //{

                            if (C_Qty > 0)
                            {
                                using (var ascope = new TransactionScope())
                                {
                                    string locatioID = grnEntity.DC_Code;
                                    ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                                    prefix = rm.GetString("WST");
                                    Wastage_Auto_Num_Gen autoIncNumber = GetWastAutoIncrement(locatioID);
                                    locationId = autoIncNumber.DC_Code;
                                    inNumber = autoIncNumber.Wastage_Last_Number;
                                    int? incrementedValue = inNumber + 1;
                                    var WSincrement = DB.Wastage_Auto_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                                    WSincrement.Wastage_Last_Number = incrementedValue;
                                    _unitOfWork.WastageNumIncrementRepository.Update(WSincrement);
                                    _unitOfWork.Save();
                                    wsNumber = prefix + "/" + locationId + "/" + String.Format("{0:00000}", inNumber);
                                    ascope.Complete();
                                }
                                using (var bscope = new TransactionScope())
                                {
                                    var Wastage = new Wastage_Creation
                                    {
                                        Wastage_Number = wsNumber,
                                        DC_Code = grnEntity.DC_Code,
                                        Ref_Id = grnId,
                                        Ref_Number = grnEntity.CDN_Number,
                                        Wastage_Type = "Customer Return",
                                        Wastage_raisedBy = grnEntity.CreatedBy,
                                        Wastage_Rls_Date = grnEntity.GRN_Rls_Date,
                                        is_Deleted = false,
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = grnEntity.CreatedBy,
                                        Wastage_Approval_Flag = null,
                                        is_Syunc = false,
                                    };

                                    _unitOfWork.WastageRepository.Insert(Wastage);
                                    _unitOfWork.Save();

                                    int? wsId = Wastage.Wastage_Id;
                                    string wastageType = Wastage.Wastage_Type;

                                    var wmodel = new Wastage_Line_item();
                                    foreach (GrnLineItemsEntity paSub in grnEntity.GrnDetails)
                                    {
                                        wmodel.Wastage_Number = wsNumber;
                                        wmodel.Wastage_Id = wsId;
                                        wmodel.SKU_ID = paSub.SKU_ID;
                                        wmodel.SKU_Code = paSub.SKU_Code;
                                        wmodel.SKU_Name = paSub.SKU_Name;
                                        wmodel.UOM = paSub.UOM;
                                        wmodel.Grade = "C";
                                        wmodel.Ref_Id = grnId;
                                        wmodel.Ref_Line_Id = grnLineId;
                                        wmodel.Wastage_Qty = paSub.C_Accepted_Qty;
                                        wmodel.Reason = "Customer Return";
                                        wmodel.Stock_Reduce_Flag = false;

                                        _unitOfWork.WastageSubRepository.Insert(wmodel);
                                        _unitOfWork.Save();
                                    }
                                    bscope.Complete();
                                }
                            }

                            if (A_Qty != 0 || B_Qty != 0)
                            {
                                using (var cscope = new TransactionScope())
                                {

                                    var re = (from y in DB.Dispatch_Creation
                                              join l in DB.Dispatch_Line_item on y.Dispatch_Id equals l.Dispatch_Id
                                              where y.Customer_Dispatch_Number == grnEntity.CDN_Number
                                              select l).ToList();
                                    foreach (var yy in re)
                                    {
                                        foreach (GrnLineItemsEntity pbSub in grnEntity.GrnDetails)
                                        {
                                            using (var uscope = new TransactionScope())
                                            {
                                                if (pbSub.A_Accepted_Qty != 0 && pbSub.B_Accepted_Qty != 0)
                                                {
                                                    if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "A" && pbSub.A_Accepted_Qty != 0)
                                                    {
                                                        yy.Return_Qty = pbSub.A_Accepted_Qty;
                                                        _unitOfWork.DispatchSubRepository.Update(yy);
                                                        _unitOfWork.Save();
                                                    }
                                                    if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "B" && pbSub.B_Accepted_Qty != 0)
                                                    {
                                                        yy.Return_Qty = pbSub.B_Accepted_Qty;
                                                        _unitOfWork.DispatchSubRepository.Update(yy);
                                                        _unitOfWork.Save();
                                                    }
                                                }
                                                else if (pbSub.A_Accepted_Qty != 0 || pbSub.B_Accepted_Qty != 0)
                                                {
                                                    if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "A" && pbSub.A_Accepted_Qty != 0)
                                                    {
                                                        yy.Return_Qty = pbSub.A_Accepted_Qty;
                                                        _unitOfWork.DispatchSubRepository.Update(yy);
                                                        _unitOfWork.Save();
                                                    }
                                                    else if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "B" && pbSub.B_Accepted_Qty != 0)
                                                    {
                                                        yy.Return_Qty = pbSub.B_Accepted_Qty;
                                                        _unitOfWork.DispatchSubRepository.Update(yy);
                                                        _unitOfWork.Save();
                                                    }
                                                }
                                                uscope.Complete();
                                            }
                                        }
                                    }
                                    cscope.Complete();

                                }


                                using (var dscope = new TransactionScope())
                                {
                                    var inv = (from y in DB.Dispatch_Creation
                                               join t in DB.Invoice_Creation on y.Customer_Dispatch_Number equals t.Customer_Dispatch_Number
                                               join l in DB.Invoice_Line_item on t.invoice_Id equals l.Invoice_Id
                                               where y.Customer_Dispatch_Number == grnEntity.CDN_Number && t.Customer_Dispatch_Number == grnEntity.CDN_Number && y.Invoice_Flag == true
                                               select l).ToList();
                                    foreach (var zz in inv)
                                    {
                                        foreach (GrnLineItemsEntity pcSub in grnEntity.GrnDetails)
                                        {
                                            using (var uscope = new TransactionScope())
                                            {
                                                if (pcSub.A_Accepted_Qty != 0 && pcSub.B_Accepted_Qty != 0)
                                                {
                                                    if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "A" && pcSub.A_Accepted_Qty != 0)
                                                    {
                                                        zz.Return_Qty = pcSub.A_Accepted_Qty;
                                                        zz.Invoice_Qty = zz.Invoice_Qty - pcSub.A_Accepted_Qty;
                                                        zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                        _unitOfWork.InvoiceSubRepository.Update(zz);
                                                        _unitOfWork.Save();
                                                    }
                                                    if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "B" && pcSub.B_Accepted_Qty != 0)
                                                    {
                                                        zz.Return_Qty = pcSub.B_Accepted_Qty;
                                                        zz.Invoice_Qty = zz.Invoice_Qty - pcSub.B_Accepted_Qty;
                                                        zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                        _unitOfWork.InvoiceSubRepository.Update(zz);
                                                        _unitOfWork.Save();
                                                    }
                                                }
                                                else if (pcSub.A_Accepted_Qty != 0 || pcSub.B_Accepted_Qty != 0)
                                                {
                                                    if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "A" && pcSub.A_Accepted_Qty != 0)
                                                    {
                                                        zz.Return_Qty = pcSub.A_Accepted_Qty;
                                                        zz.Invoice_Qty = zz.Invoice_Qty - pcSub.A_Accepted_Qty;
                                                        zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                        _unitOfWork.InvoiceSubRepository.Update(zz);
                                                        _unitOfWork.Save();
                                                    }
                                                    else if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "B" && pcSub.B_Accepted_Qty != 0)
                                                    {
                                                        zz.Return_Qty = pcSub.B_Accepted_Qty;
                                                        zz.Invoice_Qty = zz.Invoice_Qty - pcSub.B_Accepted_Qty;
                                                        zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                        _unitOfWork.InvoiceSubRepository.Update(zz);
                                                        _unitOfWork.Save();
                                                    }
                                                }
                                                uscope.Complete();
                                            }
                                        }
                                    }
                                    dscope.Complete();
                                }
                            }
                        }
                        //  }
                        var GRNLIds = (grnEntity.GrnDetails).ToList();

                        var grnLineItemLs = DB.GRN_Line_item.Where(f => f.GRN_Number == grnNumber).ToList();
                        var WantToDelete = (from dL in grnLineItemLs
                                            where !(GRNLIds.Any(item2 => item2.GRN_Line_Id == dL.GRN_Line_Id))
                                            select dL).ToList();
                        if (WantToDelete.Count != 0)
                        {
                            foreach (var WL in WantToDelete)
                            {
                                var gd = _unitOfWork.GrnSubRepository.GetByID(WL.GRN_Line_Id);


                                //if (gd != null)
                                //{
                                gd.Is_Deleted = true;
                                gd.UpdatedDate = DateTime.Now;
                                _unitOfWork.GrnSubRepository.Update(gd);
                                _unitOfWork.Save();
                                //}
                            }
                        }
                        DB.Stock123();
                        DB.SaveChanges();


                    }
                    scope.Complete();
                }

            }
            return grnNumber;
        }
        //-----------------------------------DELETEGRN----------------------

        public bool DeleteGRN(int grnId)
        {
            var success = false;
            if (grnId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var g = _unitOfWork.GrnRepository.GetByID(grnId);
                    if (g != null)
                    {
                        g.is_Deleted = true;

                        DateTime Today = DateTime.Now;
                        var check = (from ee in DB.Stirnkage_Summary
                                     where ee.DC_Code == g.DC_Code
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
                        g.UpdatedDate = DateTime.Now;
                        _unitOfWork.GrnRepository.Update(g);
                        _unitOfWork.Save();

                        DB.Grn_Detection_Service(grnId);
                        DB.SaveChanges();

                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public string CreateGrn(GrnEntity grnEntity)
        {
            string grnNumber, locationId, GRN_prefix, prefix, wsNumber;
            int? incNumber, inNumber;
            DateTime Today = DateTime.Now;
            double? C_Qty = 0, B_Qty = 0, A_Qty = 0;
            using (var iscope = new TransactionScope())
            {
                string locationID = grnEntity.DC_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                GRN_prefix = rm.GetString("GRNT");
                GRN_NUM_Generation autoIncNumber = GetAutoIncrement(locationID);
                locationId = autoIncNumber.DC_Code;
                incNumber = autoIncNumber.GRN_Last_Number;
                string year = autoIncNumber.Financial_Year;
                int? incrementedValue = incNumber + 1;
                var Grnincrement = DB.GRN_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                Grnincrement.GRN_Last_Number = incrementedValue;
                _unitOfWork.AutoIncrementRepositoryGrn.Update(Grnincrement);
                _unitOfWork.Save();

                grnNumber = GRN_prefix + "/" + locationId + "/" + year + "/" + String.Format("{0:00000}", incNumber);
                iscope.Complete();
            }

            using (var scope = new TransactionScope())
            {
                var grnCreation = new GRN_Creation
                  {
                      GRN_Number = grnNumber,
                      DC_Code = grnEntity.DC_Code,
                      PO_Number = grnEntity.PO_Number,
                      CDN_Number = grnEntity.CDN_Number,
                      GRN_Rls_Date = DateTime.Now,
                      Voucher_Type = grnEntity.Voucher_Type,
                      SKU_Type_Id = grnEntity.SKU_Type_Id,
                      SKU_Type = grnEntity.SKU_Type,
                      Customer_Id = grnEntity.Customer_Id,
                      Vehicle_No = grnEntity.Vehicle_No,
                      Sales_Person_Id = grnEntity.Sales_Person_Id,
                      Sales_Person_Name = grnEntity.Sales_Person_Name,
                      Route_Id = grnEntity.Route_Id,
                      Route_Code = grnEntity.Route_Code,
                      Route = grnEntity.Route,
                      Customer_code = grnEntity.Customer_code,
                      Customer_Name = grnEntity.Customer_Name,
                      Supplier_Id = grnEntity.Supplier_Id,
                      Supplier_code = grnEntity.Supplier_code,
                      Supplier_Name = grnEntity.Supplier_Name,
                      is_Deleted = false,
                      STN_DC_Id = grnEntity.STN_DC_Id,
                      STN_DC_Code = grnEntity.STN_DC_Code,
                      STN_DC_Name = grnEntity.STN_DC_Name,
                      CreatedDate = DateTime.Now,
                      is_Syunc = false,
                      CreatedBy = grnEntity.CreatedBy,
                  };
                _unitOfWork.GrnRepository.Insert(grnCreation);
                _unitOfWork.Save();
                //
                int? grnId = grnCreation.INW_Id;

                var check = (from ee in DB.Stirnkage_Summary
                             where ee.DC_Code == grnEntity.DC_Code
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
               


                var model = new GRN_Line_item();
                foreach (var pSub in grnEntity.GrnDetails)
                {
                    model.GRN_Number = grnNumber;
                    model.INW_id = grnId;
                    model.SKU_ID = pSub.SKU_ID;
                    model.SKU_Code = pSub.SKU_Code;
                    model.SKU_Name = pSub.SKU_Name;
                    model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                    model.SKU_SubType = pSub.SKU_SubType;
                    model.UOM = pSub.UOM;
                    model.PO_QTY = pSub.PO_QTY;
                    model.Strinkage_Qty = pSub.Strinkage_Qty;
                    model.A_Accepted_Qty = pSub.A_Accepted_Qty;
                    A_Qty = pSub.A_Accepted_Qty;
                    model.B_Accepted_Qty = pSub.B_Accepted_Qty;
                    B_Qty = pSub.B_Accepted_Qty;
                    model.C_Accepted_Qty = pSub.C_Accepted_Qty;
                    C_Qty = pSub.C_Accepted_Qty;
                    model.A_Accepted_Price = pSub.A_Accepted_Price;
                    model.B_Accepted_Price = pSub.B_Accepted_Price;
                    model.C_Accepted_Price = pSub.C_Accepted_Price;
                    model.A_Converted_Qty = pSub.A_Converted_Qty;
                    model.B_Converted_Qty = pSub.B_Converted_Qty;
                    model.C_Converted_Qty = pSub.C_Converted_Qty;
                    model.Grade = pSub.Grade;
                    model.Billed_Qty = pSub.Billed_Qty;
                    model.Price_Book_Id = pSub.Price_Book_Id;
                    model.Price = pSub.Price;
                    model.Remark = pSub.Remark;
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = pSub.CreatedBy;
                    model.moved = false;

                    _unitOfWork.GrnSubRepository.Insert(model);
                    _unitOfWork.Save();

                    int grnLineId = model.GRN_Line_Id;
                    foreach (var skuSub in pSub.GRNSKULineItems)
                    {
                        var skuLSub = new GRN_SKU_Line_Items();
                        skuLSub.GRN_Line_Id = grnLineId;
                        skuLSub.SKU_Id = skuSub.SKU_Id;
                        skuLSub.A_Qty = skuSub.A_Qty;
                        skuLSub.B_Qty = skuSub.B_Qty;
                        skuLSub.C_Qty = skuSub.C_Qty;
                        skuLSub.Barcode = skuSub.Barcode;
                        skuLSub.Batch_Number = skuSub.Batch_Number;
                        skuLSub.UOM = skuSub.UOM;
                        skuLSub.CreatedDate = DateTime.Now;
                        skuLSub.CreatedBy = grnEntity.CreatedBy;
                        skuLSub.is_Deleted = false;

                        _unitOfWork.GrnSKULineItemRepository.Insert(skuLSub);
                        _unitOfWork.Save();

                    }
                    if (pSub.GRNSKULineItemsConsumables != null)
                    {
                        foreach (var t in pSub.GRNSKULineItemsConsumables)
                        {
                            var skuCSub = new GRN_Consumables();
                            skuCSub.GRN_Line_Id_FK = grnLineId;
                            skuCSub.SKU_Id = t.SKU_Id.Value;
                            skuCSub.Grade = t.Grade;
                            skuCSub.Consumable_Qty = t.Consumable_Qty;
                            skuCSub.UOM = t.UOM;
                            skuCSub.CreatedDate = DateTime.Now;
                            skuCSub.CreatedBy = grnEntity.CreatedBy;
                            skuCSub.is_Deleted = false;

                            _unitOfWork.GrnConsumablesRepository.Insert(skuCSub);
                            _unitOfWork.Save();
                        }
                    }

                    if (grnEntity.CDN_Number != null && grnEntity.Voucher_Type == "Customer Return")
                    {
                        if (C_Qty > 0)
                        {
                            using (var ascope = new TransactionScope())
                            {
                                string locatioID = grnEntity.DC_Code;
                                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                                prefix = rm.GetString("WST");
                                Wastage_Auto_Num_Gen autoIncNumber = GetWastAutoIncrement(locatioID);
                                locationId = autoIncNumber.DC_Code;
                                inNumber = autoIncNumber.Wastage_Last_Number;
                                int? incrementedValue = inNumber + 1;
                                var WSincrement = DB.Wastage_Auto_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                                WSincrement.Wastage_Last_Number = incrementedValue;
                                _unitOfWork.WastageNumIncrementRepository.Update(WSincrement);
                                _unitOfWork.Save();
                                wsNumber = prefix + "/" + locationId + "/" + String.Format("{0:00000}", inNumber);
                                ascope.Complete();
                            }
                            using (var bscope = new TransactionScope())
                            {
                                var Wastage = new Wastage_Creation
                                {
                                    Wastage_Number = wsNumber,
                                    DC_Code = grnEntity.DC_Code,
                                    Ref_Id = grnId,
                                    Ref_Number = grnEntity.CDN_Number,
                                    Wastage_Type = "Customer Return",
                                    Wastage_raisedBy = grnEntity.CreatedBy,
                                    Wastage_Rls_Date = grnEntity.GRN_Rls_Date,
                                    is_Deleted = false,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = grnEntity.CreatedBy,
                                    Wastage_Approval_Flag = null,
                                    is_Syunc = false,
                                };

                                _unitOfWork.WastageRepository.Insert(Wastage);
                                _unitOfWork.Save();

                                int? wsId = Wastage.Wastage_Id;
                                string wastageType = Wastage.Wastage_Type;

                                var wmodel = new Wastage_Line_item();
                                foreach (GrnLineItemsEntity paSub in grnEntity.GrnDetails)
                                {
                                    wmodel.Wastage_Number = wsNumber;
                                    wmodel.Wastage_Id = wsId;
                                    wmodel.SKU_ID = paSub.SKU_ID;
                                    wmodel.SKU_Code = paSub.SKU_Code;
                                    wmodel.SKU_Name = paSub.SKU_Name;
                                    wmodel.UOM = paSub.UOM;
                                    wmodel.Grade = "C";
                                    wmodel.Ref_Id = grnId;
                                    wmodel.Ref_Line_Id = grnLineId;
                                    wmodel.Wastage_Qty = paSub.C_Accepted_Qty;
                                    wmodel.Reason = "Customer Return";
                                    wmodel.Stock_Reduce_Flag = false;

                                    _unitOfWork.WastageSubRepository.Insert(wmodel);
                                    _unitOfWork.Save();
                                }
                                bscope.Complete();
                            }
                        }

                        if (A_Qty != 0 || B_Qty != 0)
                        {
                            using (var cscope = new TransactionScope())
                            {

                                var re = (from y in DB.Dispatch_Creation
                                          join l in DB.Dispatch_Line_item on y.Dispatch_Id equals l.Dispatch_Id
                                          where y.Customer_Dispatch_Number == grnEntity.CDN_Number
                                          select l).ToList();
                                foreach (var yy in re)
                                {
                                    foreach (GrnLineItemsEntity pbSub in grnEntity.GrnDetails)
                                    {
                                        using (var uscope = new TransactionScope())
                                        {
                                            if (pbSub.A_Accepted_Qty != 0 && pbSub.B_Accepted_Qty != 0)
                                            {
                                                if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "A" && pbSub.A_Accepted_Qty != 0)
                                                {
                                                    yy.Return_Qty = pbSub.A_Accepted_Qty;
                                                    _unitOfWork.DispatchSubRepository.Update(yy);
                                                    _unitOfWork.Save();
                                                }
                                                if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "B" && pbSub.B_Accepted_Qty != 0)
                                                {
                                                    yy.Return_Qty = pbSub.B_Accepted_Qty;
                                                    _unitOfWork.DispatchSubRepository.Update(yy);
                                                    _unitOfWork.Save();
                                                }
                                            }
                                            else if (pbSub.A_Accepted_Qty != 0 || pbSub.B_Accepted_Qty != 0)
                                            {
                                                if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "A" && pbSub.A_Accepted_Qty != 0)
                                                {
                                                    yy.Return_Qty = pbSub.A_Accepted_Qty;
                                                    _unitOfWork.DispatchSubRepository.Update(yy);
                                                    _unitOfWork.Save();
                                                }
                                                else if (yy.SKU_Name == pbSub.SKU_Name && yy.Grade == "B" && pbSub.B_Accepted_Qty != 0)
                                                {
                                                    yy.Return_Qty = pbSub.B_Accepted_Qty;
                                                    _unitOfWork.DispatchSubRepository.Update(yy);
                                                    _unitOfWork.Save();
                                                }
                                            }
                                            uscope.Complete();
                                        }
                                    }
                                }
                                cscope.Complete();
                            }
                            using (var dscope = new TransactionScope())
                            {
                                var inv = (from y in DB.Dispatch_Creation
                                           join t in DB.Invoice_Creation on y.Customer_Dispatch_Number equals t.Customer_Dispatch_Number
                                           join l in DB.Invoice_Line_item on t.invoice_Id equals l.Invoice_Id
                                           where y.Customer_Dispatch_Number == grnEntity.CDN_Number && t.Customer_Dispatch_Number == grnEntity.CDN_Number && y.Invoice_Flag == true
                                           select l).ToList();
                                foreach (var zz in inv)
                                {
                                    foreach (GrnLineItemsEntity pcSub in grnEntity.GrnDetails)
                                    {
                                        using (var uscope = new TransactionScope())
                                        {
                                            if (pcSub.A_Accepted_Qty != 0 && pcSub.B_Accepted_Qty != 0)
                                            {
                                                if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "A" && pcSub.A_Accepted_Qty != 0)
                                                {
                                                    zz.Return_Qty = pcSub.A_Accepted_Qty;
                                                    zz.Invoice_Qty = zz.Invoice_Qty - pcSub.A_Accepted_Qty;
                                                    zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                    _unitOfWork.InvoiceSubRepository.Update(zz);
                                                    _unitOfWork.Save();
                                                }
                                                if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "B" && pcSub.B_Accepted_Qty != 0)
                                                {
                                                    zz.Return_Qty = pcSub.B_Accepted_Qty;
                                                    zz.Invoice_Qty = zz.Invoice_Qty - pcSub.B_Accepted_Qty;
                                                    zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                    _unitOfWork.InvoiceSubRepository.Update(zz);
                                                    _unitOfWork.Save();
                                                }
                                            }
                                            else if (pcSub.A_Accepted_Qty != 0 || pcSub.B_Accepted_Qty != 0)
                                            {
                                                if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "A" && pcSub.A_Accepted_Qty != 0)
                                                {
                                                    zz.Return_Qty = pcSub.A_Accepted_Qty;
                                                    zz.Invoice_Qty = zz.Invoice_Qty - pcSub.A_Accepted_Qty;
                                                    zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                    _unitOfWork.InvoiceSubRepository.Update(zz);
                                                    _unitOfWork.Save();
                                                }
                                                else if (zz.SKU_Name == pcSub.SKU_Name && zz.Grade == "B" && pcSub.B_Accepted_Qty != 0)
                                                {
                                                    zz.Return_Qty = pcSub.B_Accepted_Qty;
                                                    zz.Invoice_Qty = zz.Invoice_Qty - pcSub.B_Accepted_Qty;
                                                    zz.Invoice_Amount = zz.Invoice_Qty * zz.Rate;
                                                    _unitOfWork.InvoiceSubRepository.Update(zz);
                                                    _unitOfWork.Save();
                                                }
                                            }
                                            uscope.Complete();
                                        }
                                    }
                                }
                                dscope.Complete();
                            }
                        }

                    }
                }
                DB.Stock123();
                DB.SaveChanges();

                scope.Complete();
                return grnCreation.GRN_Number;
            }
        }
        public Wastage_Auto_Num_Gen GetWastAutoIncrement(string locatioId)
        {
            var autoinc = DB.Wastage_Auto_Num_Gen.Where(x => x.DC_Code == locatioId).FirstOrDefault();
            var model = new Wastage_Auto_Num_Gen
            {
                Id = autoinc.Id,
                Wastage_Last_Number = autoinc.Wastage_Last_Number,
                DC_Code = autoinc.DC_Code
            };

            return model;
        }
        public GRN_NUM_Generation GetAutoIncrement(string locationId)
        {
            var autoinc = DB.GRN_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new GRN_NUM_Generation
            {
                GRN_Num_Gen_Id = autoinc.GRN_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                GRN_Last_Number = autoinc.GRN_Last_Number
            };

            return model;
        }

        public List<GrnEntity> GetGrnLineItem(string id)
        {
            var list = (from x in DB.GRN_Creation
                        where x.GRN_Number == id && x.is_Deleted == false
                        select new GrnEntity
                        {
                            INW_Id = x.INW_Id,
                            GRN_Number = x.GRN_Number,
                            PO_Number = x.PO_Number,
                            GRN_Rls_Date = x.GRN_Rls_Date,
                            CDN_Number = x.CDN_Number,
                            Voucher_Type = x.Voucher_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Vehicle_No = x.Vehicle_No,
                            Sales_Person_Id = x.Sales_Person_Id,
                            Sales_Person_Name = x.Sales_Person_Name,
                            Route_Id = x.Route_Id,
                            Route_Code = x.Route_Code,
                            Route = x.Route,
                            STN_DC_Id = x.STN_DC_Id,
                            STN_DC_Code = x.STN_DC_Code,
                            STN_DC_Name = x.STN_DC_Name,
                            Customer_Id = x.Customer_Id,
                            Customer_code = x.Customer_code,
                            Customer_Name = x.Customer_Name,
                            Supplier_Id = x.Supplier_Id,
                            Supplier_code = x.Supplier_code,
                            Supplier_Name = x.Supplier_Name,
                            DC_Code = x.DC_Code,
                            CreatedBy = x.CreatedBy,
                            Purchase = (from a in DB.Purchase_Order
                                        where a.PO_Number == x.PO_Number
                                        select new PurchasePOEntity
                                        {
                                            PO_raise_by = a.PO_raise_by
                                        }).ToList(),
                            GrnDetails = (from a in DB.GRN_Line_item
                                          where a.INW_id == x.INW_Id && a.Is_Deleted != true
                                          orderby a.SKU_Name
                                          select new GrnLineItemsEntity
                                          {
                                              GRN_Line_Id = a.GRN_Line_Id,
                                              INW_id = a.INW_id,
                                              GRN_Number = a.GRN_Number,
                                              SKU_ID = a.SKU_ID,
                                              SKU_Code = a.SKU_Code,
                                              SKU_Name = a.SKU_Name,
                                              //  SKU_Type=a.SKU_Type,
                                              SKU_SubType_Id = a.SKU_SubType_Id,
                                              SKU_SubType = a.SKU_SubType,
                                              UOM = a.UOM,
                                              PO_QTY = a.PO_QTY,
                                              A_Accepted_Qty = a.A_Accepted_Qty,
                                              B_Accepted_Qty = a.B_Accepted_Qty,
                                              C_Accepted_Qty = a.C_Accepted_Qty,
                                              Strinkage_Qty = a.Strinkage_Qty,
                                              A_Accepted_Price = a.A_Accepted_Price,
                                              B_Accepted_Price = a.B_Accepted_Price,
                                              C_Accepted_Price = a.C_Accepted_Price,
                                              A_Converted_Qty = a.A_Converted_Qty,
                                              B_Converted_Qty = a.B_Converted_Qty,
                                              C_Converted_Qty = a.C_Converted_Qty,
                                              Grade = a.Grade,
                                              Billed_Qty = a.Billed_Qty,
                                              Price_Book_Id = a.Price_Book_Id,
                                              Price = a.Price,
                                              Remark = a.Remark,
                                              Total_Accepted_Qty = (a.A_Accepted_Qty != null ? a.A_Accepted_Qty : 0) + (a.B_Accepted_Qty != null ? a.B_Accepted_Qty : 0)
                                          }).ToList(),
                        }).ToList();
            return list;
        }

        public List<poNumber> GetPoNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Purchase_Order
                         where x.Delivery_Date.Value.Year == date.Value.Year && x.Delivery_Date.Value.Month == date.Value.Month && x.Delivery_Date.Value.Day == date.Value.Day && x.is_Deleted == false && x.PO_Approval_Flag == true && x.DC_Code == Ulocation && x.Status == "Open"
                         select new poNumber
                         {
                             PO_Number = x.PO_Number
                         }).ToList();
            return query;
        }

        public List<stnNumber> GetStnNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Dispatch_Creation
                         where x.Delivery_Date.Value.Year == date.Value.Year && x.Delivery_Date.Value.Month == date.Value.Month && x.Delivery_Date.Value.Day == date.Value.Day && x.is_Deleted == false && x.Delivery_Location_Code == Ulocation && x.Status != "Closed"
                         select new stnNumber
                         {
                             STN_Number = x.Stock_Xfer_Dispatch_Number
                         }).ToList();
            return query;
        }

        public List<stiNumber> GetStiNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Stock_Transfer_Indent
                         where x.STI_RLS_date.Value.Year == date.Value.Year && x.STI_RLS_date.Value.Month == date.Value.Month && x.STI_RLS_date.Value.Day == date.Value.Day && x.is_Deleted == false && x.STI_Approval_Flag == true && x.Indent_Raised_by_DC_Code == Ulocation && x.Status == "Open"
                         select new stiNumber
                         {
                             STI_Number = x.STI_Number
                         }).ToList();
            return query;
        }



        public List<stiNumber> GetStiNumbersforSTN(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Stock_Transfer_Indent
                         where x.STI_RLS_date.Value.Year == date.Value.Year && x.STI_RLS_date.Value.Month == date.Value.Month && x.STI_RLS_date.Value.Day == date.Value.Day && x.is_Deleted == false && x.STI_Approval_Flag == true && x.Material_Source == Ulocation && x.Status == "Open"
                         select new stiNumber
                         {
                             STI_Number = x.STI_Number
                         }).ToList();
            return query;
        }

        public List<GrnEntity> GetGRN(string Ulocation)
        {
            var query = (from x in DB.GRN_Creation
                         where x.is_Deleted == false && x.DC_Code == Ulocation
                         select new GrnEntity
                         {
                             INW_Id = x.INW_Id,
                             GRN_Number = x.GRN_Number,
                             PO_Number = x.PO_Number,
                             CDN_Number = x.CDN_Number,
                             GRN_Rls_Date = x.GRN_Rls_Date,
                             Voucher_Type = x.Voucher_Type,
                             Vehicle_No = x.Vehicle_No,
                             Sales_Person_Id = x.Sales_Person_Id,
                             Sales_Person_Name = x.Sales_Person_Name,
                             Route_Id = x.Route_Id,
                             Route_Code = x.Route_Code,
                             Route = x.Route,
                             Customer_Id = x.Customer_Id,
                             Customer_code = x.Customer_code,
                             Customer_Name = x.Customer_Name,
                             Supplier_Id = x.Supplier_Id,
                             Supplier_code = x.Supplier_code,
                             Supplier_Name = x.Supplier_Name,
                             STN_DC_Code = x.STN_DC_Code,
                             STN_DC_Name = x.STN_DC_Name,
                             STN_DC_Id = x.STN_DC_Id,
                             SKU_Type = x.SKU_Type,
                             DC_Code = x.DC_Code

                         }).ToList();
            return query;
        }

        //public List<GrnEntity> GetGRNAND(DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation)
        //{
        //    var qu = (from a in DB.GRN_Creation
        //              where (a.GRN_Rls_Date.Value >= startDate.Value)&& (a.GRN_Rls_Date.Value<= endDate.Value) && a.is_Deleted == false && a.Supplier_Name == supplierName && a.DC_Code == Ulocation
        //              select new GrnEntity
        //              {
        //                  INW_Id = a.INW_Id,
        //                  PO_Number = a.PO_Number,
        //                  Voucher_Type = a.Voucher_Type,
        //                  Supplier_Id = a.Supplier_Id,
        //                  Supplier_code = a.Supplier_code,
        //                  Supplier_Name = a.Supplier_Name,
        //                  GRN_Rls_Date = a.GRN_Rls_Date,
        //                  GRN_Number = a.GRN_Number,
        //                  CDN_Number=a.CDN_Number,
        //                  STN_DC_Code = a.STN_DC_Code,
        //                  STN_DC_Name = a.STN_DC_Name,
        //                  STN_DC_Id = a.STN_DC_Id,
        //                  SKU_Type=a.SKU_Type,
        //                  DC_Code = a.DC_Code,
        //                  Counting = (from p in DB.GRN_Line_item
        //                              where p.INW_id == a.INW_Id
        //                              select new
        //                              {
        //                                  INW_Id = p.INW_id
        //                              }).Count()
        //              }).ToList();
        //    return qu;
        //}

        //public List<GrnEntity> GetGRNOR(DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation)
        //{
        //    var qu = (from a in DB.GRN_Creation
        //              where ((a.GRN_Rls_Date.Value>= startDate.Value) && (a.GRN_Rls_Date.Value <= endDate.Value) || a.Supplier_Name == supplierName) && a.is_Deleted == false && a.DC_Code == Ulocation
        //              select new GrnEntity
        //              {
        //                  INW_Id = a.INW_Id,
        //                  PO_Number = a.PO_Number,
        //                  Voucher_Type = a.Voucher_Type,
        //                  Supplier_Id = a.Supplier_Id,
        //                  Supplier_code = a.Supplier_code,
        //                  Supplier_Name = a.Supplier_Name,
        //                  GRN_Rls_Date = a.GRN_Rls_Date,
        //                  GRN_Number = a.GRN_Number,
        //                  CDN_Number=a.CDN_Number,
        //                  STN_DC_Code = a.STN_DC_Code,
        //                  STN_DC_Name = a.STN_DC_Name,
        //                  STN_DC_Id = a.STN_DC_Id,
        //                  SKU_Type=a.SKU_Type,
        //                  DC_Code = a.DC_Code,
        //                  Counting = (from p in DB.GRN_Line_item
        //                              where p.INW_id == a.INW_Id
        //                              select new
        //                              {
        //                                  INW_Id = p.INW_id
        //                              }).Count()
        //              }).ToList();
        //    return qu;
        //}
        // public List<GrnEntity> SearchGRN(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation, string Url = "null")
        // {

        ////     IQueryable<GrnEntity> qu;

        //     List<GrnEntity> result = new List<GrnEntity>();
        //     var menuAccess = (from t in DB.Role_Menu_Access
        //                       join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
        //                       where t.Role_Id == roleId && s.Url == Url
        //                       select t.Menu_Previlleges
        //     ).FirstOrDefault();
        //     int isDel, isViw, isEdt, isApp, iCrt;
        //     //  int iCrt;

        //     iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
        //     isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
        //     isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
        //     isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
        //     isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);


        //     //var menuAccess = DB.Role_Menu_Access
        //     //   .Join
        //     //   (
        //     //       DB.Menu_Master,
        //     //       c => c.Menu_Id,
        //     //       d => d.Menu_Id,
        //     //       (c, d) => new { c, d }
        //     //   )
        //     //   .Where(e => e.c.Role_Id == roleId).Where(g => g.d.Menu_Id == g.c.Menu_Id && g.d.Url == Url).GroupBy(e => new { e.d.Menu_Id })
        //     //   .Select(x => new FetchMenuDetails

        //     //   {
        //     //       MenuID = x.Key.Menu_Id,
        //     //       MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
        //     //       MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
        //     //       RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
        //     //       ControllerName = x.Select(c => c.d.Url).Distinct(),
        //     //       ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
        //     //   }).FirstOrDefault();
        // var qu = (from a in DB.GRN_Creation
        //           where a.is_Deleted == false && a.DC_Code == Ulocation
        //           select a).AsEnumerable().Select(a => new GrnEntity
        //           {
        //               INW_Id = a.INW_Id,
        //               PO_Number = a.PO_Number,
        //               Voucher_Type = a.Voucher_Type,
        //               Customer_Id = a.Customer_Id,
        //               Customer_code = a.Customer_code,
        //               Customer_Name = a.Customer_Name,
        //               Supplier_Id = a.Supplier_Id,
        //               Supplier_code = a.Supplier_code,
        //               Supplier_Name = a.Supplier_Name,
        //               GRN_Rls_Date = a.GRN_Rls_Date,
        //               GRN_Number = a.GRN_Number,
        //               CDN_Number = a.CDN_Number,
        //               STN_DC_Code = a.STN_DC_Code,
        //               STN_DC_Name = a.STN_DC_Name,
        //               STN_DC_Id = a.STN_DC_Id,
        //               SKU_Type = a.SKU_Type,
        //               DC_Code = a.DC_Code,
        //               //Menu_Id = menuAccess.MenuID,
        //               //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
        //               is_Create = iCrt,
        //               is_Delete =isDel,
        //               is_Edit = isEdt,
        //               is_Approval = isApp,
        //               is_View = isViw,
        //               Counting = (from p in DB.GRN_Line_item
        //                           where p.INW_id == a.INW_Id
        //                           select new
        //                           {
        //                               INW_Id = p.INW_id
        //                           }).Count(),
        //               GrnDetails = (from m in DB.GRN_Line_item
        //                             where m.INW_id == a.INW_Id
        //                             orderby m.SKU_Name
        //                             select new GrnLineItemsEntity
        //                             {
        //                                 GRN_Line_Id = m.GRN_Line_Id,
        //                                 INW_id = m.INW_id,
        //                                 GRN_Number = m.GRN_Number,
        //                                 SKU_ID = m.SKU_ID,
        //                                 SKU_Code = m.SKU_Code,
        //                                 SKU_Name = m.SKU_Name,
        //                                 //  SKU_Type=m.SKU_Type,
        //                                 SKU_SubType = m.SKU_SubType,
        //                                 UOM = m.UOM,
        //                                 PO_QTY = m.PO_QTY,
        //                                 A_Accepted_Qty = m.A_Accepted_Qty,
        //                                 B_Accepted_Qty = m.B_Accepted_Qty,
        //                                 C_Accepted_Qty = m.C_Accepted_Qty,
        //                                 A_Accepted_Price = m.A_Accepted_Price,
        //                                 B_Accepted_Price = m.B_Accepted_Price,
        //                                 C_Accepted_Price = m.C_Accepted_Price,
        //                                 Billed_Qty = m.Billed_Qty,
        //                                 A_Converted_Qty = m.A_Converted_Qty,
        //                                 B_Converted_Qty = m.B_Converted_Qty,
        //                                 C_Converted_Qty = m.C_Converted_Qty,
        //                                 Grade = m.Grade,
        //                                 Price = m.Price,
        //                                 Remark = m.Remark,
        //                                 Total_Accepted_Qty = (m.A_Accepted_Qty != null ? m.A_Accepted_Qty : 0) + (m.B_Accepted_Qty != null ? m.B_Accepted_Qty : 0),
        //                                 PO_Number = a.PO_Number,
        //                                 CDN_Number = a.CDN_Number,
        //                                 STN_DC_Code = a.STN_DC_Code,
        //                                 STN_DC_Name = a.STN_DC_Name,
        //                                 SKU_Type = a.SKU_Type,
        //                                 Supplier_Name = a.Supplier_Name,
        //                                 DC_Code = a.DC_Code,
        //                                 GRN_Rls_Date = a.GRN_Rls_Date,
        //                                 Voucher_Type = a.Voucher_Type,
        //                                 CreatedDate = a.CreatedDate,
        //                                 CreatedBy = a.CreatedBy,
        //                                 UpdatedBy = a.UpdatedBy,
        //                                 UpdatedDate = a.UpdatedDate,
        //                                 Customer_Id = a.Customer_Id,
        //                                 Customer_code = a.Customer_code,
        //                                 Customer_Name = a.Customer_Name



        //                             }).ToList(),
        //           });

        //     if (startDate.Value != null && endDate.Value != null && Ulocation != "null")
        //     {
        //         qu = qu.Where(a => a.GRN_Rls_Date.Value >= startDate.Value && a.GRN_Rls_Date.Value <= endDate.Value);
        //     }
        //     if (supplierName != "null")
        //     {
        //         qu = qu.Where(a => a.Supplier_Name == supplierName);
        //     }

        //     result = qu.ToList();

        //     return result;
        // }
        public List<GrnEntity> SearchGRN(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation, string Url = "null")
        {

            //     IQueryable<GrnEntity> qu;

            List<GrnEntity> result = new List<GrnEntity>();
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

            var qu = (from a in DB.GRN_Creation
                      where a.is_Deleted == false && a.DC_Code == Ulocation
                      select new GrnEntity
                      //a).AsEnumerable().Select(a => 
                      {
                          INW_Id = a.INW_Id,
                          PO_Number = a.PO_Number,
                          Voucher_Type = a.Voucher_Type,
                          Customer_Id = a.Customer_Id,
                          Customer_code = a.Customer_code,
                          Customer_Name = a.Customer_Name,
                          Supplier_Id = a.Supplier_Id,
                          Vehicle_No = a.Vehicle_No,
                          Sales_Person_Id = a.Sales_Person_Id,
                          Sales_Person_Name = a.Sales_Person_Name,
                          Route_Id = a.Route_Id,
                          Route_Code = a.Route_Code,
                          Route = a.Route,
                          Supplier_code = a.Supplier_code,
                          Supplier_Name = a.Supplier_Name,
                          GRN_Rls_Date = a.GRN_Rls_Date,
                          GRN_Number = a.GRN_Number,
                          CDN_Number = a.CDN_Number,
                          STN_DC_Code = a.STN_DC_Code,
                          STN_DC_Name = a.STN_DC_Name,
                          STN_DC_Id = a.STN_DC_Id,
                          SKU_Type = a.SKU_Type,
                          DC_Code = a.DC_Code,
                          //
                          //Menu_Id = menuAccess.MenuID,
                          //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.GRN_Line_item
                                      where p.INW_id == a.INW_Id && p.Is_Deleted != true
                                      select new
                                      {
                                          INW_Id = p.INW_id
                                      }).Count(),
                          GrnDetails = (from m in DB.GRN_Line_item
                                        where m.INW_id == a.INW_Id && m.Is_Deleted != true
                                        orderby m.SKU_Name
                                        select new GrnLineItemsEntity
                                        {
                                            GRN_Line_Id = m.GRN_Line_Id,
                                            INW_id = m.INW_id,
                                            GRN_Number = m.GRN_Number,
                                            SKU_ID = m.SKU_ID,
                                            SKU_Code = m.SKU_Code,
                                            SKU_Name = m.SKU_Name,
                                            //  SKU_Type=m.SKU_Type,
                                            //
                                            SKU_SubType = m.SKU_SubType,
                                            UOM = m.UOM,
                                            PO_QTY = m.PO_QTY,
                                            A_Accepted_Qty = m.A_Accepted_Qty,
                                            B_Accepted_Qty = m.B_Accepted_Qty,
                                            C_Accepted_Qty = m.C_Accepted_Qty,
                                            A_Accepted_Price = m.A_Accepted_Price,
                                            B_Accepted_Price = m.B_Accepted_Price,
                                            Strinkage_Qty = m.Strinkage_Qty,
                                            C_Accepted_Price = m.C_Accepted_Price,
                                            Billed_Qty = m.Billed_Qty,
                                            A_Converted_Qty = m.A_Converted_Qty,
                                            B_Converted_Qty = m.B_Converted_Qty,
                                            C_Converted_Qty = m.C_Converted_Qty,
                                            Grade = m.Grade,
                                            Price = m.Price,
                                            Remark = m.Remark,
                                            Total_Accepted_Qty = (m.A_Accepted_Qty != null ? m.A_Accepted_Qty : 0) + (m.B_Accepted_Qty != null ? m.B_Accepted_Qty : 0),
                                            PO_Number = a.PO_Number,
                                            CDN_Number = a.CDN_Number,
                                            STN_DC_Code = a.STN_DC_Code,
                                            STN_DC_Name = a.STN_DC_Name,
                                            SKU_Type = a.SKU_Type,
                                            Supplier_Name = a.Supplier_Name,
                                            DC_Code = a.DC_Code,
                                            GRN_Rls_Date = a.GRN_Rls_Date,
                                            Voucher_Type = a.Voucher_Type,
                                            CreatedDate = a.CreatedDate,
                                            CreatedBy = a.CreatedBy,
                                            UpdatedBy = a.UpdatedBy,
                                            UpdatedDate = a.UpdatedDate,
                                            Customer_Id = a.Customer_Id,
                                            Customer_code = a.Customer_code,
                                            Customer_Name = a.Customer_Name
                                        }).ToList(),
                      });
            if (startDate.Value != null && endDate.Value != null && Ulocation != "null")
            {
                qu = qu.Where(a => a.GRN_Rls_Date.Value >= startDate.Value && a.GRN_Rls_Date.Value <= endDate.Value);
            }
            if (supplierName != "null")
            {
                qu = qu.Where(a => a.Supplier_Name == supplierName);
            }

            result = qu.ToList();
            //foreach (var t in result)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return result;
        }
        public List<GrnEntity> SearchGRNCR(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation, string Url = "null")
        {

            //     IQueryable<GrnEntity> qu;

            List<GrnEntity> result = new List<GrnEntity>();
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

            var qu = (from a in DB.GRN_Creation
                      where a.is_Deleted == false && a.DC_Code == Ulocation && a.Voucher_Type=="Customer Return"
                      select new GrnEntity
                      //a).AsEnumerable().Select(a => 
                      {
                          INW_Id = a.INW_Id,
                          PO_Number = a.PO_Number,
                          Voucher_Type = a.Voucher_Type,
                          Customer_Id = a.Customer_Id,
                          Customer_code = a.Customer_code,
                          Customer_Name = a.Customer_Name,
                          Supplier_Id = a.Supplier_Id,
                          Vehicle_No = a.Vehicle_No,
                          Sales_Person_Id = a.Sales_Person_Id,
                          Sales_Person_Name = a.Sales_Person_Name,
                          Route_Id = a.Route_Id,
                          Route_Code = a.Route_Code,
                          Route = a.Route,
                          Supplier_code = a.Supplier_code,
                          Supplier_Name = a.Supplier_Name,
                          GRN_Rls_Date = a.GRN_Rls_Date,
                          GRN_Number = a.GRN_Number,
                          CDN_Number = a.CDN_Number,
                          STN_DC_Code = a.STN_DC_Code,
                          STN_DC_Name = a.STN_DC_Name,
                          STN_DC_Id = a.STN_DC_Id,
                          SKU_Type = a.SKU_Type,
                          DC_Code = a.DC_Code,
                          //
                          //Menu_Id = menuAccess.MenuID,
                          //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          Counting = (from p in DB.GRN_Line_item
                                      where p.INW_id == a.INW_Id && p.Is_Deleted != true
                                      select new
                                      {
                                          INW_Id = p.INW_id
                                      }).Count(),
                          GrnDetails = (from m in DB.GRN_Line_item
                                        where m.INW_id == a.INW_Id && m.Is_Deleted != true
                                        orderby m.SKU_Name
                                        select new GrnLineItemsEntity
                                        {
                                            GRN_Line_Id = m.GRN_Line_Id,
                                            INW_id = m.INW_id,
                                            GRN_Number = m.GRN_Number,
                                            SKU_ID = m.SKU_ID,
                                            SKU_Code = m.SKU_Code,
                                            SKU_Name = m.SKU_Name,
                                            //  SKU_Type=m.SKU_Type,
                                            //
                                            SKU_SubType = m.SKU_SubType,
                                            UOM = m.UOM,
                                            PO_QTY = m.PO_QTY,
                                            A_Accepted_Qty = m.A_Accepted_Qty,
                                            B_Accepted_Qty = m.B_Accepted_Qty,
                                            C_Accepted_Qty = m.C_Accepted_Qty,
                                            A_Accepted_Price = m.A_Accepted_Price,
                                            B_Accepted_Price = m.B_Accepted_Price,
                                            Strinkage_Qty = m.Strinkage_Qty,
                                            C_Accepted_Price = m.C_Accepted_Price,
                                            Billed_Qty = m.Billed_Qty,
                                            A_Converted_Qty = m.A_Converted_Qty,
                                            B_Converted_Qty = m.B_Converted_Qty,
                                            C_Converted_Qty = m.C_Converted_Qty,
                                            Grade = m.Grade,
                                            Price = m.Price,
                                            Remark = m.Remark,
                                            Total_Accepted_Qty = (m.A_Accepted_Qty != null ? m.A_Accepted_Qty : 0) + (m.B_Accepted_Qty != null ? m.B_Accepted_Qty : 0),
                                            PO_Number = a.PO_Number,
                                            CDN_Number = a.CDN_Number,
                                            STN_DC_Code = a.STN_DC_Code,
                                            STN_DC_Name = a.STN_DC_Name,
                                            SKU_Type = a.SKU_Type,
                                            Supplier_Name = a.Supplier_Name,
                                            DC_Code = a.DC_Code,
                                            GRN_Rls_Date = a.GRN_Rls_Date,
                                            Voucher_Type = a.Voucher_Type,
                                            CreatedDate = a.CreatedDate,
                                            CreatedBy = a.CreatedBy,
                                            UpdatedBy = a.UpdatedBy,
                                            UpdatedDate = a.UpdatedDate,
                                            Customer_Id = a.Customer_Id,
                                            Customer_code = a.Customer_code,
                                            Customer_Name = a.Customer_Name
                                        }).ToList(),
                      });
            if (startDate.Value != null && endDate.Value != null && Ulocation != "null")
            {
                qu = qu.Where(a => a.GRN_Rls_Date.Value >= startDate.Value && a.GRN_Rls_Date.Value <= endDate.Value);
            }
            if (supplierName != "null")
            {
                qu = qu.Where(a => a.Supplier_Name == supplierName);
            }

            result = qu.ToList();
            //foreach (var t in result)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return result;
        }
        //---------------------------DELETEGRNLINEITEM----------------------------
        public bool DeleteGRNOrderLineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.GrnSubRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.GrnSubRepository.Update(p);
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
