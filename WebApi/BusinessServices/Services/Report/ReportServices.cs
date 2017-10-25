using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class ReportServices : IReportServices
    {
        private readonly LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public ReportServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        //public List<ShrinkageReportEntity> ShrinkageReport(string Ulocation = "null")
        //{
        //    List<ShrinkageReportEntity> k = new List<ShrinkageReportEntity>();
        //    DateTime? today = DateTime.Now;
        //    int s = 0;
        //    s = (from u in DB.Physical_Stock
        //             where u.is_Deleted != true &&
        //             u.CreatedDate.Value.Year == today.Value.Year &&
        //             u.CreatedDate.Value.Month == today.Value.Month && 
        //             u.CreatedDate.Value.Day == today.Value.Day &&
        //             u.DC_Code == Ulocation
        //             select u.Phy_Stock_Id).Count();
        //    if (s == 0)
        //    {
        //        return k;
        //    }
        //    //
        //    var output = new ShrinkageReportEntity();

        //    output.CDN_ProcessShrinkage = (DB.Dispatch_Creation
        //                                      .Join(DB.Dispatch_Line_item,
        //                                      sc => sc.Dispatch_Id,
        //                                      soc => soc.Dispatch_Id,
        //                                      (sc, soc) => new { sc, soc })
        //                                      .Where(z =>
        //                                      z.sc.CreatedDate.Value.Year == today.Value.Year
        //                                      && z.sc.CreatedDate.Value.Month == today.Value.Month
        //                                      && z.sc.CreatedDate.Value.Day == today.Value.Day
        //                                      && z.sc.is_Deleted != true
        //                                      && z.soc.Strinkage_Qty != null
        //                                      && z.sc.Dispatch_Location_Code == Ulocation)
        //                                     .Sum(z => (double?)(z.soc.Strinkage_Qty))) ?? 0;

        //   output.GRN_CustReturnShrinkage = (DB.GRN_Creation
        //                                        .Join(DB.GRN_Line_item,
        //                                        sc => sc.GRN_Number,
        //                                        soc => soc.GRN_Number,
        //                                        (sc, soc) => new { sc, soc })
        //                                        .Where(z =>
        //                                        z.sc.CreatedDate.Value.Year == today.Value.Year
        //                                        && z.sc.CreatedDate.Value.Month == today.Value.Month
        //                                        && z.sc.CreatedDate.Value.Day == today.Value.Day
        //                                        && z.soc.Strinkage_Qty != null
        //                                        && z.sc.is_Deleted != true && z.sc.DC_Code == Ulocation)
        //                                        .Sum(z => (double?)(z.soc.Strinkage_Qty))) ?? 0;
        //    //
        //    double stock = DB.Stocks.Where(x => x.DC_Code == Ulocation).Sum(x => (double?)(x.Closing_Qty)) ?? 0;
        //    double phystock = DB.Physical_Stock.Where(F =>F.is_Deleted == false && F.CreatedDate == today.Value && F.DC_Code == Ulocation).Sum(x => (double?)(x.Closing_Qty)) ?? 0;

        //    output.Stock_FloorShrinkage = stock - phystock;

        //    k.Add(output);
        //    if (k == null)
        //    {
        //        return k;
        //    }
        //    //

        //    return k;

        //}

        public List<ShrinkageReportEntity> ShrinkageSKUwiseReport(string Ulocation = "null")
        {
            List<ShrinkageReportEntity> k = new List<ShrinkageReportEntity>();
            DateTime? today = DateTime.Now;
            int s = 0;
            s = (from u in DB.Physical_Stock
                 where u.is_Deleted != true &&
                 u.CreatedDate.Value.Year == today.Value.Year &&
                 u.CreatedDate.Value.Month == today.Value.Month &&
                 u.CreatedDate.Value.Day == today.Value.Day &&
                 u.DC_Code == Ulocation
                 select u.Phy_Stock_Id).Count();
            if (s == 0)
            {
                return k;
            }
            var skuList = (from a in DB.SKU_Master
                           select a).ToList();
          
            foreach(var hh in skuList)
            {
                var output = new ShrinkageReportEntity();
                output.SKU_Name = hh.SKU_Name;
                output.CDN_ProcessShrinkage = (DB.Dispatch_Creation
                                           .Join(DB.Dispatch_Line_item,
                                           sc => sc.Dispatch_Id,
                                           soc => soc.Dispatch_Id,
                                           (sc, soc) => new { sc, soc })
                                           .Where(z =>
                                           z.sc.CreatedDate.Value.Year == today.Value.Year
                                           && z.sc.CreatedDate.Value.Month == today.Value.Month
                                           && z.sc.CreatedDate.Value.Day == today.Value.Day
                                           && z.sc.is_Deleted != true
                                           && z.soc.SKU_Name==hh.SKU_Name
                                           && z.soc.Strinkage_Qty != null
                                           && z.sc.Dispatch_Location_Code == Ulocation)
                                          .Sum(z => (double?)(z.soc.Strinkage_Qty))) ?? 0;
                
                output.GRN_CustReturnShrinkage = (DB.GRN_Creation
                                                     .Join(DB.GRN_Line_item,
                                                     sc => sc.GRN_Number,
                                                     soc => soc.GRN_Number,
                                                     (sc, soc) => new { sc, soc })
                                                     .Where(z =>
                                                     z.sc.CreatedDate.Value.Year == today.Value.Year
                                                     && z.sc.CreatedDate.Value.Month == today.Value.Month
                                                     && z.sc.CreatedDate.Value.Day == today.Value.Day
                                                     && z.soc.SKU_Name == hh.SKU_Name
                                                     && z.soc.Strinkage_Qty != null
                                                     && z.sc.is_Deleted != true && z.sc.DC_Code == Ulocation)
                                                     .Sum(z => (double?)(z.soc.Strinkage_Qty.Value))) ?? 0;
                
                double stock = DB.Stocks.Where(x => x.DC_Code == Ulocation && x.SKU_Name==hh.SKU_Name).Sum(x => (double?)(x.Closing_Qty)) ?? 0;
                double phystock = DB.Physical_Stock.Where(F => F.is_Deleted == false && F.CreatedDate == today.Value && F.DC_Code == Ulocation && F.SKU_Name==hh.SKU_Name).Sum(x => (double?)(x.Closing_Qty)) ?? 0;

                output.Stock_FloorShrinkage = stock - phystock;

                k.Add(output);
                output = new ShrinkageReportEntity();
            }          
                     
            if (k == null)
            {
                return k;
            }

            return k;

        }
        public List<GRNReportEntity> GRNReport(DateTime? premonth, DateTime? month, DateTime? day, string Ulocation = "null")
        {
            IQueryable<GRNReportEntity> qu;
            List<GRNReportEntity> pre;
            List<GRNReportEntity> premnth;
            List<GRNReportEntity> result = new List<GRNReportEntity>();

            qu = (from a in DB.REPORT_OF_IN_OUT_SUMMARY
                  where a.DC_Code == Ulocation
                  select new GRNReportEntity
                  {
                      DC_Code = a.DC_Code,
                      Grade = a.Grade,
                      CreatedDate = a.CreatedDate,
                      IN_SKU_Count = a.IN_SKU_Count,
                      IN_TOTAL_QTY = a.IN_TOTAL_QTY,
                      OUT_SKU_Count = a.OUT_SKU_Count,
                      OUT_TOTAL_QTY = a.OUT_TOTAL_QTY,
                      SKU_Type = a.SKU_Type,
                      UOM = a.UOM
                  });
            //
            if (premonth != null)
            {
                DateTime? StaticMay = new DateTime(premonth.Value.Year - 1, 5, 1);
                DateTime? ToDate = new DateTime();
                if (premonth.Value.Month == 01)
                {
                    ToDate = new DateTime(premonth.Value.Year - 1, 12, premonth.Value.Day);
                    pre = qu.Where(a => (a.CreatedDate.Value.Year == StaticMay.Value.Year && a.CreatedDate.Value.Month >= StaticMay.Value.Month)).OrderBy(a => a.CreatedDate).ToList();
                    premnth = qu.Where(a => (a.CreatedDate.Value.Year == ToDate.Value.Year && a.CreatedDate.Value.Month <= ToDate.Value.Month)).OrderBy(a => a.CreatedDate).ToList();
                    foreach (var t in premnth)
                    {
                        pre.Add(t);
                    }
                }
                else
                {
                    ToDate = new DateTime(premonth.Value.Year, premonth.Value.Month - 1, premonth.Value.Day);
                    pre = qu.Where(a => (a.CreatedDate.Value.Year >= StaticMay.Value.Year && a.CreatedDate.Value.Month >= StaticMay.Value.Month)).OrderBy(a => a.CreatedDate).ToList();
                    premnth = qu.Where(a => (a.CreatedDate.Value.Year <= ToDate.Value.Year && a.CreatedDate.Value.Month <= ToDate.Value.Month)).OrderBy(a => a.CreatedDate).ToList();
                    foreach (var t in premnth)
                    {
                        pre.Add(t);
                    }
                }
                //
                result = pre;
                //qu = qu.Where(a => a.CreatedDate.Value.Month == premonth.Value.Month && a.CreatedDate.Value.Year == premonth.Value.Year);
            }
            if (month != null)
            {
                qu = qu.Where(a => a.CreatedDate.Value.Month == month.Value.Month && a.CreatedDate.Value.Year == month.Value.Year);
                result = qu.OrderBy(a => a.CreatedDate).ToList();
            }
            if (day != null)
            {
                qu = qu.Where(a => a.CreatedDate.Value.Day == day.Value.Day && a.CreatedDate.Value.Month == day.Value.Month && a.CreatedDate.Value.Year == day.Value.Year);
                result = qu.OrderBy(a => a.CreatedDate).ToList();
            }



            return result;
        }


        public List<StockSummaryReportEntity> GetStockSummaryReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<StockSummaryReportEntity> qu;
            List<StockSummaryReportEntity> result = new List<StockSummaryReportEntity>();

            qu = (from x in DB.REPORT_STOCK_SUMMARY_UPDATED
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.DC_Code == dccode
                  select new StockSummaryReportEntity
                  {
                      Number_of_SKU = x.Number_of_SKU,
                      TOTAL_GRADE_VALUE = x.TOTAL_GRADE_VALUE,
                      DC_Code = x.DC_Code,
                      SKU_Category = x.SKU_Category,
                      A_GRADE = x.A_GRADE,
                      B_GRADE = x.B_GRADE,
                      CreatedDate = x.CreatedDate
                  });
            //
            result = qu.ToList();

            return result;
        }

        public List<WastageReportEntity> WastageReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<WastageReportEntity> qu;
            List<WastageReportEntity> result = new List<WastageReportEntity>();

            qu = (from x in DB.REPORT_WASTAGE_QTY
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.DC_Code == dccode
                  select new WastageReportEntity
                  {
                      DC_Code = x.DC_Code,
                      C_QTY = x.C_QTY,
                      CreatedDate = x.CreatedDate,
                      Floor = x.Floor,
                      Process = x.Process,
                      SKU_Category = x.SKU_Category,
                      Total_qty = x.Total_qty

                  });

            result = qu.ToList();

            return result;
        }

        public List<PurchaseReportEntity> PurchaseReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<PurchaseReportEntity> qu;
            List<PurchaseReportEntity> result = new List<PurchaseReportEntity>();

            qu = (from x in DB.REPORT_PURCHASE
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.Expr1 == dccode
                  select new PurchaseReportEntity
                  {
                      DC_Code = x.Expr1,
                      Fill_Rate = x.Fill_Rate,
                      CreatedDate = x.CreatedDate,
                      Po_Qty = x.Po_Qty,
                      Recived_Qty = x.Recived_Qty,
                      SKU_Category = x.SKU_Category,

                  });

            result = qu.ToList();

            return result;
        }

        public List<STIDISPATCHReportEntity> STIReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<STIDISPATCHReportEntity> qu;
            List<STIDISPATCHReportEntity> result = new List<STIDISPATCHReportEntity>();

            qu = (from x in DB.REPORT_STI_DISPATCH
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.Material_Source == dccode
                  select new STIDISPATCHReportEntity
                  {
                      Material_Source = x.Material_Source,
                      Fill_Rate = x.Fill_Rate,
                      CreatedDate = x.CreatedDate,
                      Dispatch_Qty = x.Dispatch_Qty,
                      Qty = x.Qty,
                      SKU_Category = x.SKU_Category


                  });

            result = qu.ToList();

            return result;
        }

        public List<DispatchQTYReportEntity> DispatchQtyReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<DispatchQTYReportEntity> qu;
            List<DispatchQTYReportEntity> result = new List<DispatchQTYReportEntity>();

            qu = (from x in DB.REPORT_DISPATCH
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.Delivery_Location_Code == dccode
                  select new DispatchQTYReportEntity
                  {
                      Delivery_Location_Code = x.Delivery_Location_Code,
                      A = x.A,
                      CreatedDate = x.CreatedDate,
                      B = x.B,
                      No_SKU = x.No_SKU,
                      SKU_Category = x.SKU_Category,
                      Total_qty = x.Total_qty

                  });

            result = qu.ToList();

            return result;
        }

        public List<DispatchPackTypeBasedReportEntity> DispatchPackTypeBasedReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<DispatchPackTypeBasedReportEntity> qu;
            List<DispatchPackTypeBasedReportEntity> result = new List<DispatchPackTypeBasedReportEntity>();

            qu = (from x in DB.REPORT_DISPATCH_PACKTYPE_BASED
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.Delivery_Location_Code == dccode
                  select new DispatchPackTypeBasedReportEntity
                  {
                      Delivery_Location_Code = x.Delivery_Location_Code,
                      SKU_Category = x.SKU_Category,
                      CreatedDate = x.CreatedDate,
                      Branded_Bulk = x.Branded_Bulk,

                      Branded_Retail = x.Branded_Retail,
                      Total = x.Total,
                      Un_Branded_Bulk = x.Un_Branded_Bulk,
                      Number_of_SKU = x.Number_of_SKU,


                  });

            result = qu.ToList();

            return result;
        }
        public List<CustomerRateFillReportEntity> GetCustomerRateFillReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<CustomerRateFillReportEntity> qu;
            List<CustomerRateFillReportEntity> result = new List<CustomerRateFillReportEntity>();

            qu = (from x in DB.REPORT_CUSTOMER_FILL_RATE
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.DC_Code == dccode
                  select new CustomerRateFillReportEntity
                  {

                      DC_Code = x.DC_Code,
                      CreatedDate = x.CreatedDate,
                      DC_Sale = x.DC_Sale,
                      Distributor = x.Distributor,
                      Exporter = x.Exporter,
                      HORECA = x.HORECA,
                      General_Trade = x.General_Trade,
                      Market_Sale = x.Market_Sale,
                      Modern_Retail = x.Modern_Retail,
                      Type = x.Type,
                      Wholesaler = x.Wholesaler
                  });

            result = qu.ToList();

            return result;
        }

        public List<CustomerRateFillMonthBasedReportEntity> GetCustomerRateFillReportMonthBased(string Type, string dccode)
        {

            List<CustomerRateFillMonthBasedReportEntity> result = new List<CustomerRateFillMonthBasedReportEntity>();
            List<CustomerRateFillMonthBasedReportEntity> Finalresult = new List<CustomerRateFillMonthBasedReportEntity>();

            List<CustomerRateFillMonthBasedReportEntity> pre12Month = new List<CustomerRateFillMonthBasedReportEntity>();


            result = (from x in DB.REPORT_MONTH_BASED_ORDER
                      where x.Type == Type && x.DC_Code == dccode
                      select new CustomerRateFillMonthBasedReportEntity
                      {
                          Category = x.Category,
                          DC_Code = x.DC_Code,
                          CreatedDate = x.CreatedDate,
                          Current_Month_Total_Count = x.Total_Count,
                          Type = x.Type
                      }).ToList();

            pre12Month = (from x in DB.REPORT_12MONTH_BASED_ORDER
                          where x.Type == Type && x.DC_Code == dccode
                          select new CustomerRateFillMonthBasedReportEntity
                          {
                              Category = x.Category,
                              DC_Code = x.DC_Code,
                              Previous_Month_Total_Count = x.Total_Count,
                              CreatedDate = x.CreatedDate,
                              Type = x.Type
                          }).ToList();

            Finalresult = (from first in result
                           join second in pre12Month
                           on first.DC_Code equals second.DC_Code
                           where first.Category == second.Category && first.CreatedDate == second.CreatedDate && first.Type == second.Type
                           select new CustomerRateFillMonthBasedReportEntity
                           {
                               Category = first.Category,
                               DC_Code = first.DC_Code,
                               CreatedDate = first.CreatedDate,
                               Current_Month_Total_Count = first.Current_Month_Total_Count,
                               Type = first.Type,
                               Previous_Month_Total_Count = second.Previous_Month_Total_Count
                           }).ToList();



            return Finalresult;
        }
        public List<CSIReportEntity> CSIBasedReport(DateTime fromdate, DateTime todate, string dccode)
        {
            IQueryable<CSIReportEntity> qu;
            List<CSIReportEntity> res = new List<CSIReportEntity>();
            List<CSIReportEntity> result = new List<CSIReportEntity>();
            //  double? uu;
            qu = (from x in DB.REPORT_CSI
                  where x.CreatedDate >= fromdate && x.CreatedDate <= todate && x.DC_Code == dccode
                  select new CSIReportEntity
                  {
                      DC_Code = x.DC_Code,
                      SKU_Category = x.SKU_Category,
                      CreatedDate = x.CreatedDate,
                      CDN_Qty = x.CDN_Qty,
                      //uu=Math.Round(x.CSI_Qty, 2, MidpointRounding.AwayFromZero),


                      CSI_Qty = x.CSI_Qty,
                      Customer_return_qty = x.Customer_return_qty,
                      Fill_Rate = x.Fill_Rate,
                      Invoice_Qty = x.Invoice_Qty
                  });

            //res = qu.ToList();
            result = qu.ToList();
            //foreach (var y in res)
            //{
            //    var EE = new CSIReportEntity();
            //    EE.Customer_return_qty=y.Customer_return_qty;
            //    EE.DC_Code = y.DC_Code;
            //    EE.CreatedDate = y.CreatedDate;
            //    EE.CSI_Qty = y.CSI_Qty;
            //    EE.Fill_Rate = y.Fill_Rate;
            //    EE.SKU_Category = y.SKU_Category;
            //    EE.CDN_Qty = y.CDN_Qty;
            //    result.Add(uu);
            //}

            return result;
        }
        public IEnumerable<CustFillRateEntity> CustomerFillRatereportDD()
        {
            return new CustFillRateEntity[]{
              new CustFillRateEntity { Id=1, Value="CSI"},
              new CustFillRateEntity { Id=2, Value="CDN"},
              new CustFillRateEntity { Id=3, Value="INVOICE"}
        };
        }
        //-------------------------- HELPER CLASSESS--------------------
        public IEnumerable<GRNDDEntity> GRNreportDD()
        {
            return new GRNDDEntity[]{
              new GRNDDEntity { Id=1, Value="For The Day"},
              new GRNDDEntity { Id=2, Value="For The Current Month"},
              new GRNDDEntity { Id=3, Value="For The Past Month"}
        };

        }
    }
}
