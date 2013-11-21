namespace CacheKiller
{
    internal class AssetFile
    {
        public string Path { get; set; }

        public string Hash { get; set; }

        public string Render(string formatString)
        {
            var value = Path;
            if(!string.IsNullOrWhiteSpace(Hash))
            {
                value += "?v=" + Hash;
            }
            return string.Format(formatString, value);
        }
    }
}