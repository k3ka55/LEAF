using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace BusinessServices
{
    public class SKUService : ISKUService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public SKUService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public BusinessEntities.SKUEntity GetskuById(int skuId)
        {
            var sku = _unitOfWork.SKURepository.GetByID(skuId);
            if (sku != null)
            {
                Mapper.CreateMap<SKU_Master, SKUEntity>();
                var skuModel = Mapper.Map<SKU_Master, SKUEntity>(sku);
                return skuModel;
            }
            return null;
        }

        public List<SKUEntity> GetSKUByCategory(string skuCat)
        {
            List<SKUEntity> list = new List<SKUEntity>();

            list = (from x in DB.SKU_Master
                    where x.SKU_Category == skuCat
                    select new SKUEntity
                    {
                        SKU_Name = x.SKU_Name
                    }).ToList();
            return list;
        }


        public IEnumerable<BusinessEntities.SKUEntity> GetAllSku(int? roleId, string Url)
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


            var sku = (from y in DB.SKU_Master
                       orderby y.SKU_Name
                       select
                           //y).AsEnumerable().Select(y => 
                       new SKUEntity
                            {
                                SKU_Id = y.SKU_Id,
                                SKU_Code = y.SKU_Code,
                                SKU_Name = y.SKU_Name,
                                SKU_Category = y.SKU_Category,
                                Receiving_JDP = y.Receiving_JDP,
                                UOM = y.UOM,
                                Chennai_Alias = y.Chennai_Alias,
                                Coimbatore_Alias = y.Coimbatore_Alias,
                                Ooty_Alias = y.Ooty_Alias,
                                HSN_Code = y.HSN_Code,
                                Bangalore_Alias = y.Bangalore_Alias,
                                CreatedDate = y.CreatedDate,
                                UpdatedDate = y.UpdatedDate,
                                CreatedBy = y.CreatedBy,
                                UpdatedBy = y.UpdatedBy,
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
                                //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"])
                            }).ToList();
           
            return sku;


        }

        public bool CreateSKU(BusinessEntities.SKUEntity skuEntity)
        {
            using (var scope = new TransactionScope())
            {
                var sku = new SKU_Master
                {
                    SKU_Code = skuEntity.SKU_Code,
                    SKU_Name = skuEntity.SKU_Name,
                    Receiving_JDP = skuEntity.Receiving_JDP,
                    UOM = skuEntity.UOM,
                    HSN_Code = skuEntity.HSN_Code,
                    CGST = 0,
                    SGST = 0,
                    Total_GST = 0,
                    Chennai_Alias = skuEntity.Chennai_Alias,
                    SKU_Category = skuEntity.SKU_Category,
                    Coimbatore_Alias = skuEntity.Coimbatore_Alias,
                    Ooty_Alias = skuEntity.Ooty_Alias,
                    Bangalore_Alias = skuEntity.Bangalore_Alias,
                    CreatedDate = DateTime.Now,
                    CreatedBy = skuEntity.CreatedBy
                };
                _unitOfWork.SKURepository.Insert(sku);
                _unitOfWork.Save();
                //
                //try
                //{
                //    _unitOfWork.SKURepository.Insert(sku);
                //    _unitOfWork.Save();

                //    int skuId = sku.SKU_Id;
                //    var model = new SKU_Main_Sub_Mapping();
                //    foreach (skuMappingEntity skuMapping in skuEntity.skuMapping)
                //    {
                //        model.SKU_Id = skuId;
                //        model.SKU_Main_Group_Id = skuMapping.SKU_Main_Group_Id;
                //        model.SKU_Sub_Group_Id = skuMapping.SKU_Sub_Group_Id;

                //        _unitOfWork.SkuMappingRepository.Insert(model);
                //        _unitOfWork.Save();
                //    }

                //    
                //}
                //catch (Exception)
                //{
                //    return false;
                //}
                scope.Complete();
                return true;
            }

        }

        public List<skuReturnMappingEntity> getskuBasemainsub(int skuMappingId)
        {
            var query = from dc in DB.SKU_Main_Sub_Mapping
                        join skumg in DB.SKU_Main_Group on dc.SKU_Main_Group_Id equals skumg.SKU_Main_Group_Id
                        join skusg in DB.SKU_Sub_Group on dc.SKU_Sub_Group_Id equals skusg.SKU_Sub_Group_Id
                        where dc.SKU_Sub_Group_Id == skuMappingId
                        select new skuReturnMappingEntity
                        {
                            SKU_Main_Group_Id = skumg.SKU_Main_Group_Id,
                            SKU_Sub_Group_Id = skusg.SKU_Sub_Group_Id,
                            SKU_Main_Description = skumg.SKU_Description,
                            SKU_Sub_Description = skusg.SKU_Description
                        };
            var result = query.ToList();
            return result;

        }


        public bool UpdateSKU(int skuId, BusinessEntities.SKUEntity skuEntity)
        {
            var success = false;
            if (skuEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var sku = _unitOfWork.SKURepository.GetByID(skuId);
                    if (sku != null)
                    {
                        sku.SKU_Id = skuEntity.SKU_Id;
                        sku.SKU_Code = skuEntity.SKU_Code;
                        sku.SKU_Name = skuEntity.SKU_Name;
                        sku.SKU_Category = skuEntity.SKU_Category;
                        sku.Receiving_JDP = skuEntity.Receiving_JDP;
                        sku.UOM = skuEntity.UOM;
                        sku.HSN_Code = skuEntity.HSN_Code;
                        sku.Chennai_Alias = skuEntity.Chennai_Alias;
                        sku.Coimbatore_Alias = skuEntity.Coimbatore_Alias;
                        sku.Ooty_Alias = skuEntity.Ooty_Alias;
                        sku.Bangalore_Alias = skuEntity.Bangalore_Alias;
                        sku.UpdatedDate = DateTime.Now;
                        sku.UpdatedBy = skuEntity.UpdatedBy;

                        try
                        {
                            _unitOfWork.SKURepository.Update(sku);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                        catch (Exception)
                        {
                            success = false;
                            return success;
                        }

                    }
                }
            }
            return success;
        }

        public bool DeleteSKU(int skuId)
        {
            var success = false;
            if (skuId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var sku = _unitOfWork.SKURepository.GetByID(skuId);
                    if (sku != null)
                    {
                        try
                        {
                            _unitOfWork.SKURepository.Delete(sku);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                        catch (Exception)
                        {
                            success = false;
                            return success;
                        }

                    }
                }
            }
            return success;
        }

    }
}