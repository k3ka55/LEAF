using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using BusinessServices;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DCMasterController : ApiController
    {
        private readonly IDCMasterService _dcmasterServices;
        
        #region Public Constructor
        public DCMasterController()
        {
            _dcmasterServices = new DCMasterService();
        }

       
        //--------------------------------Get(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int? roleId, string Url)
        {
            var dcmaster = _dcmasterServices.GetAllDCMaster(roleId, Url);
            if (dcmaster != null)
            {
                var DCMasterEntities = dcmaster as List<DCMasterEntity> ?? dcmaster.ToList();
                if (DCMasterEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, DCMasterEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records not found");
        }

        [HttpGet]
        public HttpResponseMessage GetByLocationSupplier(string Code)
        {
            var dcmaster = _dcmasterServices.GetByLocationSupplier(Code);
            if (dcmaster != null)
                return Request.CreateResponse(HttpStatusCode.OK, dcmaster);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this id");
        }

             [HttpGet]
        public HttpResponseMessage GetByLocationCustomer(string Code)
             {
            var dcmaster = _dcmasterServices.GetByLocationCustomer(Code);
            if (dcmaster != null)
                return Request.CreateResponse(HttpStatusCode.OK, dcmaster);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this id");
        }
        //--------------------------------GetCountries(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage GetCoutries()
        {
            var dcmaster = _dcmasterServices.GetAllCountries();
            if (dcmaster != null)
            {
                var CountryEntities = dcmaster as List<CountryEntity> ?? dcmaster.ToList();
                if (CountryEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, CountryEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records not found");
        }
        //--------------------------------Get(GET,[id])---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var dcmaster = _dcmasterServices.GetdcmasterById(id);
            if (dcmaster != null)
                return Request.CreateResponse(HttpStatusCode.OK, dcmaster);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this id");
        }
        //--------------------------------CreateDCMaster(GET)---------------------------------------
        [HttpPost]
        public int CreateDCMaster([FromBody] DCMasterEntity dcmasterEntity)
        {
            return _dcmasterServices.CreateDCMaster(dcmasterEntity);
        }
        //--------------------------------UpdateDCMaster(GET)---------------------------------------
        [HttpPost]
        public bool UpdateDCMaster([FromBody]DCMasterEntity dcmasterEntity)
        {
            if (dcmasterEntity.Dc_Id > 0)
            {
                return _dcmasterServices.UpdateDCMaster(dcmasterEntity.Dc_Id, dcmasterEntity);
            }
            return false;
        }
        //--------------------------------Delete(GET)------------------------------------------------
        [HttpPost]
        public bool DeleteDCMaster(int id)
        {
            if (id > 0)
                return _dcmasterServices.DeleteDCMaster(id);
            return false;
        }
        #endregion

    }
}
