//using BusinessEntities;
//using DataModel;
//using DataModel.UnitOfWork;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Reflection;
//using System.Resources;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace BusinessServices
//{
//    public class StockTransferServices : IStockTransferServices
//    {
//        LEAFDBEntities DB = new LEAFDBEntities();
//        private readonly UnitOfWork _unitOfWork;

//        public StockTransferServices()
//        {
//            _unitOfWork = new UnitOfWork();
//        }


//        public List<StockEntity> GetSK(DateTime? startDate,DateTime? endDate, string Ulocation)
//        {
//            var qu = (from a in DB.Stocks
//                   //   where (a.Closing_Date_Time.Value.Year >= startDate.Value.Year && a.Closing_Date_Time.Value.Month >= startDate.Value.Month && a.Closing_Date_Time.Value.Day >= startDate.Value.Day) && (a.Closing_Date_Time.Value.Year <= endDate.Value.Year && a.Closing_Date_Time.Value.Month <= endDate.Value.Month && a.Closing_Date_Time.Value.Day <= endDate.Value.Day) && a.DC_Code == Ulocation
//                      select new StockEntity
//                      {
//                        Supplier_Id=a.Supplier_Id,
//                        Supplier_Name=a.Supplier_Name,
//                        Supplier_Code=a.Supplier_Code,
//                        //SKU_SubType_Id=a.SKU_SubType_Id,
//                        //SKU_SubType=a.SKU_SubType,
//                        SKU_Type_Id=a.SKU_Type_Id,
//                        SKU_Type=a.SKU_Type,
//                        //Pack_Type_Id=a.Pack_Type_Id,
//                        //Pack_Size=a.Pack_Size,
//                        //Pack_Weight_Type_Id=a.Pack_Weight_Type_Id,
//                        //Pack_Weight_Type=a.Pack_Weight_Type,
//                        //Pack_Type=a.Pack_Type,
//                        CreatedDate=DateTime.Now,
//                        UpdateDate=DateTime.Now,
//                        Stock_Id=a.Stock_Id,
//                        Stock_code=a.Stock_code,
//                        SKU_Id=a.SKU_Id,
//                        SKU_Code=a.SKU_Code,
//                        SKU_Name=a.SKU_Name,          
//                        DC_id=a.DC_id,
//                        DC_Code=a.DC_Code,     
//                        DC_Name=a.DC_Name,
//                        Grade=a.Grade,
//                        Closing_Qty=a.Closing_Qty,
//                        UOM=a.UOM,
//                        Aging=a.Aging,
//                        Closing_Date_Time=a.Closing_Date_Time                        
//                      }).ToList();
//            return qu;
//        }

//        public bool MoveStock()
//        {
//            using (var scope = new TransactionScope())
//            {
//                var grnline = (from k in DB.GRN_Creation
//                               join g in DB.GRN_Line_item on k.GRN_Number equals g.GRN_Number
//                               //group g by g.SKU_Name into m
//                               select new { k, g}).Where(s => s.g.moved == false).ToList();
//                var grp = (from grop in grnline
//                           group grop by grop.g.SKU_Name into m
//                           select new
//                           {
//                               Sum = m.Sum(s => s.g.Billed_Qty)
//                           }).ToList();
                
                
//                foreach (var grn in grnline)
//                {
//                    var stock = DB.Stocks.Where(x => x.SKU_Name == grn.g.SKU_Name).FirstOrDefault();

                   
//                        var check_grn_moved = from y in DB.GRN_Line_item
//                                              where y.SKU_Name==grn.g.SKU_Name && y.GRN_Line_Id == grn.g.GRN_Line_Id
//                                              select y;

//                        if (check_grn_moved.Any())
//                        {
//                            var totalGrnQty = (from x in DB.GRN_Line_item
//                                               where x.SKU_Name==grn.g.SKU_Name && x.GRN_Line_Id == grn.g.GRN_Line_Id
//                                               select x.Billed_Qty).Sum();

//                            var stk = DB.Stocks.Where(x => x.SKU_Name == grn.g.SKU_Name).FirstOrDefault();
//                            if (stk!=null)
//                            {
//                                var stkupdate = new Stock
//                                {
//                                    Stock_Id = stk.Stock_Id,
//                                    Closing_Qty = stk.Closing_Qty + totalGrnQty,
//                                    UpdateDate = DateTime.Now,
//                                    CreatedDate = DateTime.Now
//                                };
//                                    //stk.Stock_Id = stk.Stock_Id;
//                                    //stk.Closing_Qty = stk.Closing_Qty + totalGrnQty;
//                                    //stk.UpdateDate = DateTime.Now;
//                                    //stk.CreatedDate = DateTime.Now;
//                                _unitOfWork.StockRepository.Update(stkupdate);
//                                _unitOfWork.Save();
//                                    //DB.Entry(stk).State = EntityState.Modified;
//                                    //DB.SaveChanges();

//                                    var stransfer = new Stock_Transaction
//                                    {
//                                        Stock_Code = stk.Stock_code,
//                                        INW_id = grn.g.INW_id,
//                                        GRN_Line_Id = grn.g.GRN_Line_Id,
//                                        GRN_Number = grn.g.GRN_Number,
//                                        SKU_Id = grn.g.SKU_ID,
//                                        SKU_Code = grn.g.SKU_Code,
//                                        SKU_Name = grn.g.SKU_Name,
//                                        //SKU_SubType_Id=grn.g.SKU_SubType_Id,
//                                        //SKU_SubType=grn.g.SKU_SubType,
//                                        SKU_Type_Id=grn.k.SKU_Type_Id,
//                                        SKU_Type=grn.k.SKU_Type,
//                                        Supplier_ID = grn.k.Supplier_Id,
//                                        Supplier_Code=grn.k.Supplier_code,
//                                        Supplier_Name=grn.k.Supplier_Name,
//                                        Qty = stk.Closing_Qty,
//                                        UOM = stk.UOM,
//                                        //Pack_Type_Id =stk.Pack_Type_Id,
//                                        //Pack_Size =stk.Pack_Size,
//                                        //Pack_Weight_Type_Id=stk.Pack_Weight_Type_Id,
//                                        //Pack_Weight_Type=stk.Pack_Weight_Type,
//                                        //Pack_Type=stk.Pack_Type,
//                                       // Grade=stk.Grade,  
//                                        UpdateDate =DateTime.Now,
//                                        CreatedDate = DateTime.Now
//                                    };
//                                    _unitOfWork.StockTransationRepository.Insert(stransfer);
//                                    _unitOfWork.Save();
//                                    //DB.Stock_Transaction.Add(stransfer);
//                                    //DB.SaveChanges();
                                
//                            }
//                            else
//                            {
//                                string locationID = grn.k.DC_Code;
//                                Stock_Code_Num_Gen autoinc = DB.Stock_Code_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
//                                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
//                                string prefix = rm.GetString("STKI");
//                                string locationId = autoinc.DC_Code;
//                                int? incNumber = autoinc.Stock_Last_Number;
//                                string stockNumber =prefix + "/" + locationId + "/" + String.Format("{0:00000}", incNumber);

//                                var stockItem = new Stock
//                                {
//                                     Supplier_Id=grn.k.Supplier_Id,
//                                     Supplier_Name=grn.k.Supplier_Name,
//                                     Supplier_Code=grn.k.Supplier_code,
//                                     //SKU_SubType_Id=grn.g.SKU_SubType_Id,
//                                     //SKU_SubType=grn.g.SKU_SubType,
//                                     SKU_Type_Id=grn.k.SKU_Type_Id,
//                                     SKU_Type=grn.k.SKU_Type,
//                        //Pack_Type_Id=grn.g.Pack_Type_Id,
//                        //Pack_Size=grn.g.Pack_Size,
//                        //Pack_Weight_Type_Id=grn.g.Pack_Weight_Type_Id,
//                        //Pack_Weight_Type=grn.g.Pack_Weight_Type,
//                        //Pack_Type=grn.g.Pack_Type,                      
//                                     UpdateDate=DateTime.Now,                         
//                                     DC_Code=grn.k.DC_Code,                    
//                                     UOM=grn.g.UOM,                   
//                                     Stock_code = stockNumber,
//                                     SKU_Id = grn.g.SKU_ID,
//                                     SKU_Code = grn.g.SKU_Code,
//                                     SKU_Name = grn.g.SKU_Name,
//                                     Closing_Qty = totalGrnQty,
//                                     CreatedDate = DateTime.Now,
//                                };
//                                _unitOfWork.StockRepository.Insert(stockItem);
//                                _unitOfWork.Save();
//                                //DB.Stocks.Add(stockItem);
//                                //DB.SaveChanges();

//                                var stransfer = new Stock_Transaction
//                                {
//                                        Stock_Code = stockItem.Stock_code,
//                                        INW_id = grn.g.INW_id,
//                                        GRN_Line_Id = grn.g.GRN_Line_Id,
//                                        GRN_Number = grn.g.GRN_Number,
//                                        SKU_Id = grn.g.SKU_ID,
//                                        SKU_Code = grn.g.SKU_Code,
//                                        SKU_Name = grn.g.SKU_Name,
//                                        //SKU_SubType_Id=grn.g.SKU_SubType_Id,
//                                        //SKU_SubType=grn.g.SKU_SubType,
//                                        SKU_Type_Id=grn.k.SKU_Type_Id,
//                                        SKU_Type=grn.k.SKU_Type,
//                                        Supplier_ID = grn.k.Supplier_Id,
//                                        Supplier_Code=grn.k.Supplier_code,
//                                        Supplier_Name=grn.k.Supplier_Name, 
//                                        //Pack_Type_Id =stockItem.Pack_Type_Id,
//                                        //Pack_Size =stockItem.Pack_Size,
//                                        //Pack_Weight_Type_Id=stockItem.Pack_Weight_Type_Id,
//                                        //Pack_Weight_Type=stockItem.Pack_Weight_Type,
//                                        //Pack_Type=stockItem.Pack_Type,
//                                        //Grade=stockItem.Grade,  
//                                        UpdateDate =DateTime.Now,
//                                        CreatedDate = DateTime.Now,
//                                        Qty = stockItem.Closing_Qty,
//                                        UOM = stockItem.UOM
                                   
//                                };
//                                _unitOfWork.StockTransationRepository.Insert(stransfer);
//                                _unitOfWork.Save();
//                                //DB.Stock_Transaction.Add(stransfer);
//                                //DB.SaveChanges();

//                                int? incrementedValue = incNumber + 1;

//                                    autoinc.Stock_Last_Number = incrementedValue;
//                                    _unitOfWork.StockAutoIncrementRepository.Update(autoinc);
//                                    _unitOfWork.Save();

//                            }


//                            var grnUpdate = DB.GRN_Line_item.Where(x => x.GRN_Line_Id == grn.g.GRN_Line_Id && x.SKU_Name==grn.g.SKU_Name).FirstOrDefault();

//                            grnUpdate.moved = true;
                             
//                            DB.Entry(grnUpdate).State = EntityState.Modified;
//                            DB.SaveChanges();

//                        }
//                        else
//                        {

//                        }
                    
//                }
//                scope.Complete();
//            }

//            return true;
//        }

//    }
//}
