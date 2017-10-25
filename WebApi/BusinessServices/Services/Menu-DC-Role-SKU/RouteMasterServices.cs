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
    public class RouteMasterServices : IRouteMaster
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public RouteMasterServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool Update(string Route_Code, RouteMasterEntity routeEntity)
        {
            var success = false;
            if (routeEntity != null)
            {
                var CLT = from ord in DB.Route_Master
                          where ord.Route_Code == Route_Code
                          select ord;
                try
                {
                    foreach (var ord in CLT)
                    {
                        DB.Route_Master.Remove(ord);
                    }
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                using (var scope = new TransactionScope())
                {
                    var model = new Route_Master();
                    if (routeEntity.Routes.Count() != 0)
                    {
                        foreach (var fSub in routeEntity.Routes)
                        {
                            model.Orgin_Location_Id = routeEntity.Orgin_Location_Id;
                            model.Orgin_Location_Code = routeEntity.Orgin_Location_Code;
                            model.Orgin_Location = routeEntity.Orgin_Location;
                            //model.Target_Location_Id = routeEntity.Target_Location_Id;
                            //model.Target_Location_Code = routeEntity.Target_Location_Code;
                            //model.Target_Loc_Type = routeEntity.Target_Loc_Type;
                            model.Route_Code = Route_Code;
                            model.Remarks = routeEntity.Remarks;
                            model.Route_Id = fSub.Route_Id;
                            model.Target_Location = routeEntity.Target_Location;
                            model.CreatedBy = routeEntity.UpdatedBy;
                            model.CreatedDate = DateTime.Now;
                            model.UpdatedBy = routeEntity.UpdatedBy;
                            model.UpdatedDate = DateTime.Now;
                            model.Route = fSub.Route;
                            _unitOfWork.RouteRepository.Insert(model);
                            _unitOfWork.Save();
                        }
                    }

                    else
                    {
                        model.Orgin_Location_Id = routeEntity.Orgin_Location_Id;
                        model.Orgin_Location_Code = routeEntity.Orgin_Location_Code;
                        model.Orgin_Location = routeEntity.Orgin_Location;
                        //model.Target_Location_Id = RoutMast.Target_Location_Id;
                        //model.Target_Location_Code = RoutMast.Target_Location_Code;
                        //  model.Target_Loc_Type = RoutMast.Target_Loc_Type;
                        model.Route_Code = Route_Code;
                        model.Remarks = routeEntity.Remarks;
                        model.Target_Location = routeEntity.Target_Location;
                        model.CreatedBy = routeEntity.UpdatedBy;
                        model.CreatedDate = DateTime.Now;

                        _unitOfWork.RouteRepository.Insert(model);
                        _unitOfWork.Save();
                    }

                    scope.Complete();
                }
                success = true;
            }

            return success;
        }
        //public bool UpdateCustSKUMaster(RouteMasterEntity customerSKUMasterModelEntity)
        //{
        //    var success = false;
        //    if (customerSKUMasterModelEntity != null)
        //    {
        //        using (var scope = new TransactionScope())
        //        {
        //            if (customerSKUMasterModelEntity.Routes != null)
        //            {
        //                var oldOneEquip = (from wTd in DB.Route_Master
        //                                   where wTd.Route_Id == customerSKUMasterModelEntity.Route_Id
        //                                   select wTd.Route_Master_Id).ToList();
        //                var tempEquip = customerSKUMasterModelEntity.Routes;
        //                var newOneEquip = tempEquip.Select(a => a.Route_Master_Id).ToList();

        //                var wantTodeleteEquip = oldOneEquip.Except(newOneEquip).ToList();

        //                if (wantTodeleteEquip != null)
        //                {
        //                    foreach (var t in wantTodeleteEquip)
        //                    {
        //                        var wd = (from dw in DB.Route_Master
        //                                  where dw.Route_Master_Id == t && dw.Route_Id == customerSKUMasterModelEntity.Route_Id
        //                                  select dw).FirstOrDefault();
        //                        _unitOfWork.CustomerSKUMappingRepository.Delete(wd);
        //                        _unitOfWork.Save();

        //                        DB.SaveChanges();
        //                    }

        //                }
        //            }
        //            var sku = new Customer_SKU_Mapping();
        //            foreach (var t in customerSKUMasterModelEntity.Routes)
        //            {
        //                if (t.Route_Master_Id != 0)
        //                {
        //                    var Usku = _unitOfWork.RouteRepository.GetByID(t.Route_Master_Id);
        //                    if (sku != null)
        //                    {
        //                        Usku.Route_Id = customerSKUMasterModelEntity.Route_Id;
        //                        Usku.Customer_Code = customerSKUMasterModelEntity.Customer_Code;
        //                        Usku.Customer_Name = customerSKUMasterModelEntity.Customer_Name;
        //                        Usku.Customer_SKU_Name = t.Customer_SKU_Name;
        //                        Usku.SKU_Id = t.SKU_Id;
        //                        Usku.SKU_Code = t.SKU_Code;
        //                        Usku.SKU_Name = t.SKU_Name;
        //                        Usku.UOM = t.UOM;
        //                        Usku.EAN_Number = t.EAN_Number;
        //                        Usku.Price = t.Price;
        //                        Usku.UpdatedDate = DateTime.UtcNow;
        //                        Usku.Customer_SKU_Name = t.Customer_SKU_Name;
        //                        Usku.UpdatedBy = t.UpdatedBy;

        //                        _unitOfWork.CustomerSKUMappingRepository.Update(Usku);
        //                        _unitOfWork.Save();
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }
        //                }
        //                else if (t.Customer_SKU_Mapping_Id == 0)
        //                {
        //                    sku.Customer_Id = customerSKUMasterModelEntity.Customer_Id;
        //                    sku.Customer_Code = customerSKUMasterModelEntity.Customer_Code;
        //                    sku.Customer_Name = customerSKUMasterModelEntity.Customer_Name;
        //                    sku.Customer_SKU_Name = t.Customer_SKU_Name;
        //                    sku.SKU_Id = t.SKU_Id;
        //                    sku.SKU_Code = t.SKU_Code;
        //                    sku.SKU_Name = t.SKU_Name;
        //                    sku.UOM = t.UOM;
        //                    sku.EAN_Number = t.EAN_Number;
        //                    sku.Price = t.Price;
        //                    sku.Customer_SKU_Name = t.Customer_SKU_Name;
        //                    sku.UpdatedBy = t.UpdatedBy;
        //                    sku.CreatedDate = DateTime.UtcNow;
        //                    sku.UpdatedDate = DateTime.UtcNow;
        //                    sku.Customer_SKU_Name = t.Customer_SKU_Name;
        //                    sku.CreatedBy = t.UpdatedBy;

        //                    _unitOfWork.CustomerSKUMappingRepository.Insert(sku);
        //                    _unitOfWork.Save();
        //                    success = true;

        //                }
        //                else
        //                {
        //                    return false;
        //                }

        //            }
        //            scope.Complete();
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public dynamic GetAll()
        {
            var V = (from a in DB.Route_Master
                     orderby a.Route
                     select new
                     {
                         Route_Master_Id = a.Route_Master_Id,
                         Route_Code = a.Route_Code,
                         Route_Id = a.Route_Id,
                         Route = a.Route
                     }).ToList();


            return V;
        }
        public dynamic GetAllForMapping()
        {
            var V = (from f in DB.Route_Master
                     group f by new
                     {
                         f.Route_Code
                     } into temp

                     select new
                     {
                         Route = (from a in DB.Route_Master
                                  where a.Route_Code == temp.Key.Route_Code
                                  orderby a.Route
                                  select new
                                  {
                                      Orgin_Location_Id = a.Orgin_Location_Id,
                                      Orgin_Location_Code = a.Orgin_Location_Code,
                                      Orgin_Location = a.Orgin_Location,
                                      Route_Code = a.Route_Code,
                                      Target_Location_Id = a.Target_Location_Id,
                                      Target_Location_Code = a.Target_Location_Code,
                                      Target_Location = a.Target_Location,
                                      Remrks = a.Remarks,
                                      Route = a.Route
                                  }).ToList()
                     }).ToList();


            return V;
        }
        public dynamic GetAllRoutes(int? roleId, string Url)
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
            var V = (from f in DB.Route_Master
                     group f by new
                     {
                         f.Orgin_Location_Id,
                         f.Orgin_Location_Code,
                         f.Orgin_Location,
                         f.Target_Location_Id,
                         f.Target_Location_Code,
                         f.Target_Location,
                         f.Route_Code
                     } into temp

                     select new
                     {
                         Orgin_Location_Id = temp.Key.Orgin_Location_Id,
                         Orgin_Location_Code = temp.Key.Orgin_Location_Code,
                         Orgin_Location = temp.Key.Orgin_Location,
                         Route_Code = temp.Key.Route_Code,
                         Target_Location_Id = temp.Key.Target_Location_Id,
                         Target_Location_Code = temp.Key.Target_Location_Code,
                         Target_Location = temp.Key.Target_Location,
                         Route = (from a in DB.Route_Master
                                  where a.Target_Location_Code == temp.Key.Target_Location_Code && a.Orgin_Location_Code == temp.Key.Orgin_Location_Code && a.Route_Code == temp.Key.Route_Code
                                  orderby a.Route
                                  select new
                                  {
                                      Remrks = a.Remarks,
                                      Route = a.Route
                                  }).ToList(),
                         //var V = (from f in DB.Route_Master
                         //         group f by new
                         //         {
                         //           f.Route_Code
                         //         } into temp

                         //         select new
                         //         {                         
                         //             Route = (from a in DB.Route_Master
                         //                      where a.Route_Code == temp.Key.Route_Code
                         //                      orderby a.Route
                         //                      select new
                         //                      {
                         //                          Orgin_Location_Id = a.Orgin_Location_Id,
                         //                          Orgin_Location_Code = a.Orgin_Location_Code,
                         //                          Orgin_Location = a.Orgin_Location,
                         //                          Route_Code = a.Route_Code,
                         //                          Target_Location_Id = a.Target_Location_Id,
                         //                          Target_Location_Code = a.Target_Location_Code,
                         //                          Target_Location = a.Target_Location,
                         //                          Remrks = a.Remarks,
                         //                          Route = a.Route
                         //                      }).ToList(),


                         is_Create = iCrt,
                         is_Delete = isDel,
                         is_Edit = isEdt,
                         is_Approval = isApp,
                         is_View = isViw,
                         // LineItem = temp
                     }).ToList();
            return V;
        }
        public RouteMasterEntity Get(string Route_Code)
        {
            var qry = (from a in DB.Route_Master
                       where a.Route_Code == Route_Code
                       orderby a.Route
                       select new RouteMasterEntity
                        {
                            Route_Master_Id = a.Route_Master_Id,
                            Orgin_Location_Code = a.Orgin_Location_Code,
                            Orgin_Location_Id = a.Orgin_Location_Id,
                            Orgin_Location = a.Orgin_Location,
                            //Target_Location_Id=a.Target_Location_Id,
                            //Target_Location_Code=a.Target_Location_Code,
                            //Target_Loc_Type=a.Target_Loc_Type,
                            Target_Location = a.Target_Location,
                            CreatedBy = a.CreatedBy,
                            CreatedDate = a.CreatedDate,
                            UpdatedDate = a.UpdatedDate,
                            UpdatedBy = a.UpdatedBy,
                            Remarks = a.Remarks,
                            Route_Code = a.Route_Code,
                            Routes = (from aa in DB.Route_Master
                                      where aa.Route_Code == a.Route_Code && aa.Route != null
                                      orderby aa.Route
                                      select new RoutesList
                                       {
                                           Remarks = aa.Remarks,
                                           Route_Id = aa.Route_Id.Value,
                                           Route = aa.Route,
                                       }).ToList(),


                        }).FirstOrDefault();

            return qry;


        }
        //public IEnumerable<RouteMasterEntity> GetAllRoutes()
        //{
        //    //   var result = new List<RouteMasterEntity>();
        //    //var qry = new List<RouteMasterEntity>();
        //    //List<RouteMasterEntity> yyy = new List<RouteMasterEntity>();
        //    //List<RouteMasterEntity> cc
        //    var qry = (from a in DB.Route_Master
        //               orderby a.Route
        //               select new RouteMasterEntity
        //                {
        //                    Route_Master_Id = a.Route_Master_Id,
        //                    Orgin_Location = a.Orgin_Location,
        //                    Target_Location = a.Target_Location,
        //                    CreatedBy = a.CreatedBy,
        //                    CreatedDate = a.CreatedDate,
        //                    UpdatedDate = a.UpdatedDate,
        //                    UpdatedBy = a.UpdatedBy,
        //                    Route_Code = a.Route_Code,
        //                    Routes = (from aa in DB.Route_Master
        //                              where aa.Route_Code == a.Route_Code
        //                              orderby aa.Route
        //                              select new RoutesList
        //                               {
        //                                   Route = aa.Route,
        //                               }).ToList(),


        //                }).ToList().Distinct();
        //    //foreach (RouteMasterEntity iy in cc)
        //    //{
        //    //    var oyp = new RouteMasterEntity
        //    //    {
        //    //        Template_Id = iy.Template_Id,
        //    //        Template_name = iy.Template_name,
        //    //        Cust_Id = iy.Cust_Id,
        //    //        Customer_Code = iy.Customer_Code,
        //    //        Cust_Name = iy.Cust_Name
        //    //    };
        //    //    yyy.Add(oyp);
        //    //}

        //    //List<RouteMasterEntity> Custs = new List<RouteMasterEntity>();

        //    //foreach (RouteMasterEntity uu in cc)
        //    //{
        //    //    qry = (from y in DB.Customer_LabelTemplate_Mapping
        //    //           join map in DB.Customer_Label_Template on y.Template_Id equals map.Template_Id
        //    //           join cst in DB.Customers on y.Customer_Id equals cst.Cust_Id
        //    //           where y.Template_Id == uu.Template_Id
        //    //           orderby y.Template_Id
        //    //           select new RouteMasterEntity
        //    //           {
        //    //               Template_Id = y.Template_Id,
        //    //               Template_name = map.Template_name,
        //    //               Cust_Id = cst.Cust_Id,
        //    //               Customer_Code = cst.Customer_Code,
        //    //               Cust_Name = cst.Customer_Name
        //    //           }).Distinct().ToList();
        //    //    foreach (var tt in qry)
        //    //    {
        //    //        var op = new RouteMasterEntity
        //    //        {
        //    //            Template_Id = tt.Template_Id,
        //    //            Template_name = tt.Template_name,
        //    //            Cust_Id = tt.Cust_Id,
        //    //            Customer_Code = tt.Customer_Code,
        //    //            Cust_Name = tt.Cust_Name
        //    //        };
        //    //        Custs.Add(op);
        //    //    }

        //    //}
        //    //result = Custs.ToList();
        //    return qry;
        //

        //}
        public Route_Master_Auto_Num_Gen GetRouterAutoIncrement()
        {
            var autoinc = DB.Route_Master_Auto_Num_Gen.Where(x => x.Route_Master_Auto_Gen_Id == 1).FirstOrDefault();

            var model = new Route_Master_Auto_Num_Gen
            {

                Route_Master_Auto_Gen_Id = autoinc.Route_Master_Auto_Gen_Id,
                Route_Prefix = autoinc.Route_Prefix,
                Route_Last_No = autoinc.Route_Last_No
            };

            return model;
        }
        public bool InsertRoute(RouteMasterEntity RoutMast)
        {
            string RCode, R_prefix;
            int? incNumber;

            using (var iscope = new TransactionScope())
            {
                Route_Master_Auto_Num_Gen autoIncNumber = GetRouterAutoIncrement();
                incNumber = autoIncNumber.Route_Last_No;
                R_prefix = autoIncNumber.Route_Prefix;
                int? incrementedValue = incNumber + 1;
                var CSIincrement = DB.Route_Master_Auto_Num_Gen.Where(x => x.Route_Master_Auto_Gen_Id == 1).FirstOrDefault();
                CSIincrement.Route_Last_No = incrementedValue;
                _unitOfWork.RouteMasterNumGenRepository.Update(CSIincrement);
                _unitOfWork.Save();
                RCode = R_prefix + String.Format("{0:0}", incNumber);
                iscope.Complete();
            }
            using (var scope = new TransactionScope())
            {
                var model = new Route_Master();
                if (RoutMast.Routes.Count() != 0)
                {
                    foreach (var fSub in RoutMast.Routes)
                    {
                        model.Orgin_Location_Id = RoutMast.Orgin_Location_Id;
                        model.Orgin_Location_Code = RoutMast.Orgin_Location_Code;
                        model.Orgin_Location = RoutMast.Orgin_Location;
                        //model.Target_Location_Id = RoutMast.Target_Location_Id;
                        //model.Target_Location_Code = RoutMast.Target_Location_Code;
                        //  model.Target_Loc_Type = RoutMast.Target_Loc_Type;
                        model.Route_Code = RCode;
                        model.Remarks = RoutMast.Remarks;
                        model.Route_Id = fSub.Route_Id;
                        model.Target_Location = RoutMast.Target_Location;
                        model.CreatedBy = RoutMast.CreatedBy;
                        model.CreatedDate = DateTime.Now;
                        model.Route = fSub.Route;
                        _unitOfWork.RouteRepository.Insert(model);
                        _unitOfWork.Save();
                    }
                }
                else
                {
                    model.Orgin_Location_Id = RoutMast.Orgin_Location_Id;
                    model.Orgin_Location_Code = RoutMast.Orgin_Location_Code;
                    model.Orgin_Location = RoutMast.Orgin_Location;
                    //model.Target_Location_Id = RoutMast.Target_Location_Id;
                    //model.Target_Location_Code = RoutMast.Target_Location_Code;
                    //  model.Target_Loc_Type = RoutMast.Target_Loc_Type;
                    model.Route_Code = RCode;
                    model.Remarks = RoutMast.Remarks;
                    model.Target_Location = RoutMast.Target_Location;
                    model.CreatedBy = RoutMast.CreatedBy;
                    model.CreatedDate = DateTime.Now;

                    _unitOfWork.RouteRepository.Insert(model);
                    _unitOfWork.Save();
                }

                scope.Complete();
            }
            return true;
        }


        public bool DeleteRoute(string Route_Code)
        {
            var success = false;

            var CLT = from ord in DB.Route_Master
                      where ord.Route_Code == Route_Code
                      select ord;
            foreach (var ord in CLT)
            {
                DB.Route_Master.Remove(ord);
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
