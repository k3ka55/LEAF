using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DispatchEntity
    {
        public int Dispatch_Id { get; set; }
        public Nullable<int> Dispatched_Location_ID { get; set; }
        public string Dispatch_Location_Code { get; set; }
        public int OTP { get; set; }
        public string Acceptance_Status { get; set; }
        public Boolean GRN_Flag {get;set;}
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_code { get; set; }
        public string Customer_Location_Name { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Dispatch_Number { get; set; }
        public string Stock_Xfer_Dispatch_Number { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public Nullable<System.DateTime> Indent_Rls_Date { get; set; }
        public string Delievery_Type { get; set; }
        public string SKU_Type { get; set; }
        public string Order_Reference { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
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
        public Nullable<double> Total_Amount { get; set; }
        public string AMNTinWord { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public int CreateByUId { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<int> STI_Id { get; set; }
        public string STI_Number { get; set; }
        public Nullable<int> CSI_Id { get; set; }
        public string CSI_Number { get; set; }
        public int lineItemsCount { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public Nullable<bool> Invoice_Flag { get; set; }
        public Nullable<int> Price_Template_ID { get; set; }
        public string Price_Template_Code { get; set; }
        public string Price_Template_Name { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public string Vehicle_No { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Route_Id { get; set; }
        public string Route_Code { get; set; }
        public string Route { get; set; }
        public Nullable<System.DateTime> Price_Template_Valitity_upto { get; set; }
        public List<CustomerEntity> CustomerAddress { get; set; }
        public List<DispatchLineItemsEntity> Line_Items { get; set; }
        public IEnumerable<DIS_Qty_SumEntity> Total_Qty_Sum { get; set; }
        public IEnumerable<DISV_Qty_SumEntity> Dispatch_Qty_Sum { get; set; }
        public IEnumerable<Cartons_SumEntity> Cartons_Sum { get; set; }
        public IEnumerable<Crates_SumEntity> Crates_Sum { get; set; }
        public IEnumerable<Gunny_Bags_SumEntity> Gunny_Bags_Sum { get; set; }
        public IEnumerable<PP_Bags_SumEntity> PP_Bags_Sum { get; set; }
        public List<DCAddressEntity> DCAddress { get; set; }
        public IEnumerable<pack_total> Total_Pack { get; set; }
        public IEnumerable<STIDetails> STI_Details { get;set;}
        //public IEnumerable<DCAddressEntity> DelDC_Details { get; set; }
    }
    public class DispatchAcceptedEntity
    {
      //  public int Auto_Inc { get; set; }
        public int Dispatch_Id { get; set; }
        public int OTP { get; set; }
        public List<DispatchAcceptedLineItemEntity> LineItems{get;set;}
        
    }
    public class OTPWriteEntity
    {
        public int Auto_Inc { get; set; }
        public int Dispatch_Id { get; set; }
        public string CDN_Number { get; set; }
        public string OTP { get; set; }
       // public List<DispatchAcceptedLineItemEntity> LineItems { get; set; }

    }
      public class DispatchAcceptedLineItemEntity
    {
        public int Auto_Inc { get; set; }
        public int Dispatch_Id { get; set; }
        public int OTP { get; set; }
      //  public int LineItems { get; set; }
        public int Dispatch_Line_Id { get; set; }
        public double Accepted_Qty { get; set; }               
    }
    public class Cartons_SumEntity
    {
        public Nullable<double> Cartons_Sum { get; set; }
    }
    public class Crates_SumEntity
    {
        public Nullable<double> Crates_Sum { get; set; }
    }
    public class Gunny_Bags_SumEntity
    {
        public Nullable<double> Gunny_Bags_Sum { get; set; }
    }
    public class PP_Bags_SumEntity
    {
        public Nullable<double> PP_Bags_Sum { get; set; }
    }
    
    public class pack_total
    {
        public Nullable<double> Total_Pack { get; set; }
    }

    public class STIDetails
    {
        public string STI_Type { get; set; }
        public string STI_Raised_By { get; set; }
        public List<DCAddressEntity> Delivery_DCAddress { get; set; }
        public List<DCAddressEntity> Source_DCAddress { get; set; }        
        public Nullable<DateTime> Expected_Delivery_Date { get; set; }
        public Nullable<DateTime> IndentDate { get; set; }
    }

    //public class AltDCDetails
    //{
    //    public List<DCAddressEntity> Delivery_DCAddress { get; set; }
    //    public List<DCAddressEntity> Source_DCAddress { get; set; }  
    //}

    public class DispatchResponseEntity
    {
        public int DispatchID { get; set; }
        public string DispatchNumber { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message {get;set;}
    }
    public class InvoiceResponseEntity
    {
        public int InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }

    public class DispatchConsumablesEntity
    {
        public int Dispatch_Consumables_Id { get; set; }
        public Nullable<int> Dispatch_Line_Id_FK { get; set; }
        public Nullable<int> Dispatch_SKU_Line_Items_Id_FK { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public Nullable<double> Consumable_Qty { get; set; }
        public string Grade { get; set; }
        public string UOM { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
    }

    public class DispatchLineItemsEntity
    {
        public string Acceptance_Status { get; set; }
        public int Dispatch_Line_Id { get; set; }
        public Nullable<int> Dispatch_Id { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string Customer_Location_Name { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_Code { get; set; }
        public string Supplier_Name { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public string Pack_Size { get; set; }
        public Nullable<int> Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; }
        public Nullable<double> Indent_Qty { get; set; }
        public Nullable<double> Dispatch_Qty { get; set; }
        public Nullable<double> Received_Qty { get; set; }
        public Nullable<double> Strinkage_Qty { get; set; }
        public Nullable<double> Accepted_Qty { get; set; }
        public Nullable<double> Return_Qty { get; set; }
        public Nullable<double> Unit_Rate { get; set; }
        public Nullable<double> Dispatch_Value { get; set; }
        public Nullable<double> No_of_Packed_Item { get; set; }
        public string Pack_Type { get; set; }
        public string Dispatch_Pack_Type { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> is_Stk_Update { get; set; }
        public Nullable<double> Converted_Unit_Value { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public string Tally_Status { get; set; }
        public string Dispatch_Location_Code { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Dispatch_Number { get; set; }
        public string Stock_Xfer_Dispatch_Number { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public Nullable<System.DateTime> Indent_Rls_Date { get; set; }
        public string Delievery_Type { get; set; }
        public string SKU_Type { get; set; }
        public string Dispatch_Type { get; set; }
        public string Delivery_done_by { get; set; }
        public Nullable<System.DateTime> Dispatch_time_stamp { get; set; }
        public string Delivery_Location_Code { get; set; }
        public Nullable<System.DateTime> Expected_Delivery_date { get; set; }
        public Nullable<System.DateTime> Expected_Delivery_time { get; set; }
        public Nullable<double> Total_Amount { get; set; }
        public string STI_Number { get; set; }
        public string CSI_Number { get; set; }
        public string Price_Template_Name { get; set; }
        public Nullable<System.DateTime> Price_Template_Valitity_upto { get; set; }
        public IEnumerable<DispatchSKULineItemsEntity> DispatchSKULineItems { get; set; }
        public IEnumerable<DispatchConsumablesEntity> DispatchLineItemConsumables { get; set; }
    }

    public class DispatchSKULineItemsEntity
    {
        public int Dispatch_SKU_Line_Items_Id { get; set; }
        public Nullable<int> Dispatch_Line_Id { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public Nullable<double> Dispatch_Qty { get; set; }
        public string Barcode { get; set; }
        public string Batch_Number { get; set; }
        public string UOM { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
    }
    
    public class DispatchUpdateEntity
    {
        public string id { get; set; }
        public int Dispatch_Id { get; set; }
        public IEnumerable<DLIDs> DispatchLIds { get; set; }
    }
    public class DLIDs
    {
        public int Dispatch_LineId { get; set; }
        public int statusflag { get; set; }
    }
    public class CustomerNumGenerationEntity
    {
        public int Dispatch_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Customer_Dispatch_Last_Number { get; set; }
    }

    public class StockXferNumGenerationEntity
    {
        public int Dispatch_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Stock_Xfer_Dispatch_Last_Number { get; set; }
    }

    public class DispatchSupplierTrackEntity
    {
        public int Dispatch_Track_Id { get; set; }
        public int Dispatch_Id { get; set; }
        public string Dispatch_Number { get; set; }
        public Nullable<int> Dispatch_Line_Id { get; set; }
        public Nullable<int> INW_id { get; set; }
        public string GRN_Number { get; set; }
        public Nullable<int> GRN_Line_Id { get; set; }
        public Nullable<int> Stock_ID { get; set; }
        public string Stock_code { get; set; }
        public Nullable<int> Supplier_ID { get; set; }
        public string Supplier { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_name { get; set; }
        public string QTY { get; set; }
        public string Price { get; set; }
        public Nullable<double> Billed_Qty { get; set; }
        public Nullable<double> A_Accepted_Qty { get; set; }
        public Nullable<double> B_Accepted_Qty { get; set; }
        public Nullable<double> C_Accepted_Qty { get; set; }
        public Nullable<double> A_Accepted_Price { get; set; }
        public Nullable<double> B_Accepted_Price { get; set; }
        public Nullable<double> C_Accepted_Price { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateBy { get; set; }
    }

    public class stockTransactionEntity
    {
        public int ST_Tran_ID { get; set; }
        public string Stock_Code { get; set; }
        public Nullable<int> INW_id { get; set; }
        public string GRN_Number { get; set; }
        public Nullable<int> GRN_Line_Id { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> Supplier_ID { get; set; }
        public string Supplier { get; set; }
        public Nullable<double> Qty { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }


    public class stockAvail
    {
        public string available { get; set; }
        public string sku_status { get; set; }
        public bool status { get; set; }
        public string SKU_name { get; set; }
        public string Grade { get; set; }
        public Nullable<double> AvailQty { get; set; }
    }

    public class DispatchNumber
    {
        public string Customer_Dispatch_Number { get; set; }
    }

    public class wasteageApproval
    {
        public List<approvalList> approvalDispatches { get; set; }
    }

    public class approvalList
    {
        public int Dispatch_Id { get; set; }
        public string Dispatch_Number { get; set; }
        public Nullable<bool> is_wastage_Approved { get; set; }
        public Nullable<int> is_wastage_approved_user_id { get; set; }
        public string is_wastage_approved_by { get; set; }
    }

    public class InvoiceEntity
    {
        public int invoice_Id { get; set; }
        public string Invoice_Number { get; set; }
        public string Order_Reference { get; set; }
        public Nullable<int> Dispatch_id { get; set; }
        public string Customer_Dispatch_Number { get; set; }
        public Nullable<System.DateTime> Invoice_Date { get; set; }
        public string Invoice_Type { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_code { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<int> DC_LID { get; set; }
        public string DC_LCode { get; set; }       
        public string Term_of_Payment { get; set; }
        public string Supplier_Ref { get; set; }
        public string Buyer_Order_No { get; set; }
        public Nullable<System.DateTime> Order_Date { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> is_Invoice_Approved { get; set; }
        public Nullable<int> is_Invoice_approved_user_id { get; set; }
        public string is_Invoice_approved_by { get; set; }
        public Nullable<System.DateTime> is_Invoice_approved_date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<double> Total_Amount { get; set; }
        public string AMNTinWord { get; set; }
        public int IlineItemsCount { get; set; }
        public List<CDNDate> CDNCreatedDate { get; set; }
        public List<csi_details> CSI_Details { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public List<InvoiceLineItemEntity> InvoiceLineItems { get; set; }
        public List<CustAddressEntity> CustAddress { get; set; }
        public List<DCAddressEntity> DCAddress { get; set; }
    }

    public class csi_details
    {
        public Nullable<int> CSI_Id { get; set; }
        public string CSI_Number { get; set; }
    }

    public class CDNDate
    {
        public Nullable<DateTime> CDNCDate { get; set; }
    }
    public class DCAddressEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string County { get; set; }
        public string GST_Number { get; set; }
        public string UIN_No { get; set; }
        public string State { get; set; }

        public string CIN_No { get; set; }
        public string FSSAI_No { get; set; }
        public string PAN_No { get; set; }
        public string CST_No { get; set; }
        public string TIN_No { get; set; }
        public string City { get; set; }
        public Nullable<int> PinCode { get; set; }
    }
    public class CustAddressEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Customer_GST_Number { get; set; }
        public string Customer_Ref_Number { get; set; }
        public Nullable<int> Pincode { get; set; }
        public string Customer_Tin_Number { get; set; }
        public string Primary_Contact_Name { get; set; }
        public Nullable<long> Contact_Number { get; set; }
        public string Primary_Email_ID { get; set; }
        public string Secondary_Email_ID { get; set; }
        public List<DelieveryAddressEntity> DelieveryAddress { get; set; }
    }
    public class InvoiceLineItemEntity
    {
        public int Invoice_Line_Id { get; set; }
        public Nullable<int> Invoice_Id { get; set; }
        public string Invoice_Number { get; set; }
        public Nullable<int> Dispatch_Line_id { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Return_Qty { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Invoice_Qty { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Invoice_Amount { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<double> Dispatch_Qty { get; set; }
        public Nullable<double> Converted_Unit_Value { get; set; }
      
        public Nullable<double> Total_Amount { get; set; }
        public string UpdateBy { get; set; }

        public string Customer_Dispatch_Number { get; set; }
        public Nullable<System.DateTime> Invoice_Date { get; set; }
        public string Invoice_Type { get; set; }
        public string SKU_Type { get; set; }
        public string Customer_Name { get; set; }
        public string DC_LCode { get; set; }
        public string Term_of_Payment { get; set; }
        public string Supplier_Ref { get; set; }
        public string Buyer_Order_No { get; set; }
        public Nullable<System.DateTime> Order_Date { get; set; }
        public string Remark { get; set; }
       
    }
   

    public class InvoiceNumberGeneration
    {
        public int Invoice_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Invoice_Last_Number { get; set; }
    }

    public class invoiceApproval
    {
        public List<approvalInvoiceList> approvalInvoices { get; set; }
    }

    public class approvalInvoiceList
    {
        public int invoice_Id { get; set; }
        public string Invoice_Number { get; set; }
        public Nullable<bool> is_Invoice_Approved { get; set; }
        public Nullable<int> is_Invoice_approved_user_id { get; set; }
        public string is_Invoice_approved_by { get; set; }
    }
}
