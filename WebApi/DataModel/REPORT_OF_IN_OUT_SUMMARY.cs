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
    
    public partial class REPORT_OF_IN_OUT_SUMMARY
    {
        public string DC_Code { get; set; }
        public string SKU_Type { get; set; }
        public string Grade { get; set; }
        public string UOM { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int IN_SKU_Count { get; set; }
        public double IN_TOTAL_QTY { get; set; }
        public int OUT_SKU_Count { get; set; }
        public double OUT_TOTAL_QTY { get; set; }
    }
}