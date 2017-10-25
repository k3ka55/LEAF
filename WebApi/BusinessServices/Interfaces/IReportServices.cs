using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IReportServices
    {
     //    List<ShrinkageReportEntity> ShrinkageReport(string Ulocation = "null");
        List<GRNReportEntity> GRNReport(DateTime? premonth, DateTime? month, DateTime? day, string Ulocation = "null");
        IEnumerable<GRNDDEntity> GRNreportDD();
        List<ShrinkageReportEntity> ShrinkageSKUwiseReport(string Ulocation = "null");
        IEnumerable<CustFillRateEntity> CustomerFillRatereportDD();
        List<StockSummaryReportEntity> GetStockSummaryReport(DateTime fromdate, DateTime todate, string dccode);
        List<WastageReportEntity> WastageReport(DateTime fromdate, DateTime todate, string dccode);
        List<PurchaseReportEntity> PurchaseReport(DateTime fromdate, DateTime todate, string dccode);
        List<STIDISPATCHReportEntity> STIReport(DateTime fromdate, DateTime todate, string dccode);
        List<DispatchQTYReportEntity> DispatchQtyReport(DateTime fromdate, DateTime todate, string dccode);
        List<DispatchPackTypeBasedReportEntity> DispatchPackTypeBasedReport(DateTime fromdate, DateTime todate, string dccode);
        List<CSIReportEntity> CSIBasedReport(DateTime fromdate, DateTime todate, string dccode);
        List<CustomerRateFillReportEntity> GetCustomerRateFillReport(DateTime fromdate, DateTime todate, string dccode);
        List<CustomerRateFillMonthBasedReportEntity> GetCustomerRateFillReportMonthBased(string Type, string dccode);
    }
}
