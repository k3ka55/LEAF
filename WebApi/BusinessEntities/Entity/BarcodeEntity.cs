using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{

    public class TemplateNameAvailEntity
    {
        public IEnumerable<TemplateName> TempName { get; set; }
    }
    public class TemplateName
    {
        public string Available{get;set;}
        public Nullable<bool> Status { get; set; }
    }

    public class PrintedBarcodeDetailsEntity
    {
        public int Printed_Barcode_ID { get; set; }
        public string Generated_Bar_Code { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Template_Id { get; set; }
        public string Template_Name { get; set; }
        public string Product_Type { get; set; }
        public Nullable<System.DateTime> Packed_On { get; set; }
        public string Best_Before { get; set; }
        public Nullable<int> Number_Of_Copies { get; set; }
        public string FSSAI { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Name { get; set; }
        public string SKU_Leaf_flag { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Quantity { get; set; }
        public string EAN { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public string Updated_By { get; set; }
    }
    public class CustTemplateEntity
    {
        public string Template_Id { get; set; }
        public string Template_name { get; set; }
        public string Template_Type { get; set; }
      //  public string Field_name { get; set; }
        public Nullable<int> Cust_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Cust_Name { get; set; }
        public List<CustIdEntity> CustSet { get; set; }
             
    }
    

    public class CustomerLabelTemplateEntity
    {
        public int Cust_Label_Template_Id { get; set; }
        public string Template_Id { get; set; }
        public string Template_name { get; set; }
        public string Template_Type { get; set; }
        public List<FieldEntity> Fields { get; set; }
        public string Field_Value { get; set; }
        public Nullable<int> Field_Id { get; set; }
        public string Field_Name { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<FieldEntity> FieldsSet { get; set; }
        
    }
    public class FieldEntity
    {
        public string Template_Id { get; set; }
        public string Template_name { get; set; }
        public Nullable<int> Field_Id { get; set; }
        public string Field_Name { get; set; }
        public string Field_Value { get; set; }
        public string Remarks { get; set; }
    }
    public class CustIdEntity
    {

        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
    }
    public class CustomerLabelTemplateMappingEntity
    {
        public int Cust_Label_map_Id { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Template_Id { get; set; }
        public string Template_Name { get; set; }
        public string Customer_Name { get; set; }
        public List<CustIdEntity> CustIds { get; set; }
    }
    public class CustomerSKUMasterEntity
    {
        public int Customer_SKU_Master_Id { get; set; }
        public string Customer_SKU_Code { get; set; }
        public string Customer_SKU_Name { get; set; }
        public string UOM { get; set; }
        public string EAN_Number { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class LabelFieldsEntity
    {
        public int Field_Id { get; set; }
        public string Field_Name { get; set; }
        public string Data_Type { get; set; }
        public Nullable<int> Size { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
