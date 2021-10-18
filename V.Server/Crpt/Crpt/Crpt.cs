using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crpt
{
    public static class Crpt
    {
        public static string Crypt(string Text, string Password)
        {
            string res = string.Empty;
            int p = 0;

            foreach (char c in Text)
            {
                if (p > (Password.Length - 1))
                {
                    p = 0;
                }

                int dif = (int)c + (int)Password[p];
                if (dif > 0xff)
                {
                    res = res + string.Format("{0:X2}", dif - 0xff);
                }
                else
                {
                    res = res + string.Format("{0:X2}", dif);
                }
                p++;
            }

            return res;
        }

        public static string Decrypt(string Text, string Password)
        {
            string res = string.Empty;
            int p = 0;

            if ((Text.Length % 2) == 0)
            {
                for (int i = 0; i < (Text.Length / 2); i++)
                {
                    if (p > (Password.Length - 1))
                    {
                        p = 0;
                    }

                    string part = string.Format("{0}{1}", Text[2*i], Text[2*i + 1]);
                    int dif = Int32.Parse(part, System.Globalization.NumberStyles.AllowHexSpecifier);

                    if (dif < (int)Password[p])
                    {
                        dif += 0xff;
                    }

                    res = res + (char)(dif - (int)Password[p]);
                    p++;
                }
            }

            return res;
        }
    }
}
