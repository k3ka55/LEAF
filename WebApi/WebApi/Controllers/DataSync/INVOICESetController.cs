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
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class INVOICESetController : ApiController
    {
         public readonly IINVOICEASetToLive _dataSync;

            #region Public Constructor
             string fileName = "INVOICE_" + DateTime.Now.ToString("dd-MM-yyyy");
             string Path;

            public INVOICESetController()
            {
                if (fileName.Contains("-"))
                    fileName = fileName.Replace("-", "_");

                Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

                if (!File.Exists(Path))
                {
                    File.Create(Path).Close();
                }
                _dataSync = new INVOICESetToLive();
            }

            [HttpGet]
            public HttpResponseMessage setINVOICE()
            {
                try
                {
                    _dataSync.setInvoice();
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
