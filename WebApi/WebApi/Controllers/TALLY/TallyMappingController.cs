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
    public class TallyMappingController : ApiController
    {
        private readonly ITallyMapping _skuService;

        #region Public Constructor
   public TallyMappingController()
        {
            _skuService = new TallyMappingServices();
        }

        #endregion
        //--------------------------------Get(GET)-----------------------------------------------
         [HttpGet]
        public IEnumerable<TallyMappingEntity> Get(int? roleId, string Url)
        {
           // var sku = 
             return  _skuService.GetAllTallyMapping(roleId, Url);
            //if (sku != null)
            //{
            //    var SKUEntities = sku as List<TallyMappingEntity> ?? sku.Where(a=>a.Is_Delete==false).ToList();
            //    if (SKUEntities.Any())
            //        return Request.CreateResponse(HttpStatusCode.OK, SKUEntities);
            //}
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        }
         //--------------------------------Get(GET,[id])-----------------------------------------
         [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var sku = _skuService.GetTallyMappingById(id);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
         //--------------------------------GetSKUByCategory(GET,[skuCat])-----------------------------------------
         //[HttpGet]
         //public HttpResponseMessage GetSKUByCategory(string skuCat)
         //{
         //    var sku = _skuService.GetSKUByCategory(skuCat);
         //    if (sku != null)
         //        return Request.CreateResponse(HttpStatusCode.OK, sku);
         //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this Category");
         //}
        
         //--------------------------------getSkuMapping(GET)------------------------------------
        //[HttpGet]
        // public HttpResponseMessage getSkuMapping(int skuMapId)
        // {
        //     var sku = _skuService.getskuBasemainsub(skuMapId);
        //     if (sku != null)
        //     {
        //         return Request.CreateResponse(HttpStatusCode.OK, sku);
        //     }
        //     return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        // }
         //--------------------------------CreateSKU(POST)---------------------------------------
        [HttpPost]
         public ResponseEntity CreateTallyMapping([FromBody] TallyMappingEntity TallyMappingEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _skuService.CreateTallyMapping(TallyMappingEntity);
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
        public bool UpdateTallyMapping([FromBody]TallyMappingEntity TallyMappingEntity)
        {
            if (TallyMappingEntity.ID > 0)
            {
                return _skuService.UpdateTallyMapping(TallyMappingEntity.ID, TallyMappingEntity);
            }
            return false;
        }
//--------------------------------DeleteSKU(POST)---------------------------------------
        [HttpPost]
        public bool DeleteTallyMapping(int id)
        {
            if (id > 0)
                return _skuService.DeleteTallyMapping(id);
            return false;
        }
    }
}