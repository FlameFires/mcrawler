
using System;
using System.Collections.Generic;
using System.Text;

namespace MaskCrawler.Utils
{
    public class HAPUtil
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
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(text);

                var list = doc.DocumentNode.SelectNodes(pattern);
                if(list == null || list.Count == 0)
                {
                    return null;
                }

                string temp = string.Empty;
                (State state, string key) = ParsePattern(pattern);
                result = new List<string>();
                foreach (var node in list)
                {
                    temp = state switch
                    {
                        State.Attribute => node.Attributes?[key]?.Value,
                        State.Text => node.GetDirectInnerText(),
                        _ => null
                    };

                    if (!string.IsNullOrEmpty(temp))
                        result.Add(temp);
                }
            }
            catch (Exception)
            {
#if DEBUG

                throw;
#endif
            }
            return result;
        }

        private enum State
        {
            Text,
            Attribute,

        }

        private static Tuple<State, string> ParsePattern(string pattern)
        {
            if (pattern is null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            try
            {
                var kvs = pattern.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (kvs.Length == 0)
                    return null;

                var key = kvs[kvs.Length - 1];

                if (key.Equals("text()", StringComparison.OrdinalIgnoreCase))
                {
                    return Tuple.Create(State.Text, key);
                }
                else if (key.IndexOfAny(new char[] { '@' }, 0) != -1)
                {
                    return Tuple.Create(State.Attribute, key.Substring(1, key.Length - 1));
                }
                else
                {
                    return Tuple.Create(State.Text, key);
                }
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