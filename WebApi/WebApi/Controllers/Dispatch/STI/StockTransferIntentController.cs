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
    public class StockTransferIntentController : ApiController
    {
        private readonly IStockTransferIntentServices _stockServices;

        #region Public Constructor
        public StockTransferIntentController()
        {
            _stockServices = new StockTransferIntentServices();
        }

        //--------------------------------Get(GET)-----------------------------------------------
        [HttpGet]
        public List<StockTransferIntentEntity> Get(string id)
        {
            return _stockServices.GetStockLineItem(id);
        }

        [HttpPost]
        public HttpResponseMessage ExcelImport(fileImportSTI fileDetail)
        {
             return Request.CreateResponse(_stockServices.ExcelImportForCI(fileDetail));
        }


        [HttpGet]
        public List<StockTransferIntentEntity> GetForModel(string id)
        {
            return _stockServices.GetStockLineItemForModel(id);
        }
       
        //----------------------------GetSTApprovalList(GET)-------------------------------------
        [HttpGet]
        public List<StockTransferIntentEntity> GetSTApprovalList(string Ulocation)
        {
            return _stockServices.GetSTApprovalList(Ulocation);
        }

        [HttpPost]
        public ResponseEntity STIStatusUpdate(STIUpdateEntity stiEntity)
        {
            ResponseEntity response = new ResponseEntity();


            if (stiEntity.id != "" || stiEntity.id != null)
            {
                bool responseID = _stockServices.STIUpdateById(stiEntity);
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

        //---------------------------------------POST--------------------------------------
        [HttpPost]
        public ResponseEntity CreateStockTransfer(StockTransferIntentEntity stockTransfer)
        {
            ResponseEntity rs = new ResponseEntity();
            string response = _stockServices.CreateStockTransfer(stockTransfer);
            if (response != null)
            {
                rs.sti_Number = response;
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
        public ResponseEntity UpdateStockTransfer([FromBody]StockTransferIntentEntity stockTransferIntentEntity)
        {
            ResponseEntity response = new ResponseEntity();
            string responseCode;
            if (stockTransferIntentEntity.STI_id > 0)
            {
                responseCode = _stockServices.UpdateStockTransfer(stockTransferIntentEntity.STI_id, stockTransferIntentEntity);
                if (responseCode != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.sti_Number = responseCode;
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

        //------------------------------------SEARCH----------------------------------------


       [HttpGet]
        public HttpResponseMessage SearchStockTransfer(int? roleId, DateTime? startDate, DateTime? endDate, string status = "null", string ProcessedBy = "null", string ULocation = "null", string Url = "null")
        {
            List<StockTransferIntentEntity> f = new List<StockTransferIntentEntity>();

            if ((startDate != null && endDate != null) && (ProcessedBy != "null" || ULocation != "null") && (roleId != null && Url != "null"))
            {
                var result = _stockServices.SearchStockTransfer(roleId,startDate, endDate, status, ProcessedBy, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage SearchSTApproval(int? roleId, DateTime? startDate, DateTime? endDate, string status = "null", string ULocation = "null", string Url = "null")
        {
            List<StockTransferIntentEntity> f = new List<StockTransferIntentEntity>();

            if ((startDate != null && endDate != null) && status != "null" && (ULocation != "null") && (roleId != null && Url != "null"))
            {
                var result = _stockServices.GetSTAAND(roleId, startDate, endDate, status, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }
            else if (((startDate != null && endDate != null) || status != "null") && (ULocation != "null") && (roleId != null && Url != "null"))
            {
                var result = _stockServices.GetSTAOR(roleId, startDate, endDate, status, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
            
        //-----------------------------------------DELETE-------------------------------------
        [HttpPost]
        public HttpResponseMessage DeleteStockTransfer(int id)
        {
            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _stockServices.DeleteStockTransfer(id);
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
        public HttpResponseMessage DeleteSTILineItem(int Id)
        {

            string message = "";
            bool responseID;
            if (Id > 0)
            {
                responseID = _stockServices.DeleteSTIOrderLineItem(Id);
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
        //-------------------------------stApproval(POST)----------------------------------------
        [HttpPost]
        public ResponseEntity stApproval([FromBody]StockTransferIntentEntity stEntity)
        {
            ResponseEntity response = new ResponseEntity();
            if (stEntity.STI_id > 0)
            {
                bool responseCode = _stockServices.stApproval(stEntity);
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
        //-------------------------------stApprovalBulkApproval(POST)----------------------------------------

        [HttpPost]
        public ResponseEntity stiBulkApproval(STIbulkApprovalEntity bulkEntity)
        {
            ResponseEntity response = new ResponseEntity();

            bool responseCode = _stockServices.stiBulkApproval(bulkEntity);
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
        //----------------------------getStatusCodes(GET)----------------------------------------
        [HttpGet]
        public List<Tuple<string>> getStatusCodes()
        {
            return _stockServices.getStatuses();
        }

        #endregion
    }
}
