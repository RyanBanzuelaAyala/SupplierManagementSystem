using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.Email
{
    public static class Email
    {
        /// <summary>
        /// To send an email as notification or report of the system status
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="_title"></param>
        /// <returns></returns>
        public static OperationResult SendEmail(string _msg, string _title)
        {
            var op = new OperationResult();

            if(string.IsNullOrWhiteSpace(_msg))
            {
                op.Success = false;
                op.AddMessage("Body must not be empty");

                return op;
            }

            if(string.IsNullOrWhiteSpace(_title))
            {
                op.Success = false;
                op.AddMessage("Title must not be empty");

                return op;
            }

            try
            {
                MailMessage mailMessage = new MailMessage();
                              
                var to = ReCPeople();

                foreach(var m in to)
                {
                    mailMessage.To.Add(m);
                }

                mailMessage.Subject = _title + DateTime.Now.ToString("yyyyMMdd");
                mailMessage.Body = _msg;
                mailMessage.From = new MailAddress("info@danubeco.com", "DANUBE");

                SmtpClient smtpMail = new SmtpClient();

                smtpMail.Host = "Exchange.bindawood.com";
                mailMessage.IsBodyHtml = true;
                smtpMail.Send(mailMessage);

                op.Success = true;

            }
            catch (Exception e)
            {
                op.Success = false;
                op.AddMessage(e.Message);
            }

            return op;
        }

        private static List<string> ReCPeople()
        {
            var iList = new List<string>();

            using (StreamReader sr = File.OpenText(@"C:\wamp\www\App\em.d"))
            {
                string s = String.Empty;
                while((s = sr.ReadLine()) != null)
                {
                    iList.Add(s);
                }
            }

            return iList;
        }
    }
}
