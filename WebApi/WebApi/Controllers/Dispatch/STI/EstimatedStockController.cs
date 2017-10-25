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
    public class EstimatedStockController : ApiController
    {
    //
         private readonly IEstimatedStockService _estimatedStockServices;

        #region Public Constructor
        public EstimatedStockController()
        {
            _estimatedStockServices = new EstimatedStockServices();
        }

       
        //---------------------------------GET-----------------------------------------------
        //[HttpGet]
        //public List<PurchaseOrderEntity> Get(string id)
        //{
        //    return _purchsServices.GetPurchaseLineItemforPO(id);
        //}
        //[HttpGet]
        //public List<PurchaseOrderEntity> GetforGRN(string id)
        //{
        //    return _purchsServices.GetPurchaseLineItemforGRN(id);
        //}
        //[HttpGet]
        //public List<PurchaseOrderEntity> GetPOApprovalList(string Ulocation)
        //{
        //    return _purchsServices.GetPOApprovalList(Ulocation);
        //}
        ////---------------------------------GETSTATUSCODES------------------------------------
        //[HttpGet]
        //public List<Tuple<string>> getStatusCodes()
        //{
        //    return _purchsServices.getStatuses();
        //}       
        //---------------------------------------POST-----------------------------------------
        [HttpPost]
        public ResponseEntity Create(EstimtedStkEntity estimatedEntity)
        {
            ResponseEntity rs = new ResponseEntity();
            bool response = _estimatedStockServices.CreateEstimatedStock(estimatedEntity);
            if (response != false)
            {
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
        
         [HttpPost]
         public HttpResponseMessage ExcelImport(fileImportEST fileDetail)
         {
             return Request.CreateResponse(_estimatedStockServices.ExcelImportESTK(fileDetail));
         }
        ////---------------------------------------------SEARCH--------------------------------
        [HttpGet]
        public HttpResponseMessage Search(int? roleId, DateTime? date, string ULocation = "null",string SKU_Type="null", string Url = "null")
        {
            List<EstimatedStockEntitySearch> f = new List<EstimatedStockEntitySearch>();

            if (date != null && ULocation != "null" && roleId != null && SKU_Type!=null && Url != null)
            {
                var result = _estimatedStockServices.GetEstimatedStock(roleId, date, ULocation,SKU_Type, Url);
                f = result;
                return Request.CreateResponse(f);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
            }
        }
        [HttpPost]
        public HttpResponseMessage CheckEstimatedStock(EstimtedStkEntity material)
        {
            //RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
            return Request.CreateResponse(_estimatedStockServices.CheckMaterialMaster(material));
        }
        [HttpGet]
        public HttpResponseMessage Delete(int id,string deleteby)
        {
            //
            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _estimatedStockServices.DeleteEstimatedStock(id,deleteby);
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
      
        //[HttpPost]
        //public ResponseEntity UpdatePurchaseOrder([FromBody]PurchaseOrderEntity purchaseOrderEntity)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    string responseCode;
        //    if (purchaseOrderEntity.Po_id > 0)
        //    {
        //        responseCode = _purchsServices.UpdatePurchaseOrder(purchaseOrderEntity.Po_id, purchaseOrderEntity);
        //        if (responseCode != null)
        //        {
        //            response.statusCode = HttpStatusCode.OK;
        //            response.po_Number = responseCode;
        //            response.message = " Updated Successfully.";
        //        }
        //        else
        //        {
        //            response.statusCode = HttpStatusCode.InternalServerError;
        //            response.message = "Update Failed";
        //        }
        //    }
        //    else
        //    {
        //        response.message = "Invalid ID";
        //        response.statusCode = HttpStatusCode.NotModified;
        //    }
        //    return response;
        //}
       
        //[HttpGet]
        //public HttpResponseMessage SearchPurchaseOrderforPOEdit(int? roleId, string ULocation = "null", string Url = "null",string poNumber="null")
        //{
        //    List<POWithLineItemEntity> f = new List<POWithLineItemEntity>();

        //    if ((ULocation != "null" && roleId != null && Url != "null") &&(poNumber!="null"))
        //    {
        //        var result = _purchsServices.GetPOforEditALL(roleId, ULocation, Url, poNumber);
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
        //    else if (( ULocation != "null" && roleId != null && Url != "null") || (poNumber != "null"))
        //    {
        //        var result = _purchsServices.GetPOforEditOR(roleId, ULocation, Url, poNumber);
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
  
        //    else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //        }
        //}
        
        //[HttpGet]
        //public HttpResponseMessage SearchPOApprovalList( DateTime? startDate, DateTime? endDate, string supplierName = "null", string ULocation = "null")
        //{
        //    List<PurchaseOrderEntity> f = new List<PurchaseOrderEntity>();

        //    if ((startDate != null && endDate != null) && supplierName != "null" && (ULocation != "null"))
        //    {
        //        var result = _purchsServices.GetPOApprovalAND( startDate, endDate, supplierName, ULocation);
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
        //    else if (((startDate != null && endDate != null) || supplierName != "null") && (ULocation != "null"))
        //    {
        //        var result = _purchsServices.GetPOApprovalOR( startDate, endDate, supplierName, ULocation);
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}
     
        //-----------------------------------------DELETE--------------------------------------
      
        //[HttpGet]
        //public HttpResponseMessage DeletePurchaseOrderLineItem(int Id)
        //{

        //    string message = "";
        //    bool responseID;
        //    if (Id > 0)
        //    {
        //        responseID = _purchsServices.DeletePurchaseOrderLineItem(Id);
        //        if (responseID)
        //        {
        //            message = string.Format("POLineItem eleted Successfully.");
        //            return Request.CreateResponse(HttpStatusCode.OK, message);
        //        }
        //        else
        //        {
        //            message = string.Format("POLineItem Not Found");
        //            return Request.CreateResponse(HttpStatusCode.NotFound, message);
        //        }
        //    }
        //    else
        //    {
        //        message = string.Format("Invalid ID");
        //        return Request.CreateResponse(HttpStatusCode.NotModified, message);
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage UpdateLineItemPORate(int Id,string poNumber,double? ARate, double? BRate, double? CRate)
        //{
        //    string message = "";
        //    bool responseID;
        //    if (Id > 0)
        //    {
        //        responseID = _purchsServices.UpdateLineItemPORate(Id,poNumber,ARate, BRate, CRate);
        //        if (responseID)
        //        {
        //            message = string.Format("PORate updated Successfully.");
        //            return Request.CreateResponse(HttpStatusCode.OK, message);
        //        }
        //        else
        //        {
        //            message = string.Format("PORate Not Found");
        //            return Request.CreateResponse(HttpStatusCode.NotFound, message);
        //        }
        //    }
        //    else
        //    {
        //        message = string.Format("Invalid ID");
        //        return Request.CreateResponse(HttpStatusCode.NotModified, message);
        //    }
        //}
        ////---------------------------------POAPPROVAL-----------------------------------------  
        //[HttpPost]
        //public ResponseEntity poApproval([FromBody]PurchaseOrderEntity poEntity)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    if (poEntity.Po_id > 0)
        //    {
        //        bool responseCode = _purchsServices.poApproval(poEntity);
        //        if (responseCode)
        //        {
        //            response.statusCode = HttpStatusCode.OK;
        //            response.message = "Success";
        //        }
        //        else
        //        {
        //            response.statusCode = HttpStatusCode.InternalServerError;
        //            response.message = "Not Success";
        //        }
        //    }
        //    else
        //    {
        //        response.statusCode = HttpStatusCode.NotFound;
        //        response.message = "Invalied PONumber";
        //    }
        //    return response;
        //}
        ////---------------------------------POBULKAPPROVAL-------------------------------------
        //[HttpPost]
        //public ResponseEntity poBulkApproval(bulkApprovalEntity bulkEntity)
        //{
        //    ResponseEntity response = new ResponseEntity();
            
        //        bool responseCode = _purchsServices.poBulkApproval(bulkEntity);
        //        if (responseCode)
        //        {
        //            response.statusCode = HttpStatusCode.OK;
        //            response.message = "Success";
        //        }
        //        else
        //        {
        //            response.statusCode = HttpStatusCode.InternalServerError;
        //            response.message = "Not Success";
        //        }
        //    return response;
        //}
      #endregion
    }
}

