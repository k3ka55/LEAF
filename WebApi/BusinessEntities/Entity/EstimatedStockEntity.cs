﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entity
{
    //public class StockFromPhysicalStockEntity
    //{
    //    public List<PhysicalEntity> StrinkageStock { get; set; }
    //    public string DC_Code { get; set; }
    //    public string CreateBy { get; set; }
        
    //}

    //

    public class EstimtedStkEntity
    {
        public int Est_Stock_Id { get; set; }
        public string Est_Stock_code { get; set; }
        public Nullable<int> DC_id { get; set; }
        public string DC_Code { get; set; }
        public string DC_Name { get; set; }
        public Nullable<int> SKU_Id { get; set; }
        public Nullable<int> Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Code { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public Nullable<int> SKU_Type_Id { get; set; }
        public string SKU_Type { get; set; }
        public Nullable<int> Pack_Type_Id { get; set; }
        public string Pack_Size { get; set; }
        public Nullable<int> Pack_Weight_Type_Id { get; set; }
        public string Pack_Weight_Type { get; set; }
        public string Pack_Type { get; set; }
        public Nullable<double> System_Stock { get; set; }
        public Nullable<double> Closing_Qty { get; set; }
        public Nullable<double> Shrinkage_Qty { get; set; }
        public string UOM { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> Closing_Date_Time { get; set; }
        public string Aging { get; set; }
        public string Floor_Supervisor { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public Nullable<bool> is_Syunc { get; set; }
        public Nullable<bool> is_Deleted { get; set; }
        public Nullable<bool> is_Create { get; set; }
        public Nullable<bool> is_Edit { get; set; }
        public Nullable<bool> is_View { get; set; }
        public Nullable<bool> is_Delete { get; set; }
        public List<EstimatedStockEntity> PhyStock { get; set; }
    }
    public class fileImportEST
    {      
        public string DC_Code { get; set; }
        //public string SKUType { get; set; }
        public string FileString { get; set; }
    }
  public class EstimatedStockEntity
  {
      public int Est_Stock_Id { get; set; }
      public string Est_Stock_code { get; set; }
      public List<EstimatedStockEntity> ValidDatas { get; set; }
      public List<EstimatedStockEntity> InvalidDatas { get; set; }
      public List<int> InvalidLineNumbers { get; set; }
      public int LineNumber { get; set; }
      public bool status { get; set; }
      public int lineNumber { get; set; }
      public string Message { get; set; }
      public string errorItem { get; set; }
      public Nullable<int> DC_id { get; set; }
      public string DC_Code { get; set; }
      public string DC_Name { get; set; }
      public Nullable<int> SKU_Id { get; set; }
      public Nullable<int> Supplier_Id { get; set; }
      public string Supplier_Name { get; set; }
      public string Supplier_Code { get; set; }
      public string SKU_Code { get; set; }
      public string SKU_Name { get; set; }
      public Nullable<int> SKU_Type_Id { get; set; }
      public string SKU_Type { get; set; }
      public Nullable<int> Pack_Type_Id { get; set; }
      public string Pack_Size { get; set; }
      public Nullable<int> Pack_Weight_Type_Id { get; set; }
      public string Pack_Weight_Type { get; set; }
      public string Pack_Type { get; set; }
      public Nullable<double> Closing_Qty { get; set; }
      public Nullable<double> Price { get; set; }
      public string UOM { get; set; }
      public string Grade { get; set; }
      public Nullable<System.DateTime> Closing_Date_Time { get; set; }
      public string Aging { get; set; }
      public string Floor_Supervisor { get; set; }
      public Nullable<System.DateTime> CreatedDate { get; set; }
      public Nullable<System.DateTime> UpdateDate { get; set; }
      public string CreateBy { get; set; }
      public string UpdateBy { get; set; }
      public Nullable<bool> is_Deleted { get; set; }

      public int? Menu_Id { get; set; }
      public string Menu_Name { get; set; }
      public int is_Create { get; set; }
      public int is_Edit { get; set; }
      public int is_Approval { get; set; }
      public int is_View { get; set; }
      public int is_Delete { get; set; }
  }
  public class EstimatedStockEntitySearch
  {
      public int Est_Stock_Id { get; set; }
      public string Est_Stock_code { get; set; }
      public string DC_Name { get; set; }
      public string SKU_Name { get; set; }
      public Nullable<int> SKU_Type_Id { get; set; }
      public string SKU_Type { get; set; }
      public Nullable<double> Closing_Qty { get; set; }
      public string UOM { get; set; }
      public string Grade { get; set; }
     public Nullable<System.DateTime> CreatedDate { get; set; }
      public string CreateBy { get; set; }
      public int is_Create { get; set; }
      public int is_Edit { get; set; }
      public int is_Approval { get; set; }
      public int is_View { get; set; }
      public int is_Delete { get; set; }
  }
}
