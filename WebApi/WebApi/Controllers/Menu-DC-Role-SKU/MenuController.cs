using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using BusinessServices;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MenuController : ApiController
    {
        private readonly IMenuService _menuServices;

        #region Public Constructor


        public MenuController()
        {
            _menuServices = new MenuService();
        }
        //--------------------------------Get(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var list = _menuServices.GetMenuList(id);
                return Request.CreateResponse(list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message);
            }           
            
        }
              [HttpGet]
        public HttpResponseMessage Get(int id,string From)
        {
            try
            {
                var list = _menuServices.GetMenuListA(id,From);
                return Request.CreateResponse(list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message);
            }           
            
        }
      
        [HttpPost]
        public ResponseEntity DeleteMenu(int id)
        {
            ResponseEntity response = new ResponseEntity();
            if (id > 0)
            {
                bool responseCode = _menuServices.DeleteMenu(id);
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


        [HttpPost]
        public HttpResponseMessage createMenu(MenuMasterEntity menu)
        {
            ResponseEntity rs = new ResponseEntity();

            try
            {
                int id = _menuServices.createMenu(menu);
                if (id != 0)
                {
                    rs.MenuId = id;
                    rs.statusCode = HttpStatusCode.Created;
                    rs.message = "Menu Created Successfully";
                }
                else
                {
                    rs.statusCode = HttpStatusCode.InternalServerError;
                    rs.message = "Menu Creation Faild";
                }
                return Request.CreateResponse(rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message);
            }
           
        }

        [HttpPost]
        public HttpResponseMessage updateMenu(MenuMasterEntity menu)
        {
            ResponseEntity rs = new ResponseEntity();
            try
            {
                if (menu.Menu_Id > 0)
                {
                    bool response = _menuServices.updateMenu(menu.Menu_Id, menu);
                    if (response)
                    {
                        rs.MenuId = menu.Menu_Id;
                        rs.statusCode = HttpStatusCode.OK;
                        rs.message = "Menu Updated Successfully";
                    }
                    else
                    {
                        rs.statusCode = HttpStatusCode.InternalServerError;
                        rs.message = "Menu Updation Faild";
                    }
                }
                else
                {
                    rs.statusCode = HttpStatusCode.NotAcceptable;
                    rs.message = "Invalide MenuId";
                }

                return Request.CreateResponse(rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message);
            }
                        
        }

        [HttpGet]
        public HttpResponseMessage getMenus()
        {
            try
            {
                var list = _menuServices.GetMenuLists();

                return Request.CreateResponse(list);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(ex.Message);
            }
            
        }

        [HttpGet]
        public HttpResponseMessage getSingleMenuList(int menuID)
        {
            try
            {
                var list = _menuServices.GetMenuListByID(menuID);

                return Request.CreateResponse(list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(ex.Message);
            }
            
        }

        //[HttpGet]
        //public HttpResponseMessage SearchforMenu(int? roleId,string Url = "null")
        //{
        //    List<subMenuEntity> f = new List<subMenuEntity>();

        //    if (Url != null && roleId != null) 
        //    {
        //        var result = _menuServices.GetMenu(roleId,Url);
        //        f = result;

        //        return Request.CreateResponse(f);
        //    }
        

        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Data Found");
        //    }
        //}


        #endregion
    }
}
       

