﻿using BusinessServices.Interfaces;
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
    public class CDNGETController : ApiController
    {
        public readonly ICDNGetFromLocal _dataSync;

            #region Public Constructor
            string fileName = "CDN_" + DateTime.Now.ToString("dd-MM-yyyy");
            string Path;

            public CDNGETController()
            {
                if (fileName.Contains("-"))
                    fileName = fileName.Replace("-", "_");

                Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

                if (!File.Exists(Path))
                {
                    File.Create(Path).Close();
                }
                _dataSync = new CDNGetFromLocal();
            }

            [HttpGet]
            public HttpResponseMessage getCDN()
            {
                try
                {
                    _dataSync.CDNGet();
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
                return Request.CreateResponse(HttpStatusCode.OK,"Data Fetched Successfully");
            }

            [HttpGet]
            public HttpResponseMessage updateCDN()
            {
                try
                {
                    _dataSync.CDN_Update();
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

            [HttpGet]
            public HttpResponseMessage setCDN()
            {
                try
                {
                    _dataSync.CDNSet();
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
