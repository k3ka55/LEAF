using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace BusinessServices
{
    public class SalesRouteMappingService : ISalesRouteMappingService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public SalesRouteMappingService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool UpdateSaleRouteMapping(int? Sales_Person_Id, SalesRoutemappingEntity routeEntity)
        {
            var success = false;
            if (routeEntity != null)
            {
                var CLT = from ord in DB.Sales_Route_Mapping
                          where ord.Sales_Person_Id == Sales_Person_Id
                          select ord;

                try
                {
                    foreach (var ord in CLT)
                    {
                        DB.Sales_Route_Mapping.Remove(ord);
                    }
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                using (var scope = new TransactionScope())
                {
                    var model = new Sales_Route_Mapping();
                    foreach (var fSub in routeEntity.SalesRoutes)
                    {
                        model.Sales_Person_Id = Sales_Person_Id;
                        model.Sales_Person_Name = routeEntity.Sales_Person_Name;
                        model.Route_Id = fSub.Route_Id;
                        model.Orgin_Location_Id = fSub.Orgin_Location_Id;
                        model.Orgin_Location_Code = fSub.Orgin_Location_Code;
                        model.Orgin_Location_Name = fSub.Orgin_Location_Name;
                        model.Target_Location_Id = fSub.Target_Location_Id;
                        model.Target_Location_Code = fSub.Target_Location_Code;
                        model.Target_Location_Name = fSub.Target_Location_Name;
                        model.Route_Alias_Name = fSub.Route_Alias_Name;

                        //model.Route_Alias_Name = fSub.Route_Name;
                        model.Route_Code = fSub.Route_Code;
                        model.CreatedBy = routeEntity.UpdatedBy;
                        model.CreatedDate = DateTime.Now;
                        model.UpdatedBy = routeEntity.UpdatedBy;
                        model.UpdatedDate = DateTime.Now;
                        _unitOfWork.SalesRouteMappingRepository.Insert(model);
                        _unitOfWork.Save();
                    }
                    scope.Complete();
                }
                success = true;
            }

            return success;
        }

        public dynamic GetAllSaleRouteMapping(int? roleId, string Url)
        {
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
            var V = (from f in DB.Sales_Route_Mapping
                     group f by new
                     {
                         f.Sales_Person_Id,
                         f.Sales_Person_Name,
                     } into temp

                     select new
                     {
                         Sales_Person_Id = temp.Key.Sales_Person_Id,
                         Sales_Person_Name = temp.Key.Sales_Person_Name,
                         SalesRoutes = (from a in DB.Sales_Route_Mapping
                                        where a.Sales_Person_Id == temp.Key.Sales_Person_Id
                                        orderby a.Route_Alias_Name
                                        select new
                                        {
                                            Orgin_Location_Id = a.Orgin_Location_Id,
                                            Orgin_Location_Code = a.Orgin_Location_Code,
                                            Orgin_Location_Name = a.Orgin_Location_Name,
                                            Target_Location_Id = a.Target_Location_Id,
                                            Target_Location_Code = a.Target_Location_Code,
                                            Target_Location_Name = a.Target_Location_Name,
                                            Route_Alias_Name = a.Route_Alias_Name,
                                            Route_Id = a.Route_Id,
                                            //  Route_Name = a.Route_Alias_Name,
                                            Route_Code = a.Route_Code
                                        }).ToList(),
                         is_Create = iCrt,
                         is_Delete = isDel,
                         is_Edit = isEdt,
                         is_Approval = isApp,
                         is_View = isViw,
                         // LineItem = temp
                     }).ToList();
            return V;
        }
        public SalesRoutemappingEntity Get(int Sales_Person_Id)
        {

            //var ty=(from aa in DB.Sales_Route_Mapping
            //                               where aa.Sales_Person_Id == Sales_Person_Id
            //                          orderby aa.Route_Name
            //                          select new SalesRoutesList
            //                           {
            //                               Route_Id = aa.Route_Id,
            //                               Route_Name = aa.Route_Name,
            //                               Route_Code = aa.Route_Code
            //                           }).ToList(),
            var qry = (from a in DB.Sales_Route_Mapping
                       where a.Sales_Person_Id == Sales_Person_Id
                       orderby a.Route_Alias_Name
                       select new SalesRoutemappingEntity
                        {
                            Sales_Route_Mapping_Id = a.Sales_Route_Mapping_Id,
                            Sales_Person_Id = a.Sales_Person_Id,
                            Sales_Person_Name = a.Sales_Person_Name,
                            Orgin_Location_Id = a.Orgin_Location_Id,
                            Orgin_Location_Code = a.Orgin_Location_Code,
                            Orgin_Location_Name = a.Orgin_Location_Name,
                            Target_Location_Id = a.Target_Location_Id,
                            Target_Location_Code = a.Target_Location_Code,
                            Target_Location_Name = a.Target_Location_Name,
                            Route_Alias_Name = a.Route_Alias_Name,

                            CreatedBy = a.CreatedBy,
                            CreatedDate = a.CreatedDate,
                            UpdatedDate = a.UpdatedDate,
                            UpdatedBy = a.UpdatedBy,
                            SalesRoutes = (from aa in DB.Sales_Route_Mapping
                                           where aa.Sales_Person_Id == Sales_Person_Id
                                           orderby aa.Route_Alias_Name
                                           select new SalesRoutesList
                                            {
                                                Orgin_Location_Id = aa.Orgin_Location_Id,
                                                Orgin_Location_Code = aa.Orgin_Location_Code,
                                                Orgin_Location_Name = aa.Orgin_Location_Name,
                                                Target_Location_Id = aa.Target_Location_Id,
                                                Target_Location_Code = aa.Target_Location_Code,
                                                Target_Location_Name = aa.Target_Location_Name,
                                                Route_Id = aa.Route_Id,
                                                Route_Alias_Name = aa.Route_Alias_Name,
                                                Route_Code = aa.Route_Code
                                            }).ToList(),
                        }).FirstOrDefault();

            return qry;


        }
        public dynamic GetSalesPersons(string Route_Code)
        {
            var qry = (from a in DB.Sales_Route_Mapping
                       where a.Route_Code == Route_Code
                       orderby a.Route_Alias_Name
                       select new
                       {
                           SalesPersons = (from aa in DB.Sales_Route_Mapping
                                           where aa.Route_Alias_Name == a.Route_Alias_Name
                                           orderby aa.Route_Alias_Name
                                           select new
                                           {
                                               Orgin_Location_Id = a.Orgin_Location_Id,
                                               Orgin_Location_Code = a.Orgin_Location_Code,
                                               Orgin_Location_Name = a.Orgin_Location_Name,
                                               Target_Location_Id = a.Target_Location_Id,
                                               Target_Location_Code = a.Target_Location_Code,
                                               Target_Location_Name = a.Target_Location_Name,
                                               Route_Alias_Name = a.Route_Alias_Name,
                                               Route_Code = a.Route_Code,
                                               Sales_Route_Mapping_Id = a.Sales_Route_Mapping_Id,
                                               Sales_Person_Id = a.Sales_Person_Id,
                                               Sales_Person_Name = a.Sales_Person_Name
                                           }).ToList(),
                       }).FirstOrDefault();

            return qry;


        }

        public bool InsertSaleRouteMapping(SalesRoutemappingEntity RoutMast)
        {
            using (var scope = new TransactionScope())
            {
                var model = new Sales_Route_Mapping();
                foreach (var fSub in RoutMast.SalesRoutes)
                {
                    model.Orgin_Location_Id = fSub.Orgin_Location_Id;
                    model.Orgin_Location_Code = fSub.Orgin_Location_Code;
                    model.Orgin_Location_Name = fSub.Orgin_Location_Name;
                    model.Target_Location_Id = fSub.Target_Location_Id;
                    model.Target_Location_Code = fSub.Target_Location_Code;
                    model.Target_Location_Name = fSub.Target_Location_Name;
                    model.Route_Alias_Name = fSub.Route_Alias_Name;
                    model.Sales_Person_Id = RoutMast.Sales_Person_Id;
                    model.Sales_Person_Name = RoutMast.Sales_Person_Name;
                    model.Route_Id = fSub.Route_Id;
                    // model.Route_Alias_Name = fSub.Route_Alias_Name;
                    model.Route_Code = fSub.Route_Code;
                    model.CreatedBy = RoutMast.CreatedBy;
                    model.CreatedDate = DateTime.Now;
                    _unitOfWork.SalesRouteMappingRepository.Insert(model);
                    _unitOfWork.Save();
                }


                scope.Complete();
            }
            return true;
        }


        public bool DeleteSaleRouteMapping(int Sales_Person_Id)
        {
            var success = false;

            var CLT = from ord in DB.Sales_Route_Mapping
                      where ord.Sales_Person_Id == Sales_Person_Id
                      select ord;
            foreach (var ord in CLT)
            {
                DB.Sales_Route_Mapping.Remove(ord);
            }
            try
            {
                success = true;
                DB.SaveChanges();
            }
            catch (Exception)
            {
                success = false;
                return success;
            }

            return success;
        }
    }
}

