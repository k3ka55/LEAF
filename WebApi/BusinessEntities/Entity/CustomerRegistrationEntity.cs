using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
 public class CustomerRegistrationEntity
    {
        public string Customer_Name { get; set; }
        public string Parent_Company_Name { get; set; }
        public string Primary_Contact_Name { get; set; }
        public Nullable<long> Contact_Number { get; set; }
        public int Cust_Reg_Id { get; set; }
        public string StoreImageFile { get; set; }
        public string StoreOwnerImageFile { get; set; }
        public string Primary_Email_ID { get; set; }
        public string Secondary_Email_ID { get; set; }
        public string Location { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }
        public string Region { get; set; }
        public Nullable<int> Category_Id { get; set; }
        public string Category { get; set; }
        public Nullable<int> Servicing_DC_Id { get; set; }
        public string Servicing_DC_Code { get; set; }
        public string Servicing_DC { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public Nullable<int> Store_Type_Id { get; set; }
        public string Store_Type { get; set; }
        public Nullable<System.DateTime> Engagement_Start_Date { get; set; }
        public string Store_Image_Display_Name { get; set; }
        public string Store_Image_Name { get; set; }
        public string Store_Owner_Image_Display_Name { get; set; }
        public string Store_Owner_Image_Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Created_From { get; set; }
        public string Approved_by { get; set; }
        public Nullable<bool> Is_Approved { get; set; }
        public Nullable<System.DateTime> Date_Of_Approval { get; set; }
        public Nullable<bool> Is_Delete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<RegDelieveryAddressEntity> DelieveryAddress { get; set; }
       // public IEnumerable<RegDelieveryAddressEntity> DelieveryAddresses { get; set; }
    }
 public class RegDelieveryAddressEntity
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
