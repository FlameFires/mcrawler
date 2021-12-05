using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MaskCrawler.Utils
{
    public class ReflectUtil
    {
        public static TOut Convert<TOut>(object obj)
        {
            try
            {
                Type outType = typeof(TOut);
                TOut outObj = (TOut)Activator.CreateInstance(outType);

                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    var p = outType.GetProperties().Where(t=>t.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if(p!= null)
                    {
                        p.SetValue(outObj, property.GetValue(obj, null), null);
                    }
                }
                return outObj;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
