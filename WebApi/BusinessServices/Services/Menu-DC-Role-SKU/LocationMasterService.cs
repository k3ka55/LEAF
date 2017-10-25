using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices.Services.Menu_DC_Role_SKU
{
    public class LocationMasterService : ILocationMasterService
    {
        private readonly UnitOfWork _unitOfWork;

        public LocationMasterService()
        {
            _unitOfWork = new UnitOfWork();
        }
        LEAFDBEntities DB = new LEAFDBEntities();

        public LocationMasterEntity GetlocationmasterById(int locationId)
        {
            var dcmaster = _unitOfWork.LocationsMasterRepository.GetByID(locationId);
            if (dcmaster != null)
            {
                Mapper.CreateMap<Location_Master, LocationMasterEntity>();
                var dcmasterModel = Mapper.Map<Location_Master, LocationMasterEntity>(dcmaster);
                return dcmasterModel;
            }
            return null;
        }

        public IEnumerable<LocationMasterEntity> GetAllLocationMaster(int? roleId, string Url)
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

              var dcmaster = (from x in DB.Location_Master
                            select 
                            //x).AsEnumerable().Select(x => 
                            new LocationMasterEntity
                            {
                                Location_Code = x.Location_Code,
                                Location_Name = x.Location_Name,
                                Address1 = x.Address1,
                                Address2 = x.Address2,
                                County = x.County,
                                State = x.State,
                                City = x.City,
                                Region = x.Region,
                                Region_Code = x.Region_Code,
                                Region_Id = x.Region_Id,
                                PinCode = x.PinCode,
                                CreatedBy = x.CreatedBy,
                                CreatedDate = x.CreatedDate,
                                Location_Id = x.Location_Id,
                                UpdatedBy = x.UpdatedBy,
                                UpdatedDate = x.UpdatedDate,
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

              foreach (var t in dcmaster)
              {
                  t.is_Create = iCrt;
                  t.is_Delete = isDel;
                  t.is_Edit = isEdt;
                  t.is_Approval = isApp;
                  t.is_View = isViw;
              }
            return dcmaster;
            //if (dcmaster.Any())
            // {
            //     Mapper.CreateMap<Location_Master, LocationMasterEntity>();
            //     var dcmasterModel = Mapper.Map<List<Location_Master>, List<LocationMasterEntity>>(dcmaster);
            //     return dcmasterModel;
            // }
            // return null;
        }

        public int createLocationMaster(LocationMasterEntity locationEntity)
        {
            using (var scope = new TransactionScope())
            {
                var location = new Location_Master
                {
                    Location_Code = locationEntity.Location_Code,
                    Location_Name = locationEntity.Location_Name,
                    Address1 = locationEntity.Address1,
                    Address2 = locationEntity.Address2,
                    County = locationEntity.County,
                    State = locationEntity.State,
                    City = locationEntity.City,
                    Region = locationEntity.Region,
                    Region_Code = locationEntity.Region_Code,
                    Region_Id = locationEntity.Region_Id,
                    PinCode = locationEntity.PinCode,
                    CreatedBy = locationEntity.CreatedBy,
                    CreatedDate = locationEntity.CreatedDate,
                };

                _unitOfWork.LocationMasterRepository.Insert(location);
                _unitOfWork.Save();

                scope.Complete();

                return location.Location_Id;
            }

          //  return 0;
        }

        public bool updateLocationMaster(int id, LocationMasterEntity locationEntity)
        {
            using (var scope = new TransactionScope())
            {
                var lmaster = _unitOfWork.LocationsMasterRepository.GetByID(id);

                if (lmaster != null)
                {
                    lmaster.Location_Code = locationEntity.Location_Code;
                    lmaster.Location_Name = locationEntity.Location_Name;
                    lmaster.Address1 = locationEntity.Address1;
                    lmaster.Address2 = locationEntity.Address2;
                    lmaster.County = locationEntity.County;
                    lmaster.State = locationEntity.State;
                    lmaster.City = locationEntity.City;
                    lmaster.Region = locationEntity.Region;
                    lmaster.Region_Code = locationEntity.Region_Code;
                    lmaster.Region_Id = locationEntity.Region_Id;
                    lmaster.PinCode = locationEntity.PinCode;
                    lmaster.CreatedBy = locationEntity.CreatedBy;
                    lmaster.CreatedDate = locationEntity.CreatedDate;

                    _unitOfWork.LocationsMasterRepository.Update(lmaster);
                    _unitOfWork.Save();
                    scope.Complete();
                    return true;
                }
            }

            return false;
        }

        public bool DeleteLocation(int locationId)
        {
            if (locationId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var lmaster = _unitOfWork.LocationsMasterRepository.GetByID(locationId);
                    if (lmaster != null)
                    {
                        _unitOfWork.LocationsMasterRepository.Delete(lmaster);
                        _unitOfWork.Save();
                    }

                    var locationAccess = DB.User_Location_Access.Where(x => x.Location_id == locationId).ToList();
                    foreach (var location in locationAccess)
                    {
                        DB.User_Location_Access.Remove(location);
                        DB.SaveChanges();
                    }

                    scope.Complete();
                    return true;
                }
            }
            return false;
        }
    }
}
