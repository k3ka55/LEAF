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
    public class TallyMappingServices : ITallyMapping
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public TallyMappingServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public TallyMappingEntity GetTallyMappingById(int skuId)
        {
            //var TALLY_MAPPING = _unitOfWork.TallyMappingRepository.GetByID(skuId);
            var ty = (from y in DB.TALLY_MAPPING
                      where y.Is_Delete == false && y.ID == skuId
                      select new TallyMappingEntity
                      {

                          ID = y.ID,
                          LEDGER_NAME = y.LEDGER_NAME,
                          LEDGER_CODE = y.LEDGER_CODE,
                          GROUP_NAME = y.GROUP_NAME,
                          COST_CENTRE_NAME = y.COST_CENTRE_NAME,
                          VOUCHER_TYPE = y.VOUCHER_TYPE,
                          Activity = y.Activity,
                          Module_Name = y.Module_Name,
                          CreatedDate = y.CreatedDate,
                          UpdatedDate = y.UpdatedDate,
                          CreatedBy = y.CreatedBy,
                          UpdatedBy = y.UpdatedBy,
                      }).FirstOrDefault();
            //if (TALLY_MAPPING != null)
            //{
            //    Mapper.CreateMap<TALLY_MAPPING, TallyMappingEntity>();
            //    var skuModel = Mapper.Map<TALLY_MAPPING, TallyMappingEntity>(TALLY_MAPPING);
            //    return skuModel;
            //}
            return ty;
        }

        //public List<TallyMappingEntity> GetSKUByCategory(string skuCat)
        //{
        //    List<TallyMappingEntity> list = new List<TallyMappingEntity>();

        //    list = (from x in DB.TALLY_MAPPING
        //            where x. == skuCat
        //            select new TallyMappingEntity
        //            {
        //                SKU_Name = x.SKU_Name
        //            }).ToList();
        //    return list;
        //}

        public IEnumerable<BusinessEntities.TallyMappingEntity> GetAllTallyMapping(int? roleId, string Url)
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

            var TALLY_MAPPING = (from y in DB.TALLY_MAPPING
                                 where  y.Is_Delete==false
                                 orderby y.Activity 
                                 select
                                     //y).AsEnumerable().Select(y => 
                                 new TallyMappingEntity
                                      {
                                          ID = y.ID,
                                          LEDGER_NAME = y.LEDGER_NAME,
                                          LEDGER_CODE = y.LEDGER_CODE,
                                          GROUP_NAME = y.GROUP_NAME,
                                          COST_CENTRE_NAME = y.COST_CENTRE_NAME,
                                          VOUCHER_TYPE = y.VOUCHER_TYPE,
                                          Activity = y.Activity,
                                          Module_Name = y.Module_Name,
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
            //foreach (var t in TALLY_MAPPING)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}
            return TALLY_MAPPING;
        }

        public bool CreateTallyMapping(BusinessEntities.TallyMappingEntity TallyMappingEntity)
        {
            using (var scope = new TransactionScope())
            {
                var tally = new TALLY_MAPPING
                {
                    LEDGER_CODE = TallyMappingEntity.LEDGER_CODE,
                    LEDGER_NAME = TallyMappingEntity.LEDGER_NAME,
                    Activity = TallyMappingEntity.Activity,
                    COST_CENTRE_NAME = TallyMappingEntity.COST_CENTRE_NAME,
                    GROUP_NAME = TallyMappingEntity.GROUP_NAME,
                    Module_Name = TallyMappingEntity.Module_Name,
                    VOUCHER_TYPE = TallyMappingEntity.VOUCHER_TYPE,
                    Is_Delete=false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = TallyMappingEntity.CreatedBy
                };
                _unitOfWork.TallyMappingRepository.Insert(tally);
                _unitOfWork.Save();

                scope.Complete();
                return true;
            }

        }

        public bool UpdateTallyMapping(int skuId, BusinessEntities.TallyMappingEntity TallyMappingEntity)
        {
            var success = false;
            if (TallyMappingEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var TALLY_MAPPING = _unitOfWork.TallyMappingRepository.GetByID(skuId);
                    if (TALLY_MAPPING != null)
                    {
                        TALLY_MAPPING.LEDGER_CODE = TallyMappingEntity.LEDGER_CODE;
                        TALLY_MAPPING.LEDGER_NAME = TallyMappingEntity.LEDGER_NAME;
                        TALLY_MAPPING.Activity = TallyMappingEntity.Activity;
                        TALLY_MAPPING.COST_CENTRE_NAME = TallyMappingEntity.COST_CENTRE_NAME;
                        TALLY_MAPPING.GROUP_NAME = TallyMappingEntity.GROUP_NAME;
                        TALLY_MAPPING.Module_Name = TallyMappingEntity.Module_Name;
                        TALLY_MAPPING.VOUCHER_TYPE = TallyMappingEntity.VOUCHER_TYPE;
                        TALLY_MAPPING.UpdatedDate = DateTime.Now;
                        TALLY_MAPPING.UpdatedBy = TallyMappingEntity.UpdatedBy;
                        try
                        {
                            _unitOfWork.TallyMappingRepository.Update(TALLY_MAPPING);
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

        public bool DeleteTallyMapping(int skuId)
        {
            var success = false;
            if (skuId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var TALLY_MAPPING = _unitOfWork.TallyMappingRepository.GetByID(skuId);
                    if (TALLY_MAPPING != null)
                    {
                        try
                        {
                            TALLY_MAPPING.Is_Delete = true;
                            _unitOfWork.TallyMappingRepository.Update(TALLY_MAPPING);
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