using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WastageController : ApiController
    {
        private readonly IWastageService _wastageServices;

        #region Public Constructor
        public WastageController()
        {
            _wastageServices = new WastageService();
        }

        //--------------------------------GET-----------------------------------------------

        [HttpGet]
        public HttpResponseMessage SetWeight(double weight)
        {
            WeightClass.Weight = weight;

            return Request.CreateResponse("Success");
        }

        [HttpGet]
        public HttpResponseMessage getWeight()
        {
            return Request.CreateResponse("Weight : " + WeightClass.Weight);
        }

        //---------------------------------GET-----------------------------------------------
        [HttpGet]
        public List<WastageEntity> Get(string id)
        {
            return _wastageServices.GetWastageItem(id);
        }
        //[HttpGet]
        //public IEnumerable<WS_QtySumEntityModel> GetDCWastages(DateTime? date, string DCCode)
        //{
        //    return _wastageServices.GetDCWastages(date, DCCode);
        //}
        [HttpGet]
        public IEnumerable<WS_QtySumEntityModel> GetDCWastages(DateTime? fromDate, DateTime? toDate, string DCCode)
        {
            return _wastageServices.GetDCWastages(fromDate, toDate, DCCode);
        }
         
        
        [HttpGet]
        public List<GrnEntity> GetCDNWastage(string cdnNumber)
        {
            return _wastageServices.GetCDNwastage(cdnNumber);
        }
       
        //---------------------------------------POST-----------------------------------------
        [HttpPost]
        public ResponseEntity CreateWastage(WastageEntity wsEntity)
        {
            ResponseEntity rs = new ResponseEntity();
            string response = _wastageServices.CreateWastage(wsEntity);
            if (response != null)
            {
                rs.ws_Number = response;
                rs.statusCode = HttpStatusCode.Created;
                rs.message = "Inserted Successfully";
            }
            else
            {
                rs.statusCode = HttpStatusCode.InternalServerError;
                rs.message = "Inserted UnSuccessfully";
            }

            return rs;
        }

        [HttpGet]
        public HttpResponseMessage GetCdnNumbers(DateTime? date, string Ulocation)
        {
            List<cdnNumber> f = new List<cdnNumber>();

            if (date != null)
            {
                var result = _wastageServices.GetCdnNumbers(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpPost]
        public ResponseEntity UpdateWastage([FromBody]WastageEntity wsEntity)
        {
            ResponseEntity response = new ResponseEntity();
            string responseCode;
            if (wsEntity.Wastage_Id > 0)
            {
                responseCode = _wastageServices.UpdateWastage(wsEntity.Wastage_Id, wsEntity);
                if (responseCode != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.ws_Number = responseCode;
                    response.message = " Updated Successfully.";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Update Failed";
                }
            }
            else
            {
                response.message = "Invalid ID";
                response.statusCode = HttpStatusCode.NotModified;
            }
            return response;
        }
        //---------------------------------------------SEARCH--------------------------------
        //[HttpGet]
        //public HttpResponseMessage SearchWastage(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType = "null", string ULocation = "null", string Url = "null")
        //{
        //    List<WastageEntity> f = new List<WastageEntity>();

        //    if ((startDate != null && endDate != null) && wastageType != "null" && (ULocation != "null" && roleId != null && Url != null))
        //    {
        //        var result = _wastageServices.GetWSALL(roleId, startDate, endDate, wastageType, ULocation, Url);
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
        //    else if ((startDate != null && endDate != null) && wastageType == "null" && (ULocation != "null" && roleId != null && Url != null))
        //    {
        //        var result = _wastageServices.GetWSOR(roleId, startDate, endDate, wastageType, ULocation, Url);
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage SearchWastage(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType = "null", string ULocation = "null", string Url = "null")
        {
            List<WastageEntity> f = new List<WastageEntity>();

            if ((startDate != null && endDate != null) && (ULocation != "null" && roleId != null && Url != null))
            {
                var result = _wastageServices.SearchWastage(roleId, startDate, endDate, wastageType, ULocation, Url);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage SearchWSApprovalList(int? roleId, DateTime? startDate, DateTime? endDate, string wastageType = "null", string ULocation = "null", string Url = "null")
        {
            List<WastageEntity> f = new List<WastageEntity>();

            if ((startDate != null && endDate != null) && wastageType != "null" && (ULocation != "null" && roleId != null && Url != null))
            {
                var result = _wastageServices.GetWSApprovalAND(roleId, startDate, endDate, wastageType, ULocation, Url);
                f = result;
                return Request.CreateResponse(f);
            }
            else if (((startDate != null && endDate != null) || wastageType != "null") && (ULocation != "null" && roleId != null && Url != null))
            {
                var result = _wastageServices.GetWSApprovalOR(roleId, startDate, endDate, wastageType, ULocation, Url);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
     
        //-----------------------------------------DELETE--------------------------------------
        [HttpGet]
        public HttpResponseMessage DeleteWastage(int wsId)
        {

            string message = "";
            bool responseID;
            if (wsId > 0)
            {
                responseID = _wastageServices.DeleteWastage(wsId);
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
                return Request.CreateResponse(HttpStatusCode.NotModified, message);
            }
        }
      
        [HttpGet]
        public HttpResponseMessage DeleteWastageLineItem(int Id)
        {

            string message = "";
            bool responseID;
            if (Id > 0)
            {
                responseID = _wastageServices.DeleteWastageLineItem(Id);
                if (responseID)
                {
                    message = string.Format("WSLineItem eleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("WSLineItem Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid ID");
                return Request.CreateResponse(HttpStatusCode.NotModified, message);
            }
        }

     
      
        //---------------------------------POBULKAPPROVAL-------------------------------------
        [HttpPost]
        public ResponseEntity wsBulkApproval(bulkWastApprovalEntity bulkwasEntity)
        {
            ResponseEntity response = new ResponseEntity();

            bool responseCode = _wastageServices.wsBulkApproval(bulkwasEntity);
                if (responseCode)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = "Success";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Not Success";
                }
            return response;
        }
      #endregion
    }
}
