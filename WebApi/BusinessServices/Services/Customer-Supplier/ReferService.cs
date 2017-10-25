//using AutoMapper;
//using BusinessEntities;
//using DataModel;
//using DataModel.UnitOfWork;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace BusinessServices
//{
//    public class CustomerServices : ICustomerServices
//    {
//        private readonly UnitOfWork _unitOfWork;
//        public CustomerServices()
//        {
//            _unitOfWork = new UnitOfWork();
//        }

//----------------------------------GET BY ID------------------------------

//        public BusinessEntities.CustomerEntity GetcustomerById(int customerId)
//        {
//            var customer = _unitOfWork.CustomerRepository.GetByID(customerId);
//            if (customer != null)
//            {
//                Mapper.CreateMap<Customer, CustomerEntity>();
//                var customerModel = Mapper.Map<Customer, CustomerEntity>(customer);
//                return customerModel;
//            }
//            return null;
//        }

//-----------------------------------GET ALL----------------------------------

//        public IEnumerable<BusinessEntities.CustomerEntity> GetAllCustomer()
//        {
//            var customer = _unitOfWork.CustomerRepository.GetAll().ToList();
//            if (customer.Any())
//            {
//                Mapper.CreateMap<Customer, CustomerEntity>();
//                var customerModel = Mapper.Map<List<Customer>, List<CustomerEntity>>(customer);
//                return customerModel;
//            }
//            return null;
//        }

//----------------------------------CREATE CUSTOMER--------------------------

//        public bool CreateCustomer(BusinessEntities.CustomerEntity customerEntity)
//        {
//            using (var scope = new TransactionScope())
//            {
//                var customer = new Customer
//                {
//                    Customer_Code = customerEntity.Customer_Code,
//                    Customer_Name = customerEntity.Customer_Name,
//                    Address1 = customerEntity.Address1,
//                    Address2 = customerEntity.Address2,
//                    City = customerEntity.City,
//                    State = customerEntity.State,
//                    District = customerEntity.District,
//                    Pincode = customerEntity.Pincode,
//                    Primary_Contact_Name = customerEntity.Primary_Contact_Name,
//                    Contact_Number = customerEntity.Contact_Number,
//                    Location = customerEntity.Location,
//                    CreatedDate = DateTime.Now,
//                    CreatedBy = customerEntity.CreatedBy,
//                };
//                try
//                {
//                    _unitOfWork.CustomerRepository.Insert(customer);
//                    _unitOfWork.Save();
//                    scope.Complete();
//                }
//                catch (Exception e)
//                {
//                    return false;
//                }

//                return true;
//            }

//        }

//---------------------------------UPDATE CUSTOMER---------------------------------

//        public bool UpdateCustomer(int customerId, BusinessEntities.CustomerEntity customerEntity)
//        {
//            var success = false;
//            if (customerEntity != null)
//            {
//                using (var scope = new TransactionScope())
//                {
//                    var customer = _unitOfWork.CustomerRepository.GetByID(customerId);
//                    if (customer != null)
//                    {
//                        customer.Customer_Code = customerEntity.Customer_Code;
//                        customer.Customer_Name = customerEntity.Customer_Name;
//                        customer.Address1 = customerEntity.Address1;
//                        customer.Address2 = customerEntity.Address2;
//                        customer.City = customerEntity.City;
//                        customer.State = customerEntity.State;
//                        customer.District = customerEntity.District;
//                        customer.Pincode = customerEntity.Pincode;
//                        customer.Primary_Contact_Name = customerEntity.Primary_Contact_Name;
//                        customer.Contact_Number = customerEntity.Contact_Number;
//                        customer.Location = customerEntity.Location;
//                        customer.UpdatedDate = DateTime.Now;
//                        customer.UpdatedBy = customerEntity.UpdatedBy;
//                        try
//                        {
//                            _unitOfWork.CustomerRepository.Update(customer);
//                            _unitOfWork.Save();
//                            scope.Complete();
//                            success = true;
//                        }
//                        catch (Exception e)
//                        {
//                            success = false;
//                            return success;
//                        }

//                    }
//                }
//            }
//            return success;
//        }

//-----------------------------------------DELETE CUSTOMER-------------------------

//        public bool DeleteCustomer(int customerId)
//        {
//            var success = false;
//            if (customerId > 0)
//            {
//                using (var scope = new TransactionScope())
//                {
//                    var customer = _unitOfWork.CustomerRepository.GetByID(customerId);
//                    if (customer != null)
//                    {
//                        try
//                        {
//                            _unitOfWork.CustomerRepository.Delete(customer);
//                            _unitOfWork.Save();
//                            scope.Complete();
//                            success = true;
//                        }
//                        catch (Exception)
//                        {
//                            success = false;
//                            return success;
//                        }

//                    }
//                }
//            }
//            return success;
//        }
//    }
//}



