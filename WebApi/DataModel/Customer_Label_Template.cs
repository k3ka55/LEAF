//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer_Label_Template
    {
        public int Cust_Label_Template_Id { get; set; }
        public string Template_Id { get; set; }
        public string Template_name { get; set; }
        public string Template_Type { get; set; }
        public Nullable<int> Field_Id { get; set; }
        public string Field_Name { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Field_Value { get; set; }
    }
}