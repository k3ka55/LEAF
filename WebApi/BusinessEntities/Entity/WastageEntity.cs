using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class WastageEntity
    {
        public int Wastage_Id { get; set; }
        public string Wastage_Number { get; set; }
        public Nullable<int> DC_Id { get; set; }
        
        public string DC_Code { get; set; }
        public string DC_Name { get; set; }
        public Nullable<int> Ref_Id { get; set; }
        public string Ref_Number { get; set; }
        public string Wastage_Type { get; set; }
        public string Wastage_raisedBy { get; set; }
        public Nullable<System.DateTime> Wastage_Rls_Date { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public string Remark { get; set; }
        public string Reject_Reason { get; set; }
        public Nullable<bool> Wastage_Approval_Flag { get; set; }
        public Nullable<int> Wastage_approved_user_id { get; set; }
        public string Wastage_approved_by { get; set; }
        public Nullable<System.DateTime> Wastage_approved_date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public IEnumerable<WS_QtySumEntity> WS_Qty_Sum { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public int Counting { get; set; }

        public List<WastageLineItemEntity> WastageLineDetails { get; set; }
    }



    public class WastageLineItemEntity
    {
        public int Wastage_Line_Id { get; set; }
        public Nullable<int> Wastage_Id { get; set; }
        public string Wastage_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> Ref_Id { get; set; }
        public Nullable<int> Ref_Line_Id { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Wastage_Qty { get; set; }
        public Nullable<double> Wasted_Qty_Price { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> Stock_Reduce_Flag { get; set; }
        public Nullable<bool> is_Stk_Update { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string DC_Name { get; set; }
        public string Ref_Number { get; set; }
        public string Wastage_Type { get; set; }
        public string Wastage_raisedBy { get; set; }
        public Nullable<System.DateTime> Wastage_Rls_Date { get; set; }
        public string Remark { get; set; }
        public string Wastage_approved_by { get; set; }
        public Nullable<System.DateTime> Wastage_approved_date { get; set; }
    }
    public class WastageSupplierInfoEntity
    {
        public int Id { get; set; }
        public Nullable<int> Wastage_Line_Id { get; set; }
        public string Wastage_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string Grade { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_Code { get; set; }
        public string Supplier_name { get; set; }
    }

    public class WS_QtySumEntityModel
    {
        public string DC_Code { get; set; }
        public Nullable<double> Total_Qty_Sum { get; set; }
        public string SkuName { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<double> Process { get; set; }
        public Nullable<double> Floor { get; set; }
        public string Grade { get; set; }
        public string Unit { get; set; }
        public string Wastage_Type { get; set; }
    }
    public class WS_QtySumEntity
    {
        public string DC_Code { get; set; }
        public double? Total_Qty_Sum { get; set; }
        public string SkuName { get; set; }
        public string SKU_Type { get; set; }
        public double? Process { get; set; }
        public double? Floor { get; set; }
        public string Grade { get; set; }
        public string Unit { get; set; }
        public string Wastage_Type { get; set; }
    }

    public class WeightClass
    {
        public static double Weight { get; set; }
    }

    public class bulkWastApprovalEntity
    {
        public List<bulkWastIdsEntity> bulkWastid { get; set; }
    }

    public class bulkWastIdsEntity
    {
        public int Wastage_Id { get; set; }
        public Nullable<bool> Wastage_Approval_Flag { get; set; }
        public Nullable<System.DateTime> Wastage_approved_date { get; set; }
        public string Reason { get; set; }
        public Nullable<int> Wastage_approved_user_id { get; set; }
        public string Wastage_approved_by { get; set; }


    }

}

