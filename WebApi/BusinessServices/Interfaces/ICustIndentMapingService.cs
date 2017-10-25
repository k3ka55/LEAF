using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface ICustIndentMapingService
    {
        bool CreateCustomerIndentMapping(Customer_CustomerIndetMap_Entity intendEntity);
        bool UpdateCustomerIndentMapping(int? cId, Customer_CustomerIndetMap_Entity intendEntity);
        List<Customer_CustomerIndetMap_Entity> getIndentMapping();
        Customer_CustomerIndetMap_Entity getSingleIndentMapping(int id);
        bool deleteMapping(int id);
        //List<CustomerIndents> getIndents(int id);
        List<Customer_CustomerIndetMap_Entity> searchMappingIndent(int? roleId, int regionid, int locationid, int dcid, string Url);
        List<Customer_CustomerIndetMap_Entity> searchMapping(int? roleId, string region, string location, string dccode, string Url);
    }
}
