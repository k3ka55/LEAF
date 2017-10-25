using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{

    public class UserModel
    {
        public int User_id { get; set; }
        public string User_Name { get; set; }
        public string User_Login_Type { get; set; }
    }
    public class UserDetailsEntity
    {
        public int User_id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email_Id { get; set; }
        public Nullable<long> Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string User_Login_Type { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }

        public List<UserDCLocationAccessEntity> userDCLocation { get; set; }
        public List<UserLocationAccessEntity> userLocation { get; set; }
        public List<UserRoleAccessEntity> userroleAccess { get; set; }
        
    }


}
