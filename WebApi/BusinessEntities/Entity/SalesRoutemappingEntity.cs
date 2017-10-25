using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class SalesRoutemappingEntity
    {
        public int Sales_Route_Mapping_Id { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Route_Id { get; set; }
        public Nullable<int> Orgin_Location_Id { get; set; }
        public string Orgin_Location_Code { get; set; }
        public string Orgin_Location_Name { get; set; }
        public Nullable<int> Target_Location_Id { get; set; }
        public string Target_Location_Code { get; set; }
        public string Target_Location_Name { get; set; }
        public string Route_Alias_Name { get; set; }
        //public string Route_Name { get; set; }
        public string Route_Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<SalesRoutesList> SalesRoutes { get; set; }
    }

    public class SalesRoutesList
    {
        public Nullable<int> Orgin_Location_Id { get; set; }
        public string Orgin_Location_Code { get; set; }
        public string Orgin_Location_Name { get; set; }
        public Nullable<int> Target_Location_Id { get; set; }
        public string Target_Location_Code { get; set; }
        public string Target_Location_Name { get; set; }
        public string Route_Alias_Name { get; set; }
        public Nullable<int> Route_Id { get; set; }
       
        public string Route_Code { get; set; }
    }
}
