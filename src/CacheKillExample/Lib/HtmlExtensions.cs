using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace ImprovedBundles.Lib
{
    public static class HtmlExtensions
    {
        public static IHtmlString RenderScripts(this HtmlHelper helper, string path, bool turnOffOptimization = false)
        {
            var context = (HttpContextBase)new HttpContextWrapper(HttpContext.Current);
            if(!turnOffOptimization)
            {
                if(BundleTable.EnableOptimizations)
                {
                    return Scripts.Render(path);
                }
            }
            const string script = @"<script src=""{0}"" type=""text/javascript""></script>";
            return MvcHtmlString.Create(GetInsertString(script, context, path));
        }

        public static IHtmlString RenderStyles(this HtmlHelper helper, string path)
        {
            var context = helper.ViewContext.RequestContext.HttpContext;
            if(BundleTable.EnableOptimizations)
            {
                return Styles.Render(path);
            }

            const string stylesheet = @"<link href=""{0}"" rel=""stylesheet"">";
            return MvcHtmlString.Create(GetInsertString(stylesheet, context, path));
        }

        private static string GetInsertString(string formatString, HttpContextBase context, string path)
        {
            var bundleFiles = GetBundleFiles(path, context);
            if(bundleFiles != null)
            {
                StringBuilder builder = new StringBuilder();
                using (var sha = new SHA256CryptoServiceProvider())
                {
                    foreach (var fileInfo in bundleFiles)
                    {
                        builder.AppendLine(ProcessPath(fileInfo.IncludedVirtualPath, context, sha, formatString));
                    }
                }
                return builder.ToString();
            }
            using (var sha = new SHA256CryptoServiceProvider())
            {
                return ProcessPath(path, context, sha, formatString);
            }
        }

        private static string ProcessPath(string virtualPath, HttpContextBase context, SHA256CryptoServiceProvider sha, string formatString)
        {
            var server = context.Server;
            var filePath = server.MapPath(virtualPath);
            if(!File.Exists(filePath))
            {
                return String.Format(formatString, UrlHelper.GenerateContentUrl(virtualPath, context));
            }
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return String.Format(
                    formatString,
                    UrlHelper.GenerateContentUrl(virtualPath, context) + "?v="
                    + HttpServerUtility.UrlTokenEncode(sha.ComputeHash(stream)));
            }
        }

        private static IEnumerable<BundleFile> GetBundleFiles(string virtualPath, HttpContextBase httpContext)
        {
            Bundle bundleFor = BundleTable.Bundles.GetBundleFor(virtualPath);
            if(bundleFor == null)
            {
                return null;
            }
            BundleContext context = new BundleContext(httpContext, BundleTable.Bundles, virtualPath);
            return GetBundleResponse(bundleFor, context).Files;
        }

        private static BundleResponse GetBundleResponse(Bundle bundle, BundleContext context)
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