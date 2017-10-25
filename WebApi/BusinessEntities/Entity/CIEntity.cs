using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
  public class CIEntity
    {
        public int Indent_ID { get; set; }
        public string Indent_Code { get; set; }
        public string Indent_Name { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region { get; set; }
        public string Indent_Type { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Delivery_Address { get; set; }
        public string Dispatch_DC_Code { get; set; }
        public string Delivery_cycle { get; set; }
        public Nullable<System.DateTime> Delivery_Expected_Date { get; set; }
        public string Delivery_Type { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<int> Price_Template_ID { get; set; }
        public string Price_Template_Code { get; set; }
        public string Price_Template_Name { get; set; }
       // public string Price_Template_Code { get; set; }
        public Nullable<System.DateTime> Price_Template_Valitity_upto { get; set; }
        public Nullable<System.DateTime> Validatity_Date { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string Region_Code { get; set; }
        public int counting { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public string Stopwatch { get; set; }
        public List<CustomerIndentLineItemEntity> LineItems { get; set; }
      }

  public class CustomerIndentLineItemEntity
  {

      public int CI_Line_Id { get; set; }
      public Nullable<int> Indent_ID { get; set; }
      public Nullable<int> SKU_Id { get; set; }
      public string SKU_Name { get; set; }
      public Nullable<int> SKU_SubType_Id { get; set; }
      public string SKU_SubType { get; set; }
      public Nullable<int> Pack_Type_Id { get; set; }
      public string Pack_Type { get; set; }
      public string Pack_Size { get; set; }
      public Nullable<int> Pack_Weight_Type_Id { get; set; }
      public string Pack_Weight_Type { get; set; }
      public string UOM { get; set; }
      public string Grade { get; set; }
      public Nullable<double> Price { get; set; }
      public double Estimated_Stock_Qty { get; set; }
      public Nullable<double> Dispatch_Qty { get; set; }
      public Nullable<System.DateTime> CreatedDate { get; set; }
      public Nullable<System.DateTime> UpdateDate { get; set; }
      public string CreateBy { get; set; }
      public string UpdateBy { get; set; }
      public string HSN_Code { get; set; }
      public Nullable<double> Total_GST { get; set; }
      public Nullable<double> CGST { get; set; }
      public Nullable<double> SGST { get; set; }
      public string Indent_Code { get; set; }
      public string Indent_Name { get; set; }
      public Nullable<int> DC_Id { get; set; }
      public string DC_Code { get; set; }
      public Nullable<int> Location_Id { get; set; }
      public string Location_Code { get; set; }
      public Nullable<int> Region_Id { get; set; }
      public string Region { get; set; }
      public string Indent_Type { get; set; }
      public Nullable<int> Customer_Id { get; set; }
      public string Customer_Name { get; set; }
      public string Customer_Code { get; set; }
      public string Customer_Delivery_Address { get; set; }
      public string Dispatch_DC_Code { get; set; }
      public string Delivery_cycle { get; set; }
      public Nullable<System.DateTime> Delivery_Expected_Date { get; set; }
      public string Delivery_Type { get; set; }
      public Nullable<int> SKU_Type_Id { get; set; }
      public string SKU_Type { get; set; }
      public Nullable<int> Price_Template_ID { get; set; }
      public string Price_Template_Code { get; set; }
      public string Price_Template_Name { get; set; }
      // public string Price_Template_Code { get; set; }
      public Nullable<System.DateTime> Price_Template_Valitity_upto { get; set; }
      public string Reason { get; set; }
     
      public string Region_Code { get; set; }










  }
  public class CustomerIndentEditReturnEntity
  {
      public int Indent_ID { get; set; }
      public string Indent_Name { get; set; }
      public string Indent_Code { get; set; }
      public Nullable<int> Customer_Id { get; set; }
      public string Customer_Name { get; set; }
      public string Customer_Code { get; set; }
  }

    public class CustomerIndentReturnEntity
    {
        public int Indent_ID { get; set; }
        public string Indent_Name { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region { get; set; }
        public string Region_Code { get; set; }
        public string Indent_Code { get; set; }
    }

    public class CIExcelImport
    {
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
    }

    public class fileImport
    {
        public int indentID { get; set; }
        public string FileString { get; set; }
    }
}
