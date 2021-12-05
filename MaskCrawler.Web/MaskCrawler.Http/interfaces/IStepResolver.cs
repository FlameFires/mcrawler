using System.Collections.Generic;

namespace MaskCrawler.Http
{
    public interface IStepResolver
    {
        public IList<string> StepResolve(string content, string pattern);
    }
}
