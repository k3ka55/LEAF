using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    public class Customer_CustomerIndetMap_Entity
    {
        public int CRT_Mapping_ID { get; set; }
        public List<CustomerDetailEntity> Cutomers { get; set; }
        //public Nullable<int> Customer_Id { get; set; }
        //public string Customer_Name { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Template_Name { get; set; }
        public string Template_Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public string Region_Code { get; set; }
        public string Region { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public string DC_Code { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string Location_Code { get; set; }
        //public Nullable<int> SKU_Type_Id { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public List<CustomerIndents> customerIndet { get; set; }
    }

    public class CustomerDetailEntity
    {
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
    }

    public class cust_Code
    {
        public string Customer_Code { get; set; }
    }

    public class searchcustMapping
    {
        public List<Customer_CustomerIndetMap_Entity> rateIndent { get; set; }
        public List<CustomerIndents> customerIndet { get; set; }
    }

    public class CustomerIndents
    {
        public int Indent_ID { get; set; }
        public string Indent_Name { get; set; }
        public string Indent_Code { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> DC_Id { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
    }
}
