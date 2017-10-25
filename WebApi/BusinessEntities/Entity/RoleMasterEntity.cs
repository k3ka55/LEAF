using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class RoleMasterEntity
    {
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }

        //====================================
        public List<MenuDetailsR> Menu { get; set; }
    }

    public class MenuDetailsR
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuControllerName { get; set; }
        public bool isChecked { get; set; }
        public int? ParentID { get; set; }
        public int? Position { get; set; }
        //public string activeTopMenu { get; set; }
        //public string activeSubMenu { get; set; }
        public int SELECTED { get; set; }
        public int SELECTEDADD { get; set; }
        public int SELECTEDEDIT { get; set; }
        public int SELECTEDVIEW { get; set; }
        public int SELECTEDDELETE { get; set; }
        public int SELECTGENERATE { get; set; }
        public int SELECTAPPROVAL { get; set; }
        //public int ADD { get; set; }
        //public int EDIT { get; set; }
        //public int VIEW { get; set; }
        //public int DELETE { get; set; }
        //public int GENERATE { get; set; }
        public IEnumerable<MenuDetailsSubR> MenuDetailssub { get; set; }
        public List<MenuDetailsSubR> MenuMenuDSub { get; set; }
    }

    public class MenuDetailsSubR
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuControllerName { get; set; }
        public bool isChecked { get; set; }
        public int? ParentID { get; set; }
        public int? Position { get; set; }
        //public string activeTopMenu { get; set; }
        //public string activeSubMenu { get; set; }
        public int SELECTED { get; set; }
        public int SELECTEDADD { get; set; }
        public int SELECTEDEDIT { get; set; }
        public int SELECTEDVIEW { get; set; }
        public int SELECTEDDELETE { get; set; }
        public int SELECTEDGENERATE { get; set; }
        public int SELECTEDAPPROVAL { get; set; }
        //public int ADD { get; set; }
        //public int EDIT { get; set; }
        //public int VIEW { get; set; }
        //public int DELETE { get; set; }
        //public int GENERATE { get; set; }


    }


    public class RoleId
    {
        public int Role_Id { get; set; }

        public string Role_name { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<MenuDetailsR> Menu { get; set; }
    }

    public class MenuRoleId
    {
        public int Role_Id { get; set; }


    }

    public class RoleList
    {
        public int Role_ids { get; set; }
        public string Role_name { get; set; }
        public System.DateTime Created_date { get; set; }
        public int Count { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }

    }

    public class MenuDetailsP
    {
        public int MenuID { get; set; }
        //public string MenuName { get; set; }
        //public string MenuControllerName { get; set; }
       public bool isChecked { get; set; }
        public int ParentID { get; set; }
        public int Position { get; set; }
        // public string activeTopMenu { get; set; }
        // public string activeSubMenu { get; set; }
        //  public int SELECTED { get; set; }
        public int ADD { get; set; }
        public int EDIT { get; set; }
        public int VIEW { get; set; }
        public int DELETE { get; set; }
        public int GENERATE { get; set; }
        public int APPROVAL { get; set; }
        public IEnumerable<MenuDetailsSubP> MenuDetailssub { get; set; }
    }
    public class MenuDetails
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuControllerName { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int Delete { get; set; }
        public int View { get; set; }
        public int Generate { get; set; }
        public int Approval { get; set; }
      public bool isChecked { get; set; }
        public int ParentID { get; set; }
        public int Position { get; set; }
        //public string activeTopMenu { get; set; }
        //public string activeSubMenu { get; set; }
        public IEnumerable<MenuDetailsSub> MenuDetailssub { get; set; }
    }


    public class MenuDetailsSub
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuControllerName { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int Delete { get; set; }
        public int View { get; set; }
        public int Generate { get; set; }
        public int Approval { get; set; }
        public bool isChecked { get; set; }
        public int ParentID { get; set; }
        public int Position { get; set; }
        //public string activeTopMenu { get; set; }
        //public string activeSubMenu { get; set; }

    }
    public class FetchMenuDetails
    {
        public int MenuID { get; set; }
        public IEnumerable<string> MenuName { get; set; }
        public IEnumerable<string> MenuPrevilages { get; set; }
        public IEnumerable<string> RolePrevilages { get; set; }

        public IEnumerable<string> ControllerName { get; set; }
        public IEnumerable<int> ParentID { get; set; }
        public IEnumerable<int> Position { get; set; }
        //public IEnumerable<string> activeTopMenu { get; set; }
        //public IEnumerable<string> activeSubMenu { get; set; }

    }

    public class FetchMenuDetails1
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuPrevilages { get; set; }
        public IEnumerable<string> RolePrevilages { get; set; }

        public string ControllerName { get; set; }
        public int? ParentID { get; set; }
        public int? Position { get; set; }
        //public IEnumerable<string> activeTopMenu { get; set; }
        //public IEnumerable<string> activeSubMenu { get; set; }

    }

    public class MenuDetailsSubP
    {
        public int MenuID { get; set; }
        // public string MenuName { get; set; }
        //  public string MenuControllerName { get; set; }
        public bool isChecked { get; set; }
        // public int ParentID { get; set; }
        //  public int Position { get; set; }
        //  public string activeTopMenu { get; set; }
        //  public string activeSubMenu { get; set; }
        //  public int SELECTED { get; set; }
        public int ADD { get; set; }
        public int EDIT { get; set; }
        public int VIEW { get; set; }
        public int DELETE { get; set; }
        public int GENERATE { get; set; }
        public int APPROVAL { get; set; }

    }




}
