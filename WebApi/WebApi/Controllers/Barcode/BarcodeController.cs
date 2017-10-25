using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BarcodeController : ApiController
    {
        private readonly IBarcodeLabelTemplate _barcodeService;

        #region Public Constructor
        public BarcodeController()
        {
            _barcodeService = new BarcodeLabelTemplateService();
        }
        #endregion
        [HttpGet]
        public List<CustomerLabelTemplateEntity> GetLabelTemplateByName(string Template_name)
        {
            return _barcodeService.GetLabelTemplateByName(Template_name);
        }
        [HttpGet]
        public List<CustomerLabelTemplateMappingEntity> GetMappedCustByTId(string Template_Id)
        {
            return _barcodeService.GetMappedCustomerByTId(Template_Id);
        }
        [HttpGet]
        public TemplateName CheckTempNameAvalibility(string TempName)
        {
            return _barcodeService.CheckTempNameAvalibility(TempName);
        }


        [HttpGet]
        public HttpResponseMessage GetAllCustomers()
        {
            var sku = _barcodeService.GetAllCustomers();
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        [HttpGet]
        public HttpResponseMessage GetAllCustomerTemplate()
        {
            var sku = _barcodeService.GetAllCustomerTemplate();
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        //--------------------------------GET-----------------------------------------------
        [HttpGet]
        public HttpResponseMessage GetCustomerTemplate(int cust_Id)
        {
            var sku = _barcodeService.GetCustomerTemplate(cust_Id);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        //[HttpGet]
        //public HttpResponseMessage GetAllCustomerSKUMaster()
        //{
        //    var sku = _barcodeService.GetAllCustomerSKUMaster();
        //    if (sku != null)
        //    {
        //        var SKUEntities = sku as List<CustomerSKUMasterEntity> ?? sku.ToList();
        //        if (SKUEntities.Any())
        //            return Request.CreateResponse(HttpStatusCode.OK, SKUEntities);
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        //}
        //--------------------------------Get(GET,[id])-----------------------------------------
        //[HttpGet]
        //public HttpResponseMessage GetCustskuById(int Customer_SKU_Master_Id)
        //{
        //    var sku = _barcodeService.GetCustskuById(Customer_SKU_Master_Id);
        //    if (sku != null)
        //        return Request.CreateResponse(HttpStatusCode.OK, sku);
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        //}
        ////--------------------------------GetSKUByCategory(GET,[skuCat])-----------------------------------------
        //[HttpGet]
        //public HttpResponseMessage GetCustSKUByCategory(string Customer_SKU_Name)
        //{
        //    var sku = _barcodeService.GetCustSKUByCategory(Customer_SKU_Name);
        //    if (sku != null)
        //        return Request.CreateResponse(HttpStatusCode.OK, sku);
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this Category");
        //}
        ////--------------------------------CreateSKU(POST)---------------------------------------
        //[HttpPost]
        //public ResponseEntity CreateCustSKU([FromBody]CustomerSKUMasterEntity cskuEntity)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    bool responseCode = _barcodeService.CreateCustSKU(cskuEntity);
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
        //[HttpPost]
        //public bool UpdateCustSKU([FromBody]CustomerSKUMasterEntity custskuEntity)
        //{
        //    if (custskuEntity.Customer_SKU_Master_Id > 0)
        //    {
        //        return _barcodeService.UpdateCustSKU(custskuEntity.Customer_SKU_Master_Id, custskuEntity);
        //    }
        //    return false;
        //}
        ////--------------------------------DeleteSKU(POST)---------------------------------------
        //[HttpGet]
        //public bool DeleteCustSKU(int Customer_SKU_Master_Id)
        //{
        //    if (Customer_SKU_Master_Id > 0)
        //        return _barcodeService.DeleteCustSKU(Customer_SKU_Master_Id);
        //    return false;
        //}
        [HttpGet]
        public HttpResponseMessage GetAllPrintBarcode()
        {
            var sku = _barcodeService.GetAllPrintBarcode();
            if (sku != null)
            {
                var SKUEntities = sku as List<PrintedBarcodeDetailsEntity> ?? sku.ToList();
                if (SKUEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, SKUEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        }
        //--------------------------------Get(GET,[id])-----------------------------------------
        [HttpGet]
        public HttpResponseMessage GetPrintBarcodeById(int Printed_Barcode_ID)
        {
            var sku = _barcodeService.GetPrintBarcodeById(Printed_Barcode_ID);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        //--------------------------------GetSKUByCategory(GET,[skuCat])-----------------------------------------
        [HttpGet]
        public HttpResponseMessage GetPrintBarcodeByCategory(string Generated_Bar_Code)
        {
            var sku = _barcodeService.GetPrintBarcodeByCategory(Generated_Bar_Code);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this Category");
        }
        //--------------------------------CreateSKU(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreatePrintBarcode([FromBody]PrintedBarcodeDetailsEntity pbEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _barcodeService.CreatePrintBarcode(pbEntity);
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
        public bool UpdatePrintBarcode([FromBody]PrintedBarcodeDetailsEntity pbEntity)
        {
            if (pbEntity.Printed_Barcode_ID > 0)
            {
                return _barcodeService.UpdatePrintBarcode(pbEntity.Printed_Barcode_ID, pbEntity);
            }
            return false;
        }
        //--------------------------------DeleteSKU(POST)---------------------------------------
        //[HttpGet]
        //public bool DeletePrintBarcode(int Printed_Barcode_ID)
        //{
        //    if (Printed_Barcode_ID > 0)
        //        return _barcodeService.DeleteCustSKU(Printed_Barcode_ID);
        //    return false;
        //}
        ////--------------------------------Get(GET)-----------------------------------------------
        [HttpGet]
        public HttpResponseMessage GetAllLabelField()
        {
            var sku = _barcodeService.GetAllLabelField();
            if (sku != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        }
        //--------------------------------Get(GET,[id])-----------------------------------------
        [HttpGet]
        public HttpResponseMessage GetLabelFieldById(int Field_Id)
        {
            var sku = _barcodeService.GetLabelFieldById(Field_Id);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        //--------------------------------GetSKUByCategory(GET,[skuCat])-----------------------------------------
        [HttpGet]
        public HttpResponseMessage GetLabelFieldByCategory(string Field_Name)
        {
            var sku = _barcodeService.GetLabelFieldByCategory(Field_Name);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Records found for this Category");
        }
        //--------------------------------CreateSKU(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreateLabelField([FromBody]LabelFieldsEntity LFEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _barcodeService.CreateLabelField(LFEntity);
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
        public bool UpdateLabelField([FromBody]LabelFieldsEntity LFEntity)
        {
            if (LFEntity.Field_Id > 0)
            {
                return _barcodeService.UpdateLabelField(LFEntity.Field_Id, LFEntity);
            }
            return false;
        }
        //--------------------------------DeleteSKU(POST)---------------------------------------
        [HttpGet]
        public bool DeleteLabelField(int Field_Id)
        {
            if (Field_Id > 0)
                return _barcodeService.DeleteLabelField(Field_Id);
            return false;
        }
        //--------------------------------Get(GET)-----------------------------------------------
        [HttpGet]
        public HttpResponseMessage GetAllLabelTemplate()
        {
            var sku = _barcodeService.GetAllLabelTemplate();
            if (sku != null)
            {

                return Request.CreateResponse(HttpStatusCode.OK, sku);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
        }
        //--------------------------------Get(GET,[id])-----------------------------------------
        [HttpGet]
        public HttpResponseMessage GetLabelTemplateById(int Cust_Label_Template_Id)
        {
            var sku = _barcodeService.GetLabelTemplateById(Cust_Label_Template_Id);
            if (sku != null)
                return Request.CreateResponse(HttpStatusCode.OK, sku);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found for this id");
        }
        //--------------------------------CreateSKU(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreateLabelTemplate([FromBody]CustomerLabelTemplateEntity CLTEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _barcodeService.CreateLabelTemplate(CLTEntity);
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
        public bool UpdateLabelTemplate([FromBody]CustomerLabelTemplateEntity CLTEntity)
        {
            if (CLTEntity.Template_Id != null)
            {
                return _barcodeService.UpdateLabelTemplate(CLTEntity.Template_Id, CLTEntity);
            }
            return false;
        }
        //--------------------------------DeleteSKU(POST)---------------------------------------


        [HttpGet]
        public bool DeleteLabelTemplateMapping(string Template_Id)
        {
            if (Template_Id != null)
            {
                return _barcodeService.DeleteLabelTemplateMapping(Template_Id);
            }

            return false;

        }

        [HttpGet]
        public bool DeleteLabelTemplate(string Template_Id)
        {
            if (Template_Id != null)
                return _barcodeService.DeleteLabelTemplate(Template_Id);
            return false;
        }

        [HttpPost]
        public ResponseEntity CreateLabelTemplateMapping([FromBody]CustomerLabelTemplateMappingEntity LabelTemplateMappingEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _barcodeService.CreateLabelTemplateMapping(LabelTemplateMappingEntity);
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
        public bool UpdateLabelTemplateMapping([FromBody]CustomerLabelTemplateMappingEntity LabelTemplateMappingEntity)
        {
            if (LabelTemplateMappingEntity.Template_Id != null)
            {
                return _barcodeService.UpdateLabelTemplateMapping(LabelTemplateMappingEntity.Template_Id, LabelTemplateMappingEntity);
            }
            return false;
        }
    }
}