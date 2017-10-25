using BusinessEntities;
using BusinessServices.Interfaces;
using BusinessServices.Services.Dispatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers.Dispatch
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockConvertionController : ApiController
    {
        private readonly IStockConvertionServices _stockServices;

        #region Public Constructor
        public StockConvertionController()
        {
            _stockServices = new StockConvertionServices();
        }

        [HttpGet]
        public HttpResponseMessage getStocks(DateTime date, String dc_Code)
        {
            try
            {
                List<StockEntity> response = _stockServices.getStocks(date, dc_Code);
                return Request.CreateResponse(response);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(ex.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage getDatewiseStocks(DateTime date, String dc_Code)
        {
            try
            {

                List<StockEntity> response = _stockServices.getDatewiseStocks(date, dc_Code);
                return Request.CreateResponse(response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message.ToString());
            }
        }

      
        [HttpGet]
        public HttpResponseMessage getConvertedStocks(DateTime date, int dc_id)
        {
            try
            {
                var response = _stockServices.StockConvertedSummary(dc_id, date);
                return Request.CreateResponse(response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message.ToString());
            }
        }

        [HttpPost]
        public HttpResponseMessage ConvertStock(StockEntity AStocks)
        {
            try
            {
                bool response = _stockServices.Convert_Stock(AStocks);
                if (response)
                {
                    return Request.CreateResponse("Successfully Stock Converted");
                }
                else
                {
                    return Request.CreateResponse("Failed To Stock Converted");
                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message.ToString());
            }
        }

        #endregion
    }
}
