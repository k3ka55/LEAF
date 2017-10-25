using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using Newtonsoft.Json.Linq;

namespace BusinessServices
{
    public class DCMasterService : IDCMasterService
    {
        private readonly UnitOfWork _unitOfWork;
        public DCMasterService()
        {
            _unitOfWork = new UnitOfWork();
        }
        LEAFDBEntities DB = new LEAFDBEntities();

        public List<DCMasterEntity> DispatchGetAllDCmaster()
        {
            var query = (from x in DB.DC_Master
                         orderby x.DC_Code
                         select new DCMasterEntity
                         {
                             Dc_Id = x.Dc_Id,
                             DC_Code = x.DC_Code,
                         }).ToList();
            return query;
        }
        public List<CustEntity> GetByLocationCustomer(string Code)
        {
            List<CustEntity> list = new List<CustEntity>();

            list = (from x in DB.Customers
                    join y in DB.DC_Customer_Mapping on x.Cust_Id equals y.Cust_Id
                    where y.DC_Code == Code
                    orderby x.Customer_Name
                    select new CustEntity
                    {
                        Cust_Name = x.Customer_Name,
                        Cust_Id = x.Cust_Id,
                        Cust_Code = x.Customer_Code
                    }).ToList();
            return list;
        }
        public List<SupEntity> GetByLocationSupplier(string Code)
        {
            List<SupEntity> list = new List<SupEntity>();

            list = (from y in DB.Suppliers
                    join x in DB.DC_Supplier_Mapping on y.Supplier_ID equals x.Supplier_ID
                    where x.DC_Code == Code
                    orderby y.Supplier_Name
                    select new SupEntity
                    {
                        Sup_name = y.Supplier_Name,
                        Sup_Id = y.Supplier_ID,
                        Sup_Code = y.Supplier_code
                    }).ToList();
            return list;
        }

        public BusinessEntities.DCMasterEntity GetdcmasterById(int dcmasterId)
        {
            var dcmaster = _unitOfWork.LocationRepository.GetByID(dcmasterId);
            if (dcmaster != null)
            {
                Mapper.CreateMap<DC_Master, DCMasterEntity>();
                var dcmasterModel = Mapper.Map<DC_Master, DCMasterEntity>(dcmaster);
                return dcmasterModel;
            }
            return null;
        }

        public IEnumerable<BusinessEntities.DCMasterEntity> GetAllDCMaster(int? roleId, string Url)
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


            var dcmaster = (from y in DB.DC_Master
                            orderby y.Dc_Name
                            select
                                //y).AsEnumerable().Select(y => 
                            new DCMasterEntity
                                  {
                                      Dc_Id = y.Dc_Id,
                                      Dc_Name = y.Dc_Name,
                                      Address1 = y.Address1,
                                      Address2 = y.Address2,
                                      County = y.County,
                                      State = y.State,
                                      City = y.City,
                                      DC_Code = y.DC_Code,
                                      GST_Number = y.GST_Number,
                                      UIN_No = y.UIN_No,
                                      PinCode = y.PinCode,
                                      CreatedDate = y.CreatedDate,
                                      CreatedBy = y.CreatedBy,
                                      CIN_No = y.CIN_No,
                                      FSSAI_No = y.FSSAI_No,
                                      PAN_No = y.PAN_No,
                                      CST_No = y.CST_No,
                                      TIN_No = y.TIN_No,
                                      Region = y.Region,
                                      Region_Code = y.Region_Code,
                                      Region_Id = y.Region_Id,
                                      UpdatedBy = y.UpdatedBy,
                                      UpdatedDate = y.UpdatedDate,
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
                                      //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"])
                                  }).ToList();

            //foreach (var t in dcmaster)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return dcmaster;

        }

        public IEnumerable<BusinessEntities.CountryEntity> GetAllCountries()
        {
            var dcmaster = _unitOfWork.CountryRepository.GetAll().ToList();
            if (dcmaster.Any())
            {
                Mapper.CreateMap<Country, CountryEntity>();
                var dcmasterModel = Mapper.Map<List<Country>, List<CountryEntity>>(dcmaster);
                return dcmasterModel;
            }
            return null;
        }

        public int CreateDCMaster(DCMasterEntity dcmasterEntity)
        {
            using (var scope = new TransactionScope())
            {
                var dcmaster = new DC_Master
                {
                    Dc_Name = dcmasterEntity.Dc_Name,
                    Address1 = dcmasterEntity.Address1,
                    Address2 = dcmasterEntity.Address2,
                    County = dcmasterEntity.County,
                    State = dcmasterEntity.State,
                    City = dcmasterEntity.City,
                    DC_Code = dcmasterEntity.DC_Code,
                    PinCode = dcmasterEntity.PinCode,
                    CreatedDate = DateTime.Now,
                    CreatedBy = dcmasterEntity.CreatedBy,
                    CIN_No = dcmasterEntity.CIN_No,
                    FSSAI_No = dcmasterEntity.FSSAI_No,
                    PAN_No = dcmasterEntity.PAN_No,
                    CST_No = dcmasterEntity.CST_No,
                    TIN_No = dcmasterEntity.TIN_No,
                    Region = dcmasterEntity.Region,
                    Region_Code = dcmasterEntity.Region_Code,
                    Region_Id = dcmasterEntity.Region_Id,
                    GST_Number = dcmasterEntity.GST_Number,
                    UIN_No = dcmasterEntity.UIN_No,
                    Total_GST = 0,
                    CGST = 0,
                    SGST = 0,
                };
                _unitOfWork.LocationRepository.Insert(dcmaster);
                _unitOfWork.Save();
                scope.Complete();
                return dcmaster.Dc_Id;
            }
        }

        public bool UpdateDCMaster(int dcmasterId, DCMasterEntity dcmasterEntity)
        {
            var success = false;
            if (dcmasterEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var dcmaster = _unitOfWork.LocationRepository.GetByID(dcmasterId);
                    if (dcmaster != null)
                    {

                        dcmaster.Dc_Name = dcmasterEntity.Dc_Name;
                        dcmaster.Address1 = dcmasterEntity.Address1;
                        dcmaster.Address2 = dcmasterEntity.Address2;
                        dcmaster.County = dcmasterEntity.County;
                        dcmaster.State = dcmasterEntity.State;
                        dcmaster.City = dcmasterEntity.City;
                        dcmaster.PinCode = dcmasterEntity.PinCode;
                        dcmaster.UpdatedDate = DateTime.Now;
                        dcmaster.DC_Code = dcmasterEntity.DC_Code;
                        dcmaster.UpdatedBy = dcmasterEntity.UpdatedBy;
                        dcmaster.CIN_No = dcmasterEntity.CIN_No;
                        dcmaster.FSSAI_No = dcmasterEntity.FSSAI_No;
                        dcmaster.GST_Number = dcmasterEntity.GST_Number;
                        dcmaster.UIN_No = dcmasterEntity.UIN_No;
                        dcmaster.PAN_No = dcmasterEntity.PAN_No;
                        dcmaster.CST_No = dcmasterEntity.CST_No;
                        dcmaster.TIN_No = dcmasterEntity.TIN_No;
                        dcmaster.Region = dcmasterEntity.Region;
                        dcmaster.Region_Code = dcmasterEntity.Region_Code;
                        dcmaster.Region_Id = dcmasterEntity.Region_Id;

                        _unitOfWork.LocationRepository.Update(dcmaster);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }


        public bool DeleteDCMaster(int dcmasterId)
        {
            var success = false;
            if (dcmasterId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var dcmaster = _unitOfWork.LocationRepository.GetByID(dcmasterId);
                    if (dcmaster != null)
                    {

                        _unitOfWork.LocationRepository.Delete(dcmaster);
                        _unitOfWork.Save();

                    }

                    var locationAccess = DB.User_DC_Access.Where(x => x.DC_id == dcmasterId).ToList();
                    foreach (var location in locationAccess)
                    {
                        DB.User_DC_Access.Remove(location);
                        DB.SaveChanges();
                    }

                    scope.Complete();
                    success = true;
                }
            }
            return success;
        }
    }
}
