# CacheKillers

## Purpose
###CacheKiller
Easy way to prevent caching script ans styles files be cached by browser if they already changed. And not preventing caching if they stay same.
###CacheKiller.Bundles
Prevent bundle files to be cached in case otpimization is turned of. Also allow to turn of optimization for particular bundle.

Be carefull if you use any specific debug transformations in debug time they will not be applyed.

## Using
1. Install [CacheKiller](http://nuget.org/List/Packages/CacheKiller) via [NuGet](http://nuget.org) or [CacheKiller.Bundles](http://nuget.org/List/Packages/CacheKiller.Bundles) it will add CacheKiller as well.
2. Now in cshtml you can call CacheKiller.ScriptsRenderer.Render, CacheKiller.StylesRenderer.Render or CacheKiller.Bundles.ScriptsRenderer.Render, CacheKiller.Bundles.StylesRenderer.Render for bundles

<hr />

## License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).