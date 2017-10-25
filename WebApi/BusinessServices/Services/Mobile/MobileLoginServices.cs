using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class MobileLoginServices : IMobileLoginServices
    {

        private readonly UnitOfWork _unitOfWork;

        public MobileLoginServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        LEAFDBEntities DB = new LEAFDBEntities();
        public List<MobileRateIndentEntity> GetRateIndent(string id)
        {
            var list = (from x in DB.Rate_Template
                        where x.Template_Code == id && x.Is_Deleted == false
                        orderby x.Template_Name
                        select new MobileRateIndentEntity
                        {
                            Template_ID = x.Template_ID,
                            Template_Name = x.Template_Name,
                            DC_Code = x.DC_Code,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Valitity_upto = x.Valitity_upto,
                            CreatedDate = x.CreatedDate,
                            LineItems = (from a in DB.Rate_Template_Line_item
                                         where a.RT_id == x.Template_ID
                                         orderby a.SKU_Name
                                         select new MobileRateTemplateLineitem
                                         {
                                             SKU_SubType = a.SKU_SubType,
                                             Pack_Type = a.Pack_Type,
                                             Pack_Size = a.Pack_Size,
                                             Pack_Weight_Type = a.Pack_Weight_Type,
                                             UOM = a.UOM,
                                             Grade = a.Grade,
                                             Selling_price = a.Selling_price,
                                          }).ToList(),

                        }).ToList();

            return list;
        }

        public List<MobileRateIndentEntity> searchTemplate(int? roleId, int region_id, string location, string dccode, string Url)
        {
            List<MobileRateIndentEntity> rateINdent = new List<MobileRateIndentEntity>();
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
        
            var list = (from x in DB.Rate_Template //&& x.DC_Location == location 
                        where x.Is_Deleted == false
                        orderby x.Template_Name
                        select new MobileRateIndentEntity
                        {
                            Template_ID = x.Template_ID,
                            Template_Name = x.Template_Name,                          
                            Valitity_upto = x.Valitity_upto,
                            Location_Code=x.Location_Code,
                            Region_Id=x.Region_Id,
                            DC_Code=x.DC_Code,
                            CreatedDate = x.CreatedDate,
                            is_Create = iCrt,
                            is_Delete = isDel,
                            is_Edit = isEdt,
                            is_Approval = isApp,
                            is_View = isViw,
                            counting = (from a in DB.Rate_Template_Line_item
                                        where a.RT_id == x.Template_ID
                                        select new
                                        {
                                            RTIId = a.RT_Line_Id
                                        }).Count(),
                        
                        }).ToList();

            if (location != null && dccode == "null")
            {
                rateINdent = list.Where(x => x.Location_Code == location && x.Region_Id == region_id).ToList();
            }
            else if (location == "null" && dccode != null)
            {
                rateINdent = list.Where(x => x.DC_Code == dccode && x.Region_Id == region_id).ToList();
            }
            return rateINdent;
        }

        public List<ReturnCustomers> searchCustomers(FilterClass Filter)
        {
            List<ReturnCustomers> cust = new List<ReturnCustomers>();

            var dcCust = (from x in DB.Customers
                          where x.Is_Delete == false && x.Sales_Person_Id == Filter.Sales_Person_Id
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

        public MobileCustSuppDDEntity GetCustSuppDD()
        {
            MobileCustSuppDDEntity dropdown = new MobileCustSuppDDEntity();
            dropdown.CustCategory = ListHelper.CustCategory();
            dropdown.CustStoreType = ListHelper.CustStoreType();
         
        
            dropdown.CustRegion = (from b in DB.Region_Master
                                   orderby b.Region_Name
                                   // join a in DB.DC_Master on b.Region_Id equals a.Region_Id
                                   select new RegionEntity
                                   {
                                       Region_Id = b.Region_Id,
                                       Region_Code = b.Region_Code,
                                       // DC_Name = a.Dc_Name,
                                       Region_Name = b.Region_Name,
                                       //DC=(from a in DB.DC_Master
                                       //   where a.Region_Id==b.Region_Id
                                       //    select new DCMasterEntity
                                       //{
                                       //    Dc_Id=a.Dc_Id,
                                       //    DC_Code= a.DC_Code,
                                       //   Dc_Name = a.Dc_Name

                                       //}).ToList(), 

                                   }).ToList();

            dropdown.CustDCMaster = (from b in DB.DC_Master
                                     orderby b.Dc_Name
                                     select new DCMasterModel
                                     {
                                         DC_Id = b.Dc_Id,
                                         DC_Code = b.DC_Code,
                                         DC_City = b.Dc_Name

                                     }).ToList();

            dropdown.CustLocationMaster = (from b in DB.Location_Master
                                           orderby b.Location_Name
                                           select new LocationModel
                                           {
                                               Location_Id = b.Location_Id,
                                               Location_Code = b.Location_Code,
                                               Location_Name = b.Location_Name

                                           }).ToList();

            dropdown.SKU_Type = ListHelper.SKU_Type();



            return dropdown;

        }
        public List<DCMasterModel> GetDC()
        {
            var output = (from b in DB.DC_Master
                          orderby b.Dc_Name
                          select new DCMasterModel
                          {
                              DC_Id = b.Dc_Id,
                              DC_Code = b.DC_Code,
                              DC_City = b.City
                          }).ToList();
            return output;
        }
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
        public string CreateSaleIndent(SaleIndentEntity saleEntity)
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
                //
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
                    CSI_raised_by = saleEntity.CSI_raised_by,
                    Indent_Id = saleEntity.Indent_Id,
                    Indent_Code = saleEntity.Indent_Code,
                    CSI_From=saleEntity.CSI_From,
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
        //public DCMasterEntity GetDC(int Dc_Id)
        //{
        //    var output = (from b in DB.DC_Master
        //                  where b.Dc_Id == Dc_Id
        //                  orderby b.Dc_Name
        //                  select new DCMasterEntity
        //                  {
        //                      Dc_Id = b.Dc_Id,
        //                      DC_Code = b.DC_Code,
        //                      Dc_Name = b.Dc_Name,
        //                      Address1 = b.Address1,
        //                      Address2 = b.Address2,
        //                      CIN_No = b.CIN_No,
        //                      FSSAI_No = b.FSSAI_No,
        //                      PAN_No = b.PAN_No,
        //                      CST_No = b.CST_No,
        //                      TIN_No = b.TIN_No,
        //                      County = b.County,
        //                      State = b.State,
        //                      City = b.City,
        //                      PinCode = b.PinCode,
        //                      CreatedDate = b.CreatedDate,
        //                      UpdatedDate = b.UpdatedDate,
        //                      CreatedBy = b.CreatedBy,
        //                      UpdatedBy = b.UpdatedBy,
        //                      Region = b.Region,
        //                      Region_Id = b.Region_Id,
        //                      Region_Code = b.Region_Code
        //                  }).FirstOrDefault();
        //    return output;
        //}
        //
        //public List<csiNumber> GetCsiNumbers(string UserName)
        //{
        //    var query = (from x in DB.Dispatch_Creation
        //                 where x.CreateBy == UserName && x.is_Deleted == false
        //                 select new csiNumber
        //                 {
        //                     CSI_Number = x.CSI_Number
        //                 }).ToList();
        //    return query;
        //}
        public string AmountInWords(double? amount)
        {
            var n = (int)amount;

            if (n == 0)
                return "";
            else if (n > 0 && n <= 19)
            {
                var arr = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                return arr[n - 1] + " ";
            }
            else if (n >= 20 && n <= 99)
            {
                var arr = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                return arr[n / 10 - 2] + " " + AmountInWords(n % 10);
            }
            else if (n >= 100 && n <= 199)
            {
                return "One Hundred " + AmountInWords(n % 100);
            }
            else if (n >= 200 && n <= 999)
            {
                return AmountInWords(n / 100) + "Hundred " + AmountInWords(n % 100);
            }
            else if (n >= 1000 && n <= 1999)
            {
                return "One Thousand " + AmountInWords(n % 1000);
            }
            else if (n >= 2000 && n <= 999999)
            {
                return AmountInWords(n / 1000) + "Thousand " + AmountInWords(n % 1000);
            }
            else if (n >= 1000000 && n <= 1999999)
            {
                return "One Million " + AmountInWords(n % 1000000);
            }
            else if (n >= 1000000 && n <= 999999999)
            {
                return AmountInWords(n / 1000000) + "Million " + AmountInWords(n % 1000000);
            }
            else if (n >= 1000000000 && n <= 1999999999)
            {
                return "One Billion " + AmountInWords(n % 1000000000);
            }
            else
            {
                return AmountInWords(n / 1000000000) + "Billion " + AmountInWords(n % 1000000000);
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
        public List<MobileSingleDispatchEntity> SingleDispatchList(int id)
        {
            List<MobileSingleDispatchEntity> list = new List<MobileSingleDispatchEntity>();
            var amt = (from s in DB.Dispatch_Line_item
                       where s.Dispatch_Id == id
                       select s.Dispatch_Value).Sum();
            string amntInWords = AmountInWords(double.Parse(amt.ToString()));
            List<MobileSingleDispatchEntity> output = new List<MobileSingleDispatchEntity>();
            list = (from x in DB.Dispatch_Creation
                    where x.Dispatch_Id == id
                    select new MobileSingleDispatchEntity
                    {                       
                        Dispatch_Location_Code = x.Dispatch_Location_Code,
                        CreateBy = x.CreateBy,
                        Vehicle_No = x.Vehicle_No,
                        Indent_Rls_Date = x.Indent_Rls_Date,
                        Delivery_Date = x.Delivery_Date,
                        Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                        SKU_Type = x.SKU_Type,
                        Order_Reference = x.Order_Reference,
                        Sales_Person_Name = x.Sales_Person_Name,
                        Route_Code = x.Route_Code,
                        CSI_Id = x.CSI_Id,
                        Status=x.Status,
                        CSI_Number = x.CSI_Number,                        
                        Customer_code = x.Customer_code,
                        Customer_Name = x.Customer_Name,
                        CreatedDate = x.CreatedDate,
                        Total_Amount = amt,
                        AMNTinWord = amntInWords,
                        DCAddress = (from r in DB.DC_Master
                                     where r.DC_Code == x.Dispatch_Location_Code
                                     select new DCAddressEntity
                                     {
                                         Address1 = r.Address1,
                                         Address2 = r.Address2,
                                         County = r.County,
                                         State = r.State,
                                         PinCode = r.PinCode,
                                         FSSAI_No = r.FSSAI_No,
                                         CIN_No = r.CIN_No,
                                         CST_No = r.CST_No,
                                         PAN_No = r.PAN_No,
                                         TIN_No = r.TIN_No
                                     }).ToList(),
                        Total_Qty_Sum = (from p in DB.Dispatch_Line_item
                                         where (p.Dispatch_Id == x.Dispatch_Id)
                                         group p by p.Dispatch_Id into g
                                         select new DIS_Qty_SumEntity
                                         {
                                             Total_Qty_Sum = g.Sum(z => z.Dispatch_Qty)
                                         }),
                        Dispatch_Qty_Sum = (from p in DB.Dispatch_Line_item
                                            where (p.Dispatch_Id == x.Dispatch_Id)
                                            group p by p.Dispatch_Id into g
                                            select new DISV_Qty_SumEntity
                                            {
                                                Dis_Qty_Sum = g.Sum(z => z.Dispatch_Value)                                                
                                            }),
                        Total_Pack = (from p in DB.Dispatch_Line_item
                                      where (p.Dispatch_Id == x.Dispatch_Id)
                                      group p by p.Dispatch_Id into g
                                      select new pack_total
                                      {
                                          Total_Pack = g.Sum(z => z.No_of_Packed_Item)                                          
                                      }),                                     
                        CustomerAddress = (from m in DB.Customers
                                           where m.Customer_Code == x.Customer_code
                                           select new MobileCustomerEntity
                                           {
                                               //Cust_Id = m.Cust_Id,
                                               Customer_Code = m.Customer_Code,
                                               Customer_Name = m.Customer_Name,
                                               Customer_GST_Number = m.Customer_GST_Number,
                                               Customer_Ref_Number = m.Customer_Ref_Number,
                                               Customer_Tin_Number = m.Customer_Tin_Number,
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
                                               //Address1 = m.Address1,
                                               //Address2 = m.Address2,
                                               //City = m.City,
                                               //State = m.State,
                                               //District = m.District,
                                               //Pincode = m.Pincode,
                                               //Primary_Contact_Name = m.Primary_Contact_Name,
                                               //Contact_Number = m.Contact_Number,
                                               //Primary_Email_ID = m.Primary_Email_ID,
                                               //Secondary_Email_ID = m.Secondary_Email_ID
                                               //
                                           }).ToList(),
                        Line_Items = (from y in DB.Dispatch_Line_item
                                      where y.Dispatch_Id == x.Dispatch_Id
                                      orderby y.SKU_Name
                                      select new MobileDispatchLineItemsEntity
                                      {
                                          Dispatch_Line_Id=y.Dispatch_Line_Id,
                                          SKU_SubType = y.SKU_SubType,
                                          SKU_Name = y.SKU_Name,
                                          SKU_ID = y.SKU_ID,
                                          SKU_Code = y.SKU_Code,
                                          HSN_Code = y.HSN_Code,
                                          Indent_Qty = y.Indent_Qty,
                                          Dispatch_Qty = y.Dispatch_Qty,
                                          Received_Qty = y.Received_Qty,
                                          Accepted_Qty = y.Accepted_Qty,
                                          Return_Qty = y.Return_Qty,
                                          Dispatch_Value = y.Dispatch_Value,
                                          Dispatch_Pack_Type = y.Dispatch_Pack_Type,
                                          Pack_Type = y.Pack_Type,
                                          Pack_Size = y.Pack_Size,
                                          UOM = y.UOM,
                                          Unit_Rate = y.Unit_Rate,
                                      }).ToList(),
                    }).ToList();

            return list;
        }

        public MobileInvoiceStatementResponseEntity GetInvoiceStatement(DateTime? startDate, DateTime? endDate, string CustCode)
        {
            MobileInvoiceStatementResponseEntity Result = new MobileInvoiceStatementResponseEntity();

            Result.DateBasedCount = (from p in DB.Invoice_Creation
                                     where p.Invoice_Number == p.Invoice_Number && p.is_Invoice_Approved == true && p.is_Deleted == false
                                  && p.Invoice_Date.Value >= startDate.Value && p.Invoice_Date.Value <= endDate.Value
                                     select new
                                     {
                                         Invoice_Number = p.Invoice_Number
                                     }).Count();
            Result.OnBoardCount = (from p in DB.Invoice_Creation
                                   where p.Invoice_Number == p.Invoice_Number && p.is_Invoice_Approved == true && p.is_Deleted == false
                                   select new
                                   {
                                       Invoice_Number = p.Invoice_Number
                                   }).Count();
            Result.OnBoardAmount = (DB.Invoice_Creation
                                     .Join(DB.Invoice_Line_item,
                                     sc => sc.invoice_Id,
                                     soc => soc.Invoice_Id,
                                     (sc, soc) => new { sc, soc })
                                     .Where(p => p.sc.Invoice_Number == p.soc.Invoice_Number && p.sc.is_Invoice_Approved == true && p.sc.is_Deleted == false)
                                     .Sum(z => (double?)(z.soc.Invoice_Amount))) ?? 0;
            Result.DateBasedAmonut = (DB.Invoice_Creation
                                     .Join(DB.Invoice_Line_item,
                                     sc => sc.invoice_Id,
                                     soc => soc.Invoice_Id,
                                     (sc, soc) => new { sc, soc })
                                     .Where(p => p.sc.Invoice_Number == p.soc.Invoice_Number && p.sc.is_Invoice_Approved == true && p.sc.is_Deleted == false
                                     && p.sc.Invoice_Date.Value >= startDate.Value && p.sc.Invoice_Date.Value <= endDate.Value)
                //.Sum(z => z.soc.Strinkage_Qty ?? 0));
                                    .Sum(z => (double?)(z.soc.Invoice_Amount))) ?? 0;

            return Result;
        }

        public List<InvoiceEntity> SearchInvoiceList(DateTime? startDate, DateTime? endDate, string CustCode)
        {
            List<InvoiceEntity> Result = new List<InvoiceEntity>();

            var list = (from x in DB.Invoice_Creation
                        where x.is_Invoice_Approved == true && x.Customer_code == CustCode && x.is_Deleted == false
                        select new InvoiceEntity
                        {
                            invoice_Id = x.invoice_Id,
                            Invoice_Number = x.Invoice_Number,
                            Dispatch_id = x.Dispatch_id,
                            Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                            Invoice_Type = x.Invoice_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Invoice_Date = x.Invoice_Date,
                            Customer_Id = x.Customer_Id,
                            Customer_code = x.Customer_code,
                            Customer_Name = x.Customer_Name,
                            DC_LID = x.DC_ID,
                            DC_LCode = x.DC_Code,
                            Term_of_Payment = x.Term_of_Payment,
                            Supplier_Ref = x.Supplier_Ref,
                            Buyer_Order_No = x.Buyer_Order_No,
                            Order_Date = x.Order_Date,
                            Remark = x.Remark,
                            is_Invoice_Approved = x.is_Invoice_Approved,
                            is_Invoice_approved_by = x.is_Invoice_approved_by,
                            is_Invoice_approved_date = x.is_Invoice_approved_date,
                            is_Invoice_approved_user_id = x.is_Invoice_approved_user_id,
                            CreateBy = x.CreateBy,
                            CreatedDate = x.CreatedDate,
                            IlineItemsCount = (from p in DB.Invoice_Line_item
                                               where p.Invoice_Number == x.Invoice_Number
                                               select new
                                               {
                                                   Dispatch_Id = p.Invoice_Number
                                               }).Count(),
                            InvoiceLineItems = (from y in DB.Invoice_Line_item
                                                where y.Invoice_Id == x.invoice_Id
                                                orderby y.SKU_Name
                                                select new InvoiceLineItemEntity
                                                {
                                                    Invoice_Line_Id = y.Invoice_Line_Id,
                                                    Invoice_Id = y.Invoice_Id,
                                                    Invoice_Number = y.Invoice_Number,
                                                    Dispatch_Line_id = y.Dispatch_Line_id,
                                                    SKU_ID = y.SKU_ID,
                                                    SKU_Code = y.SKU_Code,
                                                    SKU_Name = y.SKU_Name,
                                                    Pack_Type_Id = y.Pack_Type_Id,
                                                    Pack_Type = y.Pack_Type,
                                                    SKU_SubType_Id = y.SKU_SubType_Id,
                                                    SKU_SubType = y.SKU_SubType,
                                                    Pack_Size = y.Pack_Size,
                                                    Return_Qty = y.Return_Qty,
                                                    UOM = y.UOM,
                                                    Grade = y.Grade,
                                                    Invoice_Qty = y.Invoice_Qty,
                                                    Rate = y.Rate,
                                                    Discount = y.Discount,
                                                    Invoice_Amount = y.Invoice_Amount,
                                                    Converted_Unit_Value = y.Converted_Unit_Value,
                                                    CreatedDate = x.CreatedDate,
                                                    Dispatch_Qty = y.Dispatch_Qty,
                                                    CreateBy = x.CreateBy,
                                                    UpdateBy = x.UpdateBy,
                                                    UpdateDate = x.UpdateDate,
                                                    Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                    Invoice_Date = x.Invoice_Date,
                                                    Invoice_Type = x.Invoice_Type,
                                                    SKU_Type = x.SKU_Type,
                                                    Customer_Name = x.Customer_Name,
                                                    DC_LCode = x.DC_Code,
                                                    Term_of_Payment = x.Term_of_Payment,
                                                    Supplier_Ref = x.Supplier_Ref,
                                                    Buyer_Order_No = x.Buyer_Order_No,
                                                    Order_Date = x.Order_Date,
                                                    Remark = x.Customer_Dispatch_Number
                                                }).ToList(),
                        });

            if (startDate.Value != null && endDate.Value != null)
            {
                list = list.Where(x => x.Invoice_Date.Value >= startDate.Value && x.Invoice_Date.Value <= endDate.Value);
            }

            Result = list.ToList();
           
            return Result;
        }
        public List<MobileDispatchEntity> SearchDispatchList(DateTime? startDate, DateTime? endDate, string CustomerCode)
        {
            List<MobileDispatchEntity> Result = new List<MobileDispatchEntity>();

            var DispatchList = (from x in DB.Dispatch_Creation
                                where x.Customer_code == CustomerCode && x.is_Deleted == false && x.Dispatch_Type == "Customer" && x.Invoice_Flag != true
                                select new MobileDispatchEntity
                                {
                                    Dispatched_Location_ID = x.Dispatched_Location_ID,
                                    Dispatch_Location_Code = x.Dispatch_Location_Code,
                                    Acceptance_Status = x.Acceptance_Status,
                                    Indent_Rls_Date = x.Indent_Rls_Date,
                                    CSI_Number = x.CSI_Number,
                                    Dispatch_Id = x.Dispatch_Id,
                                    Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                    Vehicle_No = x.Vehicle_No,
                                    Status=x.Status,
                                    Customer_Id = x.Customer_Id,
                                    Customer_code = x.Customer_code,
                                    Customer_Name = x.Customer_Name,
                                    Customer_Location_Name = DB.Customers.Where(i => i.Customer_Code == x.Customer_code).Select(i => i.Location).FirstOrDefault(),
                                    Delivery_Date = x.Delivery_Date,
                                    lineItemsCount = (from p in DB.Dispatch_Line_item
                                                      where p.Dispatch_Id == x.Dispatch_Id
                                                      select new
                                                      {
                                                          Dispatch_Id = p.Dispatch_Id
                                                      }).Count(),
                                });

            if (startDate.Value != null && endDate.Value != null)
            {
                DispatchList = DispatchList.Where(x => x.Delivery_Date.Value >= startDate.Value && x.Delivery_Date.Value <= endDate.Value);
            }

            Result = DispatchList.ToList();
            return Result;
        }
        public int ConnectivityCheck()
        {
            int x = 0;
            return x;
        }
        public string ReturnString(string output)
        {
            return output;
        }

        public MobileLoginResponseEntity login(UserDetailsEntity user)
        {
            MobileLoginResponseEntity loginReps = new MobileLoginResponseEntity();
            int uId = 0;
            Func<User_Details, Boolean> where = x => x.User_Name == user.User_Name && x.Password == user.Password;
            var model = _unitOfWork.UserRepository.Get(where);

            if (model != null)
            {
                loginReps.userId = model.User_id;
                uId = model.User_id;
                loginReps.message = "Login SuccessFully.";
                loginReps.status = 1;
                loginReps.userName = model.User_Name;
                loginReps.userType = model.User_Login_Type;
                loginReps.roles = (from x in DB.Role_Master
                                   join y in DB.Role_User_Access on x.Role_Id equals y.Role_Id
                                   where y.User_Id == model.User_id
                                   select new RolesEntity
                                   {
                                       roleid = x.Role_Id,
                                       rolename = x.Role_Name
                                   }).ToList();
            }
            else
            {
                loginReps.message = "Login Failed.";
                loginReps.status = 0;
            }

            return loginReps;
        }
        public List<SaleIndentEntity> SearchSA(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation,string SalesPersonName, string Url)
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
                      where a.is_Deleted == false && a.CSI_Approved_Flag == true && (a.User_Location_Code == ULocation || a.DC_Code == ULocation) && a.CreateBy == SalesPersonName
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
                //  qu = qu.Where(a => (a.CSI_Raised_date.Value.Year >= startDate.Value.Year && a.CSI_Raised_date.Value.Month >= startDate.Value.Month && a.CSI_Raised_date.Value.Day >= startDate.Value.Day) && (a.CSI_Raised_date.Value.Year <= endDate.Value.Year && a.CSI_Raised_date.Value.Month <= endDate.Value.Month && a.CSI_Raised_date.Value.Day <= endDate.Value.Day));
            }
            if (status != "null")
            {
                qu = qu.Where(a => a.Status == status);
            }

            Result = qu.ToList();
            return Result;
        }        
        public MobileLoginUserLocationResponseEntity GetLoginUserLocations(UserDetailsEntity user)
        {
            MobileLoginUserLocationResponseEntity loginReps = new MobileLoginUserLocationResponseEntity();
            string DC_Name_Check = "null";
            Func<User_Details, Boolean> where = x => x.User_id == user.User_id;
            var model = _unitOfWork.UserRepository.Get(where);

            if (model != null)
            {
                if (model.User_Login_Type == "DC")
                {
                    loginReps.location = (from x in DB.DC_Master
                                          join y in DB.User_DC_Access on x.Dc_Id equals y.DC_id
                                          where y.User_Id == model.User_id
                                          select new MobileDCLocations
                                          {
                                              Dc_Id = x.Dc_Id,
                                              DC_Code = x.DC_Code,
                                              Dc_Name = x.Dc_Name,
                                              UserType = "DC",
                                              Region_Id = x.Region_Id,
                                              Region_Code = x.Region_Code,
                                              Region_Name = x.Region
                                          }).ToList();
                    
                    loginReps.dc = new List<MobLocations>();
                    foreach (var li in loginReps.location)
                    {
                        if (loginReps.dc != null)
                        {
                            var dcli = loginReps.dc.Where(x => x.Region_Id == li.Region_Id).FirstOrDefault();
                            if (dcli != null)
                            {
                                var dcdet = new MobileDCLocations
                                {
                                    Dc_Id = li.Dc_Id,
                                    DC_Code = li.DC_Code,
                                    Dc_Name = li.Dc_Name,
                                    UserType = li.UserType
                                };
                                dcli.dcDetails.Add(dcdet);
                            }
                            else
                            {
                                var dd = new MobLocations
                                {
                                    Region_Id = li.Region_Id,
                                    Region_Name = li.Region_Name,
                                    Region_Code = li.Region_Code,
                                    dcDetails = new List<MobileDCLocations>
                                  {
                                    new MobileDCLocations
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        UserType = li.UserType
                                    }
                                }
                                };
                                loginReps.dc.Add(dd);
                            }
                        }
                        else
                        {
                            var dd = new MobLocations
                            {
                                Region_Id = li.Region_Id,
                                Region_Name = li.Region_Name,
                                Region_Code = li.Region_Code,
                                dcDetails = new List<MobileDCLocations>
                                {
                                    new MobileDCLocations
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        Offline_Flag=li.Offline_Flag,
                                         UserType = li.UserType
                                    }
                                }
                            };
                            loginReps.dc.Add(dd);
                        }
                    }
                    if (loginReps.dc.Count == 1)
                    {
                        foreach (var li in loginReps.dc)
                        {
                            foreach (var lli in li.dcDetails)
                            {
                                DC_Name_Check = lli.Dc_Name;
                            }
                        }
                        int dcount = DB.DC_Master.Where(d => d.Dc_Name == DC_Name_Check && d.Offline_Flag == true).Count();
                        if (dcount > 0)
                        {
                            loginReps.status = 2;
                        }
                    }                    
                }
                else if (model.User_Login_Type == "LOCATION")
                {
                    loginReps.location = (from x in DB.Location_Master
                                          join y in DB.User_Location_Access on x.Location_Id equals y.Location_id
                                          where y.User_Id == model.User_id
                                          select new MobileDCLocations
                                          {
                                              Dc_Id = x.Location_Id,
                                              DC_Code = x.Location_Code,
                                              Dc_Name = x.Location_Name,                                          
                                              UserType = "LOCATION",
                                              Region_Id = x.Region_Id,
                                              Region_Code = x.Region_Code,
                                              Region_Name = x.Region
                                          }).ToList();
                    loginReps.dc = new List<MobLocations>();
                    foreach (var li in loginReps.location)
                    {
                        if (loginReps.dc != null)
                        {
                            var dcli = loginReps.dc.Where(x => x.Region_Id == li.Region_Id).FirstOrDefault();
                            if (dcli != null)
                            {
                                var dcdet = new MobileDCLocations
                                {
                                    Dc_Id = li.Dc_Id,
                                    DC_Code = li.DC_Code,
                                    Dc_Name = li.Dc_Name,
                                    UserType = li.UserType
                                };
                                dcli.dcDetails.Add(dcdet);
                            }
                            else
                            {
                                var dd = new MobLocations
                                {
                                    Region_Id = li.Region_Id,
                                    Region_Name = li.Region_Name,
                                    Region_Code = li.Region_Code,
                                    dcDetails = new List<MobileDCLocations>
                                {
                                    new MobileDCLocations
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        UserType = li.UserType
                                    }
                                }
                                //
                                };
                                loginReps.dc.Add(dd);
                            }
                        }
                        else
                        {
                            var dd = new MobLocations
                            {
                                Region_Id = li.Region_Id,
                                Region_Name = li.Region_Name,
                                Region_Code = li.Region_Code,
                                dcDetails = new List<MobileDCLocations>
                                {
                                    new MobileDCLocations
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        UserType = li.UserType
                                    }
                                }
                            };
                            loginReps.dc.Add(dd);
                        }
                    }
                }
            }
            else
            {
                loginReps.message = "Login Failed.";
                loginReps.status = 0;
            }
            var CLT = (from ord in DB.Sales_Route_Mapping
                       where ord.Sales_Person_Id == user.User_id
                       select ord).FirstOrDefault();
            if(CLT!=null)
            {
                loginReps.DispatchLocations = new List<MobileDispatchLocations>
                {
                    new MobileDispatchLocations
                    {
                        Dispatch_loaction_Id=CLT.Orgin_Location_Id.Value,
                        Dispatch_loaction_Code=CLT.Orgin_Location_Code,
                        Dispatch_loaction_Name=CLT.Orgin_Location_Name
                    }
                };
            }
            return loginReps;
        }        
        //public string GenerateToken(string userName)
        //{
        //    string token = Guid.NewGuid().ToString();

        //    HttpRuntime.Cache.Add(
        //    token,
        //    userName,
        //    null,
        //    System.Web.Caching.Cache.NoAbsoluteExpiration,
        //    TimeSpan.FromMinutes(1),
        //    System.Web.Caching.CacheItemPriority.NotRemovable,
        //    null
        //    );
        //    return token;
        //}

        //public Role_Master getUserRole(int? roleId)
        //{
        //    string query = "select * from User_Role where Role_Id = '" + roleId + "'";
        //    List<Role_Master> userRole = new List<Role_Master>();
        //    Func<Role_Master, Boolean> where = x => x.Role_Id == roleId;
        //    var role = _unitOfWork.RoleRepository.Get(where);
        //    return role;
        //}

        //public bool changePassword(UserDetailsEntity userEntity)
        //{
        //    var result = false;
        //    if (userEntity != null)
        //    {
        //        using (var scope = new TransactionScope())
        //        {
        //            var user = _unitOfWork.UserRepository.GetByID(userEntity.User_id);
        //            if (user != null)
        //            {
        //                user.Password = userEntity.Password;
        //                _unitOfWork.UserRepository.Update(user);
        //                _unitOfWork.Save();
        //                scope.Complete();
        //                result = true;
        //            }
        //        }
        //    }
        //    return result;
        //}

        //public bool forgetPassword(string userName)
        //{
        //    //D:\WEB API\Layers\WebApi\BusinessServices\mail.xml
        //    string tableName = "";
        //    string senderMail = "";
        //    string mailPassword = "";
        //    string smtpserverName = "";
        //    string portNumber = "";
        //    string emailList = "";
        //    XmlTextReader reader = null;
        //    string filename = "D:\\WEB API\\Layers\\WebApi\\BusinessServices\\emaildetails.xml";
        //    reader = new XmlTextReader(filename);
        //    reader.WhitespaceHandling = WhitespaceHandling.None;
        //    while (reader.Read())
        //    {
        //        switch (reader.NodeType)
        //        {
        //            case XmlNodeType.Element: // The node is an element.
        //                tableName = reader.Name;
        //                break;
        //            case XmlNodeType.Text: //Display the text in each element.
        //                if (tableName == "mail")
        //                    senderMail = reader.Value;
        //                else if (tableName == "pw")
        //                    mailPassword = reader.Value;
        //                else if (tableName == "smtp")
        //                    smtpserverName = reader.Value;
        //                else if (tableName == "port")
        //                    portNumber = reader.Value;
        //                else if (tableName == "emlist")
        //                    emailList = reader.Value;
        //                break;
        //            case XmlNodeType.EndElement: //Display the end of the element.                        
        //                break;
        //        }
        //    }
        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient(smtpserverName);
        //    mail.From = new MailAddress(senderMail);
        //    mail.To.Add(emailList);
        //    mail.Subject = "Forget Password";
        //    mail.Body = "Change requested was from this user. user name is " + userName + "";
        //    //System.Net.Mail.Attachment attachment;
        //    //attachment = new System.Net.Mail.Attachment(saveLocation);
        //    //mail.Attachments.Add(attachment);
        //    SmtpServer.Port = int.Parse(portNumber);
        //    SmtpServer.Credentials = new System.Net.NetworkCredential(senderMail, mailPassword);
        //    SmtpServer.EnableSsl = true;
        //    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    SmtpServer.UseDefaultCredentials = false;
        //    //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
        //    //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        //    //        System.Security.Cryptography.X509Certificates.X509Chain chain,
        //    //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        //    //{
        //    //    return true;
        //    //};
        //    SmtpServer.Send(mail);
        //    return true;
        //}
    }
}
