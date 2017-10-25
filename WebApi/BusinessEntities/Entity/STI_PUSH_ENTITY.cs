using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class STI_PUSH_ENTITY
    {
        public static List<StockTransferIntentEntity> localStis = new List<StockTransferIntentEntity>();
        public static List<SyncCheckingEntity> SyncChecking = new List<SyncCheckingEntity>();

        public static List<DispatchEntity> stnRecordes = new List<DispatchEntity>();

        public static List<SaleIndentEntity> csiRecords = new List<SaleIndentEntity>();

        public static List<PurchaseOrderEntity> porecord = new List<PurchaseOrderEntity>();        

        public static List<RateIndentEntity> riRecords = new List<RateIndentEntity>();

        public static List<Material_MasterEntity> materialRecords = new List<Material_MasterEntity>();

        public static List<GrnEntity> grnrecord = new List<GrnEntity>();

        public static List<SupplierEntity> supplierRecord = new List<SupplierEntity>();

        public static List<WastageEntity> wastagerecord = new List<WastageEntity>();

        public static List<CIEntity> ciRecord = new List<CIEntity>();

        public static List<PhysicalStockEntity> physicalrecord = new List<PhysicalStockEntity>();

        public static List<InvoiceEntity> invoicerecord = new List<InvoiceEntity>();

        public static List<StockConvertionEntity> stockrecord = new List<StockConvertionEntity>();

        //------------------------------Auto Num Gen Classes-----------------------------------

        public static List<POAutoNumGen> PONumGenClass = new List<POAutoNumGen>();

        public static List<GRNNumGen> GRNNumGenClass = new List<GRNNumGen>();

        public static List<STINumGen> STINumGenClass = new List<STINumGen>();

        public static List<CDNNumGen> CDNNumGenClass = new List<CDNNumGen>();

        public static List<STNNumGen> STNNumGenClass = new List<STNNumGen>();

        public static List<WastageNumGen> WastageNumGenClass = new List<WastageNumGen>();

        public static List<InvoiceNumGen> InvoiceNumClass = new List<InvoiceNumGen>();

        public static List<CSINumGen> CSINumGenClass = new List<CSINumGen>();

        public static List<MaterialNumGen> MaterialNumGenClass = new List<MaterialNumGen>();

        public static List<RINumGen> RINumGenClass = new List<RINumGen>();

        public static List<CINumGen> CINumGenClass = new List<CINumGen>();

        public static List<StockNumGen> StockNumGenClass = new List<StockNumGen>();

    }

    public class POAutoNumGen
    {
        public int PO_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> PO_Last_Number { get; set; }
    }

    public class GRNNumGen
    {
        public int GRN_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> GRN_Last_Number { get; set; }
    }

    public class STINumGen
    {
        public int ST_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> ST_Last_Number { get; set; }
    }

    public class CDNNumGen
    {
        public int Dispatch_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Customer_Dispatch_Last_Number { get; set; }
    }

    public class STNNumGen
    {
        public int Dispatch_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Stock_Xfer_Dispatch_Last_Number { get; set; }
    }

    public class WastageNumGen
    {
        public int Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Wastage_Last_Number { get; set; }
    }

    public class InvoiceNumGen
    {
        public int Invoice_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Invoice_Last_Number { get; set; }
    }

    public class CSINumGen
    {
        public int CSI_Num_Gen_Id { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> CSI_Last_Number { get; set; }
    }

    public class MaterialNumGen
    {
        public int Material_Master_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Material_Master_Last_Number { get; set; }
    }

    public class RINumGen
    {
        public int Rate_Template_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Rate_Template_Last_Number { get; set; }
    }

    public class CINumGen
    {
        public int Customer_Indent_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Customer_Indent_Last_Number { get; set; }
    }
}
