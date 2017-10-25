using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessServices
{
   public interface IDCMasterService
    {
        DCMasterEntity GetdcmasterById(int dcmasterId);
        IEnumerable<DCMasterEntity> GetAllDCMaster(int? roleId, string Url);
        IEnumerable<CountryEntity> GetAllCountries();
        int CreateDCMaster(DCMasterEntity dcmasterEntity);
        List<DCMasterEntity> DispatchGetAllDCmaster();
        bool UpdateDCMaster(int dcmasterId, DCMasterEntity dcmasterEntity);
        bool DeleteDCMaster(int dcmasterId);
        List<CustEntity> GetByLocationCustomer(string Code);
        List<SupEntity> GetByLocationSupplier(string Code);
    }
}
