using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DispatchController : ApiController
    {
        private readonly IDispatchServices _dispatchServices;
       
        #region Public Constructor
        public DispatchController()
        {
            _dispatchServices = new DispatchServices();
        }
        
        [HttpPost]
        public ResponseEntity UpdateDispatchAcceptedQty(DispatchEntity disp)
        {
            ResponseEntity response = new ResponseEntity();
            string responseID = _dispatchServices.UpdateDispatchAcceptedQty(disp.Dispatch_Id,disp);
            if (responseID != "")
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
        public ResponseEntity DispatchAccept(DispatchAcceptedEntity requestType)
        {
            ResponseEntity response = new ResponseEntity();
            string responseID = _dispatchServices.DispatchAccept(requestType);
            if (responseID !="")
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
        public ResponseEntity UpdateCDNAccepteSts(DispatchUpdateEntity DispatchUpdateEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseID = _dispatchServices.UpdateCDNAcceptesSts(DispatchUpdateEntity);
            if (responseID !=false)
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
     
                [HttpGet]
        public string send()
        {
            return _dispatchServices.send();
        }
            [HttpGet]
        public bool CompareOTP(int Dispatch_Id, string OTP)
        {
            return _dispatchServices.CompareOTP(Dispatch_Id, OTP);
        }
        [HttpGet]
            public string SMS(string Customer_Number, int Dispatch_Id, string CDN_Number)
        {
            return _dispatchServices.SendSMS(Customer_Number, Dispatch_Id,CDN_Number);
        }
        [HttpGet]
        public HttpResponseMessage GetCdnNumbers(DateTime? date, string Ulocation)
        {
            List<cdnNumber> f = new List<cdnNumber>();

            if (date != null)
            {
                var result = _dispatchServices.GetCdnNumbers(date, Ulocation);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpPost]
        public ResponseEntity DispatchStatusUpdate(DispatchUpdateEntity DispatchUpdateEntity)
        {
            ResponseEntity response = new ResponseEntity();
           
            if (DispatchUpdateEntity.id != "" || DispatchUpdateEntity.id != null)
            {
                bool responseID = _dispatchServices.DispatchUpdateById(DispatchUpdateEntity);
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
        //---------------------------------CREATEDISPATCH--------------------------------
        [HttpPost]
        public DispatchResponseEntity CreateDispatch(DispatchEntity dispatchEntity)
        {
            //DispatchResponseEntity r = new DispatchResponseEntity();
        
        //    string response = _stockServices.CreateStockTransfer(stockTransfer);
        //    if (response != null)
        //    {
        //        rs.sti_Number = response;
        //        rs.statusCode = HttpStatusCode.OK;
        //        rs.message = "Inserted Successfully";
        //    }
        //    else
        //    {
        //        rs.statusCode = HttpStatusCode.InternalServerError;
        //        rs.message = "Inserted UnSuccessfully";
        //    }

        //    return rs;
        //}
            return _dispatchServices.DispatchCreation(dispatchEntity);            
        }
        [HttpGet]
        public stockAvail CheckStockAvalibility(string SKUName, double Quantity, string SKUType, string grade, string Dc_Code)
        {
            if (SKUName != null && SKUType !=null && grade !=null)
            {
                return _dispatchServices.CheckStockAvalibility(SKUName, Quantity, SKUType, grade, Dc_Code);
            }
              
            else
            {
                stockAvail avalilablility = new stockAvail();
                avalilablility.status = false;
                avalilablility.available = "Enter Valid SKUName";
                avalilablility.AvailQty = 0;
                return avalilablility;
            }

        }
        [HttpPost]
        public InvoiceResponseEntity CreateINVOICE(InvoiceEntity invoiceEntity)
        {
            ResponseEntity response = new ResponseEntity();
            return _dispatchServices.CreateInvoice(invoiceEntity);
        }
        [HttpPost]
        public HttpResponseMessage DeleteDispatch(int id)
        {

            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _dispatchServices.DeleteDispatch(id);
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
        public List<DispatchEntity> GetSingleDispatchList(int id)
        {
            return _dispatchServices.SingleDispatchList(id);
        }       
        [HttpPost]
        public ResponseEntity UpdateDispatch([FromBody]DispatchEntity dispatchEntity)
        {
            ResponseEntity response = new ResponseEntity();
            string responseCode;
            if (dispatchEntity.Dispatch_Id > 0)
            {
                responseCode = _dispatchServices.UpdateDispatch(dispatchEntity.Dispatch_Id, dispatchEntity);
                if (responseCode !="" || responseCode != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.Dispatch_Number = responseCode;
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
        [HttpPost]
        public ResponseEntity UpdateInvoice([FromBody]InvoiceEntity invoiceEntity)
        {
            ResponseEntity response = new ResponseEntity();
            int responseCode;
            if (invoiceEntity.invoice_Id > 0)
            {
                responseCode = _dispatchServices.UpdateInvoice(invoiceEntity.invoice_Id, invoiceEntity);
                if (responseCode != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.invoice_Id = responseCode;
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
        [HttpGet]
        public List<DispatchEntity> GetCustomerDispatchList(string CDNumber)
        {
            return _dispatchServices.GetCustomerDispatchList(CDNumber);
        }
        [HttpGet]
        public List<DispatchEntity> GetDispatchSTN(string stnNumber)
        {
            return _dispatchServices.DispatchSTN(stnNumber);
        }      
        //[HttpGet]
        //public HttpResponseMessage SearchDispatchList(DateTime? startDate, DateTime? endDate, string dispatchType = "null",string status="null", string ULocation = "null")
        //{
        //    List<DispatchEntity> f = new List<DispatchEntity>();

        //    if ((startDate != null && endDate != null && ULocation != "null")&& dispatchType != "null" && status!="null" )
        //    {
        //        var result = _dispatchServices.GetDSAND(startDate, endDate, dispatchType, status, ULocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else if ((startDate != null && endDate != null && ULocation != "null") && ( dispatchType != "null" || status != "null")) 
        //    {
        //        var result = _dispatchServices.GetDSSTOR(startDate, endDate, dispatchType, status, ULocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else if (((startDate != null && endDate != null) || dispatchType != "null" || status !="null") && ULocation !="null")
        //    {
        //        var result = _dispatchServices.GetDSOR(startDate, endDate, dispatchType,status, ULocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}
        [HttpGet]
        public HttpResponseMessage SearchDispatchList(int? roleId, DateTime? startDate, DateTime? endDate, string dispatchType = "null", string status = "null", string ULocation = "null", string Url = "null")
        {
            List<DispatchEntity> f = new List<DispatchEntity>();

            if ((startDate != null && endDate != null && ULocation != "null") && dispatchType != "null" && (roleId != null && Url != "null"))
            {
                var result = _dispatchServices.SearchDispatchList(roleId,startDate, endDate, dispatchType, status, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpGet]
        public HttpResponseMessage Search(int? roleId, DateTime? startDate, DateTime? endDate, string dispatchType = "null", string Route_Code = "null", string Url = "null")
        {
            List<DispatchEntity> f = new List<DispatchEntity>();

            if ((startDate != null && endDate != null ) && dispatchType != "null" && (roleId != null && Url != "null"))
            {
                var result = _dispatchServices.Search(roleId, startDate, endDate, dispatchType, Route_Code, Url);
                f = result;

                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        

        [HttpGet]
        public HttpResponseMessage SearchInvoiceList(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation = "null", string Url = "null")
        {
            List<InvoiceEntity> f = new List<InvoiceEntity>();

            if (startDate != null && endDate != null && (roleId != null && Url != "null"))
            {
                var result = _dispatchServices.SearchInvoiceList(roleId,startDate, endDate, ULocation, Url);
                f = result;

                return Request.CreateResponse(f);
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        //[HttpGet]
        //public HttpResponseMessage SearchInvoiceList(DateTime? startDate, DateTime? endDate, string ULocation = "null")
        //{
        //    List<InvoiceEntity> f = new List<InvoiceEntity>();

        //    if (startDate != null && endDate != null)
        //    {
        //        var result = _dispatchServices.GetIV(startDate,endDate,ULocation);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage SearchInvoiceNonApprovalList(int? roleId, DateTime? startDate, DateTime? endDate, string ULocation = "null", string Url = "null")
        {
            List<InvoiceEntity> f = new List<InvoiceEntity>();

            if (startDate != null && endDate != null)
            {
                var result = _dispatchServices.GetIVNA(roleId, startDate, endDate, ULocation, Url);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }

        [HttpGet]
        public List<InvoiceEntity> GetSingleInvoiceList(int id)
        {
            return _dispatchServices.GetSingleInvoiceList(id);
        }

        [HttpGet]
        public List<DispatchNumber> GetDispatchNumbers(DateTime? date, string ULocation = "null")
        {
            return _dispatchServices.GetDispatchNumbers(date, ULocation);
        }
      

        [HttpGet]
        public IEnumerable<InvoiceEntity> GetUnapprovalInvoiceList(string ULocation = "null")
        {
            return _dispatchServices.GetUnapprovalInvoiceList(ULocation);
        }


        [HttpPost]
        public ResponseEntity ApproveInvoice([FromBody]approvalInvoiceList approval)
        {
            ResponseEntity response = new ResponseEntity();
            if (approval.invoice_Id > 0)
            {
                bool responseCode = _dispatchServices.ApproveInvoice(approval);
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
            }
            else
            {
                response.statusCode = HttpStatusCode.NotFound;
                response.message = "Invalied Number";
            }
            return response;
        }

        #endregion
    }
}
