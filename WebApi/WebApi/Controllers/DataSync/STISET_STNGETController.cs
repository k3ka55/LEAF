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
    public class STISET_STNGETController : ApiController
    {
        private readonly ISTISET_STNGET _dataSync;
        #region Public Constructor
        string fileName = "STI_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;

        public STISET_STNGETController()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }
            _dataSync = new STI_Push();
        }
        
        [HttpGet]
        public HttpResponseMessage STISET()
        {
            try
            {
                _dataSync.STISet();
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
            return Request.CreateResponse(HttpStatusCode.OK,"Data Pushed Successfully");
        }
       
        [HttpGet]
        public HttpResponseMessage STNGET()
        {
            try
            {
                _dataSync.STNGet();
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
            return Request.CreateResponse(HttpStatusCode.OK,"Data Pulled Successfully");
        }

        [HttpGet]
        public HttpResponseMessage STNUPDATE()
        {
            try
            {
                _dataSync.STN_Update();
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
        public HttpResponseMessage PushSTIDatas()
        {
            try
            {
                _dataSync.STIGet();
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
            return Request.CreateResponse(HttpStatusCode.OK,"Data Pushed Successfully");
        }

        [HttpGet]
        public HttpResponseMessage PullSTNDatas()
        {
            try
            {
                _dataSync.STNSet();
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
            return Request.CreateResponse(HttpStatusCode.OK,"Data Pulled Successfully");
        }

        [HttpGet]
        public HttpResponseMessage UpdateSTI()
        {
            try
            {
                _dataSync.STI_Update_Local();
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
            return Request.CreateResponse(HttpStatusCode.OK,"Data Pulled Successfully");
        }

        #endregion
    }
}
