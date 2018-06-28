using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace eApp.Web.Client.Resources.LibraryClass
{
    public class xSms
    {
        private string _sUsername = "Bindawood";
        private string _sPassword = "123";
        private string _sName = "Danube";

        public bool SendSMS(string _sMessage, string _mobile)
        {

            string _newMobile = "966" + _mobile.Substring(1, _mobile.Length - 1);

            StreamReader objReader;

            string sURL = "http://dreamsms.net/send.aspx?UserName=" + _sUsername + "&Password=" + _sPassword + "&senderName=" + _sName + "&message=" + _sMessage + "&MobileNo=" + _mobile + "&txtlang=1";

            WebRequest wrGETURL;

            wrGETURL = WebRequest.Create(sURL);

            try
            {
                Stream objStream;

                objStream = wrGETURL.GetResponse().GetResponseStream();

                objReader = new StreamReader(objStream);

                string _status = objReader.ReadLine();

                objReader.Close();
                
                return true;
            }
            catch
            {
                return false;
            }

        }

        public string GetCode(int numberOfCharsToGenerate)
        {
            var random = new Random();
            char[] chars = "1234567890".ToCharArray();

            var sb = new StringBuilder();
            for (int i = 0; i < numberOfCharsToGenerate; i++)
            {
                int num = random.Next(0, chars.Length);
                sb.Append(chars[num]);
            }
            return sb.ToString();
        }

    }
}