using DataModel;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using BusinessEntities;
using System.Resources;
using AutoMapper;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using System.Reflection;
using System.Transactions;
using BusinessEntities.Entity;

namespace BusinessServices
{
    public class DropdownService : IDropdownService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
            private readonly UnitOfWork _unitOfWork;
            public DropdownService()
        {
            _unitOfWork = new UnitOfWork();
        }

            public List<Tuple<string>> getStatuses()
            {
                ResourceManager rm = new ResourceManager("BusinessServices.Status", Assembly.GetExecutingAssembly());
                List<Tuple<string>> list = new List<Tuple<string>>();
                list.Add(new Tuple<string>(rm.GetString("OpenStatus")));
                list.Add(new Tuple<string>(rm.GetString("CloseStatus")));
                return list;
            }

            //=========================================================VALIDATE FUNCTIONS===================================
            public string validateUserName(UserDetailsEntity valUsr)
            {
               
                var count = 0;
                if (valUsr.User_id > 0)
                {
                    count = DB.User_Details.Where(d => d.User_Name.ToLower() == valUsr.User_Name.ToLower() && d.User_id != valUsr.User_id).Count();
                }
                else
                {
                    count = DB.User_Details.Where(d => d.User_Name.ToLower() == valUsr.User_Name.ToLower()).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }

            public string validateRouteMaster(RouteMasterEntity valroute)
            {

                var count = 0;
                if (valroute.Target_Location_Code != null && valroute.Orgin_Location_Code != null)
                {
                    count = DB.Route_Master.Where(d => d.Target_Location_Code.ToLower() == valroute.Target_Location_Code.ToLower() && d.Orgin_Location_Code.ToLower() == valroute.Orgin_Location_Code.ToLower()).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }
            public string validateSKUName(SKUEntity valSku)
            {
                valSku.SKU_Name = valSku.SKU_Name.Replace("\t", String.Empty);
                var count = 0;
                if (valSku.SKU_Id > 0)
                {
                    count = DB.SKU_Master.Where(d => d.SKU_Name.ToLower() == valSku.SKU_Name.ToLower() && d.SKU_Id != valSku.SKU_Id).Count();
                }
                else
                {
                    count = DB.SKU_Master.Where(d => d.SKU_Name.ToLower() == valSku.SKU_Name.ToLower()).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }
            public string validateMaterialCode(Material_MasterEntity valma)
            {

                var count = 0;
                if (valma.Material_Id > 0)
                {
                    count = DB.Material_Master.Where(d => d.Material_Code.ToLower() == valma.Material_Code.ToLower() && d.Material_Id != valma.Material_Id && d.Is_Deleted == false).Count();
                }
                else
                {
                    count = DB.Material_Master.Where(d => d.Material_Code.ToLower() == valma.Material_Code.ToLower() && d.Is_Deleted == false).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }
            public string validateRoleName(RoleEntity valRl)
            {

                var count = 0;
                if (valRl.Role_Id > 0)
                {
                    count = DB.Role_Master.Where(d => d.Role_Name.ToLower() == valRl.Role_Name.ToLower() && d.Role_Id != valRl.Role_Id && d.Is_Delete == false).Count();
                }
                else
                {
                    count = DB.Role_Master.Where(d => d.Role_Name.ToLower() == valRl.Role_Name.ToLower() && d.Is_Delete == false).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }

            public string validateDCName(DCMasterEntity valDC)
            {

                var count = 0;
                if (valDC.Dc_Id > 0)
                {
                    count = DB.DC_Master.Where(d => d.Dc_Name.ToLower() == valDC.Dc_Name.ToLower() && d.Dc_Id != valDC.Dc_Id).Count();
                }
                else
                {
                    count = DB.DC_Master.Where(d => d.Dc_Name.ToLower() == valDC.Dc_Name.ToLower()).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }

            public string validateLocationName(LocationMasterEntity valLoc)
            {

                var count = 0;
                if (valLoc.Location_Id > 0)
                {
                    count = DB.Location_Master.Where(d => d.Location_Name.ToLower() == valLoc.Location_Name.ToLower() && d.Location_Id != valLoc.Location_Id).Count();
                }
                else
                {
                    count = DB.Location_Master.Where(d => d.Location_Name.ToLower() == valLoc.Location_Name.ToLower()).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }

            public string validateRegionName(RegionMasterEntity valReg)
            {

                var count = 0;
                if (valReg.Region_Id > 0)
                {
                    count = DB.Region_Master.Where(d => d.Region_Name.ToLower() == valReg.Region_Name.ToLower() && d.Region_Id != valReg.Region_Id).Count();
                }
                else
                {
                    count = DB.Region_Master.Where(d => d.Region_Name.ToLower() == valReg.Region_Name.ToLower()).Count();
                }

                if (count > 0)
                {
                    return "False";
                }
                else
                {
                    return "True";
                }


            }
           
            public IEnumerable<Tally_Activity> GetTallyDD()
            {                
                var output = ListHelper.Tally_Activity();
                return output;
            }
           
            public IEnumerable<Tally_Module> GetTallyModuleByName(string Tally_Module_Activity)
            {
                var output = ListHelper.Tally_Module().Where(a=>a.Tally_Module_Activity.ToLower()==Tally_Module_Activity.ToLower()).ToList();
                return output;
            }

            //===============================================================================================================
            [HttpGet]
            public CustSuppDDEntity GetCustSuppDD()
            {
                CustSuppDDEntity dropdown = new CustSuppDDEntity();
                dropdown.CustCategory = ListHelper.CustCategory();
                dropdown.CustStoreType = ListHelper.CustStoreType();
                dropdown.CustDeliveryDays = ListHelper.CustDeliveryDays();
                dropdown.CustDeliveryType = ListHelper.CustDeliveryType();
                dropdown.CustGRNRecvSchedule = ListHelper.CustGRNRecvSchedule();
                dropdown.CustGRNRecvType = ListHelper.CustGRNRecvType();
                dropdown.CustCustomerreturnPolicy = ListHelper.CustCustomerreturnPolicy();
                dropdown.CustPricingChangeSchedule = ListHelper.CustPricingChangeSchedule();
                //dropdown.CustRegion = (from b in DB.Region_Master
                //                       join a in DB.DC_Master on b.Region_Id equals a.Region_Id
                //                        select new RegionEntity
                //                       {
                //                           Region_Id = b.Region_Id,
                //                           Region_Code=b.Region_Code,
                //                           DC_Name=a.Dc_Name,
                //                         Region_Name=b.Region_Name
                //                       }).ToList();
                dropdown.CustRegion = (from b in DB.Region_Master
                                       orderby b.Region_Name
                                       // join a in DB.DC_Master on b.Region_Id equals a.Region_Id
                                       select new RegionEntity
                                       {
                                           Region_Id = b.Region_Id,
                                           Region_Code = b.Region_Code,
                                           // DC_Name = a.Dc_Name,
                                           Region_Name = b.Region_Name,
                                           //DC=(from a in DB.DC_Master
                                           //   where a.Region_Id==b.Region_Id
                                           //    select new DCMasterEntity
                                           //{
                                           //    Dc_Id=a.Dc_Id,
                                           //    DC_Code= a.DC_Code,
                                           //   Dc_Name = a.Dc_Name

                                           //}).ToList(), 

                                       }).ToList();

                dropdown.CustDCMaster = (from b in DB.DC_Master
                                         orderby b.Dc_Name
                                         select new DCMasterEntity
                                         {
                                             Dc_Id = b.Dc_Id,
                                             DC_Code = b.DC_Code,
                                             Dc_Name = b.Dc_Name

                                         }).ToList();

                dropdown.CustLocationMaster = (from b in DB.Location_Master
                                               orderby b.Location_Name
                                               select new LocationMasterEntity
                                               {
                                                   Location_Id = b.Location_Id,
                                                   Location_Code = b.Location_Code,
                                                   Location_Name = b.Location_Name

                                               }).ToList();

                dropdown.EngagementType = ListHelper.EngagementType();
                dropdown.CustIndentType = ListHelper.CustIndentType();
                dropdown.PaymentType = ListHelper.PaymentType();
                dropdown.CreditPeriod = ListHelper.CreditPeriod();




                return dropdown;

            }
            
            public CustRegDDEntity GetCustRegDD()
            {
                CustRegDDEntity dropdown = new CustRegDDEntity();
             

                dropdown.Locations = (from b in DB.Location_Master
                                               orderby b.Location_Name
                                               select new LocationModel
                                               {
                                                   Location_Id = b.Location_Id,
                                                   Location_Code = b.Location_Code,
                                                   Location_Name = b.Location_Name

                                               }).ToList();

                dropdown.CustRegCreatedBy = (from b in DB.Customer_Registration
                                             where b.Is_Approved!=true && b.Is_Delete!=true
                                             orderby b.Customer_Name
                                             group b by new{
                                             b.CreatedBy
                                             } into temp
                                     
                                             select new CustSupplierDDModel
                                      {
                                          Val_Name = temp.Key.CreatedBy,
                                        
                                      }).ToList();
               

                 dropdown.CustRegCustomers = (from b in DB.Customer_Registration
                                             where b.Is_Approved!=true && b.Is_Delete!=true
                                      orderby b.Customer_Name
                                             select new CustSupplierDDModel
                                      {
                                          Val_Name = b.Customer_Name,
                                        
                                      }).ToList();
                                     


                return dropdown;

            }

            public IEnumerable<Template_Type> GetTemplateType()
            {
                var output = ListHelper.Template_Type();
                return output;
            }

        [HttpGet]
        public DropdownEntity GetDropdowns()
        {
            DropdownEntity drop = new DropdownEntity();

            drop.PaymentCycle = (from b in DB.PaymentCycles
                                 orderby b.Payment_Cycle_Name
                                 select new PaymentCycleModel
                                 {
                                     Payment_Cycle_Name = b.Payment_Cycle_Name
                                 }).ToList();
            drop.Delivery_Cycle = (from b in DB.Delivery_Cycle
                                   orderby b.Delivery_Cycle_Name
                                   select new Delivery_CycleModel
                                   {
                                       Delivery_Cycle_Name = b.Delivery_Cycle_Name
                                   }).ToList();
            drop.Intermediate_DC = (from b in DB.Intermediate_DC
                                    orderby b.Intermediate_DC_Name
                                    select new Intermediate_DCModel
                                    {
                                        Intermediate_DC_Name = b.Intermediate_DC_Name
                                    }).ToList();
            drop.PaymentType = (from b in DB.PaymentTypes
                                orderby b.Payment_Type_Name
                                select new PaymentTypeModel
                                {
                                    PaymentTypeName = b.Payment_Type_Name
                                }).ToList();
            drop.PORequitionedBy = (from b in DB.PO_RequitionedBy
                                    orderby b.PO_Rqe_Name
                                    select new PORequitionedByModel
                                    {
                                        PO_Rqe_Name = b.PO_Rqe_Name
                                    }).ToList();
            drop.POType = (from b in DB.PO_type
                            select new POTypeModel
                           {
                               PO_Rqe_Name = b.PO_Rqe_Name
                           }).ToList();
            drop.Field_Name = (from f in DB.Label_Fields
                               orderby f.Field_Name
                               where f.is_Deleted == false
                               select new Field_Name
                               {
                                   Field_Name_Id = f.Field_Id,
                                   Field_Name_Value = f.Field_Name
                               }
                                 ).ToList();
            drop.MaterialType = (from b in DB.MaterialSources
                                 orderby b.Material_Source_Name
                                 select new MaterialModel
                                 {
                                     Material_Id = b.MaterialSource_Id,
                                     Material_Name = b.Material_Source_Name
                                 }).ToList();
            drop.VoucherType = (from b in DB.VoucherTypes
                                orderby b.MVoucherType_Name
                                select new VoucherModel
                                {
                                    VoucherType_Id = b.VoucherType_Id,
                                    VoucherType_Name = b.MVoucherType_Name
                                }).ToList();

            drop.DCMaster = (from b in DB.DC_Master
                             orderby b.Dc_Name
                             select new DCMasterModel
                             {
                                 DC_Id = b.Dc_Id,
                                 DC_Code = b.DC_Code,
                                 DC_City = b.City
                             }).ToList();
            //
            drop.Unit = (from b in DB.Units
                         select new UnitModel
                         {
                             Unit_Name = b.Unit_Name
                         }).ToList();
            drop.SKU = (from b in DB.SKU_Master
                        orderby b.SKU_Name
                        select new SKUModel
                        {
                            SKUId = b.SKU_Id,
                            SKUName = b.SKU_Name,
                            SKUCode = b.SKU_Code,
                            HSN_Code=b.HSN_Code,
                            CGST=b.CGST,
                            SGST=b.SGST,
                            Total_GST=b.Total_GST
                        }).ToList();
            
            drop.Supplier = (from b in DB.Suppliers
                             orderby b.Supplier_Name
                             select new SupplierModel
                             {
                                 supplier_Id = b.Supplier_ID,
                                 supplierCode = b.Supplier_code,
                                 SupplierName = b.Supplier_Name
                             }).ToList();
            drop.SKUMainGroup = (from b in DB.SKU_Main_Group
                                 select new SKUMainGroupModel
                                 {
                                     SKU_Main_Group_Id = b.SKU_Main_Group_Id,
                                     SKU_Description = b.SKU_Description
                                 }).ToList();
            drop.SKUSubGroup = (from b in DB.SKU_Sub_Group
                                select new SKUSubGroupModel
                                {
                                    SKU_Sub_Group_Id = b.SKU_Sub_Group_Id,
                                    SKU_Description = b.SKU_Description
                                }).ToList();

            drop.Region = (from a in DB.Region_Master
                           orderby a.Region_Name
                           select new RegionEntity
                           {
                               Region_Id = a.Region_Id,
                               Region_Code = a.Region_Code,
                               Region_Name = a.Region_Name
                           }).ToList();
            drop.Customer = (from b in DB.Customers
                             orderby b.Customer_Name
                             select new Customer_Model
                             {
                                 Cust_Id = b.Cust_Id,
                                 Cust_Code = b.Customer_Code,
                                 Cust_Name = b.Customer_Name
                             }).ToList();

            drop.LocationMaster = (from b in DB.Location_Master
                                   orderby b.Location_Name
                                           select new LocationMasterEntity
                                           {
                                               Location_Id = b.Location_Id,
                                               Location_Code = b.Location_Code,
                                               Location_Name = b.Location_Name
                                           }).ToList();
            drop.RoleMaster = (from b in DB.Role_Master
                               orderby b.Role_Name
                                   select new RoleMasterEntity
                                   {
                                       Role_Id = b.Role_Id,
                                       Role_Name = b.Role_Name
                                      
                                   }).ToList();
            drop.GradeType = ListHelper.GradeType();
            drop.MaterialResource = ListHelper.MaterialResource();
            drop.PackType = ListHelper.Pack_Type();
            drop.Pack_Size = ListHelper.Pack_Size();
           // drop.STIPackType = ListHelper.STIPack_Type();
            drop.DispatchType = ListHelper.Dispatch_Type();
            drop.GetStatus = ListHelper.GetStatus();
            drop.GetLineItemStatus = ListHelper.GetLineItemStatus();
            drop.SKU_Type = ListHelper.SKU_Type();
            drop.Customer_Category = ListHelper.Customer_Category();
            drop.SKU_SubType = ListHelper.SKU_SubType();
            drop.Invoice_Type = ListHelper.Invoice_Type();
            drop.SKU_Category = ListHelper.SKU_Category();
            drop.WastageType = ListHelper.WastageType();
            drop.Pack_Weight_Type = ListHelper.Pack_Weight_Type();
            drop.Delivery_Type = ListHelper.DeliveryType();
            drop.CSIDelivery_Cycle = ListHelper.CSIDelivery_Cycle();
            drop.Data_Type = ListHelper.DataType();
            drop.User_Login_Type=ListHelper.User_Login_Type();

            return drop;
        }

        //[HttpGet]
        //public List<Pack_Size> GetPackSize(string packType)
        //{
        //    List<Pack_Size> PackSize = new List<Pack_Size>
        //  {
        //   new Pack_Size{Pack_Size_Id=1,Pack_Size_Type="MAP",Pack_Size_Value="50 gm"},
        //   new Pack_Size{Pack_Size_Id=2,Pack_Size_Type="MAP",Pack_Size_Value="100 gm"},  
        //   new Pack_Size{Pack_Size_Id=3,Pack_Size_Type="MAP",Pack_Size_Value="200 gm"}, 
        //   new Pack_Size{Pack_Size_Id=4,Pack_Size_Type="MAP",Pack_Size_Value="250 gm"}, 
        //   new Pack_Size{Pack_Size_Id=5,Pack_Size_Type="MAP",Pack_Size_Value="300 gm"}, 
        //   new Pack_Size{Pack_Size_Id=6,Pack_Size_Type="MAP",Pack_Size_Value="350 gm"}, 
        //   new Pack_Size{Pack_Size_Id=7,Pack_Size_Type="MAP",Pack_Size_Value="400 gm"}, 
        //   new Pack_Size{Pack_Size_Id=8,Pack_Size_Type="MAP",Pack_Size_Value="500 gm"}, 
        //   new Pack_Size{Pack_Size_Id=9,Pack_Size_Type="MAP",Pack_Size_Value="750 gm"}, 
        //   new Pack_Size{Pack_Size_Id=10,Pack_Size_Type="MAP",Pack_Size_Value="800 gm"}, 
        //   new Pack_Size{Pack_Size_Id=11,Pack_Size_Type="MAP",Pack_Size_Value="1 kg"}, 
        //   new Pack_Size{Pack_Size_Id=12,Pack_Size_Type="MAP",Pack_Size_Value="5 kg"}, 
        //   new Pack_Size{Pack_Size_Id=13,Pack_Size_Type="MAP",Pack_Size_Value="10 kg"}, 
        //   new Pack_Size{Pack_Size_Id=14,Pack_Size_Type="MAP",Pack_Size_Value="15 kg"}, 
        //   new Pack_Size{Pack_Size_Id=15,Pack_Size_Type="MAP",Pack_Size_Value="20 kg"},
        //   new Pack_Size{Pack_Size_Id=16,Pack_Size_Type="OTHERS",Pack_Size_Value="50 gm"}, 
        //   new Pack_Size{Pack_Size_Id=17,Pack_Size_Type="OTHERS",Pack_Size_Value="100 gm"}, 
        //   new Pack_Size{Pack_Size_Id=18,Pack_Size_Type="OTHERS",Pack_Size_Value="150 gm"}, 
        //   new Pack_Size{Pack_Size_Id=19,Pack_Size_Type="OTHERS",Pack_Size_Value="200 gm"}, 
        //   new Pack_Size{Pack_Size_Id=20,Pack_Size_Type="OTHERS",Pack_Size_Value="250 gm"}, 
        //   new Pack_Size{Pack_Size_Id=21,Pack_Size_Type="OTHERS",Pack_Size_Value="300 gm"}, 
        //   new Pack_Size{Pack_Size_Id=22,Pack_Size_Type="OTHERS",Pack_Size_Value="350 gm"}, 
        //   new Pack_Size{Pack_Size_Id=23,Pack_Size_Type="OTHERS",Pack_Size_Value="400 gm"}, 
        //   new Pack_Size{Pack_Size_Id=24,Pack_Size_Type="OTHERS",Pack_Size_Value="450 gm"}, 
        //   new Pack_Size{Pack_Size_Id=25,Pack_Size_Type="OTHERS",Pack_Size_Value="500 gm"}
        //};
        //    var qr = (from a in PackSize
        //              where a.Pack_Size_Type == packType
        //              select new Pack_Size
        //              {
        //                  Pack_Size_Id = a.Pack_Size_Id,
        //                  Pack_Size_Value = a.Pack_Size_Value
        //              }).ToList();
        //    return qr;
        //}

        //[HttpGet]
        //public PackSizeEntity GetPackSizeA(string packType)
        //{
        //    PackSizeEntity drop = new PackSizeEntity();
        //    drop.PackSize = ListHelper.PackSize().Where(s => s.Pack_Size_Type == packType)
        //                       .Select(s => new Pack_Size
        //                       {
        //                           Pack_Size_Id = s.Pack_Size_Id,
        //                           Pack_Size_Type = s.Pack_Size_Type,
        //                           Pack_Size_Value = s.Pack_Size_Value,
        //                       }).ToList();
        //    return drop;
        //}

        //[HttpGet]
        //public dynamic GetSKUs()
        //{
        //    var query = (from t1 in DB.SKU_Master
        //                 select t1).Select(t1 => new 
        //                 {
        //                     SKU = t1,
        //                     CustSKU = DB.Customer_SKU_Master.ToList()
        //                 }).ToList();            
        //    return query;
        //}
          public List<Vehicle_No> GetVehicleNos()
        {
            var xs = ListHelper.Vehicle_No().ToList();
              
           // var result = xs.ToList();
            return xs;
        }
             [HttpGet]
        public List<PincodeEntity.PinTable> GetPincode(int id)
        {
            var xs = from s in DB.PinCodes
                     where s.PinCode1 == id
                     select new PincodeEntity.PinTable
                     {
                         City = s.City,
                         State = s.State
                     };
            var result = xs.ToList();
            return result;
        }

       public List<DCMasterModel> getDCforSTIJDM(string ULocation)
        {
           
                var dclocation = (from b in DB.DC_Master
                                  orderby b.Dc_Name
                                  select new DCMasterModel
                                                  {
                                                      DC_Id = b.Dc_Id,
                                                      DC_Code = b.DC_Code,
                                                      DC_City = b.Dc_Name,
                                                      StatusCode= HttpStatusCode.OK,
                                                      Message = "Success"
            
                                                  }).ToList();
                return dclocation;
        }
         public List<DCMasterModel> GetDC()
        {
           
                var dclocation = (from b in DB.DC_Master
                                  orderby b.Dc_Name
                                  select new DCMasterModel
                                                  {
                                                      DC_Id = b.Dc_Id,
                                                      DC_Code = b.DC_Code,
                                                      DC_City = b.Dc_Name,
                                                      StatusCode= HttpStatusCode.OK,
                                                      Message = "Success"
            
                                                  }).ToList();
                return dclocation;
        }

         public DCLocationModel GetLocDCCombine()
         {
            // List<TargetLocationModel> dclocation = new List<TargetLocationModel>();
             DCLocationModel dclocation = new DCLocationModel();
        
             
             var location = (from b in DB.Location_Master
                               orderby b.Location_Name
                               select new TargetLocationModel
                               {
                                   TargetLocation_City = b.Location_Name,
                                   TargetLocation_Code = b.Location_Code,
                                   TargetLocation_Id = b.Location_Id,
                                   TargetLocation_Type = "LOCATION",
                                   StatusCode = HttpStatusCode.OK,
                                   Message = "Success"

                               }).ToList();
             dclocation.targetLocations = location;
             var dc = (from b in DB.DC_Master
                       orderby b.Dc_Name
                       select new TargetLocationModel
                       {
                           TargetLocation_City = b.Dc_Name,
                           TargetLocation_Code = b.DC_Code,
                           TargetLocation_Id = b.Dc_Id,
                           TargetLocation_Type = "DC",
                           StatusCode = HttpStatusCode.OK,
                           Message = "Success"

                       }).ToList();
             foreach(var y in dc)
             {
                 dclocation.targetLocations.Add(y);
             }

             return dclocation;
         }


        

      





       //public List<DCMasterModel> getDCforSTINJDM(string ULocation)
       //{
       //    var dclocation = (from b in DB.DC_Master
       //                      where b.DC_Code != "PLD" && b.DC_Code != "OTY"
       //                      orderby b.Dc_Name
       //                      select new DCMasterModel
       //                     {
       //                         DC_Id = b.Dc_Id,
       //                         DC_Code = b.DC_Code,
       //                         DC_City = b.Dc_Name,
       //                         StatusCode = HttpStatusCode.OK,
       //                         Message = "Success"
       //                     }).ToList();
       //    return dclocation;

       //}
    

        public List<Tuple<string>> getMaterialResource()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.MaterialResource", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("FarmerResource")));
            list.Add(new Tuple<string>(rm.GetString("MarketResource")));

            return list;
        }
        public List<Tuple<string, string>> getSTI_Type()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.STI_Pack_Type", Assembly.GetExecutingAssembly());
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            list.Add(new Tuple<string, string>("s1", rm.GetString("s1")));
            list.Add(new Tuple<string, string>("s2", rm.GetString("s2")));
            list.Add(new Tuple<string, string>("s3", rm.GetString("s3")));

            return list;
        }

        public List<Tuple<string>> getDispatchTypes()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.Dispatch_Type", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("CustomerType")));
            list.Add(new Tuple<string>(rm.GetString("DCType")));
            list.Add(new Tuple<string>(rm.GetString("WastageType")));
            return list;
        }

        public List<Tuple<string>> getSkuTypes()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.SKU_Type", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("OrganicType")));
            list.Add(new Tuple<string>(rm.GetString("StandardType")));

            return list;
        }

        public List<Tuple<string>> getInvoiceTypes()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.Invoice_Type", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("AgentType")));
            list.Add(new Tuple<string>(rm.GetString("DirectCustType")));
            return list;
        }

        public List<Tuple<string>> getSkuCategories()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.SKU_Category", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("LeafyType")));
            list.Add(new Tuple<string>(rm.GetString("FruitType")));
            list.Add(new Tuple<string>(rm.GetString("ExoticType")));
            list.Add(new Tuple<string>(rm.GetString("TropicalType")));
            list.Add(new Tuple<string>(rm.GetString("HerbType")));
            list.Add(new Tuple<string>(rm.GetString("TemperateType")));
            return list;
        }

        public List<Tuple<string>> getPackTypes()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.Pack_Type", Assembly.GetExecutingAssembly());
            List<Tuple<string>> list = new List<Tuple<string>>();
            list.Add(new Tuple<string>(rm.GetString("GunnyType")));
            list.Add(new Tuple<string>(rm.GetString("PPType")));
            list.Add(new Tuple<string>(rm.GetString("CratesType")));
            list.Add(new Tuple<string>(rm.GetString("CartonsType")));
            return list;
        }

        public List<string> getLocations()
        {
            ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
            List<string> list = new List<string>();
            list.Add(rm.GetString("CDT"));
            list.Add(rm.GetString("GRNT"));
            list.Add(rm.GetString("INVT"));
            list.Add(rm.GetString("POT"));
            list.Add(rm.GetString("STIT"));
            list.Add(rm.GetString("STKT"));
            list.Add(rm.GetString("SDT"));
            return list;
        }

        public List<DCMasterModel> getLocation(int userId)
        {
            var dclocation = (from b in DB.DC_Master
                              join x in DB.User_DC_Access on b.DC_Code equals x.DC_Code
                              where x.User_Id == userId
                              orderby b.Dc_Name
                              select new DCMasterModel
                              {
                                  DC_Id = b.Dc_Id,
                                  DC_Code = b.DC_Code,
                                  DC_City = b.City
                              }).ToList();

            return dclocation;
        }
    }
}