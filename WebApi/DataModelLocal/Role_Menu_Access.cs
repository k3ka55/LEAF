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
    
    public partial class Role_Menu_Access
    {
        public int Menu_Role_Access_Id { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public Nullable<int> Menu_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> Mobile_Flag { get; set; }
        public Nullable<bool> Web_Flag { get; set; }
        public Nullable<int> Parent_Id { get; set; }
        public string Menu_Previlleges { get; set; }
    }
}