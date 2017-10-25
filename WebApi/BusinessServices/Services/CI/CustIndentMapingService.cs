using BusinessEntities;
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

namespace BusinessServices.Services.CI
{
    public class CustIndentMapingService : ICustIndentMapingService
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;

        public CustIndentMapingService()
        {
            _unitOfWork = new UnitOfWork();
        }

      

        public bool CreateCustomerIndentMapping(Customer_CustomerIndetMap_Entity intendEntity)
        {
            bool result = false;

            using (var scope = new TransactionScope())
            {
                foreach (var cust in intendEntity.Cutomers)
                {
                    var custMapping = new Customer_Rate_Template_Mapping
                    {
                        Customer_Id = cust.Customer_Id,
                        Customer_Name = cust.Customer_Name,
                        Template_ID = intendEntity.Template_ID,
                        Template_Code = intendEntity.Template_Code,
                        Template_Name = intendEntity.Template_Name,
                        CreateBy = intendEntity.CreateBy,
                        CreatedDate = DateTime.UtcNow,
                        Region = intendEntity.Region,
                        Region_Code = intendEntity.Region_Code,
                        Region_Id = intendEntity.Region_Id,
                        DC_Id = intendEntity.DC_Id,
                        DC_Code = intendEntity.DC_Code,
                        Location_Id = intendEntity.Location_Id,
                        Location_Code = intendEntity.Location_Code
                    };

                    _unitOfWork.CustomerIndentMappingRepository.Insert(custMapping);
                    _unitOfWork.Save();
                }

                result = true;
                scope.Complete();
            }

            return result;
        }

        public bool UpdateCustomerIndentMapping(int? cId, Customer_CustomerIndetMap_Entity intendEntity)
        {
            bool result = false;

            if (intendEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var CLT = (from ord in DB.Customer_Rate_Template_Mapping
                               where ord.Template_ID == cId
                               select ord).ToList();

                    try
                    {
                        foreach (var ord1 in CLT)
                        {
                            DB.Customer_Rate_Template_Mapping.Remove(ord1);
                        }
                        DB.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    scope.Complete();
                }

                using (var scope = new TransactionScope())
                {
                    foreach (var cust in intendEntity.Cutomers)
                    {
                        var custMapping = new Customer_Rate_Template_Mapping
                        {
                            Customer_Id = cust.Customer_Id,
                            Customer_Name = cust.Customer_Name,
                            Template_ID = intendEntity.Template_ID,
                            Template_Code = intendEntity.Template_Code,
                            Template_Name = intendEntity.Template_Name,
                            UpdateBy = intendEntity.UpdateBy,
                            UpdateDate = DateTime.UtcNow,
                            Region = intendEntity.Region,
                            Region_Code = intendEntity.Region_Code,
                            Region_Id = intendEntity.Region_Id,
                            DC_Id = intendEntity.DC_Id,
                            DC_Code = intendEntity.DC_Code,
                            Location_Id = intendEntity.Location_Id,
                            Location_Code = intendEntity.Location_Code
                        };

                        _unitOfWork.CustomerIndentMappingRepository.Insert(custMapping);
                        _unitOfWork.Save();

                        result = true;

                    }
                    scope.Complete();
                }
            }
            return result;
        }

        public List<Customer_CustomerIndetMap_Entity> searchMapping(int? roleId, string region, string location, string dccode, string Url)
        {
            List<Customer_CustomerIndetMap_Entity> returnList = new List<Customer_CustomerIndetMap_Entity>();
            List<Customer_CustomerIndetMap_Entity> returnList1 = new List<Customer_CustomerIndetMap_Entity>();

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


            var list = (from x in DB.Customer_Rate_Template_Mapping
                        orderby x.Customer_Name
                        select new Customer_CustomerIndetMap_Entity
                        {
                            CRT_Mapping_ID = x.CRT_Mapping_ID,
                            Cutomers = (from y in DB.Customer_Rate_Template_Mapping
                                        where y.Template_ID == x.Template_ID
                                        select new CustomerDetailEntity
                                        {
                                            Customer_Id = y.Customer_Id,
                                            Customer_Name = y.Customer_Name
                                        }).ToList(),
                            Template_ID = x.Template_ID,
                            Template_Code = x.Template_Code,
                            Template_Name = x.Template_Name,
                            Region_Id = x.Region_Id,
                            Region_Code = x.Region_Code,
                            Region = x.Region,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            is_Create = iCrt,
                            is_Delete = isDel,
                            is_Edit = isEdt,
                            is_Approval = isApp,
                            is_View = isViw,
                            Location_Code = x.Location_Code
                        }).ToList();

           

            if (dccode != null && location == "null")
            {
                var filterlist = list.Where(x => x.Region == region && x.DC_Code == dccode).ToList();

                foreach (var li in filterlist)
                {
                    returnList.Add(li);
                }
            }
            else if (dccode == "null" && location != null)
            {
                var filterlist = list.Where(x => x.Region == region && x.Location_Code == location).ToList();

                foreach (var li in filterlist)
                {
                    returnList.Add(li);
                }
            }

            foreach (var c in returnList)
            {
                var checkVal = returnList1.Where(x => x.Template_ID == c.Template_ID).FirstOrDefault();
                if (checkVal == null)
                {
                    returnList1.Add(c);
                }
            }
           return returnList1;
        }

        public List<Customer_CustomerIndetMap_Entity> searchMappingIndent(int? roleId, int regionid, int locationid, int dcid, string Url)
        {
            List<Customer_CustomerIndetMap_Entity> returnList = new List<Customer_CustomerIndetMap_Entity>();
            List<Customer_CustomerIndetMap_Entity> returnList1 = new List<Customer_CustomerIndetMap_Entity>();
            List<Customer_CustomerIndetMap_Entity> returnList2 = new List<Customer_CustomerIndetMap_Entity>();

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
            var list = (from x in DB.Customer_Rate_Template_Mapping
                        orderby x.Customer_Name
                        select new Customer_CustomerIndetMap_Entity
                        {
                            CRT_Mapping_ID = x.CRT_Mapping_ID,
                            Cutomers = (from y in DB.Customer_Rate_Template_Mapping
                                        where y.Template_ID == x.Template_ID
                                        select new CustomerDetailEntity
                                        {
                                            Customer_Id = y.Customer_Id,
                                            Customer_Name = y.Customer_Name
                                        }).ToList(),
                            Template_ID = x.Template_ID,
                            Template_Code = x.Template_Code,
                            Template_Name = x.Template_Name,
                            Region_Id = x.Region_Id,
                            Region_Code = x.Region_Code,
                            Region = x.Region,
                            DC_Id = x.DC_Id,
                            DC_Code = x.DC_Code,
                            Location_Id = x.Location_Id,
                            Location_Code = x.Location_Code,
                            is_Create = iCrt,
                            is_Delete = isDel,
                            is_Edit = isEdt,
                            is_Approval = isApp,
                            is_View = isViw,
                            customerIndet = (from y in DB.Customer_Indent
                                             where y.Customer_Id == x.Customer_Id && y.Price_Template_ID == x.Template_ID
                                             select new CustomerIndents
                                             {
                                                 Indent_ID = y.Indent_ID,
                                                 Indent_Name = y.Indent_Name,
                                                 Indent_Code = y.Indent_Code,
                                                 Region_Id = y.Region_Id,
                                                 DC_Id = y.DC_Id,
                                                 Location_Id = y.Location_Id,
                                                 SKU_Type_Id = y.SKU_Type_Id
                                             }).ToList()
                        }).ToList();
           
            if (dcid != 0 && locationid == 0)
            {
                var filterlist = list.Where(x => x.Region_Id == regionid && x.DC_Id == dcid).ToList();

                foreach (var li in filterlist)
                {
                    returnList.Add(li);
                }
            }
            else if (dcid == 0 && locationid != 0)
            {
                var filterlist = list.Where(x => x.Region_Id == regionid && x.Location_Id == locationid).ToList();

                foreach (var li in filterlist)
                {
                    returnList.Add(li);
                }
            }

            foreach (var list1 in returnList)
            {
                foreach (var line in list1.customerIndet)
                {
                    var aligned = new Customer_CustomerIndetMap_Entity
                    {
                        CRT_Mapping_ID = list1.CRT_Mapping_ID,
                        Cutomers = (from y in DB.Customer_Rate_Template_Mapping
                                    where y.Template_ID == list1.Template_ID
                                    select new CustomerDetailEntity
                                    {
                                        Customer_Id = y.Customer_Id,
                                        Customer_Name = y.Customer_Name
                                    }).ToList(),
                        Template_ID = list1.Template_ID,
                        Template_Code = list1.Template_Code,
                        Template_Name = list1.Template_Name,
                        Region_Id = list1.Region_Id,
                        Region_Code = list1.Region_Code,
                        Region = list1.Region,
                        DC_Id = list1.DC_Id,
                        is_Create = iCrt,
                        is_Delete = isDel,
                        is_Edit = isEdt,
                        is_Approval = isApp,
                        is_View = isViw,
                        DC_Code = list1.DC_Code,
                        Location_Id = list1.Location_Id,
                        Location_Code = list1.Location_Code,
                        customerIndet = new List<CustomerIndents>
                        {
                            new CustomerIndents
                            {
                                Indent_ID = line.Indent_ID,
                                Indent_Name = line.Indent_Name,
                                Indent_Code=line.Indent_Code,
                                Region_Id = line.Region_Id,
                                DC_Id = line.DC_Id,
                                Location_Id = line.Location_Id,
                                SKU_Type_Id = line.SKU_Type_Id
                            }
                            
                        }
                    };

                    returnList1.Add(aligned);
                }
            }


            foreach (var list1 in returnList1)
            {
                var checkVal = returnList1.Where(x => x.Template_ID == list1.Template_ID).FirstOrDefault();
                if (checkVal != null)
                {
                    returnList2.Add(list1);
                }
            }

            return returnList2;
        }

        public List<Customer_CustomerIndetMap_Entity> getIndentMapping()
        {
            List<Customer_CustomerIndetMap_Entity> returnList1 = new List<Customer_CustomerIndetMap_Entity>();

            var list = (from x in DB.Customer_Rate_Template_Mapping
                        orderby x.Customer_Name
                        select new Customer_CustomerIndetMap_Entity
                        {
                            CRT_Mapping_ID = x.CRT_Mapping_ID,
                            Cutomers = (from y in DB.Customer_Rate_Template_Mapping
                                        where y.Template_ID == x.Template_ID
                                        select new CustomerDetailEntity
                                        {
                                            Customer_Id = y.Customer_Id,
                                            Customer_Name = y.Customer_Name
                                        }).ToList(),
                            Template_ID = x.Template_ID,
                            Template_Code = x.Template_Code,
                            Template_Name = x.Template_Name,
                        }).ToList();

            foreach (var list1 in list)
            {
                var checkVal = returnList1.Where(x => x.Template_ID == list1.Template_ID).FirstOrDefault();
                if (checkVal == null)
                {
                    returnList1.Add(list1);
                }
            }

            return returnList1;
        }


        public Customer_CustomerIndetMap_Entity getSingleIndentMapping(int id)
        {
            var mapping = (from x in DB.Customer_Rate_Template_Mapping
                           where x.CRT_Mapping_ID == id
                           orderby x.Customer_Name
                           select new Customer_CustomerIndetMap_Entity
                           {
                               CRT_Mapping_ID = x.CRT_Mapping_ID,
                               Cutomers = (from y in DB.Customer_Rate_Template_Mapping
                                           where y.Template_ID == x.Template_ID
                                           select new CustomerDetailEntity
                                           {
                                               Customer_Id = y.Customer_Id,
                                               Customer_Name = y.Customer_Name,
                                               Customer_Code = (from z in DB.Customers
                                                                where z.Cust_Id == y.Customer_Id
                                                                select z.Customer_Code).FirstOrDefault()
                                           }).ToList(),
                               Template_ID = x.Template_ID,
                               Template_Code = x.Template_Code,
                               Template_Name = x.Template_Name,
                               Region = x.Region,
                               Region_Code = x.Region_Code,
                               Region_Id = x.Region_Id,
                               DC_Id = x.DC_Id,
                               DC_Code = x.DC_Code,
                               Location_Id = x.Location_Id,
                               Location_Code = x.Location_Code
                           }).FirstOrDefault();

            return mapping;
        }

        public bool deleteMapping(int id)
        {

            var mappDetail = _unitOfWork.CustomerIndentMappingRepository.GetByID(id);
            bool result = false;
            if (mappDetail != null)
            {
                using (var scope = new TransactionScope())
                {
                    _unitOfWork.CustomerIndentMappingRepository.Delete(mappDetail);
                    _unitOfWork.Save();

                    result = true;

                    scope.Complete();
                }
            }

            return result;
        }

      }
}
