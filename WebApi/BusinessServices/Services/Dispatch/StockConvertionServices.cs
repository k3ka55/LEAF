using BusinessEntities;
using BusinessServices.Interfaces;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices.Services.Dispatch
{
    public class StockConvertionServices : IStockConvertionServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;

        public StockConvertionServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<StockEntity> getDatewiseStocks(DateTime date, String dc_Code)
        {
            var stock_list = new List<StockEntity>();

            stock_list = (from x in DB.Stock_History
                          where x.DC_Code == dc_Code

                                    && x.CreatedDate.Value.Year == date.Year
                                     && x.CreatedDate.Value.Month == date.Month
                                      && x.CreatedDate.Value.Day == date.Day
                          select new StockEntity
                          {
                              Stock_Id = x.Stock_Id,
                              Stock_code = x.Stock_code,
                              DC_Code = x.DC_Code,
                              DC_Name = x.DC_Name,
                              SKU_Code = x.SKU_Code,
                              SKU_Name = x.SKU_Name,
                              SKU_Type_Id = x.SKU_Type_Id,
                              SKU_Type = x.SKU_Type,

                              Closing_Qty = x.Closing_Qty,

                              UOM = x.UOM,
                              Grade = x.Grade
                          }).ToList();

            return stock_list;
        }

        public List<StockEntity> getStocks(DateTime date,String dc_Code)
        {
            var stock_list = new List<StockEntity>();

            stock_list = (from x in DB.Stocks
                          where x.DC_Code == dc_Code && x.CreatedDate <= date
                          select new StockEntity
                          {
                              Stock_Id = x.Stock_Id,
                              Stock_code = x.Stock_code,
                              DC_Code = x.DC_Code,
                              DC_Name = x.DC_Name, 
                              SKU_Code = x.SKU_Code,
                              SKU_Name = x.SKU_Name,
                              SKU_Type_Id = x.SKU_Type_Id,
                              SKU_Type = x.SKU_Type,
                             //Dispatch = (from z in DB.Dispatch_Creation
                              //            join y in DB.Dispatch_Line_item on z.Dispatch_Id equals y.Dispatch_Id
                              //            where z.Dispatch_Location_Code == x.DC_Code && z.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false
                              //            select y.Dispatch_Qty).Sum(),
                              //Wastage = (from z in DB.Wastage_Creation
                              //           join y in DB.Wastage_Line_item on z.Wastage_Id equals y.Wastage_Id
                              //           where z.DC_Code == x.DC_Code && y.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false && z.Wastage_Approval_Flag == true
                              //           select y.Wastage_Qty).Sum(),
                              //Closing_Qty =  x.Closing_Qty > (((from z in DB.Dispatch_Creation
                              //               join y in DB.Dispatch_Line_item on z.Dispatch_Id equals y.Dispatch_Id
                              //             where z.Dispatch_Location_Code == x.DC_Code && z.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false
                              //               select y.Dispatch_Qty).Sum() != null ? (from z in DB.Dispatch_Creation
                              //               join y in DB.Dispatch_Line_item on z.Dispatch_Id equals y.Dispatch_Id
                              //             where z.Dispatch_Location_Code == x.DC_Code && z.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false
                              //               select y.Dispatch_Qty).Sum() : 0 )+((from z in DB.Wastage_Creation
                              //                  join y in DB.Wastage_Line_item on z.Wastage_Id equals y.Wastage_Id
                              //             where z.DC_Code == x.DC_Code && y.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false && z.Wastage_Approval_Flag == true
                              //                  select y.Wastage_Qty).Sum() != null? (from z in DB.Wastage_Creation
                              //                  join y in DB.Wastage_Line_item on z.Wastage_Id equals y.Wastage_Id
                              //             where z.DC_Code == x.DC_Code && y.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false && z.Wastage_Approval_Flag == true
                              //                  select y.Wastage_Qty).Sum():0))? x.Closing_Qty - (((from z in DB.Dispatch_Creation
                              //               join y in DB.Dispatch_Line_item on z.Dispatch_Id equals y.Dispatch_Id
                              //             where z.Dispatch_Location_Code == x.DC_Code && z.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false
                              //               select y.Dispatch_Qty).Sum() != null ? (from z in DB.Dispatch_Creation
                              //               join y in DB.Dispatch_Line_item on z.Dispatch_Id equals y.Dispatch_Id
                              //             where z.Dispatch_Location_Code == x.DC_Code && z.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false
                              //               select y.Dispatch_Qty).Sum() : 0 )+((from z in DB.Wastage_Creation
                              //                  join y in DB.Wastage_Line_item on z.Wastage_Id equals y.Wastage_Id
                              //             where z.DC_Code == x.DC_Code && y.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false && z.Wastage_Approval_Flag == true
                              //                  select y.Wastage_Qty).Sum() != null? (from z in DB.Wastage_Creation
                              //                  join y in DB.Wastage_Line_item on z.Wastage_Id equals y.Wastage_Id
                              //             where z.DC_Code == x.DC_Code && y.SKU_Type == x.SKU_Type && y.SKU_Name == x.SKU_Name && y.Grade == x.Grade && z.is_Deleted == false && z.Wastage_Approval_Flag == true
                              //                  select y.Wastage_Qty).Sum():0)) : 0, 
                              Closing_Qty = x.Closing_Qty,
                              //Closing_Qty = x.Closing_Qty > ((Dispatch != null ? Dispatch : 0) + (Wastage != null ? Wastage : 0)) ? ((x.Closing_Qty) - ((Dispatch != null ? Dispatch : 0) + (Wastage != null ? Wastage : 0))) : 0,
                                  //x.Closing_Qty > ((Dispatch != null ? Dispatch : 0) + (Wastage != null ? Wastage : 0)) ? x.Closing_Qty - ((Dispatch != null ? Dispatch : 0) + (Wastage != null ? Wastage : 0)) : 0,
                                  //x.Closing_Qty > (Dispatch + Wastage) ? x.Closing_Qty - (Dispatch + Wastage) : 0,
                              UOM = x.UOM,
                              Grade = x.Grade
                          }).ToList();

            return stock_list;
        }

        public bool Convert_Stock(StockEntity stkEntity)
        {
            using (var scope = new TransactionScope())
            {
                var stk = _unitOfWork.StockRepository.GetByID(stkEntity.Stock_Id);
                using (var scope8 = new TransactionScope())
                {                    

                    if (stk != null)
                    {
                        _unitOfWork.StockRepository.Delete(stk);
                        _unitOfWork.Save();
                    }

                    scope8.Complete();
                }
                using (var scope1 = new TransactionScope())
                {
                    Stock_Convertion_Tracking stockConverTrack = new Stock_Convertion_Tracking();
                    stockConverTrack.Stock_Id = stkEntity.Stock_Id;
                    stockConverTrack.Stock_Code = stkEntity.Stock_code;
                    stockConverTrack.DC_id = DB.DC_Master.Where(x => x.DC_Code == stkEntity.DC_Code).Select(x => x.Dc_Id).FirstOrDefault();
                    stockConverTrack.DC_Code = stkEntity.DC_Code;
                    stockConverTrack.SKU_Id = DB.SKU_Master.Where(x => x.SKU_Name == stkEntity.SKU_Name).Select(x => x.SKU_Id).FirstOrDefault();
                    stockConverTrack.SKU_Name = stkEntity.SKU_Name;
                    stockConverTrack.SKU_Type = stkEntity.SKU_Type;
                    stockConverTrack.Grade = stkEntity.Grade;
                    stockConverTrack.Stock_Qty = stkEntity.Closing_Qty;
                    stockConverTrack.UOM = stkEntity.UOM;
                    stockConverTrack.Type = stkEntity.Type;
                    stockConverTrack.Created_By = stkEntity.CreateBy;
                    stockConverTrack.Created_Date = DateTime.Now;
                    stockConverTrack.Is_Syunc = false;

                    _unitOfWork.StockConvertionRepository.Insert(stockConverTrack);
                    _unitOfWork.Save();
                    
                    scope1.Complete();
                }                

                scope.Complete();
            }
            //
            string stockNumber;

            foreach (StockEntity stocks in stkEntity.convertedStocks)
            {
                var stockEntry = new Stock();
                string stkCode;
                int stkId;
                using (var scope4 = new TransactionScope())
                {
                    var StockExist = DB.Stocks.Where(x => x.DC_Code == stocks.DC_Code && x.SKU_Name == stocks.SKU_Name && x.SKU_Type == stocks.SKU_Type && x.Grade == stocks.Grade).FirstOrDefault();
                    if (StockExist != null)
                    {
                        using (var scope7 = new TransactionScope())
                        {
                            var ExistQty = DB.Stocks.Where(x => x.DC_Code == stocks.DC_Code && x.SKU_Name == stocks.SKU_Name && x.SKU_Type == stocks.SKU_Type && x.Grade == stocks.Grade).Select(y => y.Closing_Qty).Sum();

                            if (ExistQty == null)
                            {
                                ExistQty = 0;
                            }

                            StockExist.Closing_Qty = ExistQty + stocks.Closing_Qty;
                            DB.Entry(StockExist).State = EntityState.Modified;
                            DB.SaveChanges();
                            //_unitOfWork.StockRepository.Update(StockExist);
                            //_unitOfWork.Save();

                            stkCode = StockExist.Stock_code;
                            stkId = StockExist.Stock_Id;

                            scope7.Complete();
                        }
                    }
                    else
                    {
                        using (var scope5 = new TransactionScope())
                        {
                            string locationID = stocks.DC_Code;
                            Stock_Code_Num_Gen autoinc = DB.Stock_Code_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
                            ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                            string prefix = rm.GetString("STKT");
                            string locationId = autoinc.DC_Code;
                            string year = autoinc.Financial_Year;
                            int? incNumber = autoinc.Stock_Last_Number;
                            stockNumber = prefix + "/" + locationId + "/" + year + "/" + String.Format("{0:00000}", incNumber);
                            int? incrementedValue = incNumber + 1;
                            autoinc.Stock_Last_Number = incrementedValue;
                            _unitOfWork.StockAutoIncrementRepository.Update(autoinc);
                            _unitOfWork.Save();
                            scope5.Complete();
                        }

                        using (var scope6 = new TransactionScope())
                        {
                            stockEntry.Stock_code = stockNumber;
                            stockEntry.DC_Code = stocks.DC_Code;
                            stockEntry.DC_Name = stocks.DC_Name;
                            stockEntry.SKU_Code = stocks.SKU_Code;
                            stockEntry.SKU_Name = stocks.SKU_Name;
                            stockEntry.SKU_Type_Id = stocks.SKU_Type_Id;
                            stockEntry.SKU_Type = stocks.SKU_Type;
                            stockEntry.Closing_Qty = stocks.Closing_Qty;
                            stockEntry.UOM = stocks.UOM;
                            stockEntry.Grade = stocks.Grade;
                            stockEntry.CreateBy = stocks.CreateBy;
                            stockEntry.CreatedDate = DateTime.Now;

                            _unitOfWork.StockRepository.Insert(stockEntry);
                            _unitOfWork.Save();

                            stkCode = stockEntry.Stock_code;
                            stkId = stockEntry.Stock_Id;

                            scope6.Complete();
                        }
                    }
                    
                    scope4.Complete();
                }

                using (var scope3 = new TransactionScope())
                {
                    Stock_Convertion_Tracking stockConverTrack = new Stock_Convertion_Tracking();

                    stockConverTrack.Stock_Id = stkId;
                    stockConverTrack.Stock_Code = stkCode;
                    stockConverTrack.DC_id = DB.DC_Master.Where(x => x.DC_Code == stocks.DC_Code).Select(x => x.Dc_Id).FirstOrDefault();
                    stockConverTrack.DC_Code = stocks.DC_Code;
                    stockConverTrack.SKU_Id = DB.SKU_Master.Where(x => x.SKU_Name == stocks.SKU_Name).Select(x => x.SKU_Id).FirstOrDefault();
                    stockConverTrack.SKU_Name = stocks.SKU_Name;
                    stockConverTrack.SKU_Type = stocks.SKU_Type;
                    stockConverTrack.Grade = stocks.Grade;
                    stockConverTrack.Stock_Qty = stocks.Closing_Qty;
                    stockConverTrack.UOM = stocks.UOM;
                    stockConverTrack.Type = stocks.Type;
                    stockConverTrack.Created_By = stocks.CreateBy;
                    stockConverTrack.Created_Date = DateTime.Now;
                    stockConverTrack.Convert_From_Stock_Code = stkEntity.Stock_code;
                    stockConverTrack.Is_Syunc = false;

                    _unitOfWork.StockConvertionRepository.Insert(stockConverTrack);
                    _unitOfWork.Save();

                    scope3.Complete();
                }
            }
            return true;
        }

        public List<StockConvertionEntity> StockConvertedSummary(int dcid,DateTime createdDate)
        {
            var returnList = (from x in DB.Stock_Convertion_Tracking
                              where x.DC_id == dcid && x.Created_Date == createdDate
                              //x.Created_Date.Value.Year == createdDate.Year
                              //&&
                              //x.Created_Date.Value.Month == createdDate.Month 
                              //&&
                              //x.Created_Date.Value.Day == createdDate.Day 
                              && x.Type == "ORIGINAL"
                              select new StockConvertionEntity
                              {
                                  Stock_Convertion_Id = x.Stock_Convertion_Id,
                                  Stock_Id = x.Stock_Convertion_Id,
                                  Stock_Code = x.Stock_Code,
                                  DC_id = x.DC_id,
                                  DC_Code = x.DC_Code,
                                  SKU_Id = x.SKU_Id,
                                  SKU_Name = x.SKU_Name,
                                  SKU_Type = x.SKU_Type,
                                  Grade = x.Grade,
                                  Stock_Qty = x.Stock_Qty,
                                  UOM = x.UOM,
                                  Type = x.Type,
                                  Created_By = x.Created_By,
                                  Created_Date = x.Created_Date,
                                  Convert_From_Stock_Code = x.Convert_From_Stock_Code,
                                  ConvertedList = (from y in DB.Stock_Convertion_Tracking
                                                   where y.Convert_From_Stock_Code == x.Stock_Code
                                                   select new StockConvertedEntity
                                                   {
                                                       Stock_Convertion_Id = y.Stock_Convertion_Id,
                                                       Stock_Id = y.Stock_Convertion_Id,
                                                       Stock_Code = y.Stock_Code,
                                                       DC_id = y.DC_id,
                                                       DC_Code = y.DC_Code,
                                                       SKU_Id = y.SKU_Id,
                                                       SKU_Name = y.SKU_Name,
                                                       SKU_Type = y.SKU_Type,
                                                       Grade = y.Grade,
                                                       Stock_Qty = y.Stock_Qty,
                                                       UOM = y.UOM,
                                                       Type = y.Type,
                                                       Created_By = y.Created_By,
                                                       Created_Date = y.Created_Date,
                                                       Convert_From_Stock_Code = y.Convert_From_Stock_Code,
                                                   }).ToList()
                              }).ToList();

            return returnList;
        }

        //public double? Dispatch { get; set; }

        //public double? Wastage { get; set; }
    }
}
