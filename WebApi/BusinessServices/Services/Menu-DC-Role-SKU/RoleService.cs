using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessServices
{
    public class RoleService : IRoleService
    {
        LEAFDBEntities DB = new LEAFDBEntities();

        private readonly UnitOfWork _unitOfWork;
        public RoleService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public RoleEntity GetroleById(int roleId)
        {
            var roleMaster = (from x in DB.Role_Master
                              where x.Role_Id == roleId
                              select new RoleEntity
                              {
                                  Role_Id = x.Role_Id,
                                  Role_Name = x.Role_Name,
                                  CreatedBy = x.CreatedBy,
                                  CreatedDate = x.CreatedDate,
                                  MenuAccess = (from y in DB.Role_Menu_Access
                                                join z in DB.Menu_Master on y.Menu_Id equals z.Menu_Id
                                                where y.Role_Id == x.Role_Id
                                                select new RoleMenuAccessEntity
                                                {
                                                    Menu_Role_Access_Id = y.Menu_Role_Access_Id,
                                                    Role_Id = y.Role_Id,
                                                    Menu_Id = y.Menu_Id,
                                                    Menu_Name = z.Menu_Name,
                                                    //is_View = y.is_View,
                                                    //is_edit = y.is_edit,
                                                    //is_Delete = y.is_Delete,
                                                    //is_Create = y.is_Create,
                                                    CreatedDate = y.CreatedDate,
                                                    CreatedBy = y.CreatedBy
                                                }).ToList(),
                              }).FirstOrDefault();
            return roleMaster;
        }

        public List<RoleEntity> GetAllRole()
        {
            var list = (from x in DB.Role_Master
                        select new RoleEntity
                        {
                            Role_Id = x.Role_Id,
                            Role_Name = x.Role_Name,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            MenuAccess = (from y in DB.Role_Menu_Access
                                          join z in DB.Menu_Master on y.Menu_Id equals z.Menu_Id
                                          where y.Role_Id == x.Role_Id
                                          select new RoleMenuAccessEntity
                                          {
                                              Menu_Role_Access_Id = y.Menu_Role_Access_Id,
                                              Role_Id = y.Role_Id,
                                              Menu_Id = y.Menu_Id,
                                              Menu_Name = z.Menu_Name,
                                              //is_View = y.is_View,
                                              //is_edit = y.is_edit,
                                              //is_Delete = y.is_Delete,
                                              //is_Create = y.is_Create,
                                              CreatedDate = y.CreatedDate,
                                              CreatedBy = y.CreatedBy
                                          }).ToList(),
                        }).ToList();
            return list;
        }

        public int CreateRole(RoleEntity roleEntity)
        {
            using (var scope = new TransactionScope())
            {
                var role = new Role_Master
                {
                    Role_Name = roleEntity.Role_Name,
                    CreatedDate = DateTime.Now,
                    CreatedBy = roleEntity.CreatedBy,
                };
                _unitOfWork.RoleRepository.Insert(role);
                _unitOfWork.Save();

                using (var scope1 = new TransactionScope())
                {
                    foreach (var list in roleEntity.MenuAccess)
                    {
                        var roleMenuAccess = new Role_Menu_Access
                        {
                            Role_Id = role.Role_Id,
                            Menu_Id = list.Menu_Id.Value,
                            //is_Create = list.is_Create,
                            //is_Delete = list.is_Delete,
                            //is_edit = list.is_edit,
                            //is_View = list.is_View,
                            CreatedBy = list.CreatedBy,
                            CreatedDate = list.CreatedDate,
                        };

                        _unitOfWork.RoleMenuAccessRepository.Insert(roleMenuAccess);
                        _unitOfWork.Save();
                    }

                    scope1.Complete();
                }

                scope.Complete();
                return role.Role_Id;
            }
        }
        
        public bool UpdateRole(int roleId, RoleEntity roleEntity)
        {
            var success = false;
            if (roleEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var role = _unitOfWork.RoleRepository.GetByID(roleId);
                    if (role != null)
                    {
                        role.Role_Name = roleEntity.Role_Name;
                        role.UpdatedDate = DateTime.Now;
                        role.UpdatedBy = roleEntity.UpdatedBy;

                        _unitOfWork.RoleRepository.Update(role);
                        _unitOfWork.Save();

                        using (var scope1 = new TransactionScope())
                        {
                            var list = DB.Role_Menu_Access.Where(x => x.Role_Id == role.Role_Id).ToList();
                            foreach(var aslist in list)
                            {
                                using (var scope2 = new TransactionScope())
                                {
                                    var access = _unitOfWork.RoleMenuAccessRepository.GetByID(aslist.Menu_Role_Access_Id);

                                    if(access != null)
                                    {
                                        _unitOfWork.RoleMenuAccessRepository.Delete(access);
                                        _unitOfWork.Save();
                                    }
                                    scope2.Complete();
                                }
                            }

                            foreach(var accessList in roleEntity.MenuAccess)
                            {
                                var roleMenuAccess = new Role_Menu_Access
                                {
                                    Role_Id = roleId,
                                    Menu_Id = accessList.Menu_Id.Value,
                                    //is_Create = accessList.is_Create,
                                    //is_Delete = accessList.is_Delete,
                                    //is_edit = accessList.is_edit,
                                    //is_View = accessList.is_View,
                                    CreatedBy = accessList.CreatedBy,
                                    CreatedDate = accessList.CreatedDate,
                                };

                                _unitOfWork.RoleMenuAccessRepository.Insert(roleMenuAccess);
                                _unitOfWork.Save();
                            }

                            scope1.Complete();
                        }

                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }


        public bool DeleteRole(int roleId)
        {
            var success = false;
            if (roleId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var role = _unitOfWork.RoleRepository.GetByID(roleId);
                    if (role != null)
                    {

                        _unitOfWork.RoleRepository.Delete(role);
                        _unitOfWork.Save();


                        using (var scope1 = new TransactionScope())
                        {
                            var list = DB.Role_Menu_Access.Where(x => x.Role_Id == roleId).ToList();
                            foreach (var aslist in list)
                            {
                                using (var scope2 = new TransactionScope())
                                {
                                    var access = _unitOfWork.RoleMenuAccessRepository.GetByID(aslist.Menu_Role_Access_Id);

                                    if (access != null)
                                    {
                                        _unitOfWork.RoleMenuAccessRepository.Delete(access);
                                        _unitOfWork.Save();
                                    }

                                    scope2.Complete();
                                }
                            }

                            scope1.Complete();
                        }

                        scope.Complete();
                        success = true;
                    }

                    
                }
            }
            return success;
        }
    }
}
