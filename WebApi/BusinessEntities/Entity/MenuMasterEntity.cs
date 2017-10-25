using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class MenuMasterEntity
    {
        public int Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Url { get; set; }
        public string Icon_Name { get; set; }
        public string NG_Class { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> Mobile_Flag { get; set; }
        public Nullable<bool> Web_Flag { get; set; }
        public Nullable<int> Parent_id { get; set; }
        public Nullable<int> Sub_id { get; set; }
        public string Sub_Class { get; set; }
        public Nullable<bool> is_Create { get; set; }
        public Nullable<bool> is_Edit { get; set; }
        public Nullable<bool> is_Delete { get; set; }
        public Nullable<bool> is_View { get; set; }

        public List<subMenuEntity> SubmenuList { get; set; }
    }

    public class subMenuEntity
    {
        public int Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Url { get; set; }
        public string Icon_Name { get; set; }
        public string NG_Class { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> Parent_id { get; set; }
        public Nullable<int> Sub_id { get; set; }
        public string Sub_Class { get; set; }
        public Nullable<bool> is_Create { get; set; }
        public Nullable<bool> is_Edit { get; set; }
        public Nullable<bool> is_Delete { get; set; }
        public Nullable<bool> is_View { get; set; }

    }
}
