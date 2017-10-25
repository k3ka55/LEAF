using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class INVCashEntity
    {
       
        //public string DC_Code { get; set; }
        //public string DC_Name { get; set; }
        //public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string INVOICE_Number { get; set; }
        public Nullable<double> INVOICE_Amount { get; set; }
        //public Nullable<double> Collected_Amount { get; set; }
        public Nullable<double> OutStanding_Amount { get; set; }
        public Nullable<System.DateTime> INVOICE_Date { get; set; }
     
    }
    public class ArrayInvoiceCollectionEntity
    {
        public List<InvoiceCollectionEntity> INVCollection { get; set; }
           public string Uploaded_Excel_Display_Name { get; set; }
            public string Uploaded_Excel_Name { get; set; }
            public string UploadedBy { get; set; }
       
    }
 
    public class InvoiceCollectionEntity
    {
        public int INV_Collection_Id { get; set; }
        public string INV_Collection_Number { get; set; }
        public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public string DC_Name { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string INVOICE_Number { get; set; }
        public Nullable<double> INVOICE_Amount { get; set; }
        public Nullable<double> Collected_Amount { get; set; }
        public Nullable<double> OutStanding_Amount { get; set; }
        public Nullable<System.DateTime> INVOICE_Date { get; set; }
        public Nullable<int> INV_Collection_Excel_Tracking_id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
    }
    public class Invoice_Collection_Excel_TrackingEntity
    {
        public int INV_Collection_Excel_Tracking_id { get; set; }
        public Nullable<int> No_of_Items { get; set; }
        public string Uploaded_Excel_Display_Name { get; set; }
        public string Uploaded_Excel_Name { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public string UploadedBy { get; set; }
    }
    public class Invoice_Collection_Num_GenEntity
    {
        public int INV_Collection_Num_Gen_Id { get; set; }
        public string DC_Code { get; set; }
        public string Financial_Year { get; set; }
        public Nullable<int> INV_Collection_Last_Number { get; set; }
    }
    public class Invoice_Collection_TrackingEntity
    {
        public int INV_Collection_Tracking_Id { get; set; }
        public string INV_Collection_Number { get; set; }
        public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public string DC_Name { get; set; }
        public Nullable<int> Sales_Person_Id { get; set; }
        public string Sales_Person_Name { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string INVOICE_Number { get; set; }
        public Nullable<double> INVOICE_Amount { get; set; }
        public Nullable<double> Collected_Amount { get; set; }
        public Nullable<double> OutStanding_Amount { get; set; }
        public Nullable<System.DateTime> INVOICE_Date { get; set; }
        public string Action { get; set; }
        public string Action_DoneBy { get; set; }
        public Nullable<System.DateTime> Date_Of_Action { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
    }
}
