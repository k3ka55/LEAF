using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
   public class CustomerSKU_MasterEntity
    {
        public int Customer_SKU_Master_Id { get; set; }
        public Nullable<int> Customer_SKU_Id { get; set; }
        public string Customer_SKU_Code { get; set; }
        public string Customer_SKU_Name { get; set; }
        public string UOM { get; set; }
        public string EAN_Number { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
