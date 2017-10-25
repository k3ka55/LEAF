using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Services
{
    public class CommanService
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();

        public List<MenuDetails> LoginMenu(List<FetchMenuDetails> menuids)
        {
            List<MenuDetails> ListMenuDetails = new List<MenuDetails>();
            List<MenuDetails> ListMenuDetailsMain = new List<MenuDetails>();

            foreach (var menu in menuids)
            {
                int OutAdd = 0;
                int OutEdit = 0;
                int OutDelete = 0;
                int OutView = 0;
                int OutGenerate = 0;
                int OutApproval = 0;

                MenuDetails MenuDetails = new MenuDetails();

                var Menuid = menu.MenuID;
                var Menuprevillages = menu.MenuPrevilages;
                var MenuName = menu.MenuName;
                var MenuController = menu.ControllerName;
                var ParentID = menu.ParentID;
                var Position = menu.Position;
                //var activeTopMenu = menu.activeTopMenu;
                //var activeSubMenu = menu.activeSubMenu;

                int Add = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Add"]);
                int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Edit"]);
                int View = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["View"]);
                int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Delete"]);
                int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Generate"]);
                int Approval = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Approval"]);

                if (Add == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                        if (Add1 == 1)
                        {
                            OutAdd = 1;
                            break;
                        }
                    }
                }
                if (Edit == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                        if (Edit1 == 1)
                        {
                            OutEdit = 1;
                            break;
                        }
                    }
                }
                if (View == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                        if (View1 == 1)
                        {
                            OutView = 1;
                            break;
                        }
                    }

                }
                if (Delete == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                        if (Delete1 == 1)
                        {
                            OutDelete = 1;
                            break;
                        }
                    }

                }
                if (Generate == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                        if (Generate1 == 1)
                        {
                            OutGenerate = 1;
                            break;
                        }
                    }

                }
                if (Approval == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                        if (Approval1 == 1)
                        {
                            OutApproval = 1;
                            break;
                        }
                    }

                }

                MenuDetails.MenuID = Menuid;
                MenuDetails.MenuName = MenuName.First();
                MenuDetails.MenuControllerName = MenuController.First();
                MenuDetails.Add = OutAdd;
                MenuDetails.Edit = OutEdit;
                MenuDetails.Delete = OutDelete;
                MenuDetails.View = OutView;
                MenuDetails.Generate = OutGenerate;
                MenuDetails.Approval = OutApproval;
                MenuDetails.isChecked = false;
                MenuDetails.ParentID = ParentID.First();
                MenuDetails.Position = Position.First();
                //MenuDetails.activeTopMenu = activeTopMenu.First();
                //MenuDetails.activeSubMenu = activeSubMenu.First();

                ListMenuDetails.Add(MenuDetails);
            }

            var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

            foreach (var main in Main)
            {
                MenuDetails MenuDetailsMain = new MenuDetails();

                List<MenuDetailsSub> ListMenuDetailsSub = new List<MenuDetailsSub>();

                MenuDetailsMain.MenuID = main.MenuID;
                MenuDetailsMain.MenuName = main.MenuName;
                MenuDetailsMain.MenuControllerName = main.MenuControllerName;
                MenuDetailsMain.Add = main.Add;
                MenuDetailsMain.Edit = main.Edit;
                MenuDetailsMain.Delete = main.Delete;
                MenuDetailsMain.View = main.View;
                MenuDetailsMain.Generate = main.Generate;
                MenuDetailsMain.Approval = main.Approval;
                MenuDetailsMain.isChecked = false;
                MenuDetailsMain.ParentID = main.ParentID;
                MenuDetailsMain.Position = main.Position;
                //MenuDetailsMain.activeTopMenu = main.activeTopMenu;
                //MenuDetailsMain.activeSubMenu = main.activeSubMenu;

                var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

                foreach (var sub in Sub)
                {
                    MenuDetailsSub MenuDetailsSub = new MenuDetailsSub();
                    MenuDetailsSub.MenuID = sub.MenuID;
                    MenuDetailsSub.MenuName = sub.MenuName;
                    MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
                    MenuDetailsSub.Add = sub.Add;
                    MenuDetailsSub.Edit = sub.Edit;
                    MenuDetailsSub.Delete = sub.Delete;
                    MenuDetailsSub.View = sub.View;
                    MenuDetailsSub.Generate = sub.Generate;
                    MenuDetailsSub.Approval = sub.Approval;
                    MenuDetailsSub.isChecked = false;
                    MenuDetailsSub.ParentID = sub.ParentID;
                    MenuDetailsSub.Position = sub.Position;
                    //MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
                    //MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

                    ListMenuDetailsSub.Add(MenuDetailsSub);
                }

                MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;

                ListMenuDetailsMain.Add(MenuDetailsMain);
            }

            return ListMenuDetailsMain;

        }
        //public List<MenuDetails> RoleAddMenu(int Roleids)
        //{
        //    List<MenuDetails> ListMenuDetails = new List<MenuDetails>();
        //    List<MenuDetails> ListMenuDetailsMain = new List<MenuDetails>();

        //    var menuids = DB.Role_menu_permission
        // .Join
        // (

        //     DB.Menu_master,
        //     c => c.Menu_id,
        //     d => d.Menu_id,
        //     (c, d) => new { c, d }

        // )
        // .Where(e => e.c.Role_id == Roleids).Where(g => g.d.Menu_id == g.c.Menu_id).GroupBy(e => new { e.d.Menu_id })
        // .Select(x => new FetchMenuDetails

        // {
        //     MenuID = x.Key.Menu_id,
        //     MenuName = x.Select(c => c.d.Menu_name).Distinct(),
        //     MenuPrevilages = x.Select(c => c.d.Menu_previllages).Distinct(),
        //     RolePrevilages = x.Select(c => c.c.Role_previllages).Distinct(),
        //     ControllerName = x.Select(c => c.d.Menu_controller_name).Distinct(),
        //     ParentID = x.Select(c => c.d.Parent_id).Distinct(),
        //     Position = x.Select(c => c.d.Position).Distinct(),
        //     activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
        //     activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()

        // }).ToList();

        //    foreach (var menu in menuids)
        //    {
        //        int OutAdd = 0;
        //        int OutEdit = 0;
        //        int OutDelete = 0;
        //        int OutView = 0;
        //        int OutGenerate = 0;

        //        MenuDetails MenuDetails = new MenuDetails();

        //        var Menuid = menu.MenuID;
        //        var Menuprevillages = menu.MenuPrevilages;
        //        var MenuName = menu.MenuName;
        //        var MenuController = menu.ControllerName;
        //        var ParentID = menu.ParentID;
        //        var Position = menu.Position;
        //        var activeTopMenu = menu.activeTopMenu;
        //        var activeSubMenu = menu.activeSubMenu;

        //        int Add = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Add"]);
        //        int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Edit"]);
        //        int View = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["View"]);
        //        int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Delete"]);
        //        int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Generate"]);

        //        if (Add == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

        //                if (Add1 == 1)
        //                {
        //                    OutAdd = 1;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (Add == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

        //                if (Add1 == 2)
        //                {
        //                    OutAdd = 2;
        //                    break;
        //                }
        //            }

        //        }

        //        if (Edit == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

        //                if (Edit1 == 1)
        //                {
        //                    OutEdit = 1;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (Edit == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

        //                if (Edit1 == 2)
        //                {
        //                    OutEdit = 2;
        //                    break;
        //                }
        //            }
        //        }
        //        if (View == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

        //                if (View1 == 1)
        //                {
        //                    OutView = 1;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (View == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

        //                if (View1 == 2)
        //                {
        //                    OutView = 2;
        //                    break;
        //                }
        //            }

        //        }
        //        if (Delete == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

        //                if (Delete1 == 1)
        //                {
        //                    OutDelete = 1;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (Delete == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

        //                if (Delete1 == 2)
        //                {
        //                    OutDelete = 2;
        //                    break;
        //                }
        //            }

        //        }
        //        if (Generate == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

        //                if (Generate1 == 1)
        //                {
        //                    OutGenerate = 1;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (Generate == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

        //                if (Generate1 == 2)
        //                {
        //                    OutGenerate = 2;
        //                    break;
        //                }
        //            }

        //        }

        //        MenuDetails.MenuID = Menuid;
        //        MenuDetails.MenuName = MenuName.First();
        //        MenuDetails.MenuControllerName = MenuController.First();
        //        MenuDetails.Add = OutAdd;
        //        MenuDetails.Edit = OutEdit;
        //        MenuDetails.Delete = OutDelete;
        //        MenuDetails.View = OutView;
        //        MenuDetails.Generate = OutGenerate;
        //        MenuDetails.isChecked = false;
        //        MenuDetails.ParentID = ParentID.First();
        //        MenuDetails.Position = Position.First();
        //        MenuDetails.activeTopMenu = activeTopMenu.First();
        //        MenuDetails.activeSubMenu = activeSubMenu.First();

        //        ListMenuDetails.Add(MenuDetails);


        //    }


        //    var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

        //    foreach (var main in Main)
        //    {
        //        MenuDetails MenuDetailsMain = new MenuDetails();

        //        List<MenuDetailsSub> ListMenuDetailsSub = new List<MenuDetailsSub>();

        //        MenuDetailsMain.MenuID = main.MenuID;
        //        MenuDetailsMain.MenuName = main.MenuName;
        //        MenuDetailsMain.MenuControllerName = main.MenuControllerName;
        //        MenuDetailsMain.Add = main.Add;
        //        MenuDetailsMain.Edit = main.Edit;
        //        MenuDetailsMain.Delete = main.Delete;
        //        MenuDetailsMain.View = main.View;
        //        MenuDetailsMain.Generate = main.Generate;
        //        MenuDetailsMain.isChecked = false;
        //        MenuDetailsMain.ParentID = main.ParentID;
        //        MenuDetailsMain.Position = main.Position;
        //        MenuDetailsMain.activeTopMenu = main.activeTopMenu;
        //        MenuDetailsMain.activeSubMenu = main.activeSubMenu;

        //        var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

        //        foreach (var sub in Sub)
        //        {
        //            MenuDetailsSub MenuDetailsSub = new MenuDetailsSub();
        //            MenuDetailsSub.MenuID = sub.MenuID;
        //            MenuDetailsSub.MenuName = sub.MenuName;
        //            MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
        //            MenuDetailsSub.Add = sub.Add;
        //            MenuDetailsSub.Edit = sub.Edit;
        //            MenuDetailsSub.Delete = sub.Delete;
        //            MenuDetailsSub.View = sub.View;
        //            MenuDetailsSub.Generate = sub.Generate;
        //            MenuDetailsSub.isChecked = false;
        //            MenuDetailsSub.ParentID = sub.ParentID;
        //            MenuDetailsSub.Position = sub.Position;
        //            MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
        //            MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

        //            ListMenuDetailsSub.Add(MenuDetailsSub);
        //        }

        //        MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;
        //        ListMenuDetailsMain.Add(MenuDetailsMain);
        //    }

        //    return ListMenuDetailsMain;
        //}

        public List<MenuDetails> RoleAddMenu(int Roleids)
        {
            List<MenuDetails> ListMenuDetails = new List<MenuDetails>();
            List<MenuDetails> ListMenuDetailsMain = new List<MenuDetails>();

            var menuids = DB.Role_Menu_Access
         .Join
         (

             DB.Menu_Master,
             c => c.Menu_Id,
             d => d.Menu_Id,
             (c, d) => new { c, d }

         )
         .Where(e => e.c.Role_Id == Roleids).Where(g => g.d.Menu_Id == g.c.Menu_Id).GroupBy(e => new { e.d.Menu_Id })
         .Select(x => new FetchMenuDetails
         {
             MenuID = x.Key.Menu_Id,
             MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
             MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
             RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
             ControllerName = x.Select(c => c.d.Url).Distinct(),
             ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
            // Position = x.Select(c => c.d.Position.Value).Distinct(),
             //activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
             //activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()

         }).ToList();

            foreach (var menu in menuids)
            {
                int OutAdd = 0;
                int OutEdit = 0;
                int OutDelete = 0;
                int OutView = 0;
                int OutGenerate = 0;
                int OutApproval = 0;

                MenuDetails MenuDetails = new MenuDetails();

                var Menuid = menu.MenuID;
                var Menuprevillages = menu.MenuPrevilages;
                var MenuName = menu.MenuName;
                var MenuController = menu.ControllerName;
                var ParentID = menu.ParentID;
                var Position = menu.Position;
                //var activeTopMenu = menu.activeTopMenu;
                //var activeSubMenu = menu.activeSubMenu;

                int Add = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Add"]);
                int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Edit"]);
                int View = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["View"]);
                int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Delete"]);
                int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Generate"]);
                int Approval = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Approval"]);
                if (Add == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                        if (Add1 == 1)
                        {
                            OutAdd = 1;
                            break;
                        }
                    }
                }
                else if (Add == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                        if (Add1 == 2)
                        {
                            OutAdd = 2;
                            break;
                        }
                    }

                }

                if (Edit == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                        if (Edit1 == 1)
                        {
                            OutEdit = 1;
                            break;
                        }
                    }
                }
                else if (Edit == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                        if (Edit1 == 2)
                        {
                            OutEdit = 2;
                            break;
                        }
                    }
                }
                if (View == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                        if (View1 == 1)
                        {
                            OutView = 1;
                            break;
                        }
                    }

                }
                else if (View == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                        if (View1 == 2)
                        {
                            OutView = 2;
                            break;
                        }
                    }

                }
                if (Delete == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                        if (Delete1 == 1)
                        {
                            OutDelete = 1;
                            break;
                        }
                    }

                }
                else if (Delete == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                        if (Delete1 == 2)
                        {
                            OutDelete = 2;
                            break;
                        }
                    }

                }
                if (Generate == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                        if (Generate1 == 1)
                        {
                            OutGenerate = 1;
                            break;
                        }
                    }

                }
                else if (Generate == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                        if (Generate1 == 2)
                        {
                            OutGenerate = 2;
                            break;
                        }
                    }

                }
                if (Approval == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                        if (Approval1 == 1)
                        {
                            OutApproval = 1;
                            break;
                        }
                    }

                }
                else if (Approval == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                        if (Approval1 == 2)
                        {
                            OutApproval = 2;
                            break;
                        }
                    }

                }

                MenuDetails.MenuID = Menuid;
                MenuDetails.MenuName = MenuName.First();
                MenuDetails.MenuControllerName = MenuController.First();
                MenuDetails.Add = OutAdd;
                MenuDetails.Edit = OutEdit;
                MenuDetails.Delete = OutDelete;
                MenuDetails.View = OutView;
                MenuDetails.Generate = OutGenerate;
                MenuDetails.Approval = OutApproval;
                MenuDetails.isChecked = false;
                MenuDetails.ParentID = ParentID.First();
               // MenuDetails.Position = Position.First();

                //MenuDetails.activeTopMenu = activeTopMenu.First();
                //MenuDetails.activeSubMenu = activeSubMenu.First();

                ListMenuDetails.Add(MenuDetails);


            }


            var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

            foreach (var main in Main)
            {
                MenuDetails MenuDetailsMain = new MenuDetails();

                List<MenuDetailsSub> ListMenuDetailsSub = new List<MenuDetailsSub>();

                MenuDetailsMain.MenuID = main.MenuID;
                MenuDetailsMain.MenuName = main.MenuName;
                MenuDetailsMain.MenuControllerName = main.MenuControllerName;
                MenuDetailsMain.Add = main.Add;
                MenuDetailsMain.Edit = main.Edit;
                MenuDetailsMain.Delete = main.Delete;
                MenuDetailsMain.View = main.View;
                MenuDetailsMain.Generate = main.Generate;
                MenuDetailsMain.Approval = main.Approval;
                MenuDetailsMain.isChecked = false;
                MenuDetailsMain.ParentID = main.ParentID;
               // MenuDetailsMain.Position = main.Position;
                //MenuDetailsMain.activeTopMenu = main.activeTopMenu;
                //MenuDetailsMain.activeSubMenu = main.activeSubMenu;

                var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

                foreach (var sub in Sub)
                {
                    MenuDetailsSub MenuDetailsSub = new MenuDetailsSub();
                    MenuDetailsSub.MenuID = sub.MenuID;
                    MenuDetailsSub.MenuName = sub.MenuName;
                    MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
                    MenuDetailsSub.Add = sub.Add;
                    MenuDetailsSub.Edit = sub.Edit;
                    MenuDetailsSub.Delete = sub.Delete;
                    MenuDetailsSub.View = sub.View;
                    MenuDetailsSub.Generate = sub.Generate;
                    MenuDetailsSub.Approval = sub.Approval;
                    MenuDetailsSub.isChecked = false;
                    MenuDetailsSub.ParentID = sub.ParentID;
                    MenuDetailsSub.Position = sub.Position;
                    //MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
                    //MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

                    ListMenuDetailsSub.Add(MenuDetailsSub);
                }

                MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;
                ListMenuDetailsMain.Add(MenuDetailsMain);
            }

            return ListMenuDetailsMain;
        }

        public List<MenuDetailsR> Menu()
        {
            List<MenuDetailsR> ListMenuDetailsMain = new List<MenuDetailsR>();
            List<MenuDetailsR> ListMenuDetails = new List<MenuDetailsR>();


          //var menuids = DB.Role_Menu_Access
          //.Join
          //(
          //    DB.Menu_Master,
          //    c => c.Menu_Id,
          //    d => d.Menu_Id,
          //    (c, d) => new { c, d }

          //)
          //.Where(e => e.c.Role_Id == 1).Where(g => g.d.Menu_Id == g.c.Menu_Id).GroupBy(e => new { e.d.Menu_Id })
          //.Select(x => new FetchMenuDetails
          //{
          //    MenuID = x.Key.Menu_Id,
          //    MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
          //    MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
          //    RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
          //    ControllerName = x.Select(c => c.d.Url).Distinct(),
          //    ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
          //    Position = x.Select(c => c.d.Position.Value).Distinct(),
          //    //activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
          //    //activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()
          //}).ToList();

            var menuids = DB.Menu_Master                    
              .Select(x => new FetchMenuDetails1
              {
                  MenuID = x.Menu_Id,
                  MenuName = x.Menu_Name,
                  MenuPrevilages = x.Menu_Previlleges,
                 // RolePrevilages = x.Select(c => c.Menu_Previlleges).Distinct(),
                  ControllerName = x.Url,
                  ParentID = x.Parent_id,
                  Position = x.Position,
                  //activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
                  //activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()
              }).ToList();

            foreach (var menu in menuids)
            {
                MenuDetailsR MenuDetails = new MenuDetailsR();

                var Menuid = menu.MenuID;
                var Menuprevillages = menu.MenuPrevilages;
                var MenuName = menu.MenuName;
                var MenuController = menu.ControllerName;
                var ParentID = menu.ParentID;
                var Position = menu.Position;
                //var activeTopMenu = menu.activeTopMenu;
                //var activeSubMenu = menu.activeSubMenu;
                int OutAdd = 0;
                //var OtAdd = 0;
                //var OtEdit = 0;
                //var OtDelete = 0;
                //var OtView = 0;
                //var OtGenerate = 0;
                int OutEdit = 0;
                int OutDelete = 0;
                int OutView = 0;
                int OutGenerate = 0;
                int OutApproval = 0;

                int Add = Convert.ToInt32(JObject.Parse(Menuprevillages)["Add"]);
                int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages)["Edit"]);
                int View = Convert.ToInt32(JObject.Parse(Menuprevillages)["View"]);
                int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages)["Delete"]);
                int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages)["Generate"]);
                int Approval = Convert.ToInt32(JObject.Parse(Menuprevillages)["Approval"]);

                if (Add == 1)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                        //int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                        //if (Add1 == 1)
                        //{
                            OutAdd = 0;
                        //    break;
                        //}

                    //}
                }
                else if (Add == 2)
                {
                //    foreach (var roleP in menu.RolePrevilages)
                //    {
                //        int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                //        if (Add1 == 2)
                //        {
                            OutAdd = 2;
                    //        break;
                    //    }
                    //}
                }

                if (Edit == 1)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                    //    if (Edit1 == 1)
                    //    {
                            OutEdit = 0;
                    //        break;
                    //    }
                    //}
                }
                else if (Edit == 2)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                    //    if (Edit1 == 2)
                    //    {
                            OutEdit = 2;
                    //        break;
                    //    }
                    //}
                }
                if (View == 1)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                    //    if (View1 == 1)
                    //    {
                            OutView = 0;
                    //        break;
                    //    }
                    //}

                }
                else if (View == 2)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                    //    if (View1 == 2)
                    //    {
                            OutView = 2;
                    //        break;
                    //    }
                    //}

                }
                if (Delete == 1)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                    //    if (Delete1 == 1)
                    //    {
                            OutDelete = 0;
                    //        break;
                    //    }
                    //}
                }
                else if (Delete == 2)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                    //    if (Delete1 == 2)
                    //    {
                            OutDelete = 2;
                    //        break;
                    //    }
                    //}

                }
                if (Generate == 1)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                    //    if (Generate1 == 1)
                    //    {
                            OutGenerate = 0;
                    //        break;
                    //    }
                    //}

                }
                else if (Generate == 2)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                    //    if (Generate1 == 2)
                    //    {
                            OutGenerate = 2;
                    //        break;
                    //    }
                    //}

                }
                if (Approval == 1)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                    //    if (Approval1 == 1)
                    //    {
                            OutApproval = 0;
                    //        break;
                    //    }
                    //}

                }
                else if (Approval == 2)
                {
                    //foreach (var roleP in menu.RolePrevilages)
                    //{
                    //    int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                    //    if (Approval1 == 2)
                    //    {
                            OutApproval = 2;
                    //        break;
                    //    }
                    //}

                }

                //OutAdd = Convert.ToInt32(OtAdd);
                //OutEdit =  Convert.ToInt32(OtEdit);
                // OutDelete=  Convert.ToInt32(OtDelete);
                // OtView = Convert.ToInt32(OtView);
                // OutGenerate =  Convert.ToInt32(OtGenerate);


                MenuDetails.MenuID = Menuid;
                MenuDetails.MenuName = MenuName;
                MenuDetails.MenuControllerName = MenuController;
                MenuDetails.SELECTED = 0;
                MenuDetails.SELECTEDADD = OutAdd;
                MenuDetails.SELECTEDDELETE = OutDelete;
                MenuDetails.SELECTEDEDIT = OutEdit;
                MenuDetails.SELECTEDVIEW = OutView;
                MenuDetails.SELECTGENERATE = OutGenerate;
                MenuDetails.SELECTAPPROVAL = OutApproval;

                MenuDetails.isChecked = false;
                MenuDetails.ParentID = ParentID;
                MenuDetails.Position = Position;
                //MenuDetails.activeTopMenu = activeTopMenu.First();
                //MenuDetails.activeSubMenu = activeSubMenu.First();

                ListMenuDetails.Add(MenuDetails);
            }

            var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

            foreach (var main in Main)
            {
                MenuDetailsR MenuDetailsMain = new MenuDetailsR();

                List<MenuDetailsSubR> ListMenuDetailsSub = new List<MenuDetailsSubR>();

                MenuDetailsMain.MenuID = main.MenuID;
                MenuDetailsMain.MenuName = main.MenuName;
                MenuDetailsMain.MenuControllerName = main.MenuControllerName;
                MenuDetailsMain.SELECTED = 0;
                MenuDetailsMain.SELECTEDADD = main.SELECTEDADD;
                MenuDetailsMain.SELECTEDDELETE = main.SELECTEDDELETE;
                MenuDetailsMain.SELECTEDEDIT = main.SELECTEDEDIT;
                MenuDetailsMain.SELECTEDVIEW = main.SELECTEDVIEW;
                MenuDetailsMain.SELECTGENERATE = main.SELECTGENERATE;
                MenuDetailsMain.SELECTAPPROVAL = main.SELECTAPPROVAL;
                MenuDetailsMain.isChecked = false;
                MenuDetailsMain.ParentID = main.ParentID;
                MenuDetailsMain.Position = main.Position;
                //MenuDetailsMain.activeTopMenu = main.activeTopMenu;
                //MenuDetailsMain.activeSubMenu = main.activeSubMenu;

                var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

                foreach (var sub in Sub)
                {
                    MenuDetailsSubR MenuDetailsSub = new MenuDetailsSubR();
                    MenuDetailsSub.MenuID = sub.MenuID;
                    MenuDetailsSub.MenuName = sub.MenuName;
                    MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
                    MenuDetailsSub.SELECTED = 0;
                    MenuDetailsSub.SELECTEDADD = sub.SELECTEDADD;
                    MenuDetailsSub.SELECTEDDELETE = sub.SELECTEDDELETE;
                    MenuDetailsSub.SELECTEDEDIT = sub.SELECTEDEDIT;
                    MenuDetailsSub.SELECTEDVIEW = sub.SELECTEDVIEW;
                    MenuDetailsSub.SELECTEDGENERATE = sub.SELECTGENERATE;
                    MenuDetailsSub.SELECTEDAPPROVAL = sub.SELECTAPPROVAL;
                    MenuDetailsSub.isChecked = false;
                    MenuDetailsSub.ParentID = sub.ParentID;
                    MenuDetailsSub.Position = sub.Position;
                    //MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
                    //MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

                    ListMenuDetailsSub.Add(MenuDetailsSub);
                }

                MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;

                ListMenuDetailsMain.Add(MenuDetailsMain);
            }

            return ListMenuDetailsMain;

        }
        //public List<MenuDetailsR> Menu()
        //{
        //    List<MenuDetailsR> ListMenuDetailsMain = new List<MenuDetailsR>();
        //    List<MenuDetailsR> ListMenuDetails = new List<MenuDetailsR>();

        //    var menuids = DB.Role_menu_permission
        //  .Join
        //  (
        //      DB.Menu_master,
        //      c => c.Menu_id,
        //      d => d.Menu_id,
        //      (c, d) => new { c, d }

        //  )
        //  .Where(e => e.c.Role_id == 3).Where(g => g.d.Menu_id == g.c.Menu_id).GroupBy(e => new { e.d.Menu_id })
        //  .Select(x => new FetchMenuDetails
        //  {
        //      MenuID = x.Key.Menu_id,
        //      MenuName = x.Select(c => c.d.Menu_name).Distinct(),
        //      MenuPrevilages = x.Select(c => c.d.Menu_previllages).Distinct(),
        //      RolePrevilages = x.Select(c => c.c.Role_previllages).Distinct(),
        //      ControllerName = x.Select(c => c.d.Menu_controller_name).Distinct(),
        //      ParentID = x.Select(c => c.d.Parent_id).Distinct(),
        //      Position = x.Select(c => c.d.Position).Distinct(),
        //      activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
        //      activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()
        //  }).ToList();

        //    foreach (var menu in menuids)
        //    {
        //        MenuDetailsR MenuDetails = new MenuDetailsR();

        //        var Menuid = menu.MenuID;
        //        var Menuprevillages = menu.MenuPrevilages;
        //        var MenuName = menu.MenuName;
        //        var MenuController = menu.ControllerName;
        //        var ParentID = menu.ParentID;
        //        var Position = menu.Position;
        //        var activeTopMenu = menu.activeTopMenu;
        //        var activeSubMenu = menu.activeSubMenu;
        //        int OutAdd = 0;
        //        //var OtAdd = 0;
        //        //var OtEdit = 0;
        //        //var OtDelete = 0;
        //        //var OtView = 0;
        //        //var OtGenerate = 0;
        //        int OutEdit = 0;
        //        int OutDelete = 0;
        //        int OutView = 0;
        //        int OutGenerate = 0;


        //        int Add = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Add"]);
        //        int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Edit"]);
        //        int View = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["View"]);
        //        int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Delete"]);
        //        int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Generate"]);

        //        if (Add == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

        //                if (Add1 == 1)
        //                {
        //                    OutAdd = 1;
        //                    break;
        //                }

        //            }
        //        }
        //        else if (Add == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

        //                if (Add1 == 2)
        //                {
        //                    OutAdd = 2;
        //                    break;
        //                }
        //            }
        //        }

        //        if (Edit == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

        //                if (Edit1 == 1)
        //                {
        //                    OutEdit = 1;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (Edit == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

        //                if (Edit1 == 2)
        //                {
        //                    OutEdit = 2;
        //                    break;
        //                }
        //            }
        //        }
        //        if (View == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

        //                if (View1 == 1)
        //                {
        //                    OutView = 1;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (View == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

        //                if (View1 == 2)
        //                {
        //                    OutView = 2;
        //                    break;
        //                }
        //            }

        //        }
        //        if (Delete == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

        //                if (Delete1 == 1)
        //                {
        //                    OutDelete = 1;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (Delete == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

        //                if (Delete1 == 2)
        //                {
        //                    OutDelete = 2;
        //                    break;
        //                }
        //            }

        //        }
        //        if (Generate == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

        //                if (Generate1 == 1)
        //                {
        //                    OutGenerate = 1;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (Generate == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

        //                if (Generate1 == 2)
        //                {
        //                    OutGenerate = 2;
        //                    break;
        //                }
        //            }

        //        }

        //        //OutAdd = Convert.ToInt32(OtAdd);
        //        //OutEdit =  Convert.ToInt32(OtEdit);
        //        // OutDelete=  Convert.ToInt32(OtDelete);
        //        // OtView = Convert.ToInt32(OtView);
        //        // OutGenerate =  Convert.ToInt32(OtGenerate);


        //        MenuDetails.MenuID = Menuid;
        //        MenuDetails.MenuName = MenuName.First();
        //        MenuDetails.MenuControllerName = MenuController.First();
        //        MenuDetails.SELECTED = 0;
        //        MenuDetails.SELECTEDADD = OutAdd;
        //        MenuDetails.SELECTEDDELETE = OutDelete;
        //        MenuDetails.SELECTEDEDIT = OutEdit;
        //        MenuDetails.SELECTEDVIEW = OutView;
        //        MenuDetails.SELECTGENERATE = OutGenerate;

        //        MenuDetails.isChecked = false;
        //        MenuDetails.ParentID = ParentID.First();
        //        MenuDetails.Position = Position.First();
        //        MenuDetails.activeTopMenu = activeTopMenu.First();
        //        MenuDetails.activeSubMenu = activeSubMenu.First();

        //        ListMenuDetails.Add(MenuDetails);
        //    }

        //    var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

        //    foreach (var main in Main)
        //    {
        //        MenuDetailsR MenuDetailsMain = new MenuDetailsR();

        //        List<MenuDetailsSubR> ListMenuDetailsSub = new List<MenuDetailsSubR>();

        //        MenuDetailsMain.MenuID = main.MenuID;
        //        MenuDetailsMain.MenuName = main.MenuName;
        //        MenuDetailsMain.MenuControllerName = main.MenuControllerName;
        //        MenuDetailsMain.SELECTED = 0;
        //        MenuDetailsMain.SELECTEDADD = main.SELECTEDADD;
        //        MenuDetailsMain.SELECTEDDELETE = main.SELECTEDDELETE;
        //        MenuDetailsMain.SELECTEDEDIT = main.SELECTEDEDIT;
        //        MenuDetailsMain.SELECTEDVIEW = main.SELECTEDVIEW;
        //        MenuDetailsMain.SELECTGENERATE = main.SELECTGENERATE;
        //        MenuDetailsMain.isChecked = false;
        //        MenuDetailsMain.ParentID = main.ParentID;
        //        MenuDetailsMain.Position = main.Position;
        //        MenuDetailsMain.activeTopMenu = main.activeTopMenu;
        //        MenuDetailsMain.activeSubMenu = main.activeSubMenu;

        //        var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

        //        foreach (var sub in Sub)
        //        {
        //            MenuDetailsSubR MenuDetailsSub = new MenuDetailsSubR();
        //            MenuDetailsSub.MenuID = sub.MenuID;
        //            MenuDetailsSub.MenuName = sub.MenuName;
        //            MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
        //            MenuDetailsSub.SELECTED = 0;
        //            MenuDetailsSub.SELECTEDADD = sub.SELECTEDADD;
        //            MenuDetailsSub.SELECTEDDELETE = sub.SELECTEDDELETE;
        //            MenuDetailsSub.SELECTEDEDIT = sub.SELECTEDEDIT;
        //            MenuDetailsSub.SELECTEDVIEW = sub.SELECTEDVIEW;
        //            MenuDetailsSub.SELECTEDGENERATE = sub.SELECTGENERATE;
        //            MenuDetailsSub.isChecked = false;
        //            MenuDetailsSub.ParentID = sub.ParentID;
        //            MenuDetailsSub.Position = sub.Position;
        //            MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
        //            MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

        //            ListMenuDetailsSub.Add(MenuDetailsSub);
        //        }

        //        MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;

        //        ListMenuDetailsMain.Add(MenuDetailsMain);
        //    }

        //    return ListMenuDetailsMain;

        //}
        //public List<MenuDetailsR> MenuM()
        //{
        //    List<MenuDetailsR> ListMenuDetailsMain = new List<MenuDetailsR>();
        //    List<MenuDetailsR> ListMenuDetails = new List<MenuDetailsR>();
        //    var mn=from y in DB.Menu_master
        //           group y by y.Menu_id into e
        //           select new FetchMenuDetails
        //           {
        //               MenuID=e.Key.Menu_id
        //           }

        //var menuids = DB.Menu_master.GroupBy(e => new { e.Menu_id })
        //      .Select(x => new FetchMenuDetails
        //      {
        //          MenuID = e.Key.Menu_id,
        //          MenuName = x.Select(c => c.d.Menu_name).Distinct(),
        //          MenuPrevilages = x.Select(c => c.d.Menu_previllages).Distinct(),
        //          RolePrevilages = x.Select(c => c.c.Role_previllages).Distinct(),
        //          ControllerName = x.Select(c => c.d.Menu_controller_name).Distinct(),
        //          ParentID = x.Select(c => c.d.Parent_id).Distinct(),
        //          Position = x.Select(c => c.d.Position).Distinct(),
        //          activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
        //          activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()
        //      }).ToList();       

        //  var menuids = DB.Role_menu_permission
        //.Join
        //(
        //    DB.Menu_master,
        //    c => c.Menu_id,
        //    d => d.Menu_id,
        //    (c, d) => new { c, d }

        //)
        //.Where(e => e.c.Role_id == 3).Where(g => g.d.Menu_id == g.c.Menu_id).GroupBy(e => new { e.d.Menu_id })
        //.Select(x => new FetchMenuDetails
        //{
        //    MenuID = x.Key.Menu_id,
        //    MenuName = x.Select(c => c.d.Menu_name).Distinct(),
        //    MenuPrevilages = x.Select(c => c.d.Menu_previllages).Distinct(),
        //    RolePrevilages = x.Select(c => c.c.Role_previllages).Distinct(),
        //    ControllerName = x.Select(c => c.d.Menu_controller_name).Distinct(),
        //    ParentID = x.Select(c => c.d.Parent_id).Distinct(),
        //    Position = x.Select(c => c.d.Position).Distinct(),
        //    activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
        //    activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()
        //}).ToList();

        //    foreach (var menu in menuids)
        //    {
        //        MenuDetailsR MenuDetails = new MenuDetailsR();

        //        var Menuid = menu.MenuID;
        //        var Menuprevillages = menu.MenuPrevilages;
        //        var MenuName = menu.MenuName;
        //        var MenuController = menu.ControllerName;
        //        var ParentID = menu.ParentID;
        //        var Position = menu.Position;
        //        var activeTopMenu = menu.activeTopMenu;
        //        var activeSubMenu = menu.activeSubMenu;
        //        int OutAdd = 0;
        //        //var OtAdd = 0;
        //        //var OtEdit = 0;
        //        //var OtDelete = 0;
        //        //var OtView = 0;
        //        //var OtGenerate = 0;
        //        int OutEdit = 0;
        //        int OutDelete = 0;
        //        int OutView = 0;
        //        int OutGenerate = 0;


        //        int Add = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Add"]);
        //        int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Edit"]);
        //        int View = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["View"]);
        //        int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Delete"]);
        //        int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Generate"]);

        //        if (Add == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

        //                if (Add1 == 1)
        //                {
        //                    OutAdd = 0;
        //                    break;
        //                }

        //            }
        //        }
        //        else if (Add == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

        //                if (Add1 == 2)
        //                {
        //                    OutAdd = 2;
        //                    break;
        //                }
        //            }
        //        }

        //        if (Edit == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

        //                if (Edit1 == 1)
        //                {
        //                    OutEdit = 0;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (Edit == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

        //                if (Edit1 == 2)
        //                {
        //                    OutEdit = 2;
        //                    break;
        //                }
        //            }
        //        }
        //        if (View == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

        //                if (View1 == 1)
        //                {
        //                    OutView = 0;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (View == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

        //                if (View1 == 2)
        //                {
        //                    OutView = 2;
        //                    break;
        //                }
        //            }

        //        }
        //        if (Delete == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

        //                if (Delete1 == 1)
        //                {
        //                    OutDelete = 0;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (Delete == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

        //                if (Delete1 == 2)
        //                {
        //                    OutDelete = 2;
        //                    break;
        //                }
        //            }

        //        }
        //        if (Generate == 1)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

        //                if (Generate1 == 1)
        //                {
        //                    OutGenerate = 0;
        //                    break;
        //                }
        //            }

        //        }
        //        else if (Generate == 2)
        //        {
        //            foreach (var roleP in menu.RolePrevilages)
        //            {
        //                int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

        //                if (Generate1 == 2)
        //                {
        //                    OutGenerate = 2;
        //                    break;
        //                }
        //            }

        //        }

        //        //OutAdd = Convert.ToInt32(OtAdd);
        //        //OutEdit =  Convert.ToInt32(OtEdit);
        //        // OutDelete=  Convert.ToInt32(OtDelete);
        //        // OtView = Convert.ToInt32(OtView);
        //        // OutGenerate =  Convert.ToInt32(OtGenerate);


        //        MenuDetails.MenuID = Menuid;
        //        MenuDetails.MenuName = MenuName.First();
        //        MenuDetails.MenuControllerName = MenuController.First();
        //        MenuDetails.SELECTED = 0;
        //        MenuDetails.SELECTEDADD = OutAdd;
        //        MenuDetails.SELECTEDDELETE = OutDelete;
        //        MenuDetails.SELECTEDEDIT = OutEdit;
        //        MenuDetails.SELECTEDVIEW = OutView;
        //        MenuDetails.SELECTGENERATE = OutGenerate;

        //        MenuDetails.isChecked = false;
        //        MenuDetails.ParentID = ParentID.First();
        //        MenuDetails.Position = Position.First();
        //        MenuDetails.activeTopMenu = activeTopMenu.First();
        //        MenuDetails.activeSubMenu = activeSubMenu.First();

        //        ListMenuDetails.Add(MenuDetails);
        //    }

        //    var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

        //    foreach (var main in Main)
        //    {
        //        MenuDetailsR MenuDetailsMain = new MenuDetailsR();

        //        List<MenuDetailsSubR> ListMenuDetailsSub = new List<MenuDetailsSubR>();

        //        MenuDetailsMain.MenuID = main.MenuID;
        //        MenuDetailsMain.MenuName = main.MenuName;
        //        MenuDetailsMain.MenuControllerName = main.MenuControllerName;
        //        MenuDetailsMain.SELECTED = 0;
        //        MenuDetailsMain.SELECTEDADD = main.SELECTEDADD;
        //        MenuDetailsMain.SELECTEDDELETE = main.SELECTEDDELETE;
        //        MenuDetailsMain.SELECTEDEDIT = main.SELECTEDEDIT;
        //        MenuDetailsMain.SELECTEDVIEW = main.SELECTEDVIEW;
        //        MenuDetailsMain.SELECTGENERATE = main.SELECTGENERATE;
        //        MenuDetailsMain.isChecked = false;
        //        MenuDetailsMain.ParentID = main.ParentID;
        //        MenuDetailsMain.Position = main.Position;
        //        MenuDetailsMain.activeTopMenu = main.activeTopMenu;
        //        MenuDetailsMain.activeSubMenu = main.activeSubMenu;

        //        var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

        //        foreach (var sub in Sub)
        //        {
        //            MenuDetailsSubR MenuDetailsSub = new MenuDetailsSubR();
        //            MenuDetailsSub.MenuID = sub.MenuID;
        //            MenuDetailsSub.MenuName = sub.MenuName;
        //            MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
        //            MenuDetailsSub.SELECTED = 0;
        //            MenuDetailsSub.SELECTEDADD = sub.SELECTEDADD;
        //            MenuDetailsSub.SELECTEDDELETE = sub.SELECTEDDELETE;
        //            MenuDetailsSub.SELECTEDEDIT = sub.SELECTEDEDIT;
        //            MenuDetailsSub.SELECTEDVIEW = sub.SELECTEDVIEW;
        //            MenuDetailsSub.SELECTEDGENERATE = sub.SELECTGENERATE;
        //            MenuDetailsSub.isChecked = false;
        //            MenuDetailsSub.ParentID = sub.ParentID;
        //            MenuDetailsSub.Position = sub.Position;
        //            MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
        //            MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

        //            ListMenuDetailsSub.Add(MenuDetailsSub);
        //        }

        //        MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;

        //        ListMenuDetailsMain.Add(MenuDetailsMain);
        //    }

        //    return ListMenuDetailsMain;

        //}

        public List<MenuDetailsP> MenuPermission()
        {
            List<MenuDetailsP> ListMenuDetailsMain = new List<MenuDetailsP>();
            List<MenuDetailsP> ListMenuDetails = new List<MenuDetailsP>();

            var menuids = DB.Role_Menu_Access
          .Join
          (
              DB.Menu_Master,
              c => c.Menu_Id,
              d => d.Menu_Id,
              (c, d) => new { c, d }

          )
          .Where(e => e.c.Role_Id == 1).Where(g => g.d.Menu_Id == g.c.Menu_Id).GroupBy(e => new { e.d.Menu_Id })
          .Select(x => new FetchMenuDetails
          {
              MenuID = x.Key.Menu_Id,
              MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
              MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
              RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
              ControllerName = x.Select(c => c.d.Url).Distinct(),
              ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
              Position = x.Select(c => c.d.Position.Value).Distinct(),
              //activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
              //activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()
          }).ToList();

            foreach (var menu in menuids)
            {
                MenuDetailsP MenuDetails = new MenuDetailsP();

                var Menuid = menu.MenuID;
                var Menuprevillages = menu.MenuPrevilages;
                var MenuName = menu.MenuName;
                var MenuController = menu.ControllerName;
                var ParentID = menu.ParentID;
                var Position = menu.Position;
                //var activeTopMenu = menu.activeTopMenu;
                //var activeSubMenu = menu.activeSubMenu;
                int OutAdd = 0;
                int OutEdit = 0;
                int OutDelete = 0;
                int OutView = 0;
                int OutGenerate = 0;
                int OutApproval = 0;


                int Add = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Add"]);
                int Edit = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Edit"]);
                int View = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["View"]);
                int Delete = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Delete"]);
                int Generate = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Generate"]);
                int Approval = Convert.ToInt32(JObject.Parse(Menuprevillages.First())["Approval"]);

                if (Add == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                        if (Add1 == 1)
                        {
                            OutAdd = 1;
                            break;
                        }
                    }
                }
                else if (Add == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Add1 = Convert.ToInt32(JObject.Parse(roleP)["Add"]);

                        if (Add1 == 2)
                        {
                            OutAdd = 2;
                            break;
                        }
                    }
                }

                if (Edit == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                        if (Edit1 == 1)
                        {
                            OutEdit = 1;
                            break;
                        }
                    }
                }
                else if (Edit == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Edit1 = Convert.ToInt32(JObject.Parse(roleP)["Edit"]);

                        if (Edit1 == 2)
                        {
                            OutEdit = 2;
                            break;
                        }
                    }
                }
                if (View == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                        if (View1 == 1)
                        {
                            OutView = 1;
                            break;
                        }
                    }

                }
                else if (View == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int View1 = Convert.ToInt32(JObject.Parse(roleP)["View"]);

                        if (View1 == 2)
                        {
                            OutView = 2;
                            break;
                        }
                    }

                }
                if (Delete == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                        if (Delete1 == 1)
                        {
                            OutDelete = 1;
                            break;
                        }
                    }
                }
                else if (Delete == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Delete1 = Convert.ToInt32(JObject.Parse(roleP)["Delete"]);

                        if (Delete1 == 2)
                        {
                            OutDelete = 2;
                            break;
                        }
                    }

                }
                if (Generate == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                        if (Generate1 == 1)
                        {
                            OutGenerate = 1;
                            break;
                        }
                    }

                }
                else if (Generate == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Generate1 = Convert.ToInt32(JObject.Parse(roleP)["Generate"]);

                        if (Generate1 == 2)
                        {
                            OutGenerate = 2;
                            break;
                        }
                    }

                }
                if (Approval == 1)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                        if (Approval1 == 1)
                        {
                            OutApproval = 1;
                            break;
                        }
                    }

                }
                else if (Approval == 2)
                {
                    foreach (var roleP in menu.RolePrevilages)
                    {
                        int Approval1 = Convert.ToInt32(JObject.Parse(roleP)["Approval"]);

                        if (Approval1 == 2)
                        {
                            OutApproval = 2;
                            break;
                        }
                    }

                }


                MenuDetails.MenuID = Menuid;
                //   MenuDetails.MenuName = MenuName.First();
                //   MenuDetails.MenuControllerName = MenuController.First();
                //   MenuDetails.SELECTED = 0;
                MenuDetails.ADD = OutAdd;
                MenuDetails.DELETE = OutDelete;
                MenuDetails.EDIT = OutEdit;
                MenuDetails.VIEW = OutView;
                MenuDetails.GENERATE = OutGenerate;
                MenuDetails.APPROVAL = OutApproval;

                //  MenuDetails.isChecked = false;
                MenuDetails.ParentID = ParentID.First();
                MenuDetails.Position = Position.First();
                //   MenuDetails.activeTopMenu = activeTopMenu.First();
                //   MenuDetails.activeSubMenu = activeSubMenu.First();

                ListMenuDetails.Add(MenuDetails);
            }

            var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();

            foreach (var main in Main)
            {
                MenuDetailsP MenuDetailsMain = new MenuDetailsP();

                List<MenuDetailsSubP> ListMenuDetailsSub = new List<MenuDetailsSubP>();

                MenuDetailsMain.MenuID = main.MenuID;
                // MenuDetailsMain.MenuName = main.MenuName;
                // MenuDetailsMain.MenuControllerName = main.MenuControllerName;
                //  MenuDetailsMain.SELECTED = 0;
                MenuDetailsMain.ADD = main.ADD;
                MenuDetailsMain.DELETE = main.DELETE;
                MenuDetailsMain.EDIT = main.EDIT;
                MenuDetailsMain.VIEW = main.VIEW;
                MenuDetailsMain.GENERATE = main.GENERATE;
                MenuDetailsMain.APPROVAL = main.APPROVAL;
                //   MenuDetailsMain.isChecked = false;
                MenuDetailsMain.ParentID = main.ParentID;
                MenuDetailsMain.Position = main.Position;
                // MenuDetailsMain.activeTopMenu = main.activeTopMenu;
                // MenuDetailsMain.activeSubMenu = main.activeSubMenu;

                var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

                foreach (var sub in Sub)
                {
                    MenuDetailsSubP MenuDetailsSub = new MenuDetailsSubP();
                    MenuDetailsSub.MenuID = sub.MenuID;
                    //   MenuDetailsSub.MenuName = sub.MenuName;
                    //  MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
                    //   MenuDetailsSub.SELECTED = 0;
                    MenuDetailsSub.ADD = sub.ADD;
                    MenuDetailsSub.DELETE = sub.DELETE;
                    MenuDetailsSub.EDIT = sub.EDIT;
                    MenuDetailsSub.VIEW = sub.VIEW;
                    MenuDetailsSub.GENERATE = sub.GENERATE;
                    MenuDetailsSub.APPROVAL = sub.APPROVAL;
                    // MenuDetailsSub.isChecked = false;
                    // MenuDetailsSub.ParentID = sub.ParentID;
                    // MenuDetailsSub.Position = sub.Position;
                    //   MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
                    //   MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

                    ListMenuDetailsSub.Add(MenuDetailsSub);
                }

                MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;

                ListMenuDetailsMain.Add(MenuDetailsMain);
            }

            return ListMenuDetailsMain;

        }


        public List<MenuDetailsR> RoleMenu()
        {

            List<MenuDetailsR> ListMenuDetailsMain = new List<MenuDetailsR>();
            List<MenuDetailsR> ListMenuDetails = new List<MenuDetailsR>();


            var menuids = DB.Role_Menu_Access
          .Join
          (

              DB.Menu_Master,
              c => c.Menu_Id,
              d => d.Menu_Id,
              (c, d) => new { c, d }

          )
          .Where(e => e.c.Role_Id == 1).Where(g => g.d.Menu_Id == g.c.Menu_Id).GroupBy(e => new { e.d.Menu_Id })
          .Select(x => new FetchMenuDetails

          {
              MenuID = x.Key.Menu_Id,
              MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
              MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
              RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
              ControllerName = x.Select(c => c.d.Url).Distinct(),
              ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
              Position = x.Select(c => c.d.Position.Value).Distinct(),
              //activeTopMenu = x.Select(c => c.d.Active_top_menu_class).Distinct(),
              //activeSubMenu = x.Select(c => c.d.Active_left_menu_class).Distinct()

          }).ToList();

            foreach (var menu in menuids)
            {



                MenuDetailsR MenuDetails = new MenuDetailsR();


                var Menuid = menu.MenuID;
                var Menuprevillages = menu.MenuPrevilages;
                var MenuName = menu.MenuName;
                var MenuController = menu.ControllerName;
                var ParentID = menu.ParentID;
                var Position = menu.Position;
                //var activeTopMenu = menu.activeTopMenu;
                //var activeSubMenu = menu.activeSubMenu;



                MenuDetails.MenuID = Menuid;
                MenuDetails.MenuName = MenuName.First();
                MenuDetails.MenuControllerName = MenuController.First();
                MenuDetails.SELECTED = 0;
                MenuDetails.SELECTEDADD = 0;
                MenuDetails.SELECTEDDELETE = 0;
                MenuDetails.SELECTEDEDIT = 0;
                MenuDetails.SELECTEDVIEW = 0;
                MenuDetails.SELECTAPPROVAL = 0;

                MenuDetails.isChecked = false;
                MenuDetails.ParentID = ParentID.First();
                MenuDetails.Position = Position.First();
                //MenuDetails.activeTopMenu = activeTopMenu.First();
                //MenuDetails.activeSubMenu = activeSubMenu.First();

                ListMenuDetails.Add(MenuDetails);



            }


            var Main = ListMenuDetails.Where(c => c.ParentID == 0).OrderBy(d => d.Position).ToList();



            foreach (var main in Main)
            {
                MenuDetailsR MenuDetailsMain = new MenuDetailsR();

                List<MenuDetailsSubR> ListMenuDetailsSub = new List<MenuDetailsSubR>();

                MenuDetailsMain.MenuID = main.MenuID;
                MenuDetailsMain.MenuName = main.MenuName;
                MenuDetailsMain.MenuControllerName = main.MenuControllerName;
                MenuDetailsMain.SELECTED = 0;
                MenuDetailsMain.SELECTEDADD = 0;
                MenuDetailsMain.SELECTEDDELETE = 0;
                MenuDetailsMain.SELECTEDEDIT = 0;
                MenuDetailsMain.SELECTEDVIEW = 0;
                MenuDetailsMain.SELECTAPPROVAL = 0;
                MenuDetailsMain.isChecked = false;
                MenuDetailsMain.ParentID = main.ParentID;
                MenuDetailsMain.Position = main.Position;
                //MenuDetailsMain.activeTopMenu = main.activeTopMenu;
                //MenuDetailsMain.activeSubMenu = main.activeSubMenu;

                var Sub = ListMenuDetails.Where(c => c.ParentID == main.MenuID).OrderBy(d => d.Position).ToList();

                foreach (var sub in Sub)
                {
                    MenuDetailsSubR MenuDetailsSub = new MenuDetailsSubR();
                    MenuDetailsSub.MenuID = sub.MenuID;
                    MenuDetailsSub.MenuName = sub.MenuName;
                    MenuDetailsSub.MenuControllerName = sub.MenuControllerName;
                    MenuDetailsSub.SELECTED = 0;
                    MenuDetailsSub.SELECTEDADD = 0;
                    MenuDetailsSub.SELECTEDDELETE = 0;
                    MenuDetailsSub.SELECTEDEDIT = 0;
                    MenuDetailsSub.SELECTEDVIEW = 0;
                    MenuDetailsSub.SELECTEDAPPROVAL = 0;
                    MenuDetailsSub.isChecked = false;
                    MenuDetailsSub.ParentID = sub.ParentID;
                    MenuDetailsSub.Position = sub.Position;
                    //MenuDetailsSub.activeTopMenu = sub.activeTopMenu;
                    //MenuDetailsSub.activeSubMenu = sub.activeSubMenu;

                    ListMenuDetailsSub.Add(MenuDetailsSub);
                }

                MenuDetailsMain.MenuDetailssub = ListMenuDetailsSub;

                ListMenuDetailsMain.Add(MenuDetailsMain);
            }

            return ListMenuDetailsMain;

        }

    }
}
