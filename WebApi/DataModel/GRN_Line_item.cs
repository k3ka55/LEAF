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
    
    public partial class GRN_Line_item
    {
        public int GRN_Line_Id { get; set; }
        public Nullable<int> INW_id { get; set; }
        public string GRN_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string Tally_Status { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Strinkage_Qty { get; set; }
        public Nullable<double> PO_QTY { get; set; }
        public Nullable<double> Billed_Qty { get; set; }
        public Nullable<int> Price_Book_Id { get; set; }
        public Nullable<double> Price { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<double> B_Accepted_Qty { get; set; }
        public Nullable<double> A_Accepted_Qty { get; set; }
        public Nullable<double> C_Accepted_Qty { get; set; }
        public Nullable<double> A_Accepted_Price { get; set; }
        public Nullable<double> B_Accepted_Price { get; set; }
        public Nullable<double> C_Accepted_Price { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public Nullable<bool> moved { get; set; }
        public string Grade { get; set; }
        public Nullable<double> A_Converted_Qty { get; set; }
        public Nullable<double> B_Converted_Qty { get; set; }
        public Nullable<double> C_Converted_Qty { get; set; }
        public Nullable<int> Master_Id { get; set; }
    }
}