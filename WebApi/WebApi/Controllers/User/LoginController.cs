using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private readonly ILoginServices _loginServices;

        #region Public Constructor


        public LoginController()
        {
            _loginServices = new LoginServices();
        }
        [HttpPost]
        public HttpResponseMessage GetLoginUserLocations([FromBody]UserDetailsEntity userEntity)
        {
            var userlog = _loginServices.GetLoginUserLocations(userEntity);
            return Request.CreateResponse(HttpStatusCode.OK, userlog);
        }
        //--------------------------------login(POST)---------------------------------------
        [HttpPost]
        public HttpResponseMessage login([FromBody]UserDetailsEntity userEntity)
        {
            var userlog = _loginServices.login(userEntity);

            return Request.CreateResponse(HttpStatusCode.OK, userlog);
        }
              
        //--------------------------------changePassword(POST)---------------------------------------
        [HttpPost]
        public HttpResponseMessage changePassword([FromBody]UserDetailsEntity userEntity)
        {
            var changePassword = _loginServices.changePassword(userEntity);
            string result = "";
            if (changePassword)
            {
                result = string.Format("Password changed successfully.");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                result = string.Format("Password changed Unsuccessfully.");
                return Request.CreateResponse(HttpStatusCode.NotModified, result);
            }
        }
        //--------------------------------forgetPassword(GET)---------------------------------------
        [HttpGet]
        public HttpResponseMessage forgetPassword(string userName)
        {
            string tableName = "";
            string senderMail = "";
            string mailPassword = "";
            string smtpserverName = "";
            string portNumber = "";
            string emailList = "";
            //
            XmlTextReader reader = null;
            string filename = "D:\\WEB API\\Layers\\WebApi\\BusinessServices\\emaildetails1.xml";

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
            SmtpClient SmtpServer = new SmtpClient();
            mail.From = new MailAddress(senderMail);

            mail.Subject = "Forget Password";
            mail.Body = "Change requested was from this user. user name is " + userName + "";
            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment(saveLocation);
            //mail.Attachments.Add(attachment);
            SmtpServer.Port = int.Parse(portNumber);

            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(senderMail, mailPassword);
            SmtpServer.Host = smtpserverName;
            mail.To.Add(emailList);
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
            //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //        System.Security.Cryptography.X509Certificates.X509Chain chain,
            //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            SmtpServer.Send(mail);
            //return true;
            //bool response = _loginServices.forgetPassword(userName);
            return Request.CreateResponse(HttpStatusCode.OK, "success");
        }

        #endregion
    }
}
