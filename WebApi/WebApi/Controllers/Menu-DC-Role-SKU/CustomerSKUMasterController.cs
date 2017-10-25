using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class CustomerSKUMasterController : ApiController
    {
        private readonly ICustomerSKUMasterServices _skuService;

        #region Public Constructor
        public CustomerSKUMasterController()
        {
            _skuService = new CustomerSKUMasterService();
        }
        //
        #endregion
        //--------------------------------Get(GET)-----------------------------------------------
        [HttpGet]
        public dynamic Get(int? roleId, string Url)
        {
            return _skuService.GetAllCustSKUMaster(roleId, Url);
            //var sku = _skuService.GetAllCustSKUMaster(roleId, Url);
            //if (sku != null)
            //{
            //return Request.CreateResponse(HttpStatusCode.OK, sku);
            //}
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        }

        [HttpPost]
        public HttpResponseMessage CheckCustomerSKUMaster(CustomerSKUMasterList CustomerSKUMaster)
        {
            //RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
            return Request.CreateResponse(_skuService.CheckCustomerSKUMaster(CustomerSKUMaster));
        }
        [HttpPost]
        public HttpResponseMessage ExcelImport(fileImportSTI fileDetail)
        {
            //RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
            return Request.CreateResponse(_skuService.ExcelImportForCustSKUMapping(fileDetail));
        }
        //--------------------------------Get(GET,[id])-----------------------------------------
        [HttpGet]
        public CustomerSKUMasterModelEntity Get(int Id)
        {
            return _skuService.GetCustSKUMasterById(Id);
            //if (sku != null)
            //    return Request.CreateResponse(HttpStatusCode.OK, sku);
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        //--------------------------------GetSKUByCategory(GET,[skuCat])-----------------------------------------
        //[HttpGet]
        //public HttpResponseMessage GetCustSKUByCategory(string skuCat)
        //{
        //    var sku = _skuService.GetCustSKUMasterByCategory(skuCat);
        //    if (sku != null)
        //        return Request.CreateResponse(HttpStatusCode.OK, sku);
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this Category");
        //}

        //--------------------------------CreateSKU(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreateCustSKUMaster([FromBody] CustomerSKUMasterModelEntity skuEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _skuService.CreateCustSKUMaster(skuEntity);
            if (responseCode)
            {
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
        //--------------------------------UpdateSKU(POST)---------------------------------------
        [HttpPost]
        public bool UpdateCustSKUMaster([FromBody]CustomerSKUMasterModelEntity skuEntity)
        {
            if (skuEntity != null)
            {
                return _skuService.UpdateCustSKUMaster(skuEntity);
            }
            return false;
        }
        //--------------------------------DeleteSKU(POST)---------------------------------------
        [HttpGet]
        public bool DeleteCustSKUMaster(int id)
        {
            return _skuService.DeleteCustSKUMaster(id);

        }
    }
}