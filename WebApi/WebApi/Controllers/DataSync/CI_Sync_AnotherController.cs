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
    public class CI_Sync_AnotherController : ApiController
    {
        public readonly ICI_Sync_Another _dataSync;

        #region Public Constructor
        string fileName = "CI_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;

        public CI_Sync_AnotherController()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }
            _dataSync = new CI_Sync_Another();
        }

        [HttpGet]
        public HttpResponseMessage getCI()
        {
            try
            {
                _dataSync.getCI();
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
        public HttpResponseMessage setCI()
        {
            try
            {
                _dataSync.CISet();
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
        public HttpResponseMessage updateCI()
        {
            try
            {
                _dataSync.CI_Update();
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
