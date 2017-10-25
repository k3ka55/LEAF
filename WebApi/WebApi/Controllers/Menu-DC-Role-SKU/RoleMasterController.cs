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
    public class RoleMasterController : ApiController
    {
        private readonly IRoleMaster _roleServices;

        #region Public Constructor


        public RoleMasterController()
        {
            _roleServices = new RoleMasterServices();
        }
        //--------------------------------Get(GET)---------------------------------------
        [HttpPost]
        public string AddRoleMaster(RoleMasterEntity RoleMasterModel)
        {
            return _roleServices.AddRoleMaster(RoleMasterModel);
        }

        [HttpGet]

        public List<RoleList> GetRoleMasterList(int? roleId, string Url = "null")
        {
            return _roleServices.getRoleMasterList(roleId, Url);
        }


        [HttpPost]

        public string UpdateRoleMasterList(RoleId RoleId)
        {
            return _roleServices.updateRoleMasterList(RoleId);
        }


        [HttpPost]

        public string DeleteRoleMasterList(RoleId RoleId)
        {
            return _roleServices.deleteRoleMaster(RoleId);
        }


        // [HttpPost]
        //public List<MenuDetailsR> RoleBasedMenu()
        //{
        //    return _roleMasterServices.roleBasedMenu();
        //}

        [HttpPost]
        public List<MenuDetailsR> RoleBasedMenu(MenuRoleId MenuRoleId)
        {
            return _roleServices.roleBasedMenu(MenuRoleId);
        }


        [HttpPost]
        public List<MenuDetailsR> RoleBasedMenuNew()
        {
            return _roleServices.newRoleBasedMenu();
        }

        [HttpPost]
        public List<MenuDetailsP> Menupermissions()
        {
            return _roleServices.Menupermissions();
        }
    }
}
        #endregion
