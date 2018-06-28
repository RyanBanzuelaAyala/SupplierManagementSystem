using Core.Common.Authorization;
using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.SMS
{
    public class SMS
    {
        private string _sUsername = Auth._sUsername;
        private string _sPassword = Auth._sPassword;
        private string _sName = Auth._sName;

        OperationResult op = new OperationResult();

        public OperationResult SendSMS(string _sMessage, string _mobile)
        {

            string _newMobile = "966" + _mobile.Substring(1, _mobile.Length - 1);

            StreamReader objReader;

            string sURL = "http://dreamsms.net/send.aspx?UserName=" + _sUsername + "&Password=" + _sPassword + "&senderName=" + _sName + "&message=" + _sMessage + "&MobileNo=" + _newMobile + "&txtlang=1";
                           
            WebRequest wrGETURL;

            wrGETURL = WebRequest.Create(sURL);

            try
            {
                Stream objStream;

                objStream = wrGETURL.GetResponse().GetResponseStream();

                objReader = new StreamReader(objStream);

                string _status = objReader.ReadLine();

                if(_status.Equals("0"))
                {
                    op.Success = true;
                    op.MessageList.Add(" : SMS Sent");                    
                }
                else
                {
                    op.Success = false;
                    op.MessageList.Add(" : SMS Failed");                                        
                }

                objReader.Close();

                return op;
                
            }
            catch
            {
                op.Success = false;
                op.MessageList.Add(" : SMS Failed");

                return op;
            }

        }
    }
}
