using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using BusinessServices.Interfaces;
using BusinessServices.Services.Menu_DC_Role_SKU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers.Menu_DC_Role_SKU
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LocationMasterController : ApiController
    {
        private readonly ILocationMasterService _locmasterServices;

        #region Public Constructor
        public LocationMasterController()
        {
            _locmasterServices = new LocationMasterService();
        }

        [HttpGet]
        public HttpResponseMessage Get(int? roleId, string Url = "null")
        {
            var dcmaster = _locmasterServices.GetAllLocationMaster(roleId,Url);
            if (dcmaster != null)
            {
                var DCMasterEntities = dcmaster as List<LocationMasterEntity> ?? dcmaster.ToList();
                if (DCMasterEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, DCMasterEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records not found");
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var dcmaster = _locmasterServices.GetlocationmasterById(id);
            if (dcmaster != null)
                return Request.CreateResponse(HttpStatusCode.OK, dcmaster);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this id");
        }

        [HttpPost]
        public int CreateLocationMaster([FromBody] LocationMasterEntity dcmasterEntity)
        {
            return _locmasterServices.createLocationMaster(dcmasterEntity);
        }

        [HttpPost]
        public bool UpdateLocationMaster([FromBody]LocationMasterEntity dcmasterEntity)
        {
            if (dcmasterEntity.Location_Id > 0)
            {
                return _locmasterServices.updateLocationMaster(dcmasterEntity.Location_Id, dcmasterEntity);
            }
            return false;
        }

        [HttpPost]
        public bool DeleteLocationMaster(int id)
        {
            if (id > 0)
                return _locmasterServices.DeleteLocation(id);
            return false;
        }

         #endregion
    }
}