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
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class RoleController : ApiController
    {
        private readonly IRoleService _userroleServices;

        #region Public Constructor
        public RoleController()
        {
            _userroleServices = new RoleService();
        }

        #endregion


        public HttpResponseMessage Get()
        {
            var role = _userroleServices.GetAllRole();
            if (role != null)
            {
                var roleEntities = role as List<RoleEntity> ?? role.ToList();
                if (roleEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, roleEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Role not found");
        }


        public HttpResponseMessage Get(int id)
        {
            var role = _userroleServices.GetroleById(id);
            if (role != null)
                return Request.CreateResponse(HttpStatusCode.OK, role);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No role found for this id");
        }


        [HttpPost]
        public int CreateRole([FromBody] RoleEntity roleEntity)
        {
            return _userroleServices.CreateRole(roleEntity);
        }


        [HttpPost]
        public bool UpdateRole([FromBody]RoleEntity roleEntity)
        {
            if (roleEntity.Role_Id > 0)
            {
                return _userroleServices.UpdateRole(roleEntity.Role_Id, roleEntity);
            }
            return false;
        }


        [HttpPost]
        public bool DeleteRole(int id)
        {
            if (id > 0)
                return _userroleServices.DeleteRole(id);
            return false;
        }
    }
}
