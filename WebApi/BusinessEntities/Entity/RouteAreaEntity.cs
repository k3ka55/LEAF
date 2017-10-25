using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class RouteAreaEntity
    {
        public int Route_Area_Master_Id { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public string Location_Name { get; set; }
        public string Location_Type { get; set; }
        public string Pincode { get; set; }
        public string Area_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<RoutesAreaList> Areas { get; set; }
    }


    public class RoutesAreaList
    {
        public int Route_Area_Master_Id { get; set; }
        public string Pincode { get; set; }
        public string Area_Name { get; set; }
    }
}
