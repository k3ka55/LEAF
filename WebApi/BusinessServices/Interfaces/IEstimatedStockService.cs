using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessServices
{
    public interface IEstimatedStockService
    {
        bool CreateEstimatedStock(EstimtedStkEntity estimatedEntity);
        List<EstimatedStockEntity> CheckMaterialMaster(EstimtedStkEntity material);
        List<EstimatedStockEntity> ExcelImportESTK(fileImportEST fileDetail);
        List<EstimatedStockEntitySearch> GetEstimatedStock(int? roleId, DateTime? date, string ULocation,string SKU_Type, string Url);
        bool DeleteEstimatedStock(int phyStockId, string deletedby);
        //bool DeletePhysicalStock(int phyStockId);
        //int UpdateStockFromPhy(StockFromPhysicalStockEntity StinkageLineItems);
        //List<PurchaseOrderEntity> GetPurchaseLineItemforGRN(string id);
        //List<PurchaseOrderEntity> GetPurchaseLineItemforPO(string id);
        //List<Tuple<string>> getStatuses();
        //string UpdatePurchaseOrder(int poId, PurchaseOrderEntity POEntity);

        //bool poApproval(PurchaseOrderEntity poEntity);
        //bool DeletePurchaseOrderLineItem(int Id);
        //bool UpdateLineItemPORate(int Id, string poNumber, double? ARate, double? BRate, double? CRate);
        //bool poBulkApproval(bulkApprovalEntity bulkEntity);
        //List<PurchaseOrderEntity> GetPOApprovalOR(DateTime? startDate, DateTime? endDate, string supplierName, string ULocation);
        //List<PurchaseOrderEntity> GetPOApprovalAND(DateTime? startDate, DateTime? endDate, string supplierName, string ULocation);
        
        //List<PurchaseOrderEntity> GetPOSUDT(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
        //// List<PurchaseOrderEntity> GetPOSTSU(DateTime? date, string supplierName, string status, string Ulocation);
        //List<PurchaseOrderEntity> GetPODAST(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
        //List<PurchaseOrderEntity> GetPOAnyOne(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
        //List<POWithLineItemEntity> GetPOforEditALL(int? roleId, string ULocation, string Url, string poNumber);
        //List<POWithLineItemEntity> GetPOforEditOR(int? roleId, string ULocation, string Url, string poNumber);
        //List<PurchaseOrderEntity> GetPOApprovalList(string ULocation);
    }
}
