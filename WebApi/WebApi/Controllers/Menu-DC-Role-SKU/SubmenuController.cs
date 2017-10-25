//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using BusinessEntities;
//using BusinessServices;
//using System.Web.Http.Cors;

//namespace WebApi.Controllers
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*")]
//    public class SubmenuController : ApiController
//    {
//        private readonly ISubmenuService _submenuServices;

//         #region Public Constructor
//        public SubmenuController()
//        {
//            _submenuServices =new SubmenuService();
//        }

//        #endregion


//        public HttpResponseMessage Get()
//        {
//            var submenu = _submenuServices.GetAllSubmenu();
//            if (submenu != null)
//            {
//                var SubmenuEntities = submenu as List<SubmenuEntity> ?? submenu.ToList();
//                if (SubmenuEntities.Any())
//                    return Request.CreateResponse(HttpStatusCode.OK, SubmenuEntities);
//            }
//            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Submenu not found");
//        }


//        public HttpResponseMessage Get(int id)
//        {
//            var submenu = _submenuServices.GetsubmenuById(id);
//            if (submenu != null)
//                return Request.CreateResponse(HttpStatusCode.OK, submenu);
//            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No submenu found for this id");
//        }

//        [HttpPost]
//        public int Post([FromBody] SubmenuEntity submenuEntity)
//        {
//            return _submenuServices.CreateSubmenu(submenuEntity);
//        }

//        [HttpPost]
//        public bool UpdateSubMenu([FromBody]SubmenuEntity submenuEntity)
//        {
//            if (submenuEntity.Sub_Id > 0)
//            {
//                return _submenuServices.UpdateSubmenu(submenuEntity.Sub_Id, submenuEntity);
//            }
//            return false;
//        }

//        [HttpPost]
//        public bool DeleteSubMenu(int id)
//        {
//            if (id > 0)
//                return _submenuServices.DeleteSubmenu(id);
//            return false;
//        }
//    }
//    }

