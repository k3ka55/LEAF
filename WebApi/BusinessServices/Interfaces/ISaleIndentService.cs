using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
  public interface ISaleIndentService
    {
      List<csiCreators> GetCsiCreators(DateTime? date, string Ulocation);
      string CreateSaleIndent(SaleIndentEntity saleEntity);
      List<SaleIndentEntity> GetSaleLineItem(string id);
      List<csiNumber> GetCsiNumbers(DateTime? date, string Ulocation);
      List<csiNumber> GetCsiNumbersByCreators(DateTime? date, string Ulocation, string CreatedBy);
      List<BulkCSIExcelFieldsReturnModel> BulkCSI(fileImportBulkCSI fileDetail);
      bool BulkCSI(BulkCSIModel saleEntity);
        List<Tuple<string>> getStatuses();
        string UpdateSaleIndent(int CSIId, BusinessEntities.SaleIndentEntity saleEntity);
        bool DeleteSaleIndent(int csiId);
        bool slApproval(SaleIndentEntity slEntity);
        //List<SaleIndentEntity> GetSAAND(DateTime? startDate, DateTime? endDate, string status, string ULocation);
        //List<SaleIndentEntity> GetSAAOR(DateTime? startDate, DateTime? endDate, string status, string ULocation);
        List<SaleIndentEntity> GetSAAAND(int? roleId, DateTime? startDate, DateTime? endDate, int ULocation, string UType, string Url);
        //List<SaleIndentEntity> GetSAOR(DateTime? startDate, DateTime? endDate, string status, string ULocation);
        List<SaleIndentEntity> GetSAApprovalList(string ULocation);
        List<SaleIndentEntity> SearchSA(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url);
        List<SaleIndentEntity> SearchConsolidatedCSI(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url);
        bool DeleteCSILineItem(int Id);
        bool SAUpdateById(SALUpdateEntity csiEntity);
        bool csiBulkApproval(CSIbulkApprovalEntity bulkEntity);
        IEnumerable<int> Ex();
        string Fx(string str);
        string Ax();
        List<SearchDCLOC> getSearchLocations();
        List<ReturnCustomers> searchCustomers(FilterClass Filter);
        List<ReturnSuppliers> searchSuppliers(FilterClass Filter);
    }
}
       