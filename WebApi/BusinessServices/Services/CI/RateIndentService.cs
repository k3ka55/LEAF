using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class RateIndentService : IRateIndentService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public RateIndentService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public Rate_Template_Num_Gen GetRateTempAutoIncrement(string locationId)
        {
            var autoinc = DB.Rate_Template_Num_Gen.Where(x => x.DC_Code == locationId).FirstOrDefault();

            var model = new Rate_Template_Num_Gen
            {
                Rate_Template_Num_Gen_Id = autoinc.Rate_Template_Num_Gen_Id,
                DC_Code = autoinc.DC_Code,
                Financial_Year = autoinc.Financial_Year,
                Rate_Template_Last_Number = autoinc.Rate_Template_Last_Number
            };

            return model;
        }

        public List<RateTemplateSearchUniqueEntity> CheckUnion(RateIndentEntityUnique rate)
        {
            List<RateTemplateSearchUniqueEntity> aa = new List<RateTemplateSearchUniqueEntity>();
            List<RateTemplateSearchUniqueEntity> bb = new List<RateTemplateSearchUniqueEntity>();
            aa = rate.LineItems;

            var query = (aa.GroupBy(c => new { c.UOM, c.Grade, c.Pack_Type, c.Pack_Size, c.Pack_Weight_Type, c.SKU_Name, c.SKU_SubType })
                        .Select(g => new RateTemplateSearchUniqueEntity
                        {

                            UOM = g.Key.UOM,
                           // Material_Auto_Gen_Code = g.Key.Material_Auto_Gen_Code,
                            Pack_Size = g.Key.Pack_Size,
                            Pack_Weight_Type = g.Key.Pack_Weight_Type,
                            Pack_Type = g.Key.Pack_Type,
                            Grade = g.Key.Grade,
                            SKU_SubType = g.Key.SKU_SubType,
                            SKU_Name = g.Key.SKU_Name,

                        })).ToList();
            foreach (var y in query)
            {
                var hh = new RateTemplateSearchUniqueEntity();
                string sellingpriCe = aa.Where(a => a.Grade == y.Grade && a.UOM == y.UOM && a.Pack_Size == y.Pack_Size && a.Pack_Type == y.Pack_Type && a.Pack_Weight_Type == y.Pack_Weight_Type && a.SKU_Name == y.SKU_Name && a.SKU_SubType == y.SKU_SubType).Select(a => a.Selling_Price).FirstOrDefault();
                string Material_Auto_GenCode = aa.Where(a => a.Grade == y.Grade && a.UOM == y.UOM && a.Pack_Size == y.Pack_Size && a.Pack_Type == y.Pack_Type && a.Pack_Weight_Type == y.Pack_Weight_Type && a.SKU_Name == y.SKU_Name && a.SKU_SubType == y.SKU_SubType).Select(a => a.Material_Auto_Gen_Code).FirstOrDefault();
                hh.Grade = y.Grade;
                hh.Material_Auto_Gen_Code = Material_Auto_GenCode;
                hh.Pack_Size = y.Pack_Size;
                hh.Pack_Type = y.Pack_Type;
                hh.Pack_Weight_Type = y.Pack_Weight_Type;
                hh.Selling_Price = sellingpriCe;
                hh.SKU_Name = y.SKU_Name;
                hh.UOM = y.UOM;
                hh.SKU_SubType = y.SKU_SubType;
                bb.Add(hh);
            }

            return bb;
        }


        public bool checktempName(string name)
        {
            bool result = false;

            var list = DB.Rate_Template.Where(x => x.Template_Name == name && x.Is_Deleted == false).FirstOrDefault();

            if (list != null)
            {
                result = true;
            }

            return result;
        }

        public int CreateRateIndent(RateIndentEntity rateEntity)
        {
            string rateTempNumber, STI_prefix;
            int? incNumber;

            string locationID = "";


            using (var iscope = new TransactionScope())
            {
                if (rateEntity.Location_Code != null && rateEntity.Location_Code != "null")
                    locationID = rateEntity.Location_Code;
                else if (rateEntity.DC_Code != null && rateEntity.DC_Code != "null")
                    locationID = rateEntity.DC_Code;

                // string locationID = ciEntity.Location_Code;
                ResourceManager rm = new ResourceManager("BusinessServices.AutoGenerate", Assembly.GetExecutingAssembly());
                STI_prefix = rm.GetString("RIT");
                Rate_Template_Num_Gen autoIncNumber = GetRateTempAutoIncrement(locationID);
                locationID = autoIncNumber.DC_Code;
                incNumber = autoIncNumber.Rate_Template_Last_Number;
                int? incrementedValue = incNumber + 1;
                var STincrement = DB.Rate_Template_Num_Gen.Where(x => x.DC_Code == locationID).FirstOrDefault();
                STincrement.Rate_Template_Last_Number = incrementedValue;
                _unitOfWork.RateTemplateNumGenRepository.Update(STincrement);
                _unitOfWork.Save();
                rateTempNumber = STI_prefix + "/" + locationID + "/" + String.Format("{0:00000}", incNumber);

                iscope.Complete();
            }




            string tmeplateName = "";

            //if (rateEntity.DC_Code != null && rateEntity.Location_Code == "null")
            //{
            //    tmeplateName = rateEntity.Region_Code + "/" + rateEntity.DC_Code + "/" + rateEntity.SKU_Type_Code + "/" + rateEntity.Category_Code;
            //}
            //else if (rateEntity.DC_Code == "null" && rateEntity.Location_Code != null)
            //{
            //    tmeplateName = rateEntity.Region_Code + "/" + rateEntity.Location_Code + "/" + rateEntity.SKU_Type_Code + "/" + rateEntity.Category_Code;
            //}

            tmeplateName = rateEntity.Template_Name;

            bool checkName = checktempName(tmeplateName);
            if (checkName)
            {
                return -1;
            }
            using (var scope = new TransactionScope())
            {
                var rateTemplate = new Rate_Template
                {
                    Template_Name = tmeplateName,
                    Template_Code = rateTempNumber,
                    DC_Id = rateEntity.DC_Id,
                    DC_Code = rateEntity.DC_Code,
                    Location_Id = rateEntity.Location_Id,
                    Location_Code = rateEntity.Location_Code,
                    Region_Id = rateEntity.Region_Id,
                    Region = rateEntity.Region,
                    Region_Code = rateEntity.Region_Code,
                    Customer_Category_Code = rateEntity.Customer_Category_Code,
                    // DC_Location = rateEntity.DC_Location,
                    SKU_Type_Id = rateEntity.SKU_Type_Id,
                    SKU_Type = rateEntity.SKU_Type,
                    Customer_Category = rateEntity.Customer_Category,
                    Valitity_upto = rateEntity.Valitity_upto,
                    CreatedDate = DateTime.UtcNow,
                    CreateBy = rateEntity.CreateBy,
                    Category_Id = rateEntity.Category_Id,
                    SKU_Type_Code = rateEntity.SKU_Type_Code,
                    Is_Deleted = false,
                    Is_BNG_Synced = false,
                    Is_JDM_Synced = false
                };

                _unitOfWork.RateTemplateRepository.Insert(rateTemplate);
                _unitOfWork.Save();

                int? tId = rateTemplate.Template_ID;

                var model = new Rate_Template_Line_item();
                foreach (RateTemplateLineitem pSub in rateEntity.LineItems)
                {
                    model.RT_id = tId;
                    model.Rate_Template_Code = rateTempNumber;
                    model.Material_Auto_Num_Code = pSub.Material_Auto_Num_Code;
                    model.Material_Code = pSub.Material_Code;
                    model.SKU_Id = pSub.SKU_Id;
                    model.HSN_Code = pSub.HSN_Code;
                    model.CGST = pSub.CGST;
                    model.SGST = pSub.SGST;
                    model.Total_GST = pSub.Total_GST;
                    model.SKU_Code = pSub.SKU_Code;
                    model.SKU_Name = pSub.SKU_Name;
                    model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                    model.SKU_SubType = pSub.SKU_SubType;
                    model.Pack_Type_Id = pSub.Pack_Type_Id;
                    model.Pack_Type = pSub.Pack_Type;
                    model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                    model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                    model.UOM = pSub.UOM;
                    model.Pack_Size = pSub.Pack_Size;
                    model.Grade = pSub.Grade;
                    model.Selling_price = pSub.Selling_price;
                    model.MRP = pSub.MRP;

                    // model.Price = pSub.Price;
                    model.CreatedDate = DateTime.Now;
                    model.CreateBy = pSub.CreateBy;

                    _unitOfWork.RateTemplateLineItemRepository.Insert(model);
                    _unitOfWork.Save();

                }
                scope.Complete();
                return rateTemplate.Template_ID;
            }
        }

        public int UpdateRateIndent(int tId, RateIndentEntity rateEntity)
        {
            if (rateEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.RateTemplateRepository.GetByID(tId);
                    if (p != null)
                    {
                        p.Template_Name = rateEntity.Template_Name;
                        p.DC_Id = rateEntity.DC_Id;
                        p.DC_Code = rateEntity.DC_Code;
                        p.Location_Id = rateEntity.Location_Id;
                        p.Location_Code = rateEntity.Location_Code;
                        p.Region_Id = rateEntity.Region_Id;
                        p.Region = rateEntity.Region;
                        p.Region_Code = rateEntity.Region_Code;
                        p.Category_Id = rateEntity.Category_Id;
                        p.SKU_Type_Code = rateEntity.SKU_Type_Code;
                        p.Customer_Category_Code = rateEntity.Customer_Category_Code;
                        // p.DC_Location = rateEntity.DC_Location;
                        p.SKU_Type_Id = rateEntity.SKU_Type_Id;
                        p.SKU_Type = rateEntity.SKU_Type;
                        p.Customer_Category = rateEntity.Customer_Category;
                        //  p.Template_Code = rateEntity.Template_Code;
                        p.Valitity_upto = rateEntity.Valitity_upto;
                        p.UpdateDate = DateTime.Now;
                        p.UpdateBy = rateEntity.UpdateBy;
                        p.Is_JDM_Synced = false;
                        p.Is_BNG_Synced = false;
                        p.Is_Deleted = false;

                        _unitOfWork.RateTemplateRepository.Update(p);
                        _unitOfWork.Save();
                    }

                    var lines = DB.Rate_Template_Line_item.Where(x => x.RT_id == tId).ToList();

                    foreach (var litem in lines)
                    {
                        using (var scope1 = new TransactionScope())
                        {
                            var lineItem = _unitOfWork.RateTemplateLineItemRepository.GetByID(litem.RT_Line_Id);
                            if (lineItem != null)
                            {

                                _unitOfWork.RateTemplateLineItemRepository.Delete(lineItem);
                                _unitOfWork.Save();

                            }
                            scope1.Complete();
                        }
                    }

                    foreach (RateTemplateLineitem pSub in rateEntity.LineItems)
                    {
                        //var line = _unitOfWork.RateTemplateLineItemRepository.GetByID(pSub.RT_Line_Id);
                        var model = new Rate_Template_Line_item();

                        //if (line != null)
                        //{
                        //    line.RT_id = tId;
                        //    line.SKU_Id = pSub.SKU_Id;
                        //    line.SKU_Name = pSub.SKU_Name;
                        //    line.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        //    line.SKU_SubType = pSub.SKU_SubType;
                        //    line.UOM = pSub.UOM;
                        //    line.Pack_Type_Id = pSub.Pack_Type_Id;
                        //    line.Pack_Type = pSub.Pack_Type;
                        //    line.Pack_Size = pSub.Pack_Size;
                        //    line.Grade = pSub.Grade;
                        //    line.Price = pSub.Price;
                        //    line.UpdateDate = DateTime.Now;
                        //    line.UpdateBy = pSub.UpdateBy;

                        //    _unitOfWork.RateTemplateLineItemRepository.Update(line);
                        //    _unitOfWork.Save();
                        //}
                        //else
                        //{
                        model.RT_id = tId;
                        model.Rate_Template_Code = rateEntity.Template_Code;
                        model.Material_Auto_Num_Code = pSub.Material_Auto_Num_Code;
                        model.Material_Code = pSub.Material_Code;
                        model.SKU_Id = pSub.SKU_Id;
                        model.SKU_Code = pSub.SKU_Code;
                        model.SKU_Name = pSub.SKU_Name;
                        model.SKU_SubType_Id = pSub.SKU_SubType_Id;
                        model.SKU_SubType = pSub.SKU_SubType;
                        model.Pack_Type_Id = pSub.Pack_Type_Id;
                        model.Pack_Type = pSub.Pack_Type;
                        model.Pack_Size = pSub.Pack_Size;
                        model.HSN_Code = pSub.HSN_Code;
                        model.CGST = pSub.CGST;
                        model.SGST = pSub.SGST;
                        model.Total_GST = pSub.Total_GST;
                        model.Pack_Weight_Type_Id = pSub.Pack_Weight_Type_Id;
                        model.Pack_Weight_Type = pSub.Pack_Weight_Type;
                        model.UOM = pSub.UOM;
                        model.Grade = pSub.Grade;
                        // model.Price = pSub.Price;
                        model.Selling_price = pSub.Selling_price;
                        model.MRP = pSub.MRP;
                        model.UpdateDate = DateTime.Now;
                        model.UpdateBy = pSub.UpdateBy;

                        _unitOfWork.RateTemplateLineItemRepository.Insert(model);
                        _unitOfWork.Save();
                        //  }
                    }
                    scope.Complete();
                }
            }
            return tId;
        }


        public ReturnRate GetRateForCsi(RateInformation rateDetail)
        {
            var rate = (from x in DB.Rate_Template
                        join y in DB.Rate_Template_Line_item on x.Template_Code equals y.Rate_Template_Code
                        where x.Template_Code == rateDetail.TemplateID && x.Is_Deleted == false && y.SKU_Id == rateDetail.sku_id && y.SKU_SubType_Id == rateDetail.sku_subtype_id && y.Pack_Type_Id == rateDetail.pack_type_id && y.Pack_Size == rateDetail.pack_size && y.Grade == rateDetail.grade
                        orderby x.Template_Name
                        select new ReturnRate
                        {
                            Selling_Price = y.Selling_price
                        }).FirstOrDefault();

            return rate;
        }

        public List<Template_SKU_List> getSKUS(int ratetemplateID)
        {
            var SKUList = (from z in DB.Rate_Template_Line_item
                           where z.RT_id == ratetemplateID
                           orderby z.SKU_Name
                           select new Template_SKU_List
                           {
                               Material_Code = z.Material_Code,
                               SKUId = z.SKU_Id,
                               SKUName = z.SKU_Name,
                               SKU_Code = z.SKU_Code,
                               SKU_SubType = z.SKU_SubType,
                               SKU_SubType_Id = z.SKU_SubType_Id,
                               HSN_Code=z.HSN_Code,
                               CGST=z.CGST,
                              SGST=z.SGST,
                              Total_GST=z.Total_GST,
                               Pack_Weight_Type_Id = z.Pack_Weight_Type_Id,
                               Pack_Weight_Type = z.Pack_Weight_Type,
                               Pack_Type_Id = z.Pack_Type_Id,
                               Pack_Size = z.Pack_Size,
                               Pack_Type = z.Pack_Type,
                               UOM = z.UOM,
                               Grade = z.Grade,
                               Price = z.Selling_price
                           }).ToList();

            return SKUList;
        }

        public List<Template_Fields_SKU> GetrateTemplates(int CustomerID, string region, string location, string dccode, string skutype)//,string region,string location,string dccode
        {
            List<Template_Fields_SKU> returnList = new List<Template_Fields_SKU>();

            var temSkuList = (from x in DB.Rate_Template
                              join y in DB.Customer_Rate_Template_Mapping on x.Template_ID equals y.Template_ID
                              where y.Customer_Id == CustomerID && x.SKU_Type == skutype && x.Is_Deleted == false
                              orderby x.Template_Name
                              select new Template_Fields_SKU
                              {
                                  Template_ID = x.Template_ID,
                                  Template_Name = x.Template_Name,
                                  Valitity_upto = x.Valitity_upto,
                                  DC_Id = x.DC_Id,
                                  DC_Code = x.DC_Code,
                                  Location_Code = x.Location_Code,
                                  Location_Id = x.Location_Id,
                                  Region = x.Region,
                                  Region_Id = x.Region_Id,
                                  Template_Code = x.Template_Code,
                                  //Region_Code = x.Region_Code,
                                  //Customer_Category_Code = x.Customer_Category_Code,
                                  //SKUList = (from z in DB.Rate_Template_Line_item
                                  //           where z.RT_id == x.Template_ID
                                  //           select new Template_SKU_List
                                  //           {
                                  //               SKUId = z.SKU_Id,
                                  //               SKUName = z.SKU_Name
                                  //           }).ToList()
                              }).ToList();

            if (location != null && dccode == "null")
            {
                var list = temSkuList.Where(x => x.Region == region && x.Location_Code == location).ToList();

                foreach (var li in list)
                {
                    returnList.Add(li);
                }
            }
            else if (location == "null" && dccode != null)
            {
                var list = temSkuList.Where(x => x.Region == region && x.DC_Code == dccode).ToList();

                foreach (var li in list)
                {
                    returnList.Add(li);
                }
            }

            return returnList;
        }

        public List<SKUPrice> getPrice(string SKU_Type, string Location, string Grade, string SKU_Name, int tempID)
        {

            var list = (from x in DB.Rate_Template
                        join y in DB.Rate_Template_Line_item on x.Template_ID equals y.RT_id
                        where x.SKU_Type == SKU_Type && y.SKU_Name == SKU_Name && y.Grade == Grade && x.Template_ID == tempID
                        select new SKUPrice
                        {
                            SKUId = y.SKU_Id,
                            SKUName = y.SKU_Name,
                            price = y.Selling_price
                        }).ToList();

            return list;
        }

        public List<RateIndentEntity> searchTemplate(int? roleId, int region_id, string location, string dccode, string Url)
        {
            List<RateIndentEntity> rateINdent = new List<RateIndentEntity>();
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
            //      var menuAccess = DB.Role_Menu_Access
            //.Join
            //(
            //  DB.Menu_Master,
            //    c => c.Menu_Id,
            //    d => d.Menu_Id,
            //    (c, d) => new { c, d }
            // )
            //.Where(e => e.c.Role_Id == roleId).Where(g => g.d.Menu_Id == g.c.Menu_Id && g.d.Url == Url).GroupBy(e => new { e.d.Menu_Id })
            //.Select(x => new FetchMenuDetails
            //{
            //    MenuID = x.Key.Menu_Id,
            //    MenuName = x.Select(c => c.d.Menu_Name).Distinct(),
            //    MenuPrevilages = x.Select(c => c.d.Menu_Previlleges).Distinct(),
            //    RolePrevilages = x.Select(c => c.c.Menu_Previlleges).Distinct(),
            //    ControllerName = x.Select(c => c.d.Url).Distinct(),
            //    ParentID = x.Select(c => c.d.Parent_id.Value).Distinct(),
            //}).FirstOrDefault();

            var list = (from x in DB.Rate_Template //&& x.DC_Location == location 
                        where x.Is_Deleted == false
                        orderby x.Template_Name
                        select
                            //x).AsEnumerable().Select(x => 
                        new RateIndentEntity
                        {
                            Template_ID = x.Template_ID,
                            Template_Name = x.Template_Name,

                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Template_Code = x.Template_Code,
                            //DC_Location = x.DC_Location,
                            SKU_Type_Id = x.SKU_Type_Id,
                            Region_Code = x.Region_Code,
                            Customer_Category_Code = x.Customer_Category_Code,
                            Category_Id = x.Category_Id,
                            SKU_Type_Code = x.SKU_Type_Code,
                            SKU_Type = x.SKU_Type,
                            Customer_Category = x.Customer_Category,
                            Valitity_upto = x.Valitity_upto,
                            CreatedDate = x.CreatedDate,
                            UpdateDate = x.UpdateDate,
                            CreateBy = x.CreateBy,
                            UpdateBy = x.UpdateBy,
                            is_Create = iCrt,
                            is_Delete = isDel,
                            is_Edit = isEdt,
                            is_Approval = isApp,
                            is_View = isViw,
                            //Menu_Id = menuAccess.MenuID,
                            //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                            //is_Create = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Add"]),
                            //is_Delete = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Delete"]),
                            //is_Edit = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Edit"]),
                            //is_Approval = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Approval"]),
                            //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"]),
                            counting = (from a in DB.Rate_Template_Line_item
                                        where a.RT_id == x.Template_ID
                                        select new
                                        {
                                            RTIId = a.RT_Line_Id
                                        }).Count(),
                            LineItems = (from a in DB.Rate_Template_Line_item
                                         where a.RT_id == x.Template_ID
                                         orderby a.SKU_Name
                                         select new RateTemplateLineitem
                                         {
                                             RT_Line_Id = a.RT_Line_Id,
                                             RT_id = a.RT_id,
                                             SKU_Id = a.SKU_Id,
                                             SKU_Code = a.SKU_Code,
                                             SKU_Name = a.SKU_Name,
                                             SKU_SubType_Id = a.SKU_SubType_Id,
                                             SKU_SubType = a.SKU_SubType,
                                             Pack_Type_Id = a.Pack_Type_Id,
                                             Pack_Type = a.Pack_Type,
                                             Pack_Size = a.Pack_Size,
                                             HSN_Code = a.HSN_Code,
                                             CGST = a.CGST,
                                             SGST = a.SGST,
                                             Total_GST = a.Total_GST,
                                             Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
                                             Pack_Weight_Type = a.Pack_Weight_Type,
                                             UOM = a.UOM,
                                             Grade = a.Grade,
                                             Selling_price = a.Selling_price,
                                             MRP = a.MRP,
                                             //  Price = a.Price,
                                             CreatedDate = x.CreatedDate,
                                             UpdateDate = x.UpdateDate,
                                             CreateBy = x.CreateBy,
                                             UpdateBy = x.UpdateBy,

                                             Template_ID = x.Template_ID,
                                             Template_Name = x.Template_Name,
                                             DC_Id = x.DC_Id,
                                             DC_Code = x.DC_Code,
                                             Location_Id = x.Location_Id,
                                             Location_Code = x.Location_Code,
                                             Region_Id = x.Region_Id,
                                             Region = x.Region,
                                             Region_Code = x.Region_Code,
                                             SKU_Type_Id = x.SKU_Type_Id,
                                             SKU_Type = x.SKU_Type,
                                             SKU_Type_Code = x.SKU_Type_Code,
                                             Category_Id = x.Category_Id,

                                             Customer_Category = x.Customer_Category,
                                             Valitity_upto = x.Valitity_upto,
                                             Reason = x.Reason,
                                             Template_Code = x.Template_Code,
                                             Customer_Category_Code = x.Customer_Category_Code


                                         }).ToList(),

                        }).ToList();

            if (location != null && dccode == "null")
            {
                rateINdent = list.Where(x => x.Location_Code == location && x.Region_Id == region_id).ToList();
            }
            else if (location == "null" && dccode != null)
            {
                rateINdent = list.Where(x => x.DC_Code == dccode && x.Region_Id == region_id).ToList();
            }
            //foreach (var t in rateINdent)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return rateINdent;
        }

        public List<RateTempateResponse> searchRateTemplateforEdit(string ULocation = "null", string UDCCode = "null", string Region = "null")
        {
            List<RateTempateResponse> retuenList = new List<RateTempateResponse>();
            //string str = DateTime.UtcNow.ToString("yyyy-MM-dd");
            //DateTime currentDate = DateTime.Parse(str);

            var list = (from x in DB.Rate_Template
                        where x.Is_Deleted == false
                        orderby x.Template_Name
                        select new RateTempateResponse
                        {
                            Template_ID = x.Template_ID,
                            Template_Code = x.Template_Code,
                            Template_Name = x.Template_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Valitity_upto = x.Valitity_upto,
                            SKU_Type = x.SKU_Type
                        }).ToList();


            List<RateTempateResponse> retuenList1 = new List<RateTempateResponse>();
            if (ULocation != null && UDCCode == "null")
            {
                retuenList1 = list.Where(x => x.Location_Code == ULocation && x.Region == Region).ToList();
            }
            else if (ULocation == "null" && UDCCode != null)
            {
                retuenList1 = list.Where(x => x.DC_Code == UDCCode && x.Region == Region).ToList();
            }

            foreach (var slist in retuenList1)
            {
                retuenList.Add(slist);
            }
            return retuenList;
        }

        public List<RateTempateResponse> searchRateTemplate(string ULocation = "null", string UDCCode = "null", string Region = "null")
        {
            List<RateTempateResponse> retuenList = new List<RateTempateResponse>();
            //string str = DateTime.UtcNow.ToString("yyyy-MM-dd");
            //DateTime currentDate = DateTime.Parse(str);

            var list = (from x in DB.Rate_Template
                        where x.Is_Deleted == false
                        orderby x.Template_Name
                        select new RateTempateResponse
                        {
                            Template_ID = x.Template_ID,
                            Template_Code = x.Template_Code,
                            Template_Name = x.Template_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Valitity_upto = x.Valitity_upto,
                            SKU_Type = x.SKU_Type
                        }).ToList();
            //
            //if(SkuType != null)
            //{
            //x.Valitity_upto >= currentDate &&    x.Valitity_upto >= currentDate && 
            List<RateTempateResponse> retuenList1 = new List<RateTempateResponse>();
            if (ULocation != null && UDCCode == "null")
            {
                retuenList1 = list.Where(x => x.Location_Code == ULocation && x.Region == Region).ToList();
            }
            else if (ULocation == "null" && UDCCode != null)
            {
                retuenList1 = list.Where(x => x.DC_Code == UDCCode && x.Region == Region).ToList();
            }
            foreach (var slist in retuenList1)
            {
                retuenList.Add(slist);
            }
            //}
            //else if(SkuType == null)
            //{
            //    List<RateTempateResponse> retuenList1 = new List<RateTempateResponse>();
            //    if (ULocation != null && UDCCode == null)
            //    {
            //        retuenList1 = list.Where(x => x.Valitity_upto >= currentDate && x.Location_Code == ULocation).ToList();
            //    }
            //    else if (ULocation == null && UDCCode != null)
            //    {
            //        retuenList1 = list.Where(x => x.Valitity_upto >= currentDate && x.DC_Code == UDCCode).ToList();
            //    }

            //    foreach (var slist in retuenList1)
            //    {
            //        retuenList.Add(slist);
            //    }
            //}

            return retuenList;
        }

        //   ------------------------------------GET----------------------------------------
        public List<RateIndentEntity> GetRateIndent(string id)
        {
            var list = (from x in DB.Rate_Template
                        where x.Template_Code == id && x.Is_Deleted == false
                        orderby x.Template_Name
                        select new RateIndentEntity
                        {
                            Template_ID = x.Template_ID,
                            Template_Name = x.Template_Name,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            Region_Id = x.Region_Id,
                            Region = x.Region,
                            Template_Code = x.Template_Code,
                            //DC_Location = x.DC_Location,
                            SKU_Type_Id = x.SKU_Type_Id,
                            Category_Id = x.Category_Id,
                            SKU_Type_Code = x.SKU_Type_Code,
                            Region_Code = x.Region_Code,
                            Customer_Category_Code = x.Customer_Category_Code,
                            SKU_Type = x.SKU_Type,
                            Customer_Category = x.Customer_Category,
                            Valitity_upto = x.Valitity_upto,
                            CreatedDate = x.CreatedDate,
                            UpdateDate = x.UpdateDate,
                            CreateBy = x.CreateBy,
                            UpdateBy = x.UpdateBy,
                            //
                            LineItems = (from a in DB.Rate_Template_Line_item
                                         where a.RT_id == x.Template_ID
                                         orderby a.SKU_Name
                                         select new RateTemplateLineitem
                                         {
                                             RT_Line_Id = a.RT_Line_Id,
                                             RT_id = a.RT_id,
                                             Rate_Template_Code = a.Rate_Template_Code,
                                             Material_Code = a.Material_Code,
                                             Material_Auto_Num_Code = a.Material_Auto_Num_Code,
                                             SKU_Id = a.SKU_Id,
                                             SKU_Code = a.SKU_Code,
                                             SKU_Name = a.SKU_Name,
                                             SKU_SubType_Id = a.SKU_SubType_Id,
                                             SKU_SubType = a.SKU_SubType,
                                             Pack_Type_Id = a.Pack_Type_Id,
                                             Pack_Type = a.Pack_Type,
                                             Pack_Size = a.Pack_Size,
                                             Pack_Weight_Type_Id = a.Pack_Weight_Type_Id,
                                             Pack_Weight_Type = a.Pack_Weight_Type,
                                             UOM = a.UOM,
                                             Grade = a.Grade,
                                             Selling_price = a.Selling_price,
                                             MRP = a.MRP,
                                             //  Price = a.Price,
                                             CreatedDate = a.CreatedDate,
                                             UpdateDate = a.UpdateDate,
                                             CreateBy = a.CreateBy,
                                             UpdateBy = a.UpdateBy,
                                         }).ToList(),

                        }).ToList();

            return list;
        }

        //------------------------DELETEPURCHASEORDER----------

        public bool DeleteRateIndent(string tId, string deleteReason)
        {
            var success = false;
            if (tId != null)
            {
                using (var scope = new TransactionScope())
                {
                    var p = (from f in DB.Rate_Template
                             where f.Template_Code == tId
                             select f).FirstOrDefault();

                    if (p != null)
                    {
                        p.Is_JDM_Synced = false;
                        p.Is_BNG_Synced = false;
                        p.Is_Deleted = true;
                        p.Reason = deleteReason;
                        _unitOfWork.RateTemplateRepository.Update(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        
        public RIExcelImport ExcelImportForri(rifileImport fileDetail)
        {
            RIExcelImport roExcel = new RIExcelImport();
            string Profilepicname = "RI_" + Guid.NewGuid().ToString();
            string sPath = "";
            string vPath = "";
            string name = "";
            string dPath = "~/Areas/RI";
            string dirCreatePath = "";
            try
            {
                string RootPath = System.Web.Hosting.HostingEnvironment.MapPath(dPath);

                string ext = ".xlsx";
                dirCreatePath = RootPath;

                if (!Directory.Exists(dirCreatePath))
                {
                    Directory.CreateDirectory(RootPath);
                }
                sPath = RootPath;
                name = Profilepicname + ext;
                vPath = sPath + "\\" + name;

                if (File.Exists(vPath))
                {
                    File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
                }
                else
                {
                    File.WriteAllBytes(vPath, Convert.FromBase64String(fileDetail.FileString));
                }

                FileStream stream = File.Open(vPath, FileMode.Open, FileAccess.Read);
                stream.Position = 0;
                IExcelDataReader excelReader = null;

                if (ext == ".xls")
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (ext == ".xlsx")
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                int columnCount = result1.Tables[0].Columns.Count;

                if (columnCount > 8)
                {
                    roExcel.status = false;
                    roExcel.Message = "Extra Column's Present in Given Excel";
                    return roExcel;
                }

                //--------------------validating Excel Columns
                int lineCount = 1;

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        //  if(d.ItemArray[]!=DBNull.Value)

                        Rate_Template_Line_item ri = new Rate_Template_Line_item();
                        object[] A = { };

                        //string yy= d["Dispatch_Qty"].ToString();

                        ri.RT_id = fileDetail.rateindentID;
                        ri.SKU_Name = d["SKUName"] != null && d["SKUName"].ToString() != "" ? d["SKUName"].ToString() : "";
                        ri.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        ri.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        ri.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        ri.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        ri.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ri.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        //ri.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != "" ? d["Pack_Weight_Type"].ToString() : "";
                        //ri.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? double.Parse(d["Dispatch_Qty"].ToString()) : 0;
                        ri.Selling_price = d["Price"] != null && d["Price"].ToString() != "" ? double.Parse(d["Price"].ToString()) : 0;
                        using (var iscope = new TransactionScope())
                        {
                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == ri.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            if (subtypedetail != null)
                            {
                                ri.SKU_SubType = subtypedetail.SKU_SubType_Name;
                                ri.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "SKU_SubType_Name";
                                return roExcel;
                            }

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == ri.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            if (skuDetail != null)
                            {
                                ri.SKU_Name = skuDetail.SKU_Name;
                                ri.SKU_Id = skuDetail.SKU_Id;
                                ri.SKU_Code = skuDetail.SKU_Code;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "SKU_Name";
                                return roExcel;
                            }

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ri.UOM.ToLower().Trim()).FirstOrDefault();
                            if (uomDetail != null)
                            {
                                ri.UOM = uomDetail.Unit_Name;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "UOM";
                                return roExcel;
                            }

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ri.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            if (packtypedetail != null)
                            {
                                ri.Pack_Type = packtypedetail.Pack_Type_Name;
                                ri.Pack_Type_Id = packtypedetail.Pack_Type_Id;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "Pack_Type_Name";
                                return roExcel;
                            }

                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ri.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            if (packweightType != null)
                            {
                                ri.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                                ri.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "Pack_Weight_Type";
                                return roExcel;
                            }

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ri.Grade.ToLower().Trim()).FirstOrDefault();
                            if (gradeDetail != null)
                            {
                                ri.Grade = gradeDetail.GradeType_Name;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "Grade";
                                return roExcel;
                            }

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ri.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            if (packsizedetail != null)
                            {
                                ri.Pack_Size = packsizedetail.Pack_Size_Value;
                            }
                            else
                            {
                                roExcel.status = false;
                                roExcel.lineNumber = lineCount;
                                roExcel.Message = "Error";
                                roExcel.errorItem = "Pack_Size_Value";
                                return roExcel;
                            }

                            //var price = (from x in DB.Rate_Template_Line_item
                            //             join y in DB.Rate_Template on x.RT_id equals y.Template_ID
                            //             join z in DB.Customer_Indent on y.Template_ID equals z.Price_Template_ID
                            //             where z.Indent_ID == fileDetail.indentID && x.SKU_Id == ri.SKU_Id && x.Grade == ri.Grade
                            //             select new
                            //             {
                            //                 x.Selling_price
                            //             }).FirstOrDefault();

                            //if (price != null)
                            //{
                            //    ri.Price = double.Parse(price.Selling_price.ToString());
                            //}
                            //else
                            //{
                            //    roExcel.status = false;
                            //    roExcel.Message = "Price is not available for this LineItem '" + lineCount + "' in Rate Template";
                            //    return roExcel;
                            //}


                            iscope.Complete();
                        }

                        lineCount += 1;

                    }
                //-----------------------------------Delete All Columns
                using (var iscope1 = new TransactionScope())
                {
                    var del_list = DB.Rate_Template_Line_item.Where(x => x.RT_id == fileDetail.rateindentID).ToList();

                    foreach (var li in del_list)
                    {
                        _unitOfWork.RateTemplateLineItemRepository.Delete(li.RT_Line_Id);
                        _unitOfWork.Save();
                    }
                    iscope1.Complete();
                }
                //------------------------------------Insert New COlumns

                if (result1 != null)
                    foreach (DataRow d in result1.Tables[0].Rows)
                    {
                        Rate_Template_Line_item ri = new Rate_Template_Line_item();

                        ri.RT_id = fileDetail.rateindentID;
                        ri.SKU_Name = d["SKUName"] != null && d["SKUName"].ToString() != "" ? d["SKUName"].ToString() : "";
                        ri.SKU_SubType = d["SKU_SubType"] != null && d["SKU_SubType"].ToString() != "" ? d["SKU_SubType"].ToString() : "";
                        ri.Pack_Type = d["Pack_Type"] != null && d["Pack_Type"].ToString() != "" ? d["Pack_Type"].ToString() : "";
                        ri.Pack_Size = d["Pack_Size"] != null && d["Pack_Size"].ToString() != "" ? d["Pack_Size"].ToString() : "";
                        ri.Pack_Weight_Type = d["Pack_Weight_Type"] != null && d["Pack_Weight_Type"].ToString() != null ? d["Pack_Weight_Type"].ToString() : "";
                        ri.UOM = d["UOM"] != null && d["UOM"].ToString() != "" ? d["UOM"].ToString() : "";
                        ri.Grade = d["Grade"] != null && d["Grade"].ToString() != "" ? d["Grade"].ToString() : "";
                        //ri.Dispatch_Qty = d["Dispatch_Qty"] != null && d["Dispatch_Qty"].ToString() != "" ? double.Parse(d["Dispatch_Qty"].ToString()) : 0;
                        ri.Selling_price = d["Price"] != null && d["Price"].ToString() != "" ? double.Parse(d["Price"].ToString()) : 0;

                        using (var iscope = new TransactionScope())
                        {

                            var skuDetail = DB.SKU_Master.Where(x => x.SKU_Name.ToLower().Trim() == ri.SKU_Name.ToLower().Trim()).FirstOrDefault();
                            ri.SKU_Name = skuDetail.SKU_Name;
                            ri.SKU_Id = skuDetail.SKU_Id;

                            var subtypedetail = ListHelper.SKU_SubType().Where(x => x.SKU_SubType_Name.ToLower().Trim() == ri.SKU_SubType.ToLower().Trim()).FirstOrDefault();
                            ri.SKU_SubType = subtypedetail.SKU_SubType_Name;
                            ri.SKU_SubType_Id = subtypedetail.SKU_SubType_Id;

                            var packtypedetail = ListHelper.Pack_Type().Where(x => x.Pack_Type_Name.ToLower().Trim() == ri.Pack_Type.ToLower().Trim()).FirstOrDefault();
                            ri.Pack_Type = packtypedetail.Pack_Type_Name;
                            ri.Pack_Type_Id = packtypedetail.Pack_Type_Id;


                            var packweightType = ListHelper.Pack_Weight_Type().Where(x => x.Pack_Weight_Type_Name.ToLower().Trim() == ri.Pack_Weight_Type.ToLower().Trim()).FirstOrDefault();
                            ri.Pack_Weight_Type = packweightType.Pack_Weight_Type_Name;
                            ri.Pack_Weight_Type_Id = packweightType.Pack_Weight_Type_Id;

                            var gradeDetail = ListHelper.GradeType().Where(x => x.GradeType_Name.ToLower().Trim() == ri.Grade.ToLower().Trim()).FirstOrDefault();
                            ri.Grade = gradeDetail.GradeType_Name;

                            var uomDetail = DB.Units.Where(x => x.Unit_Name.ToLower().Trim() == ri.UOM.ToLower().Trim()).FirstOrDefault();
                            ri.UOM = uomDetail.Unit_Name;

                            var packsizedetail = ListHelper.Pack_Size().Where(x => x.Pack_Size_Value.ToLower().Trim() == ri.Pack_Size.ToLower().Trim()).FirstOrDefault();
                            ri.Pack_Size = packsizedetail.Pack_Size_Value;

                            //var price = (from x in DB.Rate_Template_Line_item
                            //             join y in DB.Rate_Template on x.RT_id equals y.Template_ID
                            //             join z in DB.Customer_Indent on y.Template_ID equals z.Price_Template_ID
                            //             where z.Indent_ID == fileDetail.indentID && x.SKU_Id == ri.SKU_Id && x.Grade == ri.Grade
                            //             select new
                            //             {
                            //                 x.Selling_price
                            //             }).FirstOrDefault();

                            //ri.Price = double.Parse(price.Selling_price.ToString());

                            var createdBy = (from x in DB.Rate_Template
                                             where x.Template_ID == ri.RT_id
                                             select new
                                             {
                                                 x.CreateBy,
                                                 x.CreatedDate,
                                                 x.UpdateBy,
                                                 x.UpdateDate
                                             }).FirstOrDefault();

                            ri.CreateBy = createdBy.CreateBy;
                            ri.CreatedDate = createdBy.CreatedDate;
                            ri.UpdateBy = createdBy.UpdateBy;
                            ri.UpdateDate = createdBy.UpdateDate;

                            using (var iscope1 = new TransactionScope())
                            {
                                _unitOfWork.RateTemplateLineItemRepository.Insert(ri);
                                _unitOfWork.Save();

                                iscope1.Complete();
                            }
                            iscope.Complete();
                        }

                        lineCount += 1;

                    }

                excelReader.Close();
            }
            catch (Exception e)
            {
                roExcel.status = false;
                roExcel.Message = e.Message.ToString();
                return roExcel;
            }
            roExcel.status = true;
            roExcel.Message = "Success";

            return roExcel;
        }


        public bool DeleteRateIndentLineItem(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var p = _unitOfWork.RateTemplateLineItemRepository.GetByID(Id);
                    if (p != null)
                    {

                        _unitOfWork.RateTemplateLineItemRepository.Delete(p);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
