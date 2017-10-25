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
    public class CustomerController : ApiController
    {
        private readonly ICustomerServices _customerServices;

        #region Public Constructor
        public CustomerController()
        {
            _customerServices = new CustomerServices();
        }

        #endregion
        //--------------------------------Get(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var customer = _customerServices.DispatchGetAllCustomer();
            if (customer != null)
            {
                //var CustomerEntities = customer as List<CustomerEntity> ?? customer.ToList();
                // if (CustomerEntities.Any())
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
        }

        [HttpGet]
        public HttpResponseMessage GetALLCust(int? roleId, string Url)
        {
            var customer = _customerServices.GetAllCustomer(roleId, Url);
            if (customer != null)
            {
                //var CustomerEntities = customer as List<CustomerEntity> ?? customer.ToList();
                // if (CustomerEntities.Any())
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
        }
        //--------------------------------GetCustomerDC(GET)---------------------------------------
        //[HttpGet]
        //public HttpResponseMessage GetCustomerDC(string dcCode)
        //{
        //    var customer = _customerServices.getCustomerDCinfo(dcCode);
        //    if (customer != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, customer);
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
        //}

        [HttpGet]
        public HttpResponseMessage searchCustomers(string location)
        {
            var cust = _customerServices.searchCustomers(location);

            return Request.CreateResponse(cust);
        }
        //--------------------------------Get(GET,[id])---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var customer = _customerServices.GetcustomerById(id);
            if (customer != null)
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Customer found for this id");
        }
        //--------------------------------CreateCustomer(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity CreateCustomer([FromBody] CustomerEntity customerEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _customerServices.CreateCustomer(customerEntity);
            if (responseCode)
            {
                response.statusCode = HttpStatusCode.OK;
                response.message = "Inserted Successfully";
            }
            else
            {
                response.statusCode = HttpStatusCode.InternalServerError;
                response.message = "Inserted UnSuccessfully";
            }
            return response;
        }
        //--------------------------------UpdateCustomer(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity UpdateCustomer([FromBody]CustomerEntity customerEntity)
        {
            ResponseEntity response = new ResponseEntity();
            if (customerEntity.Cust_Id > 0)
            {
                bool responseCode = _customerServices.UpdateCustomer(customerEntity.Cust_Id, customerEntity);
                if (responseCode)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = "Updated Successfully";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Updated UnSuccessfully";
                }
            }
            else
            {
                response.statusCode = HttpStatusCode.NotFound;
                response.message = "Invalied Id";
            }
            return response;
        }
        //--------------------------------DeleteCustomer(POST)---------------------------------------
        [HttpPost]
        public ResponseEntity DeleteCustomer(int id)
        {
            ResponseEntity response = new ResponseEntity();
            if (id > 0)
            {
                bool responseCode = _customerServices.DeleteCustomer(id);
                if (responseCode)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = "Deleted Successfully";
                }
                else
                {
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = "Deleted UnSuccessfully";
                }
            }
            else
            {
                response.statusCode = HttpStatusCode.NotFound;
                response.message = "Invalied Id";
            }
            return response;
        }
    }
}
