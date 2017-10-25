using DataModel;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using BusinessEntities;
using DataModel.UnitOfWork;
using System.Transactions;

namespace BusinessServices
{
    public class MenuService : IMenuService
    {

        private readonly UnitOfWork _unitOfWork;
        public MenuService()
        {
            _unitOfWork = new UnitOfWork();
        }

        LEAFDBEntities DB = new LEAFDBEntities();

        //public List<subMenuEntity> GetMenu(int? roleId, string Url = "null")
        //{
        //    var qu = (from a in DB.Role_Menu_Access
        //              join b in DB.Menu_Master on a.Menu_Id equals b.Menu_Id
        //              where ((a.Role_Id==roleId) &&  b.Url==Url)
        //              select new subMenuEntity
        //              {
        //                  Menu_Id=a.Menu_Id,
        //                  Menu_Name=b.Menu_Name,
        //                  is_Create=a.is_Create,
        //                  is_Edit=a.is_edit,
        //                  is_Delete=a.is_Delete,
        //                  is_View=a.is_View

        //              }).ToList();
        //    return qu;
        //}

        public int createMenu(MenuMasterEntity menu)
        {
            using (var scope = new TransactionScope())
            {
                var menuList = new Menu_Master
                {
                    Menu_Name = menu.Menu_Name,
                    Url = menu.Url,
                    Icon_Name = menu.Icon_Name,
                    NG_Class = menu.NG_Class,
                    Parent_id = menu.Parent_id,
                    Sub_id = menu.Sub_id,
                    Sub_Class = menu.Sub_Class,
                    //is_Create = menu.is_Create,
                    //is_Delete = menu.is_Delete,
                    //is_Edit = menu.is_Edit,
                    //is_View = menu.is_View
                };

                _unitOfWork.MenuListRepository.Insert(menuList);
                _unitOfWork.Save();

                scope.Complete();
                return menuList.Menu_Id;
            }
            
        }

        public bool updateMenu(int id, MenuMasterEntity menu)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                var menuitem = _unitOfWork.MenuListRepository.GetByID(id);
                if(menuitem != null)
                {
                    menuitem.Menu_Name = menu.Menu_Name;
                    menuitem.Url = menu.Url;
                    menuitem.Icon_Name = menu.Icon_Name;
                    menuitem.NG_Class = menu.NG_Class;
                    menuitem.Parent_id = menu.Parent_id;
                    menuitem.Sub_id = menu.Sub_id;
                    menuitem.Sub_Class = menu.Sub_Class;
                    //menuitem.is_Create = menu.is_Create;
                    //menuitem.is_Delete = menu.is_Delete;
                    //menuitem.is_Edit = menu.is_Edit;
                    //menuitem.is_View = menu.is_View;

                    _unitOfWork.MenuListRepository.Update(menuitem);
                    _unitOfWork.Save();
                    result = true;
                }

                scope.Complete();
            }
          
           
            return result;
        }

        public List<MenuMasterEntity> GetMenuListByID(int id)
        {
            var query = from m in DB.Menu_Master
                        orderby m.Menu_Id
                        select new MenuMasterEntity
                        {
                            Menu_Name = m.Menu_Name,
                            Parent_id = m.Parent_id,
                            Sub_id = m.Sub_id,
                            Sub_Class = m.Sub_Class,
                            Url = m.Url,
                            Icon_Name = m.Icon_Name,
                            NG_Class = m.NG_Class,
                            Menu_Id = m.Menu_Id
                        };

            List<MenuMasterEntity> menuItems = new List<MenuMasterEntity>();

            foreach (MenuMasterEntity menu in query)
            {
                if (menu.Parent_id == 0 && menu.Menu_Id == id)
                {
                    var items = new MenuMasterEntity
                    {
                        Menu_Id = menu.Menu_Id,
                        Menu_Name = menu.Menu_Name,
                        Parent_id = menu.Parent_id,
                        Sub_id = menu.Sub_id,
                        Sub_Class = menu.Sub_Class,
                        Url = menu.Url,
                        Icon_Name = menu.Icon_Name,
                        NG_Class = menu.NG_Class,
                        SubmenuList = new List<subMenuEntity>
                        {
                        }
                    };

                    menuItems.Add(items);
                }
            }

            foreach (MenuMasterEntity menulist in menuItems)
            {
                foreach (MenuMasterEntity menulist1 in query)
                {
                    if (menulist.Menu_Id == menulist1.Parent_id)
                    {
                        var menu = menuItems.Where(x => x.Menu_Id == menulist.Menu_Id);

                        if (menu.Count() != 0)
                        {
                            var submenu = new subMenuEntity
                            {
                                Menu_Id = menulist1.Menu_Id,
                                Menu_Name = menulist1.Menu_Name,
                                Parent_id = menulist1.Parent_id,
                                Sub_id = menulist1.Sub_id,
                                Sub_Class = menulist1.Sub_Class,
                                Url = menulist1.Url,
                                Icon_Name = menulist1.Icon_Name,
                                NG_Class = menulist1.NG_Class,
                            };

                            menulist.SubmenuList.Add(submenu);
                        }
                    }

                }
            }

            var result = menuItems.ToList();
            return result;
        }

        public bool DeleteMenu(int menuID)
        {
            if (menuID > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var lmaster = _unitOfWork.MenuListRepository.GetByID(menuID);
                    if (lmaster != null)
                    {
                        _unitOfWork.MenuListRepository.Delete(lmaster);
                        _unitOfWork.Save();
                        scope.Complete();
                        return true;
                    }
                }
            }
            return false;
        }

        public List<MenuMasterEntity> GetMenuLists()
        {
            var query = from m in DB.Menu_Master
                        orderby m.Menu_Id
                        select new MenuMasterEntity
                        {
                            Menu_Name = m.Menu_Name,
                            Parent_id = m.Parent_id,
                            Sub_id = m.Sub_id,
                            Sub_Class = m.Sub_Class,
                            Url = m.Url,
                            Icon_Name = m.Icon_Name,
                            NG_Class = m.NG_Class,
                            Menu_Id = m.Menu_Id
                        };

            List<MenuMasterEntity> menuItems = new List<MenuMasterEntity>();

            foreach (MenuMasterEntity menu in query)
            {
                if (menu.Parent_id == 0)
                {
                    var items = new MenuMasterEntity
                    {
                        Menu_Id = menu.Menu_Id,
                        Menu_Name = menu.Menu_Name,
                        Parent_id = menu.Parent_id,
                        Sub_id = menu.Sub_id,
                        Sub_Class = menu.Sub_Class,
                        Url = menu.Url,
                        Icon_Name = menu.Icon_Name,
                        NG_Class = menu.NG_Class,
                        SubmenuList = new List<subMenuEntity>
                        {
                        }
                    };

                    menuItems.Add(items);
                }
            }

            foreach (MenuMasterEntity menulist in menuItems)
            {
                foreach (MenuMasterEntity menulist1 in query)
                {
                    if (menulist.Menu_Id == menulist1.Parent_id)
                    {
                        var menu = menuItems.Where(x => x.Menu_Id == menulist.Menu_Id);

                        if (menu.Count() != 0)
                        {
                            var submenu = new subMenuEntity
                            {
                                Menu_Id = menulist1.Menu_Id,
                                Menu_Name = menulist1.Menu_Name,
                                Parent_id = menulist1.Parent_id,
                                Sub_id = menulist1.Sub_id,
                                Sub_Class = menulist1.Sub_Class,
                                Url = menulist1.Url,
                                Icon_Name = menulist1.Icon_Name,
                                NG_Class = menulist1.NG_Class,
                            };

                            menulist.SubmenuList.Add(submenu);
                        }
                    }

                }
            }

            var result = menuItems.ToList();
            return result;
        }

        public List<MenuMasterEntity> GetMenuList(int id)
        {
            var query = from m in DB.Menu_Master
                        join rma in DB.Role_Menu_Access on m.Menu_Id equals rma.Menu_Id
                        join rua in DB.Role_User_Access on rma.Role_Id equals rua.Role_Id
                        where rua.User_Id == id
                        orderby m.Menu_Id
                        select new MenuMasterEntity
                        {
                            Menu_Name = m.Menu_Name,
                            Parent_id = m.Parent_id,
                            Sub_id = m.Sub_id,
                            Sub_Class = m.Sub_Class,
                            Url = m.Url,
                            Icon_Name = m.Icon_Name,
                            NG_Class = m.NG_Class,
                            Menu_Id = m.Menu_Id
                        };

            List<MenuMasterEntity> menuItems = new List<MenuMasterEntity>();

            foreach (MenuMasterEntity menu in query)
            {
                if (menu.Parent_id == 0)
                {
                    var items = new MenuMasterEntity
                    {
                        Menu_Id = menu.Menu_Id,
                        Menu_Name = menu.Menu_Name,
                        Parent_id = menu.Parent_id,
                        Sub_id = menu.Sub_id,
                        Sub_Class = menu.Sub_Class,
                        Url = menu.Url,
                        Icon_Name = menu.Icon_Name,
                        NG_Class = menu.NG_Class,
                        SubmenuList = new List<subMenuEntity>
                        {
                        }
                    };

                    menuItems.Add(items);
                }
            }

            foreach (MenuMasterEntity menulist in menuItems)
            {
                foreach (MenuMasterEntity menulist1 in query)
                {
                    if (menulist.Menu_Id == menulist1.Parent_id)
                    {
                        var menu = menuItems.Where(x => x.Menu_Id == menulist.Menu_Id);

                        if (menu.Count() != 0)
                        {
                            var submenu = new subMenuEntity
                            {
                                Menu_Id = menulist1.Menu_Id,
                                Menu_Name = menulist1.Menu_Name,
                                Parent_id = menulist1.Parent_id,
                                Sub_id = menulist1.Sub_id,
                                Sub_Class = menulist1.Sub_Class,
                                Url = menulist1.Url,
                                Icon_Name = menulist1.Icon_Name,
                                NG_Class = menulist1.NG_Class,
                            };

                            menulist.SubmenuList.Add(submenu);
                        }
                    }

                }
            }

            var result = menuItems.ToList();
            return result;
        }
        public List<MenuMasterEntity> GetMenuListA(int id,string From)
        {
                       
            var query = from m in DB.Menu_Master
                        join rma in DB.Role_Menu_Access on m.Menu_Id equals rma.Menu_Id
                        join rua in DB.Role_User_Access on rma.Role_Id equals rua.Role_Id
                        where rua.User_Id == id
                        orderby m.Menu_Id
                        select new MenuMasterEntity
                        {
                            Menu_Name = m.Menu_Name,
                            Parent_id = m.Parent_id,
                            Sub_id = m.Sub_id,
                            Sub_Class = m.Sub_Class,
                            Url = m.Url,
                            Web_Flag=rma.Web_Flag,
                            Mobile_Flag=rma.Mobile_Flag,
                            Icon_Name = m.Icon_Name,
                            NG_Class = m.NG_Class,
                            Menu_Id = m.Menu_Id
                        };
            if (From == "W")
            {
                query=query.Where(s=>s.Web_Flag==true);
            }
            else if (From == "M")
            {
                query = query.Where(s => s.Mobile_Flag == true);
            }
           
            List<MenuMasterEntity> menuItems = new List<MenuMasterEntity>();

            foreach (MenuMasterEntity menu in query)
            {
                if (menu.Parent_id == 0)
                {
                    var items = new MenuMasterEntity
                    {
                        Menu_Id = menu.Menu_Id,
                        Menu_Name = menu.Menu_Name,
                        Parent_id = menu.Parent_id,
                        Sub_id = menu.Sub_id,
                        Sub_Class = menu.Sub_Class,
                        Url = menu.Url,
                        Icon_Name = menu.Icon_Name,
                        NG_Class = menu.NG_Class,
                        SubmenuList = new List<subMenuEntity>
                        {
                        }
                    };

                    menuItems.Add(items);
                }
            }

            foreach (MenuMasterEntity menulist in menuItems)
            {
                foreach (MenuMasterEntity menulist1 in query)
                {
                    if (menulist.Menu_Id == menulist1.Parent_id)
                    {
                        var menu = menuItems.Where(x => x.Menu_Id == menulist.Menu_Id);

                        if (menu.Count() != 0)
                        {
                            var submenu = new subMenuEntity
                            {
                                Menu_Id = menulist1.Menu_Id,
                                Menu_Name = menulist1.Menu_Name,
                                Parent_id = menulist1.Parent_id,
                                Sub_id = menulist1.Sub_id,
                                Sub_Class = menulist1.Sub_Class,
                                Url = menulist1.Url,
                                Icon_Name = menulist1.Icon_Name,
                                NG_Class = menulist1.NG_Class,
                            };

                            menulist.SubmenuList.Add(submenu);
                        }
                    }

                }
            }

            var result = menuItems.ToList();
            if (From != "W" && From != "M")
            {
                result = new List<MenuMasterEntity>();
            }
          
            return result;
        }
    }
}
        //  public List<MenuMasterEntity> GetMenuList(int id)
        //{
        //    var query = (from m in DB.Menu_Master
        //                 join rma in DB.Role_Menu_Access on m.Menu_Id equals rma.Menu_Id
        //                 join rua in DB.Role_User_Access on rma.Role_Id equals rua.Role_Id
        //                 where rua.User_Id == id && m.Parent_id == 0
        //                 select new MenuMasterEntity
        //                 {
        //                     Menu_Name = m.Menu_Name,
        //                     Parent_id = m.Parent_id,
        //                     Sub_id = m.Sub_id,
        //                     Sub_Class = m.Sub_Class,
        //                     Url = m.Url,
        //                     Icon_Name = m.Icon_Name,
        //                     NG_Class = m.NG_Class,
        //                     Menu_Id = m.Menu_Id,
        //                     SubmenuList = (from x in DB.Menu_Master
        //                                    where x.Parent_id == m.Menu_Id
        //                                    select new subMenuEntity
        //                                    {
        //                                        Menu_Name = x.Menu_Name,
        //                                        Parent_id = x.Parent_id,
        //                                        Sub_id = x.Sub_id,
        //                                        Sub_Class = x.Sub_Class,
        //                                        Url = x.Url,
        //                                        Icon_Name = x.Icon_Name,
        //                                        NG_Class = x.NG_Class,
        //                                        Menu_Id = x.Menu_Id
        //                                    }).ToList()
        //                 }).ToList();

        //    var result = query.ToList();
        //    return result;
        //}
        //[HttpGet]
        //public List<MenuMasterEntity> GetMenuList(int id)
        //{
        //    var query = from m in DB.Menu_Master
        //                join rma in DB.Role_Menu_Access on m.Menu_Id equals rma.Menu_Id
        //                join rua in DB.Role_User_Access on rma.Role_Id equals rua.Role_Id
        //                where rua.User_Id == id
        //                orderby m.Parent_id
        //                select new MenuMasterEntity
        //                {
        //                    Menu_Name = m.Menu_Name,
        //                    Parent_id=m.Parent_id,
        //                    Sub_id=m.Sub_id,
        //                    Sub_Class=m.Sub_Class,                                                
        //                    Url = m.Url,
        //                    Icon_Name = m.Icon_Name,
        //                    NG_Class = m.NG_Class,
        //                    Menu_Id=m.Menu_Id
        //                };
            //var xs = from x in DB.Role_User_Access
            //         join y in DB.Role_Menu_Access on x.Role_Id equals y.Role_Id
            //         where x.Role_Id == id && x.Menu_Id == y.Menu_Id
            //         select new Mainmenu
            //         {
            //             menuname = y.Menu_Name,
            //             url = y.Url,
            //             fa_name = y.IconName,
            //             ng_class = y.NGClass,
            //             Sub_menu = (from yy in DB.Sub_Menu
            //                         where x.Menu_Id == yy.Menu_Id
            //                         select new SubMenu
            //                         {

            //                             Menu_name = yy.Sub_Name,
            //                             url = yy.Url,
            //                             class_name = yy.Class
            //                         })

            //         };
            //var result = query.ToList();
//            //return result;
//        }
//    }
//}


