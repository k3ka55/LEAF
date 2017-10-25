using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class ProductEntity
    {
        public int Prod_Id { get; set; }
        public Nullable<double> SKU_Code { get; set; }
        public string SKU { get; set; }
        public string Receiving_JDP { get; set; }
        public string UOM { get; set; }
        public string Chennai_Alias { get; set; }
        public string Coimbatore_Alias { get; set; }
        public string Ooty_Alias { get; set; }
        public string Bangalore_Alias { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
