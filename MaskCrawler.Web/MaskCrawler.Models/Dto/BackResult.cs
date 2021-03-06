using MaskCrawler.Utils;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MaskCrawler.Models.Dto
{
    public class BackResult
    {
        public BackResult() : this(null)
        {
        }

        public BackResult(string msg, bool ok = true, ResultCode code = ResultCode.NoProbelm, object data = null)
        {
            Msg = msg;
            Ok = ok;
            Code = code;
            Data = data;
        }

        public string Msg { get; set; }
        public bool Ok { get; set; }
        public ResultCode Code { get; set; }
        public object Data { get; set; }

        private static string _defaultContentType = "application/json; charset=utf-8";
        private static object _jsonSetting = JsonUtil.JsonSetting;

        public static IActionResult Successed(string msg = null, object data = null)
        {
            return GetJsonResult(msg, true, ResultCode.NoProbelm, data);
        }

        public static IActionResult Failed(string msg = null, object data = null)
        {
            return GetJsonResult(msg, false, ResultCode.OperationInvalid, data);
        }

        public static IActionResult Error(string msg, object data = null)
        {
            return GetJsonResult(msg, false, ResultCode.InternalError, data);
        }

        public static IActionResult Judge(bool neutral, string sucMsg = null, string failMsg = null, object data = null)
        {
            return neutral ? Successed(sucMsg, data) : Failed(failMsg, data);
        }

        public static IActionResult Judge<TEntity>(IEnumerable<TEntity> data, string sucMsg = null, string failMsg = null)
        {
            return data != null ? Successed(sucMsg, data) : Failed(failMsg, data);
        }

        public static IActionResult Judge(object data, string sucMsg = null, string failMsg = null)
        {
            return data != null ? Successed(sucMsg, data) : Failed(failMsg, data);
        }

        public static IActionResult Judge(int num, string sucMsg = null, string failMsg = null, object data = null)
        {
            object tempData = null;
            ResultCode tempCode = ResultCode.NoProbelm;
            string tempMsg = sucMsg;
            if (num.GetBool())
            {
                tempData = data;
            }
            else
            {
                tempCode = ResultCode.NumValueLessThanOne;
                tempMsg = failMsg ?? SqlMsgCons.RESULT_VALUE_INVALID;
            }

            return GetJsonResult(tempMsg, true, tempCode, tempData);
        }
        public static IActionResult Judge<TOutDto>(int num, string sucMsg = null, object data = null)
        {
            TOutDto outObj = default;
            ResultCode tempCode = ResultCode.NoProbelm;
            string tempMsg = sucMsg;
            if (num.GetBool())
            {
                Type outType = typeof(TOutDto);
                var outPros = outType.GetProperties();
                outObj = (TOutDto)Activator.CreateInstance(outType);

                foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
                {
                    var name = propertyInfo.Name;
                    PropertyInfo p = outPros.Where(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    object val = propertyInfo.GetValue(data);
                    p.SetValue(outObj,val);

                    // TODO 无法转换对应类型
                    //if (p.GetValue(outObj) is ValueType)
                    //{
                        
                    //    ValueType valueType = (ValueType)val;
                    //    p.SetValue(outObj, valueType);
                    //}
                    //else if (p.GetType() == typeof(String))
                    //{
                    //    p.SetValue(outObj, val.ToString());
                    //}
                }

                tempMsg = string.Empty;
            }
            else
            {
                tempCode = ResultCode.NumValueLessThanOne;
                tempMsg = SqlMsgCons.RESULT_VALUE_INVALID;
            }

            return GetJsonResult(tempMsg, true, tempCode, outObj);
        }

        public static IActionResult GetJsonResult(string msg, bool ok, ResultCode code, object data = null)
        {
            var jr = new JsonResult(new BackResult(msg, ok, code, data), (JsonSerializerSettings)_jsonSetting);
            jr.ContentType = _defaultContentType;
            return jr;
        }
    }

    public enum ResultCode
    {
        [Description("没问题")]
        NoProbelm = 0,
        [Description("操作无效")]
        OperationInvalid,
        [Description("内部错误")]
        InternalError,
        [Description("未知错误")]
        Unknow,
        [Description("num 值小于1")]
        NumValueLessThanOne,
    }
}
