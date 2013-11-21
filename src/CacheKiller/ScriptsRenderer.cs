using System.Web;

namespace CacheKiller
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
            return RenderFormat(DefaultTemplateString, paths);
        }

        public static IHtmlString RenderFormat(string formatString, params string[] paths)
        {
            return Renderer.GenerateHtmlString(formatString, paths);
        }
    }
}