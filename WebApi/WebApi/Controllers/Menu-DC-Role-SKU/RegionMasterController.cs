using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using BusinessServices;
using System.Web.Http.Cors;
using BusinessServices.Interfaces;
using BusinessServices.Services;
using BusinessEntities.Entity;

namespace WebApi.Controllers
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RegionMasterController : ApiController
    {
     private readonly IRegionMasterService _regionmasterServices;
        
        #region Public Constructor
     public RegionMasterController()
        {
            _regionmasterServices = new RegionMasterService();
        }

       
        //--------------------------------Get(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var regionmaster = _regionmasterServices.GetAllRegion();
            if (regionmaster != null)
            {
                var RegionMasterEntities = regionmaster as List<RegionMasterEntity> ?? regionmaster.ToList();
                if (RegionMasterEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, RegionMasterEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records not found");
        }

        //--------------------------------Get(GET,[id])---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var regionmaster = _regionmasterServices.GetregionmasterById(id);
            if (regionmaster != null)
                return Request.CreateResponse(HttpStatusCode.OK, regionmaster);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this id");
        }
        //--------------------------------CreateDCMaster(GET)---------------------------------------
        [HttpPost]
        public int CreateRegionMaster([FromBody] RegionMasterEntity regionmasterEntity)
        {
            return _regionmasterServices.CreateRegionMaster(regionmasterEntity);
        }
        //--------------------------------UpdateDCMaster(GET)---------------------------------------
        [HttpPost]
        public bool UpdateRegionMaster([FromBody]RegionMasterEntity regionmasterEntity)
        {
            if (regionmasterEntity.Region_Id > 0)
            {
                return _regionmasterServices.UpdateRegionMaster(regionmasterEntity.Region_Id, regionmasterEntity);
            }
            return false;
        }
        //--------------------------------Delete(GET)------------------------------------------------
        [HttpPost]
        public bool DeleteRegionMaster(int id)
        {
            if (id > 0)
                return _regionmasterServices.DeleteRegionMaster(id);
            return false;
        }
        #endregion

    }
}