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
    
    public partial class DeliveryAddress
    {
        public int Delivery_Address_Id { get; set; }
        public Nullable<int> Ref_Id { get; set; }
        public string Ref_Obj_Type { get; set; }
        public Nullable<int> Delivery_Location_Id { get; set; }
        public string Delivery_Location_Name { get; set; }
        public string Delivery_Location_Code { get; set; }
        public Nullable<System.DateTime> Delivery_Time { get; set; }
        public Nullable<long> Delivery_Contact_Person_No { get; set; }
        public string Delivery_Contact_Person { get; set; }
        public string Delivery_Address { get; set; }
    }
}
