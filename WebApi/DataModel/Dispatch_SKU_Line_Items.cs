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
    
    public partial class Dispatch_SKU_Line_Items
    {
        public int Dispatch_SKU_Line_Items_Id { get; set; }
        public Nullable<int> Dispatch_Line_Id { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public Nullable<double> Dispatch_Qty { get; set; }
        public string Barcode { get; set; }
        public string Batch_Number { get; set; }
        public string UOM { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
    }
}
