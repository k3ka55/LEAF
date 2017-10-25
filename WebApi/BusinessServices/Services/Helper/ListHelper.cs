using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;


public class ListHelper
{
    public static IEnumerable<CustSupplierDDModel> CustCategory()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Modern Retail"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="General Trade"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="Distributor"},
              new CustSupplierDDModel { Val_Id=4, Val_Name="Exporter"},
              new CustSupplierDDModel { Val_Id=5, Val_Name="HORECA"},
              new CustSupplierDDModel { Val_Id=6, Val_Name="Market Sale"},
              new CustSupplierDDModel { Val_Id=7, Val_Name="Wholesaler"},
              new CustSupplierDDModel { Val_Id=8, Val_Name="DC Sale"}
            
        };
    }
    public static IEnumerable<Template_Type> Template_Type()
    {
        return new Template_Type[]{
            new Template_Type {Template_Type_Id=1, Template_Type_Name="Numeric" },
            new Template_Type {Template_Type_Id=2, Template_Type_Name="AlphaNumeric"},
           
        };
    }
    public static IEnumerable<CustSupplierDDModel> CustStoreType()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Small Format"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="Medium Format"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="Hyper Market"},
              new CustSupplierDDModel { Val_Id=4, Val_Name="Others"}
        };
    }

    public static IEnumerable<CustSupplierDDModel> CustDeliveryDays()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Sunday"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="Monday"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="Tuesday"},
              new CustSupplierDDModel { Val_Id=4, Val_Name="Wednesday"},
              new CustSupplierDDModel { Val_Id=5, Val_Name="Thursday"},
              new CustSupplierDDModel { Val_Id=6, Val_Name="Friday"},
                new CustSupplierDDModel { Val_Id=7, Val_Name="Saturday"},
              new CustSupplierDDModel { Val_Id=8, Val_Name="All Days"}
             
            
        };
    }
    public static IEnumerable<CustSupplierDDModel> CustDeliveryType()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="DC"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="DSD"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="Mixed"}            
        };
    }

    public static IEnumerable<CustSupplierDDModel> CustGRNRecvSchedule()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Same Day"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="Following Day"}   
        };
    }


    public static IEnumerable<CustSupplierDDModel> CustGRNRecvType()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Electronic (email)"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="Hard Copy"}         
            
        };
    }

    public static IEnumerable<CustSupplierDDModel> CustCustomerreturnPolicy()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="No return"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="on case 2 case basis"}            
            
        };
    }

    public static IEnumerable<CustSupplierDDModel> CustPricingChangeSchedule()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Daily"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="Weekly"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="Others"}            
        };
    }
    public static IEnumerable<CustSupplierDDModel> EngagementType()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Premium"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="Weekly"}
                      };
    }
    public static IEnumerable<CustSupplierDDModel> CustIndentType()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="D-4"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="D-3"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="D-2"},
              new CustSupplierDDModel { Val_Id=4, Val_Name="D-1"},
        };
    }


    public static IEnumerable<CustSupplierDDModel> PaymentType()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="COD"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="CHEQUE"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="Account Transfer"}            
        };
    }
    public static IEnumerable<CustSupplierDDModel> CreditPeriod()
    {
        return new CustSupplierDDModel[]{
              new CustSupplierDDModel { Val_Id=1, Val_Name="Nil"},
              new CustSupplierDDModel { Val_Id=2, Val_Name="7 Days"},
              new CustSupplierDDModel { Val_Id=3, Val_Name="14 days"},
              new CustSupplierDDModel { Val_Id=4, Val_Name="30 days"},      
              new CustSupplierDDModel { Val_Id=5, Val_Name="Other"}      
        };
    }

    //------------------------
    public static IEnumerable<GradeTypeModel> GradeType()
    {
        return new GradeTypeModel[]{
              new GradeTypeModel { GradeType_Id=1, GradeType_Name="A"},
              new GradeTypeModel { GradeType_Id=2, GradeType_Name="B"},
              //new GradeTypeModel { GradeType_Id=3, GradeType_Name="C"}
        };
    }
    public static IEnumerable<WastageTypeModel> WastageType()
    {
        return new WastageTypeModel[]{
              new WastageTypeModel { WastageTypeModel_Id=1, WastageTypeModel_Name="Process"},
              new WastageTypeModel { WastageTypeModel_Id=2, WastageTypeModel_Name="Floor"},
             // new WastageTypeModel { WastageTypeModel_Id=3, WastageTypeModel_Name="Customer Return"},
        };
    }

    public static IEnumerable<MaterialResourceModel> MaterialResource()
    {
        return new MaterialResourceModel[]{
              new MaterialResourceModel { Material_Resource_Id=1, Material_Resource_Name="Farmer"},
              new MaterialResourceModel { Material_Resource_Id=2, Material_Resource_Name="MarketPurchase"}
        };
    }
    public static IEnumerable<DataType> DataType()
    {
        return new DataType[]{
              new DataType { DataType_Id=1, DataType_Name="Text"},
              new DataType { DataType_Id=2, DataType_Name="Number"},
              new DataType { DataType_Id=3, DataType_Name="Date"}
        };
    }

    public static IEnumerable<CSIDelivery_Cycle> CSIDelivery_Cycle()
    {
        return new CSIDelivery_Cycle[]{
              new CSIDelivery_Cycle { CSIDelivery_Cycle_Id=1, CSIDelivery_Cycle_Value="D-1"},
              new CSIDelivery_Cycle { CSIDelivery_Cycle_Id=2, CSIDelivery_Cycle_Value="D-2"},
              new CSIDelivery_Cycle { CSIDelivery_Cycle_Id=3, CSIDelivery_Cycle_Value="D-3"},
              new CSIDelivery_Cycle { CSIDelivery_Cycle_Id=4, CSIDelivery_Cycle_Value="D"}
        };
    }
    public static IEnumerable<Delivery_Type> DeliveryType()
    {
        return new Delivery_Type[]{
              new Delivery_Type { Delivery_Type_Id=1, Delivery_Type_Value="Store Delivery"},
              new Delivery_Type { Delivery_Type_Id=2, Delivery_Type_Value="DC Delivery"}
        };
    }
    public static IEnumerable<StatusModel> GetStatus()
    {
        return new StatusModel[]{
              new StatusModel { StatusModel_Id=1, StatusModel_Name="Open"},
              new StatusModel { StatusModel_Id=2, StatusModel_Name="Closed"}
        };
    }
    public static IEnumerable<StatusLineItemModel> GetLineItemStatus()
    {
        return new StatusLineItemModel[]{
              new StatusLineItemModel { StatusLineItemModel_Id=1, StatusLineItemModel_Name="Open"},
              new StatusLineItemModel { StatusLineItemModel_Id=2, StatusLineItemModel_Name="Closed"},
              new StatusLineItemModel { StatusLineItemModel_Id=3, StatusLineItemModel_Name="Partial"}
        };
    }

    public static IEnumerable<Pack_Type> Pack_Type()
    {
        return new Pack_Type[]{
              new Pack_Type {Pack_Type_Id=1, Pack_Type_Name="Branded Retail"},
              new Pack_Type {Pack_Type_Id=2, Pack_Type_Name="Branded Bulk"},
              new Pack_Type {Pack_Type_Id=3, Pack_Type_Name="Un Branded Bulk"},
              new Pack_Type {Pack_Type_Id=4, Pack_Type_Name="Un Branded Retail"},
              new Pack_Type {Pack_Type_Id=5, Pack_Type_Name="Cut Retail"},
              new Pack_Type {Pack_Type_Id=6, Pack_Type_Name="Cut Branded"},
              new Pack_Type {Pack_Type_Id=7, Pack_Type_Name="Cut Bulk"},
             };
    }

    public static IEnumerable<Dispatch_Type> Dispatch_Type()
    {
        return new Dispatch_Type[]{
            new Dispatch_Type {Dispatch_Type_Id=1, Dispatch_Type_Name="Customer"},
            new Dispatch_Type {Dispatch_Type_Id=2, Dispatch_Type_Name="DC Stock Transfer"},
            };
    }
    public static IEnumerable<User_Login_Type> User_Login_Type()
    {
        return new User_Login_Type[]{
            new User_Login_Type {User_Login_Type_Id=1, User_Login_Type_Name="DC"},
            new User_Login_Type {User_Login_Type_Id=2, User_Login_Type_Name="LOCATION"}
        };
    }


    public static IEnumerable<SKU_Type> SKU_Type()
    {
        return new SKU_Type[]{
            new SKU_Type {SKU_Type_Id=1, SKU_Type_Name="Organic", SKU_Type_Code = "Org"},
            new SKU_Type {SKU_Type_Id=2, SKU_Type_Name="Standard", SKU_Type_Code = "Std"}
        };
    }

    public static IEnumerable<Customer_Category> Customer_Category()
    {
        return new Customer_Category[]{
            new Customer_Category {Category_Id=1, Category="Premium-A", Category_Code = "Prem-A"},
            new Customer_Category {Category_Id=2, Category="Premium-B", Category_Code = "Prem-B"},
            new Customer_Category {Category_Id=3, Category="Premium-C", Category_Code = "Prem-C"},
            new Customer_Category {Category_Id=4, Category="Normal-A", Category_Code = "Norm-A"},
            new Customer_Category {Category_Id=5, Category="Normal-B", Category_Code = "Norm-B"},
            new Customer_Category {Category_Id=6, Category="Normal-C", Category_Code = "Norm-C"},
            new Customer_Category {Category_Id=7, Category="Distributor-A", Category_Code = "Dist-A"},
            new Customer_Category {Category_Id=8, Category="Distributor-B", Category_Code = "Dist-B"},
            new Customer_Category {Category_Id=9, Category="Distributor-C", Category_Code = "Dist-C"},
        };
    }
    //
    public static IEnumerable<Tally_Module> Tally_Module()
    {
        return new Tally_Module[]{
            new Tally_Module {Tally_Module_Id=1, Tally_Module_Name="GRN_PO", Tally_Module_Activity = "Stock Inward"},
            new Tally_Module {Tally_Module_Id=2, Tally_Module_Name="GRN_STN", Tally_Module_Activity = "Stock Inward"},
            new Tally_Module {Tally_Module_Id=3, Tally_Module_Name="GRN_CDN", Tally_Module_Activity = "Stock Inward"},
            //new Tally_Module {Tally_Module_Id=4, Tally_Module_Name="WSTG_PROCESS", Tally_Module_Activity = "Stock Outward"},
            //new Tally_Module {Tally_Module_Id=5, Tally_Module_Name="WSTG_FLOOR", Tally_Module_Activity = "Stock Outward"},
            new Tally_Module {Tally_Module_Id=6, Tally_Module_Name="STN", Tally_Module_Activity = "Stock Outward"},
            new Tally_Module {Tally_Module_Id=7, Tally_Module_Name="CDN", Tally_Module_Activity = "Stock Outward"},
               //new Tally_Module {Tally_Module_Id=8, Tally_Module_Name="STRINKAGE_STN", Tally_Module_Activity = "Stock Outward"},
               //   new Tally_Module {Tally_Module_Id=9, Tally_Module_Name="STRINKAGE_CDN", Tally_Module_Activity = "Stock Outward"},
        };
    }
    //
    public static IEnumerable<Tally_Activity> Tally_Activity()
    {
        return new Tally_Activity[]{
            new Tally_Activity {Tally_Activity_Id=1, Tally_Activity_Name="Stock Inward" },
            new Tally_Activity {Tally_Activity_Id=2, Tally_Activity_Name="Stock Outward"},
           
        };
    }


    public static IEnumerable<SKU_SubType> SKU_SubType()
    {
        return new SKU_SubType[]{
            new SKU_SubType {SKU_SubType_Id=1, SKU_SubType_Name="Washed"},
            new SKU_SubType {SKU_SubType_Id=2, SKU_SubType_Name="UnWashed"}
        };
    }

    public static IEnumerable<Invoice_Type> Invoice_Type()
    {
        return new Invoice_Type[]{
            new Invoice_Type{Invoice_Type_Id=1, Invoice_Type_Name="Agent"},
            new Invoice_Type{Invoice_Type_Id=2, Invoice_Type_Name="Direct Customer"}
        };
    }

    public static IEnumerable<SKU_Category> SKU_Category()
    {
        return new SKU_Category[]{
            new SKU_Category{SKU_Category_Id=1, SKU_Category_Name="Exotic"},
            new SKU_Category{SKU_Category_Id=2, SKU_Category_Name="Fruit"},
            new SKU_Category{SKU_Category_Id=3, SKU_Category_Name="Herb"},
            new SKU_Category{SKU_Category_Id=4, SKU_Category_Name="Leafy"},
            new SKU_Category{SKU_Category_Id=5, SKU_Category_Name="Temperate"},
            new SKU_Category{SKU_Category_Id=6, SKU_Category_Name="Tropical"}
        };
    }

    //public static IEnumerable<Pack_Type> Pack_Type()
    //{
    //    return new Pack_Type[]{
    //        new Pack_Type{Pack_Type_Id=1,Pack_Type_Name="Cartons"},
    //        new Pack_Type{Pack_Type_Id=2,Pack_Type_Name="Crates"},
    //        new Pack_Type{Pack_Type_Id=3,Pack_Type_Name="Gunny Bags"},
    //        new Pack_Type{Pack_Type_Id=4,Pack_Type_Name="PP Bags"}
    //    };
    //}
    //public static IEnumerable<STI_Pack_Type> STIPack_Type()
    //{
    //    return new STI_Pack_Type[]{
    //          new STI_Pack_Type {STI_Pack_Type_Id=1, STI_Pack_Type_Name="MAP"},
    //          new STI_Pack_Type {STI_Pack_Type_Id=2, STI_Pack_Type_Name="Cling wrap"},
    //          new STI_Pack_Type {STI_Pack_Type_Id=3, STI_Pack_Type_Name="Punnet"},
    //          new STI_Pack_Type {STI_Pack_Type_Id=4, STI_Pack_Type_Name="Tray"},
    //          new STI_Pack_Type {STI_Pack_Type_Id=5, STI_Pack_Type_Name="Bunch"},
    //          new STI_Pack_Type {STI_Pack_Type_Id=6, STI_Pack_Type_Name="Loose"}
    //      };
    //}

    public static IEnumerable<Pack_Weight_Type> Pack_Weight_Type()
    {
        return new Pack_Weight_Type[]{
            new Pack_Weight_Type{Pack_Weight_Type_Id=1,Pack_Weight_Type_Name="Variable weight (VAR)"},
            new Pack_Weight_Type{Pack_Weight_Type_Id=2,Pack_Weight_Type_Name="Fixed Weight (FXD)"}    
        };
    }
    public static IEnumerable<Vehicle_No> Vehicle_No()
    {
        return new Vehicle_No[]{
            new Vehicle_No{Vehicle_No_Id=1,Vehicle_No_Name="KA 51 AA 2588",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=2,Vehicle_No_Name="TN 43 S 5149",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=3,Vehicle_No_Name="TN 40 J 0635",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=4,Vehicle_No_Name="TN 43 F 1557",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=5,Vehicle_No_Name="TN 43 F 1554",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=6,Vehicle_No_Name="TN 43 H 3595",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=7,Vehicle_No_Name="TN 43 Y 5022",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=8,Vehicle_No_Name="TN 43 F 5485",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=9,Vehicle_No_Name="TN 43 H 5149",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=10,Vehicle_No_Name="TN 43 F 5479",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=11,Vehicle_No_Name="TN 43 F 1545",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=12,Vehicle_No_Name="TN 43 E 1462",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=13,Vehicle_No_Name="TN 43 E 8193",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=14,Vehicle_No_Name="TN 40 J 0616",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=15,Vehicle_No_Name="TN 43 E 3864",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=16,Vehicle_No_Name="TN 40 J 0626",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=17,Vehicle_No_Name="TN 43 H 4737",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=18,Vehicle_No_Name="TN 43 E 0846",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=19,Vehicle_No_Name="TN 43 H 4732",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=20,Vehicle_No_Name="TN 43 E 0234",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=21,Vehicle_No_Name="TN 43 H 4372",Vehicle_DC_Code="JDM",Vehicle_Type=""},
            new Vehicle_No{Vehicle_No_Id=22,Vehicle_No_Name="TN 43 E 3778",Vehicle_DC_Code="JDM",Vehicle_Type=""}
        };
    }
    public static IEnumerable<Pack_Size> Pack_Size()
    {
        return new Pack_Size[]{
           new Pack_Size{Pack_Size_Id=1,Pack_Size_Value="50 gm"},
           new Pack_Size{Pack_Size_Id=2,Pack_Size_Value="100 gm"},  
           new Pack_Size{Pack_Size_Id=3,Pack_Size_Value="200 gm"}, 
           new Pack_Size{Pack_Size_Id=4,Pack_Size_Value="250 gm"}, 
           new Pack_Size{Pack_Size_Id=5,Pack_Size_Value="300 gm"}, 
           new Pack_Size{Pack_Size_Id=6,Pack_Size_Value="350 gm"}, 
           new Pack_Size{Pack_Size_Id=7,Pack_Size_Value="400 gm"}, 
           new Pack_Size{Pack_Size_Id=8,Pack_Size_Value="500 gm"}, 
           new Pack_Size{Pack_Size_Id=9,Pack_Size_Value="50 kg"}, 
           new Pack_Size{Pack_Size_Id=10,Pack_Size_Value="80 kg"}, 
           new Pack_Size{Pack_Size_Id=11,Pack_Size_Value="1 kg"}, 
           new Pack_Size{Pack_Size_Id=12,Pack_Size_Value="5 kg"}, 
           new Pack_Size{Pack_Size_Id=13,Pack_Size_Value="10 kg"}, 
           new Pack_Size{Pack_Size_Id=14,Pack_Size_Value="15 kg"}, 
           new Pack_Size{Pack_Size_Id=15,Pack_Size_Value="20 kg"},
           new Pack_Size{Pack_Size_Id=16,Pack_Size_Value="100 kg"},
           new Pack_Size{Pack_Size_Id=16,Pack_Size_Value="Loose"},
           new Pack_Size{Pack_Size_Id=17,Pack_Size_Value="600 gm"},
           new Pack_Size{Pack_Size_Id=18,Pack_Size_Value="700 gm"},
           new Pack_Size{Pack_Size_Id=19,Pack_Size_Value="800 gm"},
           new Pack_Size{Pack_Size_Id=20,Pack_Size_Value="1 pc"},
           new Pack_Size{Pack_Size_Id=21,Pack_Size_Value="2 pcs"},
           new Pack_Size{Pack_Size_Id=22,Pack_Size_Value="3 pcs"},
           new Pack_Size{Pack_Size_Id=23,Pack_Size_Value="4 pcs"},
           new Pack_Size{Pack_Size_Id=24,Pack_Size_Value="6 pcs"},
             //  new Pack_Size{Pack_Size_Id=19,Pack_Size_Value="wrapped"},
           //new Pack_Size{Pack_Size_Id=1,Pack_Size_Type="MAP",Pack_Size_Value="50 gm"},
           //new Pack_Size{Pack_Size_Id=2,Pack_Size_Type="MAP",Pack_Size_Value="100 gm"},  
           //new Pack_Size{Pack_Size_Id=3,Pack_Size_Type="MAP",Pack_Size_Value="200 gm"}, 
           //new Pack_Size{Pack_Size_Id=4,Pack_Size_Type="MAP",Pack_Size_Value="250 gm"}, 
           //new Pack_Size{Pack_Size_Id=5,Pack_Size_Type="MAP",Pack_Size_Value="300 gm"}, 
           //new Pack_Size{Pack_Size_Id=6,Pack_Size_Type="MAP",Pack_Size_Value="350 gm"}, 
           //new Pack_Size{Pack_Size_Id=7,Pack_Size_Type="MAP",Pack_Size_Value="400 gm"}, 
           //new Pack_Size{Pack_Size_Id=8,Pack_Size_Type="MAP",Pack_Size_Value="500 gm"}, 
           //new Pack_Size{Pack_Size_Id=9,Pack_Size_Type="MAP",Pack_Size_Value="50 kg"}, 
           //new Pack_Size{Pack_Size_Id=10,Pack_Size_Type="MAP",Pack_Size_Value="80 kg"}, 
           //new Pack_Size{Pack_Size_Id=11,Pack_Size_Type="MAP",Pack_Size_Value="1 kg"}, 
           //new Pack_Size{Pack_Size_Id=12,Pack_Size_Type="MAP",Pack_Size_Value="5 kg"}, 
           //new Pack_Size{Pack_Size_Id=13,Pack_Size_Type="MAP",Pack_Size_Value="10 kg"}, 
           //new Pack_Size{Pack_Size_Id=14,Pack_Size_Type="MAP",Pack_Size_Value="15 kg"}, 
           //new Pack_Size{Pack_Size_Id=15,Pack_Size_Type="MAP",Pack_Size_Value="20 kg"},
           //new Pack_Size{Pack_Size_Id=17,Pack_Size_Type="OTHERS",Pack_Size_Value="100 kg"}, 
           //new Pack_Size{Pack_Size_Id=16,Pack_Size_Type="OTHERS",Pack_Size_Value="50 gm"}, 
           
           //new Pack_Size{Pack_Size_Id=18,Pack_Size_Type="OTHERS",Pack_Size_Value="150 gm"}, 
           //new Pack_Size{Pack_Size_Id=19,Pack_Size_Type="OTHERS",Pack_Size_Value="200 gm"}, 
           //new Pack_Size{Pack_Size_Id=20,Pack_Size_Type="OTHERS",Pack_Size_Value="250 gm"}, 
           //new Pack_Size{Pack_Size_Id=21,Pack_Size_Type="OTHERS",Pack_Size_Value="300 gm"}, 
           //new Pack_Size{Pack_Size_Id=22,Pack_Size_Type="OTHERS",Pack_Size_Value="350 gm"}, 
           //new Pack_Size{Pack_Size_Id=23,Pack_Size_Type="OTHERS",Pack_Size_Value="400 gm"}, 
           //new Pack_Size{Pack_Size_Id=24,Pack_Size_Type="OTHERS",Pack_Size_Value="450 gm"}, 
           //new Pack_Size{Pack_Size_Id=25,Pack_Size_Type="OTHERS",Pack_Size_Value="500 gm"}
        };
    }
    //public static IEnumerable<Pack_Size> PackSize()
    //{
    //    return new Pack_Size[]{
    //       new Pack_Size{Pack_Size_Id=1,Pack_Size_Type="MAP",Pack_Size_Value="50 gm"},
    //       new Pack_Size{Pack_Size_Id=2,Pack_Size_Type="MAP",Pack_Size_Value="100 gm"},  
    //       new Pack_Size{Pack_Size_Id=3,Pack_Size_Type="MAP",Pack_Size_Value="200 gm"}, 
    //       new Pack_Size{Pack_Size_Id=4,Pack_Size_Type="MAP",Pack_Size_Value="250 gm"}, 
    //       new Pack_Size{Pack_Size_Id=5,Pack_Size_Type="MAP",Pack_Size_Value="300 gm"}, 
    //       new Pack_Size{Pack_Size_Id=6,Pack_Size_Type="MAP",Pack_Size_Value="350 gm"}, 
    //       new Pack_Size{Pack_Size_Id=7,Pack_Size_Type="MAP",Pack_Size_Value="400 gm"}, 
    //       new Pack_Size{Pack_Size_Id=8,Pack_Size_Type="MAP",Pack_Size_Value="500 gm"}, 
    //       new Pack_Size{Pack_Size_Id=9,Pack_Size_Type="MAP",Pack_Size_Value="750 gm"}, 
    //       new Pack_Size{Pack_Size_Id=10,Pack_Size_Type="MAP",Pack_Size_Value="800 gm"}, 
    //       new Pack_Size{Pack_Size_Id=11,Pack_Size_Type="MAP",Pack_Size_Value="1 kg"}, 
    //       new Pack_Size{Pack_Size_Id=12,Pack_Size_Type="MAP",Pack_Size_Value="5 kg"}, 
    //       new Pack_Size{Pack_Size_Id=13,Pack_Size_Type="MAP",Pack_Size_Value="10 kg"}, 
    //       new Pack_Size{Pack_Size_Id=14,Pack_Size_Type="MAP",Pack_Size_Value="15 kg"}, 
    //       new Pack_Size{Pack_Size_Id=15,Pack_Size_Type="MAP",Pack_Size_Value="20 kg"},
    //       new Pack_Size{Pack_Size_Id=16,Pack_Size_Type="OTHERS",Pack_Size_Value="50 gm"}, 
    //       new Pack_Size{Pack_Size_Id=17,Pack_Size_Type="OTHERS",Pack_Size_Value="100 gm"}, 
    //       new Pack_Size{Pack_Size_Id=18,Pack_Size_Type="OTHERS",Pack_Size_Value="150 gm"}, 
    //       new Pack_Size{Pack_Size_Id=19,Pack_Size_Type="OTHERS",Pack_Size_Value="200 gm"}, 
    //       new Pack_Size{Pack_Size_Id=20,Pack_Size_Type="OTHERS",Pack_Size_Value="250 gm"}, 
    //       new Pack_Size{Pack_Size_Id=21,Pack_Size_Type="OTHERS",Pack_Size_Value="300 gm"}, 
    //       new Pack_Size{Pack_Size_Id=22,Pack_Size_Type="OTHERS",Pack_Size_Value="350 gm"}, 
    //       new Pack_Size{Pack_Size_Id=23,Pack_Size_Type="OTHERS",Pack_Size_Value="400 gm"}, 
    //       new Pack_Size{Pack_Size_Id=24,Pack_Size_Type="OTHERS",Pack_Size_Value="450 gm"}, 
    //       new Pack_Size{Pack_Size_Id=25,Pack_Size_Type="OTHERS",Pack_Size_Value="500 gm"}
    //    };
    //}
    //public List<Pack_Size> PackSize = new List<Pack_Size>
    //{
    //       new Pack_Size{Pack_Size_Id=1,Pack_Size_Type="MAP",Pack_Size_Value="50 gm"},
    //       new Pack_Size{Pack_Size_Id=2,Pack_Size_Type="MAP",Pack_Size_Value="100 gm"},  
    //       new Pack_Size{Pack_Size_Id=3,Pack_Size_Type="MAP",Pack_Size_Value="200 gm"}, 
    //       new Pack_Size{Pack_Size_Id=4,Pack_Size_Type="MAP",Pack_Size_Value="250 gm"}, 
    //       new Pack_Size{Pack_Size_Id=5,Pack_Size_Type="MAP",Pack_Size_Value="300 gm"}, 
    //       new Pack_Size{Pack_Size_Id=6,Pack_Size_Type="MAP",Pack_Size_Value="350 gm"}, 
    //       new Pack_Size{Pack_Size_Id=7,Pack_Size_Type="MAP",Pack_Size_Value="400 gm"}, 
    //       new Pack_Size{Pack_Size_Id=8,Pack_Size_Type="MAP",Pack_Size_Value="500 gm"}, 
    //       new Pack_Size{Pack_Size_Id=9,Pack_Size_Type="MAP",Pack_Size_Value="750 gm"}, 
    //       new Pack_Size{Pack_Size_Id=10,Pack_Size_Type="MAP",Pack_Size_Value="800 gm"}, 
    //       new Pack_Size{Pack_Size_Id=11,Pack_Size_Type="MAP",Pack_Size_Value="1 kg"}, 
    //       new Pack_Size{Pack_Size_Id=12,Pack_Size_Type="MAP",Pack_Size_Value="5 kg"}, 
    //       new Pack_Size{Pack_Size_Id=13,Pack_Size_Type="MAP",Pack_Size_Value="10 kg"}, 
    //       new Pack_Size{Pack_Size_Id=14,Pack_Size_Type="MAP",Pack_Size_Value="15 kg"}, 
    //       new Pack_Size{Pack_Size_Id=15,Pack_Size_Type="MAP",Pack_Size_Value="20 kg"},
    //       new Pack_Size{Pack_Size_Id=16,Pack_Size_Type="OTHERS",Pack_Size_Value="50 gm"}, 
    //       new Pack_Size{Pack_Size_Id=17,Pack_Size_Type="OTHERS",Pack_Size_Value="100 gm"}, 
    //       new Pack_Size{Pack_Size_Id=18,Pack_Size_Type="OTHERS",Pack_Size_Value="150 gm"}, 
    //       new Pack_Size{Pack_Size_Id=19,Pack_Size_Type="OTHERS",Pack_Size_Value="200 gm"}, 
    //       new Pack_Size{Pack_Size_Id=20,Pack_Size_Type="OTHERS",Pack_Size_Value="250 gm"}, 
    //       new Pack_Size{Pack_Size_Id=21,Pack_Size_Type="OTHERS",Pack_Size_Value="300 gm"}, 
    //       new Pack_Size{Pack_Size_Id=22,Pack_Size_Type="OTHERS",Pack_Size_Value="350 gm"}, 
    //       new Pack_Size{Pack_Size_Id=23,Pack_Size_Type="OTHERS",Pack_Size_Value="400 gm"}, 
    //       new Pack_Size{Pack_Size_Id=24,Pack_Size_Type="OTHERS",Pack_Size_Value="450 gm"}, 
    //       new Pack_Size{Pack_Size_Id=25,Pack_Size_Type="OTHERS",Pack_Size_Value="500 gm"}
    //    };
}

