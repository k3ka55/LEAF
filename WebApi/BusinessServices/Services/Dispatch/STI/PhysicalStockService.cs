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
    public class PhysicalStockService : IPhysicalStock
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public PhysicalStockService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Strinkage_Stock_Adj_Num_Gen GetStrinkageAutoIncrement(string locationId)
        {
            var autoinc = DB.Strinkage_Stock_Adj_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new Strinkage_Stock_Adj_Num_Gen
            {
                Strinkage_Stock_Adj_Num_Gen_Id = autoinc.Strinkage_Stock_Adj_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Strinkage_Stock_Adj_Last_Number = autoinc.Strinkage_Stock_Adj_Last_Number
            };

            return model;
        }
        public int UpdateStockFromPhy(StockFromPhysicalStockEntity StinkageLineItems)
        {
            int Sucess = 0;

            string prefix, locationId, poNumber;
            int? incNumber;
            int stkSummary_Id;
            DateTime Today = DateTime.UtcNow;
            using (var scopeaA = new TransactionScope())
            {
                int check = (from ee in DB.Stirnkage_Summary
                             where ee.DC_Code == StinkageLineItems.DC_Code
                             && ee.CreatedDate.Value.Year == Today.Year
                              && ee.CreatedDate.Value.Month == Today.Month
                               && ee.CreatedDate.Value.Day == Today.Day
                               && ee.is_Adjusted == true
                             select ee.Stirnkage_Summary_Id).Count();

                if (check == 0)
                {


                    string locationID = StinkageLineItems.DC_Code;
                    var AA = DB.Strinkage_Stock_Adj_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
                    ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                    prefix = rm.GetString("STRKT");
                    Strinkage_Stock_Adj_Num_Gen autoIncNumber = GetStrinkageAutoIncrement(locationID);
                    locationId = autoIncNumber.DC_Code;
                    incNumber = autoIncNumber.Strinkage_Stock_Adj_Last_Number;
                    int? incrementedValue = incNumber + 1;
                    var POincrement = DB.Strinkage_Stock_Adj_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                    POincrement.Strinkage_Stock_Adj_Last_Number = incrementedValue;
                    _unitOfWork.StrinkageStockAdjNumGenRepository.Update(POincrement);
                    _unitOfWork.Save();

                    poNumber = prefix + "/" + locationId + "/" + DateTime.UtcNow + "/" + String.Format("{0:00000}", incNumber);

                    //int chktransaction=from k in DB.Stirnkage_Summary
                    //                   where k.DC_Code==StinkageLineItems.DC_Code


                    var Strinkagesummary = new Stirnkage_Summary
                    {
                        Adjustment_Batch_Code = poNumber,
                        DC_Code = StinkageLineItems.DC_Code,
                        Adjustment_Freeze = false,
                        is_Adjusted = true,
                        Closing_Date = DateTime.UtcNow,
                        CreateBy = StinkageLineItems.CreateBy,
                        CreatedDate = DateTime.UtcNow,
                    };

                    _unitOfWork.StrinkagesummaryRepository.Insert(Strinkagesummary);
                    _unitOfWork.Save();
                    stkSummary_Id = Strinkagesummary.Stirnkage_Summary_Id;


                    var AStock = new Strinkage_Stock_Adjustement();

                    if (StinkageLineItems.StrinkageStock != null)
                    {
                        foreach (var Items in StinkageLineItems.StrinkageStock)
                        {
                            AStock.Adjustment_Batch_Code = poNumber;
                            AStock.Stirnkage_Summary_Id_FK = stkSummary_Id;
                            //   AStock.Stock_Id = m.Stock_Id;
                            //AStock.Material_Code = Items.Material_Code;
                            //AStock.Stock_code = Items.Stock_code;
                            AStock.DC_Code = Items.DC_Code;
                            AStock.DC_Name = Items.DC_Name;
                            AStock.SKU_Code = Items.SKU_Code;
                            AStock.SKU_Name = Items.SKU_Name;
                            AStock.SKU_Type_Id = Items.SKU_Type_Id;
                            AStock.SKU_Type = Items.SKU_Type;
                            AStock.System_Stock_Qty = Items.System_Stock;
                            AStock.Closing_Qty = Items.Closing_Qty;
                            AStock.Strinkage_Qty = Items.Shrinkage_Qty;
                            AStock.Close_Date = DateTime.UtcNow;
                            AStock.UOM = Items.UOM;
                            AStock.Grade = Items.Grade;
                            AStock.CreatedDate = DateTime.UtcNow;
                            AStock.CreateBy = StinkageLineItems.CreateBy;
                            _unitOfWork.StrinkageStockAdjRepository.Insert(AStock);
                            _unitOfWork.Save();



                            var t = (from u in DB.Stocks
                                     where u.SKU_Name == Items.SKU_Name
                                     && u.DC_Code == Items.DC_Code
                                     && u.Grade == Items.Grade
                                     && u.SKU_Type == Items.SKU_Type
                                     select u).FirstOrDefault();


                            if (t != null)
                            {
                                t.Closing_Qty = Items.Closing_Qty;
                                //   t.Closing_Qty = t.Closing_Qty - Items.Shrinkage_Qty;
                                DB.Entry(t).State = EntityState.Modified;
                                DB.SaveChanges();

                            }
                            else
                            {
                                Stock stockConverTrack = new Stock();
                                stockConverTrack.DC_Code = Items.DC_Code;
                                stockConverTrack.DC_Name = Items.DC_Name;
                                stockConverTrack.SKU_Name = Items.SKU_Name;
                                stockConverTrack.SKU_Type_Id = Items.SKU_Type_Id;
                                stockConverTrack.SKU_Type = Items.SKU_Type;
                                stockConverTrack.Closing_Qty = Items.Closing_Qty;
                                stockConverTrack.UOM = Items.UOM;
                                stockConverTrack.Grade = Items.Grade;
                                stockConverTrack.CreateBy = StinkageLineItems.CreateBy;
                                stockConverTrack.CreatedDate = DateTime.Now;


                                _unitOfWork.StockRepository.Insert(stockConverTrack);
                                _unitOfWork.Save();
                            }




                        }
                        Sucess = 2;
                    }



                    scopeaA.Complete();
                }
                else
                {
                    Sucess = 1;
                }
            }
            return Sucess;
        }



        //--------------------------------CREATE----------------------------------------
        public bool CreatePHysicalStock(PhysicalEntity physicalEntity)
        {
            using (var scope = new TransactionScope())
            {
                var physicalStock = new Physical_Stock();
                foreach (PhysicalStockEntity pSub in physicalEntity.PhyStock)
                {
                    physicalStock.Phy_Stock_code = pSub.Phy_Stock_code;
                    physicalStock.DC_id = physicalEntity.DC_id;
                    physicalStock.DC_Code = physicalEntity.DC_Code;
                    physicalStock.DC_Name = physicalEntity.DC_Name;
                    physicalStock.Supplier_Id = pSub.Supplier_Id;
                    physicalStock.Supplier_Code = pSub.Supplier_Code;
                    physicalStock.Supplier_Name = pSub.Supplier_Name;
                    physicalStock.SKU_Id = pSub.SKU_Id;
                    physicalStock.SKU_Code = pSub.SKU_Code;
                    physicalStock.SKU_Name = pSub.SKU_Name;
                    physicalStock.SKU_Type_Id = pSub.SKU_Type_Id;
                    physicalStock.SKU_Type = pSub.SKU_Type;
                    physicalStock.Pack_Type_Id = pSub.Pack_Type_Id;
                    physicalStock.Pack_Size = pSub.Pack_Size;
                    physicalStock.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                    physicalStock.Pack_Weight_Type = pSub.Pack_Weight_Type;
                    physicalStock.Pack_Type = pSub.Pack_Type;
                    physicalStock.Closing_Qty = pSub.Closing_Qty;
                    physicalStock.UOM = pSub.UOM;
                    physicalStock.Grade = pSub.Grade;
                    physicalStock.Closing_Date_Time = physicalEntity.Closing_Date_Time;
                    physicalStock.Aging = physicalEntity.Closing_Date_Time.ToString();
                    physicalStock.Floor_Supervisor = physicalEntity.Floor_Supervisor;
                    physicalStock.CreatedDate = DateTime.Now;
                    physicalStock.CreateBy = physicalEntity.CreateBy;
                    physicalStock.is_Syunc = false;
                    physicalStock.is_Deleted = false;
                    _unitOfWork.PhysicalStockRepository.Insert(physicalStock);
                    _unitOfWork.Save();
                }
                //

                scope.Complete();
            }
            return true;
        }




        //-----------------------------------------SEARCH-------------------------------------------

        public List<PhysicalStockEntity> GetPhysicalStock(int? roleId, DateTime? date, string ULocation, string Url)
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


            var qu = (from a in DB.Physical_Stock
                      where (a.Closing_Date_Time.Value.Year == date.Value.Year && a.Closing_Date_Time.Value.Month == date.Value.Month && a.Closing_Date_Time.Value.Day == date.Value.Day) && a.DC_Code == ULocation && a.is_Deleted == false
                      select new { a }).ToList();
            List<PhysicalStockEntity> phyStkEntity = new List<PhysicalStockEntity>();

            foreach (var phy in qu)
            {
                double? Stock_Qty_Sum = 0;
                double? total_Qty = 0;

                var stockQty = DB.Stocks.Where(s => s.SKU_Name == phy.a.SKU_Name && s.DC_Code == phy.a.DC_Code && s.Grade == phy.a.Grade && s.SKU_Type == phy.a.SKU_Type).Select(a => a.Closing_Qty).Sum();

                if (stockQty == null)
                {
                    stockQty = 0;
                }

                Stock_Qty_Sum = stockQty;

                if (Stock_Qty_Sum != null)
                {
                    total_Qty = double.Parse(Stock_Qty_Sum.ToString());
                }

                PhysicalStockEntity py = new PhysicalStockEntity
                {
                    Phy_Stock_Id = phy.a.Phy_Stock_Id,
                    Phy_Stock_code = phy.a.Phy_Stock_code,
                    DC_id = phy.a.DC_id,
                    DC_Code = phy.a.DC_Code,
                    DC_Name = phy.a.DC_Name,
                    Supplier_Id = phy.a.Supplier_Id,
                    Supplier_Code = phy.a.Supplier_Code,
                    Supplier_Name = phy.a.Supplier_Name,
                    SKU_Id = phy.a.SKU_Id,
                    SKU_Code = phy.a.SKU_Code,
                    SKU_Name = phy.a.SKU_Name,
                    SKU_Type_Id = phy.a.SKU_Type_Id,
                    SKU_Type = phy.a.SKU_Type,
                    Pack_Type_Id = phy.a.Pack_Type_Id,
                    Pack_Size = phy.a.Pack_Size,
                    Pack_Weight_Type_Id = phy.a.Pack_Weight_Type_Id,
                    Pack_Weight_Type = phy.a.Pack_Weight_Type,
                    Pack_Type = phy.a.Pack_Type,
                    Closing_Qty = phy.a.Closing_Qty,
                    System_Stock = total_Qty,
                    Deviation = total_Qty - phy.a.Closing_Qty,
                    //phy.a.Closing_Qty > total_Qty ? phy.a.Closing_Qty - total_Qty : total_Qty - phy.a.Closing_Qty,
                    UOM = phy.a.UOM,
                    Grade = phy.a.Grade,
                    Closing_Date_Time = phy.a.Closing_Date_Time,
                    Aging = phy.a.Aging,
                    Floor_Supervisor = phy.a.Floor_Supervisor,
                    CreatedDate = phy.a.CreatedDate,
                    CreateBy = phy.a.CreateBy,
                    is_Create = iCrt,
                    is_Delete = isDel,
                    is_Edit = isEdt,
                    is_Approval = isApp,
                    is_View = isViw,
                    //Menu_Id = menuAccess.MenuID,
                    //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                    //is_Create = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Add"]),
                    //is_Delete = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Delete"]),
                    //is_Edit = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Edit"]),
                    //is_Approval = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Approval"]),
                    //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"]),
                };

                phyStkEntity.Add(py);
            }

            //foreach (var t in phyStkEntity)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}

            //
            return phyStkEntity;
        }
        public bool DeletePhysicalStock(int phyStockId)
        {
            var success = false;
            if (phyStockId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.PhysicalStockRepository.GetByID(phyStockId);
                    if (p != null)
                    {
                        p.is_Deleted = true;
                        _unitOfWork.PhysicalStockRepository.Update(p);
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

