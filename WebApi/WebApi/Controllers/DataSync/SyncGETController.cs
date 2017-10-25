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
    public class SyncGETController : ApiController
    {
        public readonly ISyncCheckLocal _dataSync;

        #region Public Constructor
            string fileName = "SCL_" + DateTime.Now.ToString("dd-MM-yyyy");
            string Path;

            public SyncGETController()
            {
                if (fileName.Contains("-"))
                    fileName = fileName.Replace("-", "_");

                Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

                if (!File.Exists(Path))
                {
                    File.Create(Path).Close();
                }
                _dataSync = new SyuncCheckLocal();
            }

            [HttpGet]
            public HttpResponseMessage getSync()
            {
                try
                {
                    _dataSync.getSyncDetails();
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
                return Request.CreateResponse(HttpStatusCode.OK, "Data Fetched Successfully");
            }

            [HttpGet]
            public HttpResponseMessage setSync()
            {
                try
                {
                    _dataSync.setSyncDetails();
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
                return Request.CreateResponse(HttpStatusCode.OK, "Data Inserted Successfully");
            }

            [HttpGet]
            public HttpResponseMessage updateSync()
            {
                try
                {
                    _dataSync.Sync_Update();
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
                return Request.CreateResponse(HttpStatusCode.OK, "Data Updated Successfully");
            }

        #endregion
    }
}
