using MaskCrawler.Utils;

using System.Collections.Generic;

namespace MaskCrawler.Http
{
    public class CssLoadResolver : IStepResolver
    {
        public IList<string> StepResolve(string content, string pattern)
        {
            var result = ASUtil.GetVs(content, pattern);
            return result;
        }
    }
}