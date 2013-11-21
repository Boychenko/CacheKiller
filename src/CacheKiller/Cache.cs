using System;
using System.Web;
using System.Web.Hosting;

namespace CacheKiller
{
    internal class Cache
    {
        public static AssetFile Get(HttpContextBase context, string path)
        {
            if(context != null && context.Cache != null)
            {
                return context.Cache.Get(path) as AssetFile;
            }
            return null;
        }

        public static void Put(HttpContextBase context, string path, AssetFile assetFile)
        {
            if(context == null 
                || context.Cache == null
                || String.IsNullOrEmpty(assetFile.Hash))
            {
                return;
            }
            var dep = HostingEnvironment.VirtualPathProvider.GetCacheDependency(
                path, new string[] { assetFile.Path }, DateTime.UtcNow);
            context.Cache.Insert(path, assetFile, dep);
        }
    }
}