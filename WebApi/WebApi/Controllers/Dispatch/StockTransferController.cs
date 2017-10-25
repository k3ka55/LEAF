//using BusinessEntities;
//using BusinessServices;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Cors;

//namespace WebApi.Controllers
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*")]
//    public class StockTransferController : ApiController
//    {
//        private readonly IStockTransferServices _stockServices;

//        #region Public Constructor
//        public StockTransferController()
//        {
//            _stockServices = new StockTransferServices();
//        }
//        #endregion



//        [HttpGet]
//        public HttpResponseMessage StockTransferCall()
//        {
//            bool result = _stockServices.MoveStock();
//            if(result)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.Created, "True");
//            }
//            else
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Flase");
//            }
//        }
//        //--------------------------------SearchGRN(GET)-----------------------------------------
//        [HttpGet]
//        public HttpResponseMessage SearchSK(DateTime? startDate, DateTime? endDate, string Ulocation = "null")
//        {
//            List<StockEntity> f = new List<StockEntity>();

//            if ((startDate != null && endDate != null) && Ulocation != "null")
//            {
//                var result = _stockServices.GetSK(startDate, endDate, Ulocation);
//                f = result;

//                return Request.CreateResponse(HttpStatusCode.Found,f);
//            }         
//            else
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
//            }
//        }
//    }
//}
