using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GRNReportEntity
    {
        public int INW_Id { get; set; }
        public string DC_Code { get; set; }
        public string Grade { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int IN_SKU_Count { get; set; }
        public Nullable<double> IN_TOTAL_QTY { get; set; }
        public int OUT_SKU_Count { get; set; }
        public Nullable<double> OUT_TOTAL_QTY { get; set; }
        public string SKU_Type { get; set; }
        public string UOM { get; set; }

    }
    public class ShrinkageReportEntity
    {
        public string SKU_Name { get; set; }
        public double GRN_CustReturnShrinkage { get; set; }
        public double CDN_ProcessShrinkage { get; set; }
        public double Stock_FloorShrinkage { get; set; }
    }
    public class GRNDDEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class CustFillRateEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }


    public class StockSummaryReportEntity
    {
        public string DC_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double A_GRADE { get; set; }
        public double B_GRADE { get; set; }
        public double TOTAL_GRADE_VALUE { get; set; }
        public Nullable<int> Number_of_SKU { get; set; }
    }

    public class WastageReportEntity
    {
        public string DC_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double C_QTY { get; set; }
        public double Floor { get; set; }
        public double Process { get; set; }
        public double Total_qty { get; set; }

    }
    public class PurchaseReportEntity
    {
        public string DC_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? Fill_Rate { get; set; }
        public double? Po_Qty { get; set; }
        public double? Recived_Qty { get; set; }
    }
    public class DispatchQTYReportEntity
    {
        public string Delivery_Location_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? A { get; set; }
        public double? B { get; set; }
        public double? No_SKU { get; set; }
        public double? Total_qty { get; set; }
    }
    public class STIDISPATCHReportEntity
    {
        public string Material_Source { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? Fill_Rate { get; set; }
        public double? Dispatch_Qty { get; set; }
        public double? Qty { get; set; }
    }
    public class DispatchPackTypeBasedReportEntity
    {
        public string Delivery_Location_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? Branded_Bulk { get; set; }
        public double? Branded_Retail { get; set; }
        public double? Total { get; set; }
        public double? Un_Branded_Bulk { get; set; }
        public double? Number_of_SKU { get; set; }
    }
    public class CustomerRateFillReportEntity
    {
          public string DC_Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int Modern_Retail { get; set; }
        public int General_Trade { get; set; }
        public int Distributor { get; set; }
        public int Exporter { get; set; }
        public int HORECA { get; set; }
        public int Market_Sale { get; set; }
        public int Wholesaler { get; set; }
        public int DC_Sale { get; set; }
        public string Type { get; set; }
    }
    
    public class CustomerRateFillMonthBasedReportEntity
    {
        public string DC_Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Category { get; set; }
        public Nullable<int> Current_Month_Total_Count { get; set; }
        public Nullable<int> Previous_Month_Total_Count { get; set; }
        public string Type { get; set; }
    }
    public class CSIEndReportEntity
    {
        public string DC_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? CDN_Qty { get; set; }
        public double? CSI_Qty { get; set; }
        public double? Customer_return_qty { get; set; }
        public double? Fill_Rate { get; set; }
        public double? Invoice_Qty { get; set; }
    }

     public class CSIReportEntity
    {
        public string DC_Code { get; set; }
        public string SKU_Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? CDN_Qty { get; set; }
        public double? CSI_Qty { get; set; }
        public double? Customer_return_qty { get; set; }
        public double? Fill_Rate { get; set; }
        public double? Invoice_Qty { get; set; }
    }


}
