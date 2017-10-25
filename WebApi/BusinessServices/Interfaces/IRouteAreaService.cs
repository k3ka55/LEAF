using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface  IRouteAreaService
    {
        IEnumerable<RoutesAreaList> GetAllRoutes(string Target_Location_Code, string Target_Location_Type);
        dynamic GetAllRouteAreas(int? roleId, string Url);
        bool Delete(string Location_Code);
        bool Update(string Location_Code, RouteAreaEntity routeEntity);
        RouteAreaEntity Get(string Location_Code);
        bool InsertRoute(RouteAreaEntity RoutMast);
      //  bool InsertRoute(RouteMasterEntity RoutMast);
    }
}
