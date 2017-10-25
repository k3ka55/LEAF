using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class StockEntity
    {
        public int Stock_Id { get; set; }
        public string Stock_code { get; set; }
        //public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public string DC_Name { get; set; }
        //public Nullable<int> SKU_Id { get; set; }
        //public Nullable<int> Supplier_Id { get; set; }
        //public string Supplier_Name { get; set; }
        //public string Supplier_Code { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        //public Nullable<int> SKU_SubType_Id { get; set; }
        //public string SKU_SubType { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        //public Nullable<int> Pack_Type_Id { get; set; }
        //public string Pack_Size { get; set; }
        //public Nullable<int> Pack_Weight_Type_Id { get; set; }
        //public string Pack_Weight_Type { get; set; }
        //public string Pack_Type { get; set; }
        //public double? Dispatch { get; set; }
        //public double? Wastage { get; set; }
        public Nullable<double> Closing_Qty { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public string Type { get;set;}
        //public Nullable<System.DateTime> Closing_Date_Time { get; set; }
        //public string Aging { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }

        public List<StockEntity> convertedStocks { get; set; }
    }

    public class StockNumGenerationEntity
    {
        public int Stock_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Stock_Last_Number { get; set; }

    }

    public class StockConvertionEntity
    {
        public int Stock_Convertion_Id { get; set; }
        public int Stock_Id { get; set; }
        public string Stock_Code { get; set; }
        public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Name { get; set; }
        public string SKU_Type { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Stock_Qty { get; set; }
        public string Convert_From_Stock_Code { get; set; }
        public Nullable<bool> Is_Syunc { get; set; }
        public string UOM { get; set; }
        public string Type { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }

        public List<StockConvertedEntity> ConvertedList { get; set; }
    }

    public class StockConvertedEntity
    {
        public int Stock_Convertion_Id { get; set; }
        public int Stock_Id { get; set; }
        public string Stock_Code { get; set; }
        public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Name { get; set; }
        public string SKU_Type { get; set; }
        public string Grade { get; set; }
        public Nullable<double> Stock_Qty { get; set; }
        public string Convert_From_Stock_Code { get; set; }
        public string UOM { get; set; }
        public string Type { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
    }
}
