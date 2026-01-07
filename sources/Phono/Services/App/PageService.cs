using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.Services.App;
using Phono.Views.App;
using Phono.Views.Netease;
using Phono.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Search;
using Windows.UI.ApplicationSettings;

namespace Phono.Services.App
{
    internal class PageService : IPageService
    {
        private readonly Dictionary<string, Type> _pages = new();

        public PageService()
        {
            Configure<RootPage>();
            Configure<ShellPage>();
            Configure<TestPage>();
            Configure<HomePage>();
        }

        public Type GetPageType(string key)
        {
            Type? pageType;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out pageType))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
                }
            }

            return pageType;
        }

        private void Configure<V>()
            where V : Page
        {
            lock (_pages)
            {
                var t = typeof(V);
                var key = t.Name;
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                var type = typeof(V);
                if (_pages.ContainsValue(type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }

                _pages.Add(key, type);
            }
        }
    }
}
