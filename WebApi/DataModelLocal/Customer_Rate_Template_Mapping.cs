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
    
    public partial class Customer_Rate_Template_Mapping
    {
        public int CRT_Mapping_ID { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Template_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }
        public string Region { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public string Template_Code { get; set; }
    }
}