using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class MobileLoginResponseEntity
    {

        public int userId { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public string userName { get; set; }
        public string userType { get; set; }
        //public List<subMenuEntity> permissions { get; set; }
        //public List<DCLocations> location { get; set; }
        public List<RolesEntity> roles { get; set; }
        //public List<DCLocationsr> dc { get; set; }
        //public string token { get; set; }


    }

    public class MobileInvoiceStatementResponseEntity
    {

        public int DateBasedCount { get; set; }
        public int OnBoardCount { get; set; }
        public double OnBoardAmount { get; set; }
        public double DateBasedAmonut { get; set; }
       


    }
    public class LocationModel
    {
        public int Location_Id { get; set; }
        public string Location_Code { get; set; }
        public string Location_Name { get; set; }
    }
    public class MobileCustSuppDDEntity
    {
        public IEnumerable<CustSupplierDDModel> CustCategory { get; set; }
        public IEnumerable<RegionEntity> CustRegion { get; set; }
        public IEnumerable<DCMasterModel> CustDCMaster { get; set; }
        public IEnumerable<LocationModel> CustLocationMaster { get; set; }
        public IEnumerable<CustSupplierDDModel> CustStoreType { get; set; }
        public IEnumerable<SKU_Type> SKU_Type { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class MobileCustomerEntity
    {
     
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_GST_Number { get; set; }
        public string Customer_Ref_Number { get; set; }
        public string Customer_Tin_Number { get; set; }
       // public List<DelieveryAddressEntity> DelieveryAddress { get; set; }
        public IEnumerable<DelieveryAddressEntity> DelieveryAddresses { get; set; }
      

        // public List<DCCustomer_Mapping> CustomerMapping { get; set; }
    }

    public class MobileSingleDispatchEntity
    {
        public List<MobileDispatchLineItemsEntity> Line_Items { get; set; }
        public IEnumerable<DIS_Qty_SumEntity> Total_Qty_Sum { get; set; }
        public IEnumerable<DISV_Qty_SumEntity> Dispatch_Qty_Sum { get; set; }
        public List<DCAddressEntity> DCAddress { get; set; }
        public IEnumerable<pack_total> Total_Pack { get; set; }
        public List<MobileCustomerEntity> CustomerAddress { get; set; }
        public string Status { get; set; }
        public string Customer_code { get; set; }
        public string Customer_Location_Name { get; set; }
        public string Customer_Name { get; set; }
        public string Dispatch_Location_Code { get; set; }
        public string CreateBy { get; set; }
        public string Customer_Dispatch_Number { get; set; }
        public string Vehicle_No { get; set; }
        public Nullable<double> Total_Amount { get; set; }
        public string AMNTinWord { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public Nullable<System.DateTime> Indent_Rls_Date { get; set; }
        public string SKU_Type { get; set; }
        public string Order_Reference { get; set; }
        public string Sales_Person_Name { get; set; }
        public string Route_Code { get; set; }
        public Nullable<int> CSI_Id { get; set; }
        public string CSI_Number { get; set; }
        public int lineItemsCount { get; set; }



    }



    public class MobileDispatchLineItemsEntity
    {
        public int Dispatch_Line_Id { get; set; }
        public string SKU_SubType { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Indent_Qty { get; set; }
        public Nullable<double> Dispatch_Qty { get; set; }
        public Nullable<double> Received_Qty { get; set; }
        public Nullable<double> Accepted_Qty { get; set; }
        public Nullable<double> Return_Qty { get; set; }
        public Nullable<double> Unit_Rate { get; set; }
        public Nullable<double> Dispatch_Value { get; set; }
        public string Dispatch_Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string UOM { get; set; }
        public string Pack_Type { get; set; }
    }
    public class MobileDispatchEntity
    {
        public Nullable<int> Dispatched_Location_ID { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public Nullable<System.DateTime> Indent_Rls_Date { get; set; }
        public string Customer_Dispatch_Number { get; set; }
        public string CSI_Number { get; set; }
        public string Status { get; set; }
        public string Acceptance_Status { get; set; }
        public int lineItemsCount { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_code { get; set; }
        public string Customer_Location_Name { get; set; }
        public string Customer_Name { get; set; }
        public string Dispatch_Location_Code { get; set; }
        public string Vehicle_No { get; set; }
        public int Dispatch_Id { get; set; }
        //public Nullable<int> Dispatched_Location_ID { get; set; }



        //public string Stock_Xfer_Dispatch_Number { get; set; }

        //public string Delievery_Type { get; set; }
        //public string SKU_Type { get; set; }
        //public Nullable<int> SKU_Type_Id { get; set; }
        //public string Dispatch_Type { get; set; }
        //public string Delivery_done_by { get; set; }
        //public Nullable<System.DateTime> Dispatch_time_stamp { get; set; }
        //public Nullable<int> Delivery_Location_ID { get; set; }
        //public string Delivery_Location_Code { get; set; }
        //public Nullable<System.DateTime> Expected_Delivery_date { get; set; }
        //public Nullable<System.DateTime> Expected_Delivery_time { get; set; }
        //public Nullable<bool> is_Deleted { get; set; }
        //public string Remark { get; set; }
        //public string Status { get; set; }
        //public Nullable<double> Total_Amount { get; set; }
        //public string AMNTinWord { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public string CreateBy { get; set; }
        //public int CreateByUId { get; set; }
        //public string UpdateBy { get; set; }
        //public Nullable<int> STI_Id { get; set; }
        //public string STI_Number { get; set; }
        //public Nullable<int> CSI_Id { get; set; }


        //public Nullable<bool> is_Syunc { get; set; }
        //public Nullable<bool> Invoice_Flag { get; set; }
        //public Nullable<int> Price_Template_ID { get; set; }
        //public string Price_Template_Code { get; set; }
        //public string Price_Template_Name { get; set; }
        //public int? Menu_Id { get; set; }
        //public string Menu_Name { get; set; }
        //public int is_Create { get; set; }
        //public int is_Edit { get; set; }
        //public int is_Approval { get; set; }
        //public int is_View { get; set; }
        //public int is_Delete { get; set; }
        //public string Vehicle_No { get; set; }
        //public Nullable<int> Sales_Person_Id { get; set; }
        //public string Sales_Person_Name { get; set; }
        //public Nullable<int> Route_Id { get; set; }
        //public string Route_Code { get; set; }
        //public string Route { get; set; }
        //public Nullable<System.DateTime> Price_Template_Valitity_upto { get; set; }
        //public List<CustomerEntity> CustomerAddress { get; set; }
        //public List<DispatchLineItemsEntity> Line_Items { get; set; }
        //public IEnumerable<DIS_Qty_SumEntity> Total_Qty_Sum { get; set; }
        //public IEnumerable<DISV_Qty_SumEntity> Dispatch_Qty_Sum { get; set; }
        //public List<DCAddressEntity> DCAddress { get; set; }
        //public IEnumerable<pack_total> Total_Pack { get; set; }
        //public IEnumerable<STIDetails> STI_Details { get; set; }
        ////public IEnumerable<DCAddressEntity> DelDC_Details { get; set; }
    }

    public class MobileLoginUserLocationResponseEntity
    {

        //public int userId { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        //public string userName { get; set; }
        //public string userType { get; set; }
        // public List<subMenuEntity> permissions { get; set; }
        public List<MobileDCLocations> location { get; set; }
        public List<MobLocations> dc { get; set; }
        public List<MobileDispatchLocations> DispatchLocations { get; set; }
        //  public List<RolesEntity> roles { get; set; }
        //public List<DCLocationsr> dc { get; set; }
        //public string token { get; set; }
    }
    public class MobileDispatchLocations
    {
        public int Dispatch_loaction_Id { get; set; }
        public string Dispatch_loaction_Code { get; set; }
        public string Dispatch_loaction_Name { get; set; }
    }
    public class MobileRateIndentEntity
    {
        public int Template_ID { get; set; }
        public string Template_Name { get; set; }
        public Nullable<System.DateTime> Valitity_upto { get; set; }
        public int counting { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Location_Code { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public List<MobileRateTemplateLineitem> LineItems { get; set; } 
    }
    public class MobileRateTemplateLineitem
    {
        public string SKU_SubType { get; set; }
        public string SKU_Name { get; set; }
        public string Grade { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Selling_price { get; set; }

       
    }
    
    public class MobLocations
    {
        public string Region_Name { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }

        public List<MobileDCLocations> dcDetails { get; set; }
    }
    public class MobileDCLocations
    {
        public int Dc_Id { get; set; }
        public string DC_Code { get; set; }
        public string Dc_Name { get; set; }
        //public string Address1 { get; set; }
        //public string Address2 { get; set; }
        //public string Country { get; set; }
        //public string State { get; set; }
        //public string City { get; set; }
        //public int? Pincode { get; set; }
        public string UserType { get; set; }
        public string Region_Name { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }
        public Nullable<bool> Offline_Flag { get; set; }
    }
}