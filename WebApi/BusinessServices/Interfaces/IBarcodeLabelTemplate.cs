using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
   public interface IBarcodeLabelTemplate
    {
       List<CustTemplateEntity> GetAllCustomers();
       List<CustomerLabelTemplateMappingEntity> GetMappedCustomerByTId(string Template_Id);
       bool DeleteLabelTemplateMapping(string Template_Id);
       List<CustomerLabelTemplateEntity> GetLabelTemplateByName(string Template_name);
       TemplateName CheckTempNameAvalibility(string TempName);
 //      CustomerSKUMasterEntity GetCustskuById(int Customer_SKU_Master_Id);
   //    List<CustomerSKUMasterEntity> GetCustSKUByCategory(string Customer_SKU_Name);
   //    IEnumerable<CustomerSKUMasterEntity> GetAllCustomerSKUMaster();
    //   bool CreateCustSKU(CustomerSKUMasterEntity cskuEntity);
    //   bool UpdateCustSKU(int Customer_SKU_Master_Id, CustomerSKUMasterEntity custskuEntity);
   //    bool DeleteCustSKU(int Customer_SKU_Master_Id);
       List<CustTemplateEntity> GetCustomerTemplate(int? cust_Id);
       IEnumerable<CustTemplateEntity> GetAllCustomerTemplate();
       List<LabelFieldsEntity> GetLabelFieldById(int Field_Id);
       List<LabelFieldsEntity> GetLabelFieldByCategory(string Field_Name);
       IEnumerable<LabelFieldsEntity> GetAllLabelField();
       bool CreateLabelField(LabelFieldsEntity LFEntity);
       bool UpdateLabelField(int Field_Id, LabelFieldsEntity LFEntity);
       bool DeleteLabelField(int Field_Id);
       
       List<CustomerLabelTemplateEntity> GetLabelTemplateById(int Cust_Label_Template_Id);
       IEnumerable<CustomerLabelTemplateEntity> GetAllLabelTemplate();
       bool CreateLabelTemplate(CustomerLabelTemplateEntity CLTEntity);
       //bool UpdateLabelTemplate(int Cust_Label_Template_Id, CustomerLabelTemplateEntity CLTEntity);
       bool UpdateLabelTemplate(string Template_Id, CustomerLabelTemplateEntity CLTEntity);
       bool DeleteLabelTemplate(string Template_Id);

       List<PrintedBarcodeDetailsEntity> GetPrintBarcodeById(int Printed_Barcode_ID);
       List<PrintedBarcodeDetailsEntity> GetPrintBarcodeByCategory(string Generated_Bar_Code);
       IEnumerable<PrintedBarcodeDetailsEntity> GetAllPrintBarcode();
       bool CreatePrintBarcode(PrintedBarcodeDetailsEntity pbEntity);
       bool UpdatePrintBarcode(int Printed_Barcode_ID, PrintedBarcodeDetailsEntity pbEntity);
       bool DeletePrintBarcode(int Printed_Barcode_ID);

       bool CreateLabelTemplateMapping(CustomerLabelTemplateMappingEntity LabelTemplateMappingEntity);
       bool UpdateLabelTemplateMapping(string Template_Id, CustomerLabelTemplateMappingEntity LabelTemplateMappingEntity);
    }
}
