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

namespace BusinessServices
{
    public class Customer_Indent_MappingServices : ICustomer_Indent_MappingServices
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;

        public Customer_Indent_MappingServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool CreateCustomerIndentMapping(Customer_Indent_MappingEntity intendEntity)
        {
           bool result = false;

            using (var scope = new TransactionScope())
            {
                foreach (var cust in intendEntity.IndentCutomers)
                {
                    int checkRTCustMap = 0;
                    if (intendEntity.Template_Code != null)
                    {
                        checkRTCustMap = (from i in DB.Customer_Rate_Template_Mapping
                                          where i.Template_Code == intendEntity.Template_Code && i.Customer_Id == cust.Customer_Id
                                          select i.Template_ID).Count();
                    }

                    if (checkRTCustMap > 0)
                    {
                        var custMapping = new Customer_Indent_Template_Mapping
                        {
                            Customer_Id = cust.Customer_Id,
                            Customer_Name = cust.Customer_Name,
                            Indent_ID = intendEntity.Indent_ID,
                            Indent_Code = intendEntity.Indent_Code,
                            Indent_Name = intendEntity.Indent_Name,
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

                        _unitOfWork.CustomerIndentTemplateMappingRepository.Insert(custMapping);
                        _unitOfWork.Save();

                        result = true;                       
                    }
                    else
                    {
                        result = false;
                    }
                }
                scope.Complete();
            }

            return result;
        }

        public bool UpdateCustomerIndentMapping(int? cId, Customer_Indent_MappingEntity intendEntity)
        {
            bool result = false;

            if (intendEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var CLT = (from ord in DB.Customer_Indent_Template_Mapping
                               where ord.Indent_ID == cId
                               select ord).ToList();

                    try
                    {
                        foreach (var ord1 in CLT)
                        {
                            DB.Customer_Indent_Template_Mapping.Remove(ord1);
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
                    foreach (var cust in intendEntity.IndentCutomers)
                    {
                        int checkRTCustMap = 0;
                        if (intendEntity.Template_Code != null)
                        {
                            checkRTCustMap = (from i in DB.Customer_Rate_Template_Mapping
                                              where i.Template_Code == intendEntity.Template_Code && i.Customer_Id == cust.Customer_Id
                                              select i.Template_ID).Count();
                        }

                        if (checkRTCustMap > 0)
                        {
                            var custMapping = new Customer_Indent_Template_Mapping
                            {
                                Customer_Id = cust.Customer_Id,
                                Customer_Name = cust.Customer_Name,
                                Indent_ID = intendEntity.Indent_ID,
                                Indent_Code = intendEntity.Indent_Code,
                                Indent_Name = intendEntity.Indent_Name,
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

                            _unitOfWork.CustomerIndentTemplateMappingRepository.Insert(custMapping);
                            _unitOfWork.Save();

                            result = true;

                        }
                        else
                        {
                            result = false;
                        }
                    }
                   
                    scope.Complete();
                }
            }
            return result;
        }

        public List<Customer_Indent_MappingEntity> searchCustomerIndentMapping(int? roleId, string region, string location, string dccode, string Url)
        {
            List<Customer_Indent_MappingEntity> returnList = new List<Customer_Indent_MappingEntity>();
            List<Customer_Indent_MappingEntity> returnList1 = new List<Customer_Indent_MappingEntity>();

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


            var list = (from x in DB.Customer_Indent_Template_Mapping
                        orderby x.Customer_Name
                        select new Customer_Indent_MappingEntity
                        {
                            CIT_Mapping_ID = x.CIT_Mapping_ID,
                            IndentCutomers = (from y in DB.Customer_Indent_Template_Mapping
                                              where y.Indent_Code == x.Indent_Code
                                              select new CustomerDetailIndentmappingEntity
                                        {
                                            Customer_Id = y.Customer_Id,
                                            Customer_Name = y.Customer_Name
                                        }).ToList(),
                            Indent_ID = x.Indent_ID,
                            Indent_Code = x.Indent_Code,
                            Indent_Name = x.Indent_Name,
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
                var checkVal = returnList1.Where(x => x.Indent_ID == c.Indent_ID).FirstOrDefault();
                if (checkVal == null)
                {
                    returnList1.Add(c);
                }
            }
           return returnList1;
        }

       public Customer_Indent_MappingEntity getSingleIndentMapping(int id)
        {
            var mapping = (from x in DB.Customer_Indent_Template_Mapping
                           where x.CIT_Mapping_ID == id
                           orderby x.Customer_Name
                           select new Customer_Indent_MappingEntity
                           {
                               CIT_Mapping_ID = x.CIT_Mapping_ID,
                               IndentCutomers = (from y in DB.Customer_Indent_Template_Mapping
                                                 where y.Indent_ID == x.Indent_ID
                                                 select new CustomerDetailIndentmappingEntity
                                           {
                                               Customer_Id = y.Customer_Id,
                                               Customer_Name = y.Customer_Name,
                                               Customer_Code = (from z in DB.Customers
                                                                where z.Cust_Id == y.Customer_Id
                                                                select z.Customer_Code).FirstOrDefault()
                                           }).ToList(),
                               Indent_ID = x.Indent_ID,
                               Indent_Code = x.Indent_Code,
                               Indent_Name = x.Indent_Name,
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
                           }).First();

            return mapping;
        }
      
        public bool deleteMapping(int id)
        {

            var mappDetail = _unitOfWork.CustomerIndentTemplateMappingRepository.GetByID(id);
            bool result = false;
            if (mappDetail != null)
            {
                using (var scope = new TransactionScope())
                {
                    _unitOfWork.CustomerIndentTemplateMappingRepository.Delete(mappDetail);
                    _unitOfWork.Save();

                    result = true;

                    scope.Complete();
                }
            }

            return result;
        }

     }
}