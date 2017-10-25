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
    
    public partial class Dispatch_Creation
    {
        public int Dispatch_Id { get; set; }
        public Nullable<int> Dispatched_Location_ID { get; set; }
        public string Dispatch_Location_Code { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_code { get; set; }
        public Nullable<int> STI_Id { get; set; }
        public string STI_Number { get; set; }
        public Nullable<int> CSI_Id { get; set; }
        public string CSI_Number { get; set; }
        public string Acceptance_Status { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<System.DateTime> Indent_Rls_Date { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public string Delievery_Type { get; set; }
        public string Customer_Dispatch_Number { get; set; }
        public string Stock_Xfer_Dispatch_Number { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string Dispatch_Type { get; set; }
        public string Delivery_done_by { get; set; }
        public Nullable<System.DateTime> Dispatch_time_stamp { get; set; }
        public Nullable<int> Delivery_Location_ID { get; set; }
        public string Delivery_Location_Code { get; set; }
        public Nullable<System.DateTime> Expected_Delivery_date { get; set; }
        public Nullable<System.DateTime> Expected_Delivery_time { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> Invoice_Flag { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public string Vehicle_No { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Route_Id { get; set; }
        public string Route_Code { get; set; }
        public string Route { get; set; }
        public Nullable<int> Price_Template_ID { get; set; }
        public string Price_Template_Name { get; set; }
        public Nullable<System.DateTime> Price_Template_Valitity_upto { get; set; }
        public string Price_Template_Code { get; set; }
        public string Order_Reference { get; set; }
    
        public virtual Dispatch_Creation Dispatch_Creation1 { get; set; }
        public virtual Dispatch_Creation Dispatch_Creation2 { get; set; }
    }
}