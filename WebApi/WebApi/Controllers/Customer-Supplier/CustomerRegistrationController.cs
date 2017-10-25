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
    public class CustomerRegistrationController : ApiController
    {
        private readonly ICustomerRegistrationServices _customerRegServices;

        #region Public Constructor
        public CustomerRegistrationController()
        {
            _customerRegServices = new CustomerRegistrationServices();
        }
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var customer = _customerRegServices.GetcustomerById(id);
            if (customer != null)
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Customer found for this id");
        }
        [HttpGet]
        public ResponseEntity Delete(int id,string deletedby,string Reason)
        {
            ResponseEntity response = new ResponseEntity();
            if (id > 0)
            {
                bool responseCode = _customerRegServices.DeleteCustomer(id, deletedby, Reason);
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
        #endregion
        //--------------------------------Get(GET)---------------------------------------
        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    var customer = _customerRegServices.DispatchGetAllCustomer();
        //    if (customer != null)
        //    {
        //        //var CustomerEntities = customer as List<CustomerEntity> ?? customer.ToList();
        //        // if (CustomerEntities.Any())
        //        return Request.CreateResponse(HttpStatusCode.OK, customer);
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
        //}

        //[HttpGet]
        //public HttpResponseMessage GetALLCust(int? roleId, string Url)
        //{
        //    var customer = _customerRegServices.GetAllCustomer(roleId, Url);
        //    if (customer != null)
        //    {
        //        //var CustomerEntities = customer as List<CustomerEntity> ?? customer.ToList();
        //        // if (CustomerEntities.Any())
        //        return Request.CreateResponse(HttpStatusCode.OK, customer);
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
        //}

        [HttpGet]
        public HttpResponseMessage searchCustomers(int roleId, string location = "null", string Customer_Name = "null", string CreatedBy = "null", string Url="null")
        {
            var cust = _customerRegServices.searchCustomers(roleId,location, Customer_Name, CreatedBy, Url);

            return Request.CreateResponse(cust);
        }
        ////--------------------------------Get(GET,[id])---------------------------------------
        //[HttpGet]
        //public HttpResponseMessage Get(int id)
        //{
        //    var customer = _customerRegServices.GetcustomerById(id);
        //    if (customer != null)
        //        return Request.CreateResponse(HttpStatusCode.OK, customer);
        //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Customer found for this id");
        //}
        //--------------------------------CreateCustomer(POST)---------------------------------------
       
              [HttpPost]
        public ResponseEntity RegCustomerApprove([FromBody] CustomerEntity customerEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _customerRegServices.RegCustomerApprove(customerEntity);
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
        [HttpPost]
        public ResponseEntity RegisterCustomer([FromBody] CustomerRegistrationEntity customerEntity)
        {
            ResponseEntity response = new ResponseEntity();
            bool responseCode = _customerRegServices.RegisterCustomer(customerEntity);
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
        //[HttpPost]
        //public ResponseEntity UpdateCustomer([FromBody]CustomerEntity customerEntity)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    if (customerEntity.Cust_Id > 0)
        //    {
        //        bool responseCode = _customerRegServices.UpdateCustomer(customerEntity.Cust_Id, customerEntity);
        //        if (responseCode)
        //        {
        //            response.statusCode = HttpStatusCode.OK;
        //            response.message = "Updated Successfully";
        //        }
        //        else
        //        {
        //            response.statusCode = HttpStatusCode.InternalServerError;
        //            response.message = "Updated UnSuccessfully";
        //        }
        //    }
        //    else
        //    {
        //        response.statusCode = HttpStatusCode.NotFound;
        //        response.message = "Invalied Id";
        //    }
        //    return response;
        //}
        ////--------------------------------DeleteCustomer(POST)---------------------------------------
        //[HttpPost]
        //public ResponseEntity DeleteCustomer(int id)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    if (id > 0)
        //    {
        //        bool responseCode = _customerRegServices.DeleteCustomer(id);
        //        if (responseCode)
        //        {
        //            response.statusCode = HttpStatusCode.OK;
        //            response.message = "Deleted Successfully";
        //        }
        //        else
        //        {
        //            response.statusCode = HttpStatusCode.InternalServerError;
        //            response.message = "Deleted UnSuccessfully";
        //        }
        //    }
        //    else
        //    {
        //        response.statusCode = HttpStatusCode.NotFound;
        //        response.message = "Invalied Id";
        //    }
        //    return response;
        //}
    }
}

