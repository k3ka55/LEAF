using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class CustomerRegistrationServices : ICustomerRegistrationServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public CustomerRegistrationServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        //
        //public List<Customer_Model> DispatchGetAllCustomer()
        //{
        //    var query = (from b in DB.Customers
        //                 orderby b.Customer_Name
        //                 select new Customer_Model
        //                 {
        //                     Cust_Id = b.Cust_Id,
        //                     Cust_Code = b.Customer_Code,
        //                     Cust_Name = b.Customer_Name
        //                 }).ToList();

        //    return query;
        //}

        public CustomerRegistrationEntity GetcustomerById(int customerId)
        {
            var customerModel = (from x in DB.Customer_Registration
                                 where x.Is_Delete == false && x.Cust_Reg_Id == customerId
                                 orderby x.Customer_Name
                                 select new CustomerRegistrationEntity
                       {
                           Cust_Reg_Id = x.Cust_Reg_Id,
                           Customer_Name = x.Customer_Name,
                           Parent_Company_Name = x.Parent_Company_Name,
                           Primary_Contact_Name = x.Primary_Contact_Name,
                           Contact_Number = x.Contact_Number,
                           Primary_Email_ID = x.Primary_Email_ID,
                           Secondary_Email_ID = x.Secondary_Email_ID,
                           Location = x.Location,
                           Location_Code = x.Location_Code,
                           Location_ID = x.Location_ID,
                           Region_Id = x.Region_Id,
                           Region_Code = x.Region_Code,
                           Region = x.Region,
                           Category_Id = x.Category_Id,
                           Category = x.Category,
                           Servicing_DC_Id = x.Servicing_DC_Id,
                           Servicing_DC = x.Servicing_DC,
                           Servicing_DC_Code = x.Servicing_DC_Code,
                           Store_Type_Id = x.Store_Type_Id,
                           Store_Type = x.Store_Type,
                           Engagement_Start_Date = x.Engagement_Start_Date,
                           Store_Image_Display_Name = x.Store_Image_Display_Name,
                           Store_Image_Name = x.Store_Image_Name,
                           Store_Owner_Image_Display_Name = x.Store_Owner_Image_Display_Name,
                           Store_Owner_Image_Name = x.Store_Owner_Image_Name,
                           Latitude = x.Latitude,
                           Longitude = x.Longitude,
                           CreatedDate = x.CreatedDate,
                           UpdatedDate = x.UpdatedDate,
                           CreatedBy = x.CreatedBy,
                           UpdatedBy = x.UpdatedBy,
                           DelieveryAddress = (from y in DB.DeliveryAddresses
                                               where y.Ref_Id == x.Cust_Reg_Id && y.Ref_Obj_Type == "CustomerReg"
                                               select new RegDelieveryAddressEntity
                                                 {
                                                     Delivery_Address = y.Delivery_Address,
                                                     Delivery_Contact_Person = y.Delivery_Contact_Person,
                                                     Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
                                                     Delivery_Location_Id = y.Delivery_Location_Id,
                                                     Delivery_Location_Code = y.Delivery_Location_Code,
                                                     Delivery_Location_Name = y.Delivery_Location_Name,
                                                     Delivery_Time = y.Delivery_Time,

                                                 }).ToList()

                       }).FirstOrDefault();

            return customerModel;
        }

        public bool DeleteCustomer(int customerId, string deletedby, string Reason)
        {
            var success = false;
            if (customerId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var customer = _unitOfWork.CustomerRegistrationRepository.GetByID(customerId);
                    if (customer != null)
                    {

                        customer.Is_Delete = true;
                        customer.UpdatedBy = deletedby;
                        customer.UpdatedDate = DateTime.Now;
                        customer.Reason = Reason;
                        try
                        {
                            _unitOfWork.CustomerRegistrationRepository.Update(customer);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                        catch (Exception)
                        {
                            success = false;
                            return success;
                        }

                    }
                }
            }
            return success;
        }
        public List<CustomerRegistrationEntity> searchCustomers(int roleId, string location, string Customer_Name, string CreatedBy, string Url)
        {
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id.Value == roleId && s.Url == Url
                              select t.Menu_Previlleges
        ).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);
            var list = (from x in DB.Customer_Registration
                        where x.Is_Approved != true && x.Is_Delete != true
                        orderby x.Customer_Name
                        select new CustomerRegistrationEntity
                        {
                            Cust_Reg_Id = x.Cust_Reg_Id,
                            Customer_Name = x.Customer_Name,
                            Primary_Contact_Name = x.Primary_Contact_Name,
                            Contact_Number = x.Contact_Number,
                            CreatedDate = x.CreatedDate,
                            UpdatedDate = x.UpdatedDate,
                            CreatedBy = x.CreatedBy,
                            UpdatedBy = x.UpdatedBy,
                            Primary_Email_ID = x.Primary_Email_ID,
                            Secondary_Email_ID = x.Secondary_Email_ID,
                            Location = x.Location,
                            Location_Code = x.Location_Code,
                            Parent_Company_Name = x.Primary_Contact_Name,
                            Region_Id = x.Region_Id,
                            Region_Code = x.Region_Code,
                            Region = x.Region,
                            Category_Id = x.Category_Id,
                            Category = x.Category,
                            Servicing_DC_Id = x.Servicing_DC_Id,
                            Servicing_DC = x.Servicing_DC,
                            Store_Type_Id = x.Store_Type_Id,
                            Store_Type = x.Store_Type,
                            is_Create = iCrt,
                            is_Delete = isDel,
                            is_Edit = isEdt,
                            is_Approval = isApp,
                            is_View = isViw,
                            Location_ID = x.Location_ID,
                            DelieveryAddress = (from y in DB.DeliveryAddresses
                                                where y.Ref_Id == x.Cust_Reg_Id && y.Ref_Obj_Type == "CustomerReg"
                                                select new RegDelieveryAddressEntity
                                                  {
                                                      Delivery_Address = y.Delivery_Address,
                                                      Delivery_Contact_Person = y.Delivery_Contact_Person,
                                                      Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
                                                      Delivery_Location_Id = y.Delivery_Location_Id,
                                                      Delivery_Location_Code = y.Delivery_Location_Code,
                                                      Delivery_Location_Name = y.Delivery_Location_Name,
                                                      Delivery_Time = y.Delivery_Time,

                                                  }).ToList()

                        }).ToList();

            if (location != "null")
            {
                list = list.Where(u => u.Location_Code == location).ToList();
            }
            if (Customer_Name != "null")
            {
                list = list.Where(u => u.Customer_Name == Customer_Name).ToList();
            }
            if (CreatedBy != "null")
            {
                list = list.Where(u => u.CreatedBy == CreatedBy).ToList();
            }



            return list;
        }

        //public List<CustomerEntity> GetAllCustomer(int? roleId, string Url)
        //{
        //    var menuAccess = (from t in DB.Role_Menu_Access
        //                      join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
        //                      where t.Role_Id == roleId && s.Url == Url
        //                      select t.Menu_Previlleges
        //        ).FirstOrDefault();
        //    int isDel, isViw, isEdt, isApp, iCrt;
        //    //  int iCrt;

        //    iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
        //    isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
        //    isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
        //    isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
        //    isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);




        //    // var menuAccess = DB.Role_Menu_Access
        //    //.Join
        //    //        (
        //    //        DB.Menu_Master,
        //    //        c => c.Menu_Id,
        //    //        d => d.Menu_Id,
        //    //        (c, d) => new { c, d }
        //    //        )
        //    //        .Where(e => e.c.Role_Id == roleId).Where(g => g.d.Menu_Id == g.c.Menu_Id && g.d.Url == Url).GroupBy(e => new { e.d.Menu_Id })
        //    //        .Select(x => new FetchMenuDetails
        //    //        {
        //    //            MenuID = x.Key.Menu_Id,
        //    //            MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
        //    //            //MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
        //    //            RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
        //    //            //ControllerName = x.Select(c => c.d.Url).Distinct(),
        //    //            //ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
        //    //        }).FirstOrDefault();
        //    // //var customer = _unitOfWork.CustomerRepository.GetAll().ToList();
        //    //if (customer.Any())
        //    //{
        //    var customerModel = (from x in DB.Customers
        //                         where x.Is_Delete == false
        //                         orderby x.Customer_Name
        //                         select
        //                             //x).AsEnumerable().Select(x => 
        //                         new CustomerEntity
        //                         {
        //                             Customer_Name = x.Customer_Name,
        //                             City = x.City,
        //                             Cust_Id = x.Cust_Id,
        //                             Customer_Code = x.Customer_Code,
        //                             Address1 = x.Address1,
        //                             Address2 = x.Address2,
        //                             State = x.State,
        //                             District = x.District,
        //                             Pincode = x.Pincode,
        //                             Primary_Contact_Name = x.Primary_Contact_Name,
        //                             Contact_Number = x.Contact_Number,
        //                             Remarks = x.Remarks,
        //                             CreatedDate = x.CreatedDate,
        //                             UpdatedDate = x.UpdatedDate,
        //                             CreatedBy = x.CreatedBy,
        //                             Sales_Person_Id = x.Sales_Person_Id,
        //                             Sales_Person_Name = x.Sales_Person_Name,
        //                             Route_Id = x.Route_Id,
        //                             Route_Name = x.Route_Name,
        //                             Route_Code = x.Route_Code,
        //                             UpdatedBy = x.UpdatedBy,
        //                             Primary_Email_ID = x.Primary_Email_ID,
        //                             Secondary_Email_ID = x.Secondary_Email_ID,
        //                             Location = x.Location,
        //                             Location_Code = x.Location_Code,
        //                             is_Syunc = x.is_Syunc,
        //                             Parent_Company_Name = x.Primary_Contact_Name,
        //                             Abbreviation = x.Abbreviation,
        //                             Region_Id = x.Region_Id,
        //                             Region_Code = x.Region_Code,
        //                             Region = x.Region,
        //                             Category_Id = x.Category_Id,
        //                             Category = x.Category,
        //                             Servicing_DC_Id = x.Servicing_DC_Id,
        //                             Servicing_DC = x.Servicing_DC,
        //                             Store_Type_Id = x.Store_Type_Id,
        //                             Store_Type = x.Store_Type,
        //                             Engagement_Type_Id = x.Engagement_Type_Id,
        //                             Engagement_Type = x.Engagement_Type,
        //                             No_of_Stores = x.No_of_Stores,
        //                             Transaction_Volume = x.Transaction_Volume,
        //                             Engagement_Start_Date = x.Engagement_Start_Date,
        //                             Account_Manger = x.Account_Manger,
        //                             Primary_ContactPerson_Name = x.Primary_ContactPerson_Name,
        //                             Primary_ContactPerson_No = x.Primary_ContactPerson_No,
        //                             Category_Head_Name = x.Category_Head_Name,
        //                             Business_Head_Name = x.Business_Head_Name,
        //                             Delivery_Days = x.Delivery_Days,
        //                             Delivery_Type = x.Delivery_Type,
        //                             Load_Per_Delivery = x.Load_Per_Delivery,
        //                             Indent_Type = x.Indent_Type,
        //                             Delivery_Location_Name = x.Delivery_Location_Name,
        //                             Delivery_Location_Code = x.Delivery_Location_Code,
        //                             Delivery_Location_Id = x.Delivery_Location_Id,
        //                             Delivery_Type_Id = x.Delivery_Type_Id,
        //                             Delivery_Address = x.Delivery_Address,
        //                             Delivery_Contact_Person = x.Delivery_Contact_Person,
        //                             Delivery_Contact_Person_No = x.Delivery_Contact_Person_No,
        //                             GRN_Receive_Shedule = x.GRN_Receive_Shedule,
        //                             GRN_Receive_Type_Id = x.GRN_Receive_Type_Id,
        //                             GRN_Receive_Type = x.GRN_Receive_Type,
        //                             Customer_Return_Policy = x.Customer_Return_Policy,
        //                             Pricing_Change_Schedule = x.Pricing_Change_Schedule,
        //                             Customer_Return_Policy_Id = x.Customer_Return_Policy_Id,
        //                             Pricing_Change_Schedule_Id = x.Pricing_Change_Schedule_Id,
        //                             Payment_Type_Id = x.Payment_Type_Id,
        //                             Payment_Type = x.Payment_Type,
        //                             Credit_Period_Id = x.Credit_Period_Id,
        //                             Credit_Period = x.Credit_Period,
        //                             Payment_Date = x.Payment_Date,
        //                             Credit_Period_Reason = x.Credit_Period_Reason,
        //                             Price_Change_Shedule_Reason = x.Price_Change_Shedule_Reason,
        //                             Credit_Limit = x.Credit_Limit,
        //                             Is_Delete = x.Is_Delete,
        //                             Customer_Tin_Number = x.Customer_Tin_Number,
        //                             Customer_Pan_Number = x.Customer_Pan_Number,
        //                             Customer_Account_Name = x.Customer_Account_Name,
        //                             Customer_Bank_Name = x.Customer_Bank_Name,
        //                             Customer_Branch_Name = x.Customer_Branch_Name,
        //                             Customer_Account_Number = x.Customer_Account_Number,
        //                             Bank_IFSC_Number = x.Bank_IFSC_Number,
        //                             Location_ID = x.Location_ID,
        //                             Store_Type_Other = x.Store_Type_Other,
        //                             is_Create = iCrt,
        //                             is_Delete = isDel,
        //                             is_Edit = isEdt,
        //                             is_Approval = isApp,
        //                             is_View = isViw,
        //                             //Menu_Id = menuAccess.MenuID,
        //                             //Menu_Name = menuAccess.MenuName,
        //                             //is_Create = iCrt,
        //                             //is_Delete = isDel,
        //                             //is_Edit = isEdt,
        //                             //is_Approval = isApp,
        //                             //is_View = isViw,
        //                             //is_Create = Convert.ToInt32(JObject.Parse(menuAccess.Menu_Previlleges)["Add"]),
        //                             //is_Delete = Convert.ToInt32(JObject.Parse(menuAccess.Menu_Previlleges)["Delete"]),
        //                             //is_Edit = Convert.ToInt32(JObject.Parse(menuAccess.Menu_Previlleges)["Edit"]),
        //                             //is_Approval = Convert.ToInt32(JObject.Parse(menuAccess.Menu_Previlleges)["Approval"]),
        //                             //is_View = Convert.ToInt32(JObject.Parse(menuAccess.Menu_Previlleges)["View"]),
        //                             DelieveryAddresses = (from y in DB.DeliveryAddresses
        //                                                   where y.Ref_Id == x.Cust_Id && y.Ref_Obj_Type == "Customer"
        //                                                   select new DelieveryAddressEntity
        //                                                   {
        //                                                       Delivery_Address = y.Delivery_Address,
        //                                                       Delivery_Contact_Person = y.Delivery_Contact_Person,
        //                                                       Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
        //                                                       Delivery_Location_Id = y.Delivery_Location_Id,
        //                                                       Delivery_Location_Code = y.Delivery_Location_Code,
        //                                                       Delivery_Location_Name = y.Delivery_Location_Name,
        //                                                       Delivery_Time = y.Delivery_Time,

        //                                                   }).ToList()

        //                         }).ToList();

        //    //foreach (var t in customerModel)
        //    //{
        //    //    t.is_Create = iCrt;
        //    //    t.is_Delete = isDel;
        //    //    t.is_Edit = isEdt;
        //    //    t.is_Approval = isApp;
        //    //    t.is_View = isViw;
        //    //}

        //    return customerModel;
        //    // }
        //    //return null;
        //}

        //public Customer_Code_Num_Gen GetAutoCustomerIncrement()
        //{
        //    var autoinc = DB.Customer_Code_Num_Gen.Where(x => x.Customer_Num_Gen_Id == 1).FirstOrDefault();

        //    var model = new Customer_Code_Num_Gen
        //    {
        //        Customer_Num_Gen_Id = autoinc.Customer_Num_Gen_Id,
        //        Customer_Last_Number = autoinc.Customer_Last_Number
        //    };

        //    return model;
        //}

        public bool RegisterCustomer(CustomerRegistrationEntity customerEntity)
        {
            var StoreImagePrefix = "Cust_StorePhoto_";
            var StoreOwnerPrefix = "Cust_StoreOwnerPhoto_";
            using (var scope = new TransactionScope())
            {
                {
                    var customer = new Customer_Registration
                    {
                        Customer_Name = customerEntity.Customer_Name,
                        Parent_Company_Name = customerEntity.Parent_Company_Name,
                        Primary_Contact_Name = customerEntity.Primary_Contact_Name,
                        Contact_Number = customerEntity.Contact_Number,
                        Primary_Email_ID = customerEntity.Primary_Email_ID,
                        Secondary_Email_ID = customerEntity.Secondary_Email_ID,
                        Location = customerEntity.Location,
                        Location_Code = customerEntity.Location_Code,
                        Location_ID = customerEntity.Location_ID,
                        Region_Id = customerEntity.Region_Id,
                        Region_Code = customerEntity.Region_Code,
                        Region = customerEntity.Region,
                        Category_Id = customerEntity.Category_Id,
                        Category = customerEntity.Category,
                        Servicing_DC_Id = customerEntity.Servicing_DC_Id,
                        Servicing_DC = customerEntity.Servicing_DC,
                        Servicing_DC_Code = customerEntity.Servicing_DC_Code,
                        Store_Type_Id = customerEntity.Store_Type_Id,
                        Store_Type = customerEntity.Store_Type,
                        Engagement_Start_Date = customerEntity.Engagement_Start_Date,
                        Store_Image_Display_Name = customerEntity.Store_Image_Display_Name,
                        Store_Owner_Image_Display_Name = customerEntity.Store_Owner_Image_Display_Name,
                        Latitude = customerEntity.Latitude,
                        Longitude = customerEntity.Longitude,
                        Created_From = customerEntity.Created_From,
                        Is_Delete = false,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = customerEntity.CreatedBy,
                        Is_Approved = false,

                    };

                    //public int Cust_Reg_Id { get; set; }      
                    //       public string Approved_by { get; set; }
                    //       public Nullable<bool> Is_Approved { get; set; }
                    //       public Nullable<System.DateTime> Date_Of_Approval { get; set; }
                    //       public Nullable<bool> Is_Delete { get; set; }
                    try
                    {
                        _unitOfWork.CustomerRegistrationRepository.Insert(customer);
                        _unitOfWork.Save();

                        int? delId = customer.Cust_Reg_Id;

                        var model = new DeliveryAddress();
                        foreach (RegDelieveryAddressEntity pSub in customerEntity.DelieveryAddress)
                        {
                            model.Ref_Id = delId;
                            model.Delivery_Location_Id = pSub.Delivery_Location_Id;
                            model.Delivery_Location_Code = pSub.Delivery_Location_Code;
                            model.Delivery_Location_Name = pSub.Delivery_Location_Name;
                            model.Delivery_Contact_Person = pSub.Delivery_Contact_Person;
                            model.Delivery_Address = pSub.Delivery_Address;
                            model.Delivery_Contact_Person_No = pSub.Delivery_Contact_Person_No;
                            model.Delivery_Time = pSub.Delivery_Time;
                            model.Ref_Obj_Type = "CustomerReg";

                            _unitOfWork.DelieveryAddressRepository.Insert(model);
                            _unitOfWork.Save();

                        }

                        if (customerEntity.StoreImageFile != null)
                        {
                            string sPath = "";
                            string vPath = "";
                            string name = "";
                            string dPath = "~/Areas/CustomerReg/";
                            string dirCreatePath = "";
                            string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                            int q = customer.Cust_Reg_Id;
                            string ext = "";
                            ext = Path.GetExtension(customerEntity.Store_Image_Display_Name);

                            dirCreatePath = RootPath  + q;
                            if (!Directory.Exists(dirCreatePath))
                            {
                                Directory.CreateDirectory(RootPath + q);
                            }
                            sPath = RootPath + q;
                            name = StoreImagePrefix + customer.Cust_Reg_Id + ext;
                            vPath = sPath + "\\" + name;
                            if (File.Exists(vPath))
                            {
                                File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreImageFile));
                            }
                            else
                            {
                                File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreImageFile));
                            }
                        }

                        if (customerEntity.StoreOwnerImageFile != null)
                        {
                            string sPath = "";
                            string vPath = "";
                            string name = "";
                            string dPath = "~/Areas/CustomerReg/";
                            string dirCreatePath = "";
                            string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                            int q = customer.Cust_Reg_Id;
                            string ext = "";
                            ext = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);

                            dirCreatePath = RootPath + q;
                            if (!Directory.Exists(dirCreatePath))
                            {
                                Directory.CreateDirectory(RootPath  + q);
                            }
                            sPath = RootPath  + q;
                            name = StoreOwnerPrefix + customer.Cust_Reg_Id + ext;
                            vPath = sPath + "\\" + name;
                            if (File.Exists(vPath))
                            {
                                File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreOwnerImageFile));
                            }
                            else
                            {
                                File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreOwnerImageFile));
                            }
                        }
                        //if (customerEntity.StoreOwnerImageFile != null || customerEntity.StoreImageFile!=null)
                        //{

                        //    var CustPhotonameUpdate = (from u in DB.Customer_Registration
                        //                               where u.Cust_Reg_Id == customer.Cust_Reg_Id
                        //                               select u).FirstOrDefault();
                        //    if (CustPhotonameUpdate != null)
                        //    {
                        //        string storeextn = "";
                        //        string storeownerextn = "";
                        //        storeextn = Path.GetExtension(customerEntity.Store_Image_Display_Name);
                        //        storeownerextn = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);
                        //        CustPhotonameUpdate.Store_Image_Name = StoreImagePrefix + customer.Cust_Reg_Id + storeextn;
                        //        CustPhotonameUpdate.Store_Owner_Image_Name = StoreOwnerPrefix + customer.Cust_Reg_Id + storeownerextn;
                        //        DB.Entry(CustPhotonameUpdate).State = EntityState.Modified;
                        //        DB.SaveChanges();
                        //    }
                        //}
                        if (customerEntity.StoreOwnerImageFile != null)
                        {

                            var CustPhotonameUpdate = (from u in DB.Customer_Registration
                                                       where u.Cust_Reg_Id == customer.Cust_Reg_Id
                                                       select u).FirstOrDefault();
                            if (CustPhotonameUpdate != null)
                            {
                                // string storeextn = "";
                                string storeownerextn = "";
                                //  storeextn = Path.GetExtension(customerEntity.Store_Image_Display_Name);
                                storeownerextn = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);
                                //CustPhotonameUpdate.Store_Image_Name = StoreImagePrefix + customer.Cust_Id + storeextn;
                                CustPhotonameUpdate.Store_Owner_Image_Name = StoreOwnerPrefix + customer.Cust_Reg_Id + storeownerextn;
                                CustPhotonameUpdate.Store_Owner_Image_Display_Name = customerEntity.Store_Owner_Image_Display_Name;
                                DB.Entry(CustPhotonameUpdate).State = EntityState.Modified;
                                DB.SaveChanges();
                            }
                        }
                        if (customerEntity.StoreImageFile != null)
                        {

                            var CustPhotonameUpdate =  (from u in DB.Customer_Registration
                                                       where u.Cust_Reg_Id == customer.Cust_Reg_Id
                                                       select u).FirstOrDefault();
                            if (CustPhotonameUpdate != null)
                            {
                                string storeextn = "";
                                //string storeownerextn = "";
                                storeextn = Path.GetExtension(customerEntity.Store_Image_Display_Name);
                                // storeownerextn = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);
                                CustPhotonameUpdate.Store_Image_Name = StoreImagePrefix + customer.Cust_Reg_Id + storeextn;
                                CustPhotonameUpdate.Store_Image_Display_Name = customerEntity.Store_Image_Display_Name;

                                //  CustPhotonameUpdate.Store_Owner_Image_Name = StoreOwnerPrefix + customer.Cust_Id + storeownerextn;
                                DB.Entry(CustPhotonameUpdate).State = EntityState.Modified;
                                DB.SaveChanges();
                            }
                        }
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    return true;
                }
            }
        }
        public Customer_Code_Num_Gen GetAutoCustomerIncrement()
        {
            var autoinc = DB.Customer_Code_Num_Gen.Where(x => x.Customer_Num_Gen_Id == 1).FirstOrDefault();

            var model = new Customer_Code_Num_Gen
            {
                Customer_Num_Gen_Id = autoinc.Customer_Num_Gen_Id,
                Customer_Last_Number = autoinc.Customer_Last_Number
            };

            return model;
        }
        public bool RegCustomerApprove(CustomerEntity customerEntity)
        {
            //var StoreImagePrefix = "Cust_StorePhoto_";
            //var StoreOwnerPrefix = "Cust_StoreOwnerPhoto_";
            string custCode, Cust_prefix;
            int? incNumber;
                        
            using (var iscope = new TransactionScope())
            {
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                Cust_prefix = rm.GetString("CUST");
                Customer_Code_Num_Gen autoIncNumber = GetAutoCustomerIncrement();
                incNumber = autoIncNumber.Customer_Last_Number;
                int? incrementedValue = incNumber + 1;
                var CTincrement = DB.Customer_Code_Num_Gen.Where(x => x.Customer_Num_Gen_Id == 1).FirstOrDefault();
                CTincrement.Customer_Last_Number = incrementedValue;
                _unitOfWork.CustomerNumGenRepository.Update(CTincrement);
                _unitOfWork.Save();
                custCode = Cust_prefix + "/" + String.Format("{0:00000}", incNumber);

                iscope.Complete();
            }

            using (var scope = new TransactionScope())
            {
                string joinedDeliveryDays;
                int[] delievryDays = new int[] { };
                delievryDays = (customerEntity.DeliveryDays).ToArray();
                joinedDeliveryDays = string.Join(",", Array.ConvertAll(delievryDays, item => item.ToString()));
                var CLT = (from ord in DB.Sales_Route_Mapping
                           where ord.Sales_Person_Id == customerEntity.Sales_Person_Id
                           select ord).FirstOrDefault();
                var customer = new Customer
                {
                    Customer_Name = customerEntity.Customer_Name,
                    City = customerEntity.City,
                    Customer_Code = custCode,
                    Sales_Person_Id = customerEntity.Sales_Person_Id,
                    Sales_Person_Name = customerEntity.Sales_Person_Name,
                    Delivery_Location_Name = CLT.Orgin_Location_Name,
                    Delivery_Location_Code = CLT.Orgin_Location_Code,
                    Delivery_Location_Id = CLT.Orgin_Location_Id,
                    Route_Id = CLT.Route_Id,
                    Route_Name = CLT.Route_Alias_Name,
                    Route_Code = CLT.Route_Code,
                    Address1 = customerEntity.Address1,
                    Address2 = customerEntity.Address2,
                    State = customerEntity.State,
                    District = customerEntity.District,
                    Pincode = customerEntity.Pincode,
                    Primary_Contact_Name = customerEntity.Primary_Contact_Name,
                    Contact_Number = customerEntity.Contact_Number,
                    Remarks = customerEntity.Remarks,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = customerEntity.CreatedBy,
                    Primary_Email_ID = customerEntity.Primary_Email_ID,
                    Secondary_Email_ID = customerEntity.Secondary_Email_ID,
                    Location = customerEntity.Location,
                    Location_Code = customerEntity.Location_Code,
                    //is_Syunc = customerEntity.is_Syunc,
                    Parent_Company_Name = customerEntity.Parent_Company_Name,
                    Abbreviation = customerEntity.Abbreviation,
                    Region_Id = customerEntity.Region_Id,
                    Region_Code = customerEntity.Region_Code,
                    Region = customerEntity.Region,
                    Category_Id = customerEntity.Category_Id,
                    Category = customerEntity.Category,
                    Servicing_DC_Id = customerEntity.Servicing_DC_Id,
                    Servicing_DC = customerEntity.Servicing_DC,
                    Store_Type_Id = customerEntity.Store_Type_Id,
                    Store_Type = customerEntity.Store_Type,
                    Engagement_Type_Id = customerEntity.Engagement_Type_Id,
                    Engagement_Type = customerEntity.Engagement_Type,
                    No_of_Stores = customerEntity.No_of_Stores,
                    Transaction_Volume = customerEntity.Transaction_Volume,
                    Engagement_Start_Date = customerEntity.Engagement_Start_Date,
                    Account_Manger = customerEntity.Account_Manger,
                    Primary_ContactPerson_Name = customerEntity.Primary_ContactPerson_Name,
                    Primary_ContactPerson_No = customerEntity.Primary_ContactPerson_No,
                    Category_Head_Name = customerEntity.Category_Head_Name,
                    Business_Head_Name = customerEntity.Business_Head_Name,
                    Delivery_Days = joinedDeliveryDays,
                    Delivery_Type = customerEntity.Delivery_Type,
                    Load_Per_Delivery = customerEntity.Load_Per_Delivery,
                    Indent_Type = customerEntity.Indent_Type,
                    Delivery_Type_Id = customerEntity.Delivery_Type_Id,
                    Delivery_Address = customerEntity.Delivery_Address,
                    Delivery_Contact_Person = customerEntity.Delivery_Contact_Person,
                    Delivery_Contact_Person_No = customerEntity.Delivery_Contact_Person_No,
                    GRN_Receive_Shedule = customerEntity.GRN_Receive_Shedule,
                    GRN_Receive_Type_Id = customerEntity.GRN_Receive_Type_Id,
                    GRN_Receive_Type = customerEntity.GRN_Receive_Type,
                    Customer_Return_Policy = customerEntity.Customer_Return_Policy,
                    Pricing_Change_Schedule = customerEntity.Pricing_Change_Schedule,
                    Customer_Return_Policy_Id = customerEntity.Customer_Return_Policy_Id,
                    Pricing_Change_Schedule_Id = customerEntity.Pricing_Change_Schedule_Id,
                    Payment_Type_Id = customerEntity.Payment_Type_Id,
                    Payment_Type = customerEntity.Payment_Type,
                    Credit_Period_Id = customerEntity.Credit_Period_Id,
                    Credit_Period = customerEntity.Credit_Period,
                    Payment_Date = customerEntity.Payment_Date,
                    Credit_Limit = customerEntity.Credit_Limit,
                    Credit_Period_Reason = customerEntity.Credit_Period_Reason,
                    Price_Change_Shedule_Reason = customerEntity.Price_Change_Shedule_Reason,
                    Is_Delete = false,
                    Customer_Tin_Number = customerEntity.Customer_Tin_Number,
                    Cust_Reg_Id = customerEntity.Cust_Reg_Id,
                    Customer_Pan_Number = customerEntity.Customer_Pan_Number,
                    Customer_Account_Name = customerEntity.Customer_Account_Name,
                    Customer_Bank_Name = customerEntity.Customer_Bank_Name,
                    Customer_Branch_Name = customerEntity.Customer_Branch_Name,
                    Customer_Account_Number = customerEntity.Customer_Account_Number,
                    Bank_IFSC_Number = customerEntity.Bank_IFSC_Number,                 
                    Location_ID = customerEntity.Location_ID,
                    Store_Type_Other = customerEntity.Store_Type_Other,
                    Customer_Ref_Number = customerEntity.Customer_Ref_Number,
                    Customer_GST_Number = customerEntity.Customer_GST_Number,
                    Store_Image_Display_Name = customerEntity.Store_Image_Display_Name,
                    Store_Owner_Image_Display_Name = customerEntity.Store_Owner_Image_Display_Name,
                    Store_Image_Name = customerEntity.Store_Image_Name,
                    Store_Owner_Image_Name = customerEntity.Store_Owner_Image_Name,
                    Customer_User_Id = custCode,
                    Password = custCode,
                    Longitude = customerEntity.Longitude,
                    Latitude = customerEntity.Latitude,

                };
                try
                {
                    _unitOfWork.CustomerRepository.Insert(customer);
                    _unitOfWork.Save();

                    int? delId = customer.Cust_Id;

                    var model = new DeliveryAddress();
                    foreach (DelieveryAddressEntity pSub in customerEntity.DelieveryAddress)
                    {
                        model.Ref_Id = delId;
                        model.Delivery_Location_Id = pSub.Delivery_Location_Id;
                        model.Delivery_Location_Code = pSub.Delivery_Location_Code;
                        model.Delivery_Location_Name = pSub.Delivery_Location_Name;
                        model.Delivery_Contact_Person = pSub.Delivery_Contact_Person;
                        model.Delivery_Address = pSub.Delivery_Address;
                        model.Delivery_Contact_Person_No = pSub.Delivery_Contact_Person_No;
                        model.Delivery_Time = pSub.Delivery_Time;
                        model.Ref_Obj_Type = "Customer";

                        _unitOfWork.DelieveryAddressRepository.Insert(model);
                        _unitOfWork.Save();

                    }

                    if (customerEntity.Store_Owner_Image_Name != null)
                    {
                        string fileName = customerEntity.Store_Owner_Image_Name;
                        string sPath = "";
                        string vPath = "";
                        string dPath = "~/Areas/CustomerReg/";
                        string dirCreatePath = "";
                        string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);
                        int q = customerEntity.Cust_Reg_Id;
                        string ext = "";
                        ext = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);
                        dirCreatePath = RootPath + q;
                        sPath = RootPath + q;
                        vPath = sPath;
                        //+"\\" + fileName;
                        string sourcePath = vPath;
                        string osPath = "";
                        string ovPath = "";
                        string odPath = "~/Areas/Customers/";
                        string odirCreatePath = "";
                        string oRootPath = System.Web.Hosting.HostingEnvironment.MapPath(odPath);
                        int oq = customer.Cust_Id;
                        string oext = "";
                        oext = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);
                        odirCreatePath = RootPath + oq;
                        osPath = oRootPath + oq;
                        ovPath = osPath;
                        //+"\\" + fileName;                    
                        string targetPath = ovPath;
                        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);

                        if (!System.IO.Directory.Exists(targetPath))
                        {
                            System.IO.Directory.CreateDirectory(targetPath);
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }
                        else
                        {
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }
                    }
                   if (customerEntity.Store_Image_Name != null)
                    {
                       //
                        string fileName = customerEntity.Store_Image_Name;
                        string sPath = "";
                        string vPath = "";
                        string dPath = "~/Areas/CustomerReg/";
                        string dirCreatePath = "";
                        string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);
                        int q = customerEntity.Cust_Reg_Id;
                        string ext = "";
                        ext = Path.GetExtension(customerEntity.Store_Image_Display_Name);
                        dirCreatePath = RootPath + q;
                        sPath = RootPath + q;
                        vPath = sPath;
                        //+"\\" + fileName;
                        string sourcePath = vPath;
                        string osPath = "";
                        string ovPath = "";
                        string odPath = "~/Areas/Customers/";
                        string odirCreatePath = "";
                        string oRootPath = System.Web.Hosting.HostingEnvironment.MapPath(odPath);
                        int oq = customer.Cust_Id;
                        string oext = "";
                        oext = Path.GetExtension(customerEntity.Store_Image_Display_Name);
                        odirCreatePath = RootPath + oq;
                        osPath = oRootPath + oq;
                        ovPath = osPath;
                        //+"\\" + fileName;                    
                        string targetPath = ovPath;
                        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);

                        if (!System.IO.Directory.Exists(targetPath))
                        {
                            System.IO.Directory.CreateDirectory(targetPath);
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }
                        else
                        {
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }
                    }
                                  
                    //if (System.IO.Directory.Exists(sourcePath))
                    //{
                    //    string[] files = System.IO.Directory.GetFiles(sourcePath);
                        
                    //    foreach (string s in files)
                    //    {
                    //       fileName = System.IO.Path.GetFileName(s);
                    //        destFile = System.IO.Path.Combine(targetPath, fileName);
                    //        System.IO.File.Copy(s, destFile, true);
                    //    }
                    //}
                    //if (customerEntity.StoreImageFile != null)
                    //{
                    //    string sPath = "";
                    //    string vPath = "";
                    //    string name = "";
                    //    string dPath = "~/Areas/CustomerReg";
                    //    string dirCreatePath = "";
                    //    string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                    //    int q = customer.Cust_Id;
                    //    string ext = "";
                    //    ext = Path.GetExtension(customerEntity.Store_Image_Display_Name);

                    //    dirCreatePath = RootPath + "\\" + q;
                    //    if (!Directory.Exists(dirCreatePath))
                    //    {
                    //        Directory.CreateDirectory(RootPath + "\\" + q);
                    //    }
                    //    sPath = RootPath + "\\" + q;
                    //    name = StoreImagePrefix + customer.Cust_Id + ext;
                    //    vPath = sPath + "\\" + name;
                    //    if (File.Exists(vPath))
                    //    {
                    //        File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreImageFile));
                    //    }
                    //    else
                    //    {
                    //        File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreImageFile));
                    //    }
                    //}

                    //if (customerEntity.StoreOwnerImageFile != null)
                    //{
                    //    string sPath = "";
                    //    string vPath = "";
                    //    string name = "";
                    //    string dPath = "~/Areas/CustomerReg";
                    //    string dirCreatePath = "";
                    //    string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                    //    int q = customer.Cust_Id;
                    //    string ext = "";
                    //    ext = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);

                    //    dirCreatePath = RootPath + "\\" + q;
                    //    if (!Directory.Exists(dirCreatePath))
                    //    {
                    //        Directory.CreateDirectory(RootPath + "\\" + q);
                    //    }
                    //    sPath = RootPath + "\\" + q;
                    //    name = StoreOwnerPrefix + customer.Cust_Id + ext;
                    //    vPath = sPath + "\\" + name;
                    //    if (File.Exists(vPath))
                    //    {
                    //        File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreOwnerImageFile));
                    //    }
                    //    else
                    //    {
                    //        File.WriteAllBytes(vPath, Convert.FromBase64String(customerEntity.StoreOwnerImageFile));
                    //    }
                    //}
                    var CustApprove = (from u in DB.Customer_Registration
                                       where u.Cust_Reg_Id == customerEntity.Cust_Reg_Id
                                       select u).FirstOrDefault();
                    if (CustApprove != null)
                    {
                        CustApprove.Is_Approved = true;
                        CustApprove.Approved_by = customerEntity.CreatedBy;
                        CustApprove.Date_Of_Approval = DateTime.Now;
                        DB.Entry(CustApprove).State = EntityState.Modified;
                        DB.SaveChanges();
                    }
                    var user = new User_Details
                    {
                        User_Name = custCode,
                        Password = custCode,
                        First_Name = customerEntity.Customer_Name,
                        Last_Name = customerEntity.Customer_Name,
                        Email_Id = customerEntity.Primary_Email_ID,
                        Phone = customerEntity.Contact_Number,
                        Address1 = customerEntity.Address1,
                        Address2 = customerEntity.Address2,
                        User_Login_Type = "CUSTOMER",
                        CreatedDate = DateTime.Now,
                        CreatedBy = customerEntity.CreatedBy,
                    };
                    _unitOfWork.UserRepository.Insert(user);
                    _unitOfWork.Save();
                       var roleusrAccess = new Role_User_Access
                                {
                                    User_Id = user.User_id,
                                    Role_Id = 70,
                                    CreatedBy = customerEntity.CreatedBy,
                                    CreatedDate = DateTime.UtcNow
                                };

                       _unitOfWork.RoleUserAccessRepository.Insert(roleusrAccess);
                       _unitOfWork.Save();
                    //if (customerEntity.Store_Owner_Image_Display_Name != null)
                    //{

                    //    var CustPhotonameUpdate = (from u in DB.Customers
                    //                               where u.Cust_Id == customer.Cust_Id
                    //                               select u).FirstOrDefault();
                    //    if (CustPhotonameUpdate != null)
                    //    {
                    //        string storeextn = "";
                    //        string storeownerextn = "";
                    //        storeextn = Path.GetExtension(customerEntity.Store_Image_Display_Name);
                    //        storeownerextn = Path.GetExtension(customerEntity.Store_Owner_Image_Display_Name);
                    //        CustPhotonameUpdate.Store_Image_Name = StoreImagePrefix + customer.Cust_Id + storeextn;
                    //        CustPhotonameUpdate.Store_Owner_Image_Name = StoreOwnerPrefix + customer.Cust_Id + storeownerextn;
                    //        DB.Entry(CustPhotonameUpdate).State = EntityState.Modified;
                    //        DB.SaveChanges();
                    //    }

                    //}
                    //using (var scope = new TransactionScope())
                    //{


                    //    if (userEntity.User_Login_Type == "LOCATION")
                    //    {
                    //        foreach (var location in userEntity.userLocation)
                    //        {
                    //            var locationAccess = new User_Location_Access
                    //            {
                    //                User_Id = user.User_id,
                    //                Location_id = location.Location_id,
                    //                Location_Code = location.Location_Code,
                    //                CreatedBy = userEntity.CreatedBy,
                    //                CreatedDate = DateTime.UtcNow
                    //            };

                    //            _unitOfWork.UserLocationMappingRepository.Insert(locationAccess);
                    //            _unitOfWork.Save();
                    //        }

                    //    }
                    //    else if (userEntity.User_Login_Type == "DC")
                    //    {
                    //        foreach (var dclocation in userEntity.userDCLocation)
                    //        {
                    //            var dclocationAccess = new User_DC_Access
                    //            {
                    //                User_Id = user.User_id,
                    //                DC_id = dclocation.DC_id,
                    //                DC_Code = dclocation.DC_Code,
                    //                CreatedBy = userEntity.CreatedBy,
                    //                CreatedDate = DateTime.UtcNow,
                    //            };

                    //            _unitOfWork.UserDCMappingRepository.Insert(dclocationAccess);
                    //            _unitOfWork.Save();
                    //        }
                    //    }

                    //    if (userEntity.userroleAccess != null)
                    //    {
                    //        foreach (var roleacess in userEntity.userroleAccess)
                    //        {
                    //            var roleusrAccess = new Role_User_Access
                    //            {
                    //                User_Id = user.User_id,
                    //                Role_Id = roleacess.Role_Id,
                    //                CreatedBy = user.CreatedBy,
                    //                CreatedDate = DateTime.UtcNow
                    //            };

                    //            _unitOfWork.RoleUserAccessRepository.Insert(roleusrAccess);
                    //            _unitOfWork.Save();
                    //        }
                    //    }

                    //    scope.Complete();
                    //    return user.User_id;
                    //}
                    scope.Complete();
                }

                catch (Exception e)
                {
                    return false;
                }

                return true;
            }

        }

        //public bool UpdateCustomer(int customerId, BusinessEntities.CustomerEntity customerEntity)
        //{
        //    var success = false;


        //    if (customerEntity != null)
        //    {
        //        string joinedDeliveryDays;
        //        int[] delievryDays = new int[] { };
        //        delievryDays = (customerEntity.DeliveryDays).ToArray();
        //        joinedDeliveryDays = string.Join(",", Array.ConvertAll(delievryDays, item => item.ToString()));

        //        using (var scope = new TransactionScope())
        //        {
        //            var customer = _unitOfWork.CustomerRepository.GetByID(customerId);
        //            if (customer != null)
        //            {
        //                customer.Customer_Name = customerEntity.Customer_Name;
        //                customer.City = customerEntity.City;
        //                customer.Customer_Code = customerEntity.Customer_Code;
        //                customer.Sales_Person_Id = customerEntity.Sales_Person_Id;
        //                customer.Sales_Person_Name = customerEntity.Sales_Person_Name;
        //                customer.Route_Id = customerEntity.Route_Id;
        //                customer.Route_Name = customerEntity.Route_Name;
        //                customer.Route_Code = customerEntity.Route_Code;
        //                customer.Address1 = customerEntity.Address1;
        //                customer.Address2 = customerEntity.Address2;
        //                customer.State = customerEntity.State;
        //                customer.District = customerEntity.District;
        //                customer.Pincode = customerEntity.Pincode;
        //                customer.Primary_Contact_Name = customerEntity.Primary_Contact_Name;
        //                customer.Contact_Number = customerEntity.Contact_Number;
        //                customer.Remarks = customerEntity.Remarks;
        //                customer.Primary_Email_ID = customerEntity.Primary_Email_ID;
        //                customer.Secondary_Email_ID = customerEntity.Secondary_Email_ID;
        //                customer.Location = customerEntity.Location;
        //                customer.Location_Code = customerEntity.Location_Code;
        //                customer.Parent_Company_Name = customerEntity.Primary_Contact_Name;
        //                customer.Abbreviation = customerEntity.Abbreviation;
        //                customer.Region_Id = customerEntity.Region_Id;
        //                customer.Region_Code = customerEntity.Region_Code;
        //                customer.Region = customerEntity.Region;
        //                customer.Category_Id = customerEntity.Category_Id;
        //                customer.Category = customerEntity.Category;
        //                customer.Servicing_DC_Id = customerEntity.Servicing_DC_Id;
        //                customer.Servicing_DC = customerEntity.Servicing_DC;
        //                customer.Store_Type_Id = customerEntity.Store_Type_Id;
        //                customer.Store_Type = customerEntity.Store_Type;
        //                customer.Engagement_Type_Id = customerEntity.Engagement_Type_Id;
        //                customer.Engagement_Type = customerEntity.Engagement_Type;
        //                customer.No_of_Stores = customerEntity.No_of_Stores;
        //                customer.Transaction_Volume = customerEntity.Transaction_Volume;
        //                customer.Engagement_Start_Date = customerEntity.Engagement_Start_Date;
        //                customer.Account_Manger = customerEntity.Account_Manger;
        //                customer.Primary_ContactPerson_Name = customerEntity.Primary_ContactPerson_Name;
        //                customer.Primary_ContactPerson_No = customerEntity.Primary_ContactPerson_No;
        //                customer.Category_Head_Name = customerEntity.Category_Head_Name;
        //                customer.Business_Head_Name = customerEntity.Business_Head_Name;
        //                customer.Delivery_Days = joinedDeliveryDays;
        //                customer.Delivery_Type = customerEntity.Delivery_Type;
        //                customer.Load_Per_Delivery = customerEntity.Load_Per_Delivery;
        //                customer.Indent_Type = customerEntity.Indent_Type;
        //                customer.Delivery_Location_Name = customerEntity.Delivery_Location_Name;
        //                customer.Delivery_Location_Code = customerEntity.Delivery_Location_Code;
        //                customer.Delivery_Location_Id = customerEntity.Delivery_Location_Id;
        //                customer.Delivery_Type_Id = customerEntity.Delivery_Type_Id;
        //                customer.Delivery_Address = customerEntity.Delivery_Address;
        //                customer.Delivery_Contact_Person = customerEntity.Delivery_Contact_Person;
        //                customer.Delivery_Contact_Person_No = customerEntity.Delivery_Contact_Person_No;
        //                customer.GRN_Receive_Shedule = customerEntity.GRN_Receive_Shedule;
        //                customer.GRN_Receive_Type_Id = customerEntity.GRN_Receive_Type_Id;
        //                customer.GRN_Receive_Type = customerEntity.GRN_Receive_Type;
        //                customer.Customer_Return_Policy = customerEntity.Customer_Return_Policy;
        //                customer.Pricing_Change_Schedule = customerEntity.Pricing_Change_Schedule;
        //                customer.Customer_Return_Policy_Id = customerEntity.Customer_Return_Policy_Id;
        //                customer.Pricing_Change_Schedule_Id = customerEntity.Pricing_Change_Schedule_Id;
        //                customer.Payment_Type_Id = customerEntity.Payment_Type_Id;
        //                customer.Payment_Type = customerEntity.Payment_Type;
        //                customer.Credit_Period_Id = customerEntity.Credit_Period_Id;
        //                customer.Credit_Period = customerEntity.Credit_Period;
        //                customer.Payment_Date = customerEntity.Payment_Date;
        //                customer.Credit_Limit = customerEntity.Credit_Limit;
        //                customer.Credit_Period_Reason = customerEntity.Credit_Period_Reason;
        //                customer.Price_Change_Shedule_Reason = customerEntity.Price_Change_Shedule_Reason;
        //                customer.Customer_Tin_Number = customerEntity.Customer_Tin_Number;
        //                customer.Customer_Pan_Number = customerEntity.Customer_Pan_Number;
        //                customer.Customer_Account_Name = customerEntity.Customer_Account_Name;
        //                customer.Customer_Bank_Name = customerEntity.Customer_Bank_Name;
        //                customer.Customer_Branch_Name = customerEntity.Customer_Branch_Name;
        //                customer.Customer_Account_Number = customerEntity.Customer_Account_Number;
        //                customer.Bank_IFSC_Number = customerEntity.Bank_IFSC_Number;
        //                customer.Location_ID = customerEntity.Location_ID;
        //                customer.Store_Type_Other = customerEntity.Store_Type_Other;
        //                //customer.Customer_Code = customerEntity.Customer_Code;
        //                //customer.Customer_Name = customerEntity.Customer_Name;
        //                //customer.Address1 = customerEntity.Address1;
        //                //customer.Address2 = customerEntity.Address2;
        //                //customer.City = customerEntity.City;
        //                //customer.State = customerEntity.State;
        //                //customer.District = customerEntity.District;
        //                //customer.Pincode = customerEntity.Pincode;
        //                //customer.Primary_Contact_Name = customerEntity.Primary_Contact_Name;
        //                //customer.Contact_Number = customerEntity.Contact_Number;
        //                //customer.Primary_Email_ID = customerEntity.Primary_Email_ID;
        //                //customer.Secondary_Email_ID = customerEntity.Secondary_Email_ID;
        //                customer.UpdatedDate = DateTime.Now;
        //                customer.UpdatedBy = customerEntity.UpdatedBy;
        //                //customer.Location = customerEntity.Location;
        //                //customer.Location_Code = customerEntity.Location_Code;
        //                try
        //                {
        //                    _unitOfWork.CustomerRepository.Update(customer);
        //                    _unitOfWork.Save();

        //                    var lineItemList = DB.DeliveryAddresses.Where(x => x.Ref_Id == customerId && x.Ref_Obj_Type == "Customer").ToList();
        //                    foreach (var li in lineItemList)
        //                    {
        //                        using (var scope1 = new TransactionScope())
        //                        {
        //                            var list = _unitOfWork.DelieveryAddressRepository.GetByID(li.Delivery_Address_Id);

        //                            if (list != null)
        //                            {
        //                                _unitOfWork.DelieveryAddressRepository.Delete(list);
        //                                _unitOfWork.Save();
        //                            }

        //                            scope1.Complete();
        //                        }
        //                    }

        //                    int? delId = customerId;

        //                    var model = new DeliveryAddress();
        //                    foreach (DelieveryAddressEntity pSub in customerEntity.DelieveryAddress)
        //                    {
        //                        model.Ref_Id = delId;
        //                        model.Delivery_Location_Id = pSub.Delivery_Location_Id;
        //                        model.Delivery_Location_Code = pSub.Delivery_Location_Code;
        //                        model.Delivery_Location_Name = pSub.Delivery_Location_Name;
        //                        model.Delivery_Contact_Person = pSub.Delivery_Contact_Person;
        //                        model.Delivery_Address = pSub.Delivery_Address;
        //                        model.Delivery_Contact_Person_No = pSub.Delivery_Contact_Person_No;
        //                        model.Delivery_Time = pSub.Delivery_Time;
        //                        model.Ref_Obj_Type = "Customer";

        //                        _unitOfWork.DelieveryAddressRepository.Insert(model);
        //                        _unitOfWork.Save();

        //                    }


        //                    scope.Complete();
        //                    success = true;
        //                }
        //                catch (Exception)
        //                {
        //                    success = false;
        //                    return success;
        //                }

        //            }
        //        }
        //    }
        //    return success;
        //}


        //public bool DeleteCustomer(int customerId)
        //{
        //    var success = false;
        //    if (customerId > 0)
        //    {
        //        using (var scope = new TransactionScope())
        //        {
        //            var customer = _unitOfWork.CustomerRepository.GetByID(customerId);
        //            if (customer != null)
        //            {

        //                customer.Is_Delete = true;
        //                customer.UpdatedDate = DateTime.Now;
        //                try
        //                {
        //                    _unitOfWork.CustomerRepository.Update(customer);
        //                    _unitOfWork.Save();
        //                    scope.Complete();
        //                    success = true;
        //                }
        //                catch (Exception)
        //                {
        //                    success = false;
        //                    return success;
        //                }

        //            }
        //        }
        //    }
        //    return success;
        //}
    }
}
