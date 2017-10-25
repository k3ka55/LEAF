using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class CustomerSKUMasterModelEntity
    {
        public int Customer_SKU_Mapping_Id { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }

        public int is_Create { get; set; }
        public int is_Delete { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public IEnumerable<CustomerSKUMasterLineItemModelEntity> LineItem { get; set; }
    }

    public class CustomerSKUMasterList
    {
        public List<CustomerSKUMasterLineItemModelEntity> CustomerSKUMaster { get; set; }
        public string FromScreen { get; set; }
        public string Customer_Code { get; set; }
    }
    public class CustomerSKUMasterLineItemModelEntity
    {
        public List<CustomerSKUMasterLineItemModelEntity> ValidDatas { get; set; }
        public List<CustomerSKUMasterLineItemModelEntity> InvalidDatas { get; set; }
        public Nullable<bool> ValidEdit { get; set; }
        public List<int> InvalidLineNumbers { get; set; }
        public int Customer_SKU_Mapping_Id { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string Customer_SKU_Name { get; set; }
        public string UOM { get; set; }
        public string EAN_Number { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool status { get; set; }
        public int lineNumber { get; set; }
        public string Message { get; set; }
        public string errorItem { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int LineNumber { get; set; }
       

    }

}
