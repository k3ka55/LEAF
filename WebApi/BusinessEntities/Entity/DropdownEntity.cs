using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{

    public class CustSuppDDEntity
    {
        public IEnumerable<CustSupplierDDModel> CustCategory { get; set; }

        public IEnumerable<CustSupplierDDModel> CustStoreType { get; set; }
        public IEnumerable<CustSupplierDDModel> CustDeliveryDays { get; set; }
        public IEnumerable<CustSupplierDDModel> CustDeliveryType { get; set; }
        public IEnumerable<CustSupplierDDModel> CustGRNRecvSchedule { get; set; }
        public IEnumerable<CustSupplierDDModel> CustGRNRecvType { get; set; }
        public IEnumerable<CustSupplierDDModel> CustCustomerreturnPolicy { get; set; }
        public IEnumerable<CustSupplierDDModel> CustPricingChangeSchedule { get; set; }
        public IEnumerable<RegionEntity> CustRegion { get; set; }
        public IEnumerable<CustSupplierDDModel> EngagementType { get; set; }
        public IEnumerable<CustSupplierDDModel> CustIndentType { get; set; }
        public IEnumerable<CustSupplierDDModel> PaymentType { get; set; }
        public IEnumerable<CustSupplierDDModel> CreditPeriod { get; set; }
        public IEnumerable<DCMasterEntity> CustDCMaster { get; set; }
        public IEnumerable<LocationMasterEntity> CustLocationMaster { get; set; }



        public HttpStatusCode StatusCode { get; set; }
    }
    public class CustRegDDEntity
    {
        public IEnumerable<CustSupplierDDModel> CustRegCreatedBy { get; set; }
        public IEnumerable<LocationModel> Locations { get; set; }
        public IEnumerable<CustSupplierDDModel> CustRegCustomers { get; set; }        
      
        public HttpStatusCode StatusCode { get; set; }
    }
    public class Template_Type
    {
        public int Template_Type_Id { get; set; }
        public string Template_Type_Name { get; set; }

    }
    public class DropdownEntity
    {
        public IEnumerable<DCMasterModel> DCMaster { get; set; }
        public IEnumerable<PaymentCycleModel> PaymentCycle { get; set; }
        public IEnumerable<PaymentTypeModel> PaymentType { get; set; }
        public IEnumerable<PORequitionedByModel> PORequitionedBy { get; set; }
        public IEnumerable<POTypeModel> POType { get; set; }
        public IEnumerable<UnitModel> Unit { get; set; }
        public IEnumerable<MaterialModel> MaterialType{ get; set; }
        public IEnumerable<WastageTypeModel> WastageType { get; set; }
        public IEnumerable<StatusModel> GetStatus { get; set; }
        public IEnumerable<StatusLineItemModel> GetLineItemStatus { get; set; }
        public IEnumerable<VoucherModel> VoucherType { get; set; }
        public IEnumerable<SKUModel> SKU { get; set; }
       
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<SupplierModel> Supplier { get; set; }
        public IEnumerable<SKUMainGroupModel> SKUMainGroup { get; set; }
        public IEnumerable<SKUSubGroupModel> SKUSubGroup { get; set; }
        public IEnumerable<Intermediate_DCModel> Intermediate_DC { get; set; }
        public IEnumerable<Delivery_CycleModel> Delivery_Cycle { get; set; }
        public IEnumerable<Customer_Model> Customer { get; set; }
        public IEnumerable<GradeTypeModel> GradeType { get; set; }
        public IEnumerable<LocationMasterEntity> LocationMaster { get; set; }
        public IEnumerable<RoleMasterEntity> RoleMaster { get; set; }
        //----From Resource File Classes-------------
        public IEnumerable<MaterialResourceModel> MaterialResource { get; set; }
       /// <summary>
       /// 
       /// </summary>
        public IEnumerable<Pack_Type> PackType { get; set; }
       // public IEnumerable<STI_Pack_Type> STIPackType { get; set; }
       public IEnumerable<Pack_Size> Pack_Size { get; set; }
        public IEnumerable<Dispatch_Type> DispatchType { get; set; }
        public IEnumerable<SKU_Type> SKU_Type { get; set; }
        public IEnumerable<SKU_SubType> SKU_SubType { get; set; }
        public IEnumerable<Invoice_Type> Invoice_Type { get; set; }
        public IEnumerable<SKU_Category> SKU_Category { get; set; }
        public IEnumerable<Pack_Type> Pack_Type { get; set; }
        public IEnumerable<Pack_Weight_Type> Pack_Weight_Type { get; set; }
        public IEnumerable<Delivery_Type> Delivery_Type { get; set; }
        public IEnumerable<CSIDelivery_Cycle> CSIDelivery_Cycle { get; set; }
       // public List<Pack_Size> Pack_Size { get; set; }
        public IEnumerable<DataType> Data_Type { get; set; }
        public IEnumerable<Field_Name> Field_Name { get; set; }
        public IEnumerable<Customer_Category> Customer_Category { get; set; }
        public IEnumerable<RegionEntity> Region { get; set; }
        public IEnumerable<User_Login_Type> User_Login_Type { get; set; }
      
    }
    public class barcodeDDEntity
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<Best_BeforeEntity> Best_Before { get; set; }
        public IEnumerable<SKUModel> SKU { get; set; }
       // public List<BothSKUEntity> BothSKU { get; set; }
        public IEnumerable<SKU_Type> SKU_Type { get; set; }
        public IEnumerable<Customer_Model> Customer { get; set; }
    }

    public class RegionEntity
    {
        public int Region_Id { get; set; }
        public string Region_Code { get; set; }
        public string Region_Name { get; set; }
    }

    public class STI_Pack_Type
    {
        public int STI_Pack_Type_Id { get; set; }
        public string STI_Pack_Type_Name { get; set; }
    }
    
     public class Best_BeforeEntity
    {
        public string Best_Before { get; set; }
    }
    public class PackSizeEntity
    {
        public IEnumerable<Pack_Size> PackSize { get; set; }
    }
   
    public class Field_Name
    {
        public int Field_Name_Id { get; set; }
        public string Field_Name_Value { get; set; }
    }

    public class CSIDelivery_Cycle
    {
        public int CSIDelivery_Cycle_Id { get; set; }
        public string CSIDelivery_Cycle_Value { get; set; }
    }

    public class Delivery_Type
    {
        public int Delivery_Type_Id { get; set; }
        public string Delivery_Type_Value { get; set; }
    }
   
    public class Pack_Size
    {
        public int Pack_Size_Id { get; set; }
        public string Pack_Size_Type { get; set; }
        public string Pack_Size_Value { get; set; }
    }

    public class Vehicle_No
    {
        public int Vehicle_No_Id { get; set; }
        public string Vehicle_No_Name { get; set; }
        public string Vehicle_Type { get; set; }
        public string Vehicle_DC_Code { get; set; }
    }

    public class Pack_Weight_Type
    {
        public int Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type_Name { get; set; }
    }

    public class Pack_Type
    {
        public int Pack_Type_Id { get; set; }
        public string Pack_Type_Name { get; set; }
    }

    public class SKU_Category
    {
        public int SKU_Category_Id { get; set; }
        public string SKU_Category_Name { get; set; }
    }

    public class Invoice_Type
    {
        public int Invoice_Type_Id { get; set; }
        public string Invoice_Type_Name { get; set; }
    }

    public class SKU_Type
    {
        public int SKU_Type_Id { get; set; }
        public string SKU_Type_Name { get; set; }
        public string SKU_Type_Code { get; set; }
    }
    public class SKU_SubType
    {
        public int SKU_SubType_Id { get; set; }
        public string SKU_SubType_Name { get; set; }
    }

    public class Dispatch_Type
    {
        public int Dispatch_Type_Id {get;set;}
        public string Dispatch_Type_Name {get;set;}
    }
    public class User_Login_Type
    {
        public int User_Login_Type_Id { get; set; }
        public string User_Login_Type_Name { get; set; }
    }

    //public class STI_Pack_Type
    //{
    //    public int STI_Pack_Type_Id { get; set; }
    //    public string STI_Pack_Type_Name { get; set; }
    //}

    public class Customer_Model
    {
        public int Cust_Id { get; set; }
        public string Cust_Name { get; set; }
        public string Cust_Code { get; set; }
    }
    public class Intermediate_DCModel
    {
        public int Intermediate_DC_Id { get; set; }
        public string Intermediate_DC_Name { get; set; }
    }
    public class Delivery_CycleModel
    {
        public int Delivery_Cycle_Id { get; set; }
        public string Delivery_Cycle_Name { get; set; }
    }
    public class SKUMainGroupModel
    {
          public int SKU_Main_Group_Id { get; set; }
        public string SKU_Description{get;set;}
    }
    public class SKUSubGroupModel
    {
        public int SKU_Sub_Group_Id { get; set; }
        public string SKU_Description { get; set; }
    }
    public class PaymentCycleModel
    {
        public string Payment_Cycle_Name { get; set; }
    }

    public class PaymentTypeModel
    {
        // public int PaymentType_Id { get; set; }
        public string PaymentTypeName { get; set; }
    }
    public class PORequitionedByModel
    {
        // public int PO_Req_Id { get; set; }
        public string PO_Rqe_Name { get; set; }
    }
    public class POTypeModel
    {
        // public int PO_Req_Id { get; set; }
        public string PO_Rqe_Name { get; set; }
    }
    public class UnitModel
    {
        // public int Unit_Id { get; set; }
        public string Unit_Name { get; set; }
    }
    
    public class DCMasterModel
    {
        public int DC_Id { get; set; }
        public string DC_Code { get; set; }
        public string DC_City { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }

    public class DCLocationModel
    {
        public List<TargetLocationModel> targetLocations { get; set; }
    }

    public class TargetLocationModel
    {
        public int TargetLocation_Id { get; set; }
        public string TargetLocation_Code { get; set; }
        public string TargetLocation_City { get; set; }
        public string TargetLocation_Type { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }
    public class SKUModel
    {
        public int SKUId { get; set; }
        public string SKUName { get; set; }
        public string SKUCode { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
    }
    public class SupplierModel
    {
        public int supplier_Id { get; set; }
        public string SupplierName { get; set; }
        public string supplierCode { get; set; }
    }
    public class MaterialModel
    {
        public int Material_Id { get; set; }
        public string Material_Name { get; set; }
    }
    public class MaterialResourceModel
    {
        public int Material_Resource_Id { get; set; }
        public string Material_Resource_Name { get; set; }       
    }
    public class GradeTypeModel
    {
        public int GradeType_Id { get; set; }
        public string GradeType_Name { get; set; }
    }

    public class CustSupplierDDModel
    {
        public int Val_Id { get; set; }
        public string Val_Name { get; set; }
    }
    public class WastageTypeModel
    {
        public int WastageTypeModel_Id { get; set; }
        public string WastageTypeModel_Name { get; set; }
    }
    public class DataType
    {
        public int DataType_Id { get; set; }
        public string DataType_Name { get; set; }
    }
    public class Tally_Activity
    {
        public int Tally_Activity_Id { get; set; }
        public string Tally_Activity_Name { get; set; }
     
    }
    public class Tally_Module
    {
        public int Tally_Module_Id { get; set; }
        public string Tally_Module_Name { get; set; }
        public string Tally_Module_Activity { get; set; }
    }

    public class Customer_Category
    {
        public int Category_Id { get; set; }
        public string Category { get; set; }
        public string Category_Code { get; set; }
    }

    public class StatusModel
    {
        public int StatusModel_Id { get; set; }
        public string StatusModel_Name { get; set; }
    }
    public class StatusLineItemModel
    {
        public int StatusLineItemModel_Id { get; set; }
        public string StatusLineItemModel_Name { get; set; }
    }
    public class VoucherModel
    {
        public int VoucherType_Id { get; set; }
        public string VoucherType_Name { get; set; }
    }
}