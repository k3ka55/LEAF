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
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace BusinessServices
{
    public class SaleIndentService : ISaleIndentService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public SaleIndentService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public List<BulkCSIExcelFieldsReturnModel> BulkCSI(fileImportBulkCSI fileDetail)
        {
            List<BulkCSIExcelFieldsReturnModel> stilist = new List<BulkCSIExcelFieldsReturnModel>();
            BulkCSIExcelFieldsReturnModel miDetail = new BulkCSIExcelFieldsReturnModel();

            // List<BULKCSIExcelFields> milist = new List<BULKCSIExcelFields>();
            List<BulkCSIExcelFieldsReturnModel> result = new List<BulkCSIExcelFieldsReturnModel>();

            //List<BULKCSIExcelFields> Intersect = new List<BULKCSIExcelFields>();
            //List<BULKCSIExcelFields> Except = new List<BULKCSIExcelFields>();
            //List<BULKCSIExcelFields> RawB = new List<BULKCSIExcelFields>();

            List<BulkCSIExcelFieldsReturn> EntireRows = new List<BulkCSIExcelFieldsReturn>();
            List<BulkCSIExcelFieldsReturn> Valid = new List<BulkCSIExcelFieldsReturn>();
            List<BulkCSIExcelFieldsReturn> Notvalid = new List<BulkCSIExcelFieldsReturn>();



            //List<BulkCSIExcelFields> RawB = new List<BulkCSIExcelFields>();
            //List<BulkCSIExcelFields> RawB = new List<BulkCSIExcelFields>();



            string Profilepicname = "BULKCSI_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/BULKCSI";
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

                if (columnCount > 16)
                {
                    miDetail.status = false;
                    miDetail.Message = "Extra Column's Present in Given Excel";
                    stilist.Add(miDetail);
                    return stilist;
                }

                //--------------------validating Excel Columns
                int lineCount = 1;
                int lineNumber = 2;
                if (result1 != null)
                {
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        BulkCSIExcelFieldsReturn csi = new BulkCSIExcelFieldsReturn();

                        csi.Dispatch_DC_Location_Code = d["Dispatch_DC_Location_Code"] != null && d["Dispatch_DC_Location_Code"].ToString() != "" ? d["Dispatch_DC_Location_Code"].ToString() : "";
                        csi.Delievery_Date = d["Delievery_Date"] != null && d["Delievery_Date"].ToString() != "" ? d["Delievery_Date"].ToString() : "";
                        csi.Customer_Name = d["Customer_Name"] != null && d["Customer_Name"].ToString() != "" ? d["Customer_Name"].ToString() : "";
                        csi.Customer_Code = d["Customer_Code"] != null && d["Customer_Code"].ToString() != "" ? d["Customer_Code"].ToString() : "";
                        csi.Indent_Template_Name = d["Indent_Template_Name"] != null && d["Indent_Template_Name"].ToString() != "" ? d["Indent_Template_Name"].ToString() : "";
                        csi.Price_Template_Name = d["Price_Template_Name"] != null && d["Price_Template_Name"].ToString() != "" ? d["Price_Template_Name"].ToString() : "";
                        csi.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        csi.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
                        csi.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        csi.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        csi.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        csi.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        csi.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        csi.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? d["Dispatch_Qty"].ToString() : "";
                        EntireRows.Add(csi);
                        lineCount += 1;
                    }

                }

                if (EntireRows.Count != 0)
                {

                    foreach (var datas in EntireRows)
                    {

                        if (datas.Dispatch_Qty == "")
                        {
                            datas.status = false;
                            datas.Message = "Dispatch_Qty value is not a valid number.(Excel Line No: " + lineNumber + ")";
                            Notvalid.Add(datas);
                        }
                        else
                        {
                            string num = datas.Dispatch_Qty;
                            bool s = Regex.IsMatch(num, @"^-?[0-9]\d*(\.\d+)?$");
                            // @"^\d+$"
                            if (s != true)
                            {
                                datas.status = false;
                                datas.Message = "Dispatch_Qty value is not a valid number.(Excel Line No: " + lineNumber + ")";
                                Notvalid.Add(datas);
                            }
                            else
                            {
                                int no = int.Parse(num);
                                if (no < 0)
                                {
                                    datas.status = false;
                                    datas.Message = "Dispatch_Qty value is less than 0.(Excel Line No: " + lineNumber + ")";
                                    Notvalid.Add(datas);
                                }
                                else
                                {

                                    if (datas.Delievery_Date == "")
                                    {
                                        datas.status = false;
                                        datas.Message = "Delievery_Date value is Empty.(Excel Line No: " + lineNumber + ")";
                                        Notvalid.Add(datas);
                                    }
                                    else
                                    {
                                        datas.Delievery_Date = datas.Delievery_Date;
                                        if (datas.Dispatch_DC_Location_Code == "")
                                        {
                                            datas.status = false;
                                            datas.Message = "Dispatch_DC_Location_Code is Empty.(Excel Line No: " + lineNumber + ")";
                                            Notvalid.Add(datas);
                                        }
                                        else
                                        {
                                            datas.Dispatch_DC_Location_Code = datas.Dispatch_DC_Location_Code;
                                            if (datas.Customer_Name == "")
                                            {
                                                datas.status = false;
                                                datas.Message = "Customer_Name value is Empty.(Excel Line No: " + lineNumber + ")";
                                                Notvalid.Add(datas);
                                            }
                                            else
                                            {
                                                var CustName = DB.Customers.Where(a => a.Customer_Name.ToLower() == datas.Customer_Name.ToLower() && a.Customer_Code.ToLower() == datas.Customer_Code.ToLower() && a.Is_Delete != true).FirstOrDefault();
                                                if (CustName == null)
                                                {
                                                    datas.status = false;
                                                    datas.Message = "Customer_Name/Customer_Code not valid.(Excel Line No: " + lineNumber + ")";
                                                    Notvalid.Add(datas);
                                                }
                                                else
                                                {
                                                    datas.Customer_Name = CustName.Customer_Name;
                                                    datas.Customer_Code = CustName.Customer_Code;
                                                    datas.Customer_Id = CustName.Cust_Id;
                                                    if (datas.Customer_Code == "")
                                                    {
                                                        datas.status = false;
                                                        datas.Message = "Customer_Code value is Empty.(Excel Line No: " + lineNumber + ")";
                                                        Notvalid.Add(datas);
                                                    }
                                                    else
                                                    {
                                                        if (datas.Indent_Template_Name == "")
                                                        {
                                                            datas.status = false;
                                                            datas.Message = "Indent_Template_Name value is Empty.(Excel Line No: " + lineNumber + ")";
                                                            Notvalid.Add(datas);
                                                        }
                                                        else
                                                        {
                                                            var LineHeader = DB.Customer_Indent.Where(dd => dd.Indent_Name.ToLower().Trim() == datas.Indent_Template_Name.ToLower().Trim() && dd.Is_Deleted != true).FirstOrDefault();

                                                            if (LineHeader == null)
                                                            {
                                                                datas.status = false;
                                                                datas.Message = "Line Item is not available in Customer Indent.(Excel Line No: " + lineNumber + ")";
                                                                Notvalid.Add(datas);
                                                            }
                                                            else
                                                            {
                                                                datas.Indent_Template_Name = LineHeader.Indent_Name;
                                                                datas.Indent_Type = LineHeader.Indent_Type;
                                                                datas.SKU_Type = LineHeader.SKU_Type;
                                                                datas.Delivery_cycle = LineHeader.Delivery_cycle;
                                                                datas.Delivery_Type = LineHeader.Delivery_Type;
                                                                datas.Indent_Code = LineHeader.Indent_Code;
                                                                datas.Price_Template_Code = LineHeader.Price_Template_Code;
                                                                datas.SKU_Type_Id = LineHeader.SKU_Type_Id;
                                                                datas.Indent_ID = LineHeader.Indent_ID;
                                                                datas.Price_Template_ID = LineHeader.Price_Template_ID;
                                                                datas.User_Location_Code = ((LineHeader.DC_Code != null && LineHeader.DC_Code != "null") ? LineHeader.DC_Code : LineHeader.Location_Code);
                                                                datas.User_Location_Id = (LineHeader.DC_Id != null ? LineHeader.DC_Id : LineHeader.Location_Id);
                                                                datas.User_Location = ((LineHeader.DC_Code != null && LineHeader.DC_Code != "null") ? (DB.DC_Master.Where(dd => dd.DC_Code == LineHeader.DC_Code).Select(dd => dd.Dc_Name)).FirstOrDefault() : (DB.Location_Master.Where(dd => dd.Location_Code == LineHeader.Location_Code).Select(dd => dd.Location_Name)).FirstOrDefault());
                                                                datas.User_Type = (((LineHeader.DC_Code != null && LineHeader.DC_Code != "null")) ? "DC" : "LOCATION");

                                                                if (datas.Price_Template_Name == "")
                                                                {
                                                                    datas.status = false;
                                                                    datas.Message = "Price_Template_Name value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                    Notvalid.Add(datas);
                                                                }
                                                                else
                                                                {

                                                                    var PriceTemplateDetails = DB.Customer_Indent.Where(x => x.Indent_Name.ToLower() == datas.Indent_Template_Name.ToLower() && x.Price_Template_Name.ToLower() == datas.Price_Template_Name.ToLower() && x.Is_Deleted != true).FirstOrDefault();
                                                                    if (PriceTemplateDetails == null)
                                                                    {
                                                                        datas.status = false;
                                                                        datas.Message = "Price_Template_Name/Indent_Template_Name not valid.(Excel Line No: " + lineNumber + ")";
                                                                        Notvalid.Add(datas);
                                                                    }
                                                                    else
                                                                    {
                                                                        //   datas.Price_Template_Name = datas.Price_Template_Name;
                                                                        if (datas.SKU_SubType == "")
                                                                        {
                                                                            datas.status = false;
                                                                            datas.Message = "SKU_SubType value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                            Notvalid.Add(datas);
                                                                        }
                                                                        else
                                                                        {
                                                                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == datas.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                                                                            if (subtypedetail == null)
                                                                            {
                                                                                datas.status = false;
                                                                                datas.Message = "SKU_SubType_Name not valid.(Excel Line No: " + lineNumber + ")";
                                                                                Notvalid.Add(datas);
                                                                            }
                                                                            else
                                                                            {
                                                                                datas.SKU_SubType = subtypedetail.SKU_SubType_Name;
                                                                                datas.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;
                                                                                if (datas.SKU_Name == "")
                                                                                {
                                                                                    datas.status = false;
                                                                                    datas.Message = "SKU_Name value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                    Notvalid.Add(datas);
                                                                                }
                                                                                else
                                                                                {
                                                                                    var SKUDetails = DB.SKU_Master.Where(a => a.SKU_Name.ToLower() == datas.SKU_Name.ToLower()).FirstOrDefault();
                                                                                    if (SKUDetails == null)
                                                                                    {
                                                                                        datas.status = false;
                                                                                        datas.Message = "SKU_Name not valid.(Excel Line No: " + lineNumber + ")";
                                                                                        Notvalid.Add(datas);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        datas.SKU_Name = SKUDetails.SKU_Name;
                                                                                        datas.SKU_Id = SKUDetails.SKU_Id;
                                                                                        datas.HSN_Code = SKUDetails.HSN_Code;
                                                                                        datas.Total_GST = SKUDetails.Total_GST;
                                                                                        datas.CGST = SKUDetails.CGST;
                                                                                        datas.SGST = SKUDetails.SGST;
                                                                                        if (datas.Grade == "")
                                                                                        {
                                                                                            datas.status = false;
                                                                                            datas.Message = "Grade value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                            Notvalid.Add(datas);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == datas.Grade.ToLower().Trim()).FirstOrDefault();
                                                                                            if (gradeDetail == null)
                                                                                            {
                                                                                                datas.status = false;
                                                                                                datas.Message = "Grade value not valid.(Excel Line No: " + lineNumber + ")";
                                                                                                Notvalid.Add(datas);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                datas.Grade = gradeDetail.GradeType_Name;
                                                                                                if (datas.Pack_Type == "")
                                                                                                {
                                                                                                    datas.status = false;
                                                                                                    datas.Message = "Pack_Type value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                                    Notvalid.Add(datas);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == datas.Pack_Type.ToLower().Trim()).FirstOrDefault();
                                                                                                    if (packtypedetail == null)
                                                                                                    {
                                                                                                        datas.status = false;
                                                                                                        datas.Message = "Pack_Type not valid.(Excel Line No: " + lineNumber + ")";
                                                                                                        Notvalid.Add(datas);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        datas.Pack_Type = packtypedetail.Pack_Type_Name;
                                                                                                        datas.Pack_Type_Id = packtypedetail.Pack_Type_Id;
                                                                                                        if (datas.Pack_Size == "")
                                                                                                        {
                                                                                                            datas.status = false;
                                                                                                            datas.Message = "Pack_Size value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                                            Notvalid.Add(datas);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == datas.Pack_Size.ToLower().Trim()).FirstOrDefault();
                                                                                                            if (packsizedetail == null)
                                                                                                            {
                                                                                                                datas.status = false;
                                                                                                                datas.Message = "Pack_Size not valid.(Excel Line No: " + lineNumber + ")";
                                                                                                                Notvalid.Add(datas);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                datas.Pack_Size = packsizedetail.Pack_Size_Value;
                                                                                                                if (datas.Pack_Weight_Type == "")
                                                                                                                {
                                                                                                                    datas.status = false;
                                                                                                                    datas.Message = "Pack_Weight_Type value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                                                    Notvalid.Add(datas);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == datas.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                                                                                                                    if (packweightType == null)
                                                                                                                    {
                                                                                                                        datas.status = false;
                                                                                                                        datas.Message = "Pack_Weight_Type not valid.(Excel Line No: " + lineNumber + ")";
                                                                                                                        Notvalid.Add(datas);
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        datas.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                                                                                                                        datas.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
                                                                                                                        if (datas.UOM == "")
                                                                                                                        {
                                                                                                                            datas.status = false;
                                                                                                                            datas.Message = "UOM value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                                                            Notvalid.Add(datas);
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == datas.UOM.ToLower().Trim()).FirstOrDefault();
                                                                                                                            if (uomDetail == null)
                                                                                                                            {
                                                                                                                                datas.status = false;
                                                                                                                                datas.Message = "UOM not valid.(Excel Line No: " + lineNumber + ")";
                                                                                                                                Notvalid.Add(datas);
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                datas.UOM = uomDetail.Unit_Name;
                                                                                                                                if (datas.Dispatch_Qty == "")
                                                                                                                                {
                                                                                                                                    datas.status = false;
                                                                                                                                    datas.Message = "Dispatch_Qty value is Empty.(Excel Line No: " + lineNumber + ")";
                                                                                                                                    Notvalid.Add(datas);
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    if (datas.Dispatch_Qty == "0")
                                                                                                                                    {
                                                                                                                                        datas.status = false;
                                                                                                                                        datas.Message = "Please check Dispatch_Qty column.It contains 0 Qty.(Excel Line No: " + lineNumber + ")";
                                                                                                                                        Notvalid.Add(datas);
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        datas.Dispatch_Qty = datas.Dispatch_Qty;

                                                                                                                                        var Customer = DB.Customer_Indent.Where(a => a.Indent_Name.ToLower().Trim() == datas.Indent_Template_Name.ToLower().Trim() && a.Customer_Code.ToLower() == datas.Customer_Code.ToLower() && a.Is_Deleted != true).FirstOrDefault();
                                                                                                                                        if (Customer != null)
                                                                                                                                        {
                                                                                                                                            datas.Customer_Code = Customer.Customer_Code;
                                                                                                                                            datas.Customer_Name = Customer.Customer_Name;
                                                                                                                                            datas.Customer_Id = Customer.Customer_Id;
                                                                                                                                            int mic = (from mm in DB.Customer_Indent
                                                                                                                                                       join nn in DB.Customer_Indent_Line_item on mm.Indent_Code equals nn.Indent_Code
                                                                                                                                                       where mm.Is_Deleted != true && mm.Indent_Name == datas.Indent_Template_Name && mm.Price_Template_Name == datas.Price_Template_Name && nn.SKU_SubType == datas.SKU_SubType && nn.SKU_Name == datas.SKU_Name && nn.Grade == datas.Grade && nn.Pack_Type == datas.Pack_Type && nn.Pack_Size == datas.Pack_Size && nn.Pack_Weight_Type == datas.Pack_Weight_Type && nn.UOM == datas.UOM && mm.Indent_Type == datas.Indent_Type && mm.SKU_Type == datas.SKU_Type && nn.Pack_Weight_Type_Id == datas.Pack_Weight_Type_Id && nn.SKU_Id == datas.SKU_Id && nn.SKU_SubType_Id == datas.SKU_SubType_Id && nn.Pack_Type_Id == datas.Pack_Type_Id
                                                                                                                                                       select mm).Count();

                                                                                                                                            if (mic > 0)
                                                                                                                                                Valid.Add(datas);
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                datas.status = false;
                                                                                                                                                datas.Message = "Line_Item not matched with Customer Indent Template.(Excel Line No: " + lineNumber + ")";
                                                                                                                                                Notvalid.Add(datas);
                                                                                                                                            }
                                                                                                                                        }

                                                                                                                                        else if (Customer == null)
                                                                                                                                        {
                                                                                                                                            var milkrun = DB.Customer_Indent_Template_Mapping.Where(dd => dd.Indent_Name.ToLower().Trim() == datas.Indent_Template_Name.ToLower().Trim() && dd.Customer_Name.ToLower() == datas.Customer_Name.ToLower()).FirstOrDefault();
                                                                                                                                            if (milkrun != null)
                                                                                                                                            {
                                                                                                                                                datas.Customer_Code = datas.Customer_Code;
                                                                                                                                                datas.Customer_Name = milkrun.Customer_Name;
                                                                                                                                                datas.Customer_Id = milkrun.Customer_Id;
                                                                                                                                                int mic = (from mm in DB.Customer_Indent
                                                                                                                                                           join nn in DB.Customer_Indent_Line_item on mm.Indent_Code equals nn.Indent_Code
                                                                                                                                                           where mm.Is_Deleted != true && mm.Indent_Name == datas.Indent_Template_Name && mm.Price_Template_Name == datas.Price_Template_Name && nn.SKU_SubType == datas.SKU_SubType && nn.SKU_Name == datas.SKU_Name && nn.Grade == datas.Grade && nn.Pack_Type == datas.Pack_Type && nn.Pack_Size == datas.Pack_Size && nn.Pack_Weight_Type == datas.Pack_Weight_Type && nn.UOM == datas.UOM && mm.Indent_Type == datas.Indent_Type && mm.SKU_Type == datas.SKU_Type && nn.Pack_Weight_Type_Id == datas.Pack_Weight_Type_Id && nn.SKU_Id == datas.SKU_Id && nn.SKU_SubType_Id == datas.SKU_SubType_Id && nn.Pack_Type_Id == datas.Pack_Type_Id
                                                                                                                                                           select mm).Count();

                                                                                                                                                if (mic > 0)
                                                                                                                                                    Valid.Add(datas);
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    datas.status = false;
                                                                                                                                                    datas.Message = "Line_Item not matched with Customer Indent Template.(Excel Line No: " + lineNumber + ")";
                                                                                                                                                    Notvalid.Add(datas);
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                datas.status = false;
                                                                                                                                                datas.Message = datas.Customer_Name + " (Customer Not Mapped with Indent Template).(Excel Line No: " + lineNumber + ")";
                                                                                                                                                Notvalid.Add(datas);

                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }





                        }


                        lineNumber += 1;
                    }

                }


                excelReader.Close();
                BulkCSIExcelFieldsReturnModel Ex = new BulkCSIExcelFieldsReturnModel();
                Ex.ValidDatas = Valid.ToList();
                Ex.InvalidDatas = Notvalid.ToList();
                Ex.InvalidLinesCount = Ex.InvalidDatas.Count();
                Ex.ValidLinesCount = Ex.ValidDatas.Count();
                Ex.TotalLinesCount = lineNumber - 2;
                Ex.Uploaded_Excel_Name = name;
                Ex.status = true;
                Ex.Message = "Success";
                result.Add(Ex);
            }
            catch (Exception e)
            {
                stilist = new List<BulkCSIExcelFieldsReturnModel>();
                miDetail.status = false;
                miDetail.Message = e.Message.ToString();
                stilist.Add(miDetail);
                return stilist;
            }
            //stilist[0].status = true;
            //stilist[0].Message = "Success";

            return result;
        }



        //if (result1 != null)

        //    foreach (DataRow d in result1.Tables[0].Rows)
        //    {
        //        CSIExcelFields misDetail = new CSIExcelFields();

        //        misDetail.Dispatch_DC_Location_Code = d["Dispatch_DC_Location_Code"] != null && d["Dispatch_DC_Location_Code"].ToString() != "" ? d["Dispatch_DC_Location_Code"].ToString() : "";
        //        // misDetail.Region = d["Region"] != null && d["Region"].ToString() != "" ? d["Region"].ToString() : "";
        //        misDetail.Delievery_Date = d["Delievery_Date"] != null && d["Delievery_Date"].ToString() != "" ? d["Delievery_Date"].ToString() : "";
        //        misDetail.Customer_Name = d["Customer_Name"] != null && d["Customer_Name"].ToString() != "" ? d["Customer_Name"].ToString() : "";
        //        misDetail.Customer_Code = d["Customer_Code"] != null && d["Customer_Code"].ToString() != "" ? d["Customer_Code"].ToString() : "";
        //        misDetail.Indent_Template_Name = d["Indent_Template_Name"] != null && d["Indent_Template_Name"].ToString() != "" ? d["Indent_Template_Name"].ToString() : "";
        //        misDetail.Price_Template_Name = d["Price_Template_Name"] != null && d["Price_Template_Name"].ToString() != "" ? d["Price_Template_Name"].ToString() : "";
        //        misDetail.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
        //        misDetail.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
        //        misDetail.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
        //        misDetail.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
        //        misDetail.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
        //        misDetail.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
        //        misDetail.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
        //        misDetail.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? d["Dispatch_Qty"].ToString() : "";


        //        using (var iscope = new TransactionScope())
        //        {
        //            misDetail.Dispatch_DC_Location_Code = misDetail.Dispatch_DC_Location_Code;
        //            misDetail.Customer_Name = misDetail.Customer_Name;
        //            misDetail.Customer_Code = misDetail.Customer_Code;
        //            misDetail.Indent_Template_Name = misDetail.Indent_Template_Name;
        //            misDetail.Price_Template_Name = misDetail.Price_Template_Name;

        //            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == misDetail.SKU_Name.ToLower().Trim()).FirstOrDefault();
        //            misDetail.SKU_Name = skuDetail.SKU_Name;
        //            misDetail.SKU_Id = skuDetail.SKU_Id;

        //            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == misDetail.SKU_SubType.ToLower().Trim()).FirstOrDefault();
        //            misDetail.SKU_SubType = subtypedetail.SKU_SubType_Name;
        //            misDetail.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

        //            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == misDetail.Pack_Type.ToLower().Trim()).FirstOrDefault();
        //            misDetail.Pack_Type = packtypedetail.Pack_Type_Name;
        //            misDetail.Pack_Type_Id = packtypedetail.Pack_Type_Id;

        //            var Customer = DB.Customer_Indent.Where(a => a.Indent_Name.ToLower().Trim() == misDetail.Indent_Template_Name.ToLower().Trim() && a.Customer_Code.ToLower() == misDetail.Customer_Code.ToLower() && a.Is_Deleted != true).FirstOrDefault();
        //            if (Customer != null)
        //            {
        //                misDetail.Customer_Code = Customer.Customer_Code;
        //                misDetail.Customer_Name = Customer.Customer_Name;
        //                misDetail.Customer_Id = Customer.Customer_Id;
        //            }

        //            else if (Customer == null)
        //            {
        //                var milkrun = DB.Customer_Indent_Template_Mapping.Where(dd => dd.Indent_Name.ToLower().Trim() == misDetail.Indent_Template_Name.ToLower().Trim() && dd.Customer_Name.ToLower() == misDetail.Customer_Name.ToLower()).FirstOrDefault();
        //                if (milkrun != null)
        //                {
        //                    misDetail.Customer_Code = misDetail.Customer_Code;
        //                    misDetail.Customer_Name = milkrun.Customer_Name;
        //                    misDetail.Customer_Id = milkrun.Customer_Id;
        //                }
        //            }
        //            else
        //            {
        //                miDetail.status = false;
        //                miDetail.lineNumber = lineCount;
        //                miDetail.Message = "Error";
        //                miDetail.errorItem = "Customer Not Mapped with Indent Template";
        //                stilist.Add(miDetail);
        //                return stilist;
        //            }


        //            var LineHeader = DB.Customer_Indent.Where(dd => dd.Indent_Name.ToLower().Trim() == misDetail.Indent_Template_Name.ToLower().Trim() && dd.Is_Deleted != true).FirstOrDefault();
        //            if (LineHeader != null)
        //            {
        //                misDetail.Indent_Template_Name = LineHeader.Indent_Name;
        //                misDetail.Indent_Type = LineHeader.Indent_Type;
        //                misDetail.SKU_Type = LineHeader.SKU_Type;
        //                misDetail.Delivery_cycle = LineHeader.Delivery_cycle;
        //                misDetail.Delivery_Type = LineHeader.Delivery_Type;
        //                misDetail.Indent_Code = LineHeader.Indent_Code;
        //                misDetail.Price_Template_Code = LineHeader.Price_Template_Code;
        //                misDetail.SKU_Type_Id = LineHeader.SKU_Type_Id;
        //                misDetail.Indent_ID = LineHeader.Indent_ID;
        //                misDetail.Price_Template_ID = LineHeader.Price_Template_ID;
        //                misDetail.User_Location_Code = ((LineHeader.DC_Code != null && LineHeader.DC_Code != "null") ? LineHeader.DC_Code : LineHeader.Location_Code);
        //                misDetail.User_Location_Id = (LineHeader.DC_Id != null ? LineHeader.DC_Id : LineHeader.Location_Id);
        //                misDetail.User_Location = ((LineHeader.DC_Code != null && LineHeader.DC_Code != "null") ? (DB.DC_Master.Where(dd => dd.DC_Code == LineHeader.DC_Code).Select(dd => dd.Dc_Name)).FirstOrDefault() : (DB.Location_Master.Where(dd => dd.Location_Code == LineHeader.Location_Code).Select(dd => dd.Location_Name)).FirstOrDefault());
        //                misDetail.User_Type = (((LineHeader.DC_Code != null && LineHeader.DC_Code != "null")) ? "DC" : "LOCATION");
        //            }
        //            else
        //            {
        //                miDetail.status = false;
        //                miDetail.Message = "Line Item is not available in Customer Indent.Line Id:" + lineCount;
        //                stilist.Add(miDetail);
        //                return stilist;
        //            }
        //            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == misDetail.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
        //            misDetail.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
        //            misDetail.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

        //            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == misDetail.Grade.ToLower().Trim()).FirstOrDefault();
        //            misDetail.Grade = gradeDetail.GradeType_Name;

        //            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == misDetail.UOM.ToLower().Trim()).FirstOrDefault();
        //            misDetail.UOM = uomDetail.Unit_Name;

        //            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == misDetail.Pack_Size.ToLower().Trim()).FirstOrDefault();
        //            misDetail.Pack_Size = packsizedetail.Pack_Size_Value;

        //            misDetail.Dispatch_Qty = misDetail.Dispatch_Qty;

        //            stilist.Add(misDetail);

        //            iscope.Complete();
        //        }

        //        lineCount += 1;
        //    }



        //foreach (var t in stilist)
        //{
        //    BulkCSIExcelFields ex = new BulkCSIExcelFields();
        //    // ex.Region = t.Region;
        //    ex.Dispatch_DC_Location_Code = t.Dispatch_DC_Location_Code;
        //    ex.Customer_Name = t.Customer_Name;
        //    ex.Customer_Code = t.Customer_Code;
        //    ex.Indent_Template_Name = t.Indent_Template_Name;
        //    ex.Price_Template_Name = t.Price_Template_Name;
        //    ex.SKU_SubType = t.SKU_SubType;
        //    ex.SKU_Name = t.SKU_Name;
        //    ex.Grade = t.Grade;
        //    ex.Pack_Type = t.Pack_Type;
        //    ex.Pack_Size = t.Pack_Size;
        //    ex.Pack_Weight_Type = t.Pack_Weight_Type;
        //    ex.Delievery_Date = t.Delievery_Date;
        //    ex.Dispatch_Qty = t.Dispatch_Qty;
        //    ex.UOM = t.UOM;
        //    ex.Indent_Type = t.Indent_Type;
        //    ex.SKU_Type = t.SKU_Type;
        //    ex.Pack_Weight_Type_Id = t.Pack_Weight_Type_Id;
        //    ex.SKU_Id = t.SKU_Id;
        //    ex.SKU_SubType_Id = t.SKU_SubType_Id;
        //    ex.Pack_Type_Id = t.Pack_Type_Id;
        //    ex.Indent_Type = t.Indent_Type;
        //    ex.SKU_Type = t.SKU_Type;
        //    ex.Delivery_cycle = t.Delivery_cycle;
        //    ex.Delivery_Type = t.Delivery_Type;
        //    ex.Indent_Code = t.Indent_Code;
        //    ex.Price_Template_Code = t.Price_Template_Code;
        //    ex.Customer_Id = t.Customer_Id;
        //    ex.SKU_Type_Id = t.SKU_Type_Id;
        //    ex.Indent_ID = t.Indent_ID;
        //    ex.Price_Template_ID = t.Price_Template_ID;
        //    ex.User_Location_Code = t.User_Location_Code;
        //    ex.User_Location_Id = t.User_Location_Id;
        //    ex.User_Location = t.User_Location;
        //    ex.User_Type = t.User_Type;
        //    RawB.Add(ex);
        //}

        //foreach (var y in RawB)
        //{
        //    int mic =
        //        (from mm in DB.Customer_Indent
        //         join nn in DB.Customer_Indent_Line_item on mm.Indent_Code equals nn.Indent_Code
        //         where mm.Is_Deleted != true && mm.Indent_Name == y.Indent_Template_Name && mm.Price_Template_Name == y.Price_Template_Name && nn.SKU_SubType == y.SKU_SubType && nn.SKU_Name == y.SKU_Name && nn.Grade == y.Grade && nn.Pack_Type == y.Pack_Type && nn.Pack_Size == y.Pack_Size && nn.Pack_Weight_Type == y.Pack_Weight_Type && nn.UOM == y.UOM && mm.Indent_Type == y.Indent_Type && mm.SKU_Type == y.SKU_Type && nn.Pack_Weight_Type_Id == y.Pack_Weight_Type_Id && nn.SKU_Id == y.SKU_Id && nn.SKU_SubType_Id == y.SKU_SubType_Id && nn.Pack_Type_Id == y.Pack_Type_Id
        //         select mm).Count();

        //    if (mic > 0)
        //        Intersect.Add(y);
        //    else
        //    {
        //        Except.Add(y);
        //    }
        //}




        //public List<BulkCSIExcelFields> BulkCSI(fileImportBulkCSI fileDetail)
        //{
        //    List<BulkCSIExcelFields> stilist = new List<BulkCSIExcelFields>();
        //    List<BulkCSIExcelFields> milist = new List<BulkCSIExcelFields>();
        //    //List<BulkCSIExcelFields> miilist = new List<BulkCSIExcelFields>();
        //    List<BulkCSIExcelFields> result = new List<BulkCSIExcelFields>();
        //    BulkCSIExcelFields miDetail = new BulkCSIExcelFields();

        //    string Profilepicname = "BULKCSI_" + Guid.NewGuid().ToString();
        //    string sPath = "";
        //    string vPath = "";
        //    string name = "";
        //    string dPath = "~/Areas/BULKCSI";
        //    string dirCreatePath = "";
        //    try
        //    {
        //        string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

        //        string ext = ".xlsx";
        //        dirCreatePath = RootPath;

        //        if (!Directory.Exists(dirCreatePath))
        //        {
        //            Directory.CreateDirectory(RootPath);
        //        }
        //        sPath = RootPath;
        //        name = Profilepicname + ext;
        //        vPath = sPath + "\\" + name;

        //        if (File.Exists(vPath))
        //        {
        //            File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
        //        }
        //        else
        //        {
        //            File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
        //        }

        //        FileStream stream = File.Open(vPath, FileMode.Open, FileAccess.Read);
        //        stream.Position = 0;
        //        IExcelDataReader excelReader = null;

        //        if (ext == ".xls")
        //        {
        //            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        //        }
        //        else if (ext == ".xlsx")
        //        {
        //            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //        }

        //        excelReader.IsFirstRowAsColumnNames = true;
        //        DataSet result1 = excelReader.AsDataSet();

        //        int columnCount = result1.Tables[0].Columns.Count;

        //        if (columnCount > 16)
        //        {
        //            miDetail.status = false;
        //            miDetail.Message = "Extra Column's Present in Given Excel";
        //            stilist.Add(miDetail);
        //            return stilist;
        //        }
        //        //
        //        //--------------------validating Excel Columns
        //        int lineCount = 1;

        //        if (result1 != null)
        //            foreach (DataRow d in result1.Tables[0].Rows)
        //            {
        //                CSIExcelFields csi = new CSIExcelFields();

        //                csi.Dispatch_DC_Location_Code = d["Dispatch_DC_Location_Code"] != null && d["Dispatch_DC_Location_Code"].ToString() != "" ? d["Dispatch_DC_Location_Code"].ToString() : "";
        //                csi.Delievery_Date = d["Delievery_Date"] != null && d["Delievery_Date"].ToString() != "" ? d["Delievery_Date"].ToString() : "";
        //                csi.Customer_Name = d["Customer_Name"] != null && d["Customer_Name"].ToString() != "" ? d["Customer_Name"].ToString() : "";
        //                csi.Customer_Code = d["Customer_Code"] != null && d["Customer_Code"].ToString() != "" ? d["Customer_Code"].ToString() : "";
        //                csi.Indent_Template_Name = d["Indent_Template_Name"] != null && d["Indent_Template_Name"].ToString() != "" ? d["Indent_Template_Name"].ToString() : "";
        //                csi.Price_Template_Name = d["Price_Template_Name"] != null && d["Price_Template_Name"].ToString() != "" ? d["Price_Template_Name"].ToString() : "";
        //                csi.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
        //                csi.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
        //                csi.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
        //                csi.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
        //                csi.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
        //                csi.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
        //                csi.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
        //                csi.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? d["Dispatch_Qty"].ToString() : "";

        //                if (csi.Delievery_Date == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Delievery Date column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Dispatch_DC_Location_Code == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Dispatch_DC_Location_Code column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Customer_Name == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Customer_Name column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Customer_Code == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Customer_Code column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Indent_Template_Name == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Indent_Template_Name column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Price_Template_Name == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Price_Template_Name column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.SKU_SubType == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check SKU_SubType column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.SKU_Name == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check SKU_Name column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Grade == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Grade column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Pack_Type == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Pack_Type column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Pack_Size == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Pack_Size column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Pack_Weight_Type == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Pack_Weight_Type column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.UOM == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check UOM column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Dispatch_Qty == "")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Dispatch_Qty column.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                if (csi.Dispatch_Qty == "0")
        //                {
        //                    miDetail.status = false;
        //                    miDetail.Message = "Please check Dispatch_Qty column.It contains 0 Qty.";
        //                    stilist.Add(miDetail);
        //                    return stilist;
        //                }
        //                using (var iscope = new TransactionScope())
        //                {
        //                    var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == csi.SKU_SubType.ToLower().Trim()).FirstOrDefault();
        //                    if (subtypedetail != null)
        //                    {
        //                        csi.SKU_SubType = subtypedetail.SKU_SubType_Name;
        //                        csi.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "SKU_SubType_Name";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var LineHeader = DB.Customer_Indent.Where(dd => dd.Indent_Name.ToLower().Trim() == csi.Indent_Template_Name.ToLower().Trim() && dd.Is_Deleted != true).FirstOrDefault();
        //                    if (LineHeader != null)
        //                    {
        //                        csi.Indent_Template_Name = LineHeader.Indent_Name;
        //                        csi.Indent_Type = LineHeader.Indent_Type;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.Message = "Line Item is not available in Customer Indent.Line Id:" + lineCount;
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == csi.UOM.ToLower().Trim()).FirstOrDefault();
        //                    if (uomDetail != null)
        //                    {
        //                        csi.UOM = uomDetail.Unit_Name;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "UOM";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == csi.Pack_Type.ToLower().Trim()).FirstOrDefault();
        //                    if (packtypedetail != null)
        //                    {
        //                        csi.Pack_Type = packtypedetail.Pack_Type_Name;
        //                        csi.Pack_Type_Id = packtypedetail.Pack_Type_Id;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Pack_Type_Name";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == csi.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
        //                    if (packweightType != null)
        //                    {
        //                        csi.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
        //                        csi.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Pack_Weight_Type";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == csi.Grade.ToLower().Trim()).FirstOrDefault();
        //                    if (gradeDetail != null)
        //                    {
        //                        csi.Grade = gradeDetail.GradeType_Name;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Grade";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var CustName = DB.Customers.Where(a => a.Customer_Name.ToLower() == csi.Customer_Name.ToLower() && a.Customer_Code.ToLower() == csi.Customer_Code.ToLower() && a.Is_Delete != true).FirstOrDefault();
        //                    if (CustName != null)
        //                    {
        //                        csi.Customer_Name = CustName.Customer_Name;
        //                        csi.Customer_Code = CustName.Customer_Code;
        //                        csi.Customer_Id = CustName.Cust_Id;

        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = csi.Customer_Name.ToLower() + " or " + csi.Customer_Code.ToLower().Trim() + " (not valid Customer)";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var Customer = DB.Customer_Indent.Where(a => a.Indent_Name.ToLower().Trim() == csi.Indent_Template_Name.ToLower().Trim() && a.Customer_Code.ToLower() == csi.Customer_Code.ToLower() && a.Is_Deleted != true).FirstOrDefault();
        //                    if (Customer != null)
        //                    {
        //                        csi.Customer_Code = Customer.Customer_Code;
        //                        csi.Customer_Name = Customer.Customer_Name;
        //                        csi.Customer_Id = Customer.Customer_Id;
        //                    }

        //                    else if (Customer == null)
        //                    {
        //                        var milkrun = DB.Customer_Indent_Template_Mapping.Where(dd => dd.Indent_Name.ToLower().Trim() == csi.Indent_Template_Name.ToLower().Trim() && dd.Customer_Name.ToLower() == csi.Customer_Name.ToLower()).FirstOrDefault();
        //                        if (milkrun != null)
        //                        {
        //                            csi.Customer_Code = miDetail.Customer_Code;
        //                            csi.Customer_Name = milkrun.Customer_Name;
        //                            csi.Customer_Id = milkrun.Customer_Id;
        //                        }
        //                        else
        //                        {
        //                            miDetail.status = false;
        //                            miDetail.lineNumber = lineCount;
        //                            miDetail.Message = "Error";
        //                            miDetail.errorItem = csi.Customer_Name + " (Customer Not Mapped with Indent Template)";
        //                            stilist.Add(miDetail);
        //                            return stilist;
        //                        }
        //                    }

        //                    var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == csi.Pack_Size.ToLower().Trim()).FirstOrDefault();
        //                    if (packsizedetail != null)
        //                    {
        //                        csi.Pack_Size = packsizedetail.Pack_Size_Value;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Pack_Size_Value";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    iscope.Complete();
        //                }
        //                lineCount += 1;
        //            }
        //        if (result1 != null)

        //            foreach (DataRow d in result1.Tables[0].Rows)
        //            {
        //                BulkCSIExcelFields misDetail = new BulkCSIExcelFields();

        //                misDetail.Dispatch_DC_Location_Code = d["Dispatch_DC_Location_Code"] != null && d["Dispatch_DC_Location_Code"].ToString() != "" ? d["Dispatch_DC_Location_Code"].ToString() : "";
        //                // misDetail.Region = d["Region"] != null && d["Region"].ToString() != "" ? d["Region"].ToString() : "";
        //                misDetail.Delievery_Date = d["Delievery_Date"] != null && d["Delievery_Date"].ToString() != "" ? d["Delievery_Date"].ToString() : "";
        //                misDetail.Customer_Name = d["Customer_Name"] != null && d["Customer_Name"].ToString() != "" ? d["Customer_Name"].ToString() : "";
        //                misDetail.Customer_Code = d["Customer_Code"] != null && d["Customer_Code"].ToString() != "" ? d["Customer_Code"].ToString() : "";
        //                misDetail.Indent_Template_Name = d["Indent_Template_Name"] != null && d["Indent_Template_Name"].ToString() != "" ? d["Indent_Template_Name"].ToString() : "";
        //                misDetail.Price_Template_Name = d["Price_Template_Name"] != null && d["Price_Template_Name"].ToString() != "" ? d["Price_Template_Name"].ToString() : "";
        //                misDetail.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
        //                misDetail.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
        //                misDetail.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
        //                misDetail.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
        //                misDetail.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
        //                misDetail.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
        //                misDetail.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
        //                misDetail.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? d["Dispatch_Qty"].ToString() : "";


        //                using (var iscope = new TransactionScope())
        //                {
        //                    misDetail.Dispatch_DC_Location_Code = misDetail.Dispatch_DC_Location_Code;
        //                    misDetail.Customer_Name = misDetail.Customer_Name;
        //                    misDetail.Customer_Code = misDetail.Customer_Code;
        //                    misDetail.Indent_Template_Name = misDetail.Indent_Template_Name;
        //                    misDetail.Price_Template_Name = misDetail.Price_Template_Name;

        //                    var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == misDetail.SKU_Name.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.SKU_Name = skuDetail.SKU_Name;
        //                    misDetail.SKU_Id = skuDetail.SKU_Id;

        //                    var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == misDetail.SKU_SubType.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.SKU_SubType = subtypedetail.SKU_SubType_Name;
        //                    misDetail.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

        //                    var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == misDetail.Pack_Type.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Pack_Type = packtypedetail.Pack_Type_Name;
        //                    misDetail.Pack_Type_Id = packtypedetail.Pack_Type_Id;

        //                    var Customer = DB.Customer_Indent.Where(a => a.Indent_Name.ToLower().Trim() == misDetail.Indent_Template_Name.ToLower().Trim() && a.Customer_Code.ToLower() == misDetail.Customer_Code.ToLower() && a.Is_Deleted != true).FirstOrDefault();
        //                    if (Customer != null)
        //                    {
        //                        misDetail.Customer_Code = Customer.Customer_Code;
        //                        misDetail.Customer_Name = Customer.Customer_Name;
        //                        misDetail.Customer_Id = Customer.Customer_Id;
        //                    }

        //                    else if (Customer == null)
        //                    {
        //                        var milkrun = DB.Customer_Indent_Template_Mapping.Where(dd => dd.Indent_Name.ToLower().Trim() == misDetail.Indent_Template_Name.ToLower().Trim() && dd.Customer_Name.ToLower() == misDetail.Customer_Name.ToLower()).FirstOrDefault();
        //                        if (milkrun != null)
        //                        {
        //                            misDetail.Customer_Code = misDetail.Customer_Code;
        //                            misDetail.Customer_Name = milkrun.Customer_Name;
        //                            misDetail.Customer_Id = milkrun.Customer_Id;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Customer Not Mapped with Indent Template";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }


        //                    var LineHeader = DB.Customer_Indent.Where(dd => dd.Indent_Name.ToLower().Trim() == misDetail.Indent_Template_Name.ToLower().Trim() && dd.Is_Deleted != true).FirstOrDefault();
        //                    if (LineHeader != null)
        //                    {
        //                        misDetail.Indent_Template_Name = LineHeader.Indent_Name;
        //                        misDetail.Indent_Type = LineHeader.Indent_Type;
        //                        misDetail.SKU_Type = LineHeader.SKU_Type;
        //                        misDetail.Delivery_cycle = LineHeader.Delivery_cycle;
        //                        misDetail.Delivery_Type = LineHeader.Delivery_Type;
        //                        misDetail.Indent_Code = LineHeader.Indent_Code;
        //                        misDetail.Price_Template_Code = LineHeader.Price_Template_Code;
        //                        misDetail.SKU_Type_Id = LineHeader.SKU_Type_Id;
        //                        misDetail.Indent_ID = LineHeader.Indent_ID;
        //                        misDetail.Price_Template_ID = LineHeader.Price_Template_ID;
        //                        misDetail.User_Location_Code = ((LineHeader.DC_Code != null && LineHeader.DC_Code != "null") ? LineHeader.DC_Code : LineHeader.Location_Code);
        //                        misDetail.User_Location_Id = (LineHeader.DC_Id != null ? LineHeader.DC_Id : LineHeader.Location_Id);
        //                        misDetail.User_Location = ((LineHeader.DC_Code != null && LineHeader.DC_Code != "null") ? (DB.DC_Master.Where(dd => dd.DC_Code == LineHeader.DC_Code).Select(dd => dd.Dc_Name)).FirstOrDefault() : (DB.Location_Master.Where(dd => dd.Location_Code == LineHeader.Location_Code).Select(dd => dd.Location_Name)).FirstOrDefault());
        //                        misDetail.User_Type = (((LineHeader.DC_Code != null && LineHeader.DC_Code != "null")) ? "DC" : "LOCATION");
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.Message = "Line Item is not available in Customer Indent.Line Id:" + lineCount;
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }
        //                    var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == misDetail.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
        //                    misDetail.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

        //                    var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == misDetail.Grade.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Grade = gradeDetail.GradeType_Name;

        //                    var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == misDetail.UOM.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.UOM = uomDetail.Unit_Name;

        //                    var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == misDetail.Pack_Size.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Pack_Size = packsizedetail.Pack_Size_Value;

        //                    misDetail.Dispatch_Qty = misDetail.Dispatch_Qty;

        //                    stilist.Add(misDetail);

        //                    iscope.Complete();
        //                }

        //                lineCount += 1;
        //            }
        //        excelReader.Close();

        //        List<BulkCSIExcelFields> Intersect = new List<BulkCSIExcelFields>();
        //        List<BulkCSIExcelFields> Except = new List<BulkCSIExcelFields>();
        //        List<BulkCSIExcelFields> InSteadA = new List<BulkCSIExcelFields>();
        //        List<BulkCSIExcelFields> InSteadB = new List<BulkCSIExcelFields>();
        //        List<BulkCSIExcelFields> RawA = new List<BulkCSIExcelFields>();
        //        List<BulkCSIExcelFields> RawB = new List<BulkCSIExcelFields>();

        //        foreach (var t in stilist)
        //        {
        //            BulkCSIExcelFields ex = new BulkCSIExcelFields();
        //            // ex.Region = t.Region;
        //            ex.Dispatch_DC_Location_Code = t.Dispatch_DC_Location_Code;
        //            ex.Customer_Name = t.Customer_Name;
        //            ex.Customer_Code = t.Customer_Code;
        //            ex.Indent_Template_Name = t.Indent_Template_Name;
        //            ex.Price_Template_Name = t.Price_Template_Name;
        //            ex.SKU_SubType = t.SKU_SubType;
        //            ex.SKU_Name = t.SKU_Name;
        //            ex.Grade = t.Grade;
        //            ex.Pack_Type = t.Pack_Type;
        //            ex.Pack_Size = t.Pack_Size;
        //            ex.Pack_Weight_Type = t.Pack_Weight_Type;
        //            ex.Delievery_Date = t.Delievery_Date;
        //            ex.Dispatch_Qty = t.Dispatch_Qty;
        //            ex.UOM = t.UOM;
        //            ex.Indent_Type = t.Indent_Type;
        //            ex.SKU_Type = t.SKU_Type;
        //            ex.Pack_Weight_Type_Id = t.Pack_Weight_Type_Id;
        //            ex.SKU_Id = t.SKU_Id;
        //            ex.SKU_SubType_Id = t.SKU_SubType_Id;
        //            ex.Pack_Type_Id = t.Pack_Type_Id;
        //            ex.Indent_Type = t.Indent_Type;
        //            ex.SKU_Type = t.SKU_Type;
        //            ex.Delivery_cycle = t.Delivery_cycle;
        //            ex.Delivery_Type = t.Delivery_Type;
        //            ex.Indent_Code = t.Indent_Code;
        //            ex.Price_Template_Code = t.Price_Template_Code;
        //            ex.Customer_Id = t.Customer_Id;
        //            ex.SKU_Type_Id = t.SKU_Type_Id;
        //            ex.Indent_ID = t.Indent_ID;
        //            ex.Price_Template_ID = t.Price_Template_ID;
        //            ex.User_Location_Code = t.User_Location_Code;
        //            ex.User_Location_Id = t.User_Location_Id;
        //            ex.User_Location = t.User_Location;
        //            ex.User_Type = t.User_Type;
        //            RawB.Add(ex);
        //        }

        //        foreach (var y in RawB)
        //        {
        //            int mic =
        //                (from mm in DB.Customer_Indent
        //                 join nn in DB.Customer_Indent_Line_item on mm.Indent_Code equals nn.Indent_Code
        //                 where mm.Is_Deleted != true && mm.Indent_Name == y.Indent_Template_Name && mm.Price_Template_Name == y.Price_Template_Name && nn.SKU_SubType == y.SKU_SubType && nn.SKU_Name == y.SKU_Name && nn.Grade == y.Grade && nn.Pack_Type == y.Pack_Type && nn.Pack_Size == y.Pack_Size && nn.Pack_Weight_Type == y.Pack_Weight_Type && nn.UOM == y.UOM && mm.Indent_Type == y.Indent_Type && mm.SKU_Type == y.SKU_Type && nn.Pack_Weight_Type_Id == y.Pack_Weight_Type_Id && nn.SKU_Id == y.SKU_Id && nn.SKU_SubType_Id == y.SKU_SubType_Id && nn.Pack_Type_Id == y.Pack_Type_Id
        //                 select mm).Count();

        //            if (mic > 0)
        //                Intersect.Add(y);
        //            else
        //            {
        //                Except.Add(y);
        //            }
        //        }

        //        BulkCSIExcelFields Ex = new BulkCSIExcelFields();
        //        Ex.ValidDatas = Intersect.ToList();
        //        Ex.InvalidDatas = Except.ToList();
        //        Ex.Uploaded_Excel_Name = name;
        //        Ex.status = true;
        //        Ex.Message = "Success";

        //        result.Add(Ex);
        //    }
        //    catch (Exception e)
        //    {
        //        stilist = new List<BulkCSIExcelFields>();
        //        miDetail.status = false;
        //        miDetail.Message = e.Message.ToString();
        //        stilist.Add(miDetail);
        //        return stilist;
        //    }
        //    stilist[0].status = true;
        //    stilist[0].Message = "Success";

        //    return result;
        //}





        public bool SAUpdateById(SALUpdateEntity csiEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                foreach (SALIDs lst in csiEntity.SalIds)
                {
                    CSI_Line_item lineItem = _unitOfWork.SaleSubRepository.GetByID(lst.csi_LineId);

                    if (lineItem != null)
                    {
                        if (lst.statusflag == 0)
                        {
                            lineItem.Status = "Closed";
                            _unitOfWork.SaleSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 1)
                        {
                            lineItem.Status = "Partial";
                            _unitOfWork.SaleSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 2)
                        {
                            lineItem.Status = "Exceed";
                            _unitOfWork.SaleSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                    }
                }

                scope.Complete();
            }
            success = true;
            using (var fscope = new TransactionScope())
            {
                success = false;

                SaleIndentEntity list = (from x in DB.Customer_Sale_Indent
                                         where x.CSI_Number == csiEntity.id
                                         select new SaleIndentEntity
                                            {
                                                CSI_id = x.CSI_id,
                                                CSI_Number = x.CSI_Number,
                                                Status = x.Status,
                                                LineItems = (from a in DB.CSI_Line_item
                                                             where a.CSI_Number == csiEntity.id
                                                             select new CSI_LineItems_Entity
                                                             {
                                                                 CSI_Line_Id = a.CSI_Line_Id,
                                                                 CSI_Number = a.CSI_Number,
                                                                 Status = a.Status
                                                             }).ToList(),
                                            }).FirstOrDefault();

                if (list != null)
                {
                    string state = "Closed";
                    string csiNum = "";

                    foreach (CSI_LineItems_Entity csi in list.LineItems)
                    {
                        csiNum = csi.CSI_Number;

                        if (csi.Status == "Open" || csi.Status == "Partial")
                        {
                            state = "Open";
                        }
                    }

                    if (csiNum != "" && csiNum != null)
                    {
                        if (state == "Open")
                        {
                            Customer_Sale_Indent cs = DB.Customer_Sale_Indent.Where(x => x.CSI_Number == csiNum).FirstOrDefault();

                            if (cs != null)
                            {
                                cs.Status = "Open";
                                DB.Entry(cs).State = EntityState.Modified;
                                DB.SaveChanges();
                            }
                        }
                        else if (state == "Closed")
                        {
                            Customer_Sale_Indent cs = DB.Customer_Sale_Indent.Where(x => x.CSI_Number == csiNum).FirstOrDefault();
                            if (cs != null)
                            {
                                cs.Status = "Closed";
                                DB.Entry(cs).State = EntityState.Modified;
                                DB.SaveChanges();
                            }
                        }
                    }

                }
                fscope.Complete();
                success = true;
            }

            return success;
        }


        //public List<BulkCSIExcelFields> ExcelImportFromMI(fileImportMI fileDetail)
        //{
        //    List<BulkCSIExcelFields> stilist = new List<BulkCSIExcelFields>();
        //    List<BulkCSIExcelFields> milist = new List<BulkCSIExcelFields>();
        //    //List<BulkCSIExcelFields> miilist = new List<BulkCSIExcelFields>();
        //    List<BulkCSIExcelFields> result = new List<BulkCSIExcelFields>();
        //    BulkCSIExcelFields miDetail = new BulkCSIExcelFields();

        //    string Profilepicname = "MI_" + Guid.NewGuid().ToString();
        //    string sPath = "";
        //    string vPath = "";
        //    string name = "";
        //    string dPath = "~/Areas/MI";
        //    string dirCreatePath = "";
        //    try
        //    {
        //        string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);
        //
        //        string ext = ".xlsx";
        //        dirCreatePath = RootPath;

        //        if (!Directory.Exists(dirCreatePath))
        //        {
        //            Directory.CreateDirectory(RootPath);
        //        }
        //        sPath = RootPath;
        //        name = Profilepicname + ext;
        //        vPath = sPath + "\\" + name;

        //        if (File.Exists(vPath))
        //        {
        //            File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
        //        }
        //        else
        //        {
        //            File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
        //        }

        //        FileStream stream = File.Open(vPath, FileMode.Open, FileAccess.Read);
        //        stream.Position = 0;
        //        IExcelDataReader excelReader = null;

        //        if (ext == ".xls")
        //        {
        //            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        //        }
        //        else if (ext == ".xlsx")
        //        {
        //            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //        }

        //        excelReader.IsFirstRowAsColumnNames = true;
        //        DataSet result1 = excelReader.AsDataSet();

        //        int columnCount = result1.Tables[0].Columns.Count;

        //        if (columnCount > 8)
        //        {
        //            miDetail.status = false;
        //            miDetail.Message = "Extra Column's Present in Given Excel";
        //            stilist.Add(miDetail);
        //            return stilist;
        //        }

        //        //--------------------validating Excel Columns
        //        int lineCount = 1;

        //        if (result1 != null)
        //            foreach (DataRow d in result1.Tables[0].Rows)
        //            {
        //                Material_Master ci = new Material_Master();
        //                //object[] A = { };

        //                ci.DC_Code = fileDetail.LocationCode;
        //                ci.DC_Code = fileDetail.DCCode;
        //                ci.Region = fileDetail.Region;
        //                ci.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
        //                ci.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
        //                ci.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
        //                ci.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
        //                ci.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
        //                ci.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
        //                ci.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
        //                // ci.SP = d["SP"] != null && d["SP"].ToString() != "" ? double.Parse(d["SP"].ToString()) : 0;
        //                //
        //                using (var iscope = new TransactionScope())
        //                {
        //                    var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == ci.SKU_SubType.ToLower().Trim()).FirstOrDefault();
        //                    if (subtypedetail != null)
        //                    {
        //                        ci.SKU_SubType = subtypedetail.SKU_SubType_Name;
        //                        ci.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;
        //                    }

        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "SKU_SubType_Name";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var skuDetail = DB.Material_Master.Where(dd => dd.SKU_Type == fileDetail.SKUType && (dd.Location_Code == fileDetail.LocationCode || dd.DC_Code == fileDetail.DCCode) && dd.SKU_Name.ToLower().Trim() == ci.SKU_Name.ToLower().Trim() && dd.Pack_Weight_Type.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim() && dd.Pack_Type.ToLower().Trim() == ci.Pack_Type.ToLower().Trim() && dd.Pack_Size.ToLower().Trim() == ci.Pack_Size.ToLower().Trim() && dd.Grade.ToLower().Trim() == ci.Grade.ToLower().Trim() && dd.UOM.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
        //                    if (skuDetail != null)
        //                    {
        //                        ci.SKU_Name = skuDetail.SKU_Name;
        //                        ci.SKU_Id = skuDetail.SKU_Id;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.Message = "Line Item is not available in Material Master.Line Id:" + lineCount;
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ci.UOM.ToLower().Trim()).FirstOrDefault();
        //                    if (uomDetail != null)
        //                    {
        //                        ci.UOM = uomDetail.Unit_Name;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "UOM";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ci.Pack_Type.ToLower().Trim()).FirstOrDefault();
        //                    if (packtypedetail != null)
        //                    {
        //                        ci.Pack_Type = packtypedetail.Pack_Type_Name;
        //                        ci.Pack_Type_Id = packtypedetail.Pack_Type_Id;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Pack_Type_Name";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ci.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
        //                    if (packweightType != null)
        //                    {
        //                        ci.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
        //                        ci.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Pack_Weight_Type";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ci.Grade.ToLower().Trim()).FirstOrDefault();
        //                    if (gradeDetail != null)
        //                    {
        //                        ci.Grade = gradeDetail.GradeType_Name;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Grade";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ci.Pack_Size.ToLower().Trim()).FirstOrDefault();
        //                    if (packsizedetail != null)
        //                    {
        //                        ci.Pack_Size = packsizedetail.Pack_Size_Value;
        //                    }
        //                    else
        //                    {
        //                        miDetail.status = false;
        //                        miDetail.lineNumber = lineCount;
        //                        miDetail.Message = "Error";
        //                        miDetail.errorItem = "Pack_Size_Value";
        //                        stilist.Add(miDetail);
        //                        return stilist;
        //                    }

        //                    iscope.Complete();
        //                }

        //                lineCount += 1;
        //            }

        //        if (result1 != null)

        //            foreach (DataRow d in result1.Tables[0].Rows)
        //            {
        //                BulkCSIExcelFields misDetail = new BulkCSIExcelFields();
        //                misDetail.SKU_Name = d["SKU_Name"] != null && d["SKU_Name"].ToString() != "" ? d["SKU_Name"].ToString() : "";
        //                misDetail.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
        //                misDetail.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
        //                misDetail.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
        //                misDetail.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
        //                misDetail.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
        //                misDetail.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
        //                misDetail.SP = d["SP"] != null && d["SP"].ToString() != "" ? double.Parse(d["SP"].ToString()) : 0;

        //                using (var iscope = new TransactionScope())
        //                {

        //                    var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == misDetail.SKU_Name.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.SKU_Name = skuDetail.SKU_Name;
        //                    misDetail.SKU_ID = skuDetail.SKU_Id;

        //                    var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == misDetail.SKU_SubType.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.SKU_SubType = subtypedetail.SKU_SubType_Name;
        //                    misDetail.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

        //                    var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == misDetail.Pack_Type.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Pack_Type = packtypedetail.Pack_Type_Name;
        //                    misDetail.Pack_Type_Id = packtypedetail.Pack_Type_Id;


        //                    var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == misDetail.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
        //                    misDetail.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

        //                    var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == misDetail.Grade.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Grade = gradeDetail.GradeType_Name;

        //                    var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == misDetail.UOM.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.UOM = uomDetail.Unit_Name;

        //                    var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == misDetail.Pack_Size.ToLower().Trim()).FirstOrDefault();
        //                    misDetail.Pack_Size = packsizedetail.Pack_Size_Value;

        //                    misDetail.SP = misDetail.SP;

        //                    stilist.Add(misDetail);

        //                    iscope.Complete();
        //                }

        //                lineCount += 1;

        //            }

        //        excelReader.Close();

        //        var t = stilist.ToList();
        //        var materialRaw = (from mm in DB.Material_Master
        //                           where mm.SKU_Type == fileDetail.SKUType
        //                           select mm).ToList();
        //        if (fileDetail.DCCode != "NULL")
        //        {
        //            materialRaw = materialRaw.Where(d => d.DC_Code == fileDetail.DCCode).ToList();
        //        }
        //        if (fileDetail.LocationCode != "NULL")
        //        {
        //            materialRaw = materialRaw.Where(d => d.Location_Code == fileDetail.LocationCode).ToList();
        //        }

        //        foreach (var y in t)
        //        {
        //            int mic = materialRaw.Where(d => d.SKU_Id == y.SKU_ID && d.SKU_Name == y.SKU_Name && d.Pack_Weight_Type_Id == y.Pack_Weight_Type_Id && d.Pack_Weight_Type == y.Pack_Weight_Type && d.Pack_Type == y.Pack_Type && d.Pack_Size == y.Pack_Size && d.Grade == y.Grade && d.UOM == y.UOM).ToList().Count();
        //            if (mic > 0)
        //                result.Add(y);
        //            else
        //            {
        //                result.Add(y);
        //                return result;
        //            }


        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        stilist = new List<BulkCSIExcelFields>();
        //        miDetail.status = false;
        //        miDetail.Message = e.Message.ToString();
        //        stilist.Add(miDetail);
        //        return stilist;
        //    }
        //    stilist[0].status = true;
        //    stilist[0].Message = "Success";

        //    return result;
        //}
        //----------------------------------------CREATESALEINDENT----------------------
        public bool BulkCSI(BulkCSIModel saleEntity)
        {

            int multiCSITrackingId;
            var lis = saleEntity.BulkCSI.ToList();

            var groupedCustomerList = lis.GroupBy(u => new { u.User_Location_Id, u.User_Location_Code, u.User_Location, u.User_Type, u.Delivery_cycle, u.Delivery_Type, u.Customer_Id, u.Indent_Code, u.Indent_ID, u.Dispatch_DC_Location_Code, u.Customer_Name, u.Customer_Code, u.Indent_Template_Name, u.Price_Template_ID, u.Price_Template_Code, u.Price_Template_Name, u.SKU_Type_Id, u.SKU_Type, u.Indent_Type, u.Delievery_Date })
                                                  .Select(grp => new
                                                  {
                                                      GroupID = grp.Key,
                                                      CustomerList = grp.ToList()
                                                  })
                                                  .ToList();

            List<SaleIndentEntity> lst = new List<SaleIndentEntity>();

            foreach (var y in groupedCustomerList)
            {


                //public Nullable<int> User_Location_Id { get; set; }
                //public string User_Location_Code { get; set; }
                //public string User_Location { get; set; }
                //public string User_Type { get; set; }


                SaleIndentEntity uu = new SaleIndentEntity();
                uu.Customer_Id = y.GroupID.Customer_Id;
                uu.Delivery_cycle = y.GroupID.Delivery_cycle;
                uu.Delievery_Type = y.GroupID.Delivery_Type;
                uu.CSI_type = y.GroupID.Indent_Type;
                uu.SKU_Type_Id = y.GroupID.SKU_Type_Id;
                uu.Rate_Template_Code = y.GroupID.Price_Template_Code;
                uu.Rate_Template_Id = y.GroupID.Price_Template_ID;
                uu.Indent_Id = y.GroupID.Indent_ID;
                uu.Indent_Code = y.GroupID.Indent_Code;
                uu.Delivery_Date = Convert.ToDateTime(y.GroupID.Delievery_Date);
                //uu.CSI_Raised_date = DateTime.Now;
                //uu.CSI_Approved_Flag = null;
                //uu.is_Deleted = false;
                //uu.Status = "Open";
                //uu.is_Syunc = false;
                uu.CreateBy = saleEntity.CreateBy;
                //  uu.CSI_Create_by = saleEntity.CreateBy;
                uu.CSI_raised_by = saleEntity.CreateBy;
                uu.Customer_Code = y.GroupID.Customer_Code;
                uu.SKU_Type = y.GroupID.SKU_Type;
                uu.DC_Code = y.GroupID.Dispatch_DC_Location_Code;
                uu.Rate_Template_Name = y.GroupID.Price_Template_Name;
                uu.Customer_Name = y.GroupID.Customer_Name;
                uu.Indent_Name = y.GroupID.Indent_Template_Name;
                uu.BulkLineItems = y.CustomerList.ToList();
                uu.Rate_Template_Valitity_upto = (DB.Rate_Template.Where(o => o.Template_Code == y.GroupID.Price_Template_Code).Select(o => o.Valitity_upto)).FirstOrDefault();
                uu.User_Location_Code = y.GroupID.User_Location_Code;
                uu.User_Location_Id = y.GroupID.User_Location_Id;
                uu.User_Location = y.GroupID.User_Location;
                uu.User_Type = y.GroupID.User_Type;
                //    uu.CreatedDate = DateTime.Now;
                lst.Add(uu);
            }
            int CSIcount = lst.Count();
            //
            ////var saleIndent = new MultipleCSItrackingEntity
            ////{

            ////    Excel_Name = BCSI.CSI_Create_by,
            ////    CI_Temp_Id = BCSI.CSI_type,
            ////    CI_Temp_Code =
            ////    CreatedDate = DateTime.Now,
            ////    CreateBy = saleEntity.CreateBy
            ////};

            ////_unitOfWork.SaleRepository.Insert(saleIndent);
            ////_unitOfWork.Save();

            ////int? CSIId = saleIndent.CSI_id;
            //

            var mtracking = new Multiple_CSI_tracking
            {
                No_of_CSI = CSIcount,
                Uploaded_Excel_Display_Name = saleEntity.Uploaded_Excel_Display_Name,
                Uploaded_Excel_Name = saleEntity.Uploaded_Excel_Name,
                CreatedDate = DateTime.Now,
                CreateBy = saleEntity.CreateBy
            };

            _unitOfWork.MultipleTrackingCSIRepository.Insert(mtracking);
            _unitOfWork.Save();
            multiCSITrackingId = mtracking.Multiple_CSI_tracking_id;

            foreach (var BCSI in lst)
            {
                string csiNumber, CSI_prefix;
                int? incNumber;

                using (var iscope = new TransactionScope())
                {
                    ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                    CSI_prefix = rm.GetString("CSIT");
                    CSI_NUM_Generation autoIncNumber = GetAutoIncrement();
                    incNumber = autoIncNumber.CSI_Last_Number;
                    int? incrementedValue = incNumber + 1;
                    var CSIincrement = DB.CSI_NUM_Generation.Where(x => x.CSI_Num_Gen_Id == 1).FirstOrDefault();
                    CSIincrement.CSI_Last_Number = incrementedValue;
                    _unitOfWork.CSI_NUMRepository.Update(CSIincrement);
                    _unitOfWork.Save();
                    csiNumber = CSI_prefix + "/" + String.Format("{0:00000}", incNumber);
                    iscope.Complete();
                }

                using (var scope = new TransactionScope())
                {

                    var saleIndent = new Customer_Sale_Indent
                    {
                        CSI_Number = csiNumber,
                        DC_Code = BCSI.DC_Code,
                        CSI_Raised_date = DateTime.UtcNow,
                        CSI_Timestamp = BCSI.CSI_Timestamp,
                        Customer_Id = BCSI.Customer_Id,
                        Customer_Code = BCSI.Customer_Code,
                        Customer_Name = BCSI.Customer_Name,
                        Delivery_Location_ID = BCSI.Delivery_Location_ID,
                        Delivery_Location_Code = BCSI.Delivery_Location_Code,
                        Delievery_Type = BCSI.Delievery_Type,
                        Delivery_cycle = BCSI.Delivery_cycle,
                        Delivery_Expected_Date = BCSI.Delivery_Expected_Date,
                        Delivery_Date = BCSI.Delivery_Date,
                        SKU_Type_Id = BCSI.SKU_Type_Id,
                        SKU_Type = BCSI.SKU_Type,
                        CSI_raised_by = BCSI.CSI_raised_by,
                        Indent_Id = BCSI.Indent_Id,

                        Multiple_CSI_tracking_id_FK = multiCSITrackingId,
                        Indent_Code = BCSI.Indent_Code,
                        Indent_Name = BCSI.Indent_Name,
                        Rate_Template_Id = BCSI.Rate_Template_Id,
                        Rate_Template_Code = BCSI.Rate_Template_Code,
                        Rate_Template_Name = BCSI.Rate_Template_Name,
                        Rate_Template_Valitity_upto = BCSI.Rate_Template_Valitity_upto,
                        User_Location_Id = BCSI.User_Location_Id,
                        User_Location_Code = BCSI.User_Location_Code,
                        User_Location = BCSI.User_Location,
                        User_Type = BCSI.User_Type,
                        CSI_Approved_Flag = null,
                        is_Deleted = false,
                        Status = "Open",
                        is_Syunc = false,
                        CSI_Create_by = BCSI.CSI_Create_by,
                        CSI_type = BCSI.CSI_type,
                        CreatedDate = DateTime.Now,
                        CreateBy = saleEntity.CreateBy
                    };

                    _unitOfWork.SaleRepository.Insert(saleIndent);
                    _unitOfWork.Save();

                    int? CSIId = saleIndent.CSI_id;

                    var model = new CSI_Line_item();
                    foreach (var sSub in BCSI.BulkLineItems)
                    {
                        model.CSI_id = CSIId;
                        model.CSI_Number = csiNumber;
                        model.SKU_ID = sSub.SKU_Id;
                        //  model.SKU_Code = sSub.SKU_Code;
                        model.SKU_Name = sSub.SKU_Name;
                        model.HSN_Code = sSub.HSN_Code;
                        model.Total_GST = sSub.Total_GST;
                        model.CGST = sSub.CGST;
                        model.SGST = sSub.SGST;
                        model.Pack_Size = sSub.Pack_Size;
                        model.Pack_Type_Id = sSub.Pack_Type_Id;
                        model.Pack_Type = sSub.Pack_Type;
                        model.Pack_Weight_Type_Id = sSub.Pack_Weight_Type_Id;
                        model.Pack_Weight_Type = sSub.Pack_Weight_Type;
                        model.SKU_SubType_Id = sSub.SKU_SubType_Id;
                        model.SKU_SubType = sSub.SKU_SubType;
                        model.UOM = sSub.UOM;
                        model.price = (from x in DB.Rate_Template
                                       join y in DB.Rate_Template_Line_item on x.Template_Code equals y.Rate_Template_Code
                                       where x.Template_Code == BCSI.Rate_Template_Code && x.Is_Deleted == false && y.SKU_Id == sSub.SKU_Id && y.SKU_SubType_Id == sSub.SKU_SubType_Id && y.Pack_Type_Id == sSub.Pack_Type_Id && y.Pack_Size == sSub.Pack_Size && y.Grade == sSub.Grade
                                       select y.Selling_price).FirstOrDefault();
                        model.Grade = sSub.Grade;
                        model.Indent_Qty = Convert.ToDouble(sSub.Dispatch_Qty);
                        // model.Remark = sSub.Remark;
                        model.Status = "Open";
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = saleEntity.CreateBy;

                        _unitOfWork.SaleSubRepository.Insert(model);
                        _unitOfWork.Save();

                    }
                    scope.Complete();
                }

            }

            return true;
        }
        public CSI_NUM_Generation GetAutoIncrement(string locationId)
        {
            var autoinc = DB.CSI_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new CSI_NUM_Generation
            {
                CSI_Num_Gen_Id = autoinc.CSI_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                CSI_Last_Number = autoinc.CSI_Last_Number
            };
            return model;
        }
        public string CreateSaleIndent(SaleIndentEntity saleEntity)
        {

            string csiNumber, CSI_prefix,FY;
            string locationId, locationID;
            int incNumber;
            int incrementedValue;

            using (var iscope = new TransactionScope())
            {
                locationID = saleEntity.DC_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                CSI_prefix = rm.GetString("CSIT");
                CSI_NUM_Generation autoIncNumber = GetAutoIncrement(locationID);
                locationId = autoIncNumber.DC_Code;
                FY = autoIncNumber.Financial_Year;
                incNumber = autoIncNumber.CSI_Last_Number.Value;
                incrementedValue = incNumber + 1;
                var dispatchincrement = DB.CSI_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                dispatchincrement.CSI_Last_Number = incrementedValue;
                _unitOfWork.CSI_NUMRepository.Update(dispatchincrement);
                _unitOfWork.Save();
                csiNumber = CSI_prefix + "/" + locationId + "/" + FY + "/" + String.Format("{0:00000}", incNumber);                
                iscope.Complete();
            }
           
            using (var scope = new TransactionScope())
            {
                var CLT = (from ord in DB.Sales_Route_Mapping
                           where ord.Sales_Person_Name.ToLower() == saleEntity.CreateBy.ToLower()
                           select ord).FirstOrDefault();
                var saleIndent = new Customer_Sale_Indent
                {
                    CSI_Number = csiNumber,
                    DC_Code = saleEntity.DC_Code,
                    CSI_Raised_date = saleEntity.CSI_Raised_date,
                    CSI_Timestamp = saleEntity.CSI_Timestamp,
                    Customer_Id = saleEntity.Customer_Id,
                    Customer_Code = saleEntity.Customer_Code,
                    Customer_Name = saleEntity.Customer_Name,
                    Delivery_Location_ID = saleEntity.Delivery_Location_ID,
                    Delivery_Location_Code = saleEntity.Delivery_Location_Code,
                    Delievery_Type = saleEntity.Delievery_Type,
                    Delivery_cycle = saleEntity.Delivery_cycle,
                    Delivery_Expected_Date = saleEntity.Delivery_Expected_Date,
                    Delivery_Date = saleEntity.Delivery_Date,
                    SKU_Type_Id = saleEntity.SKU_Type_Id,
                    SKU_Type = saleEntity.SKU_Type,
                    CSI_From = "WEB",
                    CSI_raised_by = saleEntity.CSI_raised_by,
                    Indent_Id = saleEntity.Indent_Id,
                    Indent_Code = saleEntity.Indent_Code,
                    Indent_Name = saleEntity.Indent_Name,
                    Rate_Template_Id = saleEntity.Rate_Template_Id,
                    Rate_Template_Code = saleEntity.Rate_Template_Code,
                    Rate_Template_Name = saleEntity.Rate_Template_Name,
                    Rate_Template_Valitity_upto = saleEntity.Rate_Template_Valitity_upto,
                    User_Location_Id = saleEntity.User_Location_Id,
                    User_Location_Code = saleEntity.User_Location_Code,
                    User_Location = saleEntity.User_Location,
                    User_Type = saleEntity.User_Type,
                    CSI_Approved_Flag = null,
                    is_Deleted = false,
                    Status = "Open",
                    is_Syunc = false,
                    CSI_Create_by = saleEntity.CSI_Create_by,
                    CSI_type = saleEntity.CSI_type,
                    CreatedDate = DateTime.Now,
                    CreateBy = saleEntity.CreateBy,
                    Expected_Delivering_Sales_Person_Id = CLT.Sales_Person_Id,
                    Expected_Delivering_Sales_Person_Name = CLT.Sales_Person_Name,
                    Expected_Route_Id = CLT.Sales_Route_Mapping_Id,
                    Expected_Route_Alias_Name = CLT.Route_Alias_Name,
                    Expected_Route_Code = CLT.Route_Code
                };

                _unitOfWork.SaleRepository.Insert(saleIndent);
                _unitOfWork.Save();

                int? CSIId = saleIndent.CSI_id;

                var model = new CSI_Line_item();
                foreach (var sSub in saleEntity.LineItems)
                {
                    model.CSI_id = CSIId;
                    model.CSI_Number = csiNumber;
                    model.SKU_ID = sSub.SKU_ID.Value;
                    model.SKU_Code = sSub.SKU_Code;
                    model.SKU_Name = sSub.SKU_Name;
                    model.Pack_Size = sSub.Pack_Size;
                    model.Pack_Type_Id = sSub.Pack_Type_Id;
                    model.Pack_Type = sSub.Pack_Type;
                    model.Pack_Weight_Type_Id = sSub.Pack_Weight_Type_Id;
                    model.Pack_Weight_Type = sSub.Pack_Weight_Type;
                    model.SKU_SubType_Id = sSub.SKU_SubType_Id;
                    model.SKU_SubType = sSub.SKU_SubType;
                    model.UOM = sSub.UOM;
                    model.HSN_Code = sSub.HSN_Code;
                    model.Total_GST = sSub.Total_GST;
                    model.CGST = sSub.CGST;
                    model.SGST = sSub.SGST;
                    model.price = sSub.price;
                    model.Grade = sSub.Grade;
                    model.Indent_Qty = sSub.Indent_Qty;
                    model.Remark = sSub.Remark;
                    model.Status = "Open";
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = sSub.CreatedBy;

                    _unitOfWork.SaleSubRepository.Insert(model);
                    _unitOfWork.Save();

                }
                scope.Complete();
                return saleIndent.CSI_Number;
            }
        }
     
        public List<SaleIndentEntity> SearchSA(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url)
        {
            List<SaleIndentEntity> Result = new List<SaleIndentEntity>();
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
            var qu = (from a in DB.Customer_Sale_Indent
                      where a.is_Deleted == false && a.CSI_Approved_Flag == true && (a.User_Location_Code == ULocation || a.DC_Code == ULocation)
                      select new SaleIndentEntity
                      {
                        CSI_id = a.CSI_id,
                        CSI_Number = a.CSI_Number,
                        DC_Code = a.DC_Code,
                        CSI_Raised_date = a.CSI_Raised_date,
                        CSI_Timestamp = a.CSI_Timestamp,
                        Customer_Id = a.Customer_Id,
                        Customer_Code = a.Customer_Code,
                        Customer_Name = a.Customer_Name,
                        Delivery_Location_ID = a.Delivery_Location_ID,
                        Delivery_Location_Code = a.Delivery_Location_Code,
                        Delievery_Type = a.Delievery_Type,
                        SKU_Type = a.SKU_Type,
                        SKU_Type_Id = a.SKU_Type_Id,
                        Delivery_cycle = a.Delivery_cycle,
                        Delivery_Expected_Date = a.Delivery_Expected_Date,
                        Delivery_Date = a.Delivery_Date,
                        CSI_raised_by = a.CSI_raised_by,
                        CSI_Approved_Flag = a.CSI_Approved_Flag,
                        CSI_Approved_by = a.CSI_Approved_by,
                        CSI_Approved_date = a.CSI_Approved_date,
                        Indent_Id = a.Indent_Id,
                        Indent_Name = a.Indent_Name,
                        User_Location = a.User_Location,
                        User_Location_Code = a.User_Location_Code,
                        User_Location_Id = a.User_Location_Id,
                        Rate_Template_Id = a.Rate_Template_Id,
                        Rate_Template_Name = a.Rate_Template_Name,
                        Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,
                        Status = a.Status,
                        Reason = a.Reason,
                        Expected_Route_Id = a.Expected_Route_Id,
                        Expected_Route_Alias_Name = a.Expected_Route_Alias_Name,
                        Expected_Route_Code = a.Expected_Route_Code,
                        Expected_Delivering_Sales_Person_Id = a.Expected_Delivering_Sales_Person_Id,
                        Expected_Delivering_Sales_Person_Name = a.Expected_Delivering_Sales_Person_Name,
                        is_Create = iCrt,
                        is_Delete = isDel,
                        is_Edit = isEdt,
                        is_Approval = isApp,
                        is_View = isViw,
                        is_Deleted = a.is_Deleted,
                        Counting = (from p in DB.CSI_Line_item
                                    where p.CSI_id == a.CSI_id
                                    select new
                                    {
                                        CSI_id = p.CSI_id
                                    }).Count(),
                        LineItems = (from s in DB.CSI_Line_item
                                     where s.CSI_id == a.CSI_id
                                     orderby s.SKU_Name
                                     select new CSI_LineItems_Entity
                                     {
                                         CSI_Line_Id = s.CSI_Line_Id,
                                         CSI_id = s.CSI_id,
                                         CSI_Number = s.CSI_Number,
                                         SKU_ID = s.SKU_ID,
                                         SKU_Code = s.SKU_Code,
                                         SKU_Name = s.SKU_Name,
                                         Expected_Route_Id = a.Expected_Route_Id,
                                         Expected_Route_Alias_Name = a.Expected_Route_Alias_Name,
                                         Expected_Route_Code = a.Expected_Route_Code,
                                         Expected_Delivering_Sales_Person_Id = a.Expected_Delivering_Sales_Person_Id,
                                         Expected_Delivering_Sales_Person_Name = a.Expected_Delivering_Sales_Person_Name,
                                         Pack_Size = s.Pack_Size,
                                         Grade = s.Grade,
                                         HSN_Code = s.HSN_Code,
                                         CGST = s.CGST,
                                         SGST = s.SGST,
                                         Total_GST = s.Total_GST,
                                         Pack_Type_Id = s.Pack_Type_Id,
                                         Pack_Type = s.Pack_Type,
                                         Pack_Weight_Type_Id = s.Pack_Weight_Type_Id,
                                         Pack_Weight_Type = s.Pack_Weight_Type,
                                         SKU_SubType_Id = s.SKU_SubType_Id,
                                         SKU_SubType = s.SKU_SubType,
                                         UOM = s.UOM,
                                         price = s.price,
                                         Indent_Qty = s.Indent_Qty,
                                         Remark = s.Remark,
                                         Status = a.Status,
                                         DC_Code = a.DC_Code,
                                         CSI_Raised_date = a.CSI_Raised_date,
                                         CSI_Timestamp = a.CSI_Timestamp,
                                         Customer_Name = a.Customer_Name,
                                         Delivery_Location_Code = a.Delivery_Location_Code,
                                         Delivery_cycle = a.Delivery_cycle,
                                         Delivery_Expected_Date = a.Delivery_Expected_Date,
                                         Delievery_Type = a.Delievery_Type,
                                         Delivery_Date = a.Delivery_Date,
                                         CSI_raised_by = a.CSI_raised_by,
                                         SKU_Type = a.SKU_Type,
                                         CSI_Approved_by = a.CSI_Approved_by,
                                         CSI_Approved_date = a.CSI_Approved_date,
                                         CSI_type = a.CSI_type,
                                         Indent_Name = a.Indent_Name,
                                         Rate_Template_Name = a.Rate_Template_Name,
                                         User_Location = a.User_Location,
                                         Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,

                                     }).ToList()
                    });

            if (startDate.Value != null && endDate.Value != null)
            {
                qu = qu.Where(a => a.CSI_Raised_date.Value >= startDate.Value && a.CSI_Raised_date.Value <= endDate.Value);
            }
            if (status != "null")
            {
                qu = qu.Where(a => a.Status == status);
            }
            Result = qu.ToList();
            //
            
            return Result;
        }
        public List<SaleIndentEntity> SearchConsolidatedCSI(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url)
        {
            List<SaleIndentEntity> Result = new List<SaleIndentEntity>();
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
            var qu = (from a in DB.Customer_Sale_Indent
                      where a.is_Deleted == false && a.CSI_Approved_Flag == true && (a.DC_Code == ULocation)
                      select new SaleIndentEntity
                      {
                          CSI_id = a.CSI_id,
                          CSI_Number = a.CSI_Number,
                          DC_Code = a.DC_Code,
                          CSI_Raised_date = a.CSI_Raised_date,
                          CSI_Timestamp = a.CSI_Timestamp,
                          Customer_Id = a.Customer_Id,
                          Customer_Code = a.Customer_Code,
                          Customer_Name = a.Customer_Name,
                          Delivery_Location_ID = a.Delivery_Location_ID,
                          Delivery_Location_Code = a.Delivery_Location_Code,
                          Delievery_Type = a.Delievery_Type,
                          SKU_Type = a.SKU_Type,
                          SKU_Type_Id = a.SKU_Type_Id,
                          Delivery_cycle = a.Delivery_cycle,
                          Delivery_Expected_Date = a.Delivery_Expected_Date,
                          Delivery_Date = a.Delivery_Date,
                          CSI_raised_by = a.CSI_raised_by,
                          CSI_Approved_Flag = a.CSI_Approved_Flag,
                          CSI_Approved_by = a.CSI_Approved_by,
                          CSI_Approved_date = a.CSI_Approved_date,
                          Indent_Id = a.Indent_Id,
                          CSI_type = a.CSI_type,
                          Indent_Name = a.Indent_Name,
                          User_Location = a.User_Location,
                          User_Location_Code = a.User_Location_Code,
                          User_Location_Id = a.User_Location_Id,
                          Rate_Template_Id = a.Rate_Template_Id,
                          Rate_Template_Name = a.Rate_Template_Name,
                          Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,
                          Expected_Route_Id = a.Expected_Route_Id,
                          Expected_Route_Alias_Name = a.Expected_Route_Alias_Name,
                          Expected_Route_Code = a.Expected_Route_Code,
                          Expected_Delivering_Sales_Person_Id = a.Expected_Delivering_Sales_Person_Id,
                          Expected_Delivering_Sales_Person_Name = a.Expected_Delivering_Sales_Person_Name,
                          Status = a.Status,
                          Reason = a.Reason,
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          is_Deleted = a.is_Deleted,
                          Counting = (from p in DB.CSI_Line_item
                                      where p.CSI_id == a.CSI_id
                                      select new
                                      {
                                          CSI_id = p.CSI_id
                                      }).Count(),
                          LineItems = (from s in DB.CSI_Line_item
                                       where s.CSI_id == a.CSI_id
                                       orderby s.SKU_Name
                                       select new CSI_LineItems_Entity
                                       {
                                           CSI_Line_Id = s.CSI_Line_Id,
                                           CSI_id = s.CSI_id,
                                           CSI_Number = s.CSI_Number,
                                           SKU_ID = s.SKU_ID,
                                           SKU_Code = s.SKU_Code,
                                           SKU_Name = s.SKU_Name,
                                           Pack_Size = s.Pack_Size,
                                           Grade = s.Grade,
                                           HSN_Code = s.HSN_Code,
                                           CGST = s.CGST,
                                           SGST = s.SGST,
                                           Total_GST = s.Total_GST,
                                           Pack_Type_Id = s.Pack_Type_Id,
                                           Pack_Type = s.Pack_Type,
                                           Pack_Weight_Type_Id = s.Pack_Weight_Type_Id,
                                           Pack_Weight_Type = s.Pack_Weight_Type,
                                           SKU_SubType_Id = s.SKU_SubType_Id,
                                           SKU_SubType = s.SKU_SubType,
                                           UOM = s.UOM,
                                           price = s.price,
                                           Indent_Qty = s.Indent_Qty,
                                           Remark = s.Remark,
                                           Status = a.Status,
                                           Expected_Route_Id = a.Expected_Route_Id,
                                           Expected_Route_Alias_Name = a.Expected_Route_Alias_Name,
                                           Expected_Route_Code = a.Expected_Route_Code,
                                           Expected_Delivering_Sales_Person_Id = a.Expected_Delivering_Sales_Person_Id,
                                           Expected_Delivering_Sales_Person_Name = a.Expected_Delivering_Sales_Person_Name,
                                           DC_Code = a.DC_Code,
                                           CSI_Raised_date = a.CSI_Raised_date,
                                           CSI_Timestamp = a.CSI_Timestamp,
                                           Customer_Name = a.Customer_Name,
                                           Delivery_Location_Code = a.Delivery_Location_Code,
                                           Delivery_cycle = a.Delivery_cycle,
                                           Delivery_Expected_Date = a.Delivery_Expected_Date,
                                           Delievery_Type = a.Delievery_Type,
                                           Delivery_Date = a.Delivery_Date,
                                           CSI_raised_by = a.CSI_raised_by,
                                           SKU_Type = a.SKU_Type,
                                           CSI_Approved_by = a.CSI_Approved_by,
                                           CSI_Approved_date = a.CSI_Approved_date,
                                           CSI_type = a.CSI_type,
                                           Indent_Name = a.Indent_Name,
                                           Rate_Template_Name = a.Rate_Template_Name,
                                           User_Location = a.User_Location,
                                           Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,

                                       }).ToList()
                      });

            if (startDate.Value != null && endDate.Value != null)
            {
                qu = qu.Where(a => a.CSI_Raised_date.Value >= startDate.Value && a.CSI_Raised_date.Value <= endDate.Value);
            }

            if (status != "null")
            {
                qu = qu.Where(a => a.Status == status);
            }

            Result = qu.ToList();
         
            return Result;
        }

        //-------------------------------------GETSTA------------------------------------

        public List<SaleIndentEntity> GetSAAAND(int? roleId, DateTime? startDate, DateTime? endDate, int ULocation, string UType, string Url)
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
          
            var qu = (from a in DB.Customer_Sale_Indent
                      where ((a.CSI_Raised_date.Value >= startDate.Value) && (a.CSI_Raised_date.Value <= endDate.Value)) && a.is_Deleted == false && a.CSI_Approved_Flag == null && a.User_Location_Id == ULocation && a.User_Type == UType
                      select new SaleIndentEntity
                      //a).AsEnumerable().Select(a => new SaleIndentEntity
                      {
                          CSI_id = a.CSI_id,
                          CSI_Number = a.CSI_Number,
                          DC_Code = a.DC_Code,
                          CSI_Raised_date = a.CSI_Raised_date,
                          CSI_Timestamp = a.CSI_Timestamp,
                          Customer_Id = a.Customer_Id,
                          Customer_Code = a.Customer_Code,
                          Customer_Name = a.Customer_Name,
                          SKU_Type = a.SKU_Type,
                          SKU_Type_Id = a.SKU_Type_Id,
                          Delivery_Location_ID = a.Delivery_Location_ID,
                          Delivery_Location_Code = a.Delivery_Location_Code,
                          Delievery_Type = a.Delievery_Type,
                          Delivery_cycle = a.Delivery_cycle,
                          Delivery_Expected_Date = a.Delivery_Expected_Date,
                          Delivery_Date = a.Delivery_Date,
                          CSI_raised_by = a.CSI_raised_by,
                          CSI_Approved_Flag = a.CSI_Approved_Flag,
                          CSI_Approved_by = a.CSI_Approved_by,
                          CSI_Approved_date = a.CSI_Approved_date,
                          Indent_Id = a.Indent_Id,
                          Indent_Code = a.Indent_Code,
                          Rate_Template_Code = a.Rate_Template_Code,
                          Indent_Name = a.Indent_Name,
                          User_Location = a.User_Location,
                          User_Location_Code = a.User_Location_Code,
                          User_Location_Id = a.User_Location_Id,
                          Rate_Template_Id = a.Rate_Template_Id,
                          Rate_Template_Name = a.Rate_Template_Name,
                          Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,
                          Status = a.Status,
                          Reason = a.Reason,
                          //Menu_Id = menuAccess.MenuID,
                          //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                          //is_Create = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Add"]),
                          //is_Delete = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Delete"]),
                          //is_Edit = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Edit"]),
                          //is_Approval = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Approval"]),
                          //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"]),
                          is_Create = iCrt,
                          is_Delete = isDel,
                          is_Edit = isEdt,
                          is_Approval = isApp,
                          is_View = isViw,
                          is_Deleted = a.is_Deleted,
                          Counting = (from p in DB.CSI_Line_item
                                      where p.CSI_id == a.CSI_id
                                      select new
                                      {
                                          CSI_id = p.CSI_id
                                      }).Count()
                      }).ToList();
            //foreach (var t in qu)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return qu;
        }
        //public List<SaleIndentEntity> GetSAAOR(DateTime? startDate, DateTime? endDate, string status, string ULocation)
        //{
        //    var qu = (from a in DB.Customer_Sale_Indent
        //              where ((a.CSI_Raised_date.Value >= startDate.Value) && (a.CSI_Raised_date.Value <= endDate.Value) || a.Status == status) && a.is_Deleted == false && a.CSI_Approved_Flag == null && a.DC_Code == ULocation
        //              select new SaleIndentEntity
        //              {
        //                  CSI_id = a.CSI_id,
        //                  CSI_Number = a.CSI_Number,
        //                  DC_Code = a.DC_Code,
        //                  CSI_Raised_date = a.CSI_Raised_date,
        //                  CSI_Timestamp = a.CSI_Timestamp,
        //                  Customer_Id = a.Customer_Id,
        //                  Customer_Code = a.Customer_Code,
        //                  Customer_Name = a.Customer_Name,
        //                  SKU_Type = a.SKU_Type,
        //                  SKU_Type_Id = a.SKU_Type_Id,
        //                  Delivery_Location_ID = a.Delivery_Location_ID,
        //                  Delivery_Location_Code = a.Delivery_Location_Code,
        //                  Delievery_Type = a.Delievery_Type,
        //                  Delivery_cycle = a.Delivery_cycle,
        //                  Delivery_Expected_Date = a.Delivery_Expected_Date,
        //                  Delivery_Date = a.Delivery_Date,
        //                  CSI_raised_by = a.CSI_raised_by,
        //                  CSI_Approved_Flag = a.CSI_Approved_Flag,
        //                  CSI_Approved_by = a.CSI_Approved_by,
        //                  CSI_Approved_date = a.CSI_Approved_date,
        //                  Indent_Id = a.Indent_Id,
        //                  User_Location = a.User_Location,
        //                  User_Location_Code = a.User_Location_Code,
        //                  User_Location_Id = a.User_Location_Id,
        //                  Indent_Name = a.Indent_Name,
        //                  Rate_Template_Id = a.Rate_Template_Id,
        //                  Rate_Template_Name = a.Rate_Template_Name,
        //                  Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,
        //                  Status = a.Status,
        //                  Reason = a.Reason,
        //                  is_Deleted = a.is_Deleted,
        //                  Counting = (from p in DB.CSI_Line_item
        //                              where p.CSI_id == a.CSI_id
        //                              select new
        //                              {
        //                                  CSI_id = p.CSI_id
        //                              }).Count()
        //              }).ToList();
        //    return qu;
        //}
        public string Ax()
        {
            int[] s = new int[] { 1, 2, 3, 4, };
            string aa = string.Join(",", Array.ConvertAll(s, item => item.ToString()));
            return aa;

        }
        public string Fx(string str)
        {

            string s = str;
            string[] values = s.Split('-');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

            }
            var k = values.Select(r => int.Parse(r) + 1);
            int[] n = k.ToArray();
            // IEnumerable<int> n = k;
            string m = string.Join("-", Array.ConvertAll(n, ks => ks.ToString()));
            return m;
            //   // List<string> names = "Tom,Scott,Bob".Split(',').Reverse().ToList();
            //  string Fyear;
            //  string[] yar;
            //  string o;
            //  DateTime CurrentDate = DateTime.Now;
            //  var year = (from d in DB.Customer_Dispatch_Num_Gen
            //                       select new Customer_Dispatch_Num_Gen
            //                           {
            //                               Financial_Year=(d.Financial_Year.Split('-')[0])
            //                           }).ToList();
            //   foreach(var y in year)
            //  {
            //  Fyear = y.Financial_Year;

            //      yar = Fyear.Split('-');
            //      for (int i = 0; i < yar.Length; i++)
            //      {
            //          yar[i] = yar[i].Trim();
            //          o = yar[i];     
            //      }
            //      var dg = yar.Select(r => r);

            //      //var k = o.Select(r => int.Parse(r + 1));
            //  }
            //  // return year;





            //  //if (CurrentDate.Month == 03 && CurrentDate.Day == 31)
            //  //{


            //  //}
            ////  return k;

        }

        public IEnumerable<int> Ex()
        {
            string s = "1,2, 3, 4";
            string[] values = s.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

            }
            var k = values.Select(r => int.Parse(r));
            return k;
            // List<string> names = "Tom,Scott,Bob".Split(',').Reverse().ToList();
        }

        //---------------------------------GETAUTOINCREMENT-----------------------

        public CSI_NUM_Generation GetAutoIncrement()
        {
            var autoinc = DB.CSI_NUM_Generation.Where(x => x.CSI_Num_Gen_Id == 1).FirstOrDefault();

            var model = new CSI_NUM_Generation
            {
                CSI_Num_Gen_Id = autoinc.CSI_Num_Gen_Id,
                Financial_Year = autoinc.Financial_Year,
                CSI_Last_Number = autoinc.CSI_Last_Number
            };

            return model;
        }

        //---------------------------------------GETCSILINEITEM--------------------

        public List<SaleIndentEntity> GetSaleLineItem(string id)
        {

            var list = (from a in DB.Customer_Sale_Indent
                        where a.CSI_Number == id && a.is_Deleted == false
                        select new SaleIndentEntity
                        {
                            CSI_id = a.CSI_id,
                            CSI_Number = a.CSI_Number,
                            DC_Code = a.DC_Code,
                            CSI_Raised_date = a.CSI_Raised_date,
                            CSI_Timestamp = a.CSI_Timestamp,
                            Customer_Id = a.Customer_Id,
                            Customer_Code = a.Customer_Code,
                            Customer_Name = a.Customer_Name,
                            SKU_Type = a.SKU_Type,
                            SKU_Type_Id = a.SKU_Type_Id,
                            Delivery_Location_ID = a.Delivery_Location_ID,
                            Delivery_Location_Code = a.Delivery_Location_Code,
                            Delievery_Type = a.Delievery_Type,
                            Delivery_cycle = a.Delivery_cycle,
                            Delivery_Expected_Date = a.Delivery_Expected_Date,
                            Delivery_Date = a.Delivery_Date,
                            CSI_raised_by = a.CSI_raised_by,
                            CSI_Approved_Flag = a.CSI_Approved_Flag,
                            CSI_Approved_by = a.CSI_Approved_by,
                            User_Location = a.User_Location,
                            User_Location_Code = a.User_Location_Code,
                            User_Location_Id = a.User_Location_Id,
                            CSI_Approved_date = a.CSI_Approved_date,
                            Indent_Id = a.Indent_Id,
                            Indent_Code = a.Indent_Code,
                            Indent_Name = a.Indent_Name,
                            Rate_Template_Id = a.Rate_Template_Id,
                            Rate_Template_Code = a.Rate_Template_Code,
                            Rate_Template_Name = a.Rate_Template_Name,
                            User_Type = a.User_Type,
                            Expected_Route_Id = a.Expected_Route_Id,
                            Expected_Route_Alias_Name = a.Expected_Route_Alias_Name,
                            Expected_Route_Code = a.Expected_Route_Code,
                            Expected_Delivering_Sales_Person_Id = a.Expected_Delivering_Sales_Person_Id,
                            Expected_Delivering_Sales_Person_Name = a.Expected_Delivering_Sales_Person_Name,
                            CreateByUId = (from g in DB.User_Details
                                           where g.User_Name.ToLower() == a.CreateBy.ToLower()
                                           select g.User_id).FirstOrDefault(),
                            Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,
                            //  Indent_Code = DB.Customer_Indent.Where(t=>t.Indent_Name == a.Indent_Name).Select(t=>t.Indent_Code).FirstOrDefault(),
                            //  Rate_Template_Code = DB.Rate_Template.Where(t=>t.Template_Name == a.Rate_Template_Name).Select(t=>t.Template_Code).FirstOrDefault(),
                            Validatity_Date = DB.Rate_Template.Where(t => t.Template_Code == a.Rate_Template_Code).Select(t => t.Valitity_upto).FirstOrDefault(),
                            CSI_type = a.CSI_type,
                            Status = a.Status,
                            Reason = a.Reason,
                            is_Deleted = a.is_Deleted,
                            CustomerAddress = (from m in DB.Customers
                                               where m.Cust_Id == a.Customer_Id
                                               select new CustomerEntity
                                               {
                                                   Cust_Id = m.Cust_Id,
                                                   Customer_Code = m.Customer_Code,
                                                   Customer_Name = m.Customer_Name,
                                                   Address1 = m.Address1,
                                                   Address2 = m.Address2,
                                                   City = m.City,
                                                   Sales_Person_Id = m.Sales_Person_Id,
                                                   Sales_Person_Name = m.Sales_Person_Name,
                                                   Route_Name = m.Route_Name,
                                                   Delivery_Location_Code = m.Delivery_Location_Code,
                                                   State = m.State,
                                                   District = m.District,
                                                   Pincode = m.Pincode,
                                                   Primary_Contact_Name = m.Primary_Contact_Name,
                                                   Contact_Number = m.Contact_Number,
                                                   Primary_Email_ID = m.Primary_Email_ID,
                                                   Secondary_Email_ID = m.Secondary_Email_ID,
                                                   DelieveryAddresses = (from y in DB.DeliveryAddresses
                                                                   where y.Ref_Id == m.Cust_Id && y.Ref_Obj_Type == "Customer"
                                                                   select new DelieveryAddressEntity
                                                                   {
                                                                       Delivery_Address = y.Delivery_Address,
                                                                       Delivery_Contact_Person = y.Delivery_Contact_Person,
                                                                       Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
                                                                       Delivery_Location_Id = y.Delivery_Location_Id,
                                                                       Delivery_Location_Code = y.Delivery_Location_Code,
                                                                       Delivery_Location_Name = y.Delivery_Location_Name,
                                                                       Delivery_Time = y.Delivery_Time,

                                                                   }).ToList()
                                               }
                                               ).ToList(),
                            Counting = (from p in DB.CSI_Line_item
                                        where p.CSI_id == a.CSI_id
                                        select new
                                        {
                                            CSI_id = p.CSI_id
                                        }).Count(),
                            SAL_Qty_Sum = (from p in DB.CSI_Line_item
                                           where p.CSI_Number == id
                                           group p by p.CSI_Number into g
                                           select new SAL_Qty_SumEntity
                                           {
                                               Total_Qty_Sum = g.Sum(z => z.Indent_Qty)

                                           }),
                            LineItems = (from s in DB.CSI_Line_item
                                         where s.CSI_id == a.CSI_id
                                         orderby s.SKU_Name
                                         select new CSI_LineItems_Entity
                                          {
                                              CSI_Line_Id = s.CSI_Line_Id,
                                              CSI_id = s.CSI_id,
                                              CSI_Number = s.CSI_Number,
                                              SKU_ID = s.SKU_ID,
                                              SKU_Code = s.SKU_Code,
                                              HSN_Code = s.HSN_Code,
                                              CGST = s.CGST,
                                              SGST = s.SGST,
                                              Total_GST = s.Total_GST,
                                              SKU_Name = s.SKU_Name,
                                              Pack_Size = s.Pack_Size,
                                              Grade = s.Grade,
                                              Pack_Type_Id = s.Pack_Type_Id,
                                              Pack_Type = s.Pack_Type,
                                              Pack_Weight_Type_Id = s.Pack_Weight_Type_Id,
                                              Pack_Weight_Type = s.Pack_Weight_Type,
                                              SKU_SubType_Id = s.SKU_SubType_Id,
                                              SKU_SubType = s.SKU_SubType,
                                              UOM = s.UOM,
                                              price = s.price,
                                              Indent_Qty = s.Indent_Qty,
                                              Remark = s.Remark,
                                              Status = s.Status
                                          }).ToList()
                        }).ToList();
            return list;
        }
        //
        //-------------------------------------GETSTAPPROVALLIST-------------------------

        public List<SaleIndentEntity> GetSAApprovalList(string ULocation)
        {
            var list = (from a in DB.Customer_Sale_Indent
                        where a.is_Deleted == false && a.DC_Code == ULocation
                        select new SaleIndentEntity
                        {
                            CSI_id = a.CSI_id,
                            CSI_Number = a.CSI_Number,
                            DC_Code = a.DC_Code,
                            CSI_Raised_date = a.CSI_Raised_date,
                            CSI_Timestamp = a.CSI_Timestamp,
                            Customer_Id = a.Customer_Id,
                            Customer_Code = a.Customer_Code,
                            Customer_Name = a.Customer_Name,
                            Delivery_Location_ID = a.Delivery_Location_ID,
                            Delivery_Location_Code = a.Delivery_Location_Code,
                            Delievery_Type = a.Delievery_Type,
                            SKU_Type = a.SKU_Type,
                            SKU_Type_Id = a.SKU_Type_Id,
                            Delivery_cycle = a.Delivery_cycle,
                            Delivery_Expected_Date = a.Delivery_Expected_Date,
                            Delivery_Date = a.Delivery_Date,
                            CSI_raised_by = a.CSI_raised_by,
                            CSI_Approved_Flag = a.CSI_Approved_Flag,
                            CSI_Approved_by = a.CSI_Approved_by,
                            CSI_Approved_date = a.CSI_Approved_date,
                            User_Location = a.User_Location,
                            User_Location_Code = a.User_Location_Code,
                            User_Location_Id = a.User_Location_Id,
                            Indent_Id = a.Indent_Id,
                            Indent_Name = a.Indent_Name,
                            Rate_Template_Id = a.Rate_Template_Id,
                            Rate_Template_Name = a.Rate_Template_Name,
                            Rate_Template_Valitity_upto = a.Rate_Template_Valitity_upto,
                            Status = a.Status,
                            Reason = a.Reason,
                            is_Deleted = a.is_Deleted,
                            Counting = (from p in DB.CSI_Line_item
                                        where p.CSI_id == a.CSI_id
                                        select new
                                        {
                                            CSI_id = p.CSI_id
                                        }).Count()
                        }).ToList();

            return list;
        }

        //---------------------------------------GETSTATUSES----------------------------

        public List<Tuple<string>> getStatuses()
        {

            ResourceManager rm = new ResourceManager("BusinessServices.StatusforSale", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("OpenStatus")));
            list.Add(new Tuple<string>(rm.GetString("PartialStatus")));
            list.Add(new Tuple<string>(rm.GetString("CloseStatus")));
            return list;

        }
        //-----------------------------------------------08-dec-2016-----------------------------------------------------------
        //--------------------------------------STAPPROVAL-------------------------------

        public bool slApproval(SaleIndentEntity slEntity)
        {
            var success = false;
            if (slEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var saleEntity = _unitOfWork.SaleRepository.GetByID(slEntity.CSI_id);
                    if (saleEntity != null)
                    {

                        saleEntity.CSI_Approved_Flag = slEntity.CSI_Approved_Flag;
                        saleEntity.CSI_Approved_date = DateTime.Now;
                        saleEntity.CSI_Approved_by = slEntity.CSI_Approved_by;
                        saleEntity.Reason = slEntity.Reason;

                        _unitOfWork.SaleRepository.Update(saleEntity);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        //--------------------------UPDATESALEINDENT------------------------------

        public string UpdateSaleIndent(int CSIId, BusinessEntities.SaleIndentEntity saleEntity)
        {
            string csiNumber = "";

            if (saleEntity != null)
            {
                using (var scope = new TransactionScope())
                {

                    var p = _unitOfWork.SaleRepository.GetByID(CSIId);
                    var CLT = (from ord in DB.Sales_Route_Mapping
                               where ord.Sales_Person_Name.ToLower() == p.CreateBy.ToLower()
                               select ord).FirstOrDefault();

                    if (p != null)
                    {
                        csiNumber = saleEntity.CSI_Number;
                        p.CSI_Number = saleEntity.CSI_Number;
                        p.DC_Code = saleEntity.DC_Code;
                        p.CSI_Raised_date = saleEntity.CSI_Raised_date;
                        p.CSI_Timestamp = saleEntity.CSI_Timestamp;
                        p.Customer_Id = saleEntity.Customer_Id;
                        p.Customer_Code = saleEntity.Customer_Code;
                        p.Customer_Name = saleEntity.Customer_Name;
                        p.SKU_Type_Id = saleEntity.SKU_Type_Id;
                        p.SKU_Type = saleEntity.SKU_Type;
                        p.Delivery_Location_ID = saleEntity.Delivery_Location_ID;
                        p.Delivery_Location_Code = saleEntity.Delivery_Location_Code;
                        p.Delievery_Type = saleEntity.Delievery_Type;
                        p.Delivery_cycle = saleEntity.Delivery_cycle;
                        p.Delivery_Expected_Date = saleEntity.Delivery_Expected_Date;
                        p.Delivery_Date = saleEntity.Delivery_Date;
                        p.CSI_type = saleEntity.CSI_type;
                        p.Indent_Id = saleEntity.Indent_Id;
                        p.Indent_Code = saleEntity.Indent_Code;
                        p.Rate_Template_Code = saleEntity.Rate_Template_Code;
                        p.User_Location = saleEntity.User_Location;
                        p.User_Location_Code = saleEntity.User_Location_Code;
                        p.User_Location_Id = saleEntity.User_Location_Id;
                        p.Indent_Name = saleEntity.Indent_Name;
                        p.User_Type = saleEntity.User_Type;
                        p.Rate_Template_Id = saleEntity.Rate_Template_Id;
                        p.Rate_Template_Name = saleEntity.Rate_Template_Name;
                        p.Rate_Template_Valitity_upto = saleEntity.Rate_Template_Valitity_upto;
                        p.is_Deleted = false;
                        p.UpdateDate = DateTime.Now;
                        p.UpdateBy = saleEntity.UpdateBy;
                        p.Expected_Delivering_Sales_Person_Id = CLT.Sales_Person_Id;
                        p.Expected_Delivering_Sales_Person_Name = CLT.Sales_Person_Name;
                        p.Expected_Route_Id = CLT.Sales_Route_Mapping_Id;
                        p.Expected_Route_Alias_Name = CLT.Route_Alias_Name;
                        p.Expected_Route_Code = CLT.Route_Code;
                        _unitOfWork.SaleRepository.Update(p);
                        _unitOfWork.Save();

                    }

                    var lineItemList = DB.CSI_Line_item.Where(x => x.CSI_id == CSIId).ToList();
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var list = _unitOfWork.SaleSubRepository.GetByID(li.CSI_Line_Id);

                            if (list != null)
                            {
                                _unitOfWork.SaleSubRepository.Delete(list);
                                _unitOfWork.Save();
                            }

                            scope1.Complete();
                        }
                    }

                    foreach (CSI_LineItems_Entity csiSub in saleEntity.LineItems)
                    {
                        // var line = _unitOfWork.SaleSubRepository.GetByID(csiSub.CSI_Line_Id);
                        var model = new CSI_Line_item();

                        //if(line != null)
                        //{
                        //    line.CSI_id = CSIId;
                        //    line.CSI_Number = csiNumber;
                        //    line.SKU_ID = csiSub.SKU_ID;
                        //    line.SKU_Code = csiSub.SKU_Code;
                        //    line.SKU_Name = csiSub.SKU_Name;
                        //    line.Pack_Size = csiSub.Pack_Size;
                        //    line.Pack_Type_Id = csiSub.Pack_Type_Id;
                        //    line.Pack_Type = csiSub.Pack_Type;
                        //    line.Pack_Weight_Type_Id = csiSub.Pack_Weight_Type_Id;
                        //    line.Pack_Weight_Type = csiSub.Pack_Weight_Type;
                        //    line.SKU_SubType_Id = csiSub.SKU_SubType_Id;
                        //    line.SKU_SubType = csiSub.SKU_SubType;
                        //    line.UOM = csiSub.UOM;
                        //    line.Grade = csiSub.Grade;
                        //    line.Indent_Qty = csiSub.Indent_Qty;
                        //    line.Remark = csiSub.Remark;
                        //    line.Status = "Open";
                        //    line.UpdatedDate = DateTime.Now;
                        //    line.UpdatedBy = csiSub.UpdatedBy;

                        //    _unitOfWork.SaleSubRepository.Update(line);
                        //    _unitOfWork.Save();
                        //}
                        //else
                        //{
                        model.CSI_id = CSIId;
                        model.CSI_Number = csiNumber;
                        model.SKU_ID = csiSub.SKU_ID.Value;
                        model.SKU_Code = csiSub.SKU_Code;
                        model.SKU_Name = csiSub.SKU_Name;
                        model.Pack_Size = csiSub.Pack_Size;
                        model.Pack_Type_Id = csiSub.Pack_Type_Id;
                        model.Pack_Type = csiSub.Pack_Type;
                        model.Pack_Weight_Type_Id = csiSub.Pack_Weight_Type_Id;
                        model.Pack_Weight_Type = csiSub.Pack_Weight_Type;
                        model.SKU_SubType_Id = csiSub.SKU_SubType_Id;
                        model.SKU_SubType = csiSub.SKU_SubType;
                        model.UOM = csiSub.UOM;
                        model.HSN_Code = csiSub.HSN_Code;
                        model.Total_GST = csiSub.Total_GST;
                        model.CGST = csiSub.CGST;
                        model.SGST = csiSub.SGST;
                        model.Grade = csiSub.Grade;
                        model.price = csiSub.price;
                        model.Indent_Qty = csiSub.Indent_Qty;
                        model.Remark = csiSub.Remark;
                        model.Status = "Open";
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = csiSub.CreatedBy;

                        _unitOfWork.SaleSubRepository.Insert(model);
                        _unitOfWork.Save();
                        //}

                    }
                    scope.Complete();
                }
            }
            return csiNumber;
        }
        //--------------------------------DELETESALEINDENT-------------------------

        public bool DeleteSaleIndent(int csiId)
        {
            var success = false;
            if (csiId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var g = _unitOfWork.SaleRepository.GetByID(csiId);
                    if (g != null)
                    {
                        g.is_Syunc = false;
                        g.is_Deleted = true;
                        _unitOfWork.SaleRepository.Update(g);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public List<csiNumber> GetCsiNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Customer_Sale_Indent
                         where x.CSI_Raised_date.Value == date.Value && x.is_Deleted == false && x.CSI_Approved_Flag == true && x.DC_Code == Ulocation && x.Status == "Open"
                         select new csiNumber
                         {
                             CSI_Number = x.CSI_Number
                         }).ToList();
            return query;
        }

        public List<csiNumber> GetCsiNumbers(string UserName)
        {
            var query = (from x in DB.Dispatch_Creation
                         where x.CreateBy == UserName && x.is_Deleted == false
                         select new csiNumber
                         {
                             CSI_Number = x.CSI_Number
                         }).ToList();
            return query;
        }

        public List<csiNumber> GetCsiNumbersByCreators(DateTime? date, string Ulocation, string CreatedBy)
        {
            var query = (from x in DB.Customer_Sale_Indent
                         where x.CSI_Raised_date.Value == date.Value && x.is_Deleted == false && x.CSI_Approved_Flag == true && x.DC_Code == Ulocation && x.Status == "Open" && x.CreateBy == CreatedBy
                         select new csiNumber
                         {
                             CSI_Number = x.CSI_Number
                         }).ToList();
            return query;
        }
        public List<csiCreators> GetCsiCreators(DateTime? date, string Ulocation)
        {
            //var query = (from x in DB.Customer_Sale_Indent
            //             where x.CSI_Raised_date.Value == date.Value && x.is_Deleted == false && x.CSI_Approved_Flag == true && x.DC_Code == Ulocation && x.Status == "Open"

            //             select new csiCreators
            //             {
            //                 CSI_CreatedBy = x.CreateBy
            //             }).ToList();
            var query = (from x in DB.Customer_Sale_Indent
                         where x.CSI_Raised_date.Value == date.Value && x.is_Deleted == false && x.CSI_Approved_Flag == true && x.DC_Code == Ulocation && x.Status == "Open"

                         group x by new
                         {
                             x.CreateBy
                         } into temp

                         select new csiCreators
                         {
                             Sales_Person_Name = temp.Key.CreateBy,
                             Sales_Person_Id = (from g in DB.User_Details
                                                where g.User_Name.ToLower() == temp.Key.CreateBy.ToLower()
                                                select g.User_id).FirstOrDefault(),


                         }).ToList();
            return query;
        }



        //---------------------------DELETECSILINEITEM----------------------------
        public bool DeleteCSILineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.SaleSubRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.SaleSubRepository.Delete(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool csiBulkApproval(CSIbulkApprovalEntity bulkEntity)
        {
            var success = false;
            foreach (CSIbulkIdsEntity id in bulkEntity.bulkid)
            {
                if (id.CSI_id > 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        var csiEntity = _unitOfWork.SaleRepository.GetByID(id.CSI_id);
                        if (csiEntity != null)
                        {
                            csiEntity.Reason = id.Reason;
                            csiEntity.CSI_Approved_Flag = id.CSI_Approved_Flag;
                            csiEntity.CSI_Approved_date = DateTime.Now;
                            csiEntity.CSI_Approved_by = id.CSI_Approved_by;

                            _unitOfWork.SaleRepository.Update(csiEntity);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        public List<ReturnCustomers> searchCustomers(FilterClass Filter)
        {
            List<ReturnCustomers> cust = new List<ReturnCustomers>();

            var dcCust = (from x in DB.Customers
                          where x.Is_Delete == false
                          orderby x.Customer_Name
                          select new ReturnCustomers
                          {
                              Cust_Id = x.Cust_Id,
                              Cust_Code = x.Customer_Code,
                              Cust_Name = x.Customer_Name,
                              DC_Code = x.Location_Code
                          }).ToList();

            foreach (var filt in Filter.FilterCustomers)
            {
                if (filt.DC_Code == "ALL")
                {
                    foreach (var fils in dcCust)
                    {
                        cust.Add(fils);
                    }
                }
                else
                {
                    var dcfiltcust = dcCust.Where(x => x.DC_Code == filt.DC_Code).ToList();
                    foreach (var custs in dcfiltcust)
                    {
                        cust.Add(custs);
                    }
                }
            }

            return cust;
        }

        public List<ReturnSuppliers> searchSuppliers(FilterClass Filter)
        {
            List<ReturnSuppliers> supplierslist = new List<ReturnSuppliers>();

            var dcCust = (from x in DB.Suppliers
                          where x.Is_Delete == false
                          orderby x.Supplier_Name
                          select new ReturnSuppliers
                          {
                              supplier_Id = x.Supplier_ID,
                              supplierCode = x.Supplier_code,
                              SupplierName = x.Supplier_Name,
                              DC_Code = x.Location_Code
                          }).ToList();

            foreach (var filt in Filter.FilterCustomers)
            {
                if (filt.DC_Code == "ALL")
                {
                    foreach (var fils in dcCust)
                    {
                        supplierslist.Add(fils);
                    }
                }
                else
                {
                    var dcfiltcust = dcCust.Where(x => x.DC_Code == filt.DC_Code).ToList();
                    foreach (var custs in dcfiltcust)
                    {
                        supplierslist.Add(custs);
                    }
                }
            }

            return supplierslist;
        }


        public List<SearchDCLOC> getSearchLocations()
        {
            List<SearchDCLOC> list = new List<SearchDCLOC>();

            var dc = (from x in DB.DC_Master
                      select new SearchDCLOC
                      {
                          Dc_Id = x.Dc_Id,
                          DC_Code = x.DC_Code,
                          Dc_Name = x.Dc_Name,
                          UserType = "DC"
                      }).ToList();

            var loc = (from x in DB.Location_Master
                       select new SearchDCLOC
                       {
                           Dc_Id = x.Location_Id,
                           DC_Code = x.Location_Code,
                           Dc_Name = x.Location_Name,
                           UserType = "LOCATION"
                       }).ToList();

            foreach (var dclist in dc)
            {
                list.Add(dclist);
            }

            foreach (var loclist in loc)
            {
                list.Add(loclist);
            }

            return list;
        }
    }
}