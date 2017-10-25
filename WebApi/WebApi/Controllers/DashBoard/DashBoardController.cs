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
    public class DashBoardController : ApiController
    {
            private readonly IDashBoardServices _dashboardServices;
        #region Public Constructor
            public DashBoardController()
        {
            _dashboardServices = new DashBoardServices();
        }
             
            [HttpGet]
            public List<DCDASHBOARD_CSISTIFORGRAPHEntity> GetDCDASHBOARD_CSISTIFORGRAPH(string DC_Code)
            {
                return _dashboardServices.GetDCDASHBOARD_CSISTIFORGRAPH(DC_Code);
            }

     

        [HttpGet]
            public HttpResponseMessage GetDCDashBoard(string DC_Code)
        {
            var user = _dashboardServices.GetDCDashBoard(DC_Code);
            DashBoardEntity f = new DashBoardEntity();
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

           #endregion
    }
}
