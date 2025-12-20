using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Phono.Contracts.Services.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phono.Services.App
{
    public class NavigationService : INavigationService
    {
        private readonly IPageService _pageService;
        private Frame _rootFrame { get; set; }
        private Frame _shellFrame { get; set; }
        private Frame _sideFrame { get; set; }

        public NavigationService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public void RegisterForFrame(Frame targetFrame, TargetFrameOption option)
        {
            switch (option) 
            {
                case 0:
                    _rootFrame = targetFrame;
                    break;
                case TargetFrameOption.ShellFrame:
                    _shellFrame = targetFrame;
                    break;
                case TargetFrameOption.SideFrame:
                    _sideFrame = targetFrame;
                    break;
            };
            targetFrame.NavigationFailed += OnNavigationFailed;
            targetFrame.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            
        }

        public void UnregisterForFrame(TargetFrameOption option)
        {
            switch (option)
            {
                case 0:
                    _rootFrame = null;
                    _rootFrame.NavigationFailed += OnNavigationFailed;
                    _rootFrame.Navigated += OnNavigated;
                    break;
                case TargetFrameOption.ShellFrame:
                    _shellFrame = null;
                    _shellFrame.NavigationFailed += OnNavigationFailed;
                    _shellFrame.Navigated += OnNavigated;
                    break;
                case TargetFrameOption.SideFrame:
                    _sideFrame = null;
                    _sideFrame.NavigationFailed += OnNavigationFailed;
                    _sideFrame.Navigated += OnNavigated;
                    break;
            };
            
        }

        public void NavigateTo(Type sourcePageType, object? parameter = null, NavigationTransitionInfo infoOverride = null, TargetFrameOption option = TargetFrameOption.RootFrame)
        {
            var frame = getTargetFrame(option);
            frame.Navigate(sourcePageType, parameter, infoOverride);
        }

        public void NavigateTo(string pageId, object? parameter = null, NavigationTransitionInfo infoOverride = null, TargetFrameOption option = TargetFrameOption.RootFrame)
        {
            var frame = getTargetFrame(option);
            Type page = _pageService.GetPageType(pageId);
            frame.Navigate(page, parameter, infoOverride);
        }

        private Frame getTargetFrame(TargetFrameOption option)
        {
            return option switch
            {
                TargetFrameOption.RootFrame => _rootFrame,
                TargetFrameOption.ShellFrame => _shellFrame,
                TargetFrameOption.SideFrame => _sideFrame,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
