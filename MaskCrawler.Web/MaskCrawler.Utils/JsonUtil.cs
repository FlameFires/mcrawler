using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;

namespace MaskCrawler.Utils
{
    public static class JsonUtil
    {
        /// <summary>
        /// string to T 数据类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T Parse<T>(this string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }

        /// <summary>
        /// T to string 数据类型转换
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public static string Parse(this object jsonObj)
        {
            try
            {
                return JsonConvert.SerializeObject(jsonObj);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }


        /// <summary>
        /// newtonsoft.json 配置
        /// </summary>
        public readonly static JsonSerializerSettings JsonSetting = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy/MM/dd HH:mm:ss",
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
