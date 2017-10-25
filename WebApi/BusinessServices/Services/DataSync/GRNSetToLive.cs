using System;
using System.Collections.Generic;
using System.Linq;
using DataModelLocal;
//using DataModel;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entity;
using System.Transactions;
using BusinessServices.Interfaces;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class GRNSetToLive : IGRNSetToLive
    {
        //LEAFDBEntities CloudDB = new LEAFDBEntities();
        LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string fileName = "GRN_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public GRNSetToLive()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName + ".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void setGRN()
        {
            int lcount = 0;
            if (STI_PUSH_ENTITY.grnrecord != null)
            {
                foreach (var grnfromLive in STI_PUSH_ENTITY.grnrecord)
                {
                    var ExistData = CloudDB.GRN_Creation.Where(x => x.GRN_Number == grnfromLive.GRN_Number).FirstOrDefault();
                    if (ExistData == null)
                    {
                        var movedGRN = new GRN_Creation();
                        using (var scope1 = new TransactionScope())
                        {
                            movedGRN.GRN_Number = grnfromLive.GRN_Number;
                            movedGRN.PO_Number = grnfromLive.PO_Number;
                            movedGRN.GRN_Rls_Date = grnfromLive.GRN_Rls_Date;
                            movedGRN.CDN_Number = grnfromLive.CDN_Number;
                            movedGRN.Voucher_Type = grnfromLive.Voucher_Type;
                            movedGRN.SKU_Type_Id = grnfromLive.SKU_Type_Id;
                            movedGRN.SKU_Type = grnfromLive.SKU_Type;
                            movedGRN.STN_DC_Id = grnfromLive.STN_DC_Id;
                            movedGRN.STN_DC_Code = grnfromLive.STN_DC_Code;
                            movedGRN.STN_DC_Name = grnfromLive.STN_DC_Name;
                            movedGRN.Supplier_Id = grnfromLive.Supplier_Id;
                            movedGRN.Customer_Name = grnfromLive.Customer_Name;
                            movedGRN.Customer_code = grnfromLive.Customer_code;
                            movedGRN.Route = grnfromLive.Route;
                            movedGRN.Route_Code = grnfromLive.Route_Code;
                            movedGRN.Route_Id = grnfromLive.Route_Id;
                            movedGRN.Sales_Person_Id = grnfromLive.Sales_Person_Id;
                            movedGRN.Sales_Person_Name = grnfromLive.Sales_Person_Name;
                            movedGRN.Vehicle_No = grnfromLive.Vehicle_No;
                            movedGRN.Supplier_code = grnfromLive.Supplier_code;
                            movedGRN.Supplier_Name = grnfromLive.Supplier_Name;
                            movedGRN.DC_Code = grnfromLive.DC_Code;
                            movedGRN.CreatedBy = grnfromLive.CreatedBy;
                            movedGRN.CreatedDate = grnfromLive.CreatedDate;
                            movedGRN.UpdatedBy = grnfromLive.UpdatedBy;
                            movedGRN.UpdatedDate = grnfromLive.UpdatedDate;
                            movedGRN.is_Deleted = grnfromLive.is_Deleted;
                            movedGRN.is_Syunc = true;

                            CloudDB.GRN_Creation.Add(movedGRN);
                            CloudDB.SaveChanges();

                            scope1.Complete();
                        }

                        int grnID = movedGRN.INW_Id;
                        using (var tw = new StreamWriter(Path, true))
                        {
                            tw.WriteLine(movedGRN.GRN_Number + " Inserted into GRN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                            tw.Close();
                        }

                        foreach (var lieitem in grnfromLive.GrnDetails)
                        {
                            var movedLineItem = new GRN_Line_item();
                            using (var scope3 = new TransactionScope())
                            {
                                movedLineItem.INW_id = grnID;
                                movedLineItem.GRN_Number = lieitem.GRN_Number;
                                movedLineItem.SKU_ID = lieitem.SKU_ID;
                                movedLineItem.SKU_Code = lieitem.SKU_Code;
                                movedLineItem.SKU_Name = lieitem.SKU_Name;
                                movedLineItem.SKU_SubType_Id = lieitem.SKU_SubType_Id;
                                movedLineItem.SKU_SubType = lieitem.SKU_SubType;
                                movedLineItem.UOM = lieitem.UOM;
                                movedLineItem.Tally_Status = lieitem.Tally_Status;
                                movedLineItem.PO_QTY = lieitem.PO_QTY;
                                movedLineItem.A_Accepted_Qty = lieitem.A_Accepted_Qty;
                                movedLineItem.B_Accepted_Qty = lieitem.B_Accepted_Qty;
                                movedLineItem.C_Accepted_Qty = lieitem.C_Accepted_Qty;
                                movedLineItem.A_Accepted_Price = lieitem.A_Accepted_Price;
                                movedLineItem.B_Accepted_Price = lieitem.B_Accepted_Price;
                                movedLineItem.C_Accepted_Price = lieitem.C_Accepted_Price;
                                movedLineItem.A_Converted_Qty = lieitem.A_Converted_Qty;
                                movedLineItem.B_Converted_Qty = lieitem.B_Converted_Qty;
                                movedLineItem.A_Converted_Qty = lieitem.C_Converted_Qty;
                                movedLineItem.Strinkage_Qty = lieitem.Strinkage_Qty;
                                movedLineItem.Billed_Qty = lieitem.Billed_Qty;
                                movedLineItem.Price_Book_Id = lieitem.Price_Book_Id;
                                movedLineItem.Price = lieitem.Price;
                                movedLineItem.Remark = lieitem.Remark;
                                movedLineItem.CreatedBy = lieitem.CreatedBy;
                                movedLineItem.CreatedDate = lieitem.CreatedDate;
                                movedLineItem.UpdatedBy = lieitem.UpdatedBy;
                                movedLineItem.UpdatedDate = lieitem.UpdatedDate;
                                movedLineItem.moved = false;

                                CloudDB.GRN_Line_item.Add(movedLineItem);
                                CloudDB.SaveChanges();

                                scope3.Complete();
                            }
                        }
                        lcount++;
                    }
                    else
                    {
                        GRNGetFromLocal csAnother = new GRNGetFromLocal();
                        csAnother.GRN_Single_Record_Update(grnfromLive.GRN_Number);
                    }
                }
                using (var scope4 = new TransactionScope())
                {
                    CloudDB.Stock123();
                    CloudDB.SaveChanges();

                    scope4.Complete();
                }

                int count = lcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Inserted into GRN. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }

            if (STI_PUSH_ENTITY.GRNNumGenClass != null)
            {
                int alcount = 0;
                foreach (var autoNums in STI_PUSH_ENTITY.GRNNumGenClass)
                {
                    using (var scope3 = new TransactionScope())
                    {
                        var list = CloudDB.GRN_NUM_Generation.Where(x => x.DC_Code == autoNums.DC_Code).FirstOrDefault();

                        if (list != null)
                        {
                            list.Financial_Year = autoNums.Financial_Year;
                            list.GRN_Last_Number = autoNums.GRN_Last_Number;

                            CloudDB.Entry(list).State = EntityState.Modified;
                            CloudDB.SaveChanges();
                        }

                        scope3.Complete();
                    }
                    alcount++;
                }
                int count = alcount;

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Auto Generated Numbers Updated. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
                STI_PUSH_ENTITY.GRNNumGenClass = null;
            }
        }
    }
}
