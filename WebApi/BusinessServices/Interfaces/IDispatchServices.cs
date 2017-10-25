using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IDispatchServices
    {
        string send();
        bool CompareOTP(int Dispatch_Id, string OTP);
        List<DispatchEntity> Search(int? roleId, DateTime? startDate, DateTime? endDate, string dispatchType, string Route, string Url = "null");
        string UpdateDispatchAcceptedQty(int Dispatch_Id, DispatchEntity dispatchEntity);
        string SendSMS(string Customer_Number, int Dispatch_Id, string CDN_Number);
        bool UpdateCDNAcceptesSts(DispatchUpdateEntity DispatchUpdateEntity);
        DispatchResponseEntity DispatchCreation(DispatchEntity dispatchEntity);
        //IEnumerable<DispatchEntity> GetUnapprovalDispatchList();
        List<cdnNumber> GetCdnNumbers(DateTime? date, string Ulocation);
        string DispatchAccept(DispatchAcceptedEntity requestType);
       // bool ApproveDispatch(wasteageApproval approval);
        string UpdateDispatch(int Dispatch_Id, DispatchEntity dispatchEntity);
        List<DispatchEntity> SingleDispatchList(int id);
        List<DispatchEntity> DispatchSTN(string stnNumber);
        //List<DispatchEntity> GetDSSTOR(DateTime? startDate, DateTime? endDate, string dispatchType, string status, string ULocation);
        List<DispatchEntity> GetCustomerDispatchList(string cdnNumber);
        //List<DispatchEntity> GetDSAND(DateTime? startDate, DateTime? endDate, string dispatchType,string status, string ULocation);
        //List<DispatchEntity> GetDSOR(DateTime? startDate, DateTime? endDate, string dispatchType,string status, string ULocation);
        IEnumerable<InvoiceEntity> GetUnapprovalInvoiceList(string ULocation);
        //List<InvoiceEntity> GetIV(DateTime? startDate, DateTime? endDate, string ULocation);
        List<InvoiceEntity> SearchInvoiceList(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation, string Url);
        List<InvoiceEntity> GetIVNA(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation, string Url);
        List<DispatchNumber> GetDispatchNumbers(DateTime? date, string ULocation);
        stockAvail CheckStockAvalibility(string SKUName, double Qty, string SKUType, string grade, string Dc_Code);
        bool ApproveInvoice(approvalInvoiceList approval);
        InvoiceResponseEntity CreateInvoice(InvoiceEntity invoiceEntity);
        bool DispatchUpdateById(DispatchUpdateEntity DispatchUpdateEntity);
        List<InvoiceEntity> GetSingleInvoiceList(int id);
        bool DeleteDispatch(int dipId);

        int UpdateInvoice(int invoiceId, InvoiceEntity invoiceEntity);
        List<DispatchEntity> SearchDispatchList(int? roleId, DateTime? startDate, DateTime? endDate, string dispatchType, string status, string ULocation, string Url);

    }
}
