using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GrnEntity
    {
        public int INW_Id { get; set; }
        public string GRN_Number { get; set; }
        public string PO_Number { get; set; }
        public string CDN_Number { get; set; }
        public DateTime? GRN_Rls_Date { get; set; }
        public string Voucher_Type { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public string Vehicle_No { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Route_Id { get; set; }
        public string Route_Code { get; set; }
        public string Route { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_code { get; set; }
        public string Supplier_Name { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_code { get; set; }
        public string Customer_Name { get; set; }
        public string DC_Code { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<int> STN_DC_Id { get; set; }
        public string STN_DC_Code { get; set; }
        public string STN_DC_Name { get; set; }
        public int Counting { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public IEnumerable<PurchasePOEntity> Purchase { get; set; }
        public IEnumerable<GRN_Qty_SumEntity> GRN_Qty_Sum { get; set; }
        public IEnumerable<GrnLineItemsEntity> GrnDetails { get; set; }

     
    }
    public class POBULKAPPROVAL
    {
        //public int statusflag { get; set; }
        public string id { get; set; }
        public List<approvalUpdateList> Line_Ids { get; set; }
    }

    public class approvalUpdateList
    {

        public int po_LineId { get; set; }
        public int statusflag { get; set; }
    }
    public class GRN_Qty_SumEntity
    {
        public Nullable<double> Total_Qty_Sum { get; set; }
    }

    public class GrnLineItemsEntity
    {
        public IEnumerable<GRN_Qty_SumEntity> GRN_Qty_Sum { get; set; }
        public IEnumerable<GRNSKULineItemsEntity> GRNSKULineItems { get; set; }
        public IEnumerable<GRNSKULineItemsConsumablesEntity> GRNSKULineItemsConsumables { get; set; }
        public int GRN_Line_Id { get; set; }
        public Nullable<int> INW_id { get; set; }
        public string GRN_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
       // public string SKU_Type { get; set; }
        public Nullable<int> SKU_SubType_Id { get; set; }
        public string SKU_SubType { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Strinkage_Qty { get; set; }
        public Nullable<double> PO_QTY { get; set; }
        public Nullable<double> Billed_Qty { get; set; }
        public Nullable<int> Price_Book_Id { get; set; }
        public Nullable<double> Price { get; set; }
        public string Remark { get; set; }
        public string Tally_Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<double> B_Accepted_Qty { get; set; }
        public Nullable<double> A_Accepted_Qty { get; set; }
        public Nullable<double> C_Accepted_Qty { get; set; }
        public Nullable<double> A_Accepted_Price { get; set; }
        public Nullable<double> B_Accepted_Price { get; set; }
        public Nullable<double> C_Accepted_Price { get; set; }
        public Nullable<double> Total_Accepted_Qty { get; set; }
        public string Grade { get; set; }
        public Nullable<double> A_Converted_Qty { get; set; }
        public Nullable<double> B_Converted_Qty { get; set; }
        public Nullable<double> C_Converted_Qty { get; set; }
        public string STN_DC_Code { get; set; }
        public string STN_DC_Name { get; set; }
        public string PO_Number { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_code { get; set; }
        public string Customer_Name { get; set; }
        public string CDN_Number { get; set; }
        public DateTime? GRN_Rls_Date { get; set; }
        public string Voucher_Type { get; set; }
        public string SKU_Type { get; set; }
        public string Supplier_Name { get; set; }
        public string DC_Code { get; set; }
    }
    public class GRNSKULineItemsConsumablesEntity
    {
            public int GRN_Consumables_Id { get; set; }
            public Nullable<int> GRN_Line_Id_FK { get; set; }
            public Nullable<int> GRN_SKU_Line_Items_Id_FK { get; set; }
            public Nullable<int> SKU_Id { get; set; }
            public string Grade { get; set; }
            public Nullable<double> Consumable_Qty { get; set; }
            public string UOM { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<bool> is_Deleted { get; set; }
      
    }

    public class GRNSKULineItemsEntity
    {
        public int GRN_SKU_Line_Items_Id { get; set; }
        public Nullable<int> GRN_Line_Id { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public Nullable<double> A_Qty { get; set; }
        public Nullable<double> B_Qty { get; set; }
        public Nullable<double> C_Qty { get; set; }
        public string Barcode { get; set; }
        public string Batch_Number { get; set; }
        public string UOM { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
    }
}
