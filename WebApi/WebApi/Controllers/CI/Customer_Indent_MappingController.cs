using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using BusinessServices.Interfaces;
using BusinessServices.Services.CI;
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
    public class Customer_Indent_MappingController : ApiController
    {
        private readonly ICustomer_Indent_MappingServices _mappServices;

        #region Public Constructor


           public Customer_Indent_MappingController()
        {
            _mappServices = new Customer_Indent_MappingServices();
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {

            string message = "";
            Customer_Indent_MappingEntity rs = new Customer_Indent_MappingEntity();
            if (id != null)
            {
                rs = _mappServices.getSingleIndentMapping(id);
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

        [HttpPost]
        public HttpResponseMessage CreateCustomerIndentMapping(Customer_Indent_MappingEntity mapp)
        {
            ResponseEntity rs = new ResponseEntity();
            if (mapp != null)
            {

                bool response = _mappServices.CreateCustomerIndentMapping(mapp);
                if (response)
                {
                   // rs.TempId = response;
                    rs.statusCode = HttpStatusCode.Created;
                    rs.message = "Inserted Successfully";
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

        //[HttpGet]
        //public HttpResponseMessage SearchIdentMapping(int? roleId, int regionid, int locationid, int dcid, string Url)
        //{
        //    var list = _mappServices.searchMappingIndent(roleId,regionid, locationid, dcid, Url);

        //    return Request.CreateResponse(list);
        //}

        [HttpGet]
        public HttpResponseMessage searchCustomerIndentMapping(int? roleId, string region, string location, string dccode, string Url)
        {
            var list = _mappServices.searchCustomerIndentMapping(roleId, region, location, dccode, Url);

            return Request.CreateResponse(list);
        }

        [HttpPost]
        public HttpResponseMessage UpdateCustomerIndentMapping([FromBody]Customer_Indent_MappingEntity mapp)
        {

            ResponseEntity response = new ResponseEntity();
            bool responseCode;
            if (mapp.CIT_Mapping_ID > 0)
            {
                responseCode = _mappServices.UpdateCustomerIndentMapping(mapp.Indent_ID, mapp);
                if (responseCode)
                {
                    response.statusCode = HttpStatusCode.OK;
                   // response.MappId = responseCode;
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

        //[HttpGet]
        //public HttpResponseMessage getMappingIndents()
        //{
        //    var result = _mappServices.getIndentMapping();

        //    return Request.CreateResponse(result);
        //}

        //[HttpGet]
        //public HttpResponseMessage getCustomerIndents(int customer_Id)
        //{
        //    var result = _mappServices.getIndents(customer_Id);

        //    return Request.CreateResponse(result);
        //}

        [HttpGet]
        public HttpResponseMessage DeleteCustomerIndentMapping(int id)
        {

            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _mappServices.deleteMapping(id);
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

        #endregion
    }
}
