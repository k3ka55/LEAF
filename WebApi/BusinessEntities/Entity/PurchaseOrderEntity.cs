using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class PurchaseOrderEntity
    {
        public int Po_id { get; set; }
        public string PO_Number { get; set; }
        public string DC_Code { get; set; }
        public DateTime? PO_RLS_date { get; set; }
        public Nullable<int> Material_Source_id { get; set; }
        public string Material_Source { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_Code { get; set; }
        public string Supplier_name { get; set; }
        public string PO_Requisitioned_by { get; set; }
        public string PO_Type { get; set; }
        public string Payment_cycle { get; set; }
        public string Payment_type { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string PO_raise_by { get; set; }
        public string PO_Approve_by { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
        public Nullable<bool> PO_Approval_Flag { get; set; }
        public Nullable<System.DateTime> PO_Approved_date { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public string Reason { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public IEnumerable<PO_Qty_SumEntity> PO_Qty_Sum { get; set; }

        public int Counting { get; set; }

        public List<PurchaseSubEntity> PurchaseDetails { get; set; }
    }
    public class POWithLineItemEntity
    {
        public int Po_id { get; set; }
        public string PO_Number { get; set; }
        public int PO_Line_Id { get; set; }
        public string DC_Code { get; set; }
        public DateTime? PO_RLS_date { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<double> A_Grade_Qty { get; set; }
        public Nullable<double> B_Grade_Qty { get; set; }

        public Nullable<double> Total_Qty { get; set; }
        public Nullable<double> Qty { get; set; }
        public Nullable<double> A_Grade_Price { get; set; }
        public Nullable<double> B_Grade_Price { get; set; }
        public string UOM { get; set; }
        public string Status { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_Code { get; set; }
        public string Supplier_name { get; set; }
        public int Counting { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public Nullable<bool> is_Create { get; set; }
        public Nullable<bool> is_Edit { get; set; }
        public Nullable<bool> is_View { get; set; }
        public Nullable<bool> is_Delete { get; set; }
     
    }
    public class PORateAudit
{
     public int Po_Rate_audit_Id { get; set; }
        public Nullable<int> Po_id { get; set; }
        public string PO_Number { get; set; }
        public string DC_Code { get; set; }
        public string PO_Type { get; set; }
        public string Sku_Type { get; set; }
        public string Sku_SubType { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> PO_Line_Id { get; set; }
        public Nullable<System.DateTime> PO_RLS_date { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Code { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Qty { get; set; }
        public Nullable<double> A_Grade_Qty { get; set; }
        public Nullable<double> B_Grade_Qty { get; set; }
       
        public Nullable<double> A_Grade_Price { get; set; }
        public Nullable<double> B_Grade_Price { get; set; }
     
}
    public class PurchasePOEntity
    {
        public string PO_raise_by { get; set; }
    }
    //[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    //public sealed class DecimalPrecisionAttribute : Attribute
    //{
    //    public DecimalPrecisionAttribute(byte precision, byte scale)
    //    {
    //        Precision = 18;
    //        Scale = 2;

    //    }

    //    public byte Precision { get; set; }
    //    public byte Scale { get; set; }

    //}
 
    public class PurchaseSubEntity
    {
        public int PO_Line_Id { get; set; }
        public Nullable<int> Po_id { get; set; }
        public string PO_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string SKU_SubType { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Qty { get; set; }
      //  public string Qtys { get; set; }
        public Nullable<double> A_Grade_Qty { get; set; }

        public Nullable<double> B_Grade_Qty { get; set; }
      // [DecimalPrecision(18, 2)]
         //  [DisplayFormat(DataFormatString = "{0:0.0#####}", ApplyFormatInEditMode = true)]
     //   [DisplayFormat(DataFormatString = "{0:0.00}")]
        //[DisplayFormat(DataFormatString = "{0:#,##0.00000#}", ApplyFormatInEditMode = true)]
       // [DataType("decimal?(18,2)")]
        public Nullable<double> Total_Qty { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<double> A_Grade_Price { get; set; }
        public Nullable<double> B_Grade_Price { get; set; }

        public string Supplier_name { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string DC_Code { get; set; }
        public string SKU_Type { get; set; }
        public DateTime? PO_RLS_date { get; set; }
        public string Material_Source { get; set; }
        public string PO_Requisitioned_by { get; set; }
        public string PO_Type { get; set; }
        public string Payment_cycle { get; set; }
        public string Payment_type { get; set; }
        public string PO_raise_by { get; set; }
        public Nullable<System.DateTime> PO_Approved_date { get; set; }
        public string PO_Approve_by { get; set; }
     
    }

    public class PO_Qty_SumEntity
    {
        public Nullable<double> Total_Qty_Sum { get; set; }
    }

    public class poNumber
    {
        public string PO_Number { get; set; }
    }

    public class stiNumber
    {
        public string STI_Number { get; set; }
    }
    public class csiNumber
    {
                public string CSI_Number { get; set; }
    }
    public class csiCreators
    {
        public string Sales_Person_Name { get; set; }
        public int Sales_Person_Id { get; set; }
    }
    public class cdnNumber
    {
        public string CDN_Number { get; set; }
    }

    public class stnNumber
    {
        public string STN_Number { get; set; }
    }
public class UpdatePORateEntity
{
    public List<LineIdsEntity> LineIds { get; set; }
}
 public class LineIdsEntity
    {
        public int LineId { get; set; }
        public Nullable<double> A_Grade_Price { get; set; }
        
    }

    public class bulkApprovalEntity
    {
        public List<bulkIdsEntity> bulkid { get; set; }
    }

    public class bulkIdsEntity
    {
        public int poId { get; set; }
        public Nullable<bool> PO_Approval_Flag { get; set; }
        public Nullable<System.DateTime> PO_Approved_date { get; set; }
        public string Reason { get; set; }
        public string PO_Approve_by { get; set; }
    }
}
