using System.Web;
using System.Web.Optimization;

namespace CacheKiller.Bundles
{
    public static class ScriptsRenderer
    {
        private const string DefaultTemplateString = @"<script src=""{0}""></script>";

        private static AssetRenderer Renderer
        {
            get
            {
                return AssetRenderer.GetInstance(new HttpContextWrapper(HttpContext.Current));
            }
        }

        public static IHtmlString Render(params string[] paths)
        {
            return Render(DefaultTemplateString, false, paths);
        }

        public static IHtmlString Render(bool turnOffOptimization, params string[] paths)
        {
            return Render(DefaultTemplateString, turnOffOptimization, paths);
        }

        public static IHtmlString Render(string formatString, bool turnOffOptimization, params string[] paths)
        {
            if(!turnOffOptimization)
            {
                if(BundleTable.EnableOptimizations)
                {
                    return Scripts.Render(paths);
                }
            }
            return Renderer.GenerateOutput(formatString, paths);
        }
    }
}