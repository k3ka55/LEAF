using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers.Mobile
{
    public class MobileLoginController : ApiController
    {
        private readonly IMobileLoginServices _loginServices;

        #region Public Constructor
       
        public MobileLoginController()
        {
            _loginServices = new MobileLoginServices();
        }
        //--------------------------------login(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreateSaleIndent(SaleIndentEntity saleEntity)
        {
            ResponseEntity rs = new ResponseEntity();
            string response = _loginServices.CreateSaleIndent(saleEntity);
            if (response != null)
            {
                rs.csi_Number = response;
                rs.statusCode = HttpStatusCode.Created;
                rs.message = "Inserted Successfully";
            }
            else
            {
                rs.statusCode = HttpStatusCode.NotImplemented;
                rs.message = "Inserted UnSuccessfully";
            }

            return rs;
        }
        [HttpGet]
        public HttpResponseMessage GetCustSuppDD()
        {
            var user = _loginServices.GetCustSuppDD();
            MobileCustSuppDDEntity f = new MobileCustSuppDDEntity();
            if (user != null)
            {
                f = user;
                f.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                f.StatusCode = HttpStatusCode.NotFound;
            }
            return Request.CreateResponse(f);
        }
        [HttpPost]
        public HttpResponseMessage GetCustomersForFilter([FromBody]FilterClass Filter)
        {
            List<ReturnCustomers> list = new List<ReturnCustomers>();
            list = _loginServices.searchCustomers(Filter);
            return Request.CreateResponse(list);
        }
        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserDetailsEntity userEntity)
        {
            var userlog = _loginServices.login(userEntity);
            return Request.CreateResponse(HttpStatusCode.OK, userlog);
        }
        [HttpGet]
        public HttpResponseMessage SearchCustomerIndentforCSI(int customerID)
        {
            var result = _loginServices.getCIForCSI(customerID);

            return Request.CreateResponse(result);
        }
        [HttpPost]
        public HttpResponseMessage GetLoginUserLocations([FromBody]UserDetailsEntity userEntity)
        {
            var userlog = _loginServices.GetLoginUserLocations(userEntity);
            return Request.CreateResponse(HttpStatusCode.OK, userlog);
        }
        [HttpGet]
        public HttpResponseMessage SearchTemplates(int? roleId, int regionid, string location, string dccode, string Url)
        {
            var list = _loginServices.searchTemplate(roleId, regionid, location, dccode, Url);

            return Request.CreateResponse(list);
        }
        [HttpGet]
        public HttpResponseMessage GetRateTemplate(string id)
        {

            string message = "";
            List<MobileRateIndentEntity> rs = new List<MobileRateIndentEntity>();
            if (id != null)
            {
                rs = _loginServices.GetRateIndent(id);
                if (rs != null)
                {
                    message = string.Format("Data got Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, rs);
                }
                else
                {
                    message = string.Format("Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid ID");
                return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, message);
            }
        }
        [HttpGet]
        public HttpResponseMessage SearchCSI(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation, string status = "null", string SalesPersonName = "null", string Url = "null")
        {
            List<SaleIndentEntity> f = new List<SaleIndentEntity>();

            if (startDate != null && endDate != null && (roleId != null && Url != "null" && SalesPersonName!="null"))
            {
                var result = _loginServices.SearchSA(roleId, startDate, endDate, status, ULocation,SalesPersonName, Url);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public List<DCMasterModel> GetDC()
        {
            return _loginServices.GetDC();
        }

        [HttpGet]
        public List<MobileSingleDispatchEntity> GetDispatchById(int id)
        {
            return _loginServices.SingleDispatchList(id);
        }


        [HttpGet]
        public int ConnectivityCheck()
        {
            return _loginServices.ConnectivityCheck();
        }

        [HttpGet]
        public string ReturnString(string s)
        {
            return _loginServices.ReturnString(s);
        }
        //

        [HttpGet]
        public HttpResponseMessage GetInvoiceStatement(DateTime? startDate, DateTime? endDate, string CustomerCode = "null")
        {
            MobileInvoiceStatementResponseEntity f = new MobileInvoiceStatementResponseEntity();

            if ((startDate != null && endDate != null && CustomerCode != "null"))
            {
                var result = _loginServices.GetInvoiceStatement(startDate, endDate, CustomerCode);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        
        [HttpGet]
        public HttpResponseMessage SearchInvoiceList(DateTime? startDate, DateTime? endDate, string CustomerCode = "null")
        {
            List<InvoiceEntity> f = new List<InvoiceEntity>();

            if ((startDate != null && endDate != null && CustomerCode != "null"))
            {
                var result = _loginServices.SearchInvoiceList(startDate, endDate, CustomerCode);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage SearchDispatchList(DateTime? startDate, DateTime? endDate, string CustomerCode = "null")
        {
            List<MobileDispatchEntity> f = new List<MobileDispatchEntity>();

            if ((startDate != null && endDate != null && CustomerCode != "null"))
            {
                var result = _loginServices.SearchDispatchList(startDate, endDate, CustomerCode);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
//
        //    [HttpGet]
        // public DCMasterEntity GetDC(int Dc_Id)
        //{

        //    return _loginServices.GetDC(Dc_Id);
        //}

    }
}
        #endregion
