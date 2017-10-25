using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class MaterialList
    {
        public List<Material_MasterEntity> MaterialMaster { get; set; }
        public string FromScreen { get; set; }
    }
    public class MIExcelFields
    {
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
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
        public Nullable<double> SP { get; set; }
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
        //  public List<StatusFields> ErrorReport { get; set; }
        //
    }
    public class fileImportMI
    {
        public string LocationCode { get; set; }
        public string DCCode { get; set; }
        public string Region { get; set; }
        public string SKUType { get; set; }
        public string FileString { get; set; }
    }
    public class Material_MasterEntity
    {
        public List<Material_MasterEntity> ValidDatas { get; set; }
        public List<Material_MasterEntity> InvalidDatas { get; set; }
        public Nullable<bool> ValidEdit { get; set; }
        public List<int> InvalidLineNumbers { get; set; }
        public int Material_Id { get; set; }
        public string Material_Code { get; set; }
        public string Material_Auto_Gen_Code { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public string Material_Name { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public string Pack_Type { get; set; }
        public string Pack_Size { get; set; }
        public Nullable<int> Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public string Reason { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public int LineNumber { get; set; }
    }
}
