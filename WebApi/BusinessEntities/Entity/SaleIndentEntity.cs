using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{

    //public class MIExcelFields
    //{
    //    public Nullable<int> SKU_ID { get; set; }
    //    public string SKU_Code { get; set; }
    //    public string SKU_Name { get; set; }
    //    public Nullable<int> SKU_SubType_Id { get; set; }
    //    public string SKU_SubType { get; set; }
    //    public Nullable<int> Pack_Type_Id { get; set; }
    //    public string Pack_Type { get; set; }
    //    public string Pack_Size { get; set; }
    //    public Nullable<int> Pack_Weight_Type_Id { get; set; }
    //    public string Pack_Weight_Type { get; set; }
    //    public string UOM { get; set; }
    //    public string Grade { get; set; }
    //    public Nullable<double> SP { get; set; }
    //    public bool status { get; set; }
    //    public int lineNumber { get; set; }
    //    public string Message { get; set; }
    //    public string errorItem { get; set; }
    //    //  public List<StatusFields> ErrorReport { get; set; }
    //    //
    //}
    //public class fileImportMI
    //{
    //    public string LocationCode { get; set; }
    //    public string DCCode { get; set; }
    //    public string Region { get; set; }
    //    public string SKUType { get; set; }
    //    public string FileString { get; set; }
    //}

    public class MultipleCSItrackingEntity
    {

        public int Multiple_CSI_tracking_id { get; set; }
        public Nullable<int> No_of_CSI { get; set; }
        public string Uploaded_Excel_Display_Name { get; set; }
        public string Uploaded_Excel_Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreateBy { get; set; }

    }







    public class SaleIndentEntity
    {
        public int CSI_id { get; set; }
        public string CSI_Number { get; set; }
        public string DC_Code { get; set; }
        public Nullable<System.DateTime> CSI_Raised_date { get; set; }
        public Nullable<System.DateTime> CSI_Timestamp { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<int> Delivery_Location_ID { get; set; }
        public string Delivery_Location_Code { get; set; }
        public string Delivery_cycle { get; set; }
        public string CSI_From { get; set; }
        public Nullable<System.DateTime> Delivery_Expected_Date { get; set; }
        public string Delievery_Type { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string CSI_raised_by { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<bool> CSI_Approved_Flag { get; set; }
        public string CSI_Approved_by { get; set; }
        public Nullable<System.DateTime> CSI_Approved_date { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public bool is_Deleted { get; set; }
        public Nullable<int> CSI_Create_by { get; set; }
        public string CSI_type { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public int Counting { get; set; }
        public Nullable<int> Indent_Id { get; set; }
        public string Indent_Name { get; set; }
        public string Indent_Code { get; set; }
        public Nullable<int> Rate_Template_Id { get; set; }
        public string Rate_Template_Name { get; set; }
        public string Rate_Template_Code { get; set; }
        public Nullable<int> User_Location_Id { get; set; }
        public string User_Location_Code { get; set; }
        public string User_Location { get; set; }
        public string User_Type { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public int CreateByUId { get; set; }
        public Nullable<int> Expected_Route_Id { get; set; }
        public string Expected_Route_Alias_Name { get; set; }
        public string Expected_Route_Code { get; set; }
        public Nullable<int> Expected_Delivering_Sales_Person_Id { get; set; }
        public string Expected_Delivering_Sales_Person_Name { get; set; }
        public Nullable<System.DateTime> Rate_Template_Valitity_upto { get; set; }
        public Nullable<System.DateTime> Validatity_Date { get; set; }
        public IEnumerable<SAL_Qty_SumEntity> SAL_Qty_Sum { get; set; }
        public IEnumerable<CSI_LineItems_Entity> LineItems { get; set; }
        public List<BulkCSIExcelFields> BulkLineItems { get; set; }
        public IEnumerable<CustomerEntity> CustomerAddress { get; set; }
    }
    public class DIS_Qty_SumEntity
    {
        public Nullable<double> Total_Qty_Sum { get; set; }
    }

    public class DISV_Qty_SumEntity
    {
        public Nullable<double> Dis_Qty_Sum { get; set; }
    }

    public class SAL_Qty_SumEntity
    {
        public Nullable<double> Total_Qty_Sum { get; set; }
    }
    public class fileImportBulkCSI
    {
        public string LocationCode { get; set; }
        public string DCCode { get; set; }
        public string Region { get; set; }
        public string SKUType { get; set; }
        public string FileString { get; set; }
    }
    public class CSIExcelFields
    {
        public string Dispatch_DC_Location_Code { get; set; }
        public string Region { get; set; }
        //    public DateTime Delievery_Date { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Indent_Template_Name { get; set; }
        public string Price_Template_Name { get; set; }
        public string SKU_SubType { get; set; }
        public string SKU_Name { get; set; }
        public string Grade { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string UOM { get; set; }
        //   public Nullable<double> Dispatch_Qty { get; set; }

        public string Delievery_Date { get; set; }
        public string Dispatch_Qty { get; set; }


        //      public string Location_Code { get; set; }
        public string Indent_Type { get; set; }

        public string SKU_Type { get; set; }
        public int Pack_Weight_Type_Id { get; set; }
        public int SKU_Id { get; set; }
        public int SKU_SubType_Id { get; set; }
        public int Pack_Type_Id { get; set; }


        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
        //  public List<StatusFields> ErrorReport { get; set; }
        //
    }

    public class BulkCSIExcelFieldsReturn
    {
        public string Dispatch_DC_Location_Code { get; set; }
        public string Region { get; set; }
        //    public DateTime Delievery_Date { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Indent_Template_Name { get; set; }
        public string Price_Template_Name { get; set; }
        public string SKU_SubType { get; set; }
        public string SKU_Name { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public string Grade { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string UOM { get; set; }
        //   public Nullable<double> Dispatch_Qty { get; set; }

        public string Delievery_Date { get; set; }
        public string Dispatch_Qty { get; set; }

        public string Delivery_cycle { get; set; }
        public string Delivery_Type { get; set; }
        public string Indent_Code { get; set; }
        public string Price_Template_Code { get; set; }
        public Nullable<int> Price_Template_ID { get; set; }
        public int Indent_ID { get; set; }
        public Nullable<int> User_Location_Id { get; set; }
        public string User_Location_Code { get; set; }
        public string User_Location { get; set; }
        public string User_Type { get; set; }
        //      public string Location_Code { get; set; }
        public string Indent_Type { get; set; }

        public string SKU_Type { get; set; }
        public int Pack_Weight_Type_Id { get; set; }
        public int SKU_Id { get; set; }
        public int SKU_SubType_Id { get; set; }
        public int Pack_Type_Id { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }

        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
    }
    public class BulkCSIModel
    {
        public List<BulkCSIExcelFields> BulkCSI { get; set; }
        public string Uploaded_Excel_Display_Name { get; set; }
        public string Uploaded_Excel_Name { get; set; }
        public string CreateBy { get; set; }
    }
    public class BulkCSIExcelFields
    {
        public string Dispatch_DC_Location_Code { get; set; }
        public string Region { get; set; }
        //    public DateTime Delievery_Date { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Indent_Template_Name { get; set; }
        public string Price_Template_Name { get; set; }
        public string SKU_SubType { get; set; }
        public string SKU_Name { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public string Grade { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string UOM { get; set; }
        //   public Nullable<double> Dispatch_Qty { get; set; }

        public string Delievery_Date { get; set; }
        public string Dispatch_Qty { get; set; }


        //      public string Location_Code { get; set; }
        public string Indent_Type { get; set; }

        public string SKU_Type { get; set; }
        public int Pack_Weight_Type_Id { get; set; }
        public int SKU_Id { get; set; }
        public int SKU_SubType_Id { get; set; }
        public int Pack_Type_Id { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string Delivery_cycle { get; set; }
        public string Delivery_Type { get; set; }
        public string Indent_Code { get; set; }
        public string Price_Template_Code { get; set; }
        public Nullable<int> Price_Template_ID { get; set; }
        public int Indent_ID { get; set; }
        public Nullable<int> User_Location_Id { get; set; }
        public string User_Location_Code { get; set; }
        public string User_Location { get; set; }
        public string User_Type { get; set; }
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
        //  public List<StatusFields> ErrorReport { get; set; }
        //
    }
    public class BulkCSIExcelFieldsReturnModel
    {
        //   public Nullable<int> User_Location_Id { get; set; }
        //   public string User_Location_Code { get; set; }
        //   public string User_Location { get; set; }
        //   public string User_Type { get; set; }
        //   public string Dispatch_DC_Location_Code { get; set; }
        ////   public string Region { get; set; }
        //   //    public DateTime Delievery_Date { get; set; }
        //   public Nullable<int> Customer_Id { get; set; }
        //   public Nullable<int> Price_Template_ID { get; set; }
        //   public Nullable<int> SKU_Type_Id { get; set; }
        //   public string Customer_Name { get; set; }
        //   public string Customer_Code { get; set; }
        //   public string Indent_Template_Name { get; set; }
        //   public string Price_Template_Name { get; set; }
        //   public string SKU_SubType { get; set; }
        //   public string SKU_Name { get; set; }
        //   public string Grade { get; set; }
        //   public string Pack_Type { get; set; }
        //   public string Pack_Size { get; set; }
        //   public string Pack_Weight_Type { get; set; }
        //   public string UOM { get; set; }
        //   //   public Nullable<double> Dispatch_Qty { get; set; }
        //   public string Indent_Code { get; set; }
        //   public string Price_Template_Code { get; set; }

        //   public string Delievery_Date { get; set; }
        //   public string Dispatch_Qty { get; set; }
        //   public string Delivery_cycle { get; set; }
        //   public string Delivery_Type { get; set; }
        //   public string Indent_Type { get; set; }

        //   public string SKU_Type { get; set; }
        //   public int Pack_Weight_Type_Id { get; set; }
        //   public int SKU_Id { get; set; }
        //   public int SKU_SubType_Id { get; set; }
        //   public int Pack_Type_Id { get; set; }
        //   public int Indent_ID { get; set; }
        public string Uploaded_Excel_Name { get; set; }
        public bool status { get; set; }
        public int lineNumber { get; set; }

        public int ValidLinesCount { get; set; }
        public int InvalidLinesCount { get; set; }
        public int TotalLinesCount { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }




        public List<BulkCSIExcelFieldsReturn> ValidDatas { get; set; }
        public List<BulkCSIExcelFieldsReturn> InvalidDatas { get; set; }
    }
    public class SALUpdateEntity
    {
        public string id { get; set; }
        public IEnumerable<SALIDs> SalIds { get; set; }
    }
    public class SALIDs
    {
        public int csi_LineId { get; set; }
        public int statusflag { get; set; }
    }

    public class CSI_LineItems_Entity
    {
        public int CSI_Line_Id { get; set; }
        public Nullable<int> CSI_id { get; set; }
        public Nullable<int> Expected_Route_Id { get; set; }
        public string Expected_Route_Alias_Name { get; set; }
        public string Expected_Route_Code { get; set; }
        public Nullable<int> Expected_Delivering_Sales_Person_Id { get; set; }
        public string Expected_Delivering_Sales_Person_Name { get; set; }
        public string CSI_Number { get; set; }
        public int? SKU_ID { get; set; }
        //public int SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string Pack_Size { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public string Pack_Type { get; set; }
        public Nullable<int> Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Indent_Qty { get; set; }
        public string Material_Type { get; set; }
        public Nullable<decimal> Total_Qty { get; set; }
        public string Remark { get; set; }
        public Nullable<double> price { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public string DC_Code { get; set; }
        public Nullable<System.DateTime> CSI_Raised_date { get; set; }
        public Nullable<System.DateTime> CSI_Timestamp { get; set; }
        public string Customer_Name { get; set; }
        public string Delivery_Location_Code { get; set; }
        public string Delivery_cycle { get; set; }
        public Nullable<System.DateTime> Delivery_Expected_Date { get; set; }
        public string Delievery_Type { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string CSI_raised_by { get; set; }
        public string SKU_Type { get; set; }
        public string CSI_Approved_by { get; set; }
        public Nullable<System.DateTime> CSI_Approved_date { get; set; }
        public string CSI_type { get; set; }
        public string Indent_Name { get; set; }
        public string Rate_Template_Name { get; set; }
        public string User_Location { get; set; }
        public Nullable<System.DateTime> Rate_Template_Valitity_upto { get; set; }
        public Nullable<System.DateTime> Validatity_Date { get; set; }
    }

    public class SI_Num_Gen_Entity
    {
        public int ST_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> ST_Last_Number { get; set; }
    }
    public class CSIbulkApprovalEntity
    {
        public List<CSIbulkIdsEntity> bulkid { get; set; }
    }

    public class CSIbulkIdsEntity
    {
        public int CSI_id { get; set; }
        public Nullable<bool> CSI_Approved_Flag { get; set; }
        public Nullable<System.DateTime> CSI_Approved_date { get; set; }
        public string Reason { get; set; }
        public string CSI_Approved_by { get; set; }
    }

    public class SearchDCLOC
    {
        public int Dc_Id { get; set; }
        public string DC_Code { get; set; }
        public string Dc_Name { get; set; }
        public string UserType { get; set; }
    }

    public class FilterClass
    {
        public List<FilterProp> FilterCustomers { get; set; }
        public int Sales_Person_Id { get; set; }
    }

    public class FilterProp
    {
        public string DC_Code { get; set; }
    }

    public class ReturnCustomers
    {
        public int? Cust_Id { get; set; }
        public string Cust_Code { get; set; }
        public string Cust_Name { get; set; }
        public string DC_Code { get; set; }
    }

    public class ReturnSuppliers
    {
        public int supplier_Id { get; set; }
        public string supplierCode { get; set; }
        public string SupplierName { get; set; }
        public string DC_Code { get; set; }
    }

    public class ReturnDCCustomers
    {
        public int? Cust_Id { get; set; }
        public string Cust_Code { get; set; }
        public string Cust_Name { get; set; }
        public string DC_Code { get; set; }
    }
}
