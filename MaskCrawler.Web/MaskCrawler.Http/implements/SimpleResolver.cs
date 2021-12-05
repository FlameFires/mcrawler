
using System;
using System.Collections.Generic;

namespace MaskCrawler.Http
{
    public class SimpleResolver : IResolver
    {
        public SimpleResolver()
        {
            RegisterResolver(ResolverTypeEnum.XPath.ToString(), new XPathResolve());
            RegisterResolver(ResolverTypeEnum.Regex.ToString(), new RegexResolve());
            RegisterResolver(ResolverTypeEnum.CssLoad.ToString(), new CssLoadResolver());
        }

        protected IDictionary<string, IStepResolver> _resolvers { get; set; } = new Dictionary<string, IStepResolver>();

        public IList<string> Resolve(string resolverName, string content, string pattern)
        {
            var resolver = _resolvers[resolverName];
            if (resolver == null)
            {
                throw new ArgumentNullException($"ResolverName({resolverName}) not found.");
            }

            var result = resolver.StepResolve(content, pattern);
            return result;
        }

        public void RegisterResolver(string name, IStepResolver stepResolver)
        {
            _resolvers.Add(name, stepResolver);
        }

        public void DetachmentResolver(string name)
        {
            _resolvers.Remove(name);
        }

        public IList<string> Resolve(ResolverInfo resolverInfo, string content)
        {
            return Resolve(resolverInfo.Type.ToString(), content, resolverInfo.Pattern);
        }
    }
}
