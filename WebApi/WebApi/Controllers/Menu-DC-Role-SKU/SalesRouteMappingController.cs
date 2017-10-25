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
    public class SalesRouteMappingController : ApiController
    {
        private readonly ISalesRouteMappingService _routeService;

        #region Public Constructor
        public SalesRouteMappingController()
        {
            _routeService = new SalesRouteMappingService();
        }
        #endregion

        [HttpGet]
        public bool Delete(int Sales_Person_Id)
        {
            if (Sales_Person_Id > 0)
            {
                return _routeService.DeleteSaleRouteMapping(Sales_Person_Id);
            }

            return false;

        }
        [HttpGet]
        public dynamic GetSalesPersons(string Route_Code)
        {
            return _routeService.GetSalesPersons(Route_Code);
        }

        [HttpGet]
        public SalesRoutemappingEntity Get(int Sales_Person_Id)
        {
            return _routeService.Get(Sales_Person_Id);
        }
        [HttpPost]
        public ResponseEntity Insert([FromBody]SalesRoutemappingEntity RoutMast)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _routeService.InsertSaleRouteMapping(RoutMast);
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
        ////--------------------------------UpdateSKU(POST)---------------------------------------
        [HttpPost]
        public bool Update([FromBody]SalesRoutemappingEntity routeEntity)
        {
            if (routeEntity.Sales_Person_Id > 0)
            {
                return _routeService.UpdateSaleRouteMapping(routeEntity.Sales_Person_Id, routeEntity);
            }
            return false;
        }


        [HttpGet]
        public dynamic GetAllSaleRouteMapping(int? roleId, string Url)
        {
            //
            //  var sku =
            return _routeService.GetAllSaleRouteMapping(roleId, Url);
            //if (sku != null)
            //    return Request.CreateResponse(HttpStatusCode.OK, sku);
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
    }
}
