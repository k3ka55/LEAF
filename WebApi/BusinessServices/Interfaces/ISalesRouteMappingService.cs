using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface  ISalesRouteMappingService
    {
        dynamic GetAllSaleRouteMapping(int? roleId, string Url);
        bool InsertSaleRouteMapping(SalesRoutemappingEntity RoutMast);
        bool DeleteSaleRouteMapping(int Sales_Person_Id);
        SalesRoutemappingEntity Get(int Sales_Person_Id);
        dynamic GetSalesPersons(string Route_Code);
        bool UpdateSaleRouteMapping(int? Sales_Person_Id, SalesRoutemappingEntity routeEntity);
    }
}
