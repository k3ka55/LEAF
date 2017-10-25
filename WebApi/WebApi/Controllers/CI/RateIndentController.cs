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

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RateIndentController : ApiController
    {

        private readonly IRateIndentService _rateIndentServices;

        #region Public Constructor
        public RateIndentController()
        {
            _rateIndentServices = new RateIndentService();
        }

        [HttpPost]
        public HttpResponseMessage ExcelImport(rifileImport fileDetail)
        {
            RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
            return Request.CreateResponse(ciData);
        }

        [HttpPost]
        public HttpResponseMessage RateForCsi(RateInformation rateDetail)
        {
            var priceList = _rateIndentServices.GetRateForCsi(rateDetail);
            return Request.CreateResponse(priceList);
        }

        [HttpPost]
        public HttpResponseMessage CheckUnion(RateIndentEntityUnique rate)
        {
            var list = _rateIndentServices.CheckUnion(rate);

            return Request.CreateResponse(list);
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {

            string message = "";
            List<RateIndentEntity> rs = new List<RateIndentEntity>();
            if (id != null)
            {
                rs = _rateIndentServices.GetRateIndent(id);
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
        public HttpResponseMessage CreateRateIndent(RateIndentEntity rateIndent)
        {
            ResponseEntity rs = new ResponseEntity();
            if (rateIndent != null)
            {

                int response = _rateIndentServices.CreateRateIndent(rateIndent);

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
        public HttpResponseMessage UpdateRateIndent([FromBody]RateIndentEntity rateIndent)
        {

            ResponseEntity response = new ResponseEntity();
            int responseCode;
            if (rateIndent.Template_ID > 0)
            {
                responseCode = _rateIndentServices.UpdateRateIndent(rateIndent.Template_ID, rateIndent);
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
        public HttpResponseMessage getSKUS(int templateID)
        {
            var list = _rateIndentServices.getSKUS(templateID);

            return Request.CreateResponse(list);
        }

        [HttpGet]
        public HttpResponseMessage SearchRateIndent(string ULocation = "null", string UDCCode = "null", string Region = "null")
        {
            List<RateTempateResponse> f = new List<RateTempateResponse>();

            var result = _rateIndentServices.searchRateTemplate(ULocation, UDCCode, Region);
            f = result;
            return Request.CreateResponse(f);
        }

        [HttpGet]
        public HttpResponseMessage SearchRateIndentForEdit(string ULocation = "null", string UDCCode = "null", string Region = "null")
        {
            List<RateTempateResponse> f = new List<RateTempateResponse>();

            var result = _rateIndentServices.searchRateTemplateforEdit(ULocation, UDCCode, Region);
            f = result;
            return Request.CreateResponse(f);
        }


        [HttpGet]
        public HttpResponseMessage getPriceForSKU(string SKU_Type, string Location, string Grade, string SKU_Name, int tempID)
        {
            var list = _rateIndentServices.getPrice(SKU_Type, Location, Grade, SKU_Name, tempID);

            return Request.CreateResponse(list);
        }

        [HttpGet]
        public HttpResponseMessage SearchTemplates(int? roleId, int regionid, string location, string dccode, string Url)
        {
            var list = _rateIndentServices.searchTemplate(roleId, regionid, location, dccode, Url);

            return Request.CreateResponse(list);
        }



        [HttpGet]
        public HttpResponseMessage getrateTemplates(int CustomerID, string region, string location, string dccode, string skutype)
        {
            var list = _rateIndentServices.GetrateTemplates(CustomerID, region, location, dccode, skutype);
            return Request.CreateResponse(list);
        }

        //-----------------------------------------DELETE--------------------------------------
        [HttpGet]
        public HttpResponseMessage DeleteRateIndent(string id, string deleteReason)
        {

            string message = "";
            bool responseID;
            if (id != null)
            {
                responseID = _rateIndentServices.DeleteRateIndent(id, deleteReason);
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
        public HttpResponseMessage DeleteRateIndentLineItem(int Id)
        {

            string message = "";
            bool responseID;
            if (Id > 0)
            {
                responseID = _rateIndentServices.DeleteRateIndentLineItem(Id);
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
