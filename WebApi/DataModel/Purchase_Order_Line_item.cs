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
    
    public partial class Purchase_Order_Line_item
    {
        public int PO_Line_Id { get; set; }
        public Nullable<int> Po_id { get; set; }
        public string PO_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Qty { get; set; }
        public Nullable<double> A_Grade_Qty { get; set; }
        public Nullable<double> B_Grade_Qty { get; set; }
        public Nullable<double> Total_Qty { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<double> A_Grade_Price { get; set; }
        public Nullable<double> B_Grade_Price { get; set; }
        public Nullable<int> Master_Id { get; set; }
    }
}
