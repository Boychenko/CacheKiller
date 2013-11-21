using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace CacheKiller
{
    internal static class UrlUtil
    {
        internal static string Url(string basePath, string path)
        {
            if(basePath != null)
            {
                path = VirtualPathUtility.Combine(basePath, path);
            }
            path = VirtualPathUtility.ToAbsolute(path);
            return HttpUtility.UrlPathEncode(path);
        }
    }
}