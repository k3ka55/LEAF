using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel;
//using DataModelLocal;
//using DataModel;
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
    public class Material_Sync_Another : IMaterial_Sync_Another
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
        //LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string fileName = "Material_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public Material_Sync_Another()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getMaterial()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.materialRecords = (from x in CloudDB.Material_Master
                                                   where x.Is_Sync == false && x.Is_Deleted == false
                                                   select new Material_MasterEntity
                                                   {
                                                       Material_Id = x.Material_Id,
                                                       Material_Auto_Gen_Code = x.Material_Auto_Gen_Code,
                                                       Material_Code = x.Material_Code,
                                                       Material_Name = x.Material_Name,
                                                       DC_Id = x.DC_Id,
                                                       DC_Code = x.DC_Code,
                                                       Location_Id = x.Location_Id,
                                                       Location_Code = x.Location_Code,
                                                       Region_Id = x.Region_Id,
                                                       Region = x.Region,
                                                       HSN_Code = x.HSN_Code,
                                                       CGST = x.CGST,
                                                       SGST = x.SGST,
                                                       Total_GST = x.Total_GST,
                                                       SKU_Id = x.SKU_Id,
                                                       SKU_Code = x.SKU_Code,
                                                       SKU_Name = x.SKU_Name,
                                                       SKU_Type_Id = x.SKU_Type_Id,
                                                       SKU_Type = x.SKU_Type,
                                                       SKU_SubType_Id = x.SKU_SubType_Id,
                                                       SKU_SubType = x.SKU_SubType,
                                                       Pack_Type_Id = x.Pack_Type_Id,
                                                       Pack_Type = x.Pack_Type,
                                                       Pack_Size = x.Pack_Size,
                                                       Pack_Weight_Type_Id = x.Pack_Weight_Type_Id,
                                                       Pack_Weight_Type = x.Pack_Weight_Type,                                                      
                                                       UOM = x.UOM,
                                                       Grade = x.Grade,
                                                       Reason = x.Reason,
                                                       Is_Deleted = x.Is_Deleted,
                                                       CreatedDate = x.CreatedDate,
                                                       CreateBy = x.CreateBy
                                                   }).ToList();

                int count = STI_PUSH_ENTITY.materialRecords.Count;
                int alcount = STI_PUSH_ENTITY.MaterialNumGenClass.Count;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Fetched from Material Master and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                
                scope.Complete();
            }
        }


        public void MaterialSet()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.materialRecords != null)
            {
                foreach (var list in STI_PUSH_ENTITY.materialRecords)
                {
                    var moveMaterial = new Material_Master();
                    using (var scope1 = new TransactionScope())
                    {
                        
                        var moveMaterial1 = CloudDB.Material_Master.Where(x => x.Material_Auto_Gen_Code == list.Material_Auto_Gen_Code).FirstOrDefault();

                        if (moveMaterial1 != null)
                        {
                            CloudDB.Material_Master.Remove(moveMaterial1);
                            CloudDB.SaveChanges();
                        }                       

                            moveMaterial.Material_Code = list.Material_Code;
                            moveMaterial.Material_Name = list.Material_Name;
                            moveMaterial.DC_Id = list.DC_Id;
                            moveMaterial.DC_Code = list.DC_Code;
                            moveMaterial.Location_Id = list.Location_Id;
                            moveMaterial.Location_Code = list.Location_Code;
                            moveMaterial.Region_Id = list.Region_Id;
                            moveMaterial.Region = list.Region;
                            moveMaterial.SKU_Id = list.SKU_Id;
                            moveMaterial.SKU_Code = list.SKU_Code;
                            moveMaterial.SKU_Name = list.SKU_Name;
                            moveMaterial.SKU_Type_Id = list.SKU_Type_Id;
                            moveMaterial.SKU_Type = list.SKU_Type;
                            moveMaterial.HSN_Code = list.HSN_Code;
                            moveMaterial.CGST = list.CGST;
                            moveMaterial.SGST = list.SGST;
                            moveMaterial.Total_GST = list.Total_GST;
                            moveMaterial.SKU_SubType_Id = list.SKU_SubType_Id;
                            moveMaterial.SKU_SubType = list.SKU_SubType;
                            moveMaterial.Pack_Type_Id = list.Pack_Type_Id;
                            moveMaterial.Pack_Type = list.Pack_Type;
                            moveMaterial.Pack_Size = list.Pack_Size;
                            moveMaterial.Pack_Weight_Type_Id = list.Pack_Weight_Type_Id;
                            moveMaterial.Pack_Weight_Type = list.Pack_Weight_Type;
                            moveMaterial.UOM = list.UOM;
                            moveMaterial.Grade = list.Grade;
                            moveMaterial.Reason = list.Reason;
                            moveMaterial.Is_Deleted = list.Is_Deleted;
                            moveMaterial.UpdateDate = list.UpdateDate;
                            moveMaterial.UpdateBy = list.UpdateBy;
                            moveMaterial.CreatedDate = list.CreatedDate;
                            moveMaterial.CreateBy = list.CreateBy;
                            moveMaterial.Material_Auto_Gen_Code = list.Material_Auto_Gen_Code;
                            moveMaterial.Is_Sync = false;
                            moveMaterial.Is_Deleted = list.Is_Deleted;

                            CloudDB.Material_Master.Add(moveMaterial);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }
                        int csiId = moveMaterial.Material_Id;
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(moveMaterial.Material_Auto_Gen_Code + " Inserted into MaterialMaster. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }
                    
                    lcount++;
                }
                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into MaterialMaster. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }

            if (STI_PUSH_ENTITY.MaterialNumGenClass != null)
            {
                int alcount = 0; 
                foreach (var autoNums in STI_PUSH_ENTITY.MaterialNumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.Material_Master_Num_Gen.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.Material_Master_Last_Number = autoNums.Material_Master_Last_Number;

                            CloudDB.Entry(list).State = EntityState.Modified;
                            CloudDB.SaveChanges();
                        }

                        scope3.Complete();
                    }
                    alcount++;
                }
                int count = alcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                STI_PUSH_ENTITY.MaterialNumGenClass = null;
            }
        }


        public void Material_Update()
        {
            int lcount = 0;
            foreach (var disp in STI_PUSH_ENTITY.materialRecords)
            {
                using (var scope3 = new TransactionScope())
                {
                    Material_Master dispUpdate = CloudDB.Material_Master.Where(x => x.Material_Id == disp.Material_Id).FirstOrDefault();

                    dispUpdate.Is_Sync = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                lcount++;
            }
            int count = lcount;

            using (var tw = new StreamWriter(Path, true))
            {
                tw.WriteLine(count + " Records Updated in MaterialMaster. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                tw.Close();
            }
            STI_PUSH_ENTITY.materialRecords = null;
        }
    }
}
