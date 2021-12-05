using AngleSharp;

using System;
using System.Collections.Generic;

namespace MaskCrawler.Utils
{
    public class ASUtil
    {
        public static IList<string> GetVs(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"“{nameof(text)}”不能为 null 或空。", nameof(text));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException($"“{nameof(pattern)}”不能为 null 或空。", nameof(pattern));
            }

            IList<string> result;
            try
            {
                var config = AngleSharp.Configuration.Default;
                var context = AngleSharp.BrowsingContext.New(config);
                var document = context.OpenAsync(req => req.Content(text)).GetAwaiter().GetResult();
                AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> list = document.QuerySelectorAll(pattern);
                result = new List<string>();
                foreach (var item in list)
                {
                    result.Add(item.ToString());
                }
                return result;
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
            }
        }
    }
}
