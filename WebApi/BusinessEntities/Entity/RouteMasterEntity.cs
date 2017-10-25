using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
   public  class RouteMasterEntity
    {
        public int Route_Master_Id { get; set; }
        public Nullable<int> Orgin_Location_Id { get; set; }
        public string Orgin_Location_Code { get; set; }
        public string Orgin_Location { get; set; }
        public Nullable<int> Target_Location_Id { get; set; }
        public string Target_Location { get; set; }
        public string Target_Location_Code { get; set; }
        public string Target_Loc_Type { get; set; }
        public string Route_Code { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Route_Id { get; set; }
        public string Route { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<RoutesList> Routes { get; set; }
    }
    
    public class RoutesList
    {
        public int Route_Master_Id { get; set; }
        public int? Route_Id { get; set; }
        public string Remarks { get; set; }
        public string Route { get; set; }
    }
}
