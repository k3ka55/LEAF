using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using DataModelLocal;
using DataModel;
using System.Threading.Tasks;
using System.Transactions;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class PHYSTKGetFromLocal : IPHYSTKGetFromLocal
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        //LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string fileName = "PhysicalStock_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public PHYSTKGetFromLocal()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getPHYSTK()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.physicalrecord = (from x in CloudDB.Physical_Stock
                                                  where x.is_Deleted == false && x.is_Syunc == false
                                                  select new PhysicalStockEntity
                                                  {
                                                      Phy_Stock_Id = x.Phy_Stock_Id,
                                                      Phy_Stock_code = x.Phy_Stock_code,
                                                      DC_id = x.DC_id,
                                                      DC_Code = x.DC_Code,
                                                      DC_Name = x.DC_Name,
                                                      Supplier_Id = x.Supplier_Id,
                                                      Supplier_Code = x.Supplier_Code,
                                                      Supplier_Name = x.Supplier_Name,
                                                      SKU_Id = x.SKU_Id,
                                                      SKU_Code = x.SKU_Code,
                                                      SKU_Name = x.SKU_Name,
                                                      SKU_Type_Id = x.SKU_Type_Id,
                                                      SKU_Type = x.SKU_Type,
                                                      Pack_Type_Id = x.Pack_Type_Id,
                                                      Pack_Size = x.Pack_Size,
                                                      Pack_Weight_Type_Id = x.Pack_Weight_Type_Id,
                                                      Pack_Weight_Type = x.Pack_Weight_Type,
                                                      Pack_Type = x.Pack_Type,
                                                      Closing_Qty = x.Closing_Qty,
                                                      UOM = x.UOM,
                                                      Grade = x.Grade,
                                                      Closing_Date_Time = x.Closing_Date_Time,
                                                      Aging = x.Aging,
                                                      Floor_Supervisor = x.Floor_Supervisor,
                                                      CreatedDate = x.CreatedDate,
                                                      CreateBy = x.CreateBy,
                                                      UpdateBy = x.UpdateBy,
                                                      UpdateDate = x.UpdateDate,
                                                      is_Deleted = x.is_Deleted
                                                  }).ToList();
                scope.Complete();
                int count = STI_PUSH_ENTITY.physicalrecord.Count;
                
                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from Physical Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
         }

        public void PHK_Single_Record_Update(string phl_stk_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Physical_Stock dispUpdate = CloudDB.Physical_Stock.Where(x => x.Phy_Stock_code == phl_stk_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(phl_stk_number + " Sync Field Updated in Physical Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void PHY_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.physicalrecord)
            {
                using (var scope3 = new TransactionScope())
                {
                    Physical_Stock dispUpdate = CloudDB.Physical_Stock.Where(x => x.Phy_Stock_Id == disp.Phy_Stock_Id).FirstOrDefault();

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
                tw.WriteLine(count + " Records Updated in Physical Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }
            STI_PUSH_ENTITY.physicalrecord = null;
        }

    }
}
