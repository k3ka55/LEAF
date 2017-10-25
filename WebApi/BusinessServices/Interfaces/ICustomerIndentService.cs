using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface ICustomerIndentService
    {
        int CreateCustomerIndent(CIEntity ciEntity);
        List<CIEntity> GetCustomerIndent(string id);
        List<CIEntity> SearchIndentFormapping(int regionid, string location, string dccode);
        bool DeleteCustomerIndent(string tId, string deleteReason);
        bool DeleteCustomerIndentLineItem(int Id);
        int UpdateCustomerIndent(int tId, CIEntity rateEntity);
        List<CIEntity> searchIndent(int? roleId, int regionid, string location, string dccode, string Url);
        List<CustomerIndentReturnEntity> getCIForCSI(int customerID);
        CIExcelImport ExcelImportForCI(fileImport fileDetail);
        List<CustomerIndentEditReturnEntity> SearchCIforCSIEdit(int customerID);
    }
}
