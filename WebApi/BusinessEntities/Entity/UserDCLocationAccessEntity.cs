using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class UserDCLocationAccessEntity
    {
        public int DC_Access_User_Id { get; set; }
        public Nullable<int> DC_id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DC_Code { get; set; }
    }

    public class UserLocationAccessEntity
    {
        public int Location_Access_User_Id { get; set; }
        public Nullable<int> Location_id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
