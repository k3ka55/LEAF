using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IStockTransferIntentServices
    {
        string CreateStockTransfer(StockTransferIntentEntity stockEntity);
        List<StockTransferIntentEntity> GetStockLineItem(string id);

        List<Tuple<string>> getStatuses();
        string UpdateStockTransfer(int stiId, StockTransferIntentEntity stockEntity);
        bool DeleteStockTransfer(int stiId);
        bool stApproval(StockTransferIntentEntity stEntity);
        //List<StockTransferIntentEntity> GetSTOR(DateTime? startDate, DateTime? endDate, string status, string ProcessedBy, string ULocation);
        //List<StockTransferIntentEntity> GetSTORSTPB(DateTime? startDate, DateTime? endDate, string status, string ProcessedBy, string ULocation);
       // List<StockTransferIntentEntity> GetSTOR(DateTime? startDate, DateTime? endDate, string status, string ProcessedBy, string ULocation);
        //List<StockTransferIntentEntity> GetSTAND(DateTime? startDate, DateTime? endDate, string status, string ProcessedBy, string ULocation);
        //List<StockTransferIntentEntity> GetSTOR( DateTime? startDate, DateTime? endDate, string status, string ULocation);
        List<StockTransferIntentEntity> GetSTAAND(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url);
        List<StockTransferIntentEntity> GetSTAOR(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ULocation, string Url);
        //List<StockTransferIntentEntity> GetSTMORSTPB(DateTime? startDate, DateTime? endDate, string status, string FLocation, string ULocation);
        //List<StockTransferIntentEntity> GetSTMAND(DateTime? startDate, DateTime? endDate, string status, string FLocation, string ULocation);
        //List<StockTransferIntentEntity> GetSTMOR(DateTime? startDate, DateTime? endDate, string status, string FLocation, string ULocation);
        List<StockTransferIntentEntity> GetSTApprovalList(string ULocation);
        bool DeleteSTIOrderLineItem(int Id);
        bool STIUpdateById(STIUpdateEntity stiEntity);
        bool stiBulkApproval(STIbulkApprovalEntity bulkEntity);
        List<StockTransferIntentEntity> GetStockLineItemForModel(string id);
        List<StockTransferIntentEntity> SearchStockTransfer(int? roleId, DateTime? startDate, DateTime? endDate, string status, string ProcessedBy, string ULocation, string Url);
        List<STIExcelFields> ExcelImportForCI(fileImportSTI fileDetail);
    }
}
