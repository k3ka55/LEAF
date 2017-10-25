using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class StockTransferIntentEntity
    {

        public int STI_id { get; set; }
        public string STI_Number { get; set; }
        public Nullable<int> Indent_Raised_by_DC_Id { get; set; }
        public string Indent_Raised_by_DC_Code { get; set; }
        public Nullable<System.DateTime> STI_RLS_date { get; set; }
        public Nullable<int> Material_Source_id { get; set; }
        public string Material_Source { get; set; }
        public string Intermediate_DC_Code { get; set; }
        public Nullable<int> Delivery_DC_id { get; set; }
        public string Delivery_DC_Code { get; set; }
        public string STI_Type { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public string STI_Delivery_cycle { get; set; }
        public Nullable<System.DateTime> DC_Delivery_Date { get; set; }
        public string STI_raise_by { get; set; }
        public string STI_Approve_by { get; set; }
        public Nullable<bool> STI_Approval_Flag { get; set; }
        public Nullable<System.DateTime> STI_Approved_date { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string DeleteReason { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int Counting { get; set; }
        public IEnumerable<ST_Qty_SumEntity> ST_Qty_Sum { get; set; }
        public IEnumerable<SIT_LineItems> STIDetails { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        
    }
    public class ST_Qty_SumEntity
    {
        public Nullable<double> Total_Qty_Sum { get; set; }
    }
    public class STIUpdateEntity
    {
        public string id { get; set; }
        public IEnumerable<STILIDs> StiLIds { get; set; }
    }
    public class STILIDs
    {
        public int sti_LineId { get; set; }
        public int statusflag { get; set; }
    }

    public class STIExcelFields
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
        public Nullable<double> Qty { get; set; }
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
      //  public List<StatusFields> ErrorReport { get; set; }

    }

    public class StatusFields
    {
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
    }

    public class fileImportSTI
    {
        public string Customer_Code { get; set; }
        public string FileString { get; set; }
    }

    public class SIT_LineItems
    {
        public int STI_Line_Id { get; set; }
        public Nullable<int> STI_id { get; set; }
        public string STI_Number { get; set; }
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
        public double? dis_Qty { get; set; }
        public Nullable<double> Qty { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Total_Qty { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string Indent_Raised_by_DC_Code { get; set; }
        public Nullable<System.DateTime> STI_RLS_date { get; set; }
        public string Material_Source { get; set; }
        public string Delivery_DC_Code { get; set; }
        public string STI_Type { get; set; }
        public Nullable<System.DateTime> DC_Delivery_Date { get; set; }
        public string STI_raise_by { get; set; }
        public string STI_Approve_by { get; set; }
        public Nullable<System.DateTime> STI_Approved_date { get; set; }
    }
    public class STIbulkApprovalEntity
    {
        public List<STIbulkIdsEntity> bulkid { get; set; }
    }
    public class STIbulkIdsEntity
    {
        public int stiId { get; set; }
        public Nullable<bool> STI_Approval_Flag { get; set; }
        public Nullable<System.DateTime> STI_Approved_date { get; set; }
        public string Reason { get; set; }
        public string STI_Approve_by { get; set; }
    }
}
