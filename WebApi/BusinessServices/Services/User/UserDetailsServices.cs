using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System.Transactions;
using BusinessEntities.Entity;
using Newtonsoft.Json.Linq;

namespace BusinessServices
{
    public class UserDetailsServices : IUserDetailsServices
    {
        private readonly UnitOfWork _unitOfWork;
        LEAFDBEntities DB = new LEAFDBEntities();

        public UserDetailsServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<UserModel> GetAll()
        {

            var userList = (from x in DB.User_Details
                            where x.User_Login_Type=="LOCATION"
                            select
                            
                                //x).AsEnumerable().Select(x =>
                            new UserModel
                            {
                                User_id = x.User_id,
                                User_Name = x.User_Name,
                                User_Login_Type = x.User_Login_Type
                            }).ToList();
            return userList;
        }
        public IEnumerable<UserDetailsEntity> GetAllUsers(int? roleId, string Url)
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

            var userList = (from x in DB.User_Details
                            where x.User_Login_Type!="CUSTOMER"
                            select

                                //x).AsEnumerable().Select(x =>
                            new UserDetailsEntity
                            {
                                User_id = x.User_id,
                                User_Name = x.User_Name,
                                Password = x.Password,
                                First_Name = x.First_Name,
                                Last_Name = x.Last_Name,
                                Email_Id = x.Email_Id,
                                Phone = x.Phone,
                                Address1 = x.Address1,
                                Address2 = x.Address2,
                                User_Login_Type = x.User_Login_Type,
                                CreatedBy = x.CreatedBy,
                                CreatedDate = x.CreatedDate,
                                is_Create = iCrt,
                                is_Delete = isDel,
                                is_Edit = isEdt,
                                is_Approval = isApp,
                                is_View = isViw,
                                userDCLocation = (from y in DB.User_DC_Access
                                                  where y.User_Id == x.User_id
                                                  select new UserDCLocationAccessEntity
                                                  {
                                                      DC_id = y.DC_id,
                                                      DC_Code = y.DC_Code
                                                  }).ToList(),
                                userLocation = (from z in DB.User_Location_Access
                                                where z.User_Id == x.User_id
                                                select new UserLocationAccessEntity
                                                {
                                                    Location_id = z.Location_id,
                                                    Location_Code = z.Location_Code
                                                }).ToList(),
                                userroleAccess = (from r in DB.Role_User_Access
                                                  where r.User_Id == x.User_id
                                                  select new UserRoleAccessEntity
                                                  {
                                                      Role_Id = r.Role_Id
                                                  }).ToList(),
                                //Menu_Id = menuAccess.MenuID,
                                //Menu_Name = menuAccess.MenuName.FirstOrDefault(),
                                //is_Create = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Add"]),
                                //is_Delete = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Delete"]),
                                //is_Edit = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Edit"]),
                                //is_Approval = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["Approval"]),
                                //is_View = Convert.ToInt32(JObject.Parse(menuAccess.MenuPrevilages.First())["View"])
                            }).ToList();
            //foreach (var t in userList)
            //{
            //    t.is_Create = iCrt;
            //    t.is_Delete = isDel;
            //    t.is_Edit = isEdt;
            //    t.is_Approval = isApp;
            //    t.is_View = isViw;
            //}

            return userList;
        }

        public List<UserDetailsEntity> GetUserById(int userId)
        {
            var userList = (from x in DB.User_Details
                            where x.User_id == userId 
                            select new UserDetailsEntity
                            {
                                User_id = x.User_id,
                                User_Name = x.User_Name,
                                Password = x.Password,
                                First_Name = x.First_Name,
                                Last_Name = x.Last_Name,
                                Email_Id = x.Email_Id,
                                Phone = x.Phone,
                                Address1 = x.Address1,
                                Address2 = x.Address2,
                                User_Login_Type = x.User_Login_Type,
                                CreatedBy = x.CreatedBy,
                                CreatedDate = x.CreatedDate,
                                userDCLocation = (from y in DB.User_DC_Access
                                                  where y.User_Id == x.User_id
                                                  select new UserDCLocationAccessEntity
                                                  {
                                                      DC_id = y.DC_id,
                                                      DC_Code = y.DC_Code
                                                  }).ToList(),
                                userLocation = (from z in DB.User_Location_Access
                                                where z.User_Id == x.User_id
                                                select new UserLocationAccessEntity
                                                {
                                                    Location_id = z.Location_id,
                                                    Location_Code = z.Location_Code
                                                }).ToList(),
                                userroleAccess = (from r in DB.Role_User_Access
                                                  where r.User_Id == x.User_id
                                                  select new UserRoleAccessEntity
                                                  {
                                                      Role_Id = r.Role_Id
                                                  }).ToList()
                            }).ToList();

            return userList;
        }

        public bool ValidateUser(string userName)
        {
            bool result = false;
            var user = DB.User_Details.Where(t => t.User_Name == userName);

            if (user != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public int CreateUser(UserDetailsEntity userEntity)
        {
            using (var scope = new TransactionScope())
            {
                var user = new User_Details
                {
                    User_Name = userEntity.User_Name,
                    Password = userEntity.Password,
                    First_Name = userEntity.First_Name,
                    Last_Name = userEntity.Last_Name,
                    Email_Id = userEntity.Email_Id,
                    Phone = userEntity.Phone,
                    Address1 = userEntity.Address1,
                    Address2 = userEntity.Address2,
                    User_Login_Type = userEntity.User_Login_Type,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userEntity.CreatedBy,
                };
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();

                if (userEntity.User_Login_Type == "LOCATION")
                {
                    foreach (var location in userEntity.userLocation)
                    {
                        var locationAccess = new User_Location_Access
                        {
                            User_Id = user.User_id,
                            Location_id = location.Location_id,
                            Location_Code = location.Location_Code,
                            CreatedBy = userEntity.CreatedBy,
                            CreatedDate = DateTime.UtcNow
                        };

                        _unitOfWork.UserLocationMappingRepository.Insert(locationAccess);
                        _unitOfWork.Save();
                    }

                }
                else if (userEntity.User_Login_Type == "DC")
                {
                    foreach (var dclocation in userEntity.userDCLocation)
                    {
                        var dclocationAccess = new User_DC_Access
                        {
                            User_Id = user.User_id,
                            DC_id = dclocation.DC_id,
                            DC_Code = dclocation.DC_Code,
                            CreatedBy = userEntity.CreatedBy,
                            CreatedDate = DateTime.UtcNow,
                        };

                        _unitOfWork.UserDCMappingRepository.Insert(dclocationAccess);
                        _unitOfWork.Save();
                    }
                }

                if (userEntity.userroleAccess != null)
                {
                    foreach (var roleacess in userEntity.userroleAccess)
                    {
                        var roleusrAccess = new Role_User_Access
                        {
                            User_Id = user.User_id,
                            Role_Id = roleacess.Role_Id,
                            CreatedBy = user.CreatedBy,
                            CreatedDate = DateTime.UtcNow
                        };

                        _unitOfWork.RoleUserAccessRepository.Insert(roleusrAccess);
                        _unitOfWork.Save();
                    }
                }

                scope.Complete();
                return user.User_id;
            }
        }

        public bool UpdateUser(int userId, UserDetailsEntity userEntity)
        {
            var success = false;
            if (userEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        user.User_Name = userEntity.User_Name;
                        user.Password = userEntity.Password;
                        user.First_Name = userEntity.First_Name;
                        user.Last_Name = userEntity.Last_Name;
                        user.Email_Id = userEntity.Email_Id;
                        user.Phone = userEntity.Phone;
                        user.Address1 = userEntity.Address1;
                        user.Address2 = userEntity.Address2;
                        user.User_Login_Type = userEntity.User_Login_Type;
                        user.UpdatedDate = DateTime.Now;
                        user.UpdatedBy = userEntity.UpdatedBy;
                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();

                    }

                    using (var scope1 = new TransactionScope())
                    {
                        if (userEntity.User_Login_Type == "DC")
                        {
                            var dcaccess = DB.User_DC_Access.Where(x => x.User_Id == userId).ToList();

                            if (dcaccess != null)
                            {
                                foreach (var dcaccss in dcaccess)
                                {
                                    var deldcaccs = DB.User_DC_Access.Where(x => x.DC_Access_User_Id == dcaccss.DC_Access_User_Id).FirstOrDefault();
                                    DB.User_DC_Access.Remove(deldcaccs);
                                    DB.SaveChanges();
                                }
                            }

                        }
                        else if (userEntity.User_Login_Type == "LOCATION")
                        {
                            var dcaccess = DB.User_Location_Access.Where(x => x.User_Id == userId).ToList();

                            if (dcaccess != null)
                            {
                                foreach (var dcaccss in dcaccess)
                                {
                                    var deldcaccs = DB.User_Location_Access.Where(x => x.Location_Access_User_Id == dcaccss.Location_Access_User_Id).FirstOrDefault();
                                    DB.User_Location_Access.Remove(dcaccss);
                                    DB.SaveChanges();
                                }
                            }
                        }

                        var roleaccess = DB.Role_User_Access.Where(x => x.User_Id == user.User_id).ToList();
                        if (roleaccess != null)
                        {
                            foreach (var roleacss in roleaccess)
                            {
                                var roleacsss = DB.Role_User_Access.Where(x => x.User_Role_Access_Id == roleacss.User_Role_Access_Id).FirstOrDefault();
                                DB.Role_User_Access.Remove(roleacsss);
                                DB.SaveChanges();
                            }
                        }

                        scope1.Complete();
                    }

                    if (userEntity.User_Login_Type == "DC")
                    {
                        foreach (var dclocation in userEntity.userDCLocation)
                        {
                            var dclocationAccess = new User_DC_Access
                            {
                                User_Id = userId,
                                DC_id = dclocation.DC_id,
                                DC_Code = dclocation.DC_Code,
                                CreatedBy = userEntity.CreatedBy,
                                CreatedDate = DateTime.UtcNow,
                            };

                            _unitOfWork.UserDCMappingRepository.Insert(dclocationAccess);
                            _unitOfWork.Save();
                        }
                    }
                    else if (userEntity.User_Login_Type == "LOCATION")
                    {

                        foreach (var location in userEntity.userLocation)
                        {
                            var locationAccess = new User_Location_Access
                            {
                                User_Id = userId,
                                Location_id = location.Location_id,
                                Location_Code = location.Location_Code,
                                CreatedBy = userEntity.CreatedBy,
                                CreatedDate = DateTime.UtcNow
                            };

                            _unitOfWork.UserLocationMappingRepository.Insert(locationAccess);
                            _unitOfWork.Save();
                        }
                    }

                    if (userEntity.userroleAccess != null)
                    {
                        foreach (var roleacess in userEntity.userroleAccess)
                        {
                            var roleusrAccess = new Role_User_Access
                            {
                                User_Id = user.User_id,
                                Role_Id = roleacess.Role_Id,
                                CreatedBy = user.CreatedBy,
                                CreatedDate = DateTime.UtcNow
                            };

                            _unitOfWork.RoleUserAccessRepository.Insert(roleusrAccess);
                            _unitOfWork.Save();
                        }
                    }

                    scope.Complete();
                    success = true;
                }

            }
            return success;
        }

        public bool DeleteUser(int userId)
        {
            var success = false;
            if (userId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {

                        _unitOfWork.UserRepository.Delete(user);
                        _unitOfWork.Save();


                        using (var scope1 = new TransactionScope())
                        {
                            if (user.User_Login_Type == "DC")
                            {
                                var dcaccess = DB.User_DC_Access.Where(x => x.User_Id == userId).ToList();

                                if (dcaccess != null)
                                {
                                    foreach (var dcaccss in dcaccess)
                                    {
                                        var deldcaccs = DB.User_DC_Access.Where(x => x.DC_Access_User_Id == dcaccss.DC_Access_User_Id).FirstOrDefault();
                                        DB.User_DC_Access.Remove(deldcaccs);
                                        DB.SaveChanges();
                                    }
                                }

                            }
                            else if (user.User_Login_Type == "LOCATION")
                            {
                                var dcaccess = DB.User_Location_Access.Where(x => x.User_Id == userId).ToList();

                                if (dcaccess != null)
                                {
                                    foreach (var dcaccss in dcaccess)
                                    {
                                        var deldcaccs = DB.User_Location_Access.Where(x => x.Location_Access_User_Id == dcaccss.Location_Access_User_Id).FirstOrDefault();
                                        DB.User_Location_Access.Remove(dcaccss);
                                        DB.SaveChanges();
                                    }
                                }
                            }

                            var roleaccess = DB.Role_User_Access.Where(x => x.User_Id == user.User_id).ToList();
                            if (roleaccess != null)
                            {
                                foreach (var roleacss in roleaccess)
                                {
                                    var roleacsss = DB.Role_User_Access.Where(x => x.User_Role_Access_Id == roleacss.User_Role_Access_Id).FirstOrDefault();
                                    DB.Role_User_Access.Remove(roleacsss);
                                    DB.SaveChanges();
                                }
                            }

                            scope1.Complete();
                        }
                    }

                    scope.Complete();
                    success = true;
                }
            }
            return success;
        }
    }
}

