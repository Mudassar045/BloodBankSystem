using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BloodBankSystem
{
    public class Validator
    {
        public static bool IsValidPassword(string password)
        {
            if (password.Length >= 6)
                return true;
            else
                return false;
        }
        public static bool IsMatchedPassword(string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsEmptyField(string data)
        {
            if (data.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsValidUserName(string username)
        {
            if (username.Contains(" ") || username.Contains(",") || username.Contains(">") || username.Contains("<") || username.Contains(";") || username.Contains(":") || username.Contains("'") || username.Contains("/"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsValidEmail(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
