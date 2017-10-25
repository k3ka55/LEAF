using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel.UnitOfWork;
using DataModelLocal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace BusinessServices.Services.DataSync
{
    public class StockSetToLive : IStockSetToLive
    {
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        StockGetFromLocal csAnother = new StockGetFromLocal();
        string fileName = "Stock_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        string stockNumber;
        
        public StockSetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void setSTK()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.stockrecord != null)
            {
                foreach (var stkfromLive in STI_PUSH_ENTITY.stockrecord)
                {
                    var ExistData = CloudDB.Stocks.Where(x => x.DC_Code == stkfromLive.DC_Code && x.SKU_Name == stkfromLive.SKU_Name && x.SKU_Type == stkfromLive.SKU_Type && x.Grade == stkfromLive.Grade).FirstOrDefault();
                    if (ExistData == null)
                    {
                        var stockDet = new Stock();
                        using (var scope1 = new TransactionScope())
                        {
                            using (var scope5 = new TransactionScope())
                            {
                                string locationID = stkfromLive.DC_Code;
                                Stock_Code_Num_Gen autoinc = CloudDB.Stock_Code_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
                                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                                string prefix = rm.GetString("STKT");
                                string locationId = autoinc.DC_Code;
                                string year = autoinc.Financial_Year;
                                int? incNumber = autoinc.Stock_Last_Number;
                                stockNumber = prefix + "/" + locationId + "/" + year + "/" + String.Format("{0:00000}", incNumber);
                                int? incrementedValue = incNumber + 1;
                                autoinc.Stock_Last_Number = incrementedValue;
                                CloudDB.Entry(autoinc).State = EntityState.Modified;
                                CloudDB.SaveChanges();
                                scope5.Complete();
                            }

                            stockDet.Stock_code = stockNumber;
                            stockDet.DC_Code = stkfromLive.DC_Code;
                            stockDet.DC_Name = CloudDB.DC_Master.Where(x => x.DC_Code == stkfromLive.DC_Code).Select(x => x.Dc_Name).FirstOrDefault();
                            stockDet.SKU_Code = CloudDB.SKU_Master.Where(x => x.SKU_Id == stkfromLive.SKU_Id).Select(x => x.SKU_Code).FirstOrDefault();
                            stockDet.SKU_Name = stkfromLive.SKU_Name;
                            var skuType = ListHelper.SKU_Type().Where(x => x.SKU_Type_Name.ToLower().Trim() == stkfromLive.SKU_Type.ToLower().Trim()).FirstOrDefault();
                            stockDet.SKU_Type_Id = skuType.SKU_Type_Id;
                            stockDet.SKU_Type = stkfromLive.SKU_Type;
                            stockDet.Closing_Qty = stkfromLive.Stock_Qty;
                            stockDet.UOM = stkfromLive.UOM;
                            stockDet.Grade = stkfromLive.Grade;
                            stockDet.CreateBy = stkfromLive.Created_By;
                            stockDet.CreatedDate = stkfromLive.Created_Date;
                            
                            CloudDB.Stocks.Add(stockDet);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                            using (var tw = new StreamWriter(Path, true))
                            {
                                tw.WriteLine(stockDet.Stock_code + " Inserted into Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                                tw.Close();
                            }
                        }
                        
                        csAnother.STK_Single_Record_Update(stkfromLive.Stock_Convertion_Id);
                        lcount++;
                    }
                    else
                    {

                        if (stkfromLive.Type == "ORIGINAL")
                        {
                            using (var scope1 = new TransactionScope())
                            {
                                ExistData.Closing_Qty = 0;
                                CloudDB.Entry(ExistData).State = EntityState.Modified;
                                CloudDB.SaveChanges();
                                scope1.Complete();
                            }                            
                        }
                        else
                        {
                            using (var scope1 = new TransactionScope())
                            {
                                ExistData.Closing_Qty = ExistData.Closing_Qty + stkfromLive.Stock_Qty;
                                //ExistData.Closing_Qty = decimal.Round(ExistData.Closing_Qty, 2, MidpointRounding.AwayFromZero);
                                CloudDB.Entry(ExistData).State = EntityState.Modified;
                                CloudDB.SaveChanges();
                                scope1.Complete();
                            }
                        }                     
                        
                                                
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(ExistData.Stock_code + " Updated into Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        csAnother.STK_Single_Record_Update(stkfromLive.Stock_Convertion_Id);
                        lcount++;
                    }
                }
                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into Physical Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }
    }
}
