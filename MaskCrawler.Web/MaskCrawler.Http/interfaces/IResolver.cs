
using System.Collections.Generic;

namespace MaskCrawler.Http
{
    public interface IResolver
    {
        IList<string> Resolve(string resolverName, string content, string pattern);
        IList<string> Resolve(ResolverInfo resolverInfo, string content);
        public void RegisterResolver(string name, IStepResolver stepResolver);
        public void DetachmentResolver(string name);
    }
}
