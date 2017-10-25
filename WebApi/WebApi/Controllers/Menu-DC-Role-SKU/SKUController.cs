﻿using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace WebApi.Controllers
{
    public class SKUController : ApiController
    {
        private readonly ISKUService _skuService;

        #region Public Constructor
        public SKUController()
        {
            _skuService = new SKUService();
        }

        #endregion
        //--------------------------------Get(GET)-----------------------------------------------
         [HttpGet]
        public HttpResponseMessage Get(int? roleId, string Url)
        {
            var sku = _skuService.GetAllSku(roleId, Url);
            if (sku != null)
            {
                var SKUEntities = sku as List<SKUEntity> ?? sku.ToList();
                if (SKUEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, SKUEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        }
         //--------------------------------Get(GET,[id])-----------------------------------------
         [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var sku = _skuService.GetskuById(id);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
         //--------------------------------GetSKUByCategory(GET,[skuCat])-----------------------------------------
         [HttpGet]
         public HttpResponseMessage GetSKUByCategory(string skuCat)
         {
             var sku = _skuService.GetSKUByCategory(skuCat);
             if (sku != null)
                 return Request.CreateResponse(HttpStatusCode.OK, sku);
             return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this Category");
         }
        
         //--------------------------------getSkuMapping(GET)------------------------------------
        [HttpGet]
         public HttpResponseMessage getSkuMapping(int skuMapId)
         {
             var sku = _skuService.getskuBasemainsub(skuMapId);
             if (sku != null)
             {
                 return Request.CreateResponse(HttpStatusCode.OK, sku);
             }
             return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
         }
         //--------------------------------CreateSKU(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreateSKU([FromBody] SKUEntity skuEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _skuService.CreateSKU(skuEntity);
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
        public bool UpdateSKU([FromBody]SKUEntity skuEntity)
        {
            if (skuEntity.SKU_Id > 0)
            {
                return _skuService.UpdateSKU(skuEntity.SKU_Id, skuEntity);
            }
            return false;
        }
        //--------------------------------DeleteSKU(POST)---------------------------------------
        [HttpPost]
        public bool DeleteSKU(int id)
        {
            if (id > 0)
                return _skuService.DeleteSKU(id);
            return false;
        }
    }
}