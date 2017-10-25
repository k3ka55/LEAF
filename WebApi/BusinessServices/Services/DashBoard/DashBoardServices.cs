using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BusinessServices
{
    public class DashBoardServices : IDashBoardServices
    {

        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public DashBoardServices()
        {
            _unitOfWork = new UnitOfWork();
        }


        [HttpGet]
        public List<DCDASHBOARD_CSISTIFORGRAPHEntity> GetDCDASHBOARD_CSISTIFORGRAPH(string DC_Code)
        {
            //  IQueryable<DCDASHBOARD_CSISTIFORGRAPHEntity> qu;
          //  List<DCDASHBOARD_CSISTIFORGRAPHEntity> result;
            var qu = (from t in DB.DCDASHBOARD_CSISTIFORGRAPH

                      where t.DC_Code == DC_Code

                      group new { t } by new
                      {
                          t.Created_year,
                          t.Created_Month,
                          t.DC_Code,
                      } into g

                      orderby g.Key.Created_year descending
                      orderby g.Key.Created_Month descending
                      select new
                      {
                          g.Key.Created_Month,
                          g.Key.Created_year,
                          g.Key.DC_Code,
                          Total_Orders = (double?)g.Sum(p => p.t.Total_Orders),
                          Total_Qty = (double?)g.Sum(p => p.t.Total_Qty),
                          Dispatched_orders = (double?)g.Sum(p => p.t.Dispatched_orders),
                          Dispatched_qty = (double?)g.Sum(p => p.t.Dispatched_qty)
                      }).Distinct();

            var query = (qu.GroupBy(c => new { c.Created_Month, c.Created_year, c.DC_Code }).OrderByDescending(g=>g.Key.Created_year)
   .Select(g => new DCDASHBOARD_CSISTIFORGRAPHEntity
   {

       Created_Month = g.Key.Created_Month,
       DC_Code = g.Key.DC_Code,
       Created_year = g.Key.Created_year,
       Total_Qty = g.Sum(c => c.Total_Qty),
       Total_Orders = g.Sum(c => c.Total_Orders),
       Dispatched_orders = g.Sum(c => c.Dispatched_orders),
       Dispatched_qty =g.Sum(c => c.Dispatched_qty),

   })).ToList();
            return query;
        }


        [HttpGet]
        public DashBoardEntity GetDCDashBoard(string DC_Code)
        {
            DashBoardEntity drop = new DashBoardEntity();
            var percent = (from b in DB.DCDASHBOARD_CSI2_CUST_PERCENTBASE
                           where b.DC_Code == DC_Code
                           select new DCDASHBOARD_CSI2_CUST_PERCENTBASEEntity
                           {
                               DC_Code = b.DC_Code,
                               No_of_CSI_ORDER = b.No_of_CSI_ORDER,
                               Customer_count = b.Customer_count,
                               Sku_Count = b.Sku_Count
                           }).FirstOrDefault();
            //
            if(percent!=null)
            {
                drop.DCDASHBOARD_CSI2CUST = (from b in DB.DCDASHBOARD_CSI2_CUST
                                             where b.DC_Code == DC_Code
                                             select new DCDASHBOARD_CSI2CUSTEntity
                                             {
                                                 DC_Code = b.DC_Code,
                                                 No_of_CSI_ORDER = b.No_of_CSI_ORDER,
                                                 No_of_Customer = b.No_of_Customer,
                                                 No_of_SKU = b.No_of_SKU,
                                                 No_of_CSI_ORDERpercent = percent.No_of_CSI_ORDER != 0 ? ((b.No_of_CSI_ORDER / percent.No_of_CSI_ORDER) * 100) : 0,
                                                 No_of_Customerpercent = percent.Customer_count != 0 ? ((b.No_of_Customer / percent.Customer_count) * 100) : 0,
                                                 No_of_SKUpercent = percent.Sku_Count != 0 ? ((b.No_of_SKU / percent.Sku_Count) * 100) : 0
                                             }).ToList();
            }
            else if(percent==null)
            {
                drop.DCDASHBOARD_CSI2CUST = (from b in DB.DCDASHBOARD_CSI2_CUST
                                             where b.DC_Code == DC_Code
                                             select new DCDASHBOARD_CSI2CUSTEntity
                                             {
                                                 DC_Code ="0",
                                                 No_of_CSI_ORDER = 0,
                                                 No_of_Customer = 0,
                                                 No_of_SKU = 0
                                            }).ToList();
            }
           var topCustpercent = (from b in DB.DCDASHBOARD_TOP5CUSTOMER
                                  where b.DC_Code == DC_Code
                                  group b by b.DC_Code into g
                                  select new
                                  {
                                      Total_Orders = g.Sum(z => z.Customer_Order)
                                  }).FirstOrDefault();
            if(topCustpercent!=null)
            {
                drop.DCDASHBOARD_TOP5CUSTOMER = (from b in DB.DCDASHBOARD_TOP5CUSTOMER
                                                 where b.DC_Code == DC_Code
                                                 select new DCDASHBOARD_TOP5CUSTOMEREntity
                                                 {
                                                     DC_Code = b.DC_Code,
                                                     Customer_Order = b.Customer_Order,
                                                     Customer_Name = b.Customer_Name,
                                                     Cust_Orderpercent = topCustpercent.Total_Orders != 0 ? ((b.Customer_Order / topCustpercent.Total_Orders) * 100) : 0

                                                 }).ToList();
            }
            else if (topCustpercent==null)
            {
                drop.DCDASHBOARD_TOP5CUSTOMER = (from b in DB.DCDASHBOARD_TOP5CUSTOMER
                                                 where b.DC_Code == DC_Code
                                                 select new DCDASHBOARD_TOP5CUSTOMEREntity
                                                 {
                                                     DC_Code = "0",
                                                     Customer_Order = 0,
                                                     Customer_Name = "0",
                                                  }).ToList();
            }
            drop.DCDASHBOARD_STOCK = (from b in DB.DCDASHBOARD_STOCK
                                      where b.DC_Code == DC_Code
                                      select new DCDASHBOARD_STOCKEntity
                                      {
                                          DC_Code = b.DC_Code,
                                          Closing_Qty = b.Closing_Qty,
                                          No_of_SKUs = b.No_of_SKUs
                                      }).ToList();

            drop.DCDASHBOARD_PO = (from b in DB.DCDASHBOARD_PO
                                   where b.DC_Code == DC_Code
                                   select new DCDASHBOARD_POEntity
                                   {
                                       DC_Code = b.DC_Code,
                                       No_of_PO = b.No_of_PO,
                                       No_of_Supplier_name = b.No_of_Supplier_name,
                                       TOTAL_Qty = b.TOTAL_Qty
                                   }).ToList();

            drop.DCDASHBOARD_GRN = (from b in DB.DCDASHBOARD_GRN
                                    where b.DC_Code == DC_Code
                                    select new DCDASHBOARD_GRNEntity
                                    {
                                        DC_Code = b.DC_Code,
                                        No_of_SKU = b.No_of_SKU,
                                        No_of_Supplier_name = b.No_of_Supplier_Name,
                                        TOTAL_Qty = b.Total_Qty
                                    }).ToList();
            drop.DCDASHBOARD_STI = (from b in DB.DCDASHBOARD_STI
                                    where b.DC_Code == DC_Code
                                    select new DCDASHBOARD_STIEntity
                                    {
                                        DC_Code = b.DC_Code,
                                        STI_Number = b.STI_Number,
                                        Qty = b.Qty

                                    }).ToList();
            drop.DCDASHBOARD_WASTAGE = (from b in DB.DCDASHBOARD_WASTAGE
                                        where b.DC_Code == DC_Code
                                        select new DCDASHBOARD_WASTAGEEntity
                                        {
                                            DC_Code = b.DC_Code,
                                            No_of_SKUs = b.No_of_SKUs,
                                            Wastage_Qty = b.Wastage_Qty

                                        }).ToList();
            drop.DCDASHBOARD_DISPATCH = (from b in DB.DCDASHBOARD_DISPATCH
                                         where b.DC_Code == DC_Code
                                         select new DCDASHBOARD_DISPATCHEntity
                                         {
                                             DC_Code = b.DC_Code,
                                             Customer_Dispatch_Number = b.Customer_Dispatch_Number,
                                             TOTAL_QTY = b.TOTAL_QTY

                                         }).ToList();
            drop.DCDASHBOARD_CSISTIADD = (from b in DB.DCDASHBOARD_CSISTIADD
                                          where b.DC_Code == DC_Code
                                          select new DCDASHBOARD_CSISTIADDEntity
                                     {
                                         Indent_Raised_by_DC_Code = b.DC_Code,
                                         Total_Orders = b.Total_Orders,
                                         Total_Qty = b.Total_Qty

                                     }).ToList();


            drop.DCDASHBOARD_TOP5SKU = (from b in DB.DCDASHBOARD_TOP5SKU
                                        where b.DC_Code == DC_Code
                                        select new DCDASHBOARD_TOP5SKUEntity
                                        {
                                            DC_Code = b.DC_Code,
                                            Closing_Qty = b.Closing_Qty,
                                            SKU_Name = b.SKU_Name

                                        }).ToList();
        

         
            //drop.DCDASHBOARD_TOP5CUSTOMER = (from b in DB.DCDASHBOARD_TOP5CUSTOMER
            //                                 where b.DC_Code == DC_Code
            //                                 select new DCDASHBOARD_TOP5CUSTOMEREntity
            //                                 {
            //                                     DC_Code = b.DC_Code,
            //                                     Customer_Order = b.Customer_Order,
            //                                     Customer_Name = b.Customer_Name

            //                                 }).ToList();
            //drop.DCDASHBOARD_CSI2CUST = (from b in DB.DCDASHBOARD_CSI2_CUST
            //                             where b.DC_Code == DC_Code
            //                             select new DCDASHBOARD_CSI2CUSTEntity
            //                             {
            //                                 DC_Code = b.DC_Code,
            //                                 No_of_CSI_ORDER = b.No_of_CSI_ORDER,
            //                                 No_of_Customer = b.No_of_Customer,
            //                                 No_of_SKU = b.No_of_SKU
            //                             }).ToList();
            return drop;
        }



    }
}
