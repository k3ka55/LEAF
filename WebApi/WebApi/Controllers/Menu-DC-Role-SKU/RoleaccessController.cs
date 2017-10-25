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
//    public class RoleaccessController : ApiController
//    {
//        private readonly IRoleaccessService _roleaccessServices;

//         #region Public Constructor
//           public RoleaccessController()
//        {
//            _roleaccessServices = new RoleaccessService();
//        }

//        #endregion


//        public HttpResponseMessage Get()
//        {
//            var roleaccess = _roleaccessServices.GetAllRoleaccess();
//            if (roleaccess != null)
//            {
//                var RoleaccessEntities = roleaccess as List<RoleaccessEntity> ?? roleaccess.ToList();
//                if (RoleaccessEntities.Any())
//                    return Request.CreateResponse(HttpStatusCode.OK, RoleaccessEntities);
//            }
//            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Roleaccess not found");
//        }


//        public HttpResponseMessage Get(int id)
//        {
//            var roleaccess = _roleaccessServices.GetroleaccessById(id);
//            if (roleaccess != null)
//                return Request.CreateResponse(HttpStatusCode.OK, roleaccess);
//            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No roleaccess found for this id");
//        }

//        [HttpPost]
//        public int Post([FromBody] RoleaccessEntity roleaccessEntity)
//        {
//            return _roleaccessServices.CreateRoleaccess(roleaccessEntity);
//        }

//        [HttpPost]
//        public bool UpdateRoleAccess([FromBody]RoleaccessEntity roleaccessEntity)
//        {
//            if (roleaccessEntity.Role_Access_Id > 0)
//            {
//                return _roleaccessServices.UpdateRoleaccess(roleaccessEntity.Role_Access_Id, roleaccessEntity);
//            }
//            return false;
//        }

//        [HttpPost]
//        public bool DeleteRoleAccess(int id)
//        {
//            if (id > 0)
//                return _roleaccessServices.DeleteRoleaccess(id);
//            return false;
//        }
//    }
//}
