using BusinessEntities;
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
   public class INVCollectionController : ApiController
    {
         private readonly IINVCashServices _dispatchServices;

        #region Public Constructor
         public INVCollectionController()
        {
            _dispatchServices = new INVCashServices();
        }

         
        [HttpGet]
         public HttpResponseMessage Get(DateTime? From, DateTime? To, string DC_Code = "null")
        {
            List<INVCashEntity> f = new List<INVCashEntity>();

            if (From != null && To != null && DC_Code != null)
            {
                var result = _dispatchServices.Search(From, To, DC_Code);
                f = result;

                return Request.CreateResponse(f);
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Input not valid");
            }
        }
    }
}
        #endregion