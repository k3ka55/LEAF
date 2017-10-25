using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using BusinessEntities.Entity;

namespace BusinessServices
{
    public class RegionMasterService : IRegionMasterService
    {
  
        private readonly UnitOfWork _unitOfWork;
        public RegionMasterService()
        {
            _unitOfWork = new UnitOfWork();
        }
        LEAFDBEntities DB = new LEAFDBEntities();

        public List<RegionMasterEntity> GetAllRegion()
        {
            var query = (from x in DB.Region_Master
                         orderby x.Region_Name
                         select new RegionMasterEntity
                         {
                             Region_Id = x.Region_Id,
                             Region_Name = x.Region_Name,
                             Region_Code=x.Region_Code
                         }).ToList();
            return query;
        }



        public BusinessEntities.RegionMasterEntity GetregionmasterById(int id)
        {
            var dcmaster = _unitOfWork.RegionMasterRepository.GetByID(id);
            if (dcmaster != null)
            {
                Mapper.CreateMap<Region_Master, RegionMasterEntity>();
                var dcmasterModel = Mapper.Map<Region_Master, RegionMasterEntity>(dcmaster);
                return dcmasterModel;
            }
            return null;
        }


        public int CreateRegionMaster(RegionMasterEntity regionmasterEntity)
        {
            using (var scope = new TransactionScope())
            {
                var regionmaster = new Region_Master
                {
                    Region_Name = regionmasterEntity.Region_Name,
                    Region_Code = regionmasterEntity.Region_Code,
                    CreateBy = regionmasterEntity.CreateBy,
                    CreatedDate = DateTime.Now,
                };
                _unitOfWork.RegionMasterRepository.Insert(regionmaster);
                _unitOfWork.Save();
                scope.Complete();
                return regionmaster.Region_Id;
            }
        }

        public bool UpdateRegionMaster(int Region_Id, BusinessEntities.RegionMasterEntity regionmasterEntity)
        {
            var success = false;
            if (regionmasterEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var regionmaster = _unitOfWork.RegionMasterRepository.GetByID(Region_Id);
                    if (regionmaster != null)
                    {

                        regionmaster.Region_Name = regionmasterEntity.Region_Name;
                        regionmaster.UpdateDate = DateTime.Now;
                        regionmaster.UpdateBy = regionmasterEntity.UpdateBy;
                        regionmaster.Region_Code = regionmasterEntity.Region_Code;
                  
                        _unitOfWork.RegionMasterRepository.Update(regionmaster);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }


        public bool DeleteRegionMaster(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var dcmaster = _unitOfWork.RegionMasterRepository.GetByID(id);
                    if (dcmaster != null)
                    {

                        _unitOfWork.RegionMasterRepository.Delete(dcmaster);
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
