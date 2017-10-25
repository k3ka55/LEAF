using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class DCMasterEntity
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
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public string GST_Number { get; set; }
        public string UIN_No { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
    }
    public class CustEntity
    {
        public string Cust_Name { get; set; }
        public int? Cust_Id { get; set; }
        public string Cust_Code { get; set; }
    }
    public class SupEntity
    {
        public string Sup_name { get; set; }
        public int? Sup_Id { get; set; }
        public string Sup_Code { get; set; }
    }
}
