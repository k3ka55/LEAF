using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
//using BusinessServices;


namespace BusinessEntities
{
    //public class MenuJoinModule
    //{
    //    public string menuname { get; set; }
    //    public string url { get; set; }
    //    public string fa_name { get; set; }
    //    public string ng_class { get; set; }
    //    public IEnumerable<SubMenu> Sub_menu { get; set; }
    //}
    public class Mainmenu
    {
        public string menuname { get; set; }
        public string url { get; set; }
        public string fa_name { get; set; }
        public string ng_class { get; set; }
        public IEnumerable<SubMenu> Sub_menu { get; set; }
    }
    public class SubMenu
    {

        public string Menu_name { get; set; }
        public string url { get; set; }
        public string class_name { get; set; }
    }
    //public class MenuJoinModule1
    //{
    //    public IEnumerable<MenuJoinModule> Menu { get; set; }
    //    public HttpStatusCode StatusCode { get; set; }
    //}
}
