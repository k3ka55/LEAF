using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessEntities;
using DataModel;

namespace BusinessServices
{
    public interface IMenuService
    {
        List<MenuMasterEntity> GetMenuList(int id);
        int createMenu(MenuMasterEntity menu);
        bool updateMenu(int id, MenuMasterEntity menu);
        List<MenuMasterEntity> GetMenuListByID(int id);
        List<MenuMasterEntity> GetMenuLists();
        bool DeleteMenu(int menuID);
        List<MenuMasterEntity> GetMenuListA(int id, string From);
       // List<subMenuEntity> GetMenu(int? roleId, string Url = "null");
    }

}