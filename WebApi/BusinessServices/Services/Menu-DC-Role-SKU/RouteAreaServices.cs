using BusinessEntities.Entity;
using BusinessServices.Interfaces;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices.Services
{
    public class RouteAreaServices : IRouteAreaService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public RouteAreaServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public dynamic GetAllRouteAreas(int? roleId, string Url)
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
            var V = (from f in DB.Route_Area_Master
                     group f by new
                     {
                         f.Location_Id,
                         f.Location_Code,
                         f.Location_Name,

                     } into temp

                     select new
                     {
                         Location_Id = temp.Key.Location_Id,
                         Location_Code = temp.Key.Location_Code,
                         Location_Name = temp.Key.Location_Name,

                         Areas = (from a in DB.Route_Area_Master
                                  where a.Location_Code == temp.Key.Location_Code
                                  orderby a.Area_Name
                                  select new
                                  {
                                      Pincode = a.Pincode,
                                      Area_Name = a.Area_Name
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
        public RouteAreaEntity Get(string Location_Code)
        {
            var qry = (from a in DB.Route_Area_Master
                       where a.Location_Code == Location_Code
                       orderby a.Area_Name
                       select new RouteAreaEntity
                       {
                           Location_Id = a.Location_Id,
                           Location_Code = a.Location_Code,
                           Location_Name = a.Location_Name,
                           //Target_Location_Id=a.Target_Location_Id,
                           //Target_Location_Code=a.Target_Location_Code,
                           //Target_Loc_Type=a.Target_Loc_Type,
                           Location_Type = a.Location_Type,
                           CreatedBy = a.CreatedBy,
                           CreatedDate = a.CreatedDate,
                           UpdatedDate = a.UpdatedDate,
                           UpdatedBy = a.UpdatedBy,
                           Areas = (from aa in DB.Route_Area_Master
                                    where aa.Location_Code == a.Location_Code
                                    orderby aa.Area_Name
                                    select new RoutesAreaList
                                     {
                                         Area_Name = aa.Area_Name,
                                         Pincode = aa.Pincode
                                     }).ToList(),
                       }).FirstOrDefault();

            return qry;


        }

        public bool InsertRoute(RouteAreaEntity RoutMast)
        {

            using (var scope = new TransactionScope())
            {
                var model = new Route_Area_Master();
                if (RoutMast.Areas.Count() != 0)
                {
                    foreach (var fSub in RoutMast.Areas)
                    {
                        model.Location_Id = RoutMast.Location_Id;
                        model.Location_Code = RoutMast.Location_Code;
                        model.Location_Name = RoutMast.Location_Name;
                        //model.Target_Location_Id = RoutMast.Target_Location_Id;
                        //model.Target_Location_Code = RoutMast.Target_Location_Code;
                        //  model.Target_Loc_Type = RoutMast.Target_Loc_Type;
                        model.Location_Type = "DC";
                        model.Pincode = fSub.Pincode;
                        model.Area_Name = fSub.Area_Name;
                        model.CreatedBy = RoutMast.CreatedBy;
                        model.CreatedDate = DateTime.Now;

                        _unitOfWork.RouteAreaRepository.Insert(model);
                        _unitOfWork.Save();
                    }
                }


                scope.Complete();
            }
            return true;
        }


        public bool Delete(string Location_Code)
        {
            var success = false;

            var CLT = from ord in DB.Route_Area_Master
                      where ord.Location_Code == Location_Code
                      select ord;
            foreach (var ord in CLT)
            {
                DB.Route_Area_Master.Remove(ord);
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
        public bool Update(string Location_Code, RouteAreaEntity routeEntity)
        {
            var success = false;
            if (routeEntity != null)
            {
                var CLT = from ord in DB.Route_Area_Master
                          where ord.Location_Code == Location_Code
                          select ord;
                try
                {
                    foreach (var ord in CLT)
                    {
                        DB.Route_Area_Master.Remove(ord);
                    }
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                using (var scope = new TransactionScope())
                {
                    var model = new Route_Area_Master();
                    if (routeEntity.Areas.Count() != 0)
                    {
                        foreach (var fSub in routeEntity.Areas)
                        {
                            model.Location_Id = routeEntity.Location_Id;
                            model.Location_Code = routeEntity.Location_Code;
                            model.Location_Name = routeEntity.Location_Name;
                            //model.Target_Location_Id = routeEntity.Target_Location_Id;
                            //model.Target_Location_Code = routeEntity.Target_Location_Code;
                            //model.Target_Loc_Type = routeEntity.Target_Loc_Type;
                            model.Location_Type = "DC";
                            model.Pincode = fSub.Pincode;
                            model.Area_Name = fSub.Area_Name;
                            model.CreatedBy = routeEntity.UpdatedBy;
                            model.CreatedDate = DateTime.Now;
                            model.UpdatedBy = routeEntity.UpdatedBy;
                            model.UpdatedDate = DateTime.Now;

                            _unitOfWork.RouteAreaRepository.Insert(model);
                            _unitOfWork.Save();
                        }
                    }
                    scope.Complete();
                }
                success = true;
            }

            return success;
        }



        public IEnumerable<RoutesAreaList> GetAllRoutes(string Target_Location_Code, string Target_Location_Type)
        {

            var qry = (from a in DB.Route_Area_Master
                       orderby a.Area_Name
                       where a.Location_Code == Target_Location_Code && a.Location_Type == Target_Location_Type
                       select new RoutesAreaList
                                           {
                                               Route_Area_Master_Id = a.Route_Area_Master_Id,

                                               Area_Name = a.Area_Name,
                                           }).ToList();
            //foreach (RouteMasterEntity iy in cc)
            //{
            //    var oyp = new RouteMasterEntity
            //    {
            //        Template_Id = iy.Template_Id,
            //        Template_name = iy.Template_name,
            //        Cust_Id = iy.Cust_Id,
            //        Customer_Code = iy.Customer_Code,
            //        Cust_Name = iy.Cust_Name
            //    };
            //    yyy.Add(oyp);
            //}

            //List<RouteMasterEntity> Custs = new List<RouteMasterEntity>();

            //foreach (RouteMasterEntity uu in cc)
            //{
            //    qry = (from y in DB.Customer_LabelTemplate_Mapping
            //           join map in DB.Customer_Label_Template on y.Template_Id equals map.Template_Id
            //           join cst in DB.Customers on y.Customer_Id equals cst.Cust_Id
            //           where y.Template_Id == uu.Template_Id
            //           orderby y.Template_Id
            //           select new RouteMasterEntity
            //           {
            //               Template_Id = y.Template_Id,
            //               Template_name = map.Template_name,
            //               Cust_Id = cst.Cust_Id,
            //               Customer_Code = cst.Customer_Code,
            //               Cust_Name = cst.Customer_Name
            //           }).Distinct().ToList();
            //    foreach (var tt in qry)
            //    {
            //        var op = new RouteMasterEntity
            //        {
            //            Template_Id = tt.Template_Id,
            //            Template_name = tt.Template_name,
            //            Cust_Id = tt.Cust_Id,
            //            Customer_Code = tt.Customer_Code,
            //            Cust_Name = tt.Cust_Name
            //        };
            //        Custs.Add(op);
            //    }

            //}
            //result = Custs.ToList();
            return qry;


        }
        //public Route_Master_Auto_Num_Gen GetRouterAutoIncrement()
        //{
        //    var autoinc = DB.Route_Master_Auto_Num_Gen.Where(x => x.Route_Master_Auto_Gen_Id == 1).FirstOrDefault();

        //    var model = new Route_Master_Auto_Num_Gen
        //    {

        //        Route_Master_Auto_Gen_Id = autoinc.Route_Master_Auto_Gen_Id,
        //        Route_Prefix = autoinc.Route_Prefix,
        //        Route_Last_No = autoinc.Route_Last_No
        //    };

        //    return model;
        //}
        //public bool InsertRoute(RouteMasterEntity RoutMast)
        //{
        //    string RCode, R_prefix;
        //    int? incNumber;

        //    using (var iscope = new TransactionScope())
        //    {
        //        Route_Master_Auto_Num_Gen autoIncNumber = GetRouterAutoIncrement();
        //        incNumber = autoIncNumber.Route_Last_No;
        //        R_prefix = autoIncNumber.Route_Prefix;
        //        int? incrementedValue = incNumber + 1;
        //        var CSIincrement = DB.Route_Master_Auto_Num_Gen.Where(x => x.Route_Master_Auto_Gen_Id == 1).FirstOrDefault();
        //        CSIincrement.Route_Last_No = incrementedValue;
        //        _unitOfWork.RouteMasterNumGenRepository.Update(CSIincrement);
        //        _unitOfWork.Save();
        //        RCode = R_prefix + String.Format("{0:0}", incNumber);
        //        iscope.Complete();
        //    }
        //    using (var scope = new TransactionScope())
        //    {
        //        var model = new Route_Master();
        //        foreach (var fSub in RoutMast.Routes)
        //        {
        //            //
        //            model.Route_Code = RCode;
        //            model.Orgin_Location = RoutMast.Orgin_Location;
        //            model.Target_Location = RoutMast.Target_Location;
        //            model.CreatedBy = RoutMast.CreatedBy;
        //            model.CreatedDate = DateTime.Now;
        //            model.Route = fSub.Route;
        //            _unitOfWork.RouteRepository.Insert(model);
        //            _unitOfWork.Save();
        //        }
        //        scope.Complete();
        //    }
        //    return true;
        //}


        //public bool DeleteRoute(string Route_Code)
        //{
        //    var success = false;

        //    var CLT = from ord in DB.Customer_LabelTemplate_Mapping
        //              where ord.Template_Id == Template_Id
        //              select ord;
        //    foreach (var ord in CLT)
        //    {
        //        DB.Customer_LabelTemplate_Mapping.Remove(ord);
        //    }
        //    try
        //    {
        //        DB.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        success = false;
        //        return success;
        //    }

        //    return success;
        //}
    }
}