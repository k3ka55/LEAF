using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
namespace WebApi.Controllers.CI
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerIndentController : ApiController
    {      

        private readonly ICustomerIndentService _customerIndentServices;

        #region Public Constructor
        public CustomerIndentController()
        {
            _customerIndentServices = new CustomerIndentService();
        }

        [HttpPost]
        public HttpResponseMessage ExcelImport(fileImport fileDetail)
        {
            CIExcelImport ciData = _customerIndentServices.ExcelImportForCI(fileDetail);
            return Request.CreateResponse(ciData);
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {

            string message = "";
            List<CIEntity> rs = new List<CIEntity>();
            //if (id < 0)
            //{
                rs = _customerIndentServices.GetCustomerIndent(id);
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
            //}
            //else
            //{
            //    message = string.Format("Invalid ID");
            //    return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, message);
            //}
        }

        //[HttpGet]
        //public HttpResponseMessage SearchIndents(DateTime? fromDate, DateTime? endDate, string location)
        //{
        //    var list = _customerIndentServices.searchIndent(fromDate, endDate, location);

        //    return Request.CreateResponse(list);
        //}

        [HttpPost]
        public HttpResponseMessage CreateCustomerIndent(CIEntity ciEntity)
        {
            ResponseEntity rs = new ResponseEntity();
            if (ciEntity != null)
            {

                int response = _customerIndentServices.CreateCustomerIndent(ciEntity);
                if (response != null)
                {
                    if (response == -1)
                    {
                        rs.statusCode = HttpStatusCode.BadRequest;
                        rs.message = "Template Name Exist";
                    }
                    else
                    {
                        rs.TempId = response;
                        rs.statusCode = HttpStatusCode.Created;
                        rs.message = "Inserted Successfully";
                    }
                    //rs.TempId = response;
                    //rs.statusCode = HttpStatusCode.Created;
                    //rs.message = "Inserted Successfully";
                }
                else
                {
                    rs.statusCode = HttpStatusCode.InternalServerError;
                    rs.message = "Inserted UnSuccessfully";
                }

                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            else
            {
                rs.message = string.Format("Invalid ID");
                return Request.CreateResponse(HttpStatusCode.NotModified, rs);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateCustomerIndent([FromBody]CIEntity ciEntity)
        {

            ResponseEntity response = new ResponseEntity();
            int responseCode;
            if (ciEntity.Indent_ID > 0)
            {
                responseCode = _customerIndentServices.UpdateCustomerIndent(ciEntity.Indent_ID, ciEntity);
                if (responseCode != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.TempId = responseCode;
                    response.message = " Updated Successfully.";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Update Failed";
                }
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                response.message = "Invalid ID";
                response.statusCode = HttpStatusCode.NotModified;
                return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, response);
            }
        }

        //---------------------------------------------SEARCH--------------------------------
        [HttpGet]
        public HttpResponseMessage SearchCustomerIndent(int? roleId, int regionid, string location, string dccode, string Url)
        {
            var result = _customerIndentServices.searchIndent(roleId, regionid, location, dccode, Url);
            
            return Request.CreateResponse(result);
        }
        [HttpGet]
        public HttpResponseMessage SearchIndentFormapping(int regionid, string location, string dccode)
        {
            var result = _customerIndentServices.SearchIndentFormapping(regionid, location, dccode);

            return Request.CreateResponse(result);
        }

        [HttpGet]
        public HttpResponseMessage SearchCustomerIndentforCSI(int customerID)
        {
            var result = _customerIndentServices.getCIForCSI(customerID);

            return Request.CreateResponse(result);
        }
        
        [HttpGet]
        public HttpResponseMessage SearchCIforCSIEdit(int customerID)
        {
            var result = _customerIndentServices.SearchCIforCSIEdit(customerID);

            return Request.CreateResponse(result);
        }
               //-----------------------------------------DELETE--------------------------------------
        [HttpGet]
        public HttpResponseMessage DeleteCustomerIndent(string id, string deleteReason)
        {

            string message = "";
            bool responseID;
            if (id != null)
            {
                responseID = _customerIndentServices.DeleteCustomerIndent(id, deleteReason);
                if (responseID)
                {
                    message = string.Format("Deleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
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
        public HttpResponseMessage DeleteCustomerIndentLineItem(int Id)
        {

            string message = "";
            bool responseID;
            if (Id > 0)
            {
                responseID = _customerIndentServices.DeleteCustomerIndentLineItem(Id);
                if (responseID)
                {
                    message = string.Format("LineItem Deleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("LineItem Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid ID");
                return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, message);
            }
        }

        #endregion
    }
}
