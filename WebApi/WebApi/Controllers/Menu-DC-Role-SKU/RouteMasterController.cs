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
    public class RouteMasterController : ApiController
    {
        private readonly IRouteMaster _routeService;

        #region Public Constructor
        public RouteMasterController()
        {
            _routeService = new RouteMasterServices();
        }
        #endregion

        [HttpGet]
        public bool DeleteRoute(string Route_Code)
        {
            if (Route_Code != null)
            {
                return _routeService.DeleteRoute(Route_Code);
            }

            return false;

        }
        [HttpGet]
        public dynamic GetAllForMapping()
        {
            return _routeService.GetAllForMapping();
        }


        [HttpGet]
        public RouteMasterEntity Get(string Route_Code)
        {
            return _routeService.Get(Route_Code);
        }

        [HttpPost]
        public ResponseEntity Insert([FromBody]RouteMasterEntity RoutMast)
        {
            //
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _routeService.InsertRoute(RoutMast);
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
        //
        ////--------------------------------UpdateSKU(POST)---------------------------------------
        [HttpPost]
        public bool Update([FromBody]RouteMasterEntity routeEntity)
        {
            if (routeEntity.Route_Code != null)
            {
                return _routeService.Update(routeEntity.Route_Code, routeEntity);
            }
            return false;
        }

        [HttpGet]
        public dynamic GetAll()
        {
            //  var sku =
            return _routeService.GetAll();
            //if (sku != null)
            //    return Request.CreateResponse(HttpStatusCode.OK, sku);
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }

        [HttpGet]
        public dynamic GetAllRoutes(int? roleId, string Url)
        {
            // 
            return _routeService.GetAllRoutes(roleId, Url);
            //if (sku != null)
            //    return Request.CreateResponse(HttpStatusCode.OK, sku);
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
    }
}
