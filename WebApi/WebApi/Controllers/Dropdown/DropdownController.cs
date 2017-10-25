using BusinessEntities;
using BusinessEntities.Entity;
using BusinessServices;
using BusinessServices.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DropdownController : ApiController
    {
        private readonly IDropdownService _dropdownServices;
        #region Public Constructor
        public DropdownController()
        {
            _dropdownServices = new DropdownService();
        }
         [HttpGet]
        public string MS()
        {
            //Set parameters
            string username = "k3ka55@gmail.com";
            string password = "jp3mi";
            string msgsender = "9003799648";
            string destinationaddr = "9003799648";
            string message = "SMS CHECK";
            string output = "SM Failed";

            // Create ViaNettSMS object with username and password
            SMSService s = new SMSService(username, password);
            // Declare Result object returned by the SendSMS function
            SMSService.Result result;
            try
            {
                // Send SMS through HTTP API
                result = s.sendSMS(msgsender, destinationaddr, message);
                // Show Send SMS response
                if (result.Success)
                {
                   output="Message successfully sent";
                   return output;
                }
                else
                {
                    output="Received error: " + result.ErrorCode + " " + result.ErrorMessage;
                    return output;
                }
               
            }
            catch (System.Net.WebException ex)
            {
                //Catch error occurred while connecting to server.
                output=ex.Message;
                return output;
            }
          
        }
        [HttpGet]
        public IEnumerable<Template_Type> GetTemplateType()
        {

            return _dropdownServices.GetTemplateType();
        }
             [HttpGet]
        public IEnumerable<Tally_Activity> GetTallyDD()
        {

            return _dropdownServices.GetTallyDD();
        }

             [HttpGet]
             public List<DCMasterModel> GetDC()
             {
                 return _dropdownServices.GetDC();
             }
         [HttpGet]
             public List<Vehicle_No> GetVehicleNos()
             {
                 return _dropdownServices.GetVehicleNos();
             }
      

             [HttpGet]
             public DCLocationModel GetLocDCCombine()
             {
                 return _dropdownServices.GetLocDCCombine();
             }



             [HttpGet]
             public IEnumerable<Tally_Module> GetTallyModuleByName(string Tally_Module_Activity)
             {

                 return _dropdownServices.GetTallyModuleByName(Tally_Module_Activity);
             }

        [HttpPost]
        public string validateUserName(UserDetailsEntity valUsr)
        {

            return _dropdownServices.validateUserName(valUsr);
        }
        [HttpPost]
        public string validateSKUName(SKUEntity valSKU)
        {

            return _dropdownServices.validateSKUName(valSKU);
        }
        [HttpPost]
        public string validateRoleName(RoleEntity valRl)
        {

            return _dropdownServices.validateRoleName(valRl);
        }
        [HttpPost]
        public string validateDCName(DCMasterEntity valDC)
        {

            return _dropdownServices.validateDCName(valDC);
        }
        [HttpPost]
        public string validateMaterialCode(Material_MasterEntity valma)
        {

            return _dropdownServices.validateMaterialCode(valma);
        }
        //string ( )
        [HttpPost]
        public string validateLocationName(LocationMasterEntity valLoc)
        {

            return _dropdownServices.validateLocationName(valLoc);
        }
        [HttpPost]
        public string validateRegionName(RegionMasterEntity valReg)
        {

            return _dropdownServices.validateRegionName(valReg);
        }
        [HttpPost]
        public string validateRouteMaster(RouteMasterEntity valroute)
        {

            return _dropdownServices.validateRouteMaster(valroute);
        }


        //--------------------------------Get(GET)---------------------------------------
        //[HttpGet]
        //public HttpResponseMessage GetPackSize(string packType)
        //{
        //    var result = _dropdownServices.GetPackSize(packType);
        //    List<Pack_Size> f = new List<Pack_Size>();
        //    if (result != null)
        //    {
        //        f = result;
        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "NO Data Found");
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage GetPackSizeA(string packType)
        //{
        //    var result = _dropdownServices.GetPackSizeA(packType);
        //    PackSizeEntity f = new PackSizeEntity();

        //    if (result != null)
        //    {
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "NO Data Found");
        //    }
        //}

        [HttpGet]
        public PincodeEntity.PinModule GetPincode(int id)
        {
            var user = _dropdownServices.GetPincode(id);
            PincodeEntity.PinModule f = new PincodeEntity.PinModule();
            f.Address = user;
            f.StatusCode = HttpStatusCode.OK;
            return f;
        }
         
   [HttpGet]
        public HttpResponseMessage GetCustRegDD()
        {
            var user = _dropdownServices.GetCustRegDD();
            CustRegDDEntity f = new CustRegDDEntity();
            if (user != null)
            {
                f = user;
                f.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                f.StatusCode = HttpStatusCode.NotFound;
            }
            return Request.CreateResponse(f);
        }
        [HttpGet]
        public HttpResponseMessage GetCustSuppDD()
        {
            var user = _dropdownServices.GetCustSuppDD();
            CustSuppDDEntity f = new CustSuppDDEntity();
            if (user != null)
            {
                f = user;
                f.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                f.StatusCode = HttpStatusCode.NotFound;
            }
            return Request.CreateResponse(f);
        }
          
        [HttpGet]
        public HttpResponseMessage getDCforSTI(string ULocation)
        {
            List<DCMasterModel> f = new List<DCMasterModel>();
            //if (ULocation == "JDM")
            //{
                var qr = _dropdownServices.getDCforSTIJDM(ULocation);
                f = qr;
            //}
            //else
            //{
            //    var qr = _dropdownServices.getDCforSTINJDM(ULocation);
            //    f = qr;
            //}
            return Request.CreateResponse(f);
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var user = _dropdownServices.GetDropdowns();
            DropdownEntity f = new DropdownEntity();
            if (user != null)
            {
                f = user;
                f.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                f.StatusCode = HttpStatusCode.NotFound;
            }
            return Request.CreateResponse(f);
        }
       
        //    [HttpGet]
        //public dynamic GetSKUs()
        //{
        //  //  var user = _dropdownServices.GetSKUs();
        //    return _dropdownServices.GetSKUs();
        //  //  BothSKUEntity f = new BothSKUEntity();
        //    //if (user != null)
        //    //{
        //    //    return Request.CreateResponse(user);
        //    //    //f = user;
        //    //    //f.StatusCode = HttpStatusCode.OK;
        //    //}
        //    //else
        //    //{
        //    //    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "NO Data Found");
        //    //  // f.StatusCode = HttpStatusCode.NotFound;
        //    //}
        //    //return Request.CreateResponse(f);
        //}
              //--------------------------------Get(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage GetByUserLocation(int userId)
        {
            if (userId > 0)
            {
                var user = _dropdownServices.getLocation(userId);
                return Request.CreateResponse(user);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "NO Data Found");
            }
        }
        //-----------------------------------------------GETDISPATCHTYPES-------------------------
        [HttpGet]
        public List<Tuple<string>> getDispatchTypes()
        {
            return _dropdownServices.getDispatchTypes();
        }

        [HttpGet]
        public List<Tuple<string, string>> getSTI_Type()
        {
            return _dropdownServices.getSTI_Type();
        }

        [HttpGet]
        public List<Tuple<string>> getSkuTypes()
        {
            return _dropdownServices.getSkuTypes();
        }

        [HttpGet]
        public List<Tuple<string>> getMaterialResource()
        {
            return _dropdownServices.getMaterialResource();
        }
         //-----------------------------------------------GETINVOICETYPES-------------------------
        [HttpGet]
        public List<Tuple<string>> getInvoiceTypes()
        {
            return _dropdownServices.getInvoiceTypes();
        }
        //-----------------------------------------------GETSKUCATEGORIES-------------------------
        [HttpGet]
        public List<Tuple<string>> getSkuCategories()
        {
            return _dropdownServices.getSkuCategories();
        }
          //-----------------------------------------------GETPACKTYPES-------------------------
        [HttpGet]
        public List<Tuple<string>> getPackTypes()
        {
            return _dropdownServices.getPackTypes();
        }
     //-----------------------------------------------GETPACKTYPES-------------------------
        [HttpGet]
        public List<string> getLocations()
        {
            return _dropdownServices.getLocations();
        }
        #endregion
    }
}
