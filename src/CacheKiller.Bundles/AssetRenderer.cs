using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;

namespace CacheKiller.Bundles
{
    internal class AssetRenderer
    {
        private readonly HttpContextBase _context;

        private readonly CacheKiller.AssetRenderer _assetRenderer;

        private static readonly object AssetRendererKey = typeof(AssetRenderer);

        public AssetRenderer(HttpContextBase context)
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }
            _assetRenderer = new CacheKiller.AssetRenderer(context);
            _context = context;
        }

        public static AssetRenderer GetInstance(HttpContextBase context)
        {
            if(context == null)
            {
                return null;
            }
            var assetManager = (AssetRenderer)context.Items[AssetRendererKey];
            if (assetManager == null)
            {
                assetManager = new AssetRenderer(context);
                context.Items[AssetRendererKey] = assetManager;
            }
            return assetManager;
        }

        public IHtmlString GenerateOutput(string templateString, params string[] paths)
        {
            var sb = new StringBuilder();
            foreach (var path in paths)
            {
                sb.AppendLine(_assetRenderer.GenerateString(templateString, GetPathsForPath(path)));
            }
            return new HtmlString(sb.ToString());
        }

        private string[] GetPathsForPath(string path)
        {
            List<string> result = new List<string>();
            var bundleFiles = GetBundleFiles(path);
            if (bundleFiles != null)
            {
                result.AddRange(bundleFiles.Select(bf => bf.IncludedVirtualPath));
            }
            else
            {
                result.Add(path);
            }
            return result.ToArray();
        }

        private IEnumerable<BundleFile> GetBundleFiles(string virtualPath)
        {
            Bundle bundleFor = BundleTable.Bundles.GetBundleFor(virtualPath);
            if (bundleFor == null)
            {
                return null;
            }
            BundleContext context = new BundleContext(_context, BundleTable.Bundles, virtualPath);
            return GetBundleResponse(bundleFor, context).Files;
        }

        private BundleResponse GetBundleResponse(Bundle bundle, BundleContext context)
        {
            BundleResponse response = bundle.CacheLookup(context);
            if (response == null || context.EnableInstrumentation)
            {
                response = bundle.GenerateBundleResponse(context);
                bundle.UpdateCache(context, response);
            }
            return response;
        }
    }
}