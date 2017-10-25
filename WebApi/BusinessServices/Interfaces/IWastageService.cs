using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IWastageService
    {
        string CreateWastage(WastageEntity wsEntity);
        List<WastageEntity> GetWastageItem(string id);
        bool wsBulkApproval(bulkWastApprovalEntity bulkwasEntity);
        string UpdateWastage(int wsId, WastageEntity wsEntity);
        bool DeleteWastage(int wsId);
        bool DeleteWastageLineItem(int Id);
        List<cdnNumber> GetCdnNumbers(DateTime? date, string Ulocation);
        List<GrnEntity> GetCDNwastage(string cdnNumber);
        IEnumerable<WS_QtySumEntityModel> GetDCWastages(DateTime? fdate, DateTime? tdate, string DCCode);
        List<WastageEntity> SearchWastage(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType, string ULocation, string Url);
        List<WastageEntity> GetWSApprovalOR(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType, string ULocation, string Url);
        List<WastageEntity> GetWSApprovalAND(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType, string ULocation, string Url);
       
    }
}
