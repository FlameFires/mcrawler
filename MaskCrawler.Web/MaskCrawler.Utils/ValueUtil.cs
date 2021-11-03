using System;

namespace MaskCrawler.Utils
{
    public static class ValueUtil
    {
        public static bool GetBool(this int val)
        {
            return Convert.ToBoolean(val);
            // return val > 0 ? true : false;
        }

        public static bool GetBool(this int? val)
        {
            return Convert.ToBoolean(val);
        }
    }
}
