using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntlOps.Code
{
    public class FormatPhoneNum
    {
        public static string FormatPhoneNumber(string phoneNum, string phoneFormat)
        {
            if (phoneFormat == "")
            {
                phoneFormat = "(###) ###-####";
            }
            Regex regexObj = new Regex(@"[^\d]");
            phoneNum = regexObj.Replace(phoneNum, "");
            if (phoneNum.Length > 0)
            {
                phoneNum = Convert.ToInt64(phoneNum).ToString(phoneFormat);
            }
            return phoneNum;
        }
    }
}
