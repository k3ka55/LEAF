using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class RoleEntity
    {
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public IEnumerable<RoleMenuAccessEntity> MenuAccess { get; set; }
    }

    public class RoleMenuAccessEntity
    {
        public int Menu_Role_Access_Id { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public string Menu_Name { get; set; }
        public Nullable<int> Menu_Id { get; set; }
        public Nullable<bool> is_Create { get; set; }
        public Nullable<bool> is_edit { get; set; }
        public Nullable<bool> is_Delete { get; set; }
        public Nullable<bool> is_View { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }        
    }
}
