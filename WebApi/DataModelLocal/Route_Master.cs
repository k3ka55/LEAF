//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModelLocal
{
    using System;
    using System.Collections.Generic;
    
    public partial class Route_Master
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
        public Nullable<int> Route_Id { get; set; }
        public string Remarks { get; set; }
        public string Route { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
