using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class SyncCheckingEntity
    {
        public int Sync_id { get; set; }
        public string Sync_Number { get; set; }
        public string DC_Code { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public string Updated_By { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public List<SyncCheckingLineItem> LineItems { get; set; }
    }

    public class SyncCheckingLineItem
    {
        public int SC_Line_Id { get; set; }
        public Nullable<int> Sync_id { get; set; }
        public string Sync_Number { get; set; }
        public Nullable<int> SKU_ID { get; set; }
        public string SKU_Name { get; set; }
        public string UOM { get; set; }
        public Nullable<double> Qty { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
