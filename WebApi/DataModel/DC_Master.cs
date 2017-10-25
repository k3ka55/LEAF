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
    
    public partial class DC_Master
    {
        public int Dc_Id { get; set; }
        public string DC_Code { get; set; }
        public string Dc_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CIN_No { get; set; }
        public string FSSAI_No { get; set; }
        public string PAN_No { get; set; }
        public string CST_No { get; set; }
        public string TIN_No { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public Nullable<int> PinCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Region { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }
        public Nullable<bool> Offline_Flag { get; set; }
        public string GST_Number { get; set; }
        public string UIN_No { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
    }
}
