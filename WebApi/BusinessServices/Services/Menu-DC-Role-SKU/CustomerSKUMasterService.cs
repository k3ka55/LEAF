using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace BusinessServices
{
    public class CustomerSKUMasterService : ICustomerSKUMasterServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public CustomerSKUMasterService()
        {
            _unitOfWork = new UnitOfWork();
        }

        //public CustomerSKUMasterModelEntity GetCustSKUMasterById(int skuId)
        //{
        //    var sku = _unitOfWork.CustomerSKUMasterRepository.GetByID(skuId);
        //    if (sku != null)
        //    {
        //        Mapper.CreateMap<Customer_SKU_Master, CustomerSKUMasterModelEntity>();
        //        var skuModel = Mapper.Map<Customer_SKU_Master, CustomerSKUMasterModelEntity>(sku);
        //        return skuModel;
        //    }
        //    return null;
        //}
        public List<CustomerSKUMasterLineItemModelEntity> ExcelImportForCustSKUMapping(fileImportSTI fileDetail)
        {
            //StatusFields ciExcel = new StatusFields();
            List<CustomerSKUMasterLineItemModelEntity> stilist = new List<CustomerSKUMasterLineItemModelEntity>();
            CustomerSKUMasterLineItemModelEntity stiDetail = new CustomerSKUMasterLineItemModelEntity();
            List<CustomerSKUMasterLineItemModelEntity> result = new List<CustomerSKUMasterLineItemModelEntity>();
            List<CustomerSKUMasterLineItemModelEntity> result2 = new List<CustomerSKUMasterLineItemModelEntity>();
            List<CustomerSKUMasterLineItemModelEntity> result3 = new List<CustomerSKUMasterLineItemModelEntity>();
            List<CustomerSKUMasterLineItemModelEntity> CheckArray = new List<CustomerSKUMasterLineItemModelEntity>();

            string Profilepicname = "CustSKU_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/CustSKU_";
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
                    stiDetail.status = false;
                    stiDetail.Message = "Extra Column's Present in Given Excel";
                    //stiDetail.ErrorReport.Add(ciExcel);
                    stilist.Add(stiDetail);
                    return stilist;
                }




                //--------------------validating Excel Columns
                int lineCount = 1;

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        CustomerSKUMasterLineItemModelEntity ci = new CustomerSKUMasterLineItemModelEntity();
                        object[] A = { };

                        ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        ci.Customer_SKU_Name = d["Customer_SKU_Name"] != null && d["Customer_SKU_Name"].ToString() != "" ? d["Customer_SKU_Name"].ToString() : "";
                        ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ci.EAN_Number = d["EAN_Number"] != null && d["EAN_Number"].ToString() != "" ? d["EAN_Number"].ToString() : "";
                        ci.Price = d["Price"] != null && d["Price"].ToString() != "" ? double.Parse(d["Price"].ToString()) : 0;
                        using (var iscope = new TransactionScope())
                        {

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            if (skuDetail != null)
                            {
                                ci.SKU_Name = skuDetail.SKU_Name;
                                ci.SKU_Id = skuDetail.SKU_Id;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "SKU_Name";
                                //stiDetail.ErrorReport.Add(ciExcel);
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
                            if (uomDetail != null)
                            {
                                ci.UOM = uomDetail.Unit_Name;
                            }
                            else
                            {
                                stiDetail.status = false;
                                stiDetail.lineNumber = lineCount;
                                stiDetail.Message = "Error";
                                stiDetail.errorItem = "UOM";
                                //stiDetail.ErrorReport.Add(ciExcel);
                                stilist.Add(stiDetail);
                                return stilist;
                            }

                            iscope.Complete();
                        }
                        ci.lineNumber = lineCount;
                        CheckArray.Add(ci);
                        lineCount += 1;
                    }


                foreach (var y in CheckArray)
                {
                    int mic = CheckArray.Where(d => d.Customer_SKU_Name == y.Customer_SKU_Name
                        && d.SKU_Name == y.SKU_Name
                        && d.UOM == y.UOM
                        && d.EAN_Number == y.EAN_Number
                        && d.Price == y.Price
                        ).ToList().Count();
                    if (mic > 1)
                    {
                        y.status = false;
                        y.lineNumber = y.lineNumber;
                        y.Message = "Repeated Line Items are not allowed";
                        stilist.Add(y);
                        return stilist;
                    }

                    else
                    {
                        y.status = true;
                        y.Message = "Sucess";
                        result.Add(y);
                    }

                }
                foreach (var yx in result)
                {
                    int micx = CheckArray.Where(d => d.SKU_Name == yx.SKU_Name
                        && d.EAN_Number == yx.EAN_Number
                        ).ToList().Count();
                    if (micx > 1)
                    {
                        yx.status = false;
                        yx.lineNumber = yx.lineNumber;
                        yx.Message = yx.SKU_Name + " and " + yx.EAN_Number + " Combination is repeated in the Uploaded Excel";
                        stilist.Add(yx);
                        return stilist;
                    }

                    else
                    {
                        yx.status = true;
                        yx.Message = "Sucess";
                        result2.Add(yx);
                    }
                }
                foreach (var yz in result2)
                {
                    int micz = DB.Customer_SKU_Mapping.Where(d => d.SKU_Name == yz.SKU_Name
                        && d.Customer_Code == fileDetail.Customer_Code
                        && d.EAN_Number == yz.EAN_Number
                        ).ToList().Count();
                    if (micz > 0)
                    {
                        yz.status = false;
                        yz.lineNumber = yz.lineNumber;
                        yz.Message = yz.SKU_Name + " and " + yz.EAN_Number + " Combination is already Exist for this customer";
                        stilist.Add(yz);
                        return stilist;
                    }

                    else
                    {
                        yz.status = true;
                        yz.Message = "Sucess";
                        result3.Add(yz);
                    }
                }

                excelReader.Close();
            }
            catch (Exception e)
            {
                stilist = new List<CustomerSKUMasterLineItemModelEntity>();
                stiDetail.status = false;
                stiDetail.Message = e.Message.ToString();
                stilist.Add(stiDetail);
                return stilist;
            }
            //stilist[0].status = true;
            //stilist[0].Message = "Success";

            //stiDetail.ErrorReport.Add(ciExcel);
            //stilist.Add(stiDetail);

            return result3;
        }

        public CustomerSKUMasterModelEntity GetCustSKUMasterById(int Id)
        {
            CustomerSKUMasterModelEntity list = new CustomerSKUMasterModelEntity();

            list = (from x in DB.Customer_SKU_Mapping
                    where x.Customer_Id == Id
                    select new CustomerSKUMasterModelEntity
                    {
                        LineItem = (from yt in DB.Customer_SKU_Mapping
                                    where yt.Customer_Id == x.Customer_Id
                                    select new CustomerSKUMasterLineItemModelEntity
                                    {
                                        Customer_SKU_Mapping_Id = yt.Customer_SKU_Mapping_Id,
                                        UOM = yt.UOM,
                                        UpdatedDate = yt.UpdatedDate,
                                        CreatedBy = yt.CreatedBy,
                                        UpdatedBy = yt.UpdatedBy,
                                        Price = yt.Price,
                                        CreatedDate = yt.CreatedDate,
                                        EAN_Number = yt.EAN_Number,
                                        SKU_Id = yt.SKU_Id,
                                        SKU_Name = yt.SKU_Name,
                                        SKU_Code = yt.SKU_Code,
                                        Customer_SKU_Name = yt.Customer_SKU_Name
                                    }).ToList(),
                        Customer_Id = x.Customer_Id,
                        Customer_Code = x.Customer_Code,
                        Customer_Name = x.Customer_Name
                    }).FirstOrDefault();
            return list;
        }

        public dynamic GetAllCustSKUMaster(int? roleId, string Url)
        {
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges).FirstOrDefault();

            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);
            var V = (from f in DB.Customer_SKU_Mapping
                     group f by new
                     {
                         f.Customer_Id,
                         f.Customer_Code,
                         f.Customer_Name
                     } into temp

                     select new
                     {
                         Customer_Id = temp.Key.Customer_Id,
                         Customer_Code = temp.Key.Customer_Code,
                         Customer_Name = temp.Key.Customer_Name,
                         is_Create = iCrt,
                         is_Delete = isDel,
                         is_Edit = isEdt,
                         is_Approval = isApp,
                         is_View = isViw,
                         LineItem = temp
                     }).ToList();
            return V;
        }



        public List<CustomerSKUMasterLineItemModelEntity> CheckCustomerSKUMaster(CustomerSKUMasterList CustomerSKUMaster)
        {
            List<CustomerSKUMasterLineItemModelEntity> Intersect = new List<CustomerSKUMasterLineItemModelEntity>();
            List<CustomerSKUMasterLineItemModelEntity> Except = new List<CustomerSKUMasterLineItemModelEntity>();
            List<CustomerSKUMasterLineItemModelEntity> result = new List<CustomerSKUMasterLineItemModelEntity>();
            List<CustomerSKUMasterLineItemModelEntity> RawB = CustomerSKUMaster.CustomerSKUMaster.ToList();
            bool? EditVal = null;
            if (CustomerSKUMaster.FromScreen == "EDIT")
            {
                //  int lineCount = 1;
                var yy = CustomerSKUMaster.CustomerSKUMaster.FirstOrDefault();

                int mic =
                    (from mm in DB.Customer_SKU_Mapping
                     where mm.Customer_SKU_Name == yy.Customer_SKU_Name
                     && mm.Customer_Code == CustomerSKUMaster.Customer_Code
                     && mm.SKU_Name == yy.SKU_Name
                     && mm.UOM == yy.UOM
                     && mm.EAN_Number == yy.EAN_Number
                  

                     select mm).Count();
                int ms = 0;
                if (mic == 0)
                {
                    ms = (from mm in DB.Customer_SKU_Mapping
                          where mm.Customer_SKU_Name == yy.Customer_SKU_Name
                       && mm.SKU_Name == yy.SKU_Name
                       && mm.UOM == yy.UOM
                       && mm.EAN_Number == yy.EAN_Number
                       
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

                CustomerSKUMasterLineItemModelEntity Ex = new CustomerSKUMasterLineItemModelEntity();
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
                        (from mm in DB.Customer_SKU_Mapping
                         where mm.Customer_Code == CustomerSKUMaster.Customer_Code
                      && mm.SKU_Name == y.SKU_Name
                      && mm.UOM == y.UOM
                      && mm.EAN_Number == y.EAN_Number
                    
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
                CustomerSKUMasterLineItemModelEntity Ex = new CustomerSKUMasterLineItemModelEntity();
                Ex.InvalidDatas = Intersect.ToList();
                Ex.InvalidLineNumbers = Ex.InvalidDatas.Select(a => a.LineNumber).ToList();
                Ex.ValidDatas = Except.ToList();
                result.Add(Ex);
                return result;
            }

        }


        public bool CreateCustSKUMaster(CustomerSKUMasterModelEntity customerSKUMasterModelEntity)
        {
            using (var scope = new TransactionScope())
            {
                var sku = new Customer_SKU_Mapping();
                foreach (var t in customerSKUMasterModelEntity.LineItem)
                {
                    sku.Customer_Id = customerSKUMasterModelEntity.Customer_Id;
                    sku.Customer_Code = customerSKUMasterModelEntity.Customer_Code;
                    sku.Customer_Name = customerSKUMasterModelEntity.Customer_Name;
                    sku.Customer_SKU_Name = t.Customer_SKU_Name;
                    sku.SKU_Id = t.SKU_Id;
                    sku.SKU_Code = t.SKU_Code;
                    sku.SKU_Name = t.SKU_Name;
                    sku.UOM = t.UOM;
                    sku.EAN_Number = t.EAN_Number;
                    sku.Price = t.Price;
                    sku.CreatedDate = DateTime.UtcNow;
                    sku.Customer_SKU_Name = t.Customer_SKU_Name;
                    sku.CreatedBy = t.CreatedBy;

                    _unitOfWork.CustomerSKUMappingRepository.Insert(sku);
                    _unitOfWork.Save();
                };

                scope.Complete();
            }


            return true;
        }



        public bool UpdateCustSKUMaster(CustomerSKUMasterModelEntity customerSKUMasterModelEntity)
        {
            //  var success = false;
            if (customerSKUMasterModelEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    if (customerSKUMasterModelEntity.LineItem != null)
                    {
                        var oldOneEquip = (from wTd in DB.Customer_SKU_Mapping
                                           where wTd.Customer_Id == customerSKUMasterModelEntity.Customer_Id
                                           select wTd.Customer_SKU_Mapping_Id).ToList();
                        var tempEquip = customerSKUMasterModelEntity.LineItem;
                        var newOneEquip = tempEquip.Select(a => a.Customer_SKU_Mapping_Id).ToList();

                        var wantTodeleteEquip = oldOneEquip.Except(newOneEquip).ToList();
                        //var wantToInsert = newOne.Except(oldOne).ToList();
                        //var interSect = oldOne.Intersect(newOne).ToList();

                        if (wantTodeleteEquip != null)
                        {
                            foreach (var t in wantTodeleteEquip)
                            {
                                var wd = (from dw in DB.Customer_SKU_Mapping
                                          where dw.Customer_SKU_Mapping_Id == t && dw.Customer_Id == customerSKUMasterModelEntity.Customer_Id
                                          select dw).FirstOrDefault();
                                _unitOfWork.CustomerSKUMappingRepository.Delete(wd);
                                _unitOfWork.Save();

                                DB.SaveChanges();
                            }

                        }
                    }
                    var sku = new Customer_SKU_Mapping();
                    foreach (var t in customerSKUMasterModelEntity.LineItem)
                    {
                        if (t.Customer_SKU_Mapping_Id != 0)
                        {
                            var Usku = _unitOfWork.CustomerSKUMappingRepository.GetByID(t.Customer_SKU_Mapping_Id);
                            if (sku != null)
                            {
                                Usku.Customer_Id = customerSKUMasterModelEntity.Customer_Id;
                                Usku.Customer_Code = customerSKUMasterModelEntity.Customer_Code;
                                Usku.Customer_Name = customerSKUMasterModelEntity.Customer_Name;
                                Usku.Customer_SKU_Name = t.Customer_SKU_Name;
                                Usku.SKU_Id = t.SKU_Id;
                                Usku.SKU_Code = t.SKU_Code;
                                Usku.SKU_Name = t.SKU_Name;
                                Usku.UOM = t.UOM;
                                Usku.EAN_Number = t.EAN_Number;
                                Usku.Price = t.Price;
                                Usku.UpdatedDate = DateTime.UtcNow;
                                Usku.Customer_SKU_Name = t.Customer_SKU_Name;
                                Usku.UpdatedBy = t.UpdatedBy;

                                _unitOfWork.CustomerSKUMappingRepository.Update(Usku);
                                _unitOfWork.Save();
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (t.Customer_SKU_Mapping_Id == 0)
                        {
                            sku.Customer_Id = customerSKUMasterModelEntity.Customer_Id;
                            sku.Customer_Code = customerSKUMasterModelEntity.Customer_Code;
                            sku.Customer_Name = customerSKUMasterModelEntity.Customer_Name;
                            sku.Customer_SKU_Name = t.Customer_SKU_Name;
                            sku.SKU_Id = t.SKU_Id;
                            sku.SKU_Code = t.SKU_Code;
                            sku.SKU_Name = t.SKU_Name;
                            sku.UOM = t.UOM;
                            sku.EAN_Number = t.EAN_Number;
                            sku.Price = t.Price;
                            sku.Customer_SKU_Name = t.Customer_SKU_Name;
                            sku.UpdatedBy = t.UpdatedBy;
                            sku.CreatedDate = DateTime.UtcNow;
                            sku.UpdatedDate = DateTime.UtcNow;
                            sku.Customer_SKU_Name = t.Customer_SKU_Name;
                            sku.CreatedBy = t.UpdatedBy;

                            _unitOfWork.CustomerSKUMappingRepository.Insert(sku);
                            _unitOfWork.Save();
                            //   success = true;

                        }
                        else
                        {
                            return false;
                        }

                    }
                    scope.Complete();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteCustSKUMaster(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var dlt = (from x in DB.Customer_SKU_Mapping
                               where x.Customer_Id == Id
                               select x).ToList();
                    foreach (var yt in dlt)
                    {
                        var sku = _unitOfWork.CustomerSKUMappingRepository.GetByID(yt.Customer_SKU_Mapping_Id);
                        if (sku != null)
                        {
                            try
                            {
                                _unitOfWork.CustomerSKUMappingRepository.Delete(sku);
                                _unitOfWork.Save();
                                success = true;
                            }
                            catch (Exception)
                            {
                                success = false;
                                return success;
                            }
                        }
                    }
                    scope.Complete();

                }
            }
            return success;
        }

    }
}