using MaskCrawler.Utils;

using System.Collections.Generic;

namespace MaskCrawler.Http
{
    public class XPathResolve : IStepResolver
    {
        public IList<string> StepResolve(string content, string pattern)
        {
            var result = HAPUtil.GetVs(content, pattern);
            return result;
        }
    }
}
