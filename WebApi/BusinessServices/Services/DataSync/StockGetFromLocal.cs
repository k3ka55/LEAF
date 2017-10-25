using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel;
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
    public class StockGetFromLocal : IStockGetFromLocal
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        //LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string fileName = "Stock_" + DateTime.Now.ToString("dd-MM-yyyy");
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();
        string Path;
        public StockGetFromLocal()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getSTK()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.stockrecord = (from x in CloudDB.Stock_Convertion_Tracking
                                                  where x.Is_Syunc == false
                                                  select new StockConvertionEntity
                                                  {
                                                      Stock_Convertion_Id = x.Stock_Convertion_Id,
                                                      Stock_Id = x.Stock_Id,
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
                                                      Updated_By = x.Updated_By,
                                                      Created_Date = x.Created_Date,
                                                      Updated_Date = x.Updated_Date,
                                                      Convert_From_Stock_Code = x.Convert_From_Stock_Code,
                                                      Is_Syunc = x.Is_Syunc
                                                  }).ToList();
                
                scope.Complete();
                int count = STI_PUSH_ENTITY.stockrecord.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from Stock. " + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void STK_Single_Record_Update(int stk_id)
        {
            using (var scope3 = new TransactionScope())
            {
                Stock_Convertion_Tracking dispUpdate = CloudDB.Stock_Convertion_Tracking.Where(x => x.Stock_Convertion_Id == stk_id).FirstOrDefault();

                dispUpdate.Is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine("Stock Convertion Tracking ID = "+stk_id + " Sync Field Updated in Physical Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void STK_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.stockrecord)
            {
                using (var scope3 = new TransactionScope())
                {
                    Stock_Convertion_Tracking dispUpdate = CloudDB.Stock_Convertion_Tracking.Where(x => x.Stock_Convertion_Id == disp.Stock_Convertion_Id).FirstOrDefault();

                    dispUpdate.Is_Syunc = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                lcount++;
            }
            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Updated in Stock Convertion Tracking. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }
            STI_PUSH_ENTITY.stockrecord = null;
        }

    }
}
