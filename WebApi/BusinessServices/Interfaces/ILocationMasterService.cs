using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface ILocationMasterService
    {
        LocationMasterEntity GetlocationmasterById(int locationId);
        IEnumerable<LocationMasterEntity> GetAllLocationMaster(int? roleId, string Url);
        int createLocationMaster(LocationMasterEntity locationEntity);
        bool updateLocationMaster(int id, LocationMasterEntity locationEntity);
        bool DeleteLocation(int locationId);
    }
}
