using BusinessEntities;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
   public interface ICustomerRegistrationServices
    {
        //List<CustomerEntity> GetAllCustomer(int? roleId, string Url);
        //List<CustomerEntity> GetcustomerById(int customerId);
       bool RegCustomerApprove(CustomerEntity customerEntity);
       bool RegisterCustomer(CustomerRegistrationEntity customerEntity);
       CustomerRegistrationEntity GetcustomerById(int customerId);
       bool DeleteCustomer(int customerId,string deletedby,string Reason);
        //bool UpdateCustomer(int customerId, CustomerEntity customerEntity);
        //bool DeleteCustomer(int customerId);
        //List<Customer_Model> DispatchGetAllCustomer();
       List<CustomerRegistrationEntity> searchCustomers(int roleId, string location, string Customer_Name, string CreatedBy, string Url);
    }
}
