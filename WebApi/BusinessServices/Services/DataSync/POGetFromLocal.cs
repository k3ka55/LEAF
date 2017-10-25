using System;
using System.Collections.Generic;
using System.Linq;
//using DataModelLocal;
using DataModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BusinessEntities.Entity;
using BusinessEntities;
using BusinessServices.Interfaces;
using System.Data.Entity;
using System.Configuration;
using System.IO;

namespace BusinessServices.Services.DataSync
{
    public class POGetFromLocal : IPOGetFromLocal
    {
        LEAFDBEntities CloudDB = new LEAFDBEntities();
       // LEAFDBEntitiesLocal CloudDB = new LEAFDBEntitiesLocal();
        string dc_Code = ConfigurationManager.AppSettings["Local_Machine"].ToString();
        string fileName = "PO_" + DateTime.Now.ToString("dd-MM-yyyy");
        string Path;
        public POGetFromLocal()
        {
            if (fileName.Contains("-"))
                fileName = fileName.Replace("-", "_");

            Path = @"" + ConfigurationManager.AppSettings["logpath"].ToString() + fileName+".txt";

            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }

        }

        public void getPO()
        {
            using (var scope = new TransactionScope())
            {
                STI_PUSH_ENTITY.porecord = (from x in CloudDB.Purchase_Order
                                            where x.is_Deleted == false && x.is_Syunc == false && x.PO_Approval_Flag == true
                                            select new PurchaseOrderEntity
                                            {
                                                Po_id = x.Po_id,
                                                PO_Number = x.PO_Number,
                                                DC_Code = x.DC_Code,
                                                PO_RLS_date = x.PO_RLS_date,
                                                Material_Source = x.Material_Source,
                                                Material_Source_id = x.Material_Source_id,
                                                Supplier_Id = x.Supplier_Id,
                                                Supplier_Code = x.Supplier_Code,
                                                Supplier_name = x.Supplier_name,
                                                PO_Requisitioned_by = x.PO_Requisitioned_by,
                                                PO_Type = x.PO_Type,
                                                Payment_cycle = x.Payment_cycle,
                                                Payment_type = x.Payment_type,
                                                SKU_Type_Id = x.SKU_Type_Id,
                                                SKU_Type = x.SKU_Type,
                                                Delivery_Date = x.Delivery_Date,
                                                PO_raise_by = x.PO_raise_by,
                                                PO_Approve_by = x.PO_Approve_by,
                                                PO_Approval_Flag = x.PO_Approval_Flag,
                                                PO_Approved_date = x.PO_Approved_date,
                                                Status = x.Status,
                                                is_Deleted = x.is_Deleted,
                                                Reason = x.Reason,
                                                CreatedDate = x.CreatedDate,
                                                CreatedBy = x.CreatedBy,
                                                UpdatedBy = x.UpdatedBy,
                                                UpdatedDate = x.UpdatedDate,
                                                PurchaseDetails = (from y in CloudDB.Purchase_Order_Line_item
                                                                       where y.Po_id == x.Po_id
                                                                   select new PurchaseSubEntity
                                                                   {
                                                                       Po_id = y.Po_id,
                                                                       PO_Number = y.PO_Number,
                                                                       SKU_ID = y.SKU_ID,
                                                                       SKU_Code = y.SKU_Code,
                                                                       SKU_Name = y.SKU_Name,
                                                                       SKU_SubType_Id = y.SKU_SubType_Id,
                                                                       SKU_SubType = y.SKU_SubType,
                                                                       UOM = y.UOM,
                                                                       Qty = y.Qty,
                                                                       A_Grade_Price = y.A_Grade_Price,
                                                                       A_Grade_Qty = y.A_Grade_Qty,
                                                                       B_Grade_Price = y.B_Grade_Price,
                                                                       B_Grade_Qty = y.B_Grade_Qty,
                                                                       Total_Qty = y.Total_Qty.Value,
                                                                       Remark = y.Remark,
                                                                       Status = y.Status,
                                                                       CreatedBy = y.CreatedBy,
                                                                       CreatedDate = y.CreatedDate,
                                                                       UpdatedBy = y.UpdatedBy,
                                                                       UpdatedDate = y.UpdatedDate
                                                                   }).ToList()
                                            }).ToList();

                STI_PUSH_ENTITY.PONumGenClass = (from c in CloudDB.PO_NUM_Generation
                                                 where c.DC_Code == dc_Code
                                                 select new POAutoNumGen
                                                 {
                                                     PO_Num_Gen_Id = c.PO_Num_Gen_Id,
                                                     DC_Code = c.DC_Code,
                                                     Financial_Year = c.Financial_Year,
                                                     PO_Last_Number = c.PO_Last_Number
                                                 }).ToList();


                int count = STI_PUSH_ENTITY.porecord.Count;
                int alcount = STI_PUSH_ENTITY.PONumGenClass.Count;
                
                    using (var tw = new StreamWriter(Path, true))
                    {
                        tw.WriteLine(count + " Records Fetched from Purchase Order and " + alcount + " Auto Generated Numbers. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                        tw.Close();
                    }
                
                scope.Complete();
            }
        }

        public void PO_Single_Record_Update(string po_number)
        {
            using (var scope3 = new TransactionScope())
            {
                Purchase_Order dispUpdate = CloudDB.Purchase_Order.Where(x => x.PO_Number == po_number).FirstOrDefault();

                dispUpdate.is_Syunc = true;

                CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope3.Complete();

                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(po_number + " Sync Field Updated in PO. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            }
        }

        public void poMasterIDUpdate(string poNumber, int? skuid, int? skuSubtypeid, string uom, int? masterId)
        {

            using (var scope = new TransactionScope())
            {
                var updateData = CloudDB.Purchase_Order_Line_item.Where(x => x.PO_Number == poNumber && x.SKU_ID == skuid && x.SKU_SubType_Id == skuSubtypeid && x.UOM == uom).FirstOrDefault();
                updateData.Master_Id = masterId;
                CloudDB.Entry(updateData).State = EntityState.Modified;
                CloudDB.SaveChanges();

                scope.Complete();
            }
        }
        
        public void PO_Update()
        {
            int lcount = 0;

            foreach (var disp in STI_PUSH_ENTITY.porecord)
            {
                using (var scope3 = new TransactionScope()) 
                {
                    Purchase_Order dispUpdate = CloudDB.Purchase_Order.Where(x => x.Po_id == disp.Po_id).FirstOrDefault();

                    dispUpdate.is_Syunc = true;

                    CloudDB.Entry(dispUpdate).State = EntityState.Modified;
                    CloudDB.SaveChanges();

                    scope3.Complete();
                }
                lcount++;
            }

            int count = lcount;
            
                using (var tw = new StreamWriter(Path, true))
                {
                    tw.WriteLine(count + " Records Updated in Purchase Order. --" + DateTime.Now.ToString("hh:mm tt") + "\n");
                    tw.Close();
                }
            
            STI_PUSH_ENTITY.porecord = null;
        }
    }
}
