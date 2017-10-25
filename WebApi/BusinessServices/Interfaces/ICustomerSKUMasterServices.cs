using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
  public interface ICustomerSKUMasterServices
    {
    //  CustomerSKUMasterModelEntity GetCustSKUMasterById(int skuId);
     // List<CustomerSKUMasterModelEntity> GetCustSKUMasterByCategory(string skuCat);
      CustomerSKUMasterModelEntity GetCustSKUMasterById(int Id);
      List<CustomerSKUMasterLineItemModelEntity> CheckCustomerSKUMaster(CustomerSKUMasterList CustomerSKUMaster);
      dynamic GetAllCustSKUMaster(int? roleId, string Url);
      List<CustomerSKUMasterLineItemModelEntity> ExcelImportForCustSKUMapping(fileImportSTI fileDetail);
      bool CreateCustSKUMaster(CustomerSKUMasterModelEntity customerSKUMasterModelEntity);
      bool UpdateCustSKUMaster(CustomerSKUMasterModelEntity customerSKUMasterModelEntity);
      bool DeleteCustSKUMaster(int Id);
      //  List<skuReturnMappingEntity> getskuBasemainsub(int skuMappingId);
    }
}
