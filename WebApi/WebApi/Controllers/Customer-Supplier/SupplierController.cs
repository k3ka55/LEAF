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
    public class SupplierController : ApiController
    {
        private readonly ISupplierServices _supplierServices;

        #region Public Constructor
        public SupplierController()
        {
            _supplierServices = new SupplierServices();
        }

        #endregion
        //--------------------------------Get(GET)-----------------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int? roleId, string Url)
        {
            var supplier = _supplierServices.GetAllSupplier(roleId, Url);
            if (supplier != null)
            {
                //string pppp = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                var SupplierEntities = supplier as List<SupplierEntity> ?? supplier.ToList();
                if (SupplierEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, SupplierEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Sales not found");
        }
        //--------------------------------Get(GET,[id])------------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var supplier = _supplierServices.GetsupplierById(id);
            if (supplier != null)
                return Request.CreateResponse(HttpStatusCode.OK, supplier);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Supplier found for this id");
        }

        //--------------------------------GetSupplierDC(GET)-------------------------------------
        //[HttpGet]
        //public HttpResponseMessage GetSupplierDC(string dcCode)
        //{
        //    var supplier = _supplierServices.getSupplierDCinfo(dcCode);
        //    if (supplier != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, supplier);
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Supplier not found");
        //}

        [HttpPost]
        public HttpResponseMessage searchSuppliers(string location)
        {
            var list = _supplierServices.searchSupplier(location);

            return Request.CreateResponse(list);
        }
        //--------------------------------CreateSupplier(POST)-----------------------------------
        [HttpPost]
        public bool CreateSupplier([FromBody] SupplierEntity supplierEntity)
        {
            return _supplierServices.CreateSupplier(supplierEntity);
        }
        //--------------------------------UpdateSupplier(POST)-----------------------------------
        [HttpPost]
        public bool UpdateSupplier([FromBody]SupplierEntity supplierEntity)
        {
            if (supplierEntity.Supplier_ID > 0)
            {
                return _supplierServices.UpdateSupplier(supplierEntity.Supplier_ID, supplierEntity);
            }
            return false;
        }
        //--------------------------------DeleteSupplier(POST)-----------------------------------
        [HttpPost]
        public bool DeleteSupplier(int id)
        {
            if (id > 0)
                return _supplierServices.DeleteSupplier(id);
            return false;
        }
    }
}