using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
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
    public class EstimatedStockServices : IEstimatedStockService
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public EstimatedStockServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Estimated_Stock_NUM_Generation GetAutoIncrement(string locationId)
        {
            var autoinc = DB.Estimated_Stock_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new Estimated_Stock_NUM_Generation
            {
                Est_Num_Gen_Id = autoinc.Est_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Est_Last_Number = autoinc.Est_Last_Number
            };

            return model;
        }
       


        //--------------------------------CREATE----------------------------------------
        public bool CreateEstimatedStock(EstimtedStkEntity estimatedEntity)
        {
            using (var scope = new TransactionScope())
            {
                var EstimatedStock = new Estimated_Stock();
                foreach (EstimatedStockEntity pSub in estimatedEntity.PhyStock)
                {
                    string prefix, locationId, poNumber,FY;
                    int? incNumber;
                    
                    string locationID = estimatedEntity.DC_Code;
                        ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                        prefix = rm.GetString("ESTT");
                        Estimated_Stock_NUM_Generation autoIncNumber = GetAutoIncrement(locationID);
                        locationId = autoIncNumber.DC_Code;
                        FY = autoIncNumber.Financial_Year;
                        incNumber = autoIncNumber.Est_Last_Number;
                        int? incrementedValue = incNumber + 1;
                        var increment = DB.Estimated_Stock_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                        increment.Est_Last_Number = incrementedValue;
                        _unitOfWork.EstimatedStockNumGenRepository.Update(increment);
                        _unitOfWork.Save();

                        poNumber = prefix + "/" + FY+ "/" + locationId + "/" + String.Format("{0:00000}", incNumber);
                    
                        EstimatedStock.Est_Stock_code = poNumber;
                    EstimatedStock.DC_id = estimatedEntity.DC_id;
                    EstimatedStock.DC_Code = estimatedEntity.DC_Code;
                    EstimatedStock.DC_Name = estimatedEntity.DC_Name;
                    EstimatedStock.Supplier_Id = pSub.Supplier_Id;
                    EstimatedStock.Supplier_Code = pSub.Supplier_Code;
                    EstimatedStock.Supplier_Name = pSub.Supplier_Name;
                    EstimatedStock.SKU_Id = pSub.SKU_Id;
                    EstimatedStock.SKU_Code = pSub.SKU_Code;
                    EstimatedStock.SKU_Name = pSub.SKU_Name;
                    EstimatedStock.SKU_Type_Id = pSub.SKU_Type_Id;
                    EstimatedStock.SKU_Type = pSub.SKU_Type;
                    EstimatedStock.Pack_Type_Id = pSub.Pack_Type_Id;
                    EstimatedStock.Pack_Size = pSub.Pack_Size;
                    EstimatedStock.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                    EstimatedStock.Pack_Weight_Type = pSub.Pack_Weight_Type;
                    EstimatedStock.Pack_Type = pSub.Pack_Type;
                    EstimatedStock.Closing_Qty = pSub.Closing_Qty;
                    EstimatedStock.UOM = pSub.UOM;
                    EstimatedStock.Grade = pSub.Grade;
                    EstimatedStock.Closing_Date_Time = estimatedEntity.Closing_Date_Time;
                    EstimatedStock.Aging = estimatedEntity.Closing_Date_Time.ToString();
                    EstimatedStock.Floor_Supervisor = estimatedEntity.Floor_Supervisor;
                    EstimatedStock.CreatedDate = DateTime.Now;
                    EstimatedStock.CreateBy = estimatedEntity.CreateBy;
                    //EstimatedStock.is_Syunc = false;
                    EstimatedStock.is_Deleted = false;
                    _unitOfWork.EstimatedStockRepository.Insert(EstimatedStock);
                    _unitOfWork.Save();
                }
                //

                scope.Complete();
            }
            return true;
        }

        public List<EstimatedStockEntity> ExcelImportESTK(fileImportEST fileDetail)
        {
            List<EstimatedStockEntity> stilist = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> milist = new List<EstimatedStockEntity>();
            //List<MIExcelFields> miilist = new List<MIExcelFields>();
            List<EstimatedStockEntity> result = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> result2 = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> result3 = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> result4 = new List<EstimatedStockEntity>();
            EstimatedStockEntity miDetail = new EstimatedStockEntity();

            string Profilepicname = "ESTK_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/ESTK";
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
                int lineCount = 2;

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        Estimated_Stock ci = new Estimated_Stock();
                      
                        ci.DC_Code = fileDetail.DC_Code;
                        ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        ci.SKU_Type = d["SKU_Type"] != null && d["SKU_Type"].ToString() != "" ? d["SKU_Type"].ToString() : "";
                        ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ci.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        ci.Closing_Qty = d["Qty"] != null && d["Qty"].ToString() != "" ? double.Parse(d["Qty"].ToString()) : 0;
                        //
                        using (var iscope = new TransactionScope())
                        {
                            var skutypedetail = ListHelper.SKU_Type().Where(x => x.SKU_Type_Name.ToLower().Trim() == ci.SKU_Type.ToLower().Trim()).FirstOrDefault();
                            if (skutypedetail != null)
                            {
                                ci.SKU_Type = skutypedetail.SKU_Type_Name;
                                ci.SKU_Type_Id = skutypedetail.SKU_Type_Id;
                            }

                            else
                            {
                                miDetail.status = false;
                                miDetail.lineNumber = lineCount;
                                miDetail.Message = "Error";
                                miDetail.errorItem = "SKU_Type_Name";
                                stilist.Add(miDetail);
                                return stilist;
                            }

                            var skuDetail = DB.SKU_Master.Where(dd => dd.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            if (skuDetail != null)
                            {
                                ci.SKU_Name = skuDetail.SKU_Name;
                                ci.SKU_Id = skuDetail.SKU_Id;
                            }
                            else
                            {
                                miDetail.status = false;
                                miDetail.Message = "SKU_Name is not available in SKU Master.Line Id:" + lineCount;
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

                            iscope.Complete();
                        }

                        lineCount += 1;
                    }

                if (result1 != null)

                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        EstimatedStockEntity misDetail = new EstimatedStockEntity();
                        misDetail.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        misDetail.SKU_Type = d["SKU_Type"] != null && d["SKU_Type"].ToString() != "" ? d["SKU_Type"].ToString() : "";
                        misDetail.DC_Code = fileDetail.DC_Code;
                        misDetail.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        misDetail.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        misDetail.Closing_Qty = d["Qty"] != null && d["Qty"].ToString() != "" ? double.Parse(d["Qty"].ToString()) : 0;

                        using (var iscope = new TransactionScope())
                        {

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == misDetail.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            misDetail.SKU_Name = skuDetail.SKU_Name;
                            misDetail.SKU_Id = skuDetail.SKU_Id;

                            var subtypedetail = ListHelper.SKU_Type().Where(x => x.SKU_Type_Name.ToLower().Trim() == misDetail.SKU_Type.ToLower().Trim()).FirstOrDefault();
                            misDetail.SKU_Type = subtypedetail.SKU_Type_Name;
                            misDetail.SKU_Type_Id = subtypedetail.SKU_Type_Id;

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == misDetail.Grade.ToLower().Trim()).FirstOrDefault();
                            misDetail.Grade = gradeDetail.GradeType_Name;

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == misDetail.UOM.ToLower().Trim()).FirstOrDefault();
                            misDetail.UOM = uomDetail.Unit_Name;

                            

                            misDetail.Closing_Qty = misDetail.Closing_Qty;

                            stilist.Add(misDetail);

                            iscope.Complete();
                        }

                        lineCount += 1;

                    }

                excelReader.Close();

                var t = stilist.ToList();
                DateTime today=DateTime.Now;
                int lineNUM = 2;
                foreach (var y in t)
                {
                    int mic = t.Where(d => d.SKU_Name == y.SKU_Name
                        && d.UOM == y.UOM
                        && d.SKU_Type == y.SKU_Type
                        && d.Grade == y.Grade
                          ).ToList().Count();
                    if (mic > 1)
                    {
                        y.status = false;
                        y.lineNumber = lineNUM;
                        y.Message = "Repeated Line Items are not allowed.Line No "+lineNUM;
                        result3.Add(y);
                        return result3;
                    }

                    //else
                    //{
                    //    y.status = true;
                    //    y.Message = "Sucess";
                    //    result.Add(y);
                    //}
                    lineNUM += 1;
                }
                var materialRaw = (from mm in DB.Estimated_Stock
                                   where mm.is_Deleted != true && mm.CreatedDate.Value.Year == today.Year && mm.CreatedDate.Value.Month == today.Month && mm.CreatedDate.Value.Day == today.Day && mm.DC_Code==fileDetail.DC_Code
                                   select mm).ToList();
                if (fileDetail.DC_Code != "NULL")
                {
                    materialRaw = materialRaw.Where(d => d.DC_Code == fileDetail.DC_Code).ToList();
                }
                foreach (var y in t)
                {
                    int lineNUMB = 2;
                    int mic = materialRaw.Where(d => d.SKU_Id == y.SKU_Id && d.SKU_Name == y.SKU_Name && d.SKU_Type == y.SKU_Type && d.Grade == y.Grade && d.UOM == y.UOM && d.is_Deleted != true && d.CreatedDate.Value.Year == today.Year && d.CreatedDate.Value.Month == today.Month && d.CreatedDate.Value.Day == today.Day && d.DC_Code == fileDetail.DC_Code).ToList().Count();
                    if (mic > 0)
                    {
                        //  result2.Add(y);
                        y.status = false;
                        y.lineNumber = lineNUMB;
                        y.Message = "Line Items already Exist.Line no " + lineNUMB;
                        result3.Add(y);
                        return result3;
                    }
                    else
                    {
                        result4.Add(y);

                    }

                    lineNUMB += 1;
                }


            }
            catch (Exception e)
            {
                stilist = new List<EstimatedStockEntity>();
                miDetail.status = false;
                miDetail.Message = e.Message.ToString();
                stilist.Add(miDetail);
                return stilist;
            }
            stilist[0].status = true;
            stilist[0].Message = "Success";

            return result4;
        }


        //-----------------------------------------SEARCH-------------------------------------------

        public List<EstimatedStockEntitySearch> GetEstimatedStock(int? roleId, DateTime? date, string ULocation, string SKU_Type, string Url)
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


            var qu = (from a in DB.Estimated_Stock
                      where (a.Closing_Date_Time.Value.Year == date.Value.Year && a.Closing_Date_Time.Value.Month == date.Value.Month && a.Closing_Date_Time.Value.Day == date.Value.Day) && a.DC_Code == ULocation && a.is_Deleted == false && a.SKU_Type.ToLower()==SKU_Type.ToLower()
                      select new EstimatedStockEntitySearch
                      {
                          Est_Stock_Id = a.Est_Stock_Id,
                          Est_Stock_code = a.Est_Stock_code,
                          DC_Name = a.DC_Name,
                          SKU_Name = a.SKU_Name,
                          SKU_Type_Id=a.SKU_Type_Id,
                          SKU_Type = a.SKU_Type,
                          Closing_Qty = a.Closing_Qty,
                          UOM = a.UOM,
                          Grade = a.Grade,
                          CreatedDate = a.CreatedDate,
                          CreateBy = a.CreateBy,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                      }).ToList();


            return qu;
        }
        public List<EstimatedStockEntity> CheckMaterialMaster(EstimtedStkEntity material)
        {
            List<EstimatedStockEntity> Intersect = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> Except = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> result = new List<EstimatedStockEntity>();
            List<EstimatedStockEntity> RawB = material.PhyStock.ToList();
            //bool? EditVal = null;
         
            
                int lineCount = 1;
                foreach (var y in RawB)
                {
                    DateTime today = DateTime.Now;
                    int mic =
                        (from mm in DB.Estimated_Stock
                         where mm.is_Deleted != true && (mm.DC_Code == y.DC_Code)
                         && mm.CreatedDate.Value.Year==today.Year
                         && mm.CreatedDate.Value.Month==today.Month
                         && mm.CreatedDate.Value.Day==today.Day
                         && mm.SKU_Type == y.SKU_Type
                         && mm.SKU_Type_Id == y.SKU_Type_Id
                         && mm.SKU_Id == y.SKU_Id
                         && mm.SKU_Name == y.SKU_Name
                         && mm.Grade == y.Grade
                         && mm.UOM == y.UOM                      
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
                EstimatedStockEntity Ex = new EstimatedStockEntity();
                Ex.InvalidDatas = Intersect.ToList();
                Ex.InvalidLineNumbers = Ex.InvalidDatas.Select(a => a.LineNumber).ToList();
                Ex.ValidDatas = Except.ToList();
                result.Add(Ex);
                return result;
            

        }

        public bool DeleteEstimatedStock(int phyStockId,string deletedby)
        {
            var success = false;
            if (phyStockId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.EstimatedStockRepository.GetByID(phyStockId);
                    if (p != null)
                    {
                        p.is_Deleted = true;
                        p.UpdateBy = deletedby;
                        p.UpdateDate = DateTime.Now;
                        _unitOfWork.EstimatedStockRepository.Update(p);
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

