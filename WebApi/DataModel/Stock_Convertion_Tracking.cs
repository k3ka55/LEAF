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
    
    public partial class Stock_Convertion_Tracking
    {
        public int Stock_Convertion_Id { get; set; }
        public int Stock_Id { get; set; }
        public string Stock_Code { get; set; }
        public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Name { get; set; }
        public string SKU_Type { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Stock_Qty { get; set; }
        public string UOM { get; set; }
        public string Type { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public string Convert_From_Stock_Code { get; set; }
        public Nullable<bool> Is_Syunc { get; set; }
    }
}
