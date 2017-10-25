﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class CustomerEntity
    {
        public int Cust_Reg_Id { get; set; }
        public string StoreImageFile { get; set; }
        public string StoreOwnerImageFile { get; set; }
        public string Customer_User_Id { get; set; }
        public string Password { get; set; }
        public string Store_Image_Display_Name { get; set; }
        public string Store_Image_Name { get; set; }
        public string Store_Owner_Image_Display_Name { get; set; }
        public string Store_Owner_Image_Name { get; set; }
        public string Customer_Name { get; set; }
        public string City { get; set; }
        public int Cust_Id { get; set; }
        public string Customer_Code { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Route_Id { get; set; }
        public string Route_Name { get; set; }
        public string Route_Code { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public Nullable<int> Pincode { get; set; }
        public string Primary_Contact_Name { get; set; }
        public Nullable<long> Contact_Number { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Primary_Email_ID { get; set; }
        public string Secondary_Email_ID { get; set; }
        public string Location { get; set; }
        public string Location_Code { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public string Parent_Company_Name { get; set; }
        public string Abbreviation { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }
        public string Region { get; set; }
        public Nullable<int> Category_Id { get; set; }
        public string Category { get; set; }
        public Nullable<int> Servicing_DC_Id { get; set; }
        public string Servicing_DC { get; set; }
        public Nullable<int> Store_Type_Id { get; set; }
        public string Store_Type { get; set; }
        public Nullable<int> Engagement_Type_Id { get; set; }
        public string Engagement_Type { get; set; }
        public Nullable<int> No_of_Stores { get; set; }
        public string Transaction_Volume { get; set; }
        public Nullable<System.DateTime> Engagement_Start_Date { get; set; }
        public string Account_Manger { get; set; }
        public string Primary_ContactPerson_Name { get; set; }
        public string Primary_ContactPerson_No { get; set; }
        public string Category_Head_Name { get; set; }
        public string Business_Head_Name { get; set; }
        public string Delivery_Days { get; set; }
        public List<int> DeliveryDays { get; set; }
        public string Delivery_Type { get; set; }
        public Nullable<double> Load_Per_Delivery { get; set; }
        public string Indent_Type { get; set; }
        public string Delivery_Location_Name { get; set; }
        public string Delivery_Location_Code { get; set; }
        public Nullable<int> Delivery_Location_Id { get; set; }
        public Nullable<int> Delivery_Type_Id { get; set; }
        public string Delivery_Address { get; set; }
        public string Delivery_Contact_Person { get; set; }
        public Nullable<long> Delivery_Contact_Person_No { get; set; }
        public string GRN_Receive_Shedule { get; set; }
        public Nullable<int> GRN_Receive_Type_Id { get; set; }
        public string GRN_Receive_Type { get; set; }
        public string Customer_Return_Policy { get; set; }
        public string Pricing_Change_Schedule { get; set; }
        public Nullable<int> Customer_Return_Policy_Id { get; set; }
        public Nullable<int> Pricing_Change_Schedule_Id { get; set; }
        public Nullable<int> Payment_Type_Id { get; set; }
        public string Payment_Type { get; set; }
        public Nullable<int> Credit_Period_Id { get; set; }
        public string Credit_Period { get; set; }
        public string Payment_Date { get; set; }
        public Nullable<double> Credit_Limit { get; set; }
        public Nullable<bool> Is_Delete { get; set; }
        public List<DelieveryAddressEntity> DelieveryAddress { get; set; }
        public IEnumerable<DelieveryAddressEntity> DelieveryAddresses { get; set; }
        public string Credit_Period_Reason { get; set; }
        public string Price_Change_Shedule_Reason { get; set; }
        public string Customer_Tin_Number { get; set; }
        public string Customer_Pan_Number { get; set; }
        public string Customer_Account_Name { get; set; }
        public string Customer_Bank_Name { get; set; }
        public string Customer_Branch_Name { get; set; }
        public string Customer_Account_Number { get; set; }
        public string Bank_IFSC_Number { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public string Store_Type_Other { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public string Customer_GST_Number { get; set; }
        public string Customer_Ref_Number { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
 
        // public List<DCCustomer_Mapping> CustomerMapping { get; set; }
    }

    //public class DCCustomer_Mapping
    //{
    //    public int Dc_Cust_map_Id { get; set; }
    //    public Nullable<int> DC_id { get; set; }
    //    public Nullable<int> Cust_Id { get; set; }
    //    public string DC_Code { get; set; }
    //    public string Cust_Code { get; set; }
    //    public string Cust_Name { get; set; }
    //}

    public class DelieveryAddressEntity
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
