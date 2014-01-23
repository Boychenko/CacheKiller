# CacheKillers

## Purpose
###CacheKiller
An easy way to prevent caching of js and css files by browser if they are changed.
###CacheKiller.Bundles
Prevent bundle files to be cached in case otpimization is turned off. Also allow to turn of optimization for particular bundle.

###If you are using [dotless](https://github.com/dotless/dotless) you should adopt configuration
```xml
<dotless disableParameters="true" />
```

## Using
1. Install [CacheKiller](http://nuget.org/List/Packages/CacheKiller) via [NuGet](http://nuget.org) or [CacheKiller.Bundles](http://nuget.org/List/Packages/CacheKiller.Bundles) it will add CacheKiller as well.
2. Now in cshtml you can call CacheKiller.ScriptsRenderer.Render, CacheKiller.StylesRenderer.Render or CacheKiller.Bundles.ScriptsRenderer.Render, CacheKiller.Bundles.StylesRenderer.Render for bundles

###Usage sample
**Original**
```html
<link href="~/Content/bootstrap.css" rel="stylesheet">
<script src="~/Scripts/jquery-1.10.2.js"></script>
@Styles.Render("~/Content/css")
@Scripts.Render("~/bundles/jquery")
```
**Updated**
```csharp
@CacheKiller.StylesRenderer.Render("~/Content/bootstrap.css")
@CacheKiller.ScriptsRenderer.Render("~/Scripts/jquery-1.10.2.js")
@CacheKiller.Bundles.StylesRenderer.Render("~/Content/css")
@CacheKiller.Bundles.ScriptsRenderer.Render("~/bundles/jquery")
```

In case if you use bundles you are free to use only CacheKiller.Bundles.StylesRenderer.Render and CacheKiller.Bundles.ScriptsRenderer.Render.
```csharp
@CacheKiller.Bundles.StylesRenderer.Render("~/Content/bootstrap.css")
@CacheKiller.Bundles.ScriptsRenderer.Render("~/Scripts/jquery-1.10.2.js")
@CacheKiller.Bundles.StylesRenderer.Render("~/Content/css")
@CacheKiller.Bundles.ScriptsRenderer.Render("~/bundles/jquery")
```

<hr />

## License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).
