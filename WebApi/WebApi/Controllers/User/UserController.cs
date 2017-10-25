using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly IUserDetailsServices _userServices;
        #region Public Constructor
        public UserController()
        {
            _userServices = new UserDetailsServices();
        }
          [HttpGet]
        public HttpResponseMessage Get()
        {
            var user = _userServices.GetAll();
            if (user != null)
                return Request.CreateResponse(HttpStatusCode.OK, user);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user found for this id");
        }
      
        //--------------------------------Get(GET)-----------------------------------------------
         [HttpGet]
        public HttpResponseMessage Get(int? roleId, string Url = "null")
        {
            //var session = HttpContext.Current.Session;
            //string[] Auth = HttpContext.Current.Request.Headers.GetValues("Authorization");

            //if(Auth != null && HttpRuntime.Cache.Get(Auth[0]) != null)
            //{
                var products = _userServices.GetAllUsers(roleId,Url);
                if (products != null)
                {
                    var productEntities = products as List<UserDetailsEntity> ?? products.ToList();
                    if (productEntities.Any())
                        return Request.CreateResponse(HttpStatusCode.OK, productEntities);
                }
              
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
              
            //}
            // else
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Authendication");
            //}                
            
        }
         //--------------------------------GetValidateUser(GET)----------------------------------
        [HttpGet]
        public HttpResponseMessage GetValidateUser(string userName)
        {
            string message = "";
            var user = _userServices.ValidateUser(userName);
            if (user != null)
            {
                message = string.Format("User Available");
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                message = string.Format("User Not Available");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, message);
            }
        }
         //--------------------------------Get(GET,[id])-----------------------------------------
         [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var user = _userServices.GetUserById(id);
            if (user != null)
                return Request.CreateResponse(HttpStatusCode.OK, user);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user found for this id");
        }
         //--------------------------------CreateUser(POST)--------------------------------------
        [HttpPost]
        public HttpResponseMessage CreateUser([FromBody] UserDetailsEntity userEntity)
        {
            int user_Id = _userServices.CreateUser(userEntity);
            string message = "";
            if (user_Id > 0)
            {
                message = string.Format("User Created Successfully.");
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                message = string.Format("User Created UnSuccessfully.");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, message);
            }
        }
        //--------------------------------UpdateUser(POST)---------------------------------------
        [HttpPost]
        public HttpResponseMessage UpdateUser([FromBody]UserDetailsEntity userEntity)
        {
            string message = "";
            bool responseID;
            if (userEntity.User_id > 0)
            {
                responseID = _userServices.UpdateUser(userEntity.User_id, userEntity);
                if (responseID)
                {
                    message = string.Format("User Updated Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("User Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid UserID");
                return Request.CreateResponse(HttpStatusCode.NotModified, message);
            }
        }
        //--------------------------------DeleteUser(POST)---------------------------------------
        [HttpPost]
        public HttpResponseMessage DeleteUser(int id)
        {
            string message = "";
            bool responseID;
            if (id > 0)
            {
                responseID = _userServices.DeleteUser(id);
                if (responseID)
                {
                    message = string.Format("User Deleted Successfully.");
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    message = string.Format("User Not Found");
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }
            }
            else
            {
                message = string.Format("Invalid UserID");
                return Request.CreateResponse(HttpStatusCode.NotModified, message);
            }
        }
        #endregion
    }
}
