using System.Web;

namespace CacheKiller
{
    public class StylesRenderer
    {
        private const string DefaultTemplateString = @"<link href=""{0}"" rel=""stylesheet"">";

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