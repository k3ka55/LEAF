using DataModel;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.Entity.SqlServer;
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
using Newtonsoft.Json.Linq;


namespace BusinessServices
{
    public class INVCashServices:IINVCashServices
    {
            LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;

        public INVCashServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<INVCashEntity> Search(DateTime? startDate, DateTime? endDate, string Ulocation)
        {
            List<INVCashEntity> Result = new List<INVCashEntity>();           
            var list = (from x in DB.Invoice_Collection
                        where x.DC_Code == Ulocation && x.is_Deleted == false
                        select new INVCashEntity
                        {
                            //DC_Code = x.DC_Code,
                            //DC_Name = x.DC_Name,
                            Sales_Person_Name = x.Sales_Person_Name,
                            Customer_Code = x.Customer_Code,
                            Customer_Name = x.Customer_Name,
                            INVOICE_Number = x.INVOICE_Number,
                            INVOICE_Amount = x.INVOICE_Amount,
                            //Collected_Amount = x.Collected_Amount,
                            OutStanding_Amount = x.OutStanding_Amount,
                            INVOICE_Date = x.INVOICE_Date,                            
                        });

            if (startDate.Value != null && endDate.Value != null)
            {
                list = list.Where(x => x.INVOICE_Date.Value >= startDate.Value && x.INVOICE_Date.Value <= endDate.Value);
            }

            Result = list.ToList();

            return Result;
        }

        public Invoice_Collection_Num_Gen GetAutoIncrement(string locationId)
        {
            var autoinc = DB.Invoice_Collection_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new Invoice_Collection_Num_Gen
            {
                INV_Collection_Num_Gen_Id = autoinc.INV_Collection_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                INV_Collection_Last_Number = autoinc.INV_Collection_Last_Number
            };

            return model;
        }
        public string InvoiceCash(ArrayInvoiceCollectionEntity INVCEntity)
        {
            string poNumber = "";

            if (INVCEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var model4 = new Invoice_Collection_Excel_Tracking();
                    model4.No_of_Items = INVCEntity.INVCollection.ToList().Count();
                    model4.Uploaded_Excel_Display_Name = INVCEntity.Uploaded_Excel_Display_Name;
                    model4.Uploaded_Excel_Name = INVCEntity.Uploaded_Excel_Name;
                    model4.UploadedBy = INVCEntity.UploadedBy;
                    model4.UploadedDate = DateTime.Now;

                    _unitOfWork.InvoiceCollectionExcelTrackingRepository.Insert(model4);
                    _unitOfWork.Save();

                    int trackId = model4.INV_Collection_Excel_Tracking_id;

                    foreach (var pSub in INVCEntity.INVCollection)
                    {
                        var line = _unitOfWork.InvoiceCollectionRepository.GetByID(pSub.INV_Collection_Id);
                        var model = new Invoice_Collection();
                        if (line != null)
                        {
                            var model1 = new Invoice_Collection_Tracking();
                            model1.INV_Collection_Number = pSub.INVOICE_Number;
                            model1.DC_id = pSub.DC_id;
                            model1.DC_Code = pSub.DC_Code;
                            model1.DC_Name = pSub.DC_Name;
                            model1.Sales_Person_Id = pSub.Sales_Person_Id;
                            model1.Sales_Person_Name = pSub.Sales_Person_Name;
                            model1.Customer_Id = pSub.Customer_Id;
                            model1.Customer_Code = pSub.Customer_Code;
                            model1.Customer_Name = pSub.Customer_Name;
                            model1.INVOICE_Number = pSub.INVOICE_Number;
                            model1.INVOICE_Amount = pSub.INVOICE_Amount;
                            model1.Collected_Amount = pSub.Collected_Amount;
                            model1.OutStanding_Amount = pSub.INVOICE_Amount - pSub.Collected_Amount;
                            model1.INVOICE_Date = pSub.INVOICE_Date;
                            model1.is_Deleted = false;
                            model1.Action = "UPDATE";
                            model1.Action_DoneBy = INVCEntity.UploadedBy;
                            model1.Date_Of_Action = DateTime.Now;

                            _unitOfWork.InvoiceCollectionTrackingRepository.Insert(model1);
                            _unitOfWork.Save();

                            line.Collected_Amount = pSub.Collected_Amount;
                            line.OutStanding_Amount = pSub.INVOICE_Amount - pSub.Collected_Amount;
                            line.INV_Collection_Excel_Tracking_id = trackId;
                            line.UpdateDate = DateTime.Now;
                            line.UpdateBy = pSub.UpdateBy;

                            _unitOfWork.InvoiceCollectionRepository.Update(line);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            string prefix, locationId, INVCNumber, FY;
                            int? incNumber;
                            string locationID = pSub.DC_Code;
                            ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                            prefix = rm.GetString("INVCT");
                            Invoice_Collection_Num_Gen autoIncNumber = GetAutoIncrement(locationID);
                            locationId = autoIncNumber.DC_Code;
                            FY = autoIncNumber.Financial_Year;
                            incNumber = autoIncNumber.INV_Collection_Last_Number;
                            int? incrementedValue = incNumber + 1;
                            var POincrement = DB.Invoice_Collection_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                            POincrement.INV_Collection_Last_Number = incrementedValue;
                            _unitOfWork.InvoiceCollectionNumGenRepository.Update(POincrement);
                            _unitOfWork.Save();

                            INVCNumber = prefix + "/" + locationId + "/" + FY + "/" + String.Format("{0:00000}", incNumber);
                           
                            var model2 = new Invoice_Collection_Tracking();
                            model2.INV_Collection_Number = INVCNumber;
                            model2.DC_id = pSub.DC_id;
                            model2.DC_Code = pSub.DC_Code;
                            model2.DC_Name = pSub.DC_Name;
                            model2.Sales_Person_Id = pSub.Sales_Person_Id;
                            model2.Sales_Person_Name = pSub.Sales_Person_Name;
                            model2.Customer_Id = pSub.Customer_Id;
                            model2.Customer_Code = pSub.Customer_Code;
                            model2.Customer_Name = pSub.Customer_Name;
                            model2.INVOICE_Number = pSub.INVOICE_Number;
                            model2.INVOICE_Amount = pSub.INVOICE_Amount;
                            model2.Collected_Amount = pSub.Collected_Amount;
                            model2.OutStanding_Amount = pSub.INVOICE_Amount - pSub.Collected_Amount;
                            model2.INVOICE_Date = pSub.INVOICE_Date;
                            model2.is_Deleted = false;
                            model2.Action = "CREATE";
                            model2.Action_DoneBy = INVCEntity.UploadedBy;
                            model2.Date_Of_Action = DateTime.Now;


                            _unitOfWork.InvoiceCollectionTrackingRepository.Insert(model2);
                            _unitOfWork.Save();



                            model.INV_Collection_Number = INVCNumber;
                            model.DC_id = pSub.DC_id;
                            model.DC_Code = pSub.DC_Code;
                            model.DC_Name = pSub.DC_Name;
                            model.Sales_Person_Id = pSub.Sales_Person_Id;
                            model.Sales_Person_Name = pSub.Sales_Person_Name;
                            model.Customer_Id = pSub.Customer_Id;
                            model.Customer_Code = pSub.Customer_Code;
                            model.Customer_Name = pSub.Customer_Name;
                            model.INVOICE_Number = pSub.INVOICE_Number;
                            model.INVOICE_Amount = pSub.INVOICE_Amount;
                            model.Collected_Amount = pSub.Collected_Amount;
                            model.OutStanding_Amount = pSub.INVOICE_Amount - pSub.Collected_Amount;
                            model.INVOICE_Date = pSub.INVOICE_Date;
                            model.INV_Collection_Excel_Tracking_id = trackId;
                            model.is_Deleted=false;
                            model.CreatedDate = DateTime.Now;
                            model.CreateBy = INVCEntity.UploadedBy;

                            _unitOfWork.InvoiceCollectionRepository.Insert(model);
                            _unitOfWork.Save();
                        }
                    }
                    scope.Complete();
                }
            }
            return poNumber;
        }






    }
}
