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
    public class GRNController : ApiController
    {
        private readonly IGrnServices _grnServices;

        #region Public Constructor
        public GRNController()
        {
            _grnServices = new GrnServices();
        }
        //------------------------------------GET---------------------------------------------
        [HttpGet]
        public List<GrnEntity> GetGRN(string Ulocation)
        {
            return _grnServices.GetGRN(Ulocation);
        }
       
        [HttpGet]
        public List<GrnEntity> Get(string id)
        {
            return _grnServices.GetGrnLineItem(id);
        }
        
        [HttpGet]
        public HttpResponseMessage GetStiNumbers(DateTime? date, string Ulocation)
        {
            List<stiNumber> f = new List<stiNumber>();

            if (date != null)
            {
                var result = _grnServices.GetStiNumbers(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStiNumbersforSTN(DateTime? date, string Ulocation)
        {
            List<stiNumber> f = new List<stiNumber>();

            if (date != null)
            {
                var result = _grnServices.GetStiNumbersforSTN(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStnNumbers(DateTime? date, string Ulocation)
        {
            List<stnNumber> f = new List<stnNumber>();

            if (date != null)
            {
                var result = _grnServices.GetStnNumbers(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        
        [HttpGet]
        public HttpResponseMessage GetPoNumbers(DateTime? date, string Ulocation)
        {
            List<poNumber> f = new List<poNumber>();

            if (date != null)
            {
                var result = _grnServices.GetPoNumbers(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetCdnNumbersforGRN(string Ulocation, string CustomerCode)
        //
        {
            List<cdnNumber> f = new List<cdnNumber>();

            if (Ulocation != null)
            {
                var result = _grnServices.GetCdnNumbersforGRN(Ulocation, CustomerCode);
         //      
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        //------------------------------------------POST----------------------------------------       
        [HttpPost]
        public ResponseEntity CreateGRN(GrnEntity grnEntity)
        {
            ResponseEntity response = new ResponseEntity();
            string responseCode = _grnServices.CreateGrn(grnEntity);
            if (responseCode != null)
            {
                response.grn_Number = responseCode;
                response.statusCode = HttpStatusCode.OK;
                response.message = "Inserted Successfully";
            }
            else
            {
                response.statusCode = HttpStatusCode.InternalServerError;
                response.message = "Inserted UnSuccessfully";
            }

            return response;
        }
      
        [HttpPost]
        public ResponseEntity UpdateGRN([FromBody]GrnEntity grnEntity)
        {
            ResponseEntity response = new ResponseEntity();
            string responseCode;

            if (grnEntity.INW_Id > 0)
            {
                responseCode = _grnServices.UpdateGRN(grnEntity.INW_Id, grnEntity);
                if (responseCode != null)
                {

                    response.statusCode = HttpStatusCode.OK;
                    response.grn_Number = responseCode;
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
        //--------------------------------------SEARCH-----------------------------------------
        //[HttpGet]
        //public HttpResponseMessage SearchGRN(DateTime? startDate, DateTime? endDate, string supplierName = "null", string Ulocation = "null")
        //{
        //    List<GrnEntity> f = new List<GrnEntity>();

        //    if ((startDate != null && endDate != null) && supplierName != "null" && Ulocation != "null")
        //    {
        //        var result = _grnServices.GetGRNAND(startDate, endDate, supplierName, Ulocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else if (((startDate != null && endDate != null) || supplierName != "null") && Ulocation != "null")
        //    {
        //        var result = _grnServices.GetGRNOR(startDate, endDate, supplierName, Ulocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}
       
        
        [HttpGet]
        public HttpResponseMessage CustomerReturn(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName = "null", string Ulocation = "null", string Url = "null")
        {
            List<GrnEntity> f = new List<GrnEntity>();

            if ((startDate != null && endDate != null) && Ulocation != "null" && (roleId != null && Url != null))
            {
                var result = _grnServices.SearchGRNCR(roleId, startDate, endDate, supplierName, Ulocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
            [HttpGet]
        public HttpResponseMessage SearchGRN(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName = "null", string Ulocation = "null", string Url = "null")
        {
            List<GrnEntity> f = new List<GrnEntity>();

            if ((startDate != null && endDate != null) && Ulocation != "null" && (roleId != null && Url != null))
            {
                var result = _grnServices.SearchGRN(roleId, startDate, endDate, supplierName, Ulocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        //---------------------------------------DELETE---------------------------------------
        [HttpPost]
        public HttpResponseMessage DeleteGRN(int id)
        {

            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _grnServices.DeleteGRN(id);
                if (responseID)
                {
                    message = string.Format("Deleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("Delete Failed");
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
        public HttpResponseMessage DeleteGRNLineItem(int Id)
        {

            string message = "";
            bool responseID;
            if (Id > 0)
            {
                responseID = _grnServices.DeleteGRNOrderLineItem(Id);
                if (responseID)
                {
                    message = string.Format("POLineItem deleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("POLineItem Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid ID");
                return Request.CreateResponse(HttpStatusCode.NotModified, message);
            }
        }
      
        //--------------------------------POStatusUpdate(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity POStatusUpdate(POBULKAPPROVAL grnEntity)
        {
            ResponseEntity response = new ResponseEntity();


            if (grnEntity.id != "" || grnEntity.id != null)
            {
                bool responseID = _grnServices.POUpdateById(grnEntity);
                if (responseID)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = "Success";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Not Success";
                }
            }
            else
            {
                response.statusCode = HttpStatusCode.NotFound;
                response.message = "Invalid Number";
            }
            return response;
        }



        #endregion
    }
}
