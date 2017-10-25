using BusinessEntities;
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
    public class SaleIndentController : ApiController
    {
     private readonly ISaleIndentService _saleServices;

        #region Public Constructor
     public SaleIndentController()
        {
            _saleServices = new SaleIndentService();
        }
        [HttpGet]
         public IEnumerable<int> Ex()
        {
          return _saleServices.Ex();
         }
        [HttpPost]
        public HttpResponseMessage ExcelImport(fileImportBulkCSI fileDetail)
        {
            //RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
            return Request.CreateResponse(_saleServices.BulkCSI(fileDetail));
        }
  //
        [HttpGet]
        public string Fx(string str)
        {
            return _saleServices.Fx(str);
        }
        [HttpGet]
        public string Ax()
        {
            return _saleServices.Ax();
        }
        //--------------------------------Get(GET)-----------------------------------------------
        [HttpGet]
        public List<SaleIndentEntity> Get(string id)
        {
            return _saleServices.GetSaleLineItem(id);
        }
        
             [HttpGet]
        public List<csiCreators> GetCsiCreators(DateTime? date, string Ulocation)
        {
            return _saleServices.GetCsiCreators(date, Ulocation);
        }
        [HttpGet]
        public List<SaleIndentEntity> GetSALApprovalList(string Ulocation)
        {
            return _saleServices.GetSAApprovalList(Ulocation);
        }
//
        //--------------------------CreateSaleIndent(POST)------------------------------------
        [HttpPost]
        public ResponseEntity CreateSaleIndent(SaleIndentEntity saleEntity)
        {
            ResponseEntity rs = new ResponseEntity();
            string response = _saleServices.CreateSaleIndent(saleEntity);
            if (response != null)
            {
                rs.csi_Number= response;
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
        [HttpPost]
        public bool BulkCSI(BulkCSIModel saleEntity)
        {
        //    ResponseEntity response = new ResponseEntity();
            //bool responseCode;
          //  if (saleEntity!=null)
    //        {
          
              //  responseCode =
                    return _saleServices.BulkCSI(saleEntity);
      //          if (responseCode != false)
       //         {
               //     response.statusCode = HttpStatusCode.OK;
                 //   response.csi_Number = responseCode;
       //             response.message = " Updated Successfully.";
                //}
               // else
            //    {
            //        response.statusCode = HttpStatusCode.NotModified;
            //        response.message = "Update Failed";
            //    }
            //}
            //else
            //{
            //    response.message = "Invalid ID";
            //    response.statusCode = HttpStatusCode.NotModified;
            //}
            //return response;
        }


        [HttpPost]
        public ResponseEntity UpdateSaleIndent([FromBody]SaleIndentEntity saleIntentEntity)
        {
            ResponseEntity response = new ResponseEntity();
            string responseCode;
            if (saleIntentEntity.CSI_id > 0)
            {
                responseCode = _saleServices.UpdateSaleIndent(saleIntentEntity.CSI_id, saleIntentEntity);
                if (responseCode != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.csi_Number = responseCode;
                    response.message = " Updated Successfully.";
                }
                else
                {
                    response.statusCode = HttpStatusCode.NotModified;
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

        [HttpGet]
        public HttpResponseMessage LocationForFilter()
        {
            List<SearchDCLOC> locations = new List<SearchDCLOC>();
            locations = _saleServices.getSearchLocations();
            return Request.CreateResponse(locations);            
        }
        
        [HttpPost]
        public HttpResponseMessage GetCustomersForFilter([FromBody]FilterClass Filter)
        {
            List<ReturnCustomers> list = new List<ReturnCustomers>();
            list = _saleServices.searchCustomers(Filter);
            return Request.CreateResponse(list);
        }

        [HttpPost]
        public HttpResponseMessage GetSuppliersForFilter([FromBody]FilterClass Filter)
        {
            List<ReturnSuppliers> list = new List<ReturnSuppliers>();
            list = _saleServices.searchSuppliers(Filter);
            return Request.CreateResponse(list);
        }

        //----------------------------SearchSaleIndent(GET)-----------------------------------
        //[HttpGet]
        //public HttpResponseMessage SearchSA(DateTime? startDate, DateTime? endDate, string ULocation, string status = "null")
        //{
        //    List<SaleIndentEntity> f = new List<SaleIndentEntity>();

        //    if ((startDate != null && endDate != null) && status != "null")
        //    {
        //        var result = _saleServices.GetSAAND(startDate, endDate, status, ULocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else if (((startDate != null && endDate != null) || status != "null"))
        //    {
        //        var result = _saleServices.GetSAOR(startDate, endDate, status, ULocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }

        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage SearchSA(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation, string status = "null", string Url = "null")
        {
            List<SaleIndentEntity> f = new List<SaleIndentEntity>();

            if (startDate != null && endDate != null && (roleId != null && Url != "null"))
            {
                var result = _saleServices.SearchSA(roleId, startDate, endDate, status, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpGet]
        public HttpResponseMessage SearchConsolidatedCSI(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation, string status = "null", string Url = "null")
        {
            List<SaleIndentEntity> f = new List<SaleIndentEntity>();

            if (startDate != null && endDate != null && (roleId != null && Url != "null"))
            {
                var result = _saleServices.SearchConsolidatedCSI(roleId, startDate, endDate, status, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetCsiNumbers(DateTime? date, string Ulocation)
        {
            List<csiNumber> f = new List<csiNumber>();

            if (date != null)
            {
                var result = _saleServices.GetCsiNumbers(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetCsiNumbersByCreators(DateTime? date, string Ulocation,string CreatedBy)
        {
            List<csiNumber> f = new List<csiNumber>();

            if (date != null)
            {
                var result = _saleServices.GetCsiNumbersByCreators(date, Ulocation, CreatedBy);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

      
        
        [HttpGet]
        public HttpResponseMessage SearchSAApproval(int? roleId, DateTime? startDate, DateTime? endDate, int ULocation, string UType, string Url = "null")
        {
            List<SaleIndentEntity> f = new List<SaleIndentEntity>();

            //if ((startDate != null && endDate != null) && status != "null" && ULocation != "null")
            //{
            var result = _saleServices.GetSAAAND(roleId, startDate, endDate, ULocation, UType, Url);
                f = result;

                return Request.CreateResponse(f);
            //}
            //else if (((startDate != null && endDate != null)||status != "null") && ULocation != "null")
            //{
            //    var result = _saleServices.GetSAAOR(startDate, endDate, status, ULocation);
            //    f = result;

            //    return Request.CreateResponse(f);
            //}

            //else
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            //}
        }
       
        //--------------------------------------------------DELETE----------------------------   
        [HttpPost]
        public HttpResponseMessage DeleteSaleIndent(int id)
        {
            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _saleServices.DeleteSaleIndent(id);
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
        //-------------------------------stApproval(POST)----------------------------------------
        [HttpPost]
        public ResponseEntity slApproval([FromBody]SaleIndentEntity slEntity)
        {
            ResponseEntity response = new ResponseEntity();
            if (slEntity.CSI_id > 0)
            {
                bool responseCode = _saleServices.slApproval(slEntity);
                if (responseCode)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = "Approval Successfully";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Approval UnSuccessfully";
                }
            }
            else
            {
                response.statusCode = HttpStatusCode.NotFound;
                response.message = "Invalied Number";
            }
            return response;
        }
      
        [HttpGet]
        public HttpResponseMessage DeleteCSILineItem(int Id)
        {

            string message = "";
            bool responseID;
            if (Id > 0)
            {
                responseID = _saleServices.DeleteCSILineItem(Id);
                if (responseID)
                {
                    message = string.Format("CSILineItem deleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("CSILineItem Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid ID");
                return Request.CreateResponse(HttpStatusCode.NotModified, message);
            }
        }

        [HttpPost]
        public ResponseEntity csiBulkApproval(CSIbulkApprovalEntity bulkEntity)
        {
            ResponseEntity response = new ResponseEntity();

            bool responseCode = _saleServices.csiBulkApproval(bulkEntity);
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

        [HttpPost]
        public ResponseEntity CSIStatusUpdate(SALUpdateEntity csiEntity)
        {
            ResponseEntity response = new ResponseEntity();


            if (csiEntity.id != "" || csiEntity.id != null)
            {
                bool responseID = _saleServices.SAUpdateById(csiEntity);
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

        //----------------------------getStatusCodes(GET)----------------------------------------
        [HttpGet]
        public List<Tuple<string>> getStatusCodes()
        {
            return _saleServices.getStatuses();
        }

        #endregion
    }
}
