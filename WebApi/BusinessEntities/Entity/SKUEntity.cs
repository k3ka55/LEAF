using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class SKUEntity
    {
        public int SKU_Id { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string SKU_Category { get; set; }
        public string Receiving_JDP { get; set; }
        public string UOM { get; set; }
        public string Chennai_Alias { get; set; }
        public string Coimbatore_Alias { get; set; }
        public string Ooty_Alias { get; set; }
        public string Bangalore_Alias { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> Total_GST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }

        public List<skuMappingEntity> skuMapping { get; set; }
    }

    public class skuMappingEntity
    {
        public int SKU_mapping__Id { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public Nullable<int> SKU_Main_Group_Id { get; set; }
        public Nullable<int> SKU_Sub_Group_Id { get; set; }
    }

    public class skuReturnMappingEntity
    {
        public int SKU_Main_Group_Id { get; set; }
        public string SKU_Main_Description { get; set; }
        public int SKU_Sub_Group_Id { get; set; }
        public string SKU_Sub_Description { get; set; }
    }
}