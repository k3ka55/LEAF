//using System.Collections.Generic;
//using System.Linq;
//using System.Transactions;
//using AutoMapper;
//using BusinessEntities;
//using DataModel;
//using DataModel.UnitOfWork;
//using System;

//namespace BusinessServices
//{
//   public class RoleaccessService:IRoleaccessService
//    {
//         private readonly UnitOfWork _unitOfWork;
//         public RoleaccessService()
//           {
//                _unitOfWork = new UnitOfWork();
//            }


//         public BusinessEntities.RoleaccessEntity GetroleaccessById(int roleaccessId)
//          {
//              var roleaccess = _unitOfWork.MenuRepository.GetByID(roleaccessId);
//              if (roleaccess != null)
//                {
//                    Mapper.CreateMap<Role_Access, RoleaccessEntity>();
//                    var roleaccessModel = Mapper.Map<Role_Access, RoleaccessEntity>(roleaccess);
//                    return roleaccessModel;
//                }
//                return null;
//            }

//         public IEnumerable<BusinessEntities.RoleaccessEntity> GetAllRoleaccess()
//            {
//                var roleaccess = _unitOfWork.MenuRepository.GetAll().ToList();
//                if (roleaccess.Any())
//                {
//                    Mapper.CreateMap<Role_Access, RoleaccessEntity>();
//                    var roleaccessModel = Mapper.Map<List<Role_Access>, List<RoleaccessEntity>>(roleaccess);
//                    return roleaccessModel;
//                }
//                return null;
//            }

//         public int CreateRoleaccess(BusinessEntities.RoleaccessEntity roleaccessEntity)
//            {
//                using (var scope = new TransactionScope())
//                {
//                    var roleaccess = new Role_Access
//                    {
//                         Role_Id=roleaccessEntity.Role_Id,
//                         Menu_Id=roleaccessEntity.Menu_Id,
//                        CreateDate=DateTime.Now,
//                        CreateBy=roleaccessEntity.CreateBy,
//                                           };
//                    _unitOfWork.MenuRepository.Insert(roleaccess);
//                    _unitOfWork.Save();
//                    scope.Complete();
//                    return roleaccess.Role_Access_Id;
//                }
//       }

//         public bool UpdateRoleaccess(int roleaccessId, BusinessEntities.RoleaccessEntity roleaccessEntity)
//            {
//                var success = false;
//                if (roleaccessEntity != null)
//                {
//                    using (var scope = new TransactionScope())
//                    {
//                        var roleaccess = _unitOfWork.MenuRepository.GetByID(roleaccessId);
//                        if (roleaccess != null)
//                        {
//                             roleaccess.Role_Id=roleaccessEntity.Role_Id;
//                         roleaccess.Menu_Id=roleaccessEntity.Menu_Id;
//                        roleaccess.UpdateDate=DateTime.Now;
//                        roleaccess.UpdateBy = roleaccessEntity.UpdateBy;

                            
//                            _unitOfWork.MenuRepository.Update(roleaccess);
//                            _unitOfWork.Save();
//                            scope.Complete();
//                            success = true;
//                        }
//                    }
//                }
//                return success;
//            }


//         public bool DeleteRoleaccess(int roleaccessId)
//            {
//                var success = false;
//                if (roleaccessId > 0)
//                {
//                    using (var scope = new TransactionScope())
//                    {
//                        var roleaccess = _unitOfWork.MenuRepository.GetByID(roleaccessId);
//                        if (roleaccess != null)
//                        {

//                            _unitOfWork.MenuRepository.Delete(roleaccess);
//                            _unitOfWork.Save();
//                            scope.Complete();
//                            success = true;
//                        }
//                    }
//                }
//                return success;
//            }
//    }
//}
