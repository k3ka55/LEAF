using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface ICustomer_Indent_MappingServices
    {
        bool CreateCustomerIndentMapping(Customer_Indent_MappingEntity intendEntity);
        bool UpdateCustomerIndentMapping(int? cId, Customer_Indent_MappingEntity intendEntity);
       // List<Customer_Indent_MappingEntity> getIndentMapping();
        Customer_Indent_MappingEntity getSingleIndentMapping(int id);
        bool deleteMapping(int id);
        //List<CustomerIndents> getIndents(int id);
      //  List<Customer_Indent_MappingEntity> searchMappingIndent(int? roleId, int regionid, int locationid, int dcid, string Url);
        List<Customer_Indent_MappingEntity> searchCustomerIndentMapping(int? roleId, string region, string location, string dccode, string Url);
    }
}
