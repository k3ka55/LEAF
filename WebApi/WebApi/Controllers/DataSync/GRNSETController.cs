using BusinessServices.Interfaces;
using BusinessServices.Services.DataSync;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers.DataSync
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GRNSETController : ApiController
    {
        public readonly IGRNSetToLive _dataSync;

            #region Public Constructor
            string fileName = "GRN_" + DateTime.Now.ToString("dd-MM-yyyy");
            string Path;

            public GRNSETController()
            {
                if (fileName.Contains("-"))
                    fileName = fileName.Replace("-", "_");

                Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

                if (!File.Exists(Path))
                {
                    File.Create(Path).Close();
                }
                _dataSync = new GRNSetToLive();
            }

            [HttpGet]
            public HttpResponseMessage setGRN()
            {
                try
                {
                    _dataSync.setGRN();
                }
                catch (Exception ex)
                {
                    using (var tw = new StreamWriter(Path, true))
                    {
                        tw.WriteLine(ex.Message.ToString() + " --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                        tw.Close();
                    }
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
                }
                return Request.CreateResponse(HttpStatusCode.OK,"Data Updated Successfully");
            }
            #endregion
    }
}
