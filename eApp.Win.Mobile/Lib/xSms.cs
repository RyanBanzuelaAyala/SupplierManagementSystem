using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eApp.Win.Mobile.Lib
{
    public class xSms
    {
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
