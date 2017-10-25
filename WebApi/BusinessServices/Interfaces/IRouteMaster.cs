using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
   public interface IRouteMaster
    {
       dynamic GetAll();
       dynamic GetAllRoutes(int? roleId, string Url);
       bool InsertRoute(RouteMasterEntity RoutMast);
       bool DeleteRoute(string Route_Code);
       dynamic GetAllForMapping();
       RouteMasterEntity Get(string Route_Code);
       bool Update(string Route_Code, RouteMasterEntity routeEntity);
    }
}
