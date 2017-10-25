using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        private readonly IReportServices _reportServices;
        #region Public Constructor
        public ReportController()
        {
            _reportServices = new ReportServices();
        }


        //================================================GRN REPORT=============================================================
        [HttpGet]
        public IEnumerable<GRNDDEntity> GRNreportDD()
        {
            return _reportServices.GRNreportDD();
        }
        [HttpGet]
        public IEnumerable<CustFillRateEntity> CustomerFillRatereportDD()
        {
            return _reportServices.CustomerFillRatereportDD();
        }


        [HttpGet]
        public HttpResponseMessage GRNReport(DateTime? premonth, DateTime? month, DateTime? day, string Ulocation = "null")
        {
            List<GRNReportEntity> f = new List<GRNReportEntity>();

            if ((premonth != null || month != null || day != null) && Ulocation != "null")
            {
                var result = _reportServices.GRNReport(premonth, month, day, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        //[HttpGet]
        //public HttpResponseMessage ShrinkageReport(string Ulocation = "null")
        //{
        //    List<ShrinkageReportEntity> f = new List<ShrinkageReportEntity>();

        //    if (Ulocation != "null")
        //    {
        //        var result = _reportServices.ShrinkageReport(Ulocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}
        //
         [HttpGet]
        public HttpResponseMessage ShrinkageReport(string Ulocation = "null")
        {
            List<ShrinkageReportEntity> f = new List<ShrinkageReportEntity>();

            if (Ulocation != "null")
            {
                var result = _reportServices.ShrinkageSKUwiseReport(Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
       

        //=======================================================================================================================
        //=================================================STOCK summary REPORT=================================================
        [HttpGet]
        public HttpResponseMessage StockSummary(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<StockSummaryReportEntity> f = new List<StockSummaryReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.GetStockSummaryReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================WASTAGE REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage WastageReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<WastageReportEntity> f = new List<WastageReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.WastageReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================PURCHASE REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage PurchaseReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<PurchaseReportEntity> f = new List<PurchaseReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.PurchaseReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================STIANDSTN REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage STIReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<STIDISPATCHReportEntity> f = new List<STIDISPATCHReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.STIReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================DISPATCHQTY REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage DispatchQtyReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<DispatchQTYReportEntity> f = new List<DispatchQTYReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.DispatchQtyReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================DISPATCHPACKTYPEBASED REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage DispatchPackTypeBasedReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<DispatchPackTypeBasedReportEntity> f = new List<DispatchPackTypeBasedReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.DispatchPackTypeBasedReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================DISPATCHPACKTYPEBASED REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage CSIBasedReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<CSIReportEntity> f = new List<CSIReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.CSIBasedReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        //==================================================DISPATCHPACKTYPEBASED REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage GetCustomerRateFillReport(DateTime fromdate, DateTime todate, string Ulocation = "null")
        {
            List<CustomerRateFillReportEntity> f = new List<CustomerRateFillReportEntity>();

            if (fromdate != null && todate != null && Ulocation != "null")
            {
                var result = _reportServices.GetCustomerRateFillReport(fromdate, todate, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        
          //==================================================DISPATCHPACKTYPEBASED REPORT=====================================================

        [HttpGet]
        public HttpResponseMessage GetCustomerRateFillReportMonthBased(string Type = "null", string dccode = "null")
        {
            List<CustomerRateFillMonthBasedReportEntity> f = new List<CustomerRateFillMonthBasedReportEntity>();

            if (dccode != "null" && Type != "null")
            {
                var result = _reportServices.GetCustomerRateFillReportMonthBased(Type, dccode);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        //=======================================================================================================================
        
        

        #endregion
    }
}
