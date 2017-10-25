using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class LocationMasterEntity
    {
        public int Location_Id { get; set; }
        public string Location_Code { get; set; }
        public string Location_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public Nullable<int> PinCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Region_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }

    }
}
