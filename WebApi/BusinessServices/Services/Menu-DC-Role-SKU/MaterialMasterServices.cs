using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices.Services
{
    public class MaterialMasterServices : IMaterialMasterServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;

        public MaterialMasterServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<Material_MasterEntity> CheckMaterialMaster(MaterialList material)
        {
            List<Material_MasterEntity> Intersect = new List<Material_MasterEntity>();
            List<Material_MasterEntity> Except = new List<Material_MasterEntity>();
            List<Material_MasterEntity> result = new List<Material_MasterEntity>();
            List<Material_MasterEntity> RawB = material.MaterialMaster.ToList();
            bool? EditVal = null;
            if (material.FromScreen == "EDIT")
            {
                //  int lineCount = 1;
                var yy = material.MaterialMaster.FirstOrDefault();
                
                int mic =
                    (from mm in DB.Material_Master
                     where mm.Is_Deleted != true && (mm.DC_Code == yy.DC_Code && mm.Location_Code == yy.Location_Code)
                     && mm.Region == yy.Region
                     && mm.Material_Id == yy.Material_Id

                     && mm.SKU_Type == yy.SKU_Type
                     && mm.SKU_Type_Id == yy.SKU_Type_Id
                     && mm.SKU_Id == yy.SKU_Id
                     && mm.SKU_Name == yy.SKU_Name
                     && mm.SKU_SubType_Id == yy.SKU_SubType_Id
                     && mm.SKU_SubType == yy.SKU_SubType
                     && mm.Grade == yy.Grade
                     && mm.UOM == yy.UOM
                     && mm.Pack_Type_Id == yy.Pack_Type_Id
                     && mm.Pack_Type == yy.Pack_Type
                     && mm.Pack_Size == yy.Pack_Size
                     && mm.Pack_Weight_Type_Id == yy.Pack_Weight_Type_Id
                     && mm.Pack_Weight_Type == yy.Pack_Weight_Type
                     select mm).Count();
                int ms = 0;
                if (mic == 0)
                {
                    ms = (from mm in DB.Material_Master
                          where mm.Is_Deleted != true && (mm.Location_Code == yy.Location_Code && mm.DC_Code == yy.DC_Code)
                          && mm.Region == yy.Region
                          && mm.SKU_Type == yy.SKU_Type
                          && mm.SKU_Type_Id == yy.SKU_Type_Id
                          && mm.SKU_Id == yy.SKU_Id
                          && mm.SKU_Name == yy.SKU_Name
                          && mm.SKU_SubType_Id == yy.SKU_SubType_Id
                          && mm.SKU_SubType == yy.SKU_SubType
                          && mm.Grade == yy.Grade
                          && mm.UOM == yy.UOM
                          && mm.Pack_Type_Id == yy.Pack_Type_Id
                          && mm.Pack_Type == yy.Pack_Type
                          && mm.Pack_Size == yy.Pack_Size
                          && mm.Pack_Weight_Type_Id == yy.Pack_Weight_Type_Id
                          && mm.Pack_Weight_Type == yy.Pack_Weight_Type
                          select mm).Count();
                }

                if (mic > 0)
                    EditVal = true;
                else if (mic == 0)
                {
                    if (ms == 0)
                        EditVal = true;
                    if (ms > 0)
                        EditVal = false;
                }

                Material_MasterEntity Ex = new Material_MasterEntity();
                Ex.ValidEdit = EditVal.Value;
                result.Add(Ex);

                return result;
            }

            else
            {
                int lineCount = 1;
                foreach (var y in RawB)
                {
                    int mic =
                        (from mm in DB.Material_Master
                         where mm.Is_Deleted != true && (mm.DC_Code == y.DC_Code && mm.Location_Code == y.Location_Code)
                         && mm.Region == y.Region
                         && mm.SKU_Type == y.SKU_Type
                         && mm.SKU_Type_Id == y.SKU_Type_Id
                         && mm.SKU_Id == y.SKU_Id
                         && mm.SKU_Name == y.SKU_Name
                         && mm.SKU_SubType_Id == y.SKU_SubType_Id
                         && mm.SKU_SubType == y.SKU_SubType
                         && mm.Grade == y.Grade
                         && mm.UOM == y.UOM
                         && mm.Pack_Type_Id == y.Pack_Type_Id
                         && mm.Pack_Type == y.Pack_Type
                         && mm.Pack_Size == y.Pack_Size
                         && mm.Pack_Weight_Type_Id == y.Pack_Weight_Type_Id
                         && mm.Pack_Weight_Type == y.Pack_Weight_Type
                         select mm).Count();
                    y.LineNumber = lineCount;
                    
                    if (mic > 0)
                        Intersect.Add(y);
                    else
                    {
                        Except.Add(y);
                    }
                    lineCount += 1;

                }
                Material_MasterEntity Ex = new Material_MasterEntity();
                Ex.InvalidDatas = Intersect.ToList();
                Ex.InvalidLineNumbers = Ex.InvalidDatas.Select(a => a.LineNumber).ToList();
                Ex.ValidDatas = Except.ToList();
                result.Add(Ex);
                return result;
            }
   
        }



        public List<MIExcelFields> ExcelImportFromMI(fileImportMI fileDetail)
        {
            List<MIExcelFields> stilist = new List<MIExcelFields>();
            List<MIExcelFields> milist = new List<MIExcelFields>();
            //List<MIExcelFields> miilist = new List<MIExcelFields>();
            List<MIExcelFields> result = new List<MIExcelFields>();
            MIExcelFields miDetail = new MIExcelFields();

            string Profilepicname = "MI_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/MI";
            string dirCreatePath = "";
            try
            {
                string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                string ext = ".xlsx";
                dirCreatePath = RootPath;

                if (!Directory.Exists(dirCreatePath))
                {
                    Directory.CreateDirectory(RootPath);
                }
                sPath = RootPath;
                name = Profilepicname + ext;
                vPath = sPath + "\\" + name;

                if (File.Exists(vPath))
                {
                    File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
                }
                else
                {
                    File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
                }

                FileStream stream = File.Open(vPath, FileMode.Open, FileAccess.Read);
                stream.Position = 0;
                IExcelDataReader excelReader = null;

                if (ext == ".xls")
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (ext == ".xlsx")
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                int columnCount = result1.Tables[0].Columns.Count;

                if (columnCount > 8)
                {
                    miDetail.status = false;
                    miDetail.Message = "Extra Column's Present in Given Excel";
                    stilist.Add(miDetail);
                    return stilist;
                }

                //--------------------validating Excel Columns
                int lineCount = 1;

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        Material_Master ci = new Material_Master();
                        //object[] A = { };

                        ci.DC_Code = fileDetail.LocationCode;
                        ci.DC_Code = fileDetail.DCCode;
                        ci.Region = fileDetail.Region;
                        ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        ci.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        ci.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        ci.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        ci.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ci.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        // ci.SP = d["SP"] != null && d["SP"].ToString() != "" ? double.Parse(d["SP"].ToString()) : 0;
                        //
                        using (var iscope = new TransactionScope())
                        {
                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == ci.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            if (subtypedetail != null)
                            {
                                ci.SKU_SubType = subtypedetail.SKU_SubType_Name;
                                ci.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;
                            }

                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "SKU_SubType_Name";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var skuDetail = DB.Material_Master.Where(dd => dd.SKU_Type == fileDetail.SKUType && (dd.Location_Code == fileDetail.LocationCode || dd.DC_Code == fileDetail.DCCode) && dd.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim() && dd.Pack_Weight_Type.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim() && dd.Pack_Type.ToLower().Trim() == ci.Pack_Type.ToLower().Trim() && dd.Pack_Size.ToLower().Trim() == ci.Pack_Size.ToLower().Trim() && dd.Grade.ToLower().Trim() == ci.Grade.ToLower().Trim() && dd.UOM.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
                            if (skuDetail != null)
                            {
                                ci.SKU_Name = skuDetail.SKU_Name;
                                ci.SKU_Id = skuDetail.SKU_Id;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.Message = "Line Item is not available in Material Master.Line Id:" + lineCount;
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
                            if (uomDetail != null)
                            {
                                ci.UOM = uomDetail.Unit_Name;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "UOM";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ci.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            if (packtypedetail != null)
                            {
                                ci.Pack_Type = packtypedetail.Pack_Type_Name;
                                ci.Pack_Type_Id = packtypedetail.Pack_Type_Id;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "Pack_Type_Name";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            if (packweightType != null)
                            {
                                ci.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                                ci.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "Pack_Weight_Type";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ci.Grade.ToLower().Trim()).FirstOrDefault();
                            if (gradeDetail != null)
                            {
                                ci.Grade = gradeDetail.GradeType_Name;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "Grade";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ci.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            if (packsizedetail != null)
                            {
                                ci.Pack_Size = packsizedetail.Pack_Size_Value;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "Pack_Size_Value";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            iscope.Complete();
                        }

                        lineCount += 1;
                    }

                if (result1 != null)

                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        MIExcelFields misDetail = new MIExcelFields();
                        misDetail.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        misDetail.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        misDetail.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        misDetail.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        misDetail.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        misDetail.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        misDetail.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        misDetail.SP = d["SP"] != null && d["SP"].ToString() != "" ? double.Parse(d["SP"].ToString()) : 0;

                        using (var iscope = new TransactionScope())
                        {

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == misDetail.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            misDetail.SKU_Name = skuDetail.SKU_Name;
                            misDetail.SKU_ID = skuDetail.SKU_Id;

                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == misDetail.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            misDetail.SKU_SubType = subtypedetail.SKU_SubType_Name;
                            misDetail.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == misDetail.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            misDetail.Pack_Type = packtypedetail.Pack_Type_Name;
                            misDetail.Pack_Type_Id = packtypedetail.Pack_Type_Id;


                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == misDetail.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            misDetail.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                            misDetail.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == misDetail.Grade.ToLower().Trim()).FirstOrDefault();
                            misDetail.Grade = gradeDetail.GradeType_Name;

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == misDetail.UOM.ToLower().Trim()).FirstOrDefault();
                            misDetail.UOM = uomDetail.Unit_Name;

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == misDetail.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            misDetail.Pack_Size = packsizedetail.Pack_Size_Value;

                            misDetail.SP = misDetail.SP;

                            stilist.Add(misDetail);

                            iscope.Complete();
                        }

                        lineCount += 1;

                    }

                excelReader.Close();

                var t = stilist.ToList();
                var materialRaw = (from mm in DB.Material_Master
                                   where mm.SKU_Type == fileDetail.SKUType
                                   select mm).ToList();
                if (fileDetail.DCCode != "NULL")
                {
                    materialRaw = materialRaw.Where(d => d.DC_Code == fileDetail.DCCode).ToList();
                }
                if (fileDetail.LocationCode != "NULL")
                {
                    materialRaw = materialRaw.Where(d => d.Location_Code == fileDetail.LocationCode).ToList();
                }

                foreach (var y in t)
                {
                    int mic = materialRaw.Where(d => d.SKU_Id == y.SKU_ID && d.SKU_Name == y.SKU_Name && d.Pack_Weight_Type_Id == y.Pack_Weight_Type_Id && d.Pack_Weight_Type == y.Pack_Weight_Type && d.Pack_Type == y.Pack_Type && d.Pack_Size == y.Pack_Size && d.Grade == y.Grade && d.UOM == y.UOM).ToList().Count();
                    if (mic > 0)
                        result.Add(y);
                    else
                    {
                        result.Add(y);
                        return result;
                    }


                }


            }
            catch (Exception e)
            {
                stilist = new List<MIExcelFields>();
                miDetail.status = false;
                miDetail.Message = e.Message.ToString();
                stilist.Add(miDetail);
                return stilist;
            }
            stilist[0].status = true;
            stilist[0].Message = "Success";

            return result;
        }

        public Material_MasterEntity get(string id)
        {
            var list = (from x in DB.Material_Master
                        where x.Material_Auto_Gen_Code == id && x.Is_Deleted == false
                        orderby x.SKU_Name
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
                            SKU_Id = x.SKU_Id,
                            SKU_Code = x.SKU_Code,
                            SKU_Name = x.SKU_Name,
                            HSN_Code = x.HSN_Code,
                            CGST = x.CGST,
                            SGST = x.SGST,
                            Total_GST = x.Total_GST,
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
                            CreatedDate = x.CreatedDate,
                            CreateBy = x.CreateBy
                        }).FirstOrDefault();

            return list;
        }

        public List<Material_MasterEntity> searchMatrerial(int? roleId, int reagionid, int locationid, int dcid, int skuTypeid, string Url)
        {
            List<Material_MasterEntity> returnList = new List<Material_MasterEntity>();
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


            var list = (from x in DB.Material_Master
                        where x.Is_Deleted == false
                        orderby x.SKU_Name
                        select
                        //x).AsEnumerable().Select(x => 
                        new Material_MasterEntity
                        {
                            Material_Id = x.Material_Id,
                            Material_Code = x.Material_Code,
                            Material_Auto_Gen_Code = x.Material_Auto_Gen_Code,
                            Material_Name = x.Material_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            SKU_Id = x.SKU_Id,
                            SKU_Code = x.SKU_Code,
                            SKU_Name = x.SKU_Name,
                            HSN_Code = x.HSN_Code,
                            CGST = x.CGST,
                            SGST = x.SGST,
                            Total_GST = x.Total_GST,
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
                            CreatedDate = x.CreatedDate,
                            CreateBy = x.CreateBy
                        }).ToList();
        
            if (locationid != 0 && dcid == 0)
            {
                var filtered = list.Where(x => x.Region_Id == reagionid && x.Location_Id == locationid && x.SKU_Type_Id == skuTypeid).ToList();

                foreach (var li in filtered)
                {
                    returnList.Add(li);
                }
            }
            else if (locationid == 0 && dcid != 0)
            {
                var filtered = list.Where(x => x.Region_Id == reagionid && x.DC_Id == dcid && x.SKU_Type_Id == skuTypeid).ToList();

                foreach (var li in filtered)
                {
                    returnList.Add(li);
                }
            }
            //foreach (var t in returnList)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return returnList;
        }

        public List<Material_MasterEntity> searchMatrerial(int reagionid, int locationid, int dcid, int skuTypeid)
        {
            List<Material_MasterEntity> returnList = new List<Material_MasterEntity>();
            
            var list = (from x in DB.Material_Master
                        where x.Is_Deleted == false
                        orderby x.SKU_Name
                        select 
                       // x).AsEnumerable().Select(x => 
                            new Material_MasterEntity
                        {
                            Material_Id = x.Material_Id,
                            Material_Code = x.Material_Code,
                            Material_Auto_Gen_Code = x.Material_Auto_Gen_Code,
                            Material_Name = x.Material_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            SKU_Id = x.SKU_Id,
                            SKU_Code = x.SKU_Code,
                            SKU_Name = x.SKU_Name,
                            HSN_Code=x.HSN_Code,
                            CGST=x.CGST,
                            SGST=x.SGST,
                            Total_GST = x.Total_GST,
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
                            CreatedDate = x.CreatedDate,
                            CreateBy = x.CreateBy
                        }).ToList();

            if (locationid != 0 && dcid == 0)
            {
                var filtered = list.Where(x => x.Region_Id == reagionid && x.Location_Id == locationid && x.SKU_Type_Id == skuTypeid).ToList();

                foreach (var li in filtered)
                {
                    returnList.Add(li);
                }
            }
            else if (locationid == 0 && dcid != 0)
            {
                var filtered = list.Where(x => x.Region_Id == reagionid && x.DC_Id == dcid && x.SKU_Type_Id == skuTypeid).ToList();

                foreach (var li in filtered)
                {
                    returnList.Add(li);
                }
            }

            return returnList;
        }

        public Material_Master_Num_Gen GetMaterialMasterAutoIncrement(string locationId)
        {
            var autoinc = DB.Material_Master_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();

            var model = new Material_Master_Num_Gen
            {
                Material_Master_Num_Gen_Id = autoinc.Material_Master_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Material_Master_Last_Number = autoinc.Material_Master_Last_Number
            };

            return model;
        }

        public bool createMaterial(MaterialList materialLists)
        {
            using (var scope = new TransactionScope())
            {
                foreach (var materialEntity in materialLists.MaterialMaster)
                {

                    string mTNumber, STI_prefix;
                    int? incNumber;

                    string locationID = "";

                    using (var iscope = new TransactionScope())
                    {
                        if (materialEntity.Location_Code != null && materialEntity.Location_Code != "null")
                            locationID = materialEntity.Location_Code;
                        else if (materialEntity.DC_Code != null && materialEntity.DC_Code != "null")
                            locationID = materialEntity.DC_Code;

                        // string locationID = ciEntity.Location_Code;
                        ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                        STI_prefix = rm.GetString("MT");
                        Material_Master_Num_Gen autoIncNumber = GetMaterialMasterAutoIncrement(locationID);
                        locationID = autoIncNumber.DC_Code;
                        incNumber = autoIncNumber.Material_Master_Last_Number;
                        int? incrementedValue = incNumber + 1;
                        var STincrement = DB.Material_Master_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
                        STincrement.Material_Master_Last_Number = incrementedValue;
                        _unitOfWork.MaterialMasterNumGenRepository.Update(STincrement);
                        _unitOfWork.Save();
                        mTNumber = STI_prefix + "/" + locationID + "/" + String.Format("{0:00000}", incNumber);

                        iscope.Complete();
                    }

                    //
                    var materialList = new Material_Master
                    {
                        Material_Code = materialEntity.Material_Code,
                        Material_Auto_Gen_Code = mTNumber,
                        Material_Name = materialEntity.Material_Name,
                        DC_Id = materialEntity.DC_Id,
                        DC_Code = materialEntity.DC_Code,
                        Location_Id = materialEntity.Location_Id,
                        Location_Code = materialEntity.Location_Code,
                        Region_Id = materialEntity.Region_Id,
                        Region = materialEntity.Region,
                        HSN_Code=materialEntity.HSN_Code,
                        CGST=materialEntity.CGST,
                        SGST=materialEntity.SGST,
                        Total_GST=materialEntity.Total_GST,
                        SKU_Id = materialEntity.SKU_Id,
                        SKU_Code = materialEntity.SKU_Code,
                        SKU_Name = materialEntity.SKU_Name,
                        SKU_Type_Id = materialEntity.SKU_Type_Id,
                        SKU_Type = materialEntity.SKU_Type,
                        SKU_SubType_Id = materialEntity.SKU_SubType_Id,
                        SKU_SubType = materialEntity.SKU_SubType,
                        Pack_Type_Id = materialEntity.Pack_Type_Id,
                        Pack_Type = materialEntity.Pack_Type,
                        Pack_Size = materialEntity.Pack_Size,
                        Pack_Weight_Type_Id = materialEntity.Pack_Weight_Type_Id,
                        Pack_Weight_Type = materialEntity.Pack_Weight_Type,
                        UOM = materialEntity.UOM,
                        Grade = materialEntity.Grade,
                        Reason = materialEntity.Reason,
                        Is_Deleted = false,
                        CreatedDate = DateTime.UtcNow,
                        CreateBy = materialEntity.CreateBy,
                        Is_Sync = false,
                    };

                    _unitOfWork.MaterialMasterRepository.Insert(materialList);
                    _unitOfWork.Save();
                }
                scope.Complete();
                return true;
            }
           // return false;
        }

        public bool updateMaterialMaster(int id, Material_MasterEntity materialEntity)
        {
            if (materialEntity != null)
            {
                using (var scope = new TransactionScope())
                {

                    var m = _unitOfWork.MaterialMasterRepository.GetByID(id);
                    if (m != null)
                    {
                        m.Material_Code = materialEntity.Material_Code;
                        m.Material_Name = materialEntity.Material_Name;
                        m.DC_Id = materialEntity.DC_Id;
                        m.DC_Code = materialEntity.DC_Code;
                        m.Location_Id = materialEntity.Location_Id;
                        m.Location_Code = materialEntity.Location_Code;
                        m.Region_Id = materialEntity.Region_Id;
                        m.Region = materialEntity.Region;
                        m.HSN_Code=materialEntity.HSN_Code;
                        m.CGST=materialEntity.CGST;
                        m.SGST=materialEntity.SGST;
                        m.Total_GST = materialEntity.Total_GST;
                        m.SKU_Id = materialEntity.SKU_Id;
                        m.SKU_Code = materialEntity.SKU_Code;
                        m.SKU_Name = materialEntity.SKU_Name;
                        m.SKU_Type_Id = materialEntity.SKU_Type_Id;
                        m.SKU_Type = materialEntity.SKU_Type;
                        m.SKU_SubType_Id = materialEntity.SKU_SubType_Id;
                        m.SKU_SubType = materialEntity.SKU_SubType;
                        m.Pack_Type_Id = materialEntity.Pack_Type_Id;
                        m.Pack_Type = materialEntity.Pack_Type;
                        m.Pack_Size = materialEntity.Pack_Size;
                        m.Pack_Weight_Type_Id = materialEntity.Pack_Weight_Type_Id;
                        m.Pack_Weight_Type = materialEntity.Pack_Weight_Type;
                        m.UOM = materialEntity.UOM;
                        m.Grade = materialEntity.Grade;
                        m.Reason = materialEntity.Reason;
                        m.Is_Deleted = false;
                        m.UpdateDate = DateTime.UtcNow;
                        m.UpdateBy = materialEntity.UpdateBy;
                        m.Is_Sync = false;

                        _unitOfWork.MaterialMasterRepository.Update(m);
                        _unitOfWork.Save();

                        scope.Complete();
                        return true;
                    }
                }
            }
            return false;
        }



        public bool deleteMaterial(string id, string reason)
        {
            using (var scope = new TransactionScope())
            {
                var m = (from f in DB.Material_Master
                         where f.Material_Auto_Gen_Code == id
                         select f).FirstOrDefault();
                if (m != null)
                {
                    m.Reason = reason;
                    m.Is_Deleted = true;
                    m.Is_Sync = false;
                    _unitOfWork.MaterialMasterRepository.Update(m);
                    _unitOfWork.Save();

                    scope.Complete();
                    return true;
                }

            }
            return false;
        }
    }
}