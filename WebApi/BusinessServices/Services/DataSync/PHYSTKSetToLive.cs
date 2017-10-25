using System;
using System.Collections.Generic;
using System.Linq;
using DataModelLocal;
//using DataModel;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entity;
using System.Transactions;
using BusinessServices.Interfaces;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class PHYSTKSetToLive : IPHYSTKSetToLive
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string fileName = "PhysicalStock_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public PHYSTKSetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void setPHYSTK()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.physicalrecord != null)
            {
                foreach (var phyfromLive in STI_PUSH_ENTITY.physicalrecord)
                {
                    var ExistData = CloudDB.Physical_Stock.Where(x => x.DC_Code == phyfromLive.DC_Code && x.SKU_Id == phyfromLive.SKU_Id && x.SKU_Type_Id == phyfromLive.SKU_Type_Id && x.Grade == phyfromLive.Grade && x.Closing_Date_Time == phyfromLive.Closing_Date_Time).FirstOrDefault();
                    if (ExistData == null)
                    {
                        var movedPHYSTK = new Physical_Stock();
                        using (var scope1 = new TransactionScope())
                        {
                            movedPHYSTK.Phy_Stock_code = phyfromLive.Phy_Stock_code;
                            movedPHYSTK.DC_id = phyfromLive.DC_id;
                            movedPHYSTK.DC_Code = phyfromLive.DC_Code;
                            movedPHYSTK.DC_Name = phyfromLive.DC_Name;
                            movedPHYSTK.Supplier_Id = phyfromLive.Supplier_Id;
                            movedPHYSTK.Supplier_Code = phyfromLive.Supplier_Code;
                            movedPHYSTK.Supplier_Name = phyfromLive.Supplier_Name;
                            movedPHYSTK.SKU_Id = phyfromLive.SKU_Id;
                            movedPHYSTK.SKU_Code = phyfromLive.SKU_Code;
                            movedPHYSTK.SKU_Name = phyfromLive.SKU_Name;
                            movedPHYSTK.SKU_Type_Id = phyfromLive.SKU_Type_Id;
                            movedPHYSTK.SKU_Type = phyfromLive.SKU_Type;
                            movedPHYSTK.Pack_Type_Id = phyfromLive.Pack_Type_Id;
                            movedPHYSTK.Pack_Size = phyfromLive.Pack_Size;
                            movedPHYSTK.Pack_Weight_Type_Id = phyfromLive.Pack_Weight_Type_Id;
                            movedPHYSTK.Pack_Weight_Type = phyfromLive.Pack_Weight_Type;
                            movedPHYSTK.Pack_Type = phyfromLive.Pack_Type;
                            movedPHYSTK.Closing_Qty = phyfromLive.Closing_Qty;
                            movedPHYSTK.UOM = phyfromLive.UOM;
                            movedPHYSTK.Grade = phyfromLive.Grade;
                            movedPHYSTK.Closing_Date_Time = phyfromLive.Closing_Date_Time;
                            movedPHYSTK.Aging = phyfromLive.Aging;
                            movedPHYSTK.Floor_Supervisor = phyfromLive.Floor_Supervisor;
                            movedPHYSTK.CreatedDate = phyfromLive.CreatedDate;
                            movedPHYSTK.CreateBy = phyfromLive.CreateBy;
                            movedPHYSTK.UpdateBy = phyfromLive.UpdateBy;
                            movedPHYSTK.UpdateDate = phyfromLive.UpdateDate;
                            movedPHYSTK.is_Deleted = phyfromLive.is_Deleted;

                            CloudDB.Physical_Stock.Add(movedPHYSTK);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                            using (var tw = new StreamWriter(Path, true))
                            {
                                tw.WriteLine(movedPHYSTK.Phy_Stock_code + " Inserted into Physical Stock. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                                tw.Close();
                            }
                        }
                        lcount++;
                    }
                    else
                    {
                        PHYSTKGetFromLocal csAnother = new PHYSTKGetFromLocal();
                        csAnother.PHK_Single_Record_Update(phyfromLive.Phy_Stock_code);
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
