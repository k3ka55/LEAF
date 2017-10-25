using BusinessEntities;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IPurchaseOrderServices
    {
        string CreatePurchaseOrder(PurchaseOrderEntity POEntity);
        List<PurchaseOrderEntity> GetPurchaseLineItemforGRN(string id);
        List<PurchaseOrderEntity> GetPurchaseLineItemforPO(string id);
        List<Tuple<string>> getStatuses();
        string UpdatePurchaseOrder(int poId, PurchaseOrderEntity POEntity);
        bool DeletePurchaseOrder(int poId, string deleteReason);
        bool poApproval(PurchaseOrderEntity poEntity);   
        bool DeletePurchaseOrderLineItem(int Id);
        bool UpdateLineItemPORate(int Id, string poNumber, double? ARate, double? BRate, double? CRate);
        bool poBulkApproval(bulkApprovalEntity bulkEntity);
        List<PurchaseOrderEntity> GetPOApprovalOR(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string ULocation, string Url);
        List<PurchaseOrderEntity> GetPOApprovalAND(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string ULocation, string Url);
        //List<PurchaseOrderEntity> GetPOALL(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
        //List<PurchaseOrderEntity> GetPOSUDT(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
       // List<PurchaseOrderEntity> GetPOSTSU(DateTime? date, string supplierName, string status, string Ulocation);
        //List<PurchaseOrderEntity> GetPODAST(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
        //List<PurchaseOrderEntity> GetPOAnyOne(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
        List<POWithLineItemEntity> GetPOforEditALL(int? roleId, string ULocation, string Url, string poNumber);
        List<POWithLineItemEntity> GetPOforEditOR(int? roleId, string ULocation, string Url, string poNumber);
        List<PurchaseOrderEntity> GetPOApprovalList(string ULocation);
        List<PurchaseOrderEntity> SearchPO(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string status, string ULocation, string Url);
    }
}
