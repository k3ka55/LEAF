using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IRoleService
    {
        RoleEntity GetroleById(int roleId);
        List<RoleEntity> GetAllRole();
        int CreateRole(RoleEntity roleEntity);
        bool UpdateRole(int roleId, RoleEntity roleEntity);
        bool DeleteRole(int roleId);
    }
}
