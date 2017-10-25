using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using BusinessServices.Interfaces;
using BusinessServices.Services;
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
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class MaterialMasterController : ApiController
    {
        private readonly IMaterialMasterServices _materialServices;

        #region Public Constructor
         public MaterialMasterController()
        {
            _materialServices = new MaterialMasterServices();
        }
       
         [HttpPost]
         public HttpResponseMessage ExcelImport(fileImportMI fileDetail)
         {
             //RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
             return Request.CreateResponse(_materialServices.ExcelImportFromMI(fileDetail));
         }
         [HttpGet]
         public HttpResponseMessage Get(string id)
         {
             var list = _materialServices.get(id);

             return Request.CreateResponse(list);

         }
         [HttpPost]
         public HttpResponseMessage CheckMaterialMaster(MaterialList material)
         {
             //RIExcelImport ciData = _rateIndentServices.ExcelImportForri(fileDetail);
             return Request.CreateResponse(_materialServices.CheckMaterialMaster(material));
         }
        [HttpGet]
         public HttpResponseMessage searchMaterial(int? roleId, int regionid, int locationid, int dcid, int skuTypeid, string Url = "null")
         {
             var list = _materialServices.searchMatrerial(roleId,regionid, locationid, dcid, skuTypeid, Url);

             return Request.CreateResponse(list);
         }

        [HttpGet]
        public HttpResponseMessage searchMaterial(int regionid, int locationid, int dcid, int skuTypeid)
        {
            var list = _materialServices.searchMatrerial(regionid, locationid, dcid, skuTypeid);

            return Request.CreateResponse(list);
        }

         [HttpPost]
        public HttpResponseMessage CreateMaterial(MaterialList materialEntity)
         {
             ResponseEntity rs = new ResponseEntity();
             bool response = _materialServices.createMaterial(materialEntity);
             if (response)
             {
                // rs.materialID = response;
                 rs.statusCode = HttpStatusCode.Created;
                 rs.message = "Inserted Successfully";
             }
             else
             {
                 rs.statusCode = HttpStatusCode.NotImplemented;
                 rs.message = "Inserted UnSuccessfully";
             }

             return Request.CreateResponse(rs);
         }

         [HttpPost]
         public HttpResponseMessage UpdateMaterial(Material_MasterEntity materialEntity)
         {
             ResponseEntity response = new ResponseEntity();
             bool responseCode;
             if (materialEntity.Material_Id > 0)
             {
                 responseCode = _materialServices.updateMaterialMaster(materialEntity.Material_Id, materialEntity);
                 if (responseCode)
                 {
                     response.statusCode = HttpStatusCode.OK;
                     response.message = " Updated Successfully.";
                 }
                 else
                 {
                     response.statusCode = HttpStatusCode.NotModified;
                     response.message = "Update Failed";
                 }
             }
             else
             {
                 response.message = "Invalid ID";
                 response.statusCode = HttpStatusCode.NotModified;
             }
             return Request.CreateResponse(response);
         }

         [HttpGet]
         public HttpResponseMessage DeleteMaterial(string id, string reason)
         {
             string message = "";
             bool responseID;
             if (id !=null)
             {
                 responseID = _materialServices.deleteMaterial(id, reason);
                 if (responseID)
                 {
                     message = string.Format("Deleted Successfully.");
                     return Request.CreateResponse(HttpStatusCode.OK, message);
                 }
                 else
                 {
                     message = string.Format("Not Found");
                     return Request.CreateResponse(HttpStatusCode.NotFound, message);
                 }
             }
             else
             {
                 message = string.Format("Invalid ID");
                 return Request.CreateResponse(HttpStatusCode.NotModified, message);
             }

         }

        #endregion
    }
}
