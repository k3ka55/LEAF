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
    
    public partial class Sync_Checking_Line_item
    {
        public int SC_Line_Id { get; set; }
        public Nullable<int> Sync_id { get; set; }
        public string Sync_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Name { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Qty { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
