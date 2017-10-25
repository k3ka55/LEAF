using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IRoleMaster
    {
        string AddRoleMaster(RoleMasterEntity RoleMasterModel);
        List<RoleList> getRoleMasterList(int? roleId, string Url);

        string updateRoleMasterList(RoleId RoleId);

        string deleteRoleMaster(RoleId RoleId);

        //List<MenuDetailsR> roleBasedMenu();
        List<MenuDetailsR> roleBasedMenu(MenuRoleId MenuRoleId);

        List<MenuDetailsR> newRoleBasedMenu();
        List<MenuDetailsP> Menupermissions();
    }
}
