using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CacheKiller
{
    public class AssetRenderer
    {
        private readonly HttpContextBase _context;

        private static readonly object AssetRendererKey = typeof(AssetRenderer);

        public AssetRenderer(HttpContextBase context)
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }
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

        public IHtmlString GenerateHtmlString(string templateString, params string[] paths)
        {
            return new HtmlString(GenerateString(templateString, paths));
        }

        public string GenerateString(string templateString, string[] paths)
        {
            var sb = new StringBuilder();
            foreach (var renderFile in GetRenderFiles(paths))
            {
                sb.AppendLine(renderFile.Render(templateString));
            }
            return sb.ToString();
        }

        private IEnumerable<AssetFile> GetRenderFiles(params string[] paths)
        {
            return paths.Select(GetFileForPath).ToList();
        }

        private AssetFile GetFileForPath(string path)
        {
            var file = Cache.Get(_context, path);
            if(file != null)
            {
                return file;
            }
            file = new AssetFile() { Hash = GetHash(path), Path = ResolveUrl(path) };
            Cache.Put(_context, path, file);
            return file;
        }

        private string ResolveUrl(string virtualPath)
        {
            Uri result;
            if (Uri.TryCreate(virtualPath, UriKind.Absolute, out result))
                return virtualPath;
            
            string str = "";
            if(_context.Request != null)
            {
                str = _context.Request.AppRelativeCurrentExecutionFilePath;
            }
            return UrlUtil.Url(str, virtualPath);
        }

        private string GetHash(string virtualPath)
        {
            var server = _context.Server;
            var filePath = server.MapPath(virtualPath);
            if(!File.Exists(filePath))
            {
                return null;
            }
            using (var sha = new SHA256CryptoServiceProvider())
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return HttpServerUtility.UrlTokenEncode(sha.ComputeHash(stream));
                }
            }
        }
    }
}