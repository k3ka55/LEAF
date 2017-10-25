using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class CustomerIndentService : ICustomerIndentService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public CustomerIndentService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool checktempName(string name)
        {
            bool result = false;

            var list = DB.Customer_Indent.Where(x => x.Indent_Name == name).FirstOrDefault();

            if (list != null)
            {
                result = true;
            }

            return result;
        }
        public CIExcelImport ExcelImportForCI(fileImport fileDetail)
        {
            CIExcelImport ciExcel = new CIExcelImport();
            string Profilepicname = "CI_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/CI";
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
                    ciExcel.status = false;
                    ciExcel.Message = "Extra Column's Present in Given Excel";
                    return ciExcel;
                }

                //--------------------validating Excel Columns
                int lineCount = 1;

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        Customer_Indent_Line_item ci = new Customer_Indent_Line_item();
                        object[] A = { };


                        ci.Indent_ID = fileDetail.indentID;
                        ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        ci.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        ci.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        ci.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        ci.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ci.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        ci.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? double.Parse(d["Dispatch_Qty"].ToString()) : 0;

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
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "SKU_SubType_Name";
                                return ciExcel;
                            }

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            if (skuDetail != null)
                            {
                                ci.SKU_Name = skuDetail.SKU_Name;
                                ci.SKU_Id = skuDetail.SKU_Id;
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "SKU_Name";
                                return ciExcel;
                            }

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
                            if (uomDetail != null)
                            {
                                ci.UOM = uomDetail.Unit_Name;
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "UOM";
                                return ciExcel;
                            }

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ci.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            if (packtypedetail != null)
                            {
                                ci.Pack_Type = packtypedetail.Pack_Type_Name;
                                ci.Pack_Type_Id = packtypedetail.Pack_Type_Id;
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "Pack_Type_Name";
                                return ciExcel;
                            }

                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            if (packweightType != null)
                            {
                                ci.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                                ci.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "Pack_Weight_Type";
                                return ciExcel;
                            }

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ci.Grade.ToLower().Trim()).FirstOrDefault();
                            if (gradeDetail != null)
                            {
                                ci.Grade = gradeDetail.GradeType_Name;
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "Grade";
                                return ciExcel;
                            }

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ci.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            if (packsizedetail != null)
                            {
                                ci.Pack_Size = packsizedetail.Pack_Size_Value;
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.lineNumber = lineCount;
                                ciExcel.Message = "Error";
                                ciExcel.errorItem = "Pack_Size_Value";
                                return ciExcel;
                            }

                            var price = (from x in DB.Rate_Template_Line_item
                                         join y in DB.Rate_Template on x.RT_id equals y.Template_ID
                                         join z in DB.Customer_Indent on y.Template_ID equals z.Price_Template_ID
                                         where z.Indent_ID == fileDetail.indentID && x.SKU_Id == ci.SKU_Id && x.Grade == ci.Grade
                                         select new
                                         {
                                             x.Selling_price
                                         }).FirstOrDefault();

                            if (price != null)
                            {
                                ci.Price = double.Parse(price.Selling_price.ToString());
                            }
                            else
                            {
                                ciExcel.status = false;
                                ciExcel.Message = "Price is not available for this LineItem '" + lineCount + "' in Rate Template";
                                return ciExcel;
                            }


                            iscope.Complete();
                        }

                        lineCount += 1;

                    }
                //-----------------------------------Delete All Columns
                using (var iscope1 = new TransactionScope())
                {
                    var del_list = DB.Customer_Indent_Line_item.Where(x => x.Indent_ID == fileDetail.indentID).ToList();

                    foreach (var li in del_list)
                    {
                        _unitOfWork.CustomerIndentLineItemRepository.Delete(li.CI_Line_Id);
                        _unitOfWork.Save();
                    }
                    iscope1.Complete();
                }
                //------------------------------------Insert New COlumns

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        Customer_Indent_Line_item ci = new Customer_Indent_Line_item();

                        ci.Indent_ID = fileDetail.indentID;
                        ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        ci.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        ci.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        ci.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        ci.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ci.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        ci.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? double.Parse(d["Dispatch_Qty"].ToString()) : 0;

                        using (var iscope = new TransactionScope())
                        {

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            ci.SKU_Name = skuDetail.SKU_Name;
                            ci.SKU_Id = skuDetail.SKU_Id;

                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == ci.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            ci.SKU_SubType = subtypedetail.SKU_SubType_Name;
                            ci.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ci.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            ci.Pack_Type = packtypedetail.Pack_Type_Name;
                            ci.Pack_Type_Id = packtypedetail.Pack_Type_Id;


                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            ci.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                            ci.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ci.Grade.ToLower().Trim()).FirstOrDefault();
                            ci.Grade = gradeDetail.GradeType_Name;

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
                            ci.UOM = uomDetail.Unit_Name;

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ci.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            ci.Pack_Size = packsizedetail.Pack_Size_Value;

                            var price = (from x in DB.Rate_Template_Line_item
                                         join y in DB.Rate_Template on x.RT_id equals y.Template_ID
                                         join z in DB.Customer_Indent on y.Template_ID equals z.Price_Template_ID
                                         where z.Indent_ID == fileDetail.indentID && x.SKU_Id == ci.SKU_Id && x.Grade == ci.Grade
                                         select new
                                         {
                                             x.Selling_price
                                         }).FirstOrDefault();

                            ci.Price = double.Parse(price.Selling_price.ToString());

                            var createdBy = (from x in DB.Customer_Indent
                                             where x.Indent_ID == ci.Indent_ID
                                             select new
                                             {
                                                 x.CreateBy,
                                                 x.CreatedDate,
                                                 x.UpdateBy,
                                                 x.UpdateDate
                                             }).FirstOrDefault();

                            ci.CreateBy = createdBy.CreateBy;
                            ci.CreatedDate = createdBy.CreatedDate;
                            ci.UpdateBy = createdBy.UpdateBy;
                            ci.UpdateDate = createdBy.UpdateDate;

                            using (var iscope1 = new TransactionScope())
                            {
                                _unitOfWork.CustomerIndentLineItemRepository.Insert(ci);
                                _unitOfWork.Save();

                                iscope1.Complete();
                            }
                            iscope.Complete();
                        }

                        lineCount += 1;

                    }

                excelReader.Close();
            }
            catch (Exception e)
            {
                ciExcel.status = false;
                ciExcel.Message = e.Message.ToString();
                return ciExcel;
            }
            ciExcel.status = true;
            ciExcel.Message = "Success";

            return ciExcel;
        }
        public Customer_Indent_Num_Gen GetAutoIncrement(string locationId)
        {
            var autoinc = DB.Customer_Indent_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();

            var model = new Customer_Indent_Num_Gen
            {
                Customer_Indent_Num_Gen_Id = autoinc.Customer_Indent_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Customer_Indent_Last_Number = autoinc.Customer_Indent_Last_Number
            };

            return model;
        }

        public int CreateCustomerIndent(CIEntity ciEntity)
        {
            string indentNumber, STI_prefix;
            int? incNumber;

            string locationID = "";


            using (var iscope = new TransactionScope())
            {
                if (ciEntity.Location_Code != null && ciEntity.Location_Code != "null")
                    locationID = ciEntity.Location_Code;
                else if (ciEntity.DC_Code != null && ciEntity.DC_Code != "null")
                    locationID = ciEntity.DC_Code;

                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                STI_prefix = rm.GetString("CIT");
                Customer_Indent_Num_Gen autoIncNumber = GetAutoIncrement(locationID);
                locationID = autoIncNumber.DC_Code;
                incNumber = autoIncNumber.Customer_Indent_Last_Number;
                int? incrementedValue = incNumber + 1;
                var STincrement = DB.Customer_Indent_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
                STincrement.Customer_Indent_Last_Number = incrementedValue;
                _unitOfWork.CustomerIndentNumGenRepository.Update(STincrement);
                _unitOfWork.Save();
                indentNumber = STI_prefix + "/" + locationID + "/" + String.Format("{0:00000}", incNumber);

                iscope.Complete();
            }


            string indentName = "";

            indentName = ciEntity.Indent_Name;

            bool checkName = checktempName(indentName);
            if (checkName)
            {
                return -1;
            }

            using (var scope = new TransactionScope())
            {
                var customerIndent = new Customer_Indent
                {
                    Indent_Name = indentName,
                    Indent_Code = indentNumber,
                    DC_Id = ciEntity.DC_Id,
                    DC_Code = ciEntity.DC_Code,
                    Location_Code = ciEntity.Location_Code,
                    Location_Id = ciEntity.Location_Id,
                    Region_Id = ciEntity.Region_Id,
                    Region = ciEntity.Region,
                    Region_Code = ciEntity.Region_Code,
                    Indent_Type = ciEntity.Indent_Type,
                    Customer_Code = ciEntity.Customer_Code,
                    Customer_Id = ciEntity.Customer_Id,
                    Customer_Name = ciEntity.Customer_Name,
                    Customer_Delivery_Address = ciEntity.Customer_Delivery_Address,
                    Dispatch_DC_Code = ciEntity.Dispatch_DC_Code,
                    Delivery_cycle = ciEntity.Delivery_cycle,
                    Delivery_Expected_Date = ciEntity.Delivery_Expected_Date,
                    Delivery_Type = ciEntity.Delivery_Type,
                    SKU_Type_Id = ciEntity.SKU_Type_Id,
                    SKU_Type = ciEntity.SKU_Type,
                    Price_Template_ID = ciEntity.Price_Template_ID,
                    Price_Template_Code = ciEntity.Price_Template_Code,
                    Price_Template_Name = ciEntity.Price_Template_Name,
                    Price_Template_Valitity_upto = ciEntity.Price_Template_Valitity_upto,
                    CreatedDate = DateTime.UtcNow,
                    CreateBy = ciEntity.CreateBy,
                    Is_Deleted = false,
                    Is_Sync = false,
                };

                _unitOfWork.CustomerIndentRepository.Insert(customerIndent);
                _unitOfWork.Save();


                int? cId = customerIndent.Indent_ID;

                var model = new Customer_Indent_Line_item();
                foreach (CustomerIndentLineItemEntity pSub in ciEntity.LineItems)
                {
                    model.Indent_ID = cId;
                    model.Indent_Code = indentNumber;
                    model.SKU_Id = pSub.SKU_Id;
                    model.SKU_Name = pSub.SKU_Name;
                    model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                    model.SKU_SubType = pSub.SKU_SubType;
                    model.Pack_Type_Id = pSub.Pack_Type_Id;
                    model.Pack_Type = pSub.Pack_Type;
                    model.UOM = pSub.UOM;
                    model.HSN_Code = pSub.HSN_Code;
                    model.Total_GST = pSub.Total_GST;
                    model.CGST = pSub.CGST;
                    model.SGST = pSub.SGST;
                    model.Pack_Size = pSub.Pack_Size;
                    model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                    model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                    model.Grade = pSub.Grade;
                    model.Price = pSub.Price;
                    model.Dispatch_Qty = pSub.Dispatch_Qty;
                    model.CreatedDate = DateTime.Now;
                    model.CreateBy = pSub.CreateBy;

                    _unitOfWork.CustomerIndentLineItemRepository.Insert(model);
                    _unitOfWork.Save();

                }
                scope.Complete();
                return customerIndent.Indent_ID;
            }
        }


        public List<CustomerIndentReturnEntity> getCIForCSI(int customerID)
        {
            List<CustomerIndentReturnEntity> output = new List<CustomerIndentReturnEntity>();

            var milkList = (from x in DB.Customer_Indent_Template_Mapping
                            where x.Customer_Id == customerID
                            select new CustomerIndentReturnEntity
                            {
                                Indent_ID = x.Indent_ID.Value,
                                Indent_Name = x.Indent_Name,
                                DC_Id = x.DC_Id,
                                DC_Code = x.DC_Code,
                                Location_Id = x.Location_Id,
                                Location_Code = x.Location_Code,
                                Region_Id = x.Region_Id,
                                Region = x.Region,
                                Indent_Code = x.Indent_Code,
                                Region_Code = x.Region_Code
                            }).ToList();

            output = milkList.ToList();
            var list = (from x in DB.Customer_Indent
                        where x.Customer_Id == customerID && x.Is_Deleted == false
                        select new CustomerIndentReturnEntity
                        {
                            Indent_ID = x.Indent_ID,
                            Indent_Name = x.Indent_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Indent_Code = x.Indent_Code,
                            Region_Code = x.Region_Code
                        }).ToList();
            foreach (var t in list)
            {
                output.Add(t);
            }

            return output;
        }


        public List<CustomerIndentEditReturnEntity> SearchCIforCSIEdit(int customerID)
        {
            List<CustomerIndentEditReturnEntity> returnList = new List<CustomerIndentEditReturnEntity>();

            var list = (from x in DB.Customer_Indent
                        where x.Customer_Id == customerID && x.Is_Deleted == false
                        select new CustomerIndentEditReturnEntity
                        {
                            Indent_ID = x.Indent_ID,
                            Indent_Name = x.Indent_Name,
                            Indent_Code = x.Indent_Code

                        }).ToList();
            var MappingList = (from x in DB.Customer_Indent_Template_Mapping
                               where x.Customer_Id == customerID
                               select new CustomerIndentEditReturnEntity
                               {
                                   Indent_ID = x.Indent_ID.Value,
                                   Indent_Name = x.Indent_Name,
                                   Indent_Code = x.Indent_Code,
                               }).ToList();

            foreach (var y in list)
            {
                returnList.Add(y);
            }
            foreach (var y in MappingList)
            {
                returnList.Add(y);
            }




            //if(dccode != null && locationcode == "null")
            //{
            //    var searchList = list.Where(x => x.DC_Code == dccode && x.Region == region).FirstOrDefault();

            //    returnList.Add(searchList);
            //}
            //else if (dccode == "null" && locationcode != null)
            //{
            //    var searchList = list.Where(x => x.Location_Code == locationcode && x.Region == region).FirstOrDefault();

            //    returnList.Add(searchList);
            //}

            return returnList;
        }
        public int UpdateCustomerIndent(int cId, CIEntity ciEntity)
        {
            if (ciEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.CustomerIndentRepository.GetByID(cId);

                    if (p != null)
                    {
                        p.Indent_Name = ciEntity.Indent_Name;
                        p.DC_Id = ciEntity.DC_Id;
                        p.DC_Code = ciEntity.DC_Code;
                        p.Location_Code = ciEntity.Location_Code;
                        p.Location_Id = ciEntity.Location_Id;
                        p.Region_Id = ciEntity.Region_Id;
                        p.Region = ciEntity.Region;
                        p.Region_Code = ciEntity.Region_Code;
                        p.Indent_Type = ciEntity.Indent_Type;
                        p.Customer_Code = ciEntity.Customer_Code;
                        p.Customer_Id = ciEntity.Customer_Id;
                        p.Customer_Name = ciEntity.Customer_Name;
                        p.Customer_Delivery_Address = ciEntity.Customer_Delivery_Address;
                        p.Dispatch_DC_Code = ciEntity.Dispatch_DC_Code;
                        p.Delivery_cycle = ciEntity.Delivery_cycle;
                        p.Delivery_Expected_Date = ciEntity.Delivery_Expected_Date;
                        p.Delivery_Type = ciEntity.Delivery_Type;
                        //    DC_Location = ciEntity.DC_Location,
                        p.SKU_Type_Id = ciEntity.SKU_Type_Id;
                        p.SKU_Type = ciEntity.SKU_Type;
                        p.Price_Template_ID = ciEntity.Price_Template_ID;
                        p.Price_Template_Name = ciEntity.Price_Template_Name;
                        p.Price_Template_Code = ciEntity.Price_Template_Code;
                        p.Price_Template_Valitity_upto = ciEntity.Price_Template_Valitity_upto;
                        p.UpdateDate = DateTime.Now;
                        p.UpdateBy = ciEntity.UpdateBy;
                        p.Is_Deleted = false;
                        p.Is_Sync = false;

                        _unitOfWork.CustomerIndentRepository.Update(p);
                        _unitOfWork.Save();
                    }

                    var lines = DB.Customer_Indent_Line_item.Where(x => x.Indent_ID == cId).ToList();

                    foreach (var litem in lines)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var line = _unitOfWork.CustomerIndentLineItemRepository.GetByID(litem.CI_Line_Id);
                            if (line != null)
                            {

                                _unitOfWork.CustomerIndentLineItemRepository.Delete(line);
                                _unitOfWork.Save();
                            }
                            scope1.Complete();
                        }
                    }

                    foreach (CustomerIndentLineItemEntity pSub in ciEntity.LineItems)
                    {
                        //var line = _unitOfWork.CustomerIndentLineItemRepository.GetByID(pSub.CI_Line_Id);
                        var model = new Customer_Indent_Line_item();

                        //if (line != null)
                        //{
                        //    line.Indent_ID = cId;
                        //    line.SKU_Name = pSub.SKU_Name;
                        //    line.SKU_SubType = pSub.SKU_SubType;
                        //    line.Pack_Type = pSub.Pack_Type;
                        //    line.Grade = pSub.Grade;
                        //    line.UOM = pSub.UOM;
                        //    line.Pack_Size = pSub.Pack_Size;
                        //    line.Price = pSub.Price;
                        //    line.UpdateDate = DateTime.Now;
                        //    line.UpdateBy = pSub.UpdateBy;

                        //    _unitOfWork.CustomerIndentLineItemRepository.Update(line);
                        //    _unitOfWork.Save();
                        //}
                        //else
                        //{

                        model.Indent_ID = cId;
                        model.Indent_Code = ciEntity.Indent_Code;
                        model.SKU_Id = pSub.SKU_Id;
                        model.SKU_Name = pSub.SKU_Name;
                        model.HSN_Code = pSub.HSN_Code;
                        model.Total_GST = pSub.Total_GST;
                        model.CGST = pSub.CGST;
                        model.SGST = pSub.SGST;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.Pack_Type_Id = pSub.Pack_Type_Id;
                        model.Pack_Type = pSub.Pack_Type;
                        model.UOM = pSub.UOM;
                        model.Pack_Size = pSub.Pack_Size;
                        model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                        model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                        model.Grade = pSub.Grade;
                        model.Price = pSub.Price;
                        model.Dispatch_Qty = pSub.Dispatch_Qty;
                        model.CreatedDate = DateTime.Now;
                        model.CreateBy = pSub.CreateBy;

                        _unitOfWork.CustomerIndentLineItemRepository.Insert(model);
                        _unitOfWork.Save();
                        //}
                    }
                    scope.Complete();
                }
            }
            return cId;
        }

        public List<CIEntity> SearchIndentFormapping(int regionid, string location, string dccode)
        {
            List<CIEntity> cilist = new List<CIEntity>();


            var list = (from x in DB.Customer_Indent //&& x.DC_Location == location 
                        where x.Is_Deleted == false
                        // orderby x.Indent_Name

                        select
                            //x).AsEnumerable().Select(x => 
                        new CIEntity
                        {
                            Indent_ID = x.Indent_ID,
                            Indent_Name = x.Indent_Name,
                            Indent_Code = x.Indent_Code,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Indent_Type = x.Indent_Type,
                            Customer_Id = x.Customer_Id,
                            Customer_Code = x.Customer_Code,
                            Customer_Name = x.Customer_Name,
                            Customer_Delivery_Address = x.Customer_Delivery_Address,
                            Dispatch_DC_Code = x.Dispatch_DC_Code,
                            Delivery_cycle = x.Delivery_cycle,
                            Delivery_Expected_Date = x.Delivery_Expected_Date,
                            Delivery_Type = x.Delivery_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Price_Template_ID = x.Price_Template_ID,
                            Price_Template_Code = x.Price_Template_Code,
                            Price_Template_Name = x.Price_Template_Name,
                            Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                            CreatedDate = x.CreatedDate,
                            UpdateDate = x.UpdateDate,
                            CreateBy = x.CreateBy,
                            UpdateBy = x.UpdateBy,

                        }).ToList();


            if (location != null && dccode == "null")
            {

                var filter = list.Where(x => x.Location_Code == location && x.Region_Id == regionid).ToList();
                if (filter != null)
                {
                    foreach (var lists in filter)
                    {
                        cilist.Add(lists);
                    }

                }

            }
            else if (location == "null" && dccode != null)
            {
                var filter = list.Where(x => x.DC_Code == dccode && x.Region_Id == regionid).ToList();
                if (filter != null)
                {
                    foreach (var lists in filter)
                    {
                        cilist.Add(lists);
                    }

                }

            }

            return cilist;
        }

        public List<CIEntity> searchIndent(int? roleId, int regionid, string location, string dccode, string Url)
        {
            Stopwatch aa = new Stopwatch();
            aa.Start();
            List<CIEntity> cilist = new List<CIEntity>();
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
            //        var menuAccess = DB.Role_Menu_Access
            //.Join
            //(
            //  DB.Menu_Master,
            //    c => c.Menu_Id,
            //    d => d.Menu_Id,
            //    (c, d) => new { c, d }
            // )
            //.Where(e => e.c.Role_Id == roleId).Where(g => g.d.Menu_Id == g.c.Menu_Id && g.d.Url == Url).GroupBy(e => new { e.d.Menu_Id })
            //.Select(x => new FetchMenuDetails
            //{
            //    MenuID = x.Key.Menu_Id,
            //    MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
            //    MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
            //    RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
            //    ControllerName = x.Select(c => c.d.Url).Distinct(),
            //    ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
            //}).FirstOrDefault();

            var list = (from x in DB.Customer_Indent //&& x.DC_Location == location 
                        where x.Is_Deleted == false
                        // orderby x.Indent_Name

                        select
                            //x).AsEnumerable().Select(x => 
                        new CIEntity
                        {
                            Indent_ID = x.Indent_ID,
                            Indent_Name = x.Indent_Name,
                            Indent_Code = x.Indent_Code,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Indent_Type = x.Indent_Type,
                            Customer_Id = x.Customer_Id,
                            Customer_Code = x.Customer_Code,
                            Customer_Name = x.Customer_Name,
                            Customer_Delivery_Address = x.Customer_Delivery_Address,
                            Dispatch_DC_Code = x.Dispatch_DC_Code,
                            Delivery_cycle = x.Delivery_cycle,
                            Delivery_Expected_Date = x.Delivery_Expected_Date,
                            Delivery_Type = x.Delivery_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Price_Template_ID = x.Price_Template_ID,
                            Price_Template_Name = x.Price_Template_Name,
                            Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                            CreatedDate = x.CreatedDate,
                            UpdateDate = x.UpdateDate,
                            CreateBy = x.CreateBy,
                            UpdateBy = x.UpdateBy,
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
                            counting = (from a in DB.Customer_Indent_Line_item
                                        where a.Indent_ID == x.Indent_ID
                                        select new
                                        {
                                            IndentIId = a.Indent_ID
                                        }).Count(),
                            LineItems = (from a in DB.Customer_Indent_Line_item
                                         where a.Indent_ID == x.Indent_ID
                                         orderby a.SKU_Name
                                         select new CustomerIndentLineItemEntity
                                         {
                                             CI_Line_Id = a.CI_Line_Id,
                                             Indent_ID = a.Indent_ID,
                                             SKU_Id = a.SKU_Id,
                                             SKU_Name = a.SKU_Name,
                                             SKU_SubType_Id = a.SKU_SubType_Id,
                                             SKU_SubType = a.SKU_SubType,
                                             Pack_Type_Id = a.Pack_Type_Id,
                                             Pack_Type = a.Pack_Type,
                                             Pack_Size = a.Pack_Size,
                                             Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
                                             Pack_Weight_Type = a.Pack_Weight_Type,
                                             UOM = a.UOM,
                                             Grade = a.Grade,
                                             Price = a.Price,
                                             Dispatch_Qty = a.Dispatch_Qty,
                                             CreatedDate = x.CreatedDate,
                                             UpdateDate = x.UpdateDate,
                                             CreateBy = x.CreateBy,
                                             UpdateBy = x.UpdateBy,


                                             Indent_Code = x.Indent_Code,
                                             Indent_Name = x.Indent_Name,
                                             DC_Id = x.DC_Id,
                                             DC_Code = x.DC_Code,
                                             Location_Id = x.Location_Id,
                                             Location_Code = x.Location_Code,
                                             Region_Id = x.Region_Id,
                                             Region = x.Region,
                                             Indent_Type = x.Indent_Type,
                                             Customer_Id = x.Customer_Id,
                                             Customer_Name = x.Customer_Name,
                                             Customer_Code = x.Customer_Code,
                                             Customer_Delivery_Address = x.Customer_Delivery_Address,
                                             Dispatch_DC_Code = x.Dispatch_DC_Code,
                                             Delivery_cycle = x.Delivery_cycle,
                                             Delivery_Expected_Date = x.Delivery_Expected_Date,
                                             Delivery_Type = x.Delivery_Type,
                                             SKU_Type_Id = x.SKU_Type_Id,
                                             SKU_Type = x.SKU_Type,
                                             Price_Template_ID = x.Price_Template_ID,
                                             Price_Template_Code = x.Price_Template_Code,
                                             Price_Template_Name = x.Price_Template_Name,

                                             Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,

                                             Reason = x.Reason

                                         }).ToList(),

                        }).ToList();
            aa.Stop();
            string Time = aa.Elapsed.ToString();


            if (location != null && dccode == "null")
            {

                var filter = list.Where(x => x.Location_Code == location && x.Region_Id == regionid).ToList();
                if (filter != null)
                {
                    foreach (var lists in filter)
                    {
                        lists.Stopwatch = Time;
                        cilist.Add(lists);
                    }

                }

            }
            else if (location == "null" && dccode != null)
            {
                var filter = list.Where(x => x.DC_Code == dccode && x.Region_Id == regionid).ToList();
                if (filter != null)
                {
                    foreach (var lists in filter)
                    {
                        lists.Stopwatch = Time;
                        cilist.Add(lists);
                    }

                }

            }
            //else
            //{
            //    cilist.Add()
            //}
            //foreach (var t in cilist)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}

            return cilist;
        }

        //   ------------------------------------GET----------------------------------------
        public List<CIEntity> GetCustomerIndent(string id)
        {
            // string RTemplateCode = (from xx in DB.Customer_Indent
            //            where xx.Indent_Code == id
            //                          select xx.Price_Template_Code).FirstOrDefault();
            //var Rlist = (from aa in DB.Rate_Template_Line_item
            //                             where aa.Rate_Template_Code == RTemplateCode
            //                             orderby aa.SKU_Name
            //                             select aa
            //                   //          new 
            //                   //          {
            //                   //              RT_Line_Id = a.RT_Line_Id,
            //                   //              RT_id = a.RT_id,
            //                   //              Rate_Template_Code = a.Rate_Template_Code,
            //                   //              Material_Code = a.Material_Code,
            //                   //              Material_Auto_Num_Code = a.Material_Auto_Num_Code,
            //                   //              SKU_Id = a.SKU_Id,
            //                   //              SKU_Code = a.SKU_Code,
            //                   //              SKU_Name = a.SKU_Name,
            //                   //              SKU_SubType_Id = a.SKU_SubType_Id,
            //                   //              SKU_SubType = a.SKU_SubType,
            //                   //              Pack_Type_Id = a.Pack_Type_Id,
            //                   //              Pack_Type = a.Pack_Type,
            //                   //              Pack_Size = a.Pack_Size,
            //                   //              Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
            //                   //              Pack_Weight_Type = a.Pack_Weight_Type,
            //                   //              UOM = a.UOM,
            //                   //              Grade = a.Grade,
            //                   //              Selling_price = a.Selling_price,
            //                   //              MRP = a.MRP,
            //                   //              //  Price = a.Price,
            //                   //              CreatedDate = a.CreatedDate,
            //                   //              UpdateDate = a.UpdateDate,
            //                   //              CreateBy = a.CreateBy,
            //                   //              UpdateBy = a.UpdateBy,
            //                   //}
            //                   ).ToList();

            DateTime today = DateTime.Now;
            var list = (from x in DB.Customer_Indent
                        where x.Indent_Code == id
                        select new CIEntity
                        {
                            Indent_ID = x.Indent_ID,
                            Indent_Code = x.Indent_Code,
                            Indent_Name = x.Indent_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Region_Code = x.Region_Code,
                            Indent_Type = x.Indent_Type,
                            Customer_Id = x.Customer_Id,
                            Customer_Code = x.Customer_Code,
                            Customer_Name = x.Customer_Name,
                            Customer_Delivery_Address = x.Customer_Delivery_Address,
                            Dispatch_DC_Code = x.Dispatch_DC_Code,
                            Delivery_cycle = x.Delivery_cycle,
                            Delivery_Expected_Date = x.Delivery_Expected_Date,
                            Delivery_Type = x.Delivery_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Price_Template_ID = x.Price_Template_ID,
                            Price_Template_Name = x.Price_Template_Name,
                            Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                            Price_Template_Code = x.Price_Template_Code,
                            //  Price_Template_Code = DB.Rate_Template.Where(t => t.Template_ID == x.Price_Template_ID).Select(t => t.Template_Code).FirstOrDefault(),
                            Validatity_Date = DB.Rate_Template.Where(t => t.Template_Code == x.Price_Template_Code).Select(t => t.Valitity_upto).FirstOrDefault(),
                            CreatedDate = x.CreatedDate,
                            UpdateDate = x.UpdateDate,
                            CreateBy = x.CreateBy,
                            UpdateBy = x.UpdateBy,
                            LineItems = (from a in DB.Customer_Indent_Line_item
                                         orderby a.SKU_Name
                                         where a.Indent_ID == x.Indent_ID
                                         select new CustomerIndentLineItemEntity
                                         {
                                             CI_Line_Id = a.CI_Line_Id,
                                             Indent_ID = a.Indent_ID,
                                             Indent_Code = a.Indent_Code,
                                             SKU_Id = a.SKU_Id,
                                             SKU_Name = a.SKU_Name,
                                             SKU_SubType_Id = a.SKU_SubType_Id,
                                             SKU_SubType = a.SKU_SubType,
                                             Pack_Type_Id = a.Pack_Type_Id,
                                             Pack_Type = a.Pack_Type,
                                             HSN_Code = a.HSN_Code,
                                             Total_GST = a.Total_GST,
                                             CGST = a.CGST,
                                             SGST = a.SGST,
                                             Pack_Size = a.Pack_Size,
                                             Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
                                             Pack_Weight_Type = a.Pack_Weight_Type,
                                             UOM = a.UOM,
                                             Grade = a.Grade,
                                             Estimated_Stock_Qty = DB.Estimated_Stock.Where(u => u.DC_Code == x.DC_Code && u.SKU_Id == a.SKU_Id && u.SKU_Type_Id == x.SKU_Type_Id && u.Grade == a.Grade && u.UOM == a.UOM && u.SKU_Name.ToLower() == a.SKU_Name.ToLower() && u.CreatedDate.Value.Year==today.Year && u.CreatedDate.Value.Month==today.Month && u.CreatedDate.Value.Day==today.Day).Select(u => u.Closing_Qty.Value).FirstOrDefault(),
                                             Price = DB.Rate_Template_Line_item.Where(u => u.RT_id == x.Price_Template_ID && u.SKU_Id == a.SKU_Id && u.SKU_SubType_Id == a.SKU_SubType_Id && u.Pack_Type_Id == a.Pack_Type_Id && u.Pack_Size == a.Pack_Size && u.Grade == a.Grade).Select(u => u.Selling_price).FirstOrDefault(),
                                             Dispatch_Qty = a.Dispatch_Qty,
                                             CreatedDate = a.CreatedDate,
                                             UpdateDate = a.UpdateDate,
                                             CreateBy = a.CreateBy,
                                             UpdateBy = a.UpdateBy,
                                         }).ToList(),

                        }).ToList();

            return list;
        }

        //------------------------DELETEPURCHASEORDER----------

        public bool DeleteCustomerIndent(string tId, string deleteReason)
        {
            var success = false;
            if (tId != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = (from f in DB.Customer_Indent
                             where f.Indent_Code == tId
                             select f).FirstOrDefault();
                    if (p != null)
                    {
                        p.Is_Sync = false;
                        p.Is_Deleted = true;
                        p.Reason = deleteReason;
                        _unitOfWork.CustomerIndentRepository.Update(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }


        public bool DeleteCustomerIndentLineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.CustomerIndentLineItemRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.CustomerIndentLineItemRepository.Delete(p);
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
