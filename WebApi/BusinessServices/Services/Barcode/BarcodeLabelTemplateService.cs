using AutoMapper;
using BusinessEntities;
using BusinessEntities.Entity;
using DataModel;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;


namespace BusinessServices
{
    public class BarcodeLabelTemplateService : IBarcodeLabelTemplate
    {
        LEAFDBEntities DB = new LEAFDBEntities();
        private readonly UnitOfWork _unitOfWork;
        public BarcodeLabelTemplateService()
        {
            _unitOfWork = new UnitOfWork();
        }
        //-------------------------------GET ALL-----------------------------

        public IEnumerable<LabelFieldsEntity> GetAllLabelField()
        {
            var LF = (from x in DB.Label_Fields
                      orderby x.Field_Name
                      where x.is_Deleted == false
                      select new LabelFieldsEntity
                      {
                          Field_Id = x.Field_Id,
                          Field_Name = x.Field_Name,
                          Data_Type = x.Data_Type,
                          Size = x.Size,
                          Remarks = x.Remarks,
                          CreatedDate = DateTime.Now,
                          CreatedBy = x.CreatedBy
                      }
                     ).ToList();

            return LF;
        }

        public IEnumerable<CustomerLabelTemplateEntity> GetAllLabelTemplate()
        {

            var LF = (from r in DB.Customer_Label_Template.AsEnumerable()
                      where r.is_Deleted == false
                      orderby r.Template_name
                      group r by r.Template_Id into IdGroup
                      let x = IdGroup.Distinct().First()

                      select new CustomerLabelTemplateEntity
                      {
                          Cust_Label_Template_Id = x.Cust_Label_Template_Id,
                          Template_Id = x.Template_Id,
                          Template_Type = x.Template_Type,
                          Template_name = x.Template_name,
                          Field_Id = x.Field_Id,
                          Field_Name = x.Field_Name,
                          Remarks = x.Remarks,
                          CreatedDate = DateTime.Now,
                          is_Deleted = false,
                          CreatedBy = x.CreatedBy
                      });


            return LF;
        }
        public IEnumerable<CustTemplateEntity> GetAllCustomerTemplate()
        {
            var qu = (from a in DB.Customer_LabelTemplate_Mapping
                      join map in DB.Customer_Label_Template on a.Template_Id equals map.Template_Id
                      join cust in DB.Customers on a.Customer_Id equals cust.Cust_Id
                      orderby map.Template_name
                      select new CustTemplateEntity
                      {
                          Template_Id = a.Template_Id,
                          Template_name = map.Template_name,
                          Template_Type = map.Template_Type,
                          Cust_Id = a.Customer_Id,
                          Cust_Name = cust.Customer_Name,
                      }).ToList();
            return qu;
        }

       public IEnumerable<PrintedBarcodeDetailsEntity> GetAllPrintBarcode()
        {
            var PB = _unitOfWork.PrintBarcodeRepository.GetAll().ToList();
            if (PB.Any())
            {
                Mapper.CreateMap<Printed_Barcode_Details, PrintedBarcodeDetailsEntity>();
                var pbModel = Mapper.Map<List<Printed_Barcode_Details>, List<PrintedBarcodeDetailsEntity>>(PB);
                return pbModel;
            }
            return null;
        }

        public Customer_Label_Auto_Num_Gen GetLabelAutoIncrement()
        {
            var autoinc = DB.Customer_Label_Auto_Num_Gen.Where(x => x.Cust_Label_Auto_Gen_Id == 1).FirstOrDefault();

            var model = new Customer_Label_Auto_Num_Gen
            {

                Cust_Label_Auto_Gen_Id = autoinc.Cust_Label_Auto_Gen_Id,
                Template_Prefix = autoinc.Template_Prefix,
                Template_Last_No = autoinc.Template_Last_No
            };

            return model;
        }

        //-----------------------------------------GET BY NAME-------------------------------

        public List<LabelFieldsEntity> GetLabelFieldByCategory(string Field_Name)
        {
            List<LabelFieldsEntity> list = new List<LabelFieldsEntity>();

            list = (from x in DB.Label_Fields
                    where x.Field_Name == Field_Name && x.is_Deleted == false
                    orderby x.Field_Name
                    select new LabelFieldsEntity
                    {
                        Field_Id = x.Field_Id,
                        Field_Name = x.Field_Name,
                        Data_Type = x.Data_Type,
                        Size = x.Size,
                        Remarks = x.Remarks,
                        CreatedDate = DateTime.Now,
                        CreatedBy = x.CreatedBy

                    }).ToList();
            return list;
        }

        public List<CustomerLabelTemplateEntity> GetLabelTemplateByName(string Templatename)
        {
            var CLT = (from a in DB.Customer_Label_Template
                       where a.Template_name == Templatename && a.is_Deleted != true
                       orderby a.Template_name
                       select new CustomerLabelTemplateEntity
                       {
                           Cust_Label_Template_Id = a.Cust_Label_Template_Id,
                           Template_Id = a.Template_Id,
                           Template_name = a.Template_name,
                           Template_Type = a.Template_Type,
                           Field_Id = a.Field_Id,
                           Field_Name = a.Field_Name,
                           Field_Value = a.Field_Value,
                           Remarks = a.Remarks,
                           CreatedDate = a.CreatedDate,
                           UpdatedDate = a.UpdatedDate,
                           CreatedBy = a.CreatedBy,
                           UpdatedBy = a.UpdatedBy
                       }).Distinct().ToList();
            List<CustomerLabelTemplateEntity> menuItems = new List<CustomerLabelTemplateEntity>();

            foreach (CustomerLabelTemplateEntity aa in CLT.Cast<CustomerLabelTemplateEntity>().Take(1))
            {
                if (aa.Template_name == Templatename)
                {

                    var items = new CustomerLabelTemplateEntity
                    {
                        Cust_Label_Template_Id = aa.Cust_Label_Template_Id,
                        Template_Id = aa.Template_Id,
                        Template_Type = aa.Template_Type,
                        Template_name = aa.Template_name,
                        CreatedDate = aa.CreatedDate,
                        UpdatedDate = aa.UpdatedDate,
                        CreatedBy = aa.CreatedBy,
                        UpdatedBy = aa.UpdatedBy,
                        FieldsSet = new List<FieldEntity>
                        {
                        }

                    };
                    menuItems.Add(items);
                }
            }
            foreach (CustomerLabelTemplateEntity menulist in menuItems)
            {
                foreach (CustomerLabelTemplateEntity menulist1 in CLT)
                {
                    if (menulist.Template_Id == menulist1.Template_Id)
                    {
                        var menu = menuItems.Where(x => x.Template_Id == menulist.Template_Id);

                        if (menu.Count() != 0)
                        {
                            var submenu = new FieldEntity
                            {
                                Field_Id = menulist1.Field_Id,
                                Field_Name = menulist1.Field_Name,
                                Remarks = menulist1.Remarks,
                                Field_Value = menulist1.Field_Value

                            };
                            menulist.FieldsSet.Add(submenu);
                        }
                    }
                }
            }
            var result = menuItems.ToList();
            return result;

        }

        public List<CustTemplateEntity> GetAllCustomers()
        {
            var result = new List<CustTemplateEntity>();
            var qry = new List<CustTemplateEntity>();
            List<CustTemplateEntity> yyy = new List<CustTemplateEntity>();
            List<CustTemplateEntity> cc = (from a in DB.Customer_Label_Template
                                           join g in DB.Customer_LabelTemplate_Mapping on a.Template_Id equals g.Template_Id
                                           where a.is_Deleted == false
                                           orderby a.Template_name
                                           select new CustTemplateEntity
                                           {
                                               Template_Id = g.Template_Id,
                                               Template_Type = a.Template_Type,
                                               Template_name = a.Template_name,
                                           }).Distinct().ToList();
            foreach (CustTemplateEntity iy in cc)
            {
                var oyp = new CustTemplateEntity
                {
                    Template_Id = iy.Template_Id,
                    Template_name = iy.Template_name,
                    Template_Type = iy.Template_Type,
                    Cust_Id = iy.Cust_Id,
                    Customer_Code = iy.Customer_Code,
                    Cust_Name = iy.Cust_Name
                };
                yyy.Add(oyp);
            }

            List<CustTemplateEntity> Custs = new List<CustTemplateEntity>();

            foreach (CustTemplateEntity uu in cc)
            {
                qry = (from y in DB.Customer_LabelTemplate_Mapping
                       join map in DB.Customer_Label_Template on y.Template_Id equals map.Template_Id
                       join cst in DB.Customers on y.Customer_Id equals cst.Cust_Id
                       where y.Template_Id == uu.Template_Id
                       orderby y.Template_Id
                       select new CustTemplateEntity
                       {
                           Template_Id = y.Template_Id,
                           Template_name = map.Template_name,
                           Cust_Id = cst.Cust_Id,
                           Template_Type = map.Template_Type,
                           Customer_Code = cst.Customer_Code,
                           Cust_Name = cst.Customer_Name
                       }).Distinct().ToList();
                foreach (var tt in qry)
                {
                    var op = new CustTemplateEntity
                    {
                        Template_Id = tt.Template_Id,
                        Template_name = tt.Template_name,
                        Cust_Id = tt.Cust_Id,
                        Template_Type = tt.Template_Type,
                        Customer_Code = tt.Customer_Code,
                        Cust_Name = tt.Cust_Name
                    };
                    Custs.Add(op);
                }

            }
            result = Custs.ToList();
            return result;

            }

        public List<PrintedBarcodeDetailsEntity> GetPrintBarcodeByCategory(string Generated_Bar_Code)
        {
            List<PrintedBarcodeDetailsEntity> list = new List<PrintedBarcodeDetailsEntity>();

            list = (from x in DB.Printed_Barcode_Details
                    where x.Generated_Bar_Code == Generated_Bar_Code && x.is_Deleted == false
                    select new PrintedBarcodeDetailsEntity
                    {
                        Printed_Barcode_ID = x.Printed_Barcode_ID,
                        Generated_Bar_Code = x.Generated_Bar_Code,
                        DC_Code = x.DC_Code,
                        Customer_Id = x.Customer_Id,
                        Customer_Name = x.Customer_Name,
                        Template_Id = x.Template_Id,
                        Template_Name = x.Template_Name,
                        Product_Type = x.Product_Type,
                        Packed_On = x.Packed_On,
                        Best_Before = x.Best_Before,
                        Number_Of_Copies = x.Number_Of_Copies,
                        FSSAI = x.FSSAI,
                        SKU_Id = x.SKU_Id,
                        SKU_Name = x.SKU_Name,
                        SKU_Leaf_flag = x.SKU_Leaf_flag,
                        UOM = x.UOM,
                        Quantity = x.Quantity,
                        EAN = x.EAN,
                        Price = x.Price,
                        CreatedDate = x.CreatedDate,
                        CreateBy = x.CreateBy
                    }).ToList();
            return list;
        }



        //-------------------------------GET BY ID-----------------------------------
        public List<LabelFieldsEntity> GetLabelFieldById(int Field_Id)
        {
            List<LabelFieldsEntity> list = new List<LabelFieldsEntity>();

            list = (from x in DB.Label_Fields
                    where x.Field_Id == Field_Id && x.is_Deleted == false
                    select new LabelFieldsEntity
                    {
                        Field_Id = x.Field_Id,
                        Field_Name = x.Field_Name,
                        Data_Type = x.Data_Type,
                        Size = x.Size,
                        Remarks = x.Remarks,
                        CreatedDate = DateTime.Now,
                        CreatedBy = x.CreatedBy

                    }).ToList();
            return list;
        }
        public List<CustomerLabelTemplateEntity> GetLabelTemplateById(int Cust_Label_Template_Id)
        {
            var CLT = _unitOfWork.CustomerLabelTemplateRepository.GetByID(Cust_Label_Template_Id);
            if (CLT != null)
            {

                var skuModel = (from a in DB.Customer_Label_Template
                                where a.Cust_Label_Template_Id == Cust_Label_Template_Id
                                select new CustomerLabelTemplateEntity
                                {
                                    Cust_Label_Template_Id = a.Cust_Label_Template_Id,
                                    Template_Id = a.Template_Id,
                                    Template_name = a.Template_name,
                                    Field_Id = a.Field_Id,
                                    Field_Name = a.Field_Name,
                                    Remarks = a.Remarks,
                                    CreatedDate = a.CreatedDate,
                                    UpdatedDate = a.UpdatedDate,
                                    CreatedBy = a.CreatedBy,
                                    UpdatedBy = a.UpdatedBy
                                }
                                    ).ToList();
                return skuModel;
            }
            return null;
        }

        public List<CustTemplateEntity> GetCustomerTemplate(int? cust_Id)
        {
            var qu = (from a in DB.Customer_Label_Template
                      join map in DB.Customer_LabelTemplate_Mapping on cust_Id equals map.Customer_Id
                      where (a.Template_Id == map.Template_Id)
                      select new CustTemplateEntity
                      {
                          Template_Id = a.Template_Id,
                          Template_name = a.Template_name,                        
                      }).ToList();
            return qu;
        }
        public List<CustomerLabelTemplateMappingEntity> GetMappedCustomerByTId(string Template_Id)
        {
            var CLT = (from a in DB.Customer_LabelTemplate_Mapping
                       join temp in DB.Customer_Label_Template on Template_Id equals temp.Template_Id
                       join map in DB.Customers on a.Customer_Id equals map.Cust_Id
                       where a.Template_Id == Template_Id
                       select new CustomerLabelTemplateMappingEntity
                       {

                           Template_Id = a.Template_Id,
                           Template_Name = temp.Template_name,
                           Customer_Id = a.Customer_Id,
                           Customer_Code = map.Customer_Code,
                           Customer_Name = map.Customer_Name

                       }).Distinct().ToList();
            List<CustomerLabelTemplateMappingEntity> menuItems = new List<CustomerLabelTemplateMappingEntity>();

            foreach (CustomerLabelTemplateMappingEntity aa in CLT.Cast<CustomerLabelTemplateMappingEntity>().Take(1))
            {
                if (aa.Template_Id == Template_Id)
                {

                    var items = new CustomerLabelTemplateMappingEntity
                    {
                        Template_Id = aa.Template_Id,
                        Template_Name = aa.Template_Name,                       
                        CustIds = new List<CustIdEntity>
                        {
                        }
                    };
                    menuItems.Add(items);
                }
            }
            foreach (CustomerLabelTemplateMappingEntity menulist in menuItems)
            {
                foreach (CustomerLabelTemplateMappingEntity menulist1 in CLT)
                {
                    if (menulist.Template_Id == menulist1.Template_Id)
                    {
                        var menu = menuItems.Where(x => x.Template_Id == menulist.Template_Id);

                        if (menu.Count() != 0)
                        {
                            var Custdts = new CustIdEntity
                            {
                                Customer_Id = menulist1.Customer_Id,
                                Customer_Code = menulist1.Customer_Code,
                                Customer_Name = menulist1.Customer_Name
                            };

                            menulist.CustIds.Add(Custdts);

                        }
                    }
                }



            }
            var result = menuItems.ToList();
            return result;

        }
       
        public List<PrintedBarcodeDetailsEntity> GetPrintBarcodeById(int Printed_Barcode_ID)
        {
            List<PrintedBarcodeDetailsEntity> list = new List<PrintedBarcodeDetailsEntity>();

            list = (from x in DB.Printed_Barcode_Details
                    where x.Printed_Barcode_ID == Printed_Barcode_ID && x.is_Deleted == false
                    select new PrintedBarcodeDetailsEntity
                    {
                        Printed_Barcode_ID = x.Printed_Barcode_ID,
                        Generated_Bar_Code = x.Generated_Bar_Code,
                        DC_Code = x.DC_Code,
                        Customer_Id = x.Customer_Id,
                        Customer_Name = x.Customer_Name,
                        Template_Id = x.Template_Id,
                        Template_Name = x.Template_Name,
                        Product_Type = x.Product_Type,
                        Packed_On = x.Packed_On,
                        Best_Before = x.Best_Before,
                        Number_Of_Copies = x.Number_Of_Copies,
                        FSSAI = x.FSSAI,
                        SKU_Id = x.SKU_Id,
                        SKU_Name = x.SKU_Name,
                        SKU_Leaf_flag = x.SKU_Leaf_flag,
                        UOM = x.UOM,
                        Quantity = x.Quantity,
                        EAN = x.EAN,
                        Price = x.Price,
                        CreatedDate = x.CreatedDate,
                        CreateBy = x.CreateBy
                    }).ToList();
            return list;
        }

        public TemplateName CheckTempNameAvalibility(string TempName)
        {
            TemplateName avalilablility = new TemplateName();
            string Temp = TempName.ToLower();
            var TemplateName = (from s in DB.Customer_Label_Template
                                where s.Template_name.ToLower() == Temp && s.is_Deleted == false
                                select new TemplateName
                                {
                                    Available = s.Template_name
                                }
                                   ).ToList();
            if (TemplateName.Count != 0)
            {
                avalilablility.Available = "Not Available.Itz Exist";
                avalilablility.Status = false;
            }
            else
            {
                avalilablility.Available = "Itz Available";
                avalilablility.Status = true;
            }
            return avalilablility;
        }

        //----------------------------------------------CREATE------------------------------

        public bool CreateLabelField(LabelFieldsEntity LFEntity)
        {
            using (var scope = new TransactionScope())
            {
                var LF = new Label_Fields
                {
                    Field_Name = LFEntity.Field_Name,
                    Data_Type = LFEntity.Data_Type,
                    Size = LFEntity.Size,
                    Remarks = LFEntity.Remarks,
                    CreatedDate = DateTime.Now,
                    is_Deleted = false,
                    CreatedBy = LFEntity.CreatedBy
                };
                _unitOfWork.LabelRepository.Insert(LF);
                _unitOfWork.Save();
                scope.Complete();
            }
            return true;

        }

        public bool CreateLabelTemplate(CustomerLabelTemplateEntity CLTEntity)
        {
            string csiNumber, CSI_prefix;
            int? incNumber;

            using (var iscope = new TransactionScope())
            {
                Customer_Label_Auto_Num_Gen autoIncNumber = GetLabelAutoIncrement();
                incNumber = autoIncNumber.Template_Last_No;
                CSI_prefix = autoIncNumber.Template_Prefix;
                int? incrementedValue = incNumber + 1;
                var CSIincrement = DB.Customer_Label_Auto_Num_Gen.Where(x => x.Cust_Label_Auto_Gen_Id == 1).FirstOrDefault();
                CSIincrement.Template_Last_No = incrementedValue;
                _unitOfWork.CustomerLabel_NUMRepository.Update(CSIincrement);
                _unitOfWork.Save();
                csiNumber = CSI_prefix + String.Format("{0:0}", incNumber);
                iscope.Complete();
            }

            using (var scope = new TransactionScope())
            {
                var model = new Customer_Label_Template();
                foreach (FieldEntity fSub in CLTEntity.Fields)
                {
                    model.Template_Id = csiNumber;
                    model.Template_name = CLTEntity.Template_name;
                    model.Field_Id = fSub.Field_Id;
                    model.Field_Name = fSub.Field_Name;
                    model.Template_Type = CLTEntity.Template_Type;
                    model.Field_Value = fSub.Field_Value;
                    model.Remarks = fSub.Remarks;
                    model.CreatedDate = DateTime.Now;
                    model.is_Deleted = false;
                    model.CreatedBy = CLTEntity.CreatedBy;
                    _unitOfWork.CustomerLabelTemplateRepository.Insert(model);
                    _unitOfWork.Save();
                }

                scope.Complete();
            }
            return true;
        }

        public bool CreateLabelTemplateMapping(CustomerLabelTemplateMappingEntity LabelTemplateMappingEntity)
        {
            using (var scope = new TransactionScope())
            {
                var model = new Customer_LabelTemplate_Mapping();
                foreach (CustIdEntity fSub in LabelTemplateMappingEntity.CustIds)
                {
                    model.Template_Id = LabelTemplateMappingEntity.Template_Id;
                    model.Customer_Id = fSub.Customer_Id;
                    _unitOfWork.CustomerLabelTemplateMappingRepository.Insert(model);
                    _unitOfWork.Save();
                }
                scope.Complete();
            }
            return true;
        }

       public bool CreatePrintBarcode(PrintedBarcodeDetailsEntity pbEntity)
        {
            using (var scope = new TransactionScope())
            {
                var PB = new Printed_Barcode_Details
                {
                    Generated_Bar_Code = pbEntity.Generated_Bar_Code,
                    DC_Code = pbEntity.DC_Code,
                    Customer_Id = pbEntity.Customer_Id,
                    Customer_Name = pbEntity.Customer_Name,
                    Template_Id = pbEntity.Template_Id,
                    Template_Name = pbEntity.Template_Name,
                    Product_Type = pbEntity.Product_Type,
                    Packed_On = pbEntity.Packed_On,
                    Best_Before = pbEntity.Best_Before,
                    Number_Of_Copies = pbEntity.Number_Of_Copies,
                    FSSAI = pbEntity.FSSAI,
                    SKU_Id = pbEntity.SKU_Id,
                    SKU_Name = pbEntity.SKU_Name,
                    SKU_Leaf_flag = pbEntity.SKU_Leaf_flag,
                    UOM = pbEntity.UOM,
                    Quantity = pbEntity.Quantity,
                    EAN = pbEntity.EAN,
                    Price = pbEntity.Price,
                    is_Deleted = false,
                    CreatedDate = pbEntity.CreatedDate,
                    CreateBy = pbEntity.CreateBy
                };
                _unitOfWork.PrintBarcodeRepository.Insert(PB);
                _unitOfWork.Save();
                scope.Complete();
            }
            return true;

        }
        //----------------------------------------UPDATE----------------------------------------

        public bool UpdateLabelField(int Field_Id, LabelFieldsEntity LFEntity)
        {
            var success = false;
            if (LFEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var LF = _unitOfWork.LabelRepository.GetByID(Field_Id);
                    if (LF != null)
                    {
                        LF.Field_Name = LFEntity.Field_Name;
                        LF.Data_Type = LFEntity.Data_Type;
                        LF.Size = LFEntity.Size;
                        LF.Remarks = LFEntity.Remarks;
                        LF.UpdatedDate = DateTime.Now;
                        LF.UpdatedBy = LFEntity.UpdatedBy;
                        _unitOfWork.LabelRepository.Update(LF);
                        _unitOfWork.Save();
                    }
                    scope.Complete();
                    success = true;
                }
            }
            return success;
        }
        public bool UpdateLabelTemplateMapping(string Template_Id, CustomerLabelTemplateMappingEntity LTMEntity)
        {
            var success = false;
            if (LTMEntity != null)
            {
                var CLT = from ord in DB.Customer_LabelTemplate_Mapping
                          where ord.Template_Id == Template_Id
                          select ord;

                try
                {
                    foreach (var ord in CLT)
                    {
                        DB.Customer_LabelTemplate_Mapping.Remove(ord);

                    }
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                using (var scope = new TransactionScope())
                {
                    var model = new Customer_LabelTemplate_Mapping();
                    foreach (CustIdEntity fSub in LTMEntity.CustIds)
                    {
                        model.Template_Id = LTMEntity.Template_Id;

                        model.Customer_Id = fSub.Customer_Id;
                        _unitOfWork.CustomerLabelTemplateMappingRepository.Insert(model);
                        _unitOfWork.Save();
                    }

                    scope.Complete();
                }
                success = true;
            }

            return success;
        }
        public bool DeleteLabelTemplateMapping(string Template_Id)
        {
            var success = false;

            var CLT = from ord in DB.Customer_LabelTemplate_Mapping
                      where ord.Template_Id == Template_Id
                      select ord;
            foreach (var ord in CLT)
            {
                DB.Customer_LabelTemplate_Mapping.Remove(ord);
            }
            try
            {
                DB.SaveChanges();
            }
            catch (Exception)
            {
                success = false;
                return success;
            }

            return success;
        }
     
       public bool DeleteLabelTemplate(string Template_Id)
        {
            var success = false;

            var CLT = from ord in DB.Customer_Label_Template
                      where ord.Template_Id == Template_Id
                      select ord;
            foreach (var ord in CLT)
            {
                ord.is_Deleted = true;
            }
            try
            {
                DB.SaveChanges();
            }
            catch (Exception)
            {
                success = false;
                return success;
            }

            return success;
        }
        public bool UpdateLabelTemplate(string Template_Id, CustomerLabelTemplateEntity CLTEntity)
        {
            var success = false;
            if (CLTEntity != null)
            {
                var CLT = from ord in DB.Customer_Label_Template
                          where ord.Template_Id == Template_Id
                          select ord;
                foreach (var ord in CLT)
                {
                    ord.is_Deleted = true;
                }
                try
                {
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                string csiNumber, CSI_prefix;
                int? incNumber;

                using (var iscope = new TransactionScope())
                {
                    Customer_Label_Auto_Num_Gen autoIncNumber = GetLabelAutoIncrement();
                    incNumber = autoIncNumber.Template_Last_No;
                    CSI_prefix = autoIncNumber.Template_Prefix;
                    int? incrementedValue = incNumber + 1;
                    var CSIincrement = DB.Customer_Label_Auto_Num_Gen.Where(x => x.Cust_Label_Auto_Gen_Id == 1).FirstOrDefault();
                    CSIincrement.Template_Last_No = incrementedValue;
                    _unitOfWork.CustomerLabel_NUMRepository.Update(CSIincrement);
                    _unitOfWork.Save();
                    csiNumber = CSI_prefix + String.Format("{0:0}", incNumber);
                    iscope.Complete();
                }

                using (var scope = new TransactionScope())
                {
                    var model = new Customer_Label_Template();
                    foreach (FieldEntity fSub in CLTEntity.Fields)
                    {
                        model.Template_Id = csiNumber;
                        model.Template_name = CLTEntity.Template_name;
                        model.Template_Type = CLTEntity.Template_Type;
                        model.Field_Id = fSub.Field_Id;
                        model.Field_Name = fSub.Field_Name;
                        model.Field_Value = fSub.Field_Value;
                        model.Remarks = fSub.Remarks;
                        model.CreatedDate = DateTime.Now;
                        model.is_Deleted = false;
                        model.CreatedBy = CLTEntity.CreatedBy;
                        _unitOfWork.CustomerLabelTemplateRepository.Insert(model);
                        _unitOfWork.Save();
                    }

                    List<Customer_LabelTemplate_Mapping> map = DB.Customer_LabelTemplate_Mapping.Where(x => x.Template_Id == Template_Id).ToList();

                    foreach (Customer_LabelTemplate_Mapping inmap in map)
                    {
                        var getId = DB.Customer_LabelTemplate_Mapping.Where(x => x.Cust_Label_map_Id == inmap.Cust_Label_map_Id).FirstOrDefault();

                        getId.Template_Id = model.Template_Id;

                        DB.Entry(getId).State = EntityState.Modified;
                        DB.SaveChanges();

                    }

                    scope.Complete();
                }
                success = true;
            }

            return success;
        }

        public bool UpdatePrintBarcode(int Printed_Barcode_ID, PrintedBarcodeDetailsEntity pbEntity)
        {
            var success = false;
            if (pbEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var PB = _unitOfWork.PrintBarcodeRepository.GetByID(Printed_Barcode_ID);
                    if (PB != null)
                    {
                        PB.Generated_Bar_Code = pbEntity.Generated_Bar_Code;
                        PB.DC_Code = pbEntity.DC_Code;
                        PB.Customer_Id = pbEntity.Customer_Id;
                        PB.Customer_Name = pbEntity.Customer_Name;
                        PB.Template_Id = pbEntity.Template_Id;
                        PB.Template_Name = pbEntity.Template_Name;
                        PB.Product_Type = pbEntity.Product_Type;
                        PB.Packed_On = pbEntity.Packed_On;
                        PB.Best_Before = pbEntity.Best_Before;
                        PB.Number_Of_Copies = pbEntity.Number_Of_Copies;
                        PB.FSSAI = pbEntity.FSSAI;
                        PB.SKU_Id = pbEntity.SKU_Id;
                        PB.SKU_Name = pbEntity.SKU_Name;
                        PB.SKU_Leaf_flag = pbEntity.SKU_Leaf_flag;
                        PB.UOM = pbEntity.UOM;
                        PB.Quantity = pbEntity.Quantity;
                        PB.EAN = pbEntity.EAN;
                        PB.Price = pbEntity.Price;
                        PB.Updated_Date = DateTime.Now;
                        PB.Updated_By = pbEntity.Updated_By;
                        _unitOfWork.PrintBarcodeRepository.Update(PB);
                        _unitOfWork.Save();
                    }
                    scope.Complete();
                    success = true;

                }
            }
            return success;
        }

        //-----------------------------------------DELETE-------------------------------------

        public bool DeleteLabelField(int Field_Id)
        {
            var success = false;
            if (Field_Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var LF = _unitOfWork.LabelRepository.GetByID(Field_Id);
                    if (LF != null)
                    {
                        try
                        {
                            LF.is_Deleted = true;
                            _unitOfWork.LabelRepository.Update(LF);
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

       public bool DeletePrintBarcode(int Printed_Barcode_ID)
        {
            var success = false;
            if (Printed_Barcode_ID > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var PB = _unitOfWork.PrintBarcodeRepository.GetByID(Printed_Barcode_ID);
                    if (PB != null)
                    {
                        try
                        {
                            PB.is_Deleted = true;
                            _unitOfWork.PrintBarcodeRepository.Update(PB);
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