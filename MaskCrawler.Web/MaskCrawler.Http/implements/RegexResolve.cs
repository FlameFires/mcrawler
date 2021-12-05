
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MaskCrawler.Http
{
    public class RegexResolve : IStepResolver
    {
        public IList<string> StepResolve(string content, string pattern)
        {
            Regex regex = new Regex(pattern);
            var list = regex.Matches(content).Select(m => m.Value).ToList();
            return list;
        }
    }
}
