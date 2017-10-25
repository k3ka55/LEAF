using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Services.ExcelReader;
//using BusinessServices.Interfaces;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Windows.Forms;

namespace BusinessServices
{
    public class SupplierServices : ISupplierServices
    {
        private readonly UnitOfWork _unitOfWork;
        DropdownEntity drop = new DropdownEntity();
        public SupplierServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        LEAFDBEntities DB = new LEAFDBEntities();

        public List<SupplierEntity> GetsupplierById(int supplierId)
        {
             var supplierModel = (from b in DB.Suppliers
                                 where b.Supplier_ID == supplierId && b.Is_Delete == false
                                 orderby b.Supplier_Name
                                 select new SupplierEntity
                                 {
                                     Supplier_Name = b.Supplier_Name,
                                     City = b.City,
                                     Supplier_code = b.Supplier_code,
                                     Farm_location = b.Farm_location,
                                     Farm_Area = b.Farm_Area,
                                     Address1 = b.Address1,
                                     Address2 = b.Address2,
                                     State = b.State,
                                     Supplier_ID = b.Supplier_ID,
                                     UpdatedDate = b.UpdatedDate,
                                     District = b.District,
                                     Pincode = b.Pincode,
                                     Primary_Contact_Name = b.Primary_Contact_Name,
                                     Contact_Number = b.Contact_Number,
                                     CreatedDate = b.CreatedDate,
                                     CreatedBy = b.CreatedBy,
                                     UpdatedBy = b.UpdatedBy,
                                     Secondary_Email_ID = b.Secondary_Email_ID,
                                     Primary_Email_ID = b.Primary_Email_ID,
                                     Location = b.Location,
                                     Location_Code = b.Location_Code,
                                     is_Syunc = b.is_Syunc,
                                     Collection_Centre_Id = b.Collection_Centre_Id,
                                     Collection_Centre = b.Collection_Centre,
                                     Date_Of_Birth = b.Date_Of_Birth,
                                     Father_Name = b.Father_Name,
                                     PAN_Number = b.PAN_Number,
                                     Bank_Name = b.Bank_Name,
                                     Bank_Account_Number = b.Bank_Account_Number,
                                     Payment_Type_Id = b.Payment_Type_Id,
                                     Payment_Type = b.Payment_Type,
                                     Photo = b.Photo,
                                     Supplier_Activation_Date = b.Supplier_Activation_Date,
                                     Supplier_IDCard_Number = b.Supplier_IDCard_Number

                                 }).ToList();

            return supplierModel;
        }

        public List<SupplierEntity> searchSupplier(string location)
        {
            var list = (from x in DB.Suppliers
                        orderby x.Supplier_Name
                        select new SupplierEntity
                        {
                            Supplier_code = x.Supplier_code,
                            Supplier_Name = x.Supplier_Name,
                            Address1 = x.Address1,
                            Address2 = x.Address2,
                            City = x.City,
                            State = x.State,
                            District = x.District,
                            Pincode = x.Pincode,
                            Primary_Contact_Name = x.Primary_Contact_Name,
                            Contact_Number = x.Contact_Number,
                            Primary_Email_ID = x.Primary_Email_ID,
                            Secondary_Email_ID = x.Secondary_Email_ID,
                            CreatedDate = x.CreatedDate,
                            CreatedBy = x.CreatedBy,
                            Location = x.Location,
                            Location_Code = x.Location_Code
                        }).ToList();

            return list;
        }
        public Supplier_Code_Num_Gen GetAutoSupllierIncrement()
        {
            var autoinc = DB.Supplier_Code_Num_Gen.Where(x => x.Supplier_Num_Gen_Id == 1).FirstOrDefault();

            var model = new Supplier_Code_Num_Gen
            {
                Supplier_Num_Gen_Id = autoinc.Supplier_Num_Gen_Id,
                Stock_Last_Number = autoinc.Stock_Last_Number
            };

            return model;
        }

        public bool CreateSupplier(SupplierEntity supplierEntity)
        {

            string suppCode, Sup_prefix;
            int? incNumber;
            using (var iscope = new TransactionScope())
            {
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                Sup_prefix = rm.GetString("SUPT");
                Supplier_Code_Num_Gen autoIncNumber = GetAutoSupllierIncrement();
                incNumber = autoIncNumber.Stock_Last_Number;
                int? incrementedValue = incNumber + 1;
                var STincrement = DB.Supplier_Code_Num_Gen.Where(x => x.Supplier_Num_Gen_Id == 1).FirstOrDefault();
                STincrement.Stock_Last_Number = incrementedValue;
                _unitOfWork.SupplierNumGenRepository.Update(STincrement);
                _unitOfWork.Save();
                suppCode = Sup_prefix + "/" + String.Format("{0:00000}", incNumber);

                iscope.Complete();
            }


            var Profilepicname = "User_profile_";
            using (var scope = new TransactionScope())
            {

                var supplier = new Supplier
                {
                    Supplier_Name = supplierEntity.Supplier_Name,
                    City = supplierEntity.City,
                    Supplier_code = suppCode,
                    Farm_location = supplierEntity.Farm_location,
                    Farm_Area = supplierEntity.Farm_Area,
                    Address1 = supplierEntity.Address1,
                    Address2 = supplierEntity.Address2,
                    State = supplierEntity.State,
                    District = supplierEntity.District,
                    Pincode = supplierEntity.Pincode,
                    Primary_Contact_Name = supplierEntity.Primary_Contact_Name,
                    Contact_Number = supplierEntity.Contact_Number,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = supplierEntity.CreatedBy,
                    UpdatedBy = supplierEntity.UpdatedBy,
                    Secondary_Email_ID = supplierEntity.Secondary_Email_ID,
                    Primary_Email_ID = supplierEntity.Primary_Email_ID,
                    Location = supplierEntity.Location,
                    Location_Code = supplierEntity.Location_Code,
                    is_Syunc = false,
                    Collection_Centre_Id = supplierEntity.Collection_Centre_Id,
                    Collection_Centre = supplierEntity.Collection_Centre,
                    Date_Of_Birth = supplierEntity.Date_Of_Birth,
                    Father_Name = supplierEntity.Father_Name,
                    PAN_Number = supplierEntity.PAN_Number,
                    Bank_Name = supplierEntity.Bank_Name,
                    Bank_Account_Number = supplierEntity.Bank_Account_Number,
                    Payment_Type_Id = supplierEntity.Payment_Type_Id,
                    Payment_Type = supplierEntity.Payment_Type,
                    //  Photo = supplierEntity.Photo,
                    Supplier_Activation_Date = supplierEntity.Supplier_Activation_Date,
                    Supplier_IDCard_Number = supplierEntity.Supplier_IDCard_Number,
                    Is_Delete = false
                    //Supplier_code = supplierEntity.Supplier_code,
                    //Supplier_Name = supplierEntity.Supplier_Name,
                    //Address1 = supplierEntity.Address1,
                    //Address2 = supplierEntity.Address2,
                    //City = supplierEntity.City,
                    //State = supplierEntity.State,
                    //District = supplierEntity.District,
                    //Pincode = supplierEntity.Pincode,
                    //Primary_Contact_Name = supplierEntity.Primary_Contact_Name,
                    //Contact_Number = supplierEntity.Contact_Number,
                    //Primary_Email_ID = supplierEntity.Primary_Email_ID,
                    //Secondary_Email_ID = supplierEntity.Secondary_Email_ID,
                    //CreatedDate = DateTime.Now,
                    //CreatedBy = supplierEntity.CreatedBy,
                    //Location = supplierEntity.Location,
                    //Location_Code = supplierEntity.Location_Code,
                };
                try
                {
                    _unitOfWork.SupplierRepository.Insert(supplier);
                    _unitOfWork.Save();
                    if (supplierEntity.File != null)
                    {
                        string sPath = "";
                        string vPath = "";
                        string name = "";
                        string dPath = "~/Areas/Suppliers/";
                        string dirCreatePath = "";
                        string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                        int q = supplier.Supplier_ID;
                        string ext = "";
                        ext = Path.GetExtension(supplierEntity.Photo);

                        dirCreatePath = RootPath + q;
                        if (!Directory.Exists(dirCreatePath))
                        {
                            Directory.CreateDirectory(RootPath + q);
                        }
                        sPath = RootPath + q;
                        name = Profilepicname + supplier.Supplier_ID + ext;
                        //name = Profilepicname + supplier.Supplier_ID + ".xlsx";
                        vPath = sPath + "\\" + name;
                        if (File.Exists(vPath))
                        {
                            File.WriteAllBytes(vPath, Convert.FromBase64String(supplierEntity.File));
                        }
                        else
                        {
                            File.WriteAllBytes(vPath, Convert.FromBase64String(supplierEntity.File));
                        }

                        //----------excel read functionality-------------

                        //var excelData = new ExcelData(vPath);
                        //var dataRows = excelData.GetData("report", true);                       

                        //-----------------end of excel read functionality-----------

                        //string con =
                        //            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + vPath + ";" +
                        //            @"Extended Properties='Excel 8.0;HDR=Yes;'";
                        //using (OleDbConnection connection = new OleDbConnection(con))
                        //{
                        //    connection.Open();
                        //    OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                        //    using (OleDbDataReader dr = command.ExecuteReader())
                        //    {
                        //        while (dr.Read())
                        //        {
                        //            var row1Col0 = dr[0];
                        //            Console.WriteLine(row1Col0);
                        //        }
                        //    }
                        //}
                    }
                    if (supplierEntity.Photo != null)
                    {

                        var SupPhotonameUpdate = (from u in DB.Suppliers
                                                  where u.Supplier_ID == supplier.Supplier_ID
                                                  select u).FirstOrDefault();

                        if (SupPhotonameUpdate != null)
                        {
                            string extn = "";
                            extn = Path.GetExtension(supplierEntity.Photo);
                            SupPhotonameUpdate.Photo = Profilepicname + supplier.Supplier_ID + extn;
                            DB.Entry(SupPhotonameUpdate).State = EntityState.Modified;
                            DB.SaveChanges();

                        }

                        
                    }





                    //int? delId = customer.Cust_Id;

                    //var model = new DeliveryAddress();
                    //foreach (DelieveryAddressEntity pSub in customerEntity.DelieveryAddress)
                    //{
                    //    model.Ref_Id = delId;
                    //    model.Delivery_Location_Id = pSub.Delivery_Location_Id;
                    //    model.Delivery_Location_Code = pSub.Delivery_Location_Code;
                    //    model.Delivery_Location_Name = pSub.Delivery_Location_Name;
                    //    model.Delivery_Contact_Person = pSub.Delivery_Contact_Person;
                    //    model.Delivery_Contact_Person_No = pSub.Delivery_Contact_Person_No;
                    //    model.Delivery_Time = pSub.Delivery_Time;
                    //    model.Ref_Obj_Type = "Customer";

                    //    _unitOfWork.DelieveryAddressRepository.Insert(model);
                    //    _unitOfWork.Save();

                    //}
                    scope.Complete();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

        }



        public bool UpdateSupplier(int supplierId, BusinessEntities.SupplierEntity supplierEntity)
        {
            var success = false;
            var Profilepicname = "User_profile_";
            if (supplierEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var supplier = _unitOfWork.SupplierRepository.GetByID(supplierId);
                    if (supplier != null)
                    {
                        supplier.Supplier_Name = supplierEntity.Supplier_Name;
                        supplier.City = supplierEntity.City;
                        supplier.Farm_location = supplierEntity.Farm_location;
                        supplier.Farm_Area = supplierEntity.Farm_Area;
                        supplier.Address1 = supplierEntity.Address1;
                        supplier.Address2 = supplierEntity.Address2;
                        supplier.State = supplierEntity.State;
                        supplier.Supplier_ID = supplierEntity.Supplier_ID;
                        supplier.UpdatedDate = DateTime.UtcNow;
                        supplier.District = supplierEntity.District;
                        supplier.Pincode = supplierEntity.Pincode;
                        supplier.Primary_Contact_Name = supplierEntity.Primary_Contact_Name;
                        supplier.Contact_Number = supplierEntity.Contact_Number;
                        supplier.UpdatedBy = supplierEntity.UpdatedBy;
                        supplier.Secondary_Email_ID = supplierEntity.Secondary_Email_ID;
                        supplier.Primary_Email_ID = supplierEntity.Primary_Email_ID;
                        supplier.Location = supplierEntity.Location;
                        supplier.Location_Code = supplierEntity.Location_Code;
                        supplier.is_Syunc = supplierEntity.is_Syunc;
                        supplier.Collection_Centre_Id = supplierEntity.Collection_Centre_Id;
                        supplier.Collection_Centre = supplierEntity.Collection_Centre;
                        supplier.Date_Of_Birth = supplierEntity.Date_Of_Birth;
                        supplier.Father_Name = supplierEntity.Father_Name;
                        supplier.PAN_Number = supplierEntity.PAN_Number;
                        supplier.Bank_Name = supplierEntity.Bank_Name;
                        supplier.Bank_Account_Number = supplierEntity.Bank_Account_Number;
                        supplier.Payment_Type_Id = supplierEntity.Payment_Type_Id;
                        supplier.Payment_Type = supplierEntity.Payment_Type;
                        supplier.Supplier_Activation_Date = supplierEntity.Supplier_Activation_Date;
                        supplier.Supplier_IDCard_Number = supplierEntity.Supplier_IDCard_Number;

                        try
                        {
                            _unitOfWork.SupplierRepository.Update(supplier);
                            _unitOfWork.Save();
                            if (supplierEntity.File != null)
                            {
                                string sPath = "";
                                string vPath = "";
                                string name = "";
                                // string dPath = "~/Uploads/users/";
                                string dPath = "~/Areas/Suppliers/";
                                //string dPath = "D:/Projects/LEAF/latest/WebApi/WebApi/Areas/";

                                //string path = "Areas\\";
                                //string path1 = "";
                                //if (Application.StartupPath.LastIndexOf("bin") > 0)
                                //    path1 = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("bin"));

                                //string dPath = path1 + path;

                                string dirCreatePath = "";
                                string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);
                               // string RootPath = dPath;

                                int q = supplierId;
                                string ext = "";
                                ext = Path.GetExtension(supplierEntity.Photo);
                                dirCreatePath = RootPath + q;
                                if (!Directory.Exists(dirCreatePath))
                                {
                                    Directory.CreateDirectory(RootPath + q);
                                }
                                sPath = RootPath + q;
                                //sPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath + q);
                                name = Profilepicname + supplierId + ext;
                                //  name = EmployeeEntity.Employee_photo;
                                vPath = sPath + "/" + name;
                                if (File.Exists(vPath))
                                {
                                    File.WriteAllBytes(vPath, Convert.FromBase64String(supplierEntity.File));
                                }
                                else
                                {
                                    File.WriteAllBytes(vPath, Convert.FromBase64String(supplierEntity.File));
                                }
                                //int SupplierId = supplier.Supplier_ID;

                                //var model = new DC_Supplier_Mapping();
                                //foreach (DCSupplier_Mapping dcMapping in supplierEntity.SupplierMapping)
                                //{
                                //    model.Supplier_ID = SupplierId;
                                //    model.DC_id = dcMapping.DC_id;
                                //    model.DC_Code = dcMapping.DC_Code;
                                //    model.Supplier_Code = dcMapping.Supplier_Code;
                                //    model.Supplier__Name = dcMapping.Supplier_Name;
                                //    _unitOfWork.SupplierSubRepository.Insert(model);
                                //    _unitOfWork.Save();
                                //}


                            }

                            if (supplierEntity.Photo != null)
                            {

                                var SupPhotonameUpdate = (from u in DB.Suppliers
                                                          where u.Supplier_ID == supplierId
                                                          select u).FirstOrDefault();

                                if (SupPhotonameUpdate != null)
                                {
                                    string extn = "";
                                    extn = Path.GetExtension(supplierEntity.Photo);
                                    SupPhotonameUpdate.Photo = Profilepicname + supplierId + extn;
                                    DB.Entry(SupPhotonameUpdate).State = EntityState.Modified;
                                    DB.SaveChanges();

                                }
                            }

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


        public bool DeleteSupplier(int supplierId)
        {
            var success = false;
            if (supplierId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var supplier = _unitOfWork.SupplierRepository.GetByID(supplierId);
                    if (supplier != null)
                    {
                        supplier.Is_Delete = true;
                        supplier.UpdatedDate = DateTime.Now;
                        try
                        {
                            _unitOfWork.SupplierRepository.Update(supplier);
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

        //public DCSupplier_Mapping getSupplierDCinfo(string dcCode)
        //{

        //    var supplier = DB.DC_Supplier_Mapping.Where(x => x.DC_Code == dcCode).FirstOrDefault();
        //    if (supplier != null)
        //    {
        //        Mapper.CreateMap<DC_Supplier_Mapping, DCSupplier_Mapping>();
        //        var supplierModel = Mapper.Map<DC_Supplier_Mapping, DCSupplier_Mapping>(supplier);
        //        return supplierModel;
        //    }
        //    return null;
        //}

        public List<SupplierEntity> GetAllSupplier(int? roleId, string Url)
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
           // var menuAccess = DB.Role_Menu_Access
           //.Join
           //        (
           //        DB.Menu_Master,
           //        c => c.Menu_Id,
           //        d => d.Menu_Id,
           //        (c, d) => new { c, d }
           //        )
           //        .Where(e => e.c.Role_Id == roleId).Where(g => g.d.Menu_Id == g.c.Menu_Id && g.d.Url == Url).GroupBy(e => new { e.d.Menu_Id })
           //        .Select(x => new FetchMenuDetails
           //        {
           //            MenuID = x.Key.Menu_Id,
           //            MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
           //            MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
           //            RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
           //            ControllerName = x.Select(c => c.d.Url).Distinct(),
           //            ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
           //        }).FirstOrDefault();

            var supplierModel = (from b in DB.Suppliers
                                 where b.Is_Delete == false
                                 orderby b.Supplier_Name
                                 select
                                 //b).AsEnumerable().Select(b => 
                                 new SupplierEntity
                                 {
                                     Supplier_Name = b.Supplier_Name,
                                     City = b.City,
                                     Supplier_ID = b.Supplier_ID,
                                     UpdatedDate = b.UpdatedDate,
                                     Supplier_code = b.Supplier_code,
                                     Farm_location = b.Farm_location,
                                     Farm_Area = b.Farm_Area,
                                     Address1 = b.Address1,
                                     Address2 = b.Address2,
                                     State = b.State,
                                     District = b.District,
                                     Pincode = b.Pincode,
                                     Primary_Contact_Name = b.Primary_Contact_Name,
                                     Contact_Number = b.Contact_Number,
                                     CreatedDate = b.CreatedDate,
                                     CreatedBy = b.CreatedBy,
                                     UpdatedBy = b.UpdatedBy,
                                     Secondary_Email_ID = b.Secondary_Email_ID,
                                     Primary_Email_ID = b.Primary_Email_ID,
                                     Location = b.Location,
                                     Location_Code = b.Location_Code,
                                     is_Syunc = b.is_Syunc,
                                     Collection_Centre_Id = -b.Collection_Centre_Id,
                                     Collection_Centre = b.Collection_Centre,
                                     Date_Of_Birth = b.Date_Of_Birth,
                                     Father_Name = b.Father_Name,
                                     PAN_Number = b.PAN_Number,
                                     Bank_Name = b.Bank_Name,
                                     Bank_Account_Number = b.Bank_Account_Number,
                                     Payment_Type_Id = b.Payment_Type_Id,
                                     Payment_Type = b.Payment_Type,
                                     Photo = b.Photo,
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
                                     Supplier_Activation_Date = b.Supplier_Activation_Date,
                                     Supplier_IDCard_Number = b.Supplier_IDCard_Number

                                 }).ToList();
            //foreach (var t in supplierModel)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}

            return supplierModel;
        }
    }
}