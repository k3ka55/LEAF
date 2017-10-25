using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class StockTransferEntity
    {
        public int ST_Tran_ID { get; set; }
        public string Stock_Code { get; set; }
        public Nullable<int> INW_id { get; set; }
        public string GRN_Number { get; set; }
        public Nullable<int> GRN_Line_Id { get; set; }
        //public Nullable<int> SKU_Id { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        //public Nullable<int> Supplier_ID { get; set; }
        public string Supplier { get; set; }
        public Nullable<double> Qty { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }

    public class StockNumGen
    {
        public int Stock_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> Stock_Last_Number { get; set; }
    }
}
