using BusinessEntities;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface ICustomerServices
    {
        // CustomerEntity GetcustomerById(int customerId);
        List<CustomerEntity> GetAllCustomer(int? roleId, string Url);
        List<CustomerEntity> GetcustomerById(int customerId);
        bool CreateCustomer(CustomerEntity customerEntity);
       
        bool UpdateCustomer(int customerId, CustomerEntity customerEntity);
        bool DeleteCustomer(int customerId);
        List<Customer_Model> DispatchGetAllCustomer();
        List<CustomerEntity> searchCustomers(string location);
        //DCCustomer_Mapping getCustomerDCinfo(string dcCode);
    }
}
