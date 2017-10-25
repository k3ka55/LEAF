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
   
//     public class SubmenuService:ISubmenuService
//        {
//           private readonly UnitOfWork _unitOfWork;
//            public SubmenuService()
//           {
//                _unitOfWork = new UnitOfWork();
//            }


//           public BusinessEntities.SubmenuEntity GetsubmenuById(int submenuId)
//          {
//               var submenu = _unitOfWork.SubMenuRepository.GetByID(submenuId);
//               if (submenu != null)
//                {
//                    Mapper.CreateMap<Sub_Menu, SubmenuEntity>();
//                    var submenuModel = Mapper.Map<Sub_Menu, SubmenuEntity>(submenu);
//                    return submenuModel;
//                }
//                return null;
//            }

//            public IEnumerable<BusinessEntities.SubmenuEntity> GetAllSubmenu()
//            {
//                var submenu = _unitOfWork.SubMenuRepository.GetAll().ToList();
//                if (submenu.Any())
//                {
//                    Mapper.CreateMap<Sub_Menu, SubmenuEntity>();
//                    var submenuModel = Mapper.Map<List<Sub_Menu>, List<SubmenuEntity>>(submenu);
//                    return submenuModel;
//                }
//                return null;
//            }

//            public int CreateSubmenu(BusinessEntities.SubmenuEntity submenuEntity)
//            {
//                using (var scope = new TransactionScope())
//                {
//                    var submenu = new Sub_Menu
//                    {
//                         Menu_Id=submenuEntity.Menu_Id,
//                         Sub_Name=submenuEntity.Sub_Name,
//                         Url=submenuEntity.Url,
//                         CreateDate=DateTime.Now,
//                         CreateBy=submenuEntity.CreateBy
//                                     };
//                    _unitOfWork.SubMenuRepository.Insert(submenu);
//                    _unitOfWork.Save();
//                    scope.Complete();
//                    return submenu.Sub_Id;
//                }
//       }

//            public bool UpdateSubmenu(int submenuId, BusinessEntities.SubmenuEntity submenuEntity)
//            {
//                var success = false;
//                if (submenuEntity != null)
//                {
//                    using (var scope = new TransactionScope())
//                    {
//                        var submenu = _unitOfWork.SubMenuRepository.GetByID(submenuId);
//                        if (submenu != null)
//                        {
//                             submenu.Menu_Id=submenuEntity.Menu_Id;
//                         submenu.Sub_Name=submenuEntity.Sub_Name;
//                         submenu.Url=submenuEntity.Url;
//                         submenu.UpdateDate=DateTime.Now;
//                         submenu.UpdateBy=submenuEntity.UpdateBy;
                            
                            
//                            _unitOfWork.SubMenuRepository.Update(submenu);
//                            _unitOfWork.Save();
//                            scope.Complete();
//                            success = true;
//                        }
//                    }
//                }
//                return success;
//            }


//            public bool DeleteSubmenu(int submenuId)
//            {
//                var success = false;
//                if (submenuId > 0)
//                {
//                    using (var scope = new TransactionScope())
//                    {
//                        var submenu = _unitOfWork.SubMenuRepository.GetByID(submenuId);
//                        if (submenu != null)
//                        {

//                            _unitOfWork.SubMenuRepository.Delete(submenu);
//                            _unitOfWork.Save();
//                            scope.Complete();
//                            success = true;
//                        }
//                    }
//                }
//                return success;
//            }
//        }
//}
