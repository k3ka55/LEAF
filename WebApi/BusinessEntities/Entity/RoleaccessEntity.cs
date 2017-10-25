using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
  public class RoleaccessEntity
    {
        public int Role_Access_Id { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public Nullable<int> Menu_Id { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}

