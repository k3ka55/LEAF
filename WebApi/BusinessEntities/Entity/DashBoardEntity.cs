using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DashBoardEntity
    {

        public IEnumerable<DCDASHBOARD_STOCKEntity> DCDASHBOARD_STOCK { get; set; }
        public IEnumerable<DCDASHBOARD_POEntity> DCDASHBOARD_PO { get; set; }
        public IEnumerable<DCDASHBOARD_GRNEntity> DCDASHBOARD_GRN { get; set; }
        public IEnumerable<DCDASHBOARD_STIEntity> DCDASHBOARD_STI { get; set; }
        public IEnumerable<DCDASHBOARD_CSISTIADDEntity> DCDASHBOARD_CSISTIADD { get; set; }
        public IEnumerable<DCDASHBOARD_WASTAGEEntity> DCDASHBOARD_WASTAGE { get; set; }
        public IEnumerable<DCDASHBOARD_DISPATCHEntity> DCDASHBOARD_DISPATCH { get; set; }
        public IEnumerable<DCDASHBOARD_TOP5CUSTOMEREntity> DCDASHBOARD_TOP5CUSTOMER { get; set; }
        public IEnumerable<DCDASHBOARD_TOP5SKUEntity> DCDASHBOARD_TOP5SKU { get; set; }
        public IEnumerable<DCDASHBOARD_CSI2CUSTEntity> DCDASHBOARD_CSI2CUST { get; set; }
        public IEnumerable<DCDASHBOARD_CSI2CUSTEntity> DCDASHBOARD_CSI { get; set; }
        public IEnumerable<DCDASHBOARD_CSISTIFORGRAPHEntity> DCDASHBOARD_CSISTIFORGRAPH { get; set; } 
        public HttpStatusCode StatusCode { get; set; }
    }
    public class DCDASHBOARD_STOCKEntity
    {
        public int? No_of_SKUs { get; set; }
        public string DC_Code { get; set; }
        public double? Closing_Qty { get; set; }
      }

    public class DCDASHBOARD_POEntity
    {
        public int? No_of_PO { get; set; }
        public int? No_of_Supplier_name { get; set; }
        public string DC_Code { get; set; }
        public double? TOTAL_Qty { get; set; }
    }

    public class DCDASHBOARD_GRNEntity
    {
        public int? No_of_SKU { get; set; }
        public int? No_of_Supplier_name { get; set; }
        public string DC_Code { get; set; }
        public double? TOTAL_Qty { get; set; }
    }
    public class DCDASHBOARD_STIEntity
    {
        public string DC_Code { get; set; }
        public Nullable<int> STI_Number { get; set; }
        public Nullable<double> Qty { get; set; }
    }
    public class DCDASHBOARD_WASTAGEEntity
    {
        public string DC_Code { get; set; }
        public Nullable<int> No_of_SKUs { get; set; }
        public Nullable<double> Wastage_Qty { get; set; }
    }
    public class DCDASHBOARD_DISPATCHEntity
    {
        public string DC_Code { get; set; }
        public Nullable<int> Customer_Dispatch_Number { get; set; }
        public Nullable<double> TOTAL_QTY { get; set; }
    }
    public class DCDASHBOARD_CSIEntity
    {
        public string DC_Code { get; set; }
        public Nullable<int> Customer_Dispatch_Number { get; set; }
        public Nullable<double> TOTAL_QTY { get; set; }
    }
    public class DCDASHBOARD_CSISTIADDEntity
    {
        public string Indent_Raised_by_DC_Code { get; set; }
        public Nullable<int> Total_Orders { get; set; }
        public Nullable<double> Total_Qty { get; set; }
    }
     public class DCDASHBOARD_TOP5CUSTOMEREntity
    {
        public string DC_Code { get; set; }
        public Nullable<int> Customer_Order { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<double> Cust_Orderpercent { get; set; }
    }

     public partial class DCDASHBOARD_CSI2_CUST_PERCENTBASEEntity
     {
         public string DC_Code { get; set; }
         public Nullable<int> Sku_Count { get; set; }
         public Nullable<int> No_of_CSI_ORDER { get; set; }
         public Nullable<int> Customer_count { get; set; }
     }
     public class DCDASHBOARD_TOP5SKUEntity
     {
         public string DC_Code { get; set; }
         public string SKU_Name { get; set; }
         public Nullable<double> Closing_Qty { get; set; }

     }
     public class DCDASHBOARD_CSI2CUSTEntity
     {
         public string DC_Code { get; set; }
         public Nullable<int> No_of_SKU { get; set; }
         public Nullable<int> No_of_CSI_ORDER { get; set; }
         public Nullable<int> No_of_Customer { get; set; }
         public Nullable<double> No_of_SKUpercent { get; set; }
         public Nullable<double> No_of_CSI_ORDERpercent { get; set; }
         public Nullable<double> No_of_Customerpercent { get; set; }
     }
     public class DCDASHBOARD_CSISTIFORGRAPHEntity
     {
         public string DC_Code { get; set; }
         public double? Total_Orders { get; set; }
         public double? Total_Qty { get; set; }
         public double? Dispatched_orders { get; set; }
         public double? Dispatched_qty { get; set; }
         public Nullable<int> Created_year { get; set; }
         public Nullable<int> Created_Month { get; set; }

     }
 }
