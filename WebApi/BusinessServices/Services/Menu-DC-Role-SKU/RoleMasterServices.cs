using AutoMapper;
using BusinessEntities;
using BusinessServices.Services;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class RoleMasterServices : IRoleMaster
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleMasterServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        LEAFDBEntities DB = new LEAFDBEntities();
        public string AddRoleMaster(RoleMasterEntity RoleMasterModel)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    Role_Master roleMaster = new Role_Master();

                    roleMaster.Role_Name = RoleMasterModel.Role_Name;
                    roleMaster.Is_Delete = false;
                    roleMaster.CreatedDate = System.DateTime.Now;
                    roleMaster.CreatedBy = RoleMasterModel.CreateBy;
                    //    roleMaster.Is_Delete = false;

                    _unitOfWork.RoleRepository.Insert(roleMaster);
                    _unitOfWork.Save();


                    foreach (var menu in RoleMasterModel.Menu)
                    {
                          if (menu.isChecked)
                        {

                        Role_Menu_Access roleMenuMain = new Role_Menu_Access();

                        roleMenuMain.Role_Id = roleMaster.Role_Id;
                        roleMenuMain.Menu_Id = menu.MenuID;
                        roleMenuMain.Parent_Id = 0;


                        string MainPrevilages = "{ 'Add' : '" + menu.SELECTEDADD + "' , 'Edit' : '" + menu.SELECTEDEDIT + "' , 'View' : '" + menu.SELECTEDVIEW + "', 'Delete' : '" + menu.SELECTEDDELETE + "', 'Generate': '" + 0 + "' , 'Approval' : '" + menu.SELECTAPPROVAL + "'}";

                        roleMenuMain.Menu_Previlleges = MainPrevilages;
                        roleMenuMain.CreatedDate = System.DateTime.Now;
                        roleMenuMain.CreatedBy = RoleMasterModel.CreateBy;

                        _unitOfWork.RoleMenuAccessRepository.Insert(roleMenuMain);
                        _unitOfWork.Save();

                        foreach (var subMenu in menu.MenuDetailssub)
                        {
                             if (subMenu.isChecked)
                                {
                            Role_Menu_Access roleMenuSub = new Role_Menu_Access();

                            roleMenuSub.Role_Id = roleMaster.Role_Id;
                            roleMenuSub.Menu_Id = subMenu.MenuID;
                            roleMenuSub.Parent_Id = menu.MenuID;

                            string SubPrevilages = "{ 'Add' : '" + subMenu.SELECTEDADD + "' , 'Edit' : '" + subMenu.SELECTEDEDIT + "' , 'View' : '" + subMenu.SELECTEDVIEW + "', 'Delete' : '" + subMenu.SELECTEDDELETE + "', 'Generate': '" + 0 + "', 'Approval' : '" + menu.SELECTAPPROVAL + "'}";

                            roleMenuSub.Menu_Previlleges = SubPrevilages;
                            roleMenuSub.CreatedDate = System.DateTime.Now;
                            roleMenuSub.CreatedBy = RoleMasterModel.CreateBy;
                            
                            _unitOfWork.RoleMenuAccessRepository.Insert(roleMenuSub);
                            _unitOfWork.Save();

                                }

                        }

                        }



                    }
                    scope.Complete();
                    return "Inserted Successfully";
                }
            }

            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public List<RoleList> getRoleMasterList(int? roleId, string Url)
        {
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges
                ).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);


            var RoleList =(from d in DB.Role_Master
                          where d.Is_Delete==false
                 select 
                 //d).AsEnumerable().Select(d =>
                 new RoleList
                {
                    Role_name = d.Role_Name,
                    Role_ids = d.Role_Id,
                    Count = DB.Role_User_Access.Where(p => p.Role_Id.ToString() == d.Role_Id.ToString()).Select(p => p.User_Id).Count(),
                    Created_date = d.CreatedDate.Value,
                    is_Create = iCrt,
                    is_Delete = isDel,
                    is_Edit = isEdt,
                    is_Approval = isApp,
                    is_View = isViw,
                    //Menu_Id = menuAccess.MenuID,
                    //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                    //is_Create = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Add"]),
                    //is_Delete = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Delete"]),
                    //is_Edit = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Edit"]),
                    //is_Approval = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Approval"]),
                    //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"])

                }).ToList();
            //foreach (var t in RoleList)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return RoleList;
        }


        public List<MenuDetailsR> roleBasedMenu(MenuRoleId MenuRoleId)
        {
            CommanService CommanService = new CommanService();

            List<MenuDetailsR> menuDetails = CommanService.Menu();

            List<MenuDetails> RolemenuDetails = CommanService.RoleAddMenu(MenuRoleId.Role_Id);

            //  List<MenuDetailsR> menuM = new List<MenuDetailsR>();

            foreach (var Main in menuDetails)
            {

                foreach (var Temp in RolemenuDetails)
                {

                    if (Temp.MenuID == Main.MenuID)
                    {
                        Main.MenuID = Temp.MenuID;
                        Main.MenuName = Temp.MenuName;
                        Main.MenuControllerName = Temp.MenuControllerName;
                        Main.SELECTED = 1;
                        if (Main.SELECTEDADD == 2)
                        {
                            Main.SELECTEDADD = 2;
                        }
                        else if (Main.SELECTEDADD == 0 && Temp.Add == 0)
                        {
                            Main.SELECTEDADD = 0;
                        }
                        else if (Main.SELECTEDADD == 0 && Temp.Add == 1)
                        {
                            Main.SELECTEDADD = 1;
                        }
                        else
                        {
                            Main.SELECTEDADD = 6;
                        }


                        // Main.SELECTEDADD = (Main.SELECTEDADD==2)?2:Temp.Add;
                        if (Main.SELECTEDADD == 2)
                        {
                            Main.SELECTEDDELETE = 2;
                        }
                        else if (Main.SELECTEDDELETE == 0 && Temp.Delete == 0)
                        {
                            Main.SELECTEDDELETE = 0;
                        }
                        else if (Main.SELECTEDDELETE == 0 && Temp.Delete == 1)
                        {
                            Main.SELECTEDDELETE = 1;
                        }
                        else
                        {
                            Main.SELECTEDDELETE = 6;
                        }
                        //  Main.SELECTEDDELETE = (Main.SELECTEDDELETE == 2) ? 2 : Temp.Delete;
                        if (Main.SELECTEDEDIT == 2)
                        {
                            Main.SELECTEDEDIT = 2;
                        }
                        else if (Main.SELECTEDEDIT == 0 && Temp.Edit == 0)
                        {
                            Main.SELECTEDEDIT = 0;
                        }
                        else if (Main.SELECTEDEDIT == 0 && Temp.Edit == 1)
                        {
                            Main.SELECTEDEDIT = 1;
                        }
                        else
                        {
                            Main.SELECTEDEDIT = 6;
                        }
                        //Main.SELECTEDEDIT = (Main.SELECTEDEDIT == 2) ? 2 : Temp.Edit;
                        if (Main.SELECTEDVIEW == 2)
                        {
                            Main.SELECTEDVIEW = 2;
                        }
                        else if (Main.SELECTEDVIEW == 0 && Temp.View == 0)
                        {
                            Main.SELECTEDVIEW = 0;
                        }
                        else if (Main.SELECTEDVIEW == 0 && Temp.View == 1)
                        {
                            Main.SELECTEDVIEW = 1;
                        }
                        else
                        {
                            Main.SELECTEDVIEW = 6;
                        }


                        if (Main.SELECTAPPROVAL == 2)
                        {
                            Main.SELECTAPPROVAL = 2;
                        }
                        else if (Main.SELECTAPPROVAL == 0 && Temp.Approval == 0)
                        {
                            Main.SELECTAPPROVAL = 0;
                        }
                        else if (Main.SELECTAPPROVAL == 0 && Temp.Approval == 1)
                        {
                            Main.SELECTAPPROVAL = 1;
                        }
                        else
                        {
                            Main.SELECTAPPROVAL = 6;
                        }






                        // Main.SELECTEDVIEW = (Main.SELECTEDVIEW == 2) ? 2 : Temp.View;
                        Main.isChecked = true;
                        Main.ParentID = Temp.ParentID;
                        Main.Position = Temp.Position;
                        //Main.activeTopMenu = Temp.activeTopMenu;
                        //Main.activeSubMenu = Temp.activeSubMenu;
                        Main.MenuMenuDSub = new List<MenuDetailsSubR>
                        {

                        };

                    }

                    //  menuM.Add(Main);

                }
                //  int TempSubId = 0;

                foreach (var subMain in Main.MenuDetailssub)
                {

                    foreach (var Ptemp in RolemenuDetails)
                    {

                        foreach (var subTemp in Ptemp.MenuDetailssub)
                        {
                            //TempSubId = subTemp.MenuID;
                            if (subTemp.MenuID == subMain.MenuID)
                            {
                                subMain.MenuID = subTemp.MenuID;
                                subMain.MenuName = subTemp.MenuName;
                                subMain.MenuControllerName = subTemp.MenuControllerName;
                                subMain.SELECTED = 1;
                                if (subMain.SELECTEDADD == 2)
                                {
                                    subMain.SELECTEDADD = 2;
                                }
                                else if (subMain.SELECTEDADD == 0 && subTemp.Add == 0)
                                {
                                    subMain.SELECTEDADD = 0;
                                }
                                else if (subMain.SELECTEDADD == 0 && subTemp.Add == 1)
                                {
                                    subMain.SELECTEDADD = 1;
                                }
                                else
                                {
                                    subMain.SELECTEDADD = 6;
                                }


                                if (subMain.SELECTEDDELETE == 2)
                                {
                                    subMain.SELECTEDDELETE = 2;
                                }
                                else if (subMain.SELECTEDDELETE == 0 && subTemp.Delete == 0)
                                {
                                    subMain.SELECTEDDELETE = 0;
                                }
                                else if (subMain.SELECTEDDELETE == 0 && subTemp.Delete == 1)
                                {
                                    subMain.SELECTEDDELETE = 1;
                                }
                                else
                                {
                                    subMain.SELECTEDDELETE = 6;
                                }


                                if (subMain.SELECTEDEDIT == 2)
                                {
                                    subMain.SELECTEDEDIT = 2;
                                }
                                else if (subMain.SELECTEDEDIT == 0 && subTemp.Edit == 0)
                                {
                                    subMain.SELECTEDEDIT = 0;
                                }
                                else if (subMain.SELECTEDEDIT == 0 && subTemp.Edit == 1)
                                {
                                    subMain.SELECTEDEDIT = 1;
                                }
                                else
                                {
                                    subMain.SELECTEDEDIT = 6;
                                }

                                if (subMain.SELECTEDVIEW == 2)
                                {
                                    subMain.SELECTEDVIEW = 2;
                                }
                                else if (subMain.SELECTEDVIEW == 0 && subTemp.View == 0)
                                {
                                    subMain.SELECTEDVIEW = 0;
                                }
                                else if (subMain.SELECTEDVIEW == 0 && subTemp.View == 1)
                                {
                                    subMain.SELECTEDVIEW = 1;
                                }
                                else
                                {
                                    subMain.SELECTEDVIEW = 6;
                                }

                                if (subMain.SELECTEDAPPROVAL == 2)
                                {
                                    subMain.SELECTEDAPPROVAL = 2;
                                }
                                else if (subMain.SELECTEDAPPROVAL == 0 && subTemp.Approval == 0)
                                {
                                    subMain.SELECTEDAPPROVAL = 0;
                                }
                                else if (subMain.SELECTEDAPPROVAL == 0 && subTemp.Approval == 1)
                                {
                                    subMain.SELECTEDAPPROVAL = 1;
                                }
                                else
                                {
                                    subMain.SELECTEDAPPROVAL = 6;
                                }


                                //  subMain.SELECTEDDELETE = (subMain.SELECTEDDELETE == 2) ? 2 : subTemp.Delete;
                                // subMain.SELECTEDEDIT = (subMain.SELECTEDEDIT == 2) ? 2 : subTemp.Edit;
                                //  subMain.SELECTEDVIEW = (subMain.SELECTEDVIEW == 2) ? 2 : subTemp.View;
                                subMain.isChecked = true;
                                subMain.ParentID = subTemp.ParentID;
                                subMain.Position = subTemp.Position;
                                //subMain.activeTopMenu = subTemp.activeTopMenu;
                                //subMain.activeSubMenu = subTemp.activeSubMenu;

                            }

                            //  Main.MenuMenuDSub.Add(subMain);
                            //else if (subTemp.MenuID != subMain.MenuID)
                            //{
                            //    subMain.MenuID = subMain.MenuID;
                            //    subMain.MenuName = subMain.MenuName;
                            //    subMain.MenuControllerName = subMain.MenuControllerName;
                            //    subMain.SELECTED = 1;
                            //    if (subMain.SELECTEDADD == 2)
                            //    {
                            //        subMain.SELECTEDADD = 2;
                            //    }
                            //    else if (subMain.SELECTEDADD == 1)
                            //    {
                            //        subMain.SELECTEDADD = 0;
                            //    }
                            //    else
                            //    {
                            //        subMain.SELECTEDADD = 6;
                            //    }


                            //    // Main.SELECTEDADD = (Main.SELECTEDADD==2)?2:Temp.Add;
                            //    if (subMain.SELECTEDDELETE == 2)
                            //    {
                            //        subMain.SELECTEDDELETE = 2;
                            //    }
                            //    else if (subMain.SELECTEDDELETE == 1)
                            //    {
                            //        subMain.SELECTEDDELETE = 0;
                            //    }
                            //    else
                            //    {
                            //        subMain.SELECTEDDELETE = 6;
                            //    }
                            //    //  Main.SELECTEDDELETE = (Main.SELECTEDDELETE == 2) ? 2 : Temp.Delete;
                            //    if (subMain.SELECTEDEDIT == 2)
                            //    {
                            //        subMain.SELECTEDEDIT = 2;
                            //    }
                            //    else if (subMain.SELECTEDEDIT == 1)
                            //    {
                            //        subMain.SELECTEDEDIT = 0;
                            //    }
                            //    else
                            //    {
                            //        subMain.SELECTEDEDIT = 6;
                            //    }
                            //    //Main.SELECTEDEDIT = (Main.SELECTEDEDIT == 2) ? 2 : Temp.Edit;
                            //    if (subMain.SELECTEDVIEW == 2)
                            //    {
                            //        subMain.SELECTEDVIEW = 2;
                            //    }
                            //    else if (subMain.SELECTEDVIEW == 1)
                            //    {
                            //        subMain.SELECTEDVIEW = 0;
                            //    }

                            //    else
                            //    {
                            //        subMain.SELECTEDVIEW = 6;
                            //    }
                            //    // Main.SELECTEDVIEW = (Main.SELECTEDVIEW == 2) ? 2 : Temp.View;
                            //    subMain.isChecked = true;
                            //    subMain.ParentID = subMain.ParentID;
                            //    subMain.Position = subMain.Position;
                            //    subMain.activeTopMenu = subMain.activeTopMenu;
                            //    subMain.activeSubMenu = subMain.activeSubMenu;

                            //}

                        }
                    }
                }
            }
            return menuDetails;

            //var result = menuM.ToList();
            //return result;
        }

        //public List<MenuDetails> roleBasedMenu(MenuRoleId MenuRoleId)
        //{

        //    CommanService CommanService = new CommanService();

        //    List<MenuDetailsR> menuDetails = CommanService.Menu();
        //    List<MenuDetailsR> ListMenuDetails = new List<MenuDetailsR>();
        //    List<MenuDetailsR> ListMenuDetailsMain = new List<MenuDetailsR>();

        //    List<MenuDetails> RolemenuDetails = CommanService.RoleAddMenu(MenuRoleId.Role_Id);


        //    //var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

        //    //foreach (var main in Main)
        //    //{
        //    //    MenuDetails MenuDetailsMain = new MenuDetails();

        //    //    List<MenuDetailsSub> ListMenuDetailsSub = new List<MenuDetailsSub>();

        //    //    MenuDetailsMain.MenuID = main.MenuID;
        //    //    MenuDetailsMain.MenuName = main.MenuName;
        //    //    MenuDetailsMain.MenuControllerName = main.MenuControllerName;
        //    //    MenuDetailsMain.Add = main.Add;
        //    //    MenuDetailsMain.Edit = main.Edit;
        //    //    MenuDetailsMain.Delete = main.Delete;
        //    //    MenuDetailsMain.View = main.View;
        //    //    MenuDetailsMain.Generate = main.Generate;
        //    //    MenuDetailsMain.isChecked = false;
        //    //    MenuDetailsMain.ParentID = main.ParentID;
        //    //    MenuDetailsMain.Position = main.Position;
        //    //    MenuDetailsMain.activeTopMenu = main.activeTopMenu;
        //    //    MenuDetailsMain.activeSubMenu = main.activeSubMenu;

        //    //    var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

        //    //    foreach (var sub in Sub)
        //    //    {
        //    //        MenuDetailsSub MenuDetailsSub = new MenuDetailsSub();
        //    //        MenuDetailsSub.MenuID = sub.MenuID;
        //    //        MenuDetailsSub.MenuName = sub.MenuName;
        //    //        MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
        //    //        MenuDetailsSub.Add = sub.Add;
        //    //        MenuDetailsSub.Edit = sub.Edit;
        //    //        MenuDetailsSub.Delete = sub.Delete;
        //    //        MenuDetailsSub.View = sub.View;
        //    //        MenuDetailsSub.Generate = sub.Generate;
        //    //        MenuDetailsSub.isChecked = false;
        //    //        MenuDetailsSub.ParentID = sub.ParentID;
        //    //        MenuDetailsSub.Position = sub.Position;
        //    //        MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
        //    //        MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

        //    //        ListMenuDetailsSub.Add(MenuDetailsSub);
        //    //    }

        //    //    MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;
        //    //    ListMenuDetailsMain.Add(MenuDetailsMain);
        //    //}




        //    //foreach (var Main in menuDetails)
        //    //{
        //    //    List<MenuDetailsSub> ListMenuDetailsSub = new List<MenuDetailsSub>();
        //    //    MenuDetails MenuDetailsMain = new MenuDetails();
        //    //    foreach (var Temp in RolemenuDetails)
        //    //    {

        //    //        if (Temp.MenuID == Main.MenuID)
        //    //        {
        //    //            Main.MenuID = Temp.MenuID;
        //    //            Main.MenuName = Temp.MenuName;
        //    //            Main.MenuControllerName = Temp.MenuControllerName;
        //    //            Main.SELECTED = 1;
        //    //            Main.SELECTEDADD = (Main.SELECTEDADD == 2) ? 2 : Temp.Add;
        //    //            Main.SELECTEDDELETE = (Main.SELECTEDDELETE == 2) ? 2 : Temp.Delete;
        //    //            Main.SELECTEDEDIT = (Main.SELECTEDEDIT == 2) ? 2 : Temp.Edit;
        //    //            Main.SELECTEDVIEW = (Main.SELECTEDVIEW == 2) ? 2 : Temp.View;
        //    //            Main.isChecked = true;
        //    //            Main.ParentID = Temp.ParentID;
        //    //            Main.Position = Temp.Position;
        //    //            Main.activeTopMenu = Temp.activeTopMenu;
        //    //            Main.activeSubMenu = Temp.activeSubMenu;

        //    //        }

        //    //        ListMenuDetails.Add(Main);


        //    //    }

        //    //    foreach (var subMain in Main.MenuDetailssub)
        //    //    {

        //    //        foreach (var Ptemp in RolemenuDetails)
        //    //        {
        //    //            foreach (var subTemp in Ptemp.MenuDetailssub)
        //    //            {

        //    //                if (subTemp.MenuID == subMain.MenuID)
        //    //                {
        //    //                    subMain.MenuID = subTemp.MenuID;
        //    //                    subMain.MenuName = subTemp.MenuName;
        //    //                    subMain.MenuControllerName = subTemp.MenuControllerName;
        //    //                    subMain.SELECTED = 1;
        //    //                    subMain.SELECTEDADD = (subMain.SELECTEDADD == 2) ? 2 : subTemp.Add;
        //    //                    subMain.SELECTEDDELETE = (subMain.SELECTEDDELETE == 2) ? 2 : subTemp.Delete;
        //    //                    subMain.SELECTEDEDIT = (subMain.SELECTEDEDIT == 2) ? 2 : subTemp.Edit;
        //    //                    subMain.SELECTEDVIEW = (subMain.SELECTEDVIEW == 2) ? 2 : subTemp.View;
        //    //                    subMain.isChecked = true;
        //    //                    subMain.ParentID = subTemp.ParentID;
        //    //                    subMain.Position = subTemp.Position;
        //    //                    subMain.activeTopMenu = subTemp.activeTopMenu;
        //    //                    subMain.activeSubMenu = subTemp.activeSubMenu;

        //    //                }
        //    //                ListMenuDetailsSub.Add(subTemp);

        //    //            }
        //    //        }


        //    //        MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;
        //    //        ListMenuDetailsMain.Add(Main);
        //    //    }
        //    //}
        //    return RolemenuDetails;
        //}



        public List<MenuDetailsR> newRoleBasedMenu()
        {
            CommanService CommanService = new CommanService();
            List<MenuDetailsR> menuDetails = CommanService.Menu();

            return menuDetails;
        }

        public List<MenuDetailsP> Menupermissions()
        {
            CommanService CommanService = new CommanService();
            List<MenuDetailsP> menuDetails = CommanService.MenuPermission();

            return menuDetails;
        }




        public string updateRoleMasterList(RoleId RoleIdS)
        {

            var RoleMaster = DB.Role_Master.Where(d => d.Role_Id == RoleIdS.Role_Id).FirstOrDefault<Role_Master>();


            try
            {
                using (var scope = new TransactionScope())
                {

                    if (RoleMaster != null)
                    {
                        RoleMaster.Role_Name = RoleIdS.Role_name;
                        RoleMaster.UpdatedBy = RoleIdS.UpdatedBy;
                        RoleMaster.UpdatedDate = DateTime.Now;
                        DB.Entry(RoleMaster).State = EntityState.Modified;
                        DB.SaveChanges();

                    }

                    DB.Role_Menu_Access.RemoveRange(DB.Role_Menu_Access.Where(d => d.Role_Id == RoleIdS.Role_Id));
                    DB.SaveChanges();



                    foreach (var menu in RoleIdS.Menu)
                    {

                        
                        if (menu.isChecked)
                        {

                        Role_Menu_Access roleMenuMain = new Role_Menu_Access();

                        roleMenuMain.Role_Id = RoleIdS.Role_Id;
                        roleMenuMain.Menu_Id = menu.MenuID;
                        roleMenuMain.Parent_Id = 0;


                        string MainPrevilages = "{ 'Add' : '" + menu.SELECTEDADD + "' , 'Edit' : '" + menu.SELECTEDEDIT + "' , 'View' : '" + menu.SELECTEDVIEW + "', 'Delete' : '" + menu.SELECTEDDELETE + "', 'Generate': '" + 0 + "', 'Approval' : '" + menu.SELECTAPPROVAL + "'}";

                        roleMenuMain.Menu_Previlleges = MainPrevilages;
                        roleMenuMain.CreatedDate = System.DateTime.Now;
                        roleMenuMain.CreatedBy = RoleIdS.CreatedBy;

                        _unitOfWork.RoleMenuAccessRepository.Insert(roleMenuMain);
                        _unitOfWork.Save();

                        foreach (var subMenu in menu.MenuDetailssub)
                        {
                            if (subMenu.isChecked)
                                {

                            Role_Menu_Access roleMenuSub = new Role_Menu_Access();

                            roleMenuSub.Role_Id = RoleIdS.Role_Id;
                            roleMenuSub.Menu_Id = subMenu.MenuID;
                            roleMenuSub.Parent_Id = menu.MenuID;

                            string SubPrevilages = "{ 'Add' : '" + subMenu.SELECTEDADD + "' , 'Edit' : '" + subMenu.SELECTEDEDIT + "' , 'View' : '" + subMenu.SELECTEDVIEW + "', 'Delete' : '" + subMenu.SELECTEDDELETE + "', 'Generate': '" + 0 + "', 'Approval' : '" + menu.SELECTAPPROVAL + "'}";

                            roleMenuSub.Menu_Previlleges = SubPrevilages;
                            roleMenuSub.CreatedDate = System.DateTime.Now;
                            roleMenuSub.UpdatedBy = RoleIdS.UpdatedBy.ToString();


                            _unitOfWork.RoleMenuAccessRepository.Insert(roleMenuSub);
                            _unitOfWork.Save();

                                }

                        }

                        }

                    }
                    scope.Complete();
                    return "Updated Successfully";
                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }



        }

        public string deleteRoleMaster(RoleId RoleId)
        {

            try
            {
                using (var scope = new TransactionScope())
                {
                    var DeleteRoleMaster = DB.Role_Master.Where(d => d.Role_Id == RoleId.Role_Id).FirstOrDefault<Role_Master>();

                    var UserID = DB.Role_User_Access.Where(d => d.Role_Id.ToString() == DeleteRoleMaster.Role_Id.ToString()).ToList();




                    foreach (var userid in UserID)
                    {
                        //var Employeee = DB.Employee_info.Where(d => d.User_id == userid.User_id).FirstOrDefault<Employee_info>();
                        //Employeee.rol = true;
                        //DB.Entry(Employeee).State = EntityState.Modified;
                        userid.Role_Id = null;
                        DB.Entry(userid).State = EntityState.Modified;

                    }

                    DB.SaveChanges();




                    if (DeleteRoleMaster != null)
                    {

                        DeleteRoleMaster.Is_Delete = true;
                        DB.Entry(DeleteRoleMaster).State = EntityState.Modified;
                        DB.SaveChanges();
                    }
                    DB.Role_Menu_Access.RemoveRange(DB.Role_Menu_Access.Where(d => d.Role_Id == RoleId.Role_Id));
                    DB.SaveChanges();

                    scope.Complete();
                    return "Deleted Successfully";
                }
            }

            catch (Exception ex)
            {

                return ex.Message;

            }

        }


    }
}
