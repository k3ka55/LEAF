using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{


    public class RateIndentEntityUnique
    {
        public List<RateTemplateSearchUniqueEntity> LineItems { get; set; }
    }
    public class RateTemplateSearchUniqueEntity
    {
        public string SKU_SubType { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string Material_Auto_Gen_Code { get; set; }
        public string Pack_Type { get; set; }
        public string SKU_Name { get; set; }
        public string Pack_Size { get; set; }
        public string UOM { get; set; }
        public string Selling_Price { get; set; }
        public string Grade { get; set; }
    }
    public class RateIndentEntity
    {
        public int Template_ID { get; set; }
        public string Template_Name { get; set; }
        public Nullable<bool> Is_JDM_Synced { get; set; }
        public Nullable<bool> Is_BNG_Synced { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
       
        public string Region { get; set; }
        public string Region_Code { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public string SKU_Type_Code { get; set; }
        public int? Category_Id { get; set; }
        //public string Category_Code { get; set; }
        public string Customer_Category { get; set; }
        public Nullable<System.DateTime> Valitity_upto { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public string Template_Code { get; set; }

        public int counting { get; set; }
        public string Customer_Category_Code { get; set; }
        // public Nullable<int> Category_Id { get; set; }
        //public string SKU_Type_Code { get; set; }
        // public string Region_Code { get; set; }
        public List<RateTemplateLineitem> LineItems { get; set; }
    }
    public class RateTemplateLineitem
    {
        public int RT_Line_Id { get; set; }
        public Nullable<int> RT_id { get; set; }
        public string Rate_Template_Code { get; set; }
        public string Material_Auto_Num_Code { get; set; }
        public string Material_Code { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public Nullable<int> Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Selling_price { get; set; }
        public Nullable<double> MRP { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }

        public int Template_ID { get; set; }
        public string Template_Name { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region { get; set; }
        public string Region_Code { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public string SKU_Type_Code { get; set; }
        public int? Category_Id { get; set; }
        //public string Category_Code { get; set; }
        public string Customer_Category { get; set; }
        public Nullable<System.DateTime> Valitity_upto { get; set; }
        public string Reason { get; set; }
        public string Template_Code { get; set; }
        public int counting { get; set; }
        public string Customer_Category_Code { get; set; }
    }
    //
    public class RateTempateResponse
    {
        public int Template_ID { get; set; }
        public string Template_Code { get; set; }
        public string Template_Name { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<System.DateTime> Valitity_upto { get; set; }
    }

    public class Template_Fields_SKU
    {
        public int Template_ID { get; set; }
        public string Template_Name { get; set; }
        public Nullable<System.DateTime> Valitity_upto { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region { get; set; }
        public string Template_Code { get; set; }
        // public IEnumerable<Template_SKU_List> SKUList { get; set; }
    }


    public class RateInformation
    {
        public string TemplateID { get; set; }
        public int sku_id { get; set; }
        public int sku_subtype_id { get; set; }
        public int pack_type_id { get; set; }
        public string pack_size { get; set; }
        public string grade { get; set; }
    }

    public class ReturnRate
    {
        public double? Selling_Price { get; set; }
    }

    public class Template_SKU_List
    {
        public string Material_Code { get; set; }
        public int? SKUId { get; set; }
        public string SKUName { get; set; }
        public string SKU_Code { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public string Pack_Type { get; set; }
        public Nullable<int> Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string Pack_Size { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public double? Price { get; set; }
    }

    public class SKUPrice
    {
        public int? SKUId { get; set; }
        public string SKUName { get; set; }
        public double? price { get; set; }
    }

    public class RIExcelImport
    {
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
    }

    public class rifileImport
    {
        public int rateindentID { get; set; }
        public string FileString { get; set; }
    }
}
