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
using System.IO;

namespace BusinessServices
{
    public class DispatchServices : IDispatchServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public readonly string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Areas/DispatchAccepted.json");
        public DispatchServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public string DispatchAccept(DispatchAcceptedEntity requestType)
        {
            string response="";
            foreach (var t in requestType.LineItems)
            { 
            string json = File.ReadAllText(filepath);
            List<DispatchAcceptedLineItemEntity> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DispatchAcceptedLineItemEntity>>(json);
            DispatchAcceptedLineItemEntity GetList = new DispatchAcceptedLineItemEntity();       

            if (jsonObj == null)
            {
                jsonObj = new List<DispatchAcceptedLineItemEntity>();               
            }                        
                GetList.Auto_Inc = jsonObj.Count + 1;
                GetList.Dispatch_Id = requestType.Dispatch_Id;
                GetList.OTP = requestType.OTP;
                GetList.Dispatch_Line_Id = t.Dispatch_Line_Id;
                GetList.Accepted_Qty = t.Accepted_Qty;
                GetList.Dispatch_Line_Id = t.Dispatch_Line_Id;
                jsonObj.Add(GetList);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filepath, output);
               
            }
                                             
            response = "Success";
            return response;

        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {

            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        public bool CompareOTP(int Dispatch_Id, string OTP)
        {
         bool output=false;
         if(Dispatch_Id>0 && OTP !=null && OTP !="null")
         {
             string json = File.ReadAllText(filepath);
             List<OTPWriteEntity> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OTPWriteEntity>>(json);
             List<OTPWriteEntity> RquestType = new List<OTPWriteEntity>();
             foreach (var requestType in jsonObj)
             {
                 OTPWriteEntity RequestTypeMaster = new OTPWriteEntity();
                 RequestTypeMaster.CDN_Number = requestType.CDN_Number;
                 RequestTypeMaster.Dispatch_Id = requestType.Dispatch_Id;
                 RequestTypeMaster.OTP = requestType.OTP;                       
                 RquestType.Add(RequestTypeMaster);
             }
             var result = RquestType.Where(a => a.Dispatch_Id == Dispatch_Id && a.OTP == OTP).ToList();
             if(result.ToList().Count>0)
             {
                 output = true;
             }
         }
          
            return output;
        }

        public string send()
        {            
            string sUser = "rams";
            string spwd = "Meta@2803";
            string sNumber = "8220800268";
            string sMessage = "New SMS Gateway";
            string sSenderID = "SMSHUB";
            string sURL = "http://cloud.smsindiahub.in/vendorsms/pushsms.aspx?user=" + sUser + "&password=" +
            spwd + "&msisdn=" + sNumber + "&sid=" + sSenderID + "&msg=" + sMessage + "&fl=0";
            string sResponse = GetResponse(sURL);
            return sResponse;
        }
        public static string GetResponse(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch
            {
                return "";
            }
        } 
       
        public string SendSMS(string Customer_Number, int Dispatch_Id, string CDN_Number)
        {

            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);

            string sUser = "rams";
            string spwd = "Meta@2803";
            string sNumber = Customer_Number.ToString();
                //"8344231019";

            string sResponse = "NOTSEND";
                //"8220800268";
            String sMessage = "One Time Password for your Order ["+CDN_Number+" ] in LEAF is "+sRandomOTP;
                   //"One Time Password for your Order [" + CDN_Number + "] in LEAF is " + sRandomOTP;
                                //
            string sSenderID = "SMSHUB";
            string sURL = "http://cloud.smsindiahub.in/vendorsms/pushsms.aspx?user=" + sUser + "&password=" + spwd + "&msisdn=" + sNumber + "&sid=" + sSenderID + "&msg=" + sMessage + "&fl=0&gwid=2";
            sResponse = GetResponse(sURL);                     
            string json = File.ReadAllText(filepath);
            List<OTPWriteEntity> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OTPWriteEntity>>(json);
            OTPWriteEntity GetList = new OTPWriteEntity();      

            if (jsonObj == null)
            {
                jsonObj = new List<OTPWriteEntity>();
            }                          
                GetList.Auto_Inc = jsonObj.Count + 1;
                GetList.Dispatch_Id = Dispatch_Id;
                GetList.OTP = sRandomOTP;
                GetList.CDN_Number = CDN_Number;
               
                jsonObj.Add(GetList);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filepath, output);

                return sResponse;
        }
        //public int SendSMS(int Customer_Number, int Dispatch_Id, string CDN_Number)
        //{

        //    string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        //    string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);
        //    // String AccountID, String Email, String Password, String Recipient, String Message
        //    WebClient Client = new WebClient();
        //    String RequestURL, RequestData;
        //    String AccountID = "CI00194855";
        //    String Email = "k3ka55@gmail.com";
        //    String Password = "ABC@123";
        //    // Customer_Number.ToString()
        //    String Recipient = "8220800268";
        //    //"8344231019";
        //    String Message = "One Time Password for your Order [" + CDN_Number + "] in LEAF is " + sRandomOTP + ".";
        //    RequestURL = "https://redoxygen.net/sms.dll?Action=SendSMS";

        //    RequestData = "AccountId=" + AccountID
        //        + "&Email=" + System.Web.HttpUtility.UrlEncode(Email)
        //        + "&Password=" + System.Web.HttpUtility.UrlEncode(Password)
        //        + "&Recipient=" + System.Web.HttpUtility.UrlEncode(Recipient)
        //        + "&Message=" + System.Web.HttpUtility.UrlEncode(Message);

        //    byte[] PostData = Encoding.ASCII.GetBytes(RequestData);
        //    byte[] Response = Client.UploadData(RequestURL, PostData);

        //    String Result = Encoding.ASCII.GetString(Response);
        //    int ResultCode = System.Convert.ToInt32(Result.Substring(0, 4));


        //    string json = File.ReadAllText(filepath);
        //    List<OTPWriteEntity> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OTPWriteEntity>>(json);
        //    OTPWriteEntity GetList = new OTPWriteEntity();


        //    if (jsonObj == null)
        //    {
        //        jsonObj = new List<OTPWriteEntity>();
        //        //GetList.Auto_Inc = jsonObj.Count + 1;
        //    }

        //    GetList.Auto_Inc = jsonObj.Count + 1;
        //    GetList.Dispatch_Id = Dispatch_Id;
        //    GetList.OTP = sRandomOTP;
        //    GetList.CDN_Number = CDN_Number;

        //    jsonObj.Add(GetList);
        //    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
        //    File.WriteAllText(filepath, output);

        //    return ResultCode;
        //}
        public bool DispatchUpdateById(DispatchUpdateEntity DispatchUpdateEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                foreach (DLIDs lst in DispatchUpdateEntity.DispatchLIds)
                {
                    Dispatch_Line_item lineItem = _unitOfWork.DispatchSubRepository.GetByID(lst.Dispatch_LineId);

                    if (lineItem != null)
                    {
                        if (lst.statusflag == 0)
                        {
                            lineItem.Status = "Closed";
                            _unitOfWork.DispatchSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 1)
                        {
                            lineItem.Status = "Partial";
                            _unitOfWork.DispatchSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 2)
                        {
                            lineItem.Status = "Exceed";
                            _unitOfWork.DispatchSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }

                    }
                }
                Dispatch_Creation stn = DB.Dispatch_Creation.Where(x => x.Stock_Xfer_Dispatch_Number == DispatchUpdateEntity.id || x.Customer_Dispatch_Number == DispatchUpdateEntity.id).FirstOrDefault();

                if (stn != null)
                {
                    stn.Status = "Closed";
                    DB.Entry(stn).State = EntityState.Modified;
                    DB.SaveChanges();
                }

                scope.Complete();
                success = true;
            }
            return success;
        }
        public bool UpdateCDNAcceptesSts(DispatchUpdateEntity DispatchUpdateEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                foreach (DLIDs lst in DispatchUpdateEntity.DispatchLIds)
                {
                    Dispatch_Line_item lineItem = _unitOfWork.DispatchSubRepository.GetByID(lst.Dispatch_LineId);

                    if (lineItem != null)
                    {
                        if (lst.statusflag == 0)
                        {
                            lineItem.Acceptance_Status = "Closed";
                            _unitOfWork.DispatchSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 1)
                        {
                            lineItem.Acceptance_Status = "Partial";
                            _unitOfWork.DispatchSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }
                        else if (lst.statusflag == 2)
                        {
                            lineItem.Acceptance_Status = "Exceed";
                            _unitOfWork.DispatchSubRepository.Update(lineItem);
                            _unitOfWork.Save();
                            success = true;
                        }

                    }
                }
                using (var fscope = new TransactionScope())
                {
                    success = false;
                    //
                    DispatchEntity list = (from x in DB.Dispatch_Creation
                                             where x.Customer_Dispatch_Number == DispatchUpdateEntity.id
                                             select new DispatchEntity
                                             {
                                                 Dispatch_Id = x.Dispatch_Id,
                                                 Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                 Acceptance_Status = x.Acceptance_Status,
                                                 Line_Items = (from a in DB.Dispatch_Line_item
                                                              where a.Dispatch_Id == DispatchUpdateEntity.Dispatch_Id
                                                              select new DispatchLineItemsEntity
                                                              {
                                                                  Dispatch_Line_Id = a.Dispatch_Line_Id,
                                                                  Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                                  Acceptance_Status = a.Acceptance_Status
                                                              }).ToList(),
                                             }).FirstOrDefault();

                    if (list != null)
                    {
                        string state = "Closed";
                        string dspNum = "";

                        foreach (DispatchLineItemsEntity csi in list.Line_Items)
                        {
                            dspNum = csi.Customer_Dispatch_Number;

                            if (csi.Acceptance_Status == "Open" || csi.Acceptance_Status == "Partial")
                            {
                                state = "Open";
                            }
                        }
                        //
                        if (dspNum != "" && dspNum != null && dspNum!="null")
                        {
                            if (state == "Open")
                            {
                                Dispatch_Creation cs = DB.Dispatch_Creation.Where(x => x.Customer_Dispatch_Number == dspNum).FirstOrDefault();

                                if (cs != null)
                                {
                                    cs.Acceptance_Status = "Open";
                                    DB.Entry(cs).State = EntityState.Modified;
                                    DB.SaveChanges();
                                }
                            }
                            else if (state == "Closed")
                            {
                                Dispatch_Creation cs = DB.Dispatch_Creation.Where(x => x.Customer_Dispatch_Number == dspNum).FirstOrDefault();
                                if (cs != null)
                                {
                                    cs.Acceptance_Status = "Closed";
                                    DB.Entry(cs).State = EntityState.Modified;
                                    DB.SaveChanges();
                                }
                            }
                        }
                        //
                    }
                    fscope.Complete();
                    success = true;
                }

                scope.Complete();
                success = true;
            }
            return success;
        }
       

        public List<cdnNumber> GetCdnNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Dispatch_Creation
                         where x.Indent_Rls_Date.Value.Year == date.Value.Year && x.Indent_Rls_Date.Value.Month == date.Value.Month && x.Indent_Rls_Date.Value.Day == date.Value.Day && x.is_Deleted == false && x.Dispatch_Location_Code == Ulocation && x.Status == "Open"
                         select new cdnNumber
                         {
                             CDN_Number = x.Customer_Dispatch_Number
                         }).ToList();
            return query;
        }

        public bool DeleteDispatch(int dipId)
        {
            var success = false;
            if (dipId > 0)
            {
                using (var scope = new TransactionScope())
                {


                    var g = _unitOfWork.DispatchRepository.GetByID(dipId);
                    if (g != null)
                    {
                        g.is_Deleted = true;


                        DateTime Today = DateTime.Now;
                        var check = (from ee in DB.Stirnkage_Summary
                                     where ee.DC_Code == g.Dispatch_Location_Code
                                     && ee.CreatedDate.Value.Year == Today.Year
                                      && ee.CreatedDate.Value.Month == Today.Month
                                       && ee.CreatedDate.Value.Day == Today.Day
                                       && ee.Adjustment_Freeze == false
                                     select ee).FirstOrDefault();
                        if (check != null)
                        {
                            check.Adjustment_Freeze = true;
                            DB.Entry(check).State = EntityState.Modified;
                            DB.SaveChanges();
                        }


                        _unitOfWork.DispatchRepository.Update(g);
                        _unitOfWork.Save();

                        DB.dispatchtostock_add(dipId);
                        DB.SaveChanges();

                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public string UpdateDispatch(int Dispatch_Id, DispatchEntity dispatchEntity)
        {

            string DispatchNumber = "";
            if (dispatchEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.DispatchRepository.GetByID(Dispatch_Id);
                    if (p != null)
                    {

                        p.Dispatched_Location_ID = dispatchEntity.Dispatched_Location_ID;
                        p.Dispatch_Location_Code = dispatchEntity.Dispatch_Location_Code;
                        p.Customer_Id = dispatchEntity.Customer_Id;
                        p.Customer_code = dispatchEntity.Customer_code;
                        p.STI_Id = dispatchEntity.STI_Id;
                        p.STI_Number = dispatchEntity.STI_Number;
                        p.CSI_Id = dispatchEntity.CSI_Id;
                        p.Order_Reference = dispatchEntity.Order_Reference;
                        p.CSI_Number = dispatchEntity.CSI_Number;
                        p.Customer_Name = dispatchEntity.Customer_Name;
                        p.Indent_Rls_Date = dispatchEntity.Indent_Rls_Date;
                        p.SKU_Type_Id = dispatchEntity.SKU_Type_Id;
                        p.SKU_Type = dispatchEntity.SKU_Type;
                        p.Delievery_Type = dispatchEntity.Delievery_Type;
                        p.Delivery_Date = dispatchEntity.Delivery_Date;
                        p.Dispatch_Type = dispatchEntity.Dispatch_Type;
                        p.Delivery_done_by = dispatchEntity.Delivery_done_by;
                        p.Dispatch_time_stamp = dispatchEntity.Dispatch_time_stamp;
                        p.Delivery_Location_ID = dispatchEntity.Delivery_Location_ID;
                        p.Delivery_Location_Code = dispatchEntity.Delivery_Location_Code;
                        p.Expected_Delivery_date = dispatchEntity.Expected_Delivery_date;
                        p.Expected_Delivery_time = dispatchEntity.Expected_Delivery_time;
                        p.Remark = dispatchEntity.Remark;
                        p.Status = "Open";
                        p.Vehicle_No = dispatchEntity.Vehicle_No;
                        p.Sales_Person_Id = dispatchEntity.Sales_Person_Id;
                        p.Sales_Person_Name = dispatchEntity.Sales_Person_Name;
                        p.Route_Id = dispatchEntity.Route_Id;
                        p.Route_Code = dispatchEntity.Route_Code;
                        p.Route = dispatchEntity.Route;
                        p.Invoice_Flag = null;
                        p.Price_Template_ID = dispatchEntity.Price_Template_ID;
                        p.Price_Template_Code = dispatchEntity.Price_Template_Code;
                        p.Price_Template_Name = dispatchEntity.Price_Template_Name;
                        p.Price_Template_Valitity_upto = dispatchEntity.Price_Template_Valitity_upto;
                        p.UpdateDate = DateTime.Now;
                        p.UpdateBy = dispatchEntity.UpdateBy;
                        p.is_Deleted = false;
                        p.is_Syunc = false;
                        _unitOfWork.DispatchRepository.Update(p);
                        _unitOfWork.Save();
                        DB.dispatchtostock_add(Dispatch_Id);
                        DB.SaveChanges();

                    }
                    DateTime Today = DateTime.Now;
                    var check = (from ee in DB.Stirnkage_Summary
                                 where ee.DC_Code == dispatchEntity.Dispatch_Location_Code
                                 && ee.CreatedDate.Value.Year == Today.Year
                                  && ee.CreatedDate.Value.Month == Today.Month
                                   && ee.CreatedDate.Value.Day == Today.Day
                                   && ee.Adjustment_Freeze == false
                                 select ee).FirstOrDefault();
                    if (check != null)
                    {
                        check.Adjustment_Freeze = true;
                        DB.Entry(check).State = EntityState.Modified;
                        DB.SaveChanges();
                    }

                    var lineItemList = DB.Dispatch_Line_item.Where(x => x.Dispatch_Id == Dispatch_Id).ToList();
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var list = _unitOfWork.DispatchSubRepository.GetByID(li.Dispatch_Line_Id);

                            if (list != null)
                            {
                                _unitOfWork.DispatchSubRepository.Delete(list);
                                _unitOfWork.Save();
                            }

                            var grnLineItems = DB.Dispatch_SKU_Line_Items.Where(f => f.Dispatch_Line_Id == li.Dispatch_Line_Id).ToList();
                            foreach (var litem in grnLineItems)
                            {
                                DB.Dispatch_SKU_Line_Items.Remove(litem);
                                DB.SaveChanges();
                            }

                            scope1.Complete();
                        }
                    }




                    if (dispatchEntity.Dispatch_Type == "Customer")
                    {
                        DispatchNumber = dispatchEntity.Customer_Dispatch_Number;
                    }
                    else if (dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                    {
                        DispatchNumber = dispatchEntity.Stock_Xfer_Dispatch_Number;
                    }

                    string dispatchType = dispatchEntity.Dispatch_Type;

                    var model = new Dispatch_Line_item();

                    foreach (var pSub in dispatchEntity.Line_Items)
                    {

                        //------------------------------------ latest calculation for stock and grn -----------------------

                        double? ActualValue = 0.0;

                        if (pSub.UOM == "Kgs" && dispatchEntity.Dispatch_Type == "Customer")
                        {
                            ActualValue = pSub.Dispatch_Qty;
                        }
                        else if (pSub.UOM == "Unit" && dispatchEntity.Dispatch_Type == "Customer")
                        {
                            ActualValue = pSub.Converted_Unit_Value;
                        }
                        else if (pSub.UOM == "Kgs" && dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                        {
                            ActualValue = pSub.Dispatch_Qty;
                        }
                        else if (pSub.UOM == "Unit" && dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                        {
                            ActualValue = pSub.Dispatch_Qty;
                        }

                        if (ActualValue == 0)
                        {
                            model.Dispatch_Id = Dispatch_Id;
                            model.SKU_ID = pSub.SKU_ID;
                            model.SKU_Code = pSub.SKU_Code;
                            model.SKU_Name = pSub.SKU_Name;
                            model.Pack_Type_Id = pSub.Pack_Type_Id;
                            model.Pack_Type = pSub.Pack_Type;
                            model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                            model.Dispatch_Pack_Type = pSub.Dispatch_Pack_Type;
                            model.SKU_SubType = pSub.SKU_SubType;
                            model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                            model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                            model.Pack_Size = pSub.Pack_Size;
                            model.HSN_Code = pSub.HSN_Code;
                            model.Total_GST = pSub.Total_GST;
                            model.CGST = pSub.CGST;
                            model.SGST = pSub.SGST;
                            model.UOM = pSub.UOM;
                            model.Grade = pSub.Grade;
                            model.Status = "Open";
                            model.Strinkage_Qty = pSub.Strinkage_Qty;
                            model.Indent_Qty = pSub.Indent_Qty;
                            model.Dispatch_Qty = pSub.Dispatch_Qty;
                            model.Dispatch_Value = pSub.Dispatch_Value;
                            model.Received_Qty = pSub.Received_Qty;
                            model.Accepted_Qty = pSub.Accepted_Qty;
                            model.Unit_Rate = pSub.Unit_Rate;
                            model.Dispatch_Value = pSub.Dispatch_Value;
                            model.No_of_Packed_Item = pSub.No_of_Packed_Item;
                            model.is_Stk_Update = false;
                            model.is_Deleted = false;
                            model.Remark = pSub.Remark;
                            model.CreatedDate = DateTime.Now;
                            model.CreateBy = pSub.CreateBy;
                            model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                            _unitOfWork.DispatchSubRepository.Insert(model);
                            _unitOfWork.Save();

                            int grnLineId = model.Dispatch_Line_Id;

                            int dispatchLineId = model.Dispatch_Line_Id;
                            var skuLSub = new Dispatch_SKU_Line_Items();
                            foreach (var skuSub in pSub.DispatchSKULineItems.Take(1))
                            {
                                skuLSub.Dispatch_Line_Id = dispatchLineId;
                                skuLSub.SKU_Id = skuSub.SKU_Id;
                                skuLSub.Dispatch_Line_Id = dispatchLineId;
                                skuLSub.Dispatch_Qty = skuSub.Dispatch_Qty;
                                skuLSub.Barcode = skuSub.Barcode;
                                skuLSub.Batch_Number = skuSub.Batch_Number;
                                skuLSub.UOM = skuSub.UOM;
                                skuLSub.CreatedDate = DateTime.Now;
                                skuLSub.CreatedBy = skuSub.CreatedBy;
                                skuLSub.is_Deleted = false;

                                _unitOfWork.DispatchSKULineItemRepository.Insert(skuLSub);
                                _unitOfWork.Save();



                            }

                            int Dispatch_SKU_Line_Items_Id_Ref = skuLSub.Dispatch_SKU_Line_Items_Id;

                            foreach (var aSub in pSub.DispatchLineItemConsumables.Take(1))
                            {
                                var consumable = new Dispatch_Consumables();

                                consumable.Dispatch_Line_Id_FK = model.Dispatch_Line_Id;
                                consumable.Dispatch_SKU_Line_Items_Id_FK = Dispatch_SKU_Line_Items_Id_Ref;
                                consumable.SKU_Id = aSub.SKU_Id;
                                consumable.Consumable_Qty = aSub.Consumable_Qty;
                                consumable.Grade = aSub.Grade;
                                consumable.UOM = aSub.UOM;
                                consumable.CreatedDate = DateTime.UtcNow;
                                consumable.CreatedBy = dispatchEntity.CreateBy;
                                consumable.is_Deleted = false;
                                _unitOfWork.DispatchConsumablesRepository.Insert(consumable);
                                _unitOfWork.Save();

                            }
                        }
                        else
                        {
                            model.Dispatch_Id = Dispatch_Id;
                            model.SKU_ID = pSub.SKU_ID;
                            model.SKU_Code = pSub.SKU_Code;
                            model.SKU_Name = pSub.SKU_Name;
                            model.Pack_Type_Id = pSub.Pack_Type_Id;
                            model.Pack_Type = pSub.Pack_Type;
                            model.HSN_Code = pSub.HSN_Code;
                            model.Total_GST = pSub.Total_GST;
                            model.CGST = pSub.CGST;
                            model.SGST = pSub.SGST;
                            model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                            model.Dispatch_Pack_Type = pSub.Dispatch_Pack_Type;
                            model.SKU_SubType = pSub.SKU_SubType;
                            model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                            model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                            model.Pack_Size = pSub.Pack_Size;
                            model.UOM = pSub.UOM;
                            model.Grade = pSub.Grade;
                            model.Status = "Open";
                            model.Indent_Qty = pSub.Indent_Qty;
                            model.Dispatch_Qty = pSub.Dispatch_Qty;
                            model.Strinkage_Qty = pSub.Strinkage_Qty;
                            model.Received_Qty = pSub.Received_Qty;
                            model.Accepted_Qty = pSub.Accepted_Qty;
                            model.Dispatch_Value = pSub.Dispatch_Value;
                            model.Unit_Rate = pSub.Unit_Rate;
                            model.Dispatch_Value = pSub.Dispatch_Value;
                            model.No_of_Packed_Item = pSub.No_of_Packed_Item;
                            model.is_Stk_Update = false;
                            model.is_Deleted = false;
                            model.Remark = pSub.Remark;
                            model.CreatedDate = DateTime.Now;
                            model.CreateBy = pSub.CreateBy;
                            model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                            _unitOfWork.DispatchSubRepository.Insert(model);
                            _unitOfWork.Save();

                            int grnLineId = model.Dispatch_Line_Id;

                            int dispatchLineId = model.Dispatch_Line_Id;
                            var skuLSub = new Dispatch_SKU_Line_Items();
                            foreach (var skuSub in pSub.DispatchSKULineItems.Take(1))
                            {
                                skuLSub.Dispatch_Line_Id = dispatchLineId;
                                skuLSub.SKU_Id = skuSub.SKU_Id;
                                skuLSub.Dispatch_Line_Id = dispatchLineId;
                                skuLSub.Dispatch_Qty = skuSub.Dispatch_Qty;
                                skuLSub.Barcode = skuSub.Barcode;
                                skuLSub.Batch_Number = skuSub.Batch_Number;
                                skuLSub.UOM = skuSub.UOM;
                                skuLSub.CreatedDate = DateTime.Now;
                                skuLSub.CreatedBy = skuSub.CreatedBy;
                                skuLSub.is_Deleted = false;

                                _unitOfWork.DispatchSKULineItemRepository.Insert(skuLSub);
                                _unitOfWork.Save();

                            }

                            int Dispatch_SKU_Line_Items_Id_Ref = skuLSub.Dispatch_SKU_Line_Items_Id;

                            foreach (var aSub in pSub.DispatchLineItemConsumables.Take(1))
                            {

                                var consumable = new Dispatch_Consumables();

                                consumable.Dispatch_Line_Id_FK = model.Dispatch_Line_Id;
                                consumable.Dispatch_SKU_Line_Items_Id_FK = Dispatch_SKU_Line_Items_Id_Ref;
                                consumable.SKU_Id = aSub.SKU_Id;
                                consumable.Consumable_Qty = aSub.Consumable_Qty;
                                consumable.Grade = aSub.Grade;
                                consumable.UOM = aSub.UOM;
                                consumable.CreatedDate = DateTime.UtcNow;
                                consumable.CreatedBy = dispatchEntity.CreateBy;
                                consumable.is_Deleted = false;
                                _unitOfWork.DispatchConsumablesRepository.Insert(consumable);
                                _unitOfWork.Save();

                            }

                            int DLineId = model.Dispatch_Line_Id;

                            List<Stock> line = DB.Stocks.Where(s => s.SKU_Name == pSub.SKU_Name && s.SKU_Type == dispatchEntity.SKU_Type && s.DC_Code == dispatchEntity.Dispatch_Location_Code && s.Grade == pSub.Grade).Select(a => a).ToList();
                            foreach (Stock m in line)
                            {
                                if (line != null)
                                {
                                    double? cQTY = m.Closing_Qty;

                                    var dispatchSupplier = new Dispatch_Supplier_Track
                                    {
                                        Dispatch_Id = Dispatch_Id,
                                        Dispatch_Number = DispatchNumber,
                                        Dispatch_Line_Id = DLineId,
                                        Stock_ID = m.Stock_Id,
                                        Stock_code = m.Stock_code,
                                        SKU_Code = m.SKU_Code,
                                        SKU_name = m.SKU_Name,
                                        Grade = m.Grade,
                                        SKU_SubType_Id = pSub.SKU_SubType_Id,
                                        SKU_SubType = pSub.SKU_SubType,
                                        Pack_Type_Id = pSub.Pack_Type_Id,
                                        Pack_Type = pSub.Pack_Type,
                                        Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id,
                                        Pack_Weight_Type = pSub.Pack_Weight_Type,
                                        Pack_Size = pSub.Pack_Size,
                                        Dispatch_pack_Type = pSub.Dispatch_Pack_Type,
                                        QTY = cQTY,
                                        Billed_Qty = cQTY,
                                        Price = pSub.Unit_Rate,
                                        CreatedDate = DateTime.Now,
                                    };

                                    _unitOfWork.DispatchSupplierTrackRepository.Insert(dispatchSupplier);
                                    _unitOfWork.Save();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }


                    }



                    DB.dispatch_proc();
                    DB.SaveChanges();



                    scope.Complete();
                }
            }
            return DispatchNumber;
        }
        public string UpdateDispatchAcceptedQty(int Dispatch_Id, DispatchEntity dispatchEntity)
        {

            string DispatchNumber = "";
            if (dispatchEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var pSub in dispatchEntity.Line_Items)
                    {
                        var p = _unitOfWork.DispatchSubRepository.GetByID(pSub.Dispatch_Line_Id);
                        if (p != null)
                        {
                            p.Return_Qty = p.Dispatch_Qty - pSub.Accepted_Qty;
                            p.Accepted_Qty = pSub.Accepted_Qty;
                            if (pSub.Accepted_Qty==0)
                            {
                                p.Return_Qty = p.Dispatch_Qty;
                            }
                            _unitOfWork.DispatchSubRepository.Update(p);
                            _unitOfWork.Save();
                        }
                    }
                    //
                    DispatchNumber = dispatchEntity.Customer_Dispatch_Number;
                    string json = File.ReadAllText(filepath);
                    List<OTPWriteEntity> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OTPWriteEntity>>(json);
                    List<OTPWriteEntity> TicketStatusList = new List<OTPWriteEntity>();

                    jsonObj.RemoveAll(d => d.Dispatch_Id == Dispatch_Id);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(filepath, output);

                    scope.Complete();
                }
            }
            return dispatchEntity.Customer_Dispatch_Number;
        }
        //DateTime Today = DateTime.Now;
        //var check = (from ee in DB.Stirnkage_Summary
        //             where ee.DC_Code == dispatchEntity.Dispatch_Location_Code
        //             && ee.CreatedDate.Value.Year == Today.Year
        //              && ee.CreatedDate.Value.Month == Today.Month
        //               && ee.CreatedDate.Value.Day == Today.Day
        //               && ee.Adjustment_Freeze == false
        //             select ee).FirstOrDefault();
        //if (check != null)
        //{
        //    check.Adjustment_Freeze = true;
        //    DB.Entry(check).State = EntityState.Modified;
        //    DB.SaveChanges();
        //}

        //var lineItemList = DB.Dispatch_Line_item.Where(x => x.Dispatch_Id == Dispatch_Id).ToList();
        //foreach (var li in lineItemList)
        //{
        //    using (var scope1 = new TransactionScope())
        //    {
        //        var list = _unitOfWork.DispatchSubRepository.GetByID(li.Dispatch_Line_Id);

        //        if (list != null)
        //        {
        //            _unitOfWork.DispatchSubRepository.Delete(list);
        //            _unitOfWork.Save();
        //        }

        //        var grnLineItems = DB.Dispatch_SKU_Line_Items.Where(f => f.Dispatch_Line_Id == li.Dispatch_Line_Id).ToList();
        //        foreach (var litem in grnLineItems)
        //        {
        //            DB.Dispatch_SKU_Line_Items.Remove(litem);
        //            DB.SaveChanges();
        //        }

        //        scope1.Complete();
        //    }
        //}




        //if (dispatchEntity.Dispatch_Type == "Customer")
        //{
        //    DispatchNumber = dispatchEntity.Customer_Dispatch_Number;
        //}
        //else if (dispatchEntity.Dispatch_Type == "DC Stock Transfer")
        //{
        //    DispatchNumber = dispatchEntity.Stock_Xfer_Dispatch_Number;
        //}

        //string dispatchType = dispatchEntity.Dispatch_Type;
                        ////------------------------------------ latest calculation for stock and grn -----------------------

                        //double? ActualValue = 0.0;

                        //if (pSub.UOM == "Kgs" && dispatchEntity.Dispatch_Type == "Customer")
                        //{
                        //    ActualValue = pSub.Dispatch_Qty;
                        //}
                        //else if (pSub.UOM == "Unit" && dispatchEntity.Dispatch_Type == "Customer")
                        //{
                        //    ActualValue = pSub.Converted_Unit_Value;
                        //}
                        //else if (pSub.UOM == "Kgs" && dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                        //{
                        //    ActualValue = pSub.Dispatch_Qty;
                        //}
                        //else if (pSub.UOM == "Unit" && dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                        //{
                        //    ActualValue = pSub.Dispatch_Qty;
                        //}

                        //if (ActualValue == 0)
                        //{
                     
                            //int grnLineId = model.Dispatch_Line_Id;

                            //int dispatchLineId = model.Dispatch_Line_Id;
                            //var skuLSub = new Dispatch_SKU_Line_Items();
                            //foreach (var skuSub in pSub.DispatchSKULineItems.Take(1))
                            //{
                            //    skuLSub.Dispatch_Line_Id = dispatchLineId;
                            //    skuLSub.SKU_Id = skuSub.SKU_Id;
                            //    skuLSub.Dispatch_Line_Id = dispatchLineId;
                            //    skuLSub.Dispatch_Qty = skuSub.Dispatch_Qty;
                            //    skuLSub.Barcode = skuSub.Barcode;
                            //    skuLSub.Batch_Number = skuSub.Batch_Number;
                            //    skuLSub.UOM = skuSub.UOM;
                            //    skuLSub.CreatedDate = DateTime.Now;
                            //    skuLSub.CreatedBy = skuSub.CreatedBy;
                            //    skuLSub.is_Deleted = false;

                            //    _unitOfWork.DispatchSKULineItemRepository.Insert(skuLSub);
                            //    _unitOfWork.Save();



                            //}

                            //int Dispatch_SKU_Line_Items_Id_Ref = skuLSub.Dispatch_SKU_Line_Items_Id;

                            //foreach (var aSub in pSub.DispatchLineItemConsumables.Take(1))
                            //{
                            //    var consumable = new Dispatch_Consumables();

                            //    consumable.Dispatch_Line_Id_FK = model.Dispatch_Line_Id;
                            //    consumable.Dispatch_SKU_Line_Items_Id_FK = Dispatch_SKU_Line_Items_Id_Ref;
                            //    consumable.SKU_Id = aSub.SKU_Id;
                            //    consumable.Consumable_Qty = aSub.Consumable_Qty;
                            //    consumable.Grade = aSub.Grade;
                            //    consumable.UOM = aSub.UOM;
                            //    consumable.CreatedDate = DateTime.UtcNow;
                            //    consumable.CreatedBy = dispatchEntity.CreateBy;
                            //    consumable.is_Deleted = false;
                            //    _unitOfWork.DispatchConsumablesRepository.Insert(consumable);
                            //    _unitOfWork.Save();

                            //}
                       // }
                        //else
                        //{
                        //    model.Dispatch_Id = Dispatch_Id;
                        //    model.SKU_ID = pSub.SKU_ID;
                        //    model.SKU_Code = pSub.SKU_Code;
                        //    model.SKU_Name = pSub.SKU_Name;
                        //    model.Pack_Type_Id = pSub.Pack_Type_Id;
                        //    model.Pack_Type = pSub.Pack_Type;
                        //    model.HSN_Code = pSub.HSN_Code;
                        //    model.Total_GST = pSub.Total_GST;
                        //    model.CGST = pSub.CGST;
                        //    model.SGST = pSub.SGST;
                        //    model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        //    model.Dispatch_Pack_Type = pSub.Dispatch_Pack_Type;
                        //    model.SKU_SubType = pSub.SKU_SubType;
                        //    model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                        //    model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                        //    model.Pack_Size = pSub.Pack_Size;
                        //    model.UOM = pSub.UOM;
                        //    model.Grade = pSub.Grade;
                        //    model.Status = "Open";
                        //    model.Indent_Qty = pSub.Indent_Qty;
                        //    model.Dispatch_Qty = pSub.Dispatch_Qty;
                        //    model.Strinkage_Qty = pSub.Strinkage_Qty;
                        //    model.Received_Qty = pSub.Received_Qty;
                        //    model.Accepted_Qty = pSub.Accepted_Qty;
                        //    model.Dispatch_Value = pSub.Dispatch_Value;
                        //    model.Unit_Rate = pSub.Unit_Rate;
                        //    model.Dispatch_Value = pSub.Dispatch_Value;
                        //    model.No_of_Packed_Item = pSub.No_of_Packed_Item;
                        //    model.is_Stk_Update = false;
                        //    model.is_Deleted = false;
                        //    model.Remark = pSub.Remark;
                        //    model.CreatedDate = DateTime.Now;
                        //    model.CreateBy = pSub.CreateBy;
                        //    model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                        //    _unitOfWork.DispatchSubRepository.Insert(model);
                        //    _unitOfWork.Save();

                        //    int grnLineId = model.Dispatch_Line_Id;

                        //    int dispatchLineId = model.Dispatch_Line_Id;
                        //    var skuLSub = new Dispatch_SKU_Line_Items();
                        //    foreach (var skuSub in pSub.DispatchSKULineItems.Take(1))
                        //    {
                        //        skuLSub.Dispatch_Line_Id = dispatchLineId;
                        //        skuLSub.SKU_Id = skuSub.SKU_Id;
                        //        skuLSub.Dispatch_Line_Id = dispatchLineId;
                        //        skuLSub.Dispatch_Qty = skuSub.Dispatch_Qty;
                        //        skuLSub.Barcode = skuSub.Barcode;
                        //        skuLSub.Batch_Number = skuSub.Batch_Number;
                        //        skuLSub.UOM = skuSub.UOM;
                        //        skuLSub.CreatedDate = DateTime.Now;
                        //        skuLSub.CreatedBy = skuSub.CreatedBy;
                        //        skuLSub.is_Deleted = false;

                        //        _unitOfWork.DispatchSKULineItemRepository.Insert(skuLSub);
                        //        _unitOfWork.Save();

                        //    }

                        //    int Dispatch_SKU_Line_Items_Id_Ref = skuLSub.Dispatch_SKU_Line_Items_Id;

                        //    foreach (var aSub in pSub.DispatchLineItemConsumables.Take(1))
                        //    {

                        //        var consumable = new Dispatch_Consumables();

                        //        consumable.Dispatch_Line_Id_FK = model.Dispatch_Line_Id;
                        //        consumable.Dispatch_SKU_Line_Items_Id_FK = Dispatch_SKU_Line_Items_Id_Ref;
                        //        consumable.SKU_Id = aSub.SKU_Id;
                        //        consumable.Consumable_Qty = aSub.Consumable_Qty;
                        //        consumable.Grade = aSub.Grade;
                        //        consumable.UOM = aSub.UOM;
                        //        consumable.CreatedDate = DateTime.UtcNow;
                        //        consumable.CreatedBy = dispatchEntity.CreateBy;
                        //        consumable.is_Deleted = false;
                        //        _unitOfWork.DispatchConsumablesRepository.Insert(consumable);
                        //        _unitOfWork.Save();

                        //    }

                        //    int DLineId = model.Dispatch_Line_Id;

                        //    List<Stock> line = DB.Stocks.Where(s => s.SKU_Name == pSub.SKU_Name && s.SKU_Type == dispatchEntity.SKU_Type && s.DC_Code == dispatchEntity.Dispatch_Location_Code && s.Grade == pSub.Grade).Select(a => a).ToList();
                        //    foreach (Stock m in line)
                        //    {
                        //        if (line != null)
                        //        {
                        //            double? cQTY = m.Closing_Qty;

                        //            var dispatchSupplier = new Dispatch_Supplier_Track
                        //            {
                        //                Dispatch_Id = Dispatch_Id,
                        //                Dispatch_Number = DispatchNumber,
                        //                Dispatch_Line_Id = DLineId,
                        //                Stock_ID = m.Stock_Id,
                        //                Stock_code = m.Stock_code,
                        //                SKU_Code = m.SKU_Code,
                        //                SKU_name = m.SKU_Name,
                        //                Grade = m.Grade,
                        //                SKU_SubType_Id = pSub.SKU_SubType_Id,
                        //                SKU_SubType = pSub.SKU_SubType,
                        //                Pack_Type_Id = pSub.Pack_Type_Id,
                        //                Pack_Type = pSub.Pack_Type,
                        //                Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id,
                        //                Pack_Weight_Type = pSub.Pack_Weight_Type,
                        //                Pack_Size = pSub.Pack_Size,
                        //                Dispatch_pack_Type = pSub.Dispatch_Pack_Type,
                        //                QTY = cQTY,
                        //                Billed_Qty = cQTY,
                        //                Price = pSub.Unit_Rate,
                        //                CreatedDate = DateTime.Now,
                        //            };

                        //            _unitOfWork.DispatchSupplierTrackRepository.Insert(dispatchSupplier);
                        //            _unitOfWork.Save();
                        //        }
                        //        else
                        //        {
                        //            break;
                        //        }
                          //  }
                        //}


                  //  }



                    //DB.dispatch_proc();
                    //DB.SaveChanges();





        public DispatchResponseEntity DispatchCreation(DispatchEntity dispatchEntity)
        {
            DispatchResponseEntity response = new DispatchResponseEntity();
            string custdipatchNumber = "";
            string stockxrefdipatchNumber = "";
            string locationID, CD_Prefix, SD_Prefix, locationId, cdispatchNumber, sdispatchNumber,FY;
            int? incNumber, incrementedValue;

            using (var iscope = new TransactionScope())
            {
                if (dispatchEntity.Dispatch_Type == "Customer")
                {
                    locationID = dispatchEntity.Dispatch_Location_Code;
                    ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                    CD_Prefix = rm.GetString("CDT");
                    Customer_Dispatch_Num_Gen autoIncNumber = GetAutoIncrement(locationID);
                    locationId = autoIncNumber.DC_Code;
                    FY=autoIncNumber.Financial_Year;
                    incNumber = autoIncNumber.Customer_Dispatch_Last_Number;
                    incrementedValue = incNumber + 1;
                    var dispatchincrement = DB.Customer_Dispatch_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                    dispatchincrement.Customer_Dispatch_Last_Number = incrementedValue;
                    _unitOfWork.CustomerDispatchAutoIncrementRepository.Update(dispatchincrement);
                    _unitOfWork.Save();
                    cdispatchNumber = CD_Prefix + "/" + locationId + "/"+FY+ "/" + String.Format("{0:00000}", incNumber);
                    custdipatchNumber = cdispatchNumber;
                }
                else if (dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                {
                    locationID = dispatchEntity.Dispatch_Location_Code;
                    ResourceManager ra = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                    SD_Prefix = ra.GetString("SDT");
                    Stock_Xfer_Dispatch_Num_Gen sautoIncNumber = StockXferAutoIncrement(locationID);
                    locationId = sautoIncNumber.DC_Code;
                    incNumber = sautoIncNumber.Stock_Xfer_Dispatch_Last_Number;
                    incrementedValue = incNumber + 1;
                    var sdispatchincrement = DB.Stock_Xfer_Dispatch_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
                    sdispatchincrement.Stock_Xfer_Dispatch_Last_Number = incrementedValue;
                    _unitOfWork.StockXferDispatchAutoIncrementRepository.Update(sdispatchincrement);
                    _unitOfWork.Save();
                    sdispatchNumber = SD_Prefix + "/" + locationId + "/" + String.Format("{0:00000}", incNumber);
                    stockxrefdipatchNumber = sdispatchNumber;
                }
                iscope.Complete();
            }
            using (var scope = new TransactionScope())
            {
                var dispatchItems = new Dispatch_Creation
                {
                    Dispatched_Location_ID = dispatchEntity.Dispatched_Location_ID,
                    Dispatch_Location_Code = dispatchEntity.Dispatch_Location_Code,
                    Customer_Dispatch_Number = custdipatchNumber,
                    Stock_Xfer_Dispatch_Number = stockxrefdipatchNumber,
                    Dispatch_Type = dispatchEntity.Dispatch_Type,
                    Delivery_Date = dispatchEntity.Delivery_Date,
                    Indent_Rls_Date = dispatchEntity.Indent_Rls_Date,
                    Delievery_Type = dispatchEntity.Delievery_Type,
                    SKU_Type_Id = dispatchEntity.SKU_Type_Id,
                    Order_Reference=dispatchEntity.Order_Reference,
                    SKU_Type = dispatchEntity.SKU_Type,
                    Delivery_done_by = dispatchEntity.Delivery_done_by,
                    Vehicle_No = dispatchEntity.Vehicle_No,
                    Sales_Person_Id = dispatchEntity.Sales_Person_Id,
                    Sales_Person_Name = dispatchEntity.Sales_Person_Name,
                    Route_Id = dispatchEntity.Route_Id,
                    Route_Code = dispatchEntity.Route_Code,
                    Route = dispatchEntity.Route,
                    Customer_Id = dispatchEntity.Customer_Id,
                    Customer_code = dispatchEntity.Customer_code,
                    Customer_Name = dispatchEntity.Customer_Name,
                    Delivery_Location_ID = dispatchEntity.Delivery_Location_ID,
                    Delivery_Location_Code = dispatchEntity.Delivery_Location_Code,
                    Dispatch_time_stamp = dispatchEntity.Dispatch_time_stamp,
                    Expected_Delivery_date = dispatchEntity.Expected_Delivery_date,
                    Expected_Delivery_time = dispatchEntity.Expected_Delivery_time,
                    Price_Template_ID = dispatchEntity.Price_Template_ID,
                    Price_Template_Code = dispatchEntity.Price_Template_Code,
                    Price_Template_Name = dispatchEntity.Price_Template_Name,
                    Price_Template_Valitity_upto = dispatchEntity.Price_Template_Valitity_upto,
                    is_Deleted = false,
                    Status = "Open",
                    CSI_Number = dispatchEntity.CSI_Number,
                    CSI_Id = dispatchEntity.CSI_Id,
                    STI_Id = dispatchEntity.STI_Id,
                    STI_Number = dispatchEntity.STI_Number,
                    Remark = dispatchEntity.Remark,
                    Invoice_Flag = null,
                    is_Syunc = false,
                    CreatedDate = DateTime.Now,
                    CreateBy = dispatchEntity.CreateBy
                };

                _unitOfWork.DispatchRepository.Insert(dispatchItems);
                _unitOfWork.Save();
                DateTime Today = DateTime.Now;
                var check = (from ee in DB.Stirnkage_Summary
                             where ee.DC_Code == dispatchEntity.Dispatch_Location_Code
                             && ee.CreatedDate.Value.Year == Today.Year
                              && ee.CreatedDate.Value.Month == Today.Month
                               && ee.CreatedDate.Value.Day == Today.Day
                               && ee.Adjustment_Freeze == false
                             select ee).FirstOrDefault();
                if (check != null)
                {
                    check.Adjustment_Freeze = true;
                    DB.Entry(check).State = EntityState.Modified;
                    DB.SaveChanges();
                }
                int DispatchId = dispatchItems.Dispatch_Id;
                string DispatchNumber = "";
                if (dispatchEntity.Dispatch_Type == "Customer")
                {
                    DispatchNumber = dispatchItems.Customer_Dispatch_Number;
                }
                else if (dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                {
                    DispatchNumber = dispatchItems.Stock_Xfer_Dispatch_Number;
                }

                string dispatchType = dispatchItems.Dispatch_Type;

                var model = new Dispatch_Line_item();

                foreach (var pSub in dispatchEntity.Line_Items)
                {

                    //------------------------------------ latest calculation for stock and grn -----------------------
                    double? stock_Qty = 0;

                    var stockQty = DB.Stocks.Where(s => s.SKU_Name == pSub.SKU_Name && s.DC_Code == dispatchEntity.Dispatch_Location_Code && s.Grade == pSub.Grade && s.SKU_Type == dispatchEntity.SKU_Type).Select(a => a.Closing_Qty).Sum();

                    if (stockQty == null)
                    {
                        stockQty = 0;
                    }

                    stock_Qty = stockQty;

                    double? ActualValue = 0.0;

                    if (pSub.UOM == "Kgs" && dispatchEntity.Dispatch_Type == "Customer")
                    {
                        ActualValue = pSub.Dispatch_Qty;
                    }
                    else if (pSub.UOM == "Unit" && dispatchEntity.Dispatch_Type == "Customer")
                    {
                        ActualValue = pSub.Converted_Unit_Value;
                    }
                    else if (pSub.UOM == "Kgs" && dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                    {
                        ActualValue = pSub.Dispatch_Qty;
                    }
                    else if (pSub.UOM == "Unit" && dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                    {
                        ActualValue = pSub.Dispatch_Qty;
                    }

                    if (ActualValue == 0)
                    {
                        model.Dispatch_Id = DispatchId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.Pack_Type_Id = pSub.Pack_Type_Id;
                        model.Pack_Type = pSub.Pack_Type;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.Dispatch_Pack_Type = pSub.Dispatch_Pack_Type;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                        model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                        model.Pack_Size = pSub.Pack_Size;
                        model.UOM = pSub.UOM;
                        model.HSN_Code = pSub.HSN_Code;
                        model.Total_GST = pSub.Total_GST;
                        model.CGST = pSub.CGST;
                        model.SGST = pSub.SGST;
                        model.Grade = pSub.Grade;
                        model.Status = "Open";
                        model.Strinkage_Qty = pSub.Strinkage_Qty;
                        model.Indent_Qty = pSub.Indent_Qty;
                        model.Dispatch_Qty = pSub.Dispatch_Qty;
                        model.Dispatch_Value = pSub.Dispatch_Value;
                        model.Received_Qty = pSub.Received_Qty;
                        model.Accepted_Qty = pSub.Accepted_Qty;
                        model.Unit_Rate = pSub.Unit_Rate;
                        model.Dispatch_Value = pSub.Dispatch_Value;
                        model.No_of_Packed_Item = pSub.No_of_Packed_Item;
                        model.is_Stk_Update = false;
                        model.is_Deleted = false;
                        model.Remark = pSub.Remark;
                        model.CreatedDate = DateTime.Now;
                        model.CreateBy = pSub.CreateBy;
                        model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                        _unitOfWork.DispatchSubRepository.Insert(model);
                        _unitOfWork.Save();
                        int dispatchLineId = model.Dispatch_Line_Id;
                        var skuLSub = new Dispatch_SKU_Line_Items();
                        foreach (var skuSub in pSub.DispatchSKULineItems)
                        {
                            skuLSub.Dispatch_Line_Id = dispatchLineId;
                            skuLSub.SKU_Id = skuSub.SKU_Id;
                            skuLSub.Dispatch_Line_Id = dispatchLineId;
                            skuLSub.Dispatch_Qty = skuSub.Dispatch_Qty;
                            skuLSub.Barcode = skuSub.Barcode;
                            skuLSub.Batch_Number = skuSub.Batch_Number;
                            skuLSub.UOM = skuSub.UOM;
                            skuLSub.CreatedDate = DateTime.Now;
                            skuLSub.CreatedBy = skuSub.CreatedBy;
                            skuLSub.is_Deleted = false;

                            _unitOfWork.DispatchSKULineItemRepository.Insert(skuLSub);
                            _unitOfWork.Save();
                        }

                        int Dispatch_SKU_Line_Items_Id_Ref = skuLSub.Dispatch_SKU_Line_Items_Id;

                        foreach (var aSub in pSub.DispatchLineItemConsumables)
                        {
                            var consumable = new Dispatch_Consumables();
                            consumable.Dispatch_Line_Id_FK = model.Dispatch_Line_Id;
                            consumable.Dispatch_SKU_Line_Items_Id_FK = Dispatch_SKU_Line_Items_Id_Ref;
                            consumable.SKU_Id = model.SKU_ID;
                            consumable.Consumable_Qty = aSub.Consumable_Qty;
                            consumable.Grade = model.Grade;
                            consumable.UOM = model.UOM;
                            consumable.CreatedDate = DateTime.UtcNow;
                            consumable.CreatedBy = dispatchEntity.CreateBy;
                            consumable.is_Deleted = false;
                            _unitOfWork.DispatchConsumablesRepository.Insert(consumable);
                            _unitOfWork.Save();
                        }
                    }
                    else if (stock_Qty >= ActualValue)
                    {
                        model.Dispatch_Id = DispatchId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.Pack_Type_Id = pSub.Pack_Type_Id;
                        model.Pack_Type = pSub.Pack_Type;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.Dispatch_Pack_Type = pSub.Dispatch_Pack_Type;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.HSN_Code = pSub.HSN_Code;
                        model.Total_GST = pSub.Total_GST;
                        model.CGST = pSub.CGST;
                        model.SGST = pSub.SGST;
                        model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                        model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                        model.Pack_Size = pSub.Pack_Size;
                        model.UOM = pSub.UOM;
                        model.Grade = pSub.Grade;
                        model.Status = "Open";
                        model.Indent_Qty = pSub.Indent_Qty;
                        model.Dispatch_Qty = pSub.Dispatch_Qty;
                        model.Received_Qty = pSub.Received_Qty;
                        model.Accepted_Qty = pSub.Accepted_Qty;
                        model.Strinkage_Qty = pSub.Strinkage_Qty;
                        model.Dispatch_Value = pSub.Dispatch_Value;
                        model.Unit_Rate = pSub.Unit_Rate;
                        model.Dispatch_Value = pSub.Dispatch_Value;
                        model.No_of_Packed_Item = pSub.No_of_Packed_Item;
                        model.is_Stk_Update = false;
                        model.is_Deleted = false;
                        model.Remark = pSub.Remark;
                        model.CreatedDate = DateTime.Now;
                        model.CreateBy = pSub.CreateBy;
                        model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                        _unitOfWork.DispatchSubRepository.Insert(model);
                        _unitOfWork.Save();

                        int dispatchLineId = model.Dispatch_Line_Id;
                        var skuLSub = new Dispatch_SKU_Line_Items();
                        foreach (var skuSub in pSub.DispatchSKULineItems)
                        {
                            skuLSub.Dispatch_Line_Id = dispatchLineId;
                            skuLSub.SKU_Id = skuSub.SKU_Id;
                            skuLSub.Dispatch_Line_Id = dispatchLineId;
                            skuLSub.Dispatch_Qty = skuSub.Dispatch_Qty;
                            skuLSub.Barcode = skuSub.Barcode;
                            skuLSub.Batch_Number = skuSub.Batch_Number;
                            skuLSub.UOM = skuSub.UOM;
                            skuLSub.CreatedDate = DateTime.Now;
                            skuLSub.CreatedBy = skuSub.CreatedBy;
                            skuLSub.is_Deleted = false;

                            _unitOfWork.DispatchSKULineItemRepository.Insert(skuLSub);
                            _unitOfWork.Save();



                        }

                        int Dispatch_SKU_Line_Items_Id_Ref = skuLSub.Dispatch_SKU_Line_Items_Id;

                        foreach (var aSub in pSub.DispatchLineItemConsumables)
                        {
                            var consumable = new Dispatch_Consumables();

                            consumable.Dispatch_Line_Id_FK = model.Dispatch_Line_Id;
                            consumable.Dispatch_SKU_Line_Items_Id_FK = Dispatch_SKU_Line_Items_Id_Ref;
                            consumable.SKU_Id = model.SKU_ID;
                            consumable.Consumable_Qty = aSub.Consumable_Qty;
                            consumable.Grade = model.Grade;
                            consumable.UOM = model.UOM;
                            consumable.CreatedDate = DateTime.UtcNow;
                            consumable.CreatedBy = dispatchEntity.CreateBy;
                            consumable.is_Deleted = false;
                            _unitOfWork.DispatchConsumablesRepository.Insert(consumable);
                            _unitOfWork.Save();

                        }

                        int DLineId = model.Dispatch_Line_Id;

                        List<Stock> line = DB.Stocks.Where(s => s.SKU_Name == pSub.SKU_Name && s.SKU_Type == dispatchEntity.SKU_Type && s.DC_Code == dispatchEntity.Dispatch_Location_Code && s.Grade == pSub.Grade).Select(a => a).ToList();
                        foreach (Stock m in line)
                        {
                            if (line != null)
                            {
                                double? cQTY = m.Closing_Qty;
                                var dispatchSupplier = new Dispatch_Supplier_Track
                                {
                                    Dispatch_Id = DispatchId,
                                    Dispatch_Number = DispatchNumber,
                                    Dispatch_Line_Id = DLineId,
                                    Stock_ID = m.Stock_Id,
                                    Stock_code = m.Stock_code,
                                    SKU_Code = m.SKU_Code,
                                    SKU_name = m.SKU_Name,
                                    Grade = m.Grade,
                                    SKU_SubType_Id = pSub.SKU_SubType_Id,
                                    SKU_SubType = pSub.SKU_SubType,
                                    Pack_Type_Id = pSub.Pack_Type_Id,
                                    Pack_Type = pSub.Pack_Type,
                                    Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id,
                                    Pack_Weight_Type = pSub.Pack_Weight_Type,
                                    Pack_Size = pSub.Pack_Size,
                                    Dispatch_pack_Type = pSub.Dispatch_Pack_Type,
                                    QTY = cQTY,
                                    Billed_Qty = cQTY,
                                    Price = pSub.Unit_Rate,
                                    CreatedDate = DateTime.Now,
                                };

                                _unitOfWork.DispatchSupplierTrackRepository.Insert(dispatchSupplier);
                                _unitOfWork.Save();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                DB.dispatch_proc();
                DB.SaveChanges();

                scope.Complete();

                response.DispatchID = dispatchItems.Dispatch_Id;
                if (dispatchEntity.Dispatch_Type == "Customer")
                {
                    response.DispatchNumber = dispatchItems.Customer_Dispatch_Number;
                }
                else if (dispatchEntity.Dispatch_Type == "DC Stock Transfer")
                {
                    response.DispatchNumber = dispatchItems.Stock_Xfer_Dispatch_Number;
                }

                response.Status = HttpStatusCode.Created;
                response.Message = "Dispatched Creation Successfully";

                return response;

            }

            response.Status = HttpStatusCode.NotImplemented;
            response.Message = "Creation Failed";
            return response;
        }
        //
        //-------------------------------------------------------------------
        public InvoiceResponseEntity CreateInvoice(InvoiceEntity invoiceEntity)
        {
            InvoiceResponseEntity response = new InvoiceResponseEntity();
            if (invoiceEntity != null)
            {
                string InvoiceNumber, locationId, INV_Prefix;
                int? incNumber;

                using (var iscope = new TransactionScope())
                {
                    string locationID = invoiceEntity.DC_LCode;
                    ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                    INV_Prefix = rm.GetString("INVT");
                    Invoice_NUM_Generation autoIncNumber = InvoiceAutoIncrement(locationID);
                    locationId = autoIncNumber.DC_Code;
                    incNumber = autoIncNumber.Invoice_Last_Number;
                    string year = autoIncNumber.Financial_Year;
                    int? incrementedValue = incNumber + 1;
                    var invincrement = DB.Invoice_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();
                    invincrement.Invoice_Last_Number = incrementedValue;
                    _unitOfWork.InvoiceAutoIncrementRepository.Update(invincrement);
                    _unitOfWork.Save();

                    InvoiceNumber = INV_Prefix + "/" + locationId + "/" + year + "/" + String.Format("{0:00000}", incNumber);

                    iscope.Complete();
                }
                using (var scope = new TransactionScope())
                {
                    var invoiceGen = new Invoice_Creation
                    {
                        Invoice_Number = InvoiceNumber,
                        Dispatch_id = invoiceEntity.Dispatch_id,
                        Customer_Dispatch_Number = invoiceEntity.Customer_Dispatch_Number,
                        Invoice_Date = invoiceEntity.Invoice_Date,
                        Invoice_Type = invoiceEntity.Invoice_Type,
                        Customer_Id = invoiceEntity.Customer_Id,
                        Customer_code = invoiceEntity.Customer_code,
                        Customer_Name = invoiceEntity.Customer_Name,
                        DC_ID = invoiceEntity.DC_LID,
                        DC_Code = invoiceEntity.DC_LCode,
                        Term_of_Payment = invoiceEntity.Term_of_Payment,
                        Supplier_Ref = invoiceEntity.Supplier_Ref,
                        Buyer_Order_No = invoiceEntity.Buyer_Order_No,
                        Order_Date = invoiceEntity.Order_Date,
                        Order_Reference=invoiceEntity.Order_Reference,
                        is_Deleted = false,
                        is_Syunc = false,
                        SKU_Type_Id = invoiceEntity.SKU_Type_Id,
                        SKU_Type = invoiceEntity.SKU_Type,
                        Remark = invoiceEntity.Remark,
                        is_Invoice_Approved = null,
                        is_Invoice_approved_user_id = invoiceEntity.is_Invoice_approved_user_id,
                        is_Invoice_approved_by = invoiceEntity.is_Invoice_approved_by,
                        is_Invoice_approved_date = invoiceEntity.is_Invoice_approved_date,
                        CreatedDate = DateTime.Now,
                        CreateBy = invoiceEntity.CreateBy
                    };
                    _unitOfWork.InvoiceRepository.Insert(invoiceGen);
                    _unitOfWork.Save();

                    int? invId = invoiceGen.invoice_Id;
                    var model = new Invoice_Line_item();
                    foreach (var pSub in invoiceEntity.InvoiceLineItems)
                    {
                        model.Invoice_Number = InvoiceNumber;
                        model.Invoice_Id = invId;
                        model.Dispatch_Line_id = pSub.Dispatch_Line_id;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.Pack_Type_Id = pSub.Pack_Type_Id;
                        model.Pack_Type = pSub.Pack_Type;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.Pack_Size = pSub.Pack_Size;
                        model.HSN_Code = pSub.HSN_Code;
                        model.Total_GST = pSub.Total_GST;
                        model.CGST = pSub.CGST;
                        model.SGST = pSub.SGST;
                        model.Grade = pSub.Grade;
                        model.UOM = pSub.UOM;
                        model.Invoice_Qty = pSub.Invoice_Qty;
                        model.Rate = pSub.Rate;
                        model.Discount = pSub.Discount;
                        model.Invoice_Amount = pSub.Invoice_Qty * pSub.Rate;
                        model.is_Deleted = false;
                        model.CreatedDate = DateTime.Now;
                        model.CreateBy = pSub.CreateBy;
                        model.Dispatch_Qty = pSub.Dispatch_Qty;
                        model.Converted_Unit_Value = pSub.Converted_Unit_Value;
                        _unitOfWork.InvoiceSubRepository.Insert(model);
                        _unitOfWork.Save();
                    }
                    int? DisId = invoiceGen.Dispatch_id;
                    Dispatch_Creation query = DB.Dispatch_Creation.Where(x => x.Dispatch_Id == DisId).FirstOrDefault();
                    if (query != null)
                    {
                        query.Invoice_Flag = true;
                        DB.Entry(query).State = EntityState.Modified;
                        DB.SaveChanges();
                    }
                    else
                    {
                        return null;
                    }
                    scope.Complete();
                    response.InvoiceID = invoiceGen.invoice_Id;
                    response.InvoiceNumber = invoiceGen.Invoice_Number;
                }

                response.Status = HttpStatusCode.OK;
                response.Message = " Creation Successfully";
            }
            else
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = "Inserted UnSuccessfully";
            }
            return response;
        }

        //-update invoice----------------
        public int UpdateInvoice(int invoiceId, InvoiceEntity invoiceEntity)
        {
            string invoiceNumber = "";
            int invId = 0;

            if (invoiceEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
                    if (p != null)
                    {
                        invoiceNumber = invoiceEntity.Invoice_Number;
                        invId = invoiceEntity.invoice_Id;
                        p.Dispatch_id = invoiceEntity.Dispatch_id;
                        p.Customer_Dispatch_Number = invoiceEntity.Customer_Dispatch_Number;
                        p.SKU_Type_Id = invoiceEntity.SKU_Type_Id;
                        p.SKU_Type = invoiceEntity.SKU_Type;
                        p.Customer_Id = invoiceEntity.Customer_Id;
                        p.Customer_code = invoiceEntity.Customer_code;
                        p.Order_Reference = invoiceEntity.Order_Reference;
                        p.Customer_Name = invoiceEntity.Customer_Name;
                        p.DC_ID = invoiceEntity.DC_LID;
                        p.DC_Code = invoiceEntity.DC_LCode;
                        p.Term_of_Payment = invoiceEntity.Term_of_Payment;
                        p.Supplier_Ref = invoiceEntity.Supplier_Ref;
                        p.Buyer_Order_No = invoiceEntity.Buyer_Order_No;
                        p.Order_Date = invoiceEntity.Order_Date;
                        p.Remark = invoiceEntity.Remark;
                        p.UpdateBy = invoiceEntity.UpdateBy;
                        p.UpdateDate = DateTime.Now;
                        _unitOfWork.InvoiceRepository.Update(p);
                        _unitOfWork.Save();
                    }

                    var lineItemList = DB.Invoice_Line_item.Where(x => x.Invoice_Id == invoiceId).ToList();
                    foreach (var li in lineItemList)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var list = _unitOfWork.InvoiceSubRepository.GetByID(li.Invoice_Line_Id);

                            if (list != null)
                            {
                                _unitOfWork.InvoiceSubRepository.Delete(list);
                                _unitOfWork.Save();
                            }

                            scope1.Complete();
                        }
                    }

                    foreach (InvoiceLineItemEntity pSub in invoiceEntity.InvoiceLineItems)
                    {

                        var model = new Invoice_Line_item();


                        model.Invoice_Number = invoiceNumber;
                        model.Invoice_Id = invoiceId;
                        model.SKU_ID = pSub.SKU_ID;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.Pack_Type_Id = pSub.Pack_Type_Id;
                        model.Pack_Type = pSub.Pack_Type;
                        model.Pack_Size = pSub.Pack_Size;
                        model.HSN_Code = pSub.HSN_Code;
                        model.Total_GST = pSub.Total_GST;
                        model.CGST = pSub.CGST;
                        model.SGST = pSub.SGST;
                        model.UOM = pSub.UOM;
                        model.Invoice_Qty = pSub.Invoice_Qty;
                        model.Return_Qty = pSub.Return_Qty;
                        model.Grade = pSub.Grade;
                        model.Rate = pSub.Rate;
                        model.Discount = pSub.Discount;
                        model.Invoice_Amount = pSub.Invoice_Amount;
                        model.CreatedDate = DateTime.Now;
                        model.CreateBy = pSub.CreateBy;
                        model.Dispatch_Qty = pSub.Dispatch_Qty;
                        model.Converted_Unit_Value = pSub.Converted_Unit_Value;

                        _unitOfWork.InvoiceSubRepository.Insert(model);
                        _unitOfWork.Save();

                    }
                    scope.Complete();
                }
            }
            return invId;
        }
        //-end---------------------------

        public Customer_Dispatch_Num_Gen GetAutoIncrement(string locationId)
        {
            var autoinc = DB.Customer_Dispatch_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();
            var model = new Customer_Dispatch_Num_Gen
            {
                Dispatch_Num_Gen_Id = autoinc.Dispatch_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Customer_Dispatch_Last_Number = autoinc.Customer_Dispatch_Last_Number
            };
            return model;
        }
        public Stock_Xfer_Dispatch_Num_Gen StockXferAutoIncrement(string locationId)
        {
            var autoinc = DB.Stock_Xfer_Dispatch_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();

            var model = new Stock_Xfer_Dispatch_Num_Gen
            {
                Dispatch_Num_Gen_Id = autoinc.Dispatch_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Stock_Xfer_Dispatch_Last_Number = autoinc.Stock_Xfer_Dispatch_Last_Number
            };

            return model;
        }
        //------Invoice---------
        public IEnumerable<InvoiceEntity> GetUnapprovalInvoiceList(string Ulocation)
        {
            var invoice = DB.Invoice_Creation.Where(x => x.is_Invoice_Approved == false && x.DC_Code == Ulocation).ToList();
            if (invoice.Any())
            {
                Mapper.CreateMap<Invoice_Creation, InvoiceEntity>();
                var invoiceModel = Mapper.Map<List<Invoice_Creation>, List<InvoiceEntity>>(invoice);
                return invoiceModel;
            }
            return null;
        }

        //----------------invoice -----------------------

        public bool ApproveInvoice(approvalInvoiceList approval)
        {
            var success = false;
            if (approval.invoice_Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var invoice = _unitOfWork.InvoiceRepository.GetByID(approval.invoice_Id);
                    if (invoice != null)
                    {
                        invoice.is_Invoice_Approved = approval.is_Invoice_Approved;
                        invoice.is_Invoice_approved_by = approval.is_Invoice_approved_by;
                        invoice.is_Invoice_approved_date = DateTime.Now;
                        invoice.is_Invoice_approved_user_id = approval.is_Invoice_approved_user_id;

                        _unitOfWork.InvoiceRepository.Update(invoice);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public List<DispatchEntity> DispatchSTN(string stnNumber)
        {
            List<DispatchEntity> list = new List<DispatchEntity>();
            var amt = (from s in DB.Dispatch_Creation
                       join r in DB.Dispatch_Line_item on s.Dispatch_Id equals r.Dispatch_Id
                       where s.Stock_Xfer_Dispatch_Number == stnNumber
                       select r.Dispatch_Value).Sum();

            string amntInWords = AmountInWords(double.Parse(amt.ToString()));

            list = (from x in DB.Dispatch_Creation
                    where x.Stock_Xfer_Dispatch_Number == stnNumber
                    select new DispatchEntity
                    {
                        Dispatch_Id = x.Dispatch_Id,
                        Dispatched_Location_ID = x.Dispatched_Location_ID,
                        Dispatch_Location_Code = x.Dispatch_Location_Code,
                        Dispatch_Type = x.Dispatch_Type,
                        Indent_Rls_Date = x.Indent_Rls_Date,
                        Delievery_Type = x.Delievery_Type,
                        Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                        Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                        STI_Id = x.STI_Id,
                        Order_Reference = x.Order_Reference,
                        STI_Number = x.STI_Number,
                        CSI_Id = x.CSI_Id,
                        CSI_Number = x.CSI_Number,
                        Vehicle_No = x.Vehicle_No,
                        Sales_Person_Id = x.Sales_Person_Id,
                        Sales_Person_Name = x.Sales_Person_Name,
                        Route_Id = x.Route_Id,
                        Route_Code = x.Route_Code,
                        Route = x.Route,
                        Customer_Id = x.Customer_Id,
                        Customer_code = x.Customer_code,
                        Customer_Name = x.Customer_Name,
                        Delivery_Date = x.Delivery_Date,
                        SKU_Type_Id = x.SKU_Type_Id,
                        SKU_Type = x.SKU_Type,
                        Dispatch_time_stamp = x.Dispatch_time_stamp,
                        Delivery_done_by = x.Delivery_done_by,
                        Delivery_Location_ID = x.Delivery_Location_ID,
                        Delivery_Location_Code = x.Delivery_Location_Code,
                        Expected_Delivery_date = x.Expected_Delivery_date,
                        Expected_Delivery_time = x.Expected_Delivery_time,
                        Remark = x.Remark,
                        Status = x.Status,
                        CreateBy = x.CreateBy,
                        CreatedDate = x.CreatedDate,
                        Total_Amount = amt,
                        AMNTinWord = amntInWords,
                        Line_Items = (from y in DB.Dispatch_Line_item
                                      where y.Dispatch_Id == x.Dispatch_Id
                                      orderby y.SKU_Name
                                      select new DispatchLineItemsEntity
                                      {
                                          Dispatch_Line_Id = y.Dispatch_Line_Id,
                                          Dispatch_Id = y.Dispatch_Id,
                                          SKU_ID = y.SKU_ID,
                                          SKU_Code = y.SKU_Code,
                                          SKU_Name = y.SKU_Name,
                                          Pack_Type_Id = y.Pack_Type_Id,
                                          Pack_Type = y.Pack_Type,
                                          Strinkage_Qty = y.Strinkage_Qty,
                                          HSN_Code = y.HSN_Code,
                                          Total_GST = y.Total_GST,
                                          CGST = y.CGST,
                                          SGST = y.SGST,
                                          Dispatch_Pack_Type = y.Dispatch_Pack_Type,
                                          SKU_SubType_Id = y.SKU_SubType_Id,
                                          SKU_SubType = y.SKU_SubType,
                                          Pack_Weight_Type_Id = y.Pack_Weight_Type_Id,
                                          Pack_Weight_Type = y.Pack_Weight_Type,
                                          Pack_Size = y.Pack_Size,
                                          UOM = y.UOM,
                                          Grade = y.Grade,
                                          Status = y.Status,
                                          Indent_Qty = y.Indent_Qty,
                                          Dispatch_Qty = y.Dispatch_Qty,
                                          Received_Qty = y.Received_Qty,
                                          Accepted_Qty = y.Accepted_Qty,
                                          Return_Qty = y.Return_Qty,
                                          Unit_Rate = y.Unit_Rate,
                                          Dispatch_Value = y.Dispatch_Value,
                                          No_of_Packed_Item = y.No_of_Packed_Item,
                                          Converted_Unit_Value = y.Converted_Unit_Value,
                                          is_Deleted = y.is_Deleted,
                                          Remark = y.Remark,
                                          CreateBy = y.CreateBy
                                      }).ToList()
                    }).ToList();

            return list;
        }

        public List<DispatchEntity> GetCustomerDispatchList(string cdnNumber)
        {
            List<DispatchEntity> list = new List<DispatchEntity>();
            var amt = (from s in DB.Dispatch_Creation
                       join r in DB.Dispatch_Line_item on s.Dispatch_Id equals r.Dispatch_Id
                       where s.Customer_Dispatch_Number == cdnNumber
                       select r.Dispatch_Value).Sum();


            string amntInWords = AmountInWords(double.Parse(amt.ToString()));

            list = (from x in DB.Dispatch_Creation
                    where x.Customer_Dispatch_Number == cdnNumber
                    select new DispatchEntity
                    {
                        Dispatch_Id = x.Dispatch_Id,
                        Dispatched_Location_ID = x.Dispatched_Location_ID,
                        Dispatch_Location_Code = x.Dispatch_Location_Code,
                        Dispatch_Type = x.Dispatch_Type,
                        Order_Reference = x.Order_Reference,
                        Indent_Rls_Date = x.Indent_Rls_Date,
                        Delievery_Type = x.Delievery_Type,
                        Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                        Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                        Customer_Id = x.Customer_Id,
                        Customer_code = x.Customer_code,
                        Customer_Name = x.Customer_Name,
                        Delivery_Date = x.Delivery_Date,
                        STI_Id = x.STI_Id,
                        Vehicle_No = x.Vehicle_No,
                        Sales_Person_Id = x.Sales_Person_Id,
                        Sales_Person_Name = x.Sales_Person_Name,
                        Route_Id = x.Route_Id,
                        Route_Code = x.Route_Code,
                        Route = x.Route,
                        STI_Number = x.STI_Number,
                        CSI_Id = x.CSI_Id,
                        CSI_Number = x.CSI_Number,
                        SKU_Type_Id = x.SKU_Type_Id,
                        SKU_Type = x.SKU_Type,
                        Dispatch_time_stamp = x.Dispatch_time_stamp,
                        Delivery_done_by = x.Delivery_done_by,
                        Delivery_Location_ID = x.Delivery_Location_ID,
                        Delivery_Location_Code = x.Delivery_Location_Code,
                        Expected_Delivery_date = x.Expected_Delivery_date,
                        Expected_Delivery_time = x.Expected_Delivery_time,
                        Remark = x.Remark,
                        Status = x.Status,
                        Total_Amount = amt,
                        AMNTinWord = amntInWords,
                        CreateBy = x.CreateBy,
                        CreatedDate = x.CreatedDate,
                        CustomerAddress = (from m in DB.Customers
                                           where m.Cust_Id == x.Customer_Id
                                           select new CustomerEntity
                                           {
                                               Cust_Id = m.Cust_Id,
                                               Customer_Code = m.Customer_Code,
                                               Customer_Name = m.Customer_Name,
                                               Address1 = m.Address1,
                                               Address2 = m.Address2,
                                               City = m.City,
                                               State = m.State,
                                               District = m.District,
                                               Pincode = m.Pincode,
                                               Primary_Contact_Name = m.Primary_Contact_Name,
                                               Contact_Number = m.Contact_Number,
                                               Primary_Email_ID = m.Primary_Email_ID,
                                               Secondary_Email_ID = m.Secondary_Email_ID
                                           }
                                         ).ToList(),
                        Line_Items = (from y in DB.Dispatch_Line_item
                                      where y.Dispatch_Id == x.Dispatch_Id
                                      orderby y.SKU_Name
                                      select new DispatchLineItemsEntity
                                      {
                                          Dispatch_Line_Id = y.Dispatch_Line_Id,
                                          Dispatch_Id = y.Dispatch_Id,
                                          SKU_ID = y.SKU_ID,
                                          SKU_Code = y.SKU_Code,
                                          SKU_Name = y.SKU_Name,
                                          Pack_Type_Id = y.Pack_Type_Id,
                                          Dispatch_Pack_Type = y.Dispatch_Pack_Type,
                                          HSN_Code = y.HSN_Code,
                                          Total_GST = y.Total_GST,
                                          CGST = y.CGST,
                                          SGST = y.SGST,
                                          Pack_Type = y.Pack_Type,
                                          SKU_SubType_Id = y.SKU_SubType_Id,
                                          SKU_SubType = y.SKU_SubType,
                                          Pack_Weight_Type_Id = y.Pack_Weight_Type_Id,
                                        
                                          Pack_Weight_Type = y.Pack_Weight_Type,
                                          Pack_Size = y.Pack_Size,
                                          UOM = y.UOM,
                                          Strinkage_Qty = y.Strinkage_Qty,
                                          Status = y.Status,
                                          Grade = y.Grade,
                                          Indent_Qty = y.Indent_Qty,
                                          Dispatch_Qty = y.Dispatch_Qty,
                                          Received_Qty = y.Received_Qty,
                                          Accepted_Qty = y.Accepted_Qty,
                                          Return_Qty = y.Return_Qty,
                                          Unit_Rate = y.Unit_Rate,
                                          Dispatch_Value = y.Dispatch_Value,
                                          No_of_Packed_Item = y.No_of_Packed_Item,
                                          is_Deleted = y.is_Deleted,
                                          Converted_Unit_Value = y.Converted_Unit_Value,
                                          Remark = y.Remark,
                                          CreateBy = y.CreateBy
                                      }).ToList()
                    }).ToList();

            return list;
        }

        public List<DispatchEntity> SingleDispatchList(int id)
        {
            List<DispatchEntity> list = new List<DispatchEntity>();
            var amt = (from s in DB.Dispatch_Line_item
                       where s.Dispatch_Id == id
                       select s.Dispatch_Value).Sum();
            string amntInWords = AmountInWords(double.Parse(amt.ToString()));
            List<DispatchEntity> output = new List<DispatchEntity>();
            list = (from x in DB.Dispatch_Creation
                    where x.Dispatch_Id == id
                    select new DispatchEntity
                    {
                        Dispatch_Id = x.Dispatch_Id,
                        Dispatched_Location_ID = x.Dispatched_Location_ID,
                        Dispatch_Location_Code = x.Dispatch_Location_Code,
                        Dispatch_Type = x.Dispatch_Type,
                        Indent_Rls_Date = x.Indent_Rls_Date,
                        Delievery_Type = x.Delievery_Type,
                        Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                        Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                        Customer_Id = x.Customer_Id,
                        SKU_Type_Id = x.SKU_Type_Id,
                        SKU_Type = x.SKU_Type,
                        STI_Id = x.STI_Id,
                        Order_Reference = x.Order_Reference,
                        Vehicle_No = x.Vehicle_No,
                        Sales_Person_Id = x.Sales_Person_Id,
                        Sales_Person_Name = x.Sales_Person_Name,
                        Route_Id = x.Route_Id,
                        Route_Code = x.Route_Code,
                        Route = x.Route,
                        STI_Number = x.STI_Number,
                        CSI_Id = x.CSI_Id,
                        CSI_Number = x.CSI_Number,
                        Customer_code = x.Customer_code,
                        Customer_Name = x.Customer_Name,
                        Delivery_Date = x.Delivery_Date,
                        Dispatch_time_stamp = x.Dispatch_time_stamp,
                        Delivery_done_by = x.Delivery_done_by,
                        Delivery_Location_ID = x.Delivery_Location_ID,
                        Delivery_Location_Code = x.Delivery_Location_Code,
                        Expected_Delivery_date = x.Expected_Delivery_date,
                        Expected_Delivery_time = x.Expected_Delivery_time,
                        Invoice_Flag = x.Invoice_Flag,
                        Price_Template_ID = x.Price_Template_ID,
                        Price_Template_Code = x.Price_Template_Code,
                        Price_Template_Name = x.Price_Template_Name,
                        Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                        Remark = x.Remark,
                        Status = x.Status,
                        CreateBy = x.CreateBy,
                        CreateByUId = (from g in DB.User_Details
                                       where g.User_Name.ToLower() == x.CreateBy.ToLower()
                                       select g.User_id).FirstOrDefault(),
                        CreatedDate = x.CreatedDate,
                        Total_Amount = amt,
                        AMNTinWord = amntInWords,
                        STI_Details = (from s in DB.Stock_Transfer_Indent
                                       join d in DB.Dispatch_Creation on s.STI_Number equals d.STI_Number
                                       where d.STI_Id == x.STI_Id
                                       select new STIDetails
                                       {
                                           STI_Type = s.STI_Type,
                                           STI_Raised_By = s.STI_raise_by,
                                           Expected_Delivery_Date = s.DC_Delivery_Date,
                                           IndentDate = s.STI_RLS_date,
                                           Delivery_DCAddress = (from r in DB.DC_Master
                                                                 where r.DC_Code == s.Delivery_DC_Code
                                                                 select new DCAddressEntity
                                                                 {
                                                                     Address1 = r.Address1,
                                                                     Address2 = r.Address2,
                                                                     County = r.County,
                                                                     State = r.State,
                                                                     GST_Number = r.GST_Number,
                                                                     UIN_No = r.UIN_No,
                                                                     PinCode = r.PinCode,
                                                                     FSSAI_No = r.FSSAI_No,
                                                                     CIN_No = r.CIN_No,
                                                                     CST_No = r.CST_No,
                                                                     PAN_No = r.PAN_No,
                                                                     TIN_No = r.TIN_No
                                                                 }).ToList(),
                                           Source_DCAddress = (from r in DB.DC_Master
                                                               where r.DC_Code == s.Material_Source
                                                               select new DCAddressEntity
                                                               {
                                                                   Address1 = r.Address1,
                                                                   Address2 = r.Address2,
                                                                   County = r.County,
                                                                   State = r.State,
                                                                   GST_Number = r.GST_Number,
                                                                   UIN_No = r.UIN_No,
                                                                   PinCode = r.PinCode,
                                                                   FSSAI_No = r.FSSAI_No,
                                                                   CIN_No = r.CIN_No,
                                                                   CST_No = r.CST_No,
                                                                   PAN_No = r.PAN_No,
                                                                   TIN_No = r.TIN_No
                                                               }).ToList()
                                       }).ToList(),

                        DCAddress = (from r in DB.DC_Master
                                     where r.DC_Code == x.Dispatch_Location_Code
                                     select new DCAddressEntity
                                     {
                                         Address1 = r.Address1,
                                         Address2 = r.Address2,
                                         County = r.County,
                                         State = r.State,
                                         GST_Number = r.GST_Number,
                                         UIN_No = r.UIN_No,
                                         PinCode = r.PinCode,
                                         FSSAI_No = r.FSSAI_No,
                                         CIN_No = r.CIN_No,
                                         CST_No = r.CST_No,
                                         PAN_No = r.PAN_No,
                                         TIN_No = r.TIN_No
                                     }).ToList(),
                        Total_Qty_Sum = (from p in DB.Dispatch_Line_item
                                         where (p.Dispatch_Id == x.Dispatch_Id)
                                         group p by p.Dispatch_Id into g
                                         select new DIS_Qty_SumEntity
                                         {
                                             Total_Qty_Sum = g.Sum(z => z.Dispatch_Qty)

                                         }),
                        Dispatch_Qty_Sum = (from p in DB.Dispatch_Line_item
                                            where (p.Dispatch_Id == x.Dispatch_Id)
                                            group p by p.Dispatch_Id into g
                                            select new DISV_Qty_SumEntity
                                            {
                                                Dis_Qty_Sum = g.Sum(z => z.Dispatch_Value)

                                            }),
                        Total_Pack = (from p in DB.Dispatch_Line_item
                                      where (p.Dispatch_Id == x.Dispatch_Id)
                                      group p by p.Dispatch_Id into g
                                      select new pack_total
                                      {
                                          Total_Pack = g.Sum(z => z.No_of_Packed_Item)
                                      }),
                        Cartons_Sum = (from p in DB.Dispatch_Line_item
                                       where (p.Dispatch_Id == x.Dispatch_Id && p.Dispatch_Pack_Type == "Cartons")
                                            group p by p.Dispatch_Id into g
                                            select new Cartons_SumEntity
                                            {
                                                Cartons_Sum = g.Sum(z => z.No_of_Packed_Item)
                                            }),
                        Crates_Sum = (from p in DB.Dispatch_Line_item
                                      where (p.Dispatch_Id == x.Dispatch_Id && p.Dispatch_Pack_Type == "Crates")
                                       group p by p.Dispatch_Id into g
                                       select new Crates_SumEntity
                                       {
                                           Crates_Sum = g.Sum(z => z.No_of_Packed_Item)
                                       }),
                        PP_Bags_Sum = (from p in DB.Dispatch_Line_item
                                       where (p.Dispatch_Id == x.Dispatch_Id && p.Dispatch_Pack_Type == "PP Bags")
                                       group p by p.Dispatch_Id into g
                                       select new PP_Bags_SumEntity
                                       {
                                           PP_Bags_Sum = g.Sum(z => z.No_of_Packed_Item)
                                       }),
                        Gunny_Bags_Sum = (from p in DB.Dispatch_Line_item
                                          where (p.Dispatch_Id == x.Dispatch_Id && p.Dispatch_Pack_Type == "Gunny Bags")
                                       group p by p.Dispatch_Id into g
                                       select new Gunny_Bags_SumEntity
                                       {
                                           Gunny_Bags_Sum = g.Sum(z => z.No_of_Packed_Item)
                                       }),
                        CustomerAddress = (from m in DB.Customers
                                           where m.Customer_Code == x.Customer_code
                                           select new CustomerEntity
                                           {
                                               Cust_Id = m.Cust_Id,
                                               Customer_Code = m.Customer_Code,
                                               Customer_Name = m.Customer_Name,
                                               Customer_GST_Number=m.Customer_GST_Number,
                                               Customer_Ref_Number=m.Customer_Ref_Number,
                                               Customer_Tin_Number=m.Customer_Tin_Number,
                                               Address1 = m.Address1,
                                               Address2 = m.Address2,
                                               City = m.City,
                                               State = m.State,
                                               District = m.District,
                                               Pincode = m.Pincode,
                                               Primary_Contact_Name = m.Primary_Contact_Name,
                                               Contact_Number = m.Contact_Number,
                                               Primary_Email_ID = m.Primary_Email_ID,
                                               Secondary_Email_ID = m.Secondary_Email_ID,
                                               DelieveryAddresses = (from y in DB.DeliveryAddresses
                                                                   where y.Ref_Id == m.Cust_Id && y.Ref_Obj_Type == "Customer"
                                                                   select new DelieveryAddressEntity
                                                                   {
                                                                       Delivery_Address = y.Delivery_Address,
                                                                       Delivery_Contact_Person = y.Delivery_Contact_Person,
                                                                       Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
                                                                       Delivery_Location_Id = y.Delivery_Location_Id,
                                                                       Delivery_Location_Code = y.Delivery_Location_Code,
                                                                       Delivery_Location_Name = y.Delivery_Location_Name,
                                                                       Delivery_Time = y.Delivery_Time,

                                                                   }).ToList()
                                           }
                                   ).ToList(),
                                   //
                        Line_Items = (from y in DB.Dispatch_Line_item
                                      where y.Dispatch_Id == x.Dispatch_Id
                                      orderby y.SKU_Name
                                      select new DispatchLineItemsEntity
                                      {
                                          Dispatch_Line_Id = y.Dispatch_Line_Id,
                                          Dispatch_Id = y.Dispatch_Id,
                                          SKU_ID = y.SKU_ID,
                                          SKU_Code = y.SKU_Code,
                                          SKU_Name = y.SKU_Name,
                                          Pack_Type_Id = y.Pack_Type_Id,
                                          HSN_Code = y.HSN_Code,
                                          Total_GST = y.Total_GST,
                                          CGST = y.CGST,
                                          SGST = y.SGST,
                                          Dispatch_Pack_Type = y.Dispatch_Pack_Type,
                                          Pack_Type = y.Pack_Type,
                                          SKU_SubType_Id = y.SKU_SubType_Id,
                                          SKU_SubType = y.SKU_SubType,
                                          Strinkage_Qty = y.Strinkage_Qty,
                                          Pack_Weight_Type_Id = y.Pack_Weight_Type_Id,
                                          Pack_Weight_Type = y.Pack_Weight_Type,
                                          Pack_Size = y.Pack_Size,
                                          UOM = y.UOM,
                                          Grade = y.Grade,
                                          Status = y.Status,
                                          Indent_Qty = y.Indent_Qty,
                                          Dispatch_Qty = y.Dispatch_Qty,
                                          Received_Qty = y.Received_Qty,
                                          Accepted_Qty = y.Accepted_Qty,
                                          Return_Qty = y.Return_Qty,
                                          Unit_Rate = y.Unit_Rate,
                                          Dispatch_Value = y.Dispatch_Value,
                                          No_of_Packed_Item = y.No_of_Packed_Item,
                                          Converted_Unit_Value = y.Converted_Unit_Value,
                                          is_Deleted = y.is_Deleted,
                                          Remark = y.Remark,
                                          CreateBy = y.CreateBy
                                      }).ToList(),
                    }).ToList();

            foreach (var t in list)
            {
                if (t.STI_Number == "Not Applicable")
                {
                    t.STI_Details = (from s in list
                                     select new STIDetails
                                    {
                                        STI_Type = null,
                                        STI_Raised_By = null,
                                        Expected_Delivery_Date = null,
                                        IndentDate = null,
                                        Delivery_DCAddress = (from r in DB.DC_Master
                                                              where r.DC_Code == t.Delivery_Location_Code
                                                              select new DCAddressEntity
                                                              {
                                                                  Address1 = r.Address1,
                                                                  Address2 = r.Address2,
                                                                  County = r.County,
                                                                  State = r.State,
                                                                  PinCode = r.PinCode,
                                                                  GST_Number = r.GST_Number,
                                                                  UIN_No = r.UIN_No,
                                                                  FSSAI_No = r.FSSAI_No,
                                                                  CIN_No = r.CIN_No,
                                                                  CST_No = r.CST_No,
                                                                  PAN_No = r.PAN_No,
                                                                  TIN_No = r.TIN_No
                                                              }).ToList(),
                                        Source_DCAddress = (from r in DB.DC_Master
                                                            where r.DC_Code == t.Dispatch_Location_Code
                                                            select new DCAddressEntity
                                                            {
                                                                Address1 = r.Address1,
                                                                Address2 = r.Address2,
                                                                County = r.County,
                                                                State = r.State,
                                                                GST_Number = r.GST_Number,
                                                                UIN_No = r.UIN_No,
                                                                PinCode = r.PinCode,
                                                                FSSAI_No = r.FSSAI_No,
                                                                CIN_No = r.CIN_No,
                                                                CST_No = r.CST_No,
                                                                PAN_No = r.PAN_No,
                                                                TIN_No = r.TIN_No
                                                            }).ToList()
                                    }).ToList();
                }
            }

            return list;
        }

        //-------------invoice----------------------

        public stockAvail CheckStockAvalibility(string SKUName, double Qty, string SKUType, string grade, string Dc_Code)
        {
            stockAvail avalilablility = new stockAvail();

            double? Stock_Qty_Sum = 0;

            bool stk = true;

            //   bool disp = true;

            var stockQty = DB.Stocks.Where(s => s.SKU_Name == SKUName && s.DC_Code == Dc_Code && s.Grade == grade && s.SKU_Type == SKUType).Select(a => a.Closing_Qty).Sum();


            if (stockQty == null)
            {
                stockQty = 0;
                stk = false;
            }
            //
            Stock_Qty_Sum = stockQty;

            if (Stock_Qty_Sum != null)
            {
                double? total_Qty = 0.0;


                total_Qty = double.Parse(Stock_Qty_Sum.ToString());

                if (!stk)
                {
                    avalilablility.sku_status = "SKU Not Found";
                }
                else
                {
                    avalilablility.sku_status = "SKU Available";
                }

                if (Qty > total_Qty)
                {
                    avalilablility.available = "Stock Not Available";
                    avalilablility.AvailQty = total_Qty;
                    avalilablility.Grade = grade;
                    avalilablility.status = false;
                }
                else
                {                    
                    avalilablility.available = "Stock Available";
                    avalilablility.AvailQty = total_Qty;
                    avalilablility.Grade = grade;
                    avalilablility.SKU_name = SKUName;
                    avalilablility.status = true;
                }
            }
            else if ((Stock_Qty_Sum == null || Stock_Qty_Sum == 0))
            {
                avalilablility.available = "SKU Not Found";
                avalilablility.status = false;
            }
            return avalilablility;
        }
      
        public List<DispatchEntity> SearchDispatchList(int? roleId, DateTime? startDate, DateTime? endDate, string dispatchType, string status, string ULocation, string Url = "null")
        {
            List<DispatchEntity> Result = new List<DispatchEntity>();
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);
            var DispatchList = (from x in DB.Dispatch_Creation
                                where x.Dispatch_Location_Code == ULocation && x.is_Deleted == false
                                select new DispatchEntity
                              {
                                  Dispatch_Id = x.Dispatch_Id,
                                  Dispatched_Location_ID = x.Dispatched_Location_ID,
                                  Dispatch_Location_Code = x.Dispatch_Location_Code,
                                  Dispatch_Type = x.Dispatch_Type,
                                  Indent_Rls_Date = x.Indent_Rls_Date,
                                  Delievery_Type = x.Delievery_Type,
                                  Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                  Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                                  SKU_Type_Id = x.SKU_Type_Id,
                                  Order_Reference = x.Order_Reference,
                                  SKU_Type = x.SKU_Type,
                                  STI_Id = x.STI_Id,
                                  Vehicle_No = x.Vehicle_No,
                                  Sales_Person_Id = x.Sales_Person_Id,
                                  Sales_Person_Name = x.Sales_Person_Name,
                                  Route_Id = x.Route_Id,
                                  Route_Code = x.Route_Code,
                                  Route = x.Route,
                                  STI_Number = x.STI_Number,
                                  Customer_Id = x.Customer_Id,
                                  Customer_code = x.Customer_code,
                                  Customer_Name = x.Customer_Name,
                                  Customer_Location_Name = DB.Customers.Where(i => i.Customer_Code == x.Customer_code).Select(i => i.Location).FirstOrDefault(),
                                  Delivery_Date = x.Delivery_Date,
                                  Dispatch_time_stamp = x.Dispatch_time_stamp,
                                  Delivery_done_by = x.Delivery_done_by,
                                  Invoice_Flag = x.Invoice_Flag,
                                  Price_Template_ID = x.Price_Template_ID,
                                  Price_Template_Name = x.Price_Template_Name,
                                  Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                                  Delivery_Location_ID = x.Delivery_Location_ID,
                                  Delivery_Location_Code = x.Delivery_Location_Code,
                                  Expected_Delivery_date = x.Expected_Delivery_date,
                                  Expected_Delivery_time = x.Expected_Delivery_time,
                                  Remark = x.Remark,
                                  Status = x.Status,
                                  is_Create = iCrt,
                                  is_Delete = isDel,
                                  is_Edit = isEdt,
                                  is_Approval = isApp,
                                  is_View = isViw,
                                  CreateBy = x.CreateBy,
                                  CreatedDate = x.CreatedDate,
                                  lineItemsCount = (from p in DB.Dispatch_Line_item
                                                    where p.Dispatch_Id == x.Dispatch_Id
                                                    select new
                                                    {
                                                        Dispatch_Id = p.Dispatch_Id
                                                    }).Count(),
                                  Line_Items = (from y in DB.Dispatch_Line_item
                                                where y.Dispatch_Id == x.Dispatch_Id
                                                orderby y.SKU_Name
                                                select new DispatchLineItemsEntity
                                                {
                                                    Dispatch_Line_Id = y.Dispatch_Line_Id,
                                                    Dispatch_Id = y.Dispatch_Id,
                                                    SKU_ID = y.SKU_ID,
                                                    SKU_Code = y.SKU_Code,
                                                    Customer_Location_Name = DB.Customers.Where(i => i.Customer_Code == x.Customer_code).Select(i => i.Location).FirstOrDefault(),
                                                    SKU_Name = y.SKU_Name,
                                                    HSN_Code = y.HSN_Code,
                                                    Total_GST = y.Total_GST,
                                                    CGST = y.CGST,
                                                    SGST = y.SGST,
                                                    Pack_Type_Id = y.Pack_Type_Id,
                                                    Dispatch_Pack_Type = y.Dispatch_Pack_Type,
                                                    Pack_Type = y.Pack_Type,
                                                    SKU_SubType_Id = y.SKU_SubType_Id,
                                                    SKU_SubType = y.SKU_SubType,
                                                    Pack_Weight_Type_Id = y.Pack_Weight_Type_Id,
                                                    Pack_Weight_Type = y.Pack_Weight_Type,
                                                    Pack_Size = y.Pack_Size,
                                                    UOM = y.UOM,
                                                    Strinkage_Qty = y.Strinkage_Qty,
                                                    Grade = y.Grade,
                                                    Indent_Qty = y.Indent_Qty,
                                                    Dispatch_Qty = y.Dispatch_Qty,
                                                    Received_Qty = y.Received_Qty,
                                                    Accepted_Qty = y.Accepted_Qty,
                                                    Return_Qty = y.Return_Qty,
                                                    Unit_Rate = y.Unit_Rate,
                                                    Dispatch_Value = y.Dispatch_Value,
                                                    No_of_Packed_Item = y.No_of_Packed_Item,
                                                    Converted_Unit_Value = y.Converted_Unit_Value,
                                                    is_Deleted = y.is_Deleted,
                                                    Remark = x.Remark,
                                                    Status = x.Status,
                                                    CreateBy = x.CreateBy,
                                                    CreatedDate = x.CreatedDate,
                                                    UpdateBy = x.UpdateBy,
                                                    UpdateDate = x.UpdateDate,
                                                    Dispatch_Location_Code = x.Dispatch_Location_Code,
                                                    Customer_Name = x.Customer_Name,
                                                    Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                    Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                                                    Delivery_Date = x.Delivery_Date,
                                                    Indent_Rls_Date = x.Indent_Rls_Date,
                                                    Delievery_Type = x.Delievery_Type,
                                                    SKU_Type = x.SKU_Type,
                                                    Dispatch_Type = x.Dispatch_Type,
                                                    Delivery_done_by = x.Delivery_done_by,
                                                    Dispatch_time_stamp = x.Dispatch_time_stamp,
                                                    Delivery_Location_Code = x.Delivery_Location_Code,
                                                    Expected_Delivery_date = x.Expected_Delivery_date,
                                                    Expected_Delivery_time = x.Expected_Delivery_time,
                                                    STI_Number = x.STI_Number,
                                                    CSI_Number = x.CSI_Number,
                                                    Price_Template_Name = x.Price_Template_Name,
                                                    Price_Template_Valitity_upto = x.Price_Template_Valitity_upto

                                                }).ToList(),
                              });



            if (startDate.Value != null && endDate.Value != null && dispatchType != "null")
            {
                DispatchList = DispatchList.Where(x => x.Delivery_Date.Value >= startDate.Value && x.Delivery_Date.Value <= endDate.Value && x.Dispatch_Type == dispatchType);
            }
            if (status != "null")
            {
                DispatchList = DispatchList.Where(x => x.Status == status);
            }


            Result = DispatchList.ToList();

            return Result;
        }
        public List<DispatchEntity> Search(int? roleId, DateTime? startDate, DateTime? endDate, string dispatchType, string Route_Code, string Url = "null")
        {
            List<DispatchEntity> Result = new List<DispatchEntity>();
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);

            DateTime yesterDay = DateTime.UtcNow.Date.AddDays(-1);
            DateTime now = DateTime.UtcNow;
            //var query = (from x in DB.Dispatch_Creation
            //             where x.Indent_Rls_Date >= yesterDay && x.Indent_Rls_Date <= now && x.is_Deleted == false && x.Dispatch_Location_Code == Ulocation && x.Invoice_Flag == null && x.Customer_code == CustomerCode
            //             //&& x.Customer_code==CustomerCode
            //             select new cdnNumber
            //             {
            //                 CDN_Number = x.Customer_Dispatch_Number
            //             }).ToList();

            //
            var DispatchList = (from x in DB.Dispatch_Creation
                                where x.is_Deleted == false && x.Invoice_Flag==null 
                                select new DispatchEntity
                                {
                                    Dispatch_Id = x.Dispatch_Id,
                                    Dispatched_Location_ID = x.Dispatched_Location_ID,
                                    Dispatch_Location_Code = x.Dispatch_Location_Code,
                                    Dispatch_Type = x.Dispatch_Type,
                                    Acceptance_Status=x.Acceptance_Status,
                                    GRN_Flag=(x.Indent_Rls_Date >= yesterDay && x.Indent_Rls_Date <= now)?true:false,
                                    Indent_Rls_Date = x.Indent_Rls_Date,
                                    Delievery_Type = x.Delievery_Type,
                                    Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                    Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                                    SKU_Type_Id = x.SKU_Type_Id,
                                    Order_Reference = x.Order_Reference,
                                    SKU_Type = x.SKU_Type,
                                    STI_Id = x.STI_Id,
                                    Vehicle_No = x.Vehicle_No,
                                    Sales_Person_Id = x.Sales_Person_Id,
                                    Sales_Person_Name = x.Sales_Person_Name,
                                    Route_Id = x.Route_Id,
                                    Route_Code = x.Route_Code,
                                    Route = x.Route,
                                    STI_Number = x.STI_Number,
                                    Customer_Id = x.Customer_Id,
                                    Customer_code = x.Customer_code,
                                    Customer_Name = x.Customer_Name,
                                    Customer_Location_Name = DB.Customers.Where(i => i.Customer_Code == x.Customer_code).Select(i => i.Location).FirstOrDefault(),
                                    Delivery_Date = x.Delivery_Date,
                                    Dispatch_time_stamp = x.Dispatch_time_stamp,
                                    Delivery_done_by = x.Delivery_done_by,
                                    Invoice_Flag = x.Invoice_Flag,
                                    Price_Template_ID = x.Price_Template_ID,
                                    Price_Template_Name = x.Price_Template_Name,
                                    Price_Template_Valitity_upto = x.Price_Template_Valitity_upto,
                                    Delivery_Location_ID = x.Delivery_Location_ID,
                                    Delivery_Location_Code = x.Delivery_Location_Code,
                                    Expected_Delivery_date = x.Expected_Delivery_date,
                                    Expected_Delivery_time = x.Expected_Delivery_time,
                                    Remark = x.Remark,
                                    Status = x.Status,
                                    is_Create = iCrt,
                                    is_Delete = isDel,
                                    is_Edit = isEdt,
                                    is_Approval = isApp,
                                    is_View = isViw,
                                    CreateBy = x.CreateBy,
                                    CreatedDate = x.CreatedDate,
                                    lineItemsCount = (from p in DB.Dispatch_Line_item
                                                      where p.Dispatch_Id == x.Dispatch_Id
                                                      select new
                                                      {
                                                          Dispatch_Id = p.Dispatch_Id
                                                      }).Count(),
                                    Line_Items = (from y in DB.Dispatch_Line_item
                                                  where y.Dispatch_Id == x.Dispatch_Id
                                                  orderby y.SKU_Name
                                                  select new DispatchLineItemsEntity
                                                  {
                                                      Dispatch_Line_Id = y.Dispatch_Line_Id,
                                                      Dispatch_Id = y.Dispatch_Id,
                                                      SKU_ID = y.SKU_ID,
                                                      SKU_Code = y.SKU_Code,
                                                      Customer_Location_Name = DB.Customers.Where(i => i.Customer_Code == x.Customer_code).Select(i => i.Location).FirstOrDefault(),
                                                      SKU_Name = y.SKU_Name,
                                                      HSN_Code = y.HSN_Code,
                                                      Total_GST = y.Total_GST,
                                                      CGST = y.CGST,
                                                      SGST = y.SGST,
                                                      Pack_Type_Id = y.Pack_Type_Id,
                                                      Dispatch_Pack_Type = y.Dispatch_Pack_Type,
                                                      Pack_Type = y.Pack_Type,
                                                      SKU_SubType_Id = y.SKU_SubType_Id,
                                                      SKU_SubType = y.SKU_SubType,
                                                      Pack_Weight_Type_Id = y.Pack_Weight_Type_Id,
                                                      Pack_Weight_Type = y.Pack_Weight_Type,
                                                      Pack_Size = y.Pack_Size,
                                                      UOM = y.UOM,
                                                      Strinkage_Qty = y.Strinkage_Qty,
                                                      Grade = y.Grade,
                                                      Indent_Qty = y.Indent_Qty,
                                                      Dispatch_Qty = y.Dispatch_Qty,
                                                      Received_Qty = y.Received_Qty,
                                                      Accepted_Qty = y.Accepted_Qty,
                                                      Return_Qty = y.Return_Qty,
                                                      Unit_Rate = y.Unit_Rate,
                                                      Dispatch_Value = y.Dispatch_Value,
                                                      No_of_Packed_Item = y.No_of_Packed_Item,
                                                      Converted_Unit_Value = y.Converted_Unit_Value,
                                                      is_Deleted = y.is_Deleted,
                                                      Remark = x.Remark,
                                                      Status = x.Status,
                                                      CreateBy = x.CreateBy,
                                                      CreatedDate = x.CreatedDate,
                                                      UpdateBy = x.UpdateBy,
                                                      UpdateDate = x.UpdateDate,
                                                      Dispatch_Location_Code = x.Dispatch_Location_Code,
                                                      Customer_Name = x.Customer_Name,
                                                      Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                      Stock_Xfer_Dispatch_Number = x.Stock_Xfer_Dispatch_Number,
                                                      Delivery_Date = x.Delivery_Date,
                                                      Indent_Rls_Date = x.Indent_Rls_Date,
                                                      Delievery_Type = x.Delievery_Type,
                                                      SKU_Type = x.SKU_Type,
                                                      Dispatch_Type = x.Dispatch_Type,
                                                      Delivery_done_by = x.Delivery_done_by,
                                                      Dispatch_time_stamp = x.Dispatch_time_stamp,
                                                      Delivery_Location_Code = x.Delivery_Location_Code,
                                                      Expected_Delivery_date = x.Expected_Delivery_date,
                                                      Expected_Delivery_time = x.Expected_Delivery_time,
                                                      STI_Number = x.STI_Number,
                                                      CSI_Number = x.CSI_Number,
                                                      Price_Template_Name = x.Price_Template_Name,
                                                      Price_Template_Valitity_upto = x.Price_Template_Valitity_upto

                                                  }).ToList(),
                                });



            if (startDate.Value != null && endDate.Value != null && dispatchType != "null")
            {
                DispatchList = DispatchList.Where(x => x.Delivery_Date.Value >= startDate.Value && x.Delivery_Date.Value <= endDate.Value && x.Dispatch_Type == dispatchType);
            }
            if (Route_Code != "null")
            {
                DispatchList = DispatchList.Where(x => x.Route_Code == Route_Code);
            }


            Result = DispatchList.ToList();

            return Result;
        }
        public Invoice_NUM_Generation InvoiceAutoIncrement(string locationId)
        {
            var autoinc = DB.Invoice_NUM_Generation.Where(x => x.DC_Code == locationId).FirstOrDefault();

            var model = new Invoice_NUM_Generation
            {

                Invoice_Num_Gen_Id = autoinc.Invoice_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Invoice_Last_Number = autoinc.Invoice_Last_Number
            };

            return model;
        }

        //-------------invoice----------------------
        public List<InvoiceEntity> GetSingleInvoiceList(int id)
        {
            List<InvoiceEntity> list = new List<InvoiceEntity>();

            var amt = (from s in DB.Invoice_Line_item
                       where s.Invoice_Id == id
                       select s.Invoice_Amount).Sum();

            string amntInWords = AmountInWords(double.Parse(amt.ToString()));

            list = (from x in DB.Invoice_Creation
                    where x.invoice_Id == id
                    select new InvoiceEntity
                    {
                        invoice_Id = x.invoice_Id,
                        Invoice_Number = x.Invoice_Number,
                        Dispatch_id = x.Dispatch_id,
                        Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                        Invoice_Type = x.Invoice_Type,
                        Order_Reference = x.Order_Reference,
                        Invoice_Date = x.Invoice_Date,
                        SKU_Type_Id = x.SKU_Type_Id,
                        SKU_Type = x.SKU_Type,
                        Customer_Id = x.Customer_Id,
                        Customer_code = x.Customer_code,
                        Customer_Name = x.Customer_Name,
                        CSI_Details = (from z in DB.Dispatch_Creation
                                       where z.Customer_Dispatch_Number == x.Customer_Dispatch_Number
                                       select new csi_details
                                       {
                                           CSI_Id = z.CSI_Id,
                                           CSI_Number = z.CSI_Number
                                       }).ToList(),     
                     
                        //CustAddress = (from y in DB.Customers
                        //               where y.Customer_Code == x.Customer_code
                        //               select new CustAddressEntity
                        //               {
                        //                   Address1 = y.Address1,
                        //                   Address2 = y.Address2,
                        //                   Contact_Number = y.Contact_Number,
                        //                   City = y.City,
                        //                   Customer_GST_Number = y.Customer_GST_Number,
                        //                   Customer_Ref_Number = y.Customer_Ref_Number,
                        //                   Customer_Tin_Number = y.Customer_Tin_Number,
                        //                   District = y.District,
                        //                   State = y.State,
                        //                   Pincode = y.Pincode,
                        //                   Primary_Contact_Name = y.Primary_Contact_Name,
                        //                   Primary_Email_ID = y.Primary_Email_ID,
                        //                   Secondary_Email_ID = y.Secondary_Email_ID,
                        //                   DelieveryAddress = (from z in DB.DeliveryAddresses
                        //                                         where z.Ref_Id == y.Cust_Id && z.Ref_Obj_Type == "Customer"
                        //                                         select new DelieveryAddressEntity
                        //                                         {
                        //                                             Delivery_Address = y.Delivery_Address,
                        //                                             Delivery_Contact_Person = y.Delivery_Contact_Person,
                        //                                             Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
                        //                                             Delivery_Location_Id = y.Delivery_Location_Id,
                        //                                             Delivery_Location_Code = y.Delivery_Location_Code,
                        //                                             Delivery_Location_Name = y.Delivery_Location_Name,
                        //                                             Delivery_Time = z.Delivery_Time,

                        //                                         }).ToList()
                        //               }).ToList(),
                        CustAddress = (from m in DB.Customers
                                           where m.Customer_Code == x.Customer_code
                                       select new CustAddressEntity
                                           {                                             
                                               Customer_GST_Number = m.Customer_GST_Number,
                                               Customer_Ref_Number = m.Customer_Ref_Number,
                                               Customer_Tin_Number = m.Customer_Tin_Number,
                                               Address1 = m.Address1,
                                               Address2 = m.Address2,
                                               City = m.City,
                                               State = m.State,
                                               District = m.District,
                                               Pincode = m.Pincode,
                                               Primary_Contact_Name = m.Primary_Contact_Name,
                                               Contact_Number = m.Contact_Number,
                                               Primary_Email_ID = m.Primary_Email_ID,
                                               Secondary_Email_ID = m.Secondary_Email_ID,
                                               DelieveryAddress = (from y in DB.DeliveryAddresses
                                                                     where y.Ref_Id == m.Cust_Id && y.Ref_Obj_Type == "Customer"
                                                                     select new DelieveryAddressEntity
                                                                     {
                                                                         Delivery_Address = y.Delivery_Address,
                                                                         Delivery_Contact_Person = y.Delivery_Contact_Person,
                                                                         Delivery_Contact_Person_No = y.Delivery_Contact_Person_No,
                                                                         Delivery_Location_Id = y.Delivery_Location_Id,
                                                                         Delivery_Location_Code = y.Delivery_Location_Code,
                                                                         Delivery_Location_Name = y.Delivery_Location_Name,
                                                                         Delivery_Time = y.Delivery_Time,
                                                                     }).ToList()
                                           }).ToList(),
                        DC_LID = x.DC_ID,
                        DC_LCode = x.DC_Code,
                        CDNCreatedDate = (from d in DB.Invoice_Creation
                                          join c in DB.Dispatch_Creation on d.Customer_Dispatch_Number equals c.Customer_Dispatch_Number
                                          where d.invoice_Id == id
                                          select new CDNDate
                                          {
                                              CDNCDate = c.CreatedDate
                                          }).ToList(),
                        DCAddress = (from r in DB.DC_Master
                                     where r.DC_Code.Contains(x.DC_Code)
                                     select new DCAddressEntity
                                     {
                                         Address1 = r.Address1,
                                         Address2 = r.Address2,
                                         City = r.City,
                                         County = r.County,
                                         State = r.State,
                                         PinCode = r.PinCode,
                                         CIN_No = r.CIN_No,
                                         GST_Number=r.GST_Number,
                                         FSSAI_No = r.FSSAI_No,
                                         CST_No = r.CST_No,
                                         PAN_No = r.PAN_No,
                                         TIN_No = r.TIN_No
                                     }).ToList(),
                        Term_of_Payment = x.Term_of_Payment,
                        Supplier_Ref = x.Supplier_Ref,
                        Buyer_Order_No = x.Buyer_Order_No,
                        Order_Date = x.Order_Date,
                        Remark = x.Remark,
                        is_Invoice_Approved = x.is_Invoice_Approved,
                        is_Invoice_approved_by = x.is_Invoice_approved_by,
                        is_Invoice_approved_date = x.is_Invoice_approved_date,
                        is_Invoice_approved_user_id = x.is_Invoice_approved_user_id,
                        CreateBy = x.CreateBy,
                        CreatedDate = x.CreatedDate,
                        Total_Amount = amt,
                        AMNTinWord = amntInWords,
                        InvoiceLineItems = (from y in DB.Invoice_Line_item
                                            where y.Invoice_Id == x.invoice_Id
                                            orderby y.SKU_Name
                                            select new InvoiceLineItemEntity
                                            {
                                                Invoice_Line_Id = y.Invoice_Line_Id,
                                                Invoice_Id = y.Invoice_Id,
                                                Invoice_Number = y.Invoice_Number,
                                                Dispatch_Line_id = y.Dispatch_Line_id,
                                                SKU_ID = y.SKU_ID,
                                                SKU_Code = y.SKU_Code,
                                                SKU_Name = y.SKU_Name,
                                                HSN_Code = y.HSN_Code,
                                                Total_GST = y.Total_GST,
                                                CGST = y.CGST,
                                                SGST = y.SGST,
                                                Pack_Type_Id = y.Pack_Type_Id,
                                                Pack_Type = y.Pack_Type,
                                                SKU_SubType_Id = y.SKU_SubType_Id,
                                                SKU_SubType = y.SKU_SubType,
                                                Pack_Size = y.Pack_Size,
                                                Return_Qty = y.Return_Qty,
                                                UOM = y.UOM,
                                                Grade = y.Grade,
                                                Invoice_Qty = y.Invoice_Qty,
                                                Rate = y.Rate,
                                                Discount = y.Discount,
                                                Invoice_Amount = y.Invoice_Amount,
                                                Converted_Unit_Value = y.Converted_Unit_Value,
                                                CreatedDate = y.CreatedDate,
                                                Dispatch_Qty = y.Dispatch_Qty,
                                                CreateBy = y.CreateBy,
                                          }).ToList(),
                    }).ToList();

            return list;
        }
        //
        //--------------------------------------INVOICE-------------------------------------------------------------
        public string AmountInWords(double? amount)
        {
            var n = (int)amount;

            if (n == 0)
                return "";
            else if (n > 0 && n <= 19)
            {
                var arr = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                return arr[n - 1] + " ";
            }
            else if (n >= 20 && n <= 99)
            {
                var arr = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                return arr[n / 10 - 2] + " " + AmountInWords(n % 10);
            }
                //
            else if (n >= 100 && n <= 199)
            {
                return "One Hundred " + AmountInWords(n % 100);
            }
            else if (n >= 200 && n <= 999)
            {
                return AmountInWords(n / 100) + "Hundred " + AmountInWords(n % 100);
            }
            else if (n >= 1000 && n <= 1999)
            {
                return "One Thousand " + AmountInWords(n % 1000);
            }
            else if (n >= 2000 && n <= 999999)
            {
                return AmountInWords(n / 1000) + "Thousand " + AmountInWords(n % 1000);
            }
            else if (n >= 1000000 && n <= 1999999)
            {
                return "One Million " + AmountInWords(n % 1000000);
            }
            else if (n >= 1000000 && n <= 999999999)
            {
                return AmountInWords(n / 1000000) + "Million " + AmountInWords(n % 1000000);
            }
            else if (n >= 1000000000 && n <= 1999999999)
            {
                return "One Billion " + AmountInWords(n % 1000000000);
            }
            else
            {
                return AmountInWords(n / 1000000000) + "Billion " + AmountInWords(n % 1000000000);
            }
        }

        public List<DispatchNumber> GetDispatchNumbers(DateTime? date, string Ulocation)
        {
            var query = (from x in DB.Dispatch_Creation
                         where x.Delivery_Date.Value.Year == date.Value.Year && x.Delivery_Date.Value.Month == date.Value.Month && x.Delivery_Date.Value.Day == date.Value.Day && x.is_Deleted == false && x.Dispatch_Type == "Customer" && x.Invoice_Flag == false && x.Dispatch_Location_Code == Ulocation
                         select new DispatchNumber
                         {
                             Customer_Dispatch_Number = x.Customer_Dispatch_Number
                         }).ToList();
            return query;
        }


        public List<InvoiceEntity> SearchInvoiceList(int? roleId, DateTime? startDate, DateTime? endDate, string Ulocation, string Url = "null")
        {

            List<InvoiceEntity> Result = new List<InvoiceEntity>();
            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges
                      ).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);
            var list = (from x in DB.Invoice_Creation
                        where x.is_Invoice_Approved == true && x.DC_Code == Ulocation && x.is_Deleted == false
                        select new InvoiceEntity
                        {
                            invoice_Id = x.invoice_Id,
                            Invoice_Number = x.Invoice_Number,
                            Dispatch_id = x.Dispatch_id,
                            Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                            Invoice_Type = x.Invoice_Type,
                            SKU_Type_Id = x.SKU_Type_Id,
                            SKU_Type = x.SKU_Type,
                            Order_Reference = x.Order_Reference,
                            Invoice_Date = x.Invoice_Date,
                            Customer_Id = x.Customer_Id,
                            Customer_code = x.Customer_code,
                            Customer_Name = x.Customer_Name,
                            DC_LID = x.DC_ID,
                            DC_LCode = x.DC_Code,
                            Term_of_Payment = x.Term_of_Payment,
                            Supplier_Ref = x.Supplier_Ref,
                            Buyer_Order_No = x.Buyer_Order_No,
                            Order_Date = x.Order_Date,
                            Remark = x.Remark,
                            is_Invoice_Approved = x.is_Invoice_Approved,
                            is_Invoice_approved_by = x.is_Invoice_approved_by,
                            is_Invoice_approved_date = x.is_Invoice_approved_date,
                            is_Invoice_approved_user_id = x.is_Invoice_approved_user_id,
                            CreateBy = x.CreateBy,
                            CreatedDate = x.CreatedDate,
                            is_Create = iCrt,
                            is_Delete = isDel,
                            is_Edit = isEdt,
                            is_Approval = isApp,
                            is_View = isViw,
                            IlineItemsCount = (from p in DB.Invoice_Line_item
                                               where p.Invoice_Number == x.Invoice_Number
                                               select new
                                               {
                                                   Dispatch_Id = p.Invoice_Number
                                               }).Count(),
                            InvoiceLineItems = (from y in DB.Invoice_Line_item
                                                where y.Invoice_Id == x.invoice_Id
                                                orderby y.SKU_Name
                                                select new InvoiceLineItemEntity
                                                {
                                                    Invoice_Line_Id = y.Invoice_Line_Id,
                                                    Invoice_Id = y.Invoice_Id,
                                                    Invoice_Number = y.Invoice_Number,
                                                    Dispatch_Line_id = y.Dispatch_Line_id,
                                                    SKU_ID = y.SKU_ID,
                                                    SKU_Code = y.SKU_Code,
                                                    SKU_Name = y.SKU_Name,
                                                    Pack_Type_Id = y.Pack_Type_Id,
                                                    Pack_Type = y.Pack_Type,
                                                    HSN_Code = y.HSN_Code,
                                                    Total_GST = y.Total_GST,
                                                    CGST = y.CGST,
                                                    SGST = y.SGST,
                                                    SKU_SubType_Id = y.SKU_SubType_Id,
                                                    SKU_SubType = y.SKU_SubType,
                                                    Pack_Size = y.Pack_Size,
                                                    Return_Qty = y.Return_Qty,
                                                    UOM = y.UOM,
                                                    Grade = y.Grade,
                                                    Invoice_Qty = y.Invoice_Qty,
                                                    Rate = y.Rate,
                                                    Discount = y.Discount,
                                                    Invoice_Amount = y.Invoice_Amount,
                                                    Converted_Unit_Value = y.Converted_Unit_Value,
                                                    CreatedDate = x.CreatedDate,
                                                    Dispatch_Qty = y.Dispatch_Qty,
                                                    CreateBy = x.CreateBy,
                                                    UpdateBy = x.UpdateBy,
                                                    UpdateDate = x.UpdateDate,
                                                    Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                                                    Invoice_Date = x.Invoice_Date,
                                                    Invoice_Type = x.Invoice_Type,
                                                    SKU_Type = x.SKU_Type,
                                                    Customer_Name = x.Customer_Name,
                                                    DC_LCode = x.DC_Code,
                                                    Term_of_Payment = x.Term_of_Payment,
                                                    Supplier_Ref = x.Supplier_Ref,
                                                    Buyer_Order_No = x.Buyer_Order_No,
                                                    Order_Date = x.Order_Date,
                                                    Remark = x.Customer_Dispatch_Number


                                                }).ToList(),
                        });


            if (startDate.Value != null && endDate.Value != null)
            {
                list = list.Where(x => x.Invoice_Date.Value >= startDate.Value && x.Invoice_Date.Value <= endDate.Value);
            }


            Result = list.ToList();

            return Result;
        }
        public List<InvoiceEntity> GetIVNA(int? roleId, DateTime? startDate, DateTime? endDate, string Ulocation, string Url)
        {

            var menuAccess = (from t in DB.Role_Menu_Access
                              join s in DB.Menu_Master on t.Menu_Id equals s.Menu_Id
                              where t.Role_Id == roleId && s.Url == Url
                              select t.Menu_Previlleges
     ).FirstOrDefault();
            int isDel, isViw, isEdt, isApp, iCrt;

            iCrt = Convert.ToInt32(JObject.Parse(menuAccess)["Add"]);
            isDel = Convert.ToInt32(JObject.Parse(menuAccess)["Delete"]);
            isEdt = Convert.ToInt32(JObject.Parse(menuAccess)["Edit"]);
            isApp = Convert.ToInt32(JObject.Parse(menuAccess)["Approval"]);
            isViw = Convert.ToInt32(JObject.Parse(menuAccess)["View"]);
            var list = (from x in DB.Invoice_Creation
                        where (x.Invoice_Date.Value >= startDate.Value) && (x.Invoice_Date.Value <= endDate.Value) && x.is_Invoice_Approved == null && x.DC_Code == Ulocation
                        select new InvoiceEntity
                     {
                         invoice_Id = x.invoice_Id,
                         Invoice_Number = x.Invoice_Number,
                         Dispatch_id = x.Dispatch_id,
                         Customer_Dispatch_Number = x.Customer_Dispatch_Number,
                         Invoice_Type = x.Invoice_Type,
                         Invoice_Date = x.Invoice_Date,
                         SKU_Type_Id = x.SKU_Type_Id,
                         SKU_Type = x.SKU_Type,
                         Customer_Id = x.Customer_Id,
                         Customer_code = x.Customer_code,
                         Customer_Name = x.Customer_Name,
                         DC_LID = x.DC_ID,
                         DC_LCode = x.DC_Code,
                         Term_of_Payment = x.Term_of_Payment,
                         Supplier_Ref = x.Supplier_Ref,
                         Buyer_Order_No = x.Buyer_Order_No,
                         Order_Date = x.Order_Date,
                         Remark = x.Remark,
                         is_Invoice_Approved = x.is_Invoice_Approved,
                         is_Invoice_approved_by = x.is_Invoice_approved_by,
                         is_Invoice_approved_date = x.is_Invoice_approved_date,
                         is_Invoice_approved_user_id = x.is_Invoice_approved_user_id,
                         CreateBy = x.CreateBy,
                         CreatedDate = x.CreatedDate,
                         is_Create = iCrt,
                         is_Delete = isDel,
                         is_Edit = isEdt,
                         is_Approval = isApp,
                         is_View = isViw,
                         IlineItemsCount = (from p in DB.Invoice_Line_item
                                            where p.Invoice_Number == x.Invoice_Number
                                            select new
                                            {
                                                Dispatch_Id = p.Invoice_Number
                                            }).Count()
                     }).ToList();


            return list;
        }

    }
}
