using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IMobileLoginServices
    {
        List<ReturnCustomers> searchCustomers(FilterClass Filter);
        List<MobileRateIndentEntity> GetRateIndent(string id);
        List<MobileRateIndentEntity> searchTemplate(int? roleId, int region_id, string location, string dccode, string Url);
        MobileCustSuppDDEntity GetCustSuppDD();
        List<SaleIndentEntity> SearchSA(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string SalesPersonName, string Url);
        MobileLoginResponseEntity login(UserDetailsEntity user);
        List<DCMasterModel> GetDC();
        string ReturnString(string output);
        string CreateSaleIndent(SaleIndentEntity saleEntity);
        MobileInvoiceStatementResponseEntity GetInvoiceStatement(DateTime? startDate, DateTime? endDate, string CustCode);
        List<InvoiceEntity> SearchInvoiceList(DateTime? startDate, DateTime? endDate, string CustCode);
        List<MobileSingleDispatchEntity> SingleDispatchList(int id);
        int ConnectivityCheck();
        List<CustomerIndentReturnEntity> getCIForCSI(int customerID);
        List<MobileDispatchEntity> SearchDispatchList(DateTime? startDate, DateTime? endDate, string CustomerCode);
        MobileLoginUserLocationResponseEntity GetLoginUserLocations(UserDetailsEntity user);
       // DCMasterEntity GetDC(int Dc_Id);
    }
}
