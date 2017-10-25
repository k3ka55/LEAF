using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class LabelEntity
    {
        public int Field_Id { get; set; }
        public string Field_Name { get; set; }
        public string Data_Type { get; set; }
        public Nullable<int> Size { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
