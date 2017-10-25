using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using BusinessServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class RouteAreaController : ApiController
    {
        private readonly IRouteAreaService _routeService;

        #region Public Constructor
        public RouteAreaController()
        {
            _routeService = new RouteAreaServices();
        }
        #endregion

        [HttpGet]
        public bool Delete(string Location_Code)
        {
            if (Location_Code != null)
            {
                return _routeService.Delete(Location_Code);
            }

            return false;

        }
        //[HttpGet]
        //public List<RouteMasterEntity> Get(string Route_Code)
        //{
        //    return _routeService.GetRouteByRId(Route_Code);
        //}
        //[HttpPost]
        //public ResponseEntity Insert([FromBody]RouteMasterEntity RoutMast)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    bool responseCode = _routeService.InsertRoute(RoutMast);
        //    if (responseCode)
        //    {
        //        response.statusCode = HttpStatusCode.OK;
        //        response.message = "Inserted Successfully";
        //    }
        //    else
        //    {
        //        response.statusCode = HttpStatusCode.InternalServerError;
        //        response.message = "Inserted UnSuccessfully";
        //    }
        //    return response;
        //}
        ////--------------------------------UpdateSKU(POST)---------------------------------------
        [HttpPost]
        public bool Update([FromBody]RouteAreaEntity routeEntity)
        {
            if (routeEntity.Location_Code != null)
            {
                return _routeService.Update(routeEntity.Location_Code, routeEntity);
            }
            return false;
        }

        [HttpPost]
        public ResponseEntity Insert([FromBody]RouteAreaEntity RoutMast)
        {
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


        [HttpGet]
        public HttpResponseMessage GetRoutesByTarget(string Target_Location_Code, string Target_Location_Type)
        {
            var sku = _routeService.GetAllRoutes(Target_Location_Code, Target_Location_Type);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }

        [HttpGet]
        public dynamic GetAllRouteAreas(int? roleId, string Url)
        {
            return _routeService.GetAllRouteAreas(roleId, Url);

        }

        [HttpGet]
        public RouteAreaEntity Get(string Location_Code)
        {
            return _routeService.Get(Location_Code);

        }

    }
}
