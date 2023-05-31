using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHAKHOA.Utils
{
    public static class Helper
    {
        public static string RandomString9Number()
        {
            Random r = new Random();
            int a = r.Next(1, 999999999);
            string code = a.ToString().PadLeft(9, '0');
            return code;
        }
    }
}