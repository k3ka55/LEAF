using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{

    public class SupplierEntity
    {
        public string Supplier_Name { get; set; }
        public string City { get; set; }
        public int Supplier_ID { get; set; }
        public string Supplier_code { get; set; }
        public string Farm_location { get; set; }
        public string Farm_Area { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public Nullable<int> Pincode { get; set; }
        public string Primary_Contact_Name { get; set; }
        public Nullable<long> Contact_Number { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Secondary_Email_ID { get; set; }
        public string Primary_Email_ID { get; set; }
        public string Location { get; set; }
        public string Location_Code { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public Nullable<int> Collection_Centre_Id { get; set; }
        public string Collection_Centre { get; set; }
        public Nullable<System.DateTime> Date_Of_Birth { get; set; }
        public string Father_Name { get; set; }
        public string PAN_Number { get; set; }
        public string Bank_Name { get; set; }
        public Nullable<long> Bank_Account_Number { get; set; }
        public Nullable<int> Payment_Type_Id { get; set; }
        public string Payment_Type { get; set; }
        public string Photo { get; set; }
        public Nullable<System.DateTime> Supplier_Activation_Date { get; set; }
        public string Supplier_IDCard_Number { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        //  public int Sup_Id { get; set; }
        //   public string Supplier_code { get; set; }
        public string File { get; set; }
        //  public List<DCSupplier_Mapping> SupplierMapping { get; set; }
    }

    //public class DCSupplier_Mapping
    //{
    //    public int Dc_Supplier_map_Id { get; set; }
    //    public Nullable<int> DC_id { get; set; }
    //    public Nullable<int> Supplier_Id { get; set; }
    //    public string DC_Code { get; set; }
    //    public string Supplier_Code { get; set; }
    //    public string Supplier_Name { get; set; }
    //}
}