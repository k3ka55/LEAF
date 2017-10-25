using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IRegionMasterService
    {
        RegionMasterEntity GetregionmasterById(int id);
        int CreateRegionMaster(RegionMasterEntity regionmasterEntity);
        List<RegionMasterEntity> GetAllRegion();
        bool UpdateRegionMaster(int Region_Id, RegionMasterEntity regionmasterEntity);
        bool DeleteRegionMaster(int id);
   
    }
}
