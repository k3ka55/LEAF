using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Web;
using BusinessEntities.Entity;
namespace BusinessServices
{
    public class LoginServices : ILoginServices
    {
        private readonly UnitOfWork _unitOfWork;

        public LoginServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        LEAFDBEntities DB = new LEAFDBEntities();

        public List<MobileDCLocations> GetLoginUserLocations(UserDetailsEntity user)
        {
            List<MobileDCLocations> loginReps = new List<MobileDCLocations>();
            //string DC_Name_Check = "null";
            Func<User_Details, Boolean> where = x => x.User_id == user.User_id;
            var model = _unitOfWork.UserRepository.Get(where);

            if (model != null)
            {
                if (model.User_Login_Type == "DC")
                {
                    loginReps = (from x in DB.DC_Master
                                          join y in DB.User_DC_Access on x.Dc_Id equals y.DC_id
                                          where y.User_Id == model.User_id
                                          select new MobileDCLocations
                                          {
                                              Dc_Id = x.Dc_Id,
                                              DC_Code = x.DC_Code,
                                              Dc_Name = x.Dc_Name,
                                              UserType = "DC",
                                              Region_Id = x.Region_Id,
                                              Region_Code = x.Region_Code,
                                              Region_Name = x.Region
                                          }).ToList();
                }
                else if (model.User_Login_Type == "LOCATION")
                {
                    var CLT = (from ord in DB.Sales_Route_Mapping
                               where ord.Sales_Person_Id == user.User_id
                               select ord).FirstOrDefault();
                    if (CLT != null)
                    {
                        loginReps = new List<MobileDCLocations>
                        {
                            new MobileDCLocations
                        {
                            Dc_Id=CLT.Orgin_Location_Id.Value,
                            DC_Code=CLT.Orgin_Location_Code,
                            Dc_Name=CLT.Orgin_Location_Name
                    }
                };
                    }
                }
            }

            return loginReps;
        }
        public loginReposnse login(UserDetailsEntity user)
        {
            //int? roleId;
            loginReposnse loginReps = new loginReposnse();
            int uId = 0; string DC_Name_Check = "null";
            Func<User_Details, Boolean> where = x => x.User_Name == user.User_Name && x.Password == user.Password;
            var model = _unitOfWork.UserRepository.Get(where);
            if (model != null)
            {
                loginReps.userId = model.User_id;
                uId = model.User_id;
                loginReps.message = "Login SuccessFully.";
                loginReps.status = 1;
                loginReps.userName = model.User_Name;
                loginReps.roles = (from x in DB.Role_Master
                                   join y in DB.Role_User_Access on x.Role_Id equals y.Role_Id
                                   where y.User_Id == model.User_id
                                   select new RolesEntity
                                   {
                                       roleid = x.Role_Id,
                                       rolename = x.Role_Name
                                   }).ToList();

                if (model.User_Login_Type == "DC")
                {
                    loginReps.location = (from x in DB.DC_Master
                                          join y in DB.User_DC_Access on x.Dc_Id equals y.DC_id
                                          where y.User_Id == model.User_id
                                          select new DCLocations
                                          {
                                              Dc_Id = x.Dc_Id,
                                              DC_Code = x.DC_Code,
                                              Dc_Name = x.Dc_Name,
                                              Address1 = x.Address1,
                                              Address2 = x.Address2,
                                              State = x.State,
                                              Offline_Flag = x.Offline_Flag,
                                              Country = x.County,
                                              City = x.City,
                                              Pincode = x.PinCode,
                                              UserType = "DC",
                                              Region_Id = x.Region_Id,
                                              Region_Code = x.Region_Code,
                                              Region_Name = x.Region
                                          }).ToList();


                    loginReps.dc = new List<DCLocationsr>();
                    foreach (var li in loginReps.location)
                    {
                        if (loginReps.dc != null)
                        {
                            var dcli = loginReps.dc.Where(x => x.Region_Id == li.Region_Id).FirstOrDefault();
                            if (dcli != null)
                            {
                                var dcdet = new reg
                                {
                                    Dc_Id = li.Dc_Id,
                                    DC_Code = li.DC_Code,
                                    Dc_Name = li.Dc_Name,
                                    Address1 = li.Address1,
                                    Address2 = li.Address2,
                                    State = li.State,
                                    Offline_Flag = li.Offline_Flag,
                                    Country = li.Country,
                                    City = li.City,
                                    Pincode = li.Pincode,
                                    UserType = li.UserType
                                };
                                dcli.dcDetails.Add(dcdet);
                            }
                            else
                            {
                                var dd = new DCLocationsr
                                {
                                    Region_Id = li.Region_Id,
                                    Region_Name = li.Region_Name,
                                    Region_Code = li.Region_Code,
                                    dcDetails = new List<reg>
                                  {
                                    new reg
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        Address1 = li.Address1,
                                        Offline_Flag=li.Offline_Flag,
                                        Address2 = li.Address2,
                                        State = li.State,
                                        Country = li.Country,
                                        City = li.City,
                                        Pincode = li.Pincode,
                                        UserType = li.UserType
                                    }
                                }
                                };
                                loginReps.dc.Add(dd);
                            }
                        }
                        else
                        {
                            var dd = new DCLocationsr
                            {
                                Region_Id = li.Region_Id,
                                Region_Name = li.Region_Name,
                                Region_Code = li.Region_Code,
                                dcDetails = new List<reg>
                                {
                                    //
                                    new reg
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        Offline_Flag=li.Offline_Flag,
                                        Address1 = li.Address1,
                                        Address2 = li.Address2,
                                        State = li.State,
                                        Country = li.Country,
                                        City = li.City,
                                        Pincode = li.Pincode,
                                        UserType = li.UserType
                                    }
                                }
                            };
                            loginReps.dc.Add(dd);
                        }
                    }

                    if (loginReps.dc.Count == 1)
                    {
                        foreach (var li in loginReps.dc)
                        {
                            foreach (var lli in li.dcDetails)
                            {
                                DC_Name_Check = lli.Dc_Name;
                            }
                        }
                        int dcount = DB.DC_Master.Where(d => d.Dc_Name == DC_Name_Check && d.Offline_Flag == true).Count();
                        if (dcount > 0)
                        {
                            loginReps.status = 2;
                        }
                    }

                }
                else if (model.User_Login_Type == "LOCATION")
                {
                    loginReps.location = (from x in DB.Location_Master
                                          join y in DB.User_Location_Access on x.Location_Id equals y.Location_id
                                          where y.User_Id == model.User_id
                                          select new DCLocations
                                          {
                                              Dc_Id = x.Location_Id,
                                              DC_Code = x.Location_Code,
                                              Dc_Name = x.Location_Name,
                                              Address1 = x.Address1,
                                              Address2 = x.Address2,
                                              State = x.State,
                                              Country = x.County,
                                              City = x.City,
                                              Pincode = x.PinCode,
                                              UserType = "LOCATION",
                                              Region_Id = x.Region_Id,
                                              Region_Code = x.Region_Code,
                                              Region_Name = x.Region
                                          }).ToList();

                    loginReps.dc = new List<DCLocationsr>();
                    foreach (var li in loginReps.location)
                    {
                        if (loginReps.dc != null)
                        {
                            var dcli = loginReps.dc.Where(x => x.Region_Id == li.Region_Id).FirstOrDefault();

                            if (dcli != null)
                            {
                                var dcdet = new reg
                                {
                                    Dc_Id = li.Dc_Id,
                                    DC_Code = li.DC_Code,
                                    Dc_Name = li.Dc_Name,
                                    Address1 = li.Address1,
                                    Address2 = li.Address2,
                                    State = li.State,
                                    Country = li.Country,
                                    City = li.City,
                                    Pincode = li.Pincode,
                                    UserType = li.UserType
                                };
                                dcli.dcDetails.Add(dcdet);
                            }
                            else
                            {
                                var dd = new DCLocationsr
                                {
                                    Region_Id = li.Region_Id,
                                    Region_Name = li.Region_Name,
                                    Region_Code = li.Region_Code,
                                    dcDetails = new List<reg>
                                {
                                    new reg
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        Address1 = li.Address1,
                                        Address2 = li.Address2,
                                        State = li.State,
                                        Country = li.Country,
                                        City = li.City,
                                        Pincode = li.Pincode,
                                        UserType = li.UserType
                                    }
                                }
                                };
                                loginReps.dc.Add(dd);
                            }
                        }
                        else
                        {
                            var dd = new DCLocationsr
                            {
                                Region_Id = li.Region_Id,
                                Region_Name = li.Region_Name,
                                Region_Code = li.Region_Code,
                                dcDetails = new List<reg>
                                {
                                    new reg
                                    {
                                        Dc_Id = li.Dc_Id,
                                        DC_Code = li.DC_Code,
                                        Dc_Name = li.Dc_Name,
                                        Address1 = li.Address1,
                                        Address2 = li.Address2,
                                        State = li.State,
                                        Country = li.Country,
                                        City = li.City,
                                        Pincode = li.Pincode,
                                        UserType = li.UserType
                                    }
                                }
                            };
                            loginReps.dc.Add(dd);
                        }
                    }
                }


                //loginReps.token = GenerateToken(model.User_Name);
            }
            else
            {
                loginReps.message = "Login Faild.";
                loginReps.status = 0;
            }

            return loginReps;
        }


        //public string GenerateToken(string userName)
        //{
        //    string token = Guid.NewGuid().ToString();

        //    HttpRuntime.Cache.Add(
        //    token,
        //    userName,
        //    null,
        //    System.Web.Caching.Cache.NoAbsoluteExpiration,
        //    TimeSpan.FromMinutes(1),
        //    System.Web.Caching.CacheItemPriority.NotRemovable,
        //    null
        //    );

        //    return token;
        //}

        public Role_Master getUserRole(int? roleId)
        {
            string query = "select * from User_Role where Role_Id = '" + roleId + "'";
            List<Role_Master> userRole = new List<Role_Master>();
            Func<Role_Master, Boolean> where = x => x.Role_Id == roleId;
            var role = _unitOfWork.RoleRepository.Get(where);
            return role;
        }

        public bool changePassword(UserDetailsEntity userEntity)
        {
            var result = false;
            if (userEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userEntity.User_id);
                    if (user != null)
                    {
                        user.Password = userEntity.Password;
                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool forgetPassword(string userName)
        {
            //D:\WEB API\Layers\WebApi\BusinessServices\mail.xml
            string tableName = "";
            string senderMail = "";
            string mailPassword = "";
            string smtpserverName = "";
            string portNumber = "";
            string emailList = "";

            XmlTextReader reader = null;
            string filename = "D:\\WEB API\\Layers\\WebApi\\BusinessServices\\emaildetails.xml";

            reader = new XmlTextReader(filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        tableName = reader.Name;
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        if (tableName == "mail")
                            senderMail = reader.Value;
                        else if (tableName == "pw")
                            mailPassword = reader.Value;
                        else if (tableName == "smtp")
                            smtpserverName = reader.Value;
                        else if (tableName == "port")
                            portNumber = reader.Value;
                        else if (tableName == "emlist")
                            emailList = reader.Value;
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.                        
                        break;
                }
            }

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtpserverName);
            mail.From = new MailAddress(senderMail);
            mail.To.Add(emailList);
            mail.Subject = "Forget Password";
            mail.Body = "Change requested was from this user. user name is " + userName + "";
            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment(saveLocation);
            //mail.Attachments.Add(attachment);
            SmtpServer.Port = int.Parse(portNumber);
            SmtpServer.Credentials = new System.Net.NetworkCredential(senderMail, mailPassword);
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
            //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //        System.Security.Cryptography.X509Certificates.X509Chain chain,
            //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            SmtpServer.Send(mail);
            return true;
        }
    }
}
