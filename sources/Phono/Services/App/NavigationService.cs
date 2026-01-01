using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Phono.Contracts.Services.App;
using System;

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
            }
            ;
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
                    if (_rootFrame != null)
                    {
                        _rootFrame.NavigationFailed -= OnNavigationFailed;
                        _rootFrame.Navigated -= OnNavigated;
                        _rootFrame = null;
                    }
                    break;
                case TargetFrameOption.ShellFrame:
                    if (_shellFrame != null)
                    {
                        _shellFrame.NavigationFailed -= OnNavigationFailed;
                        _shellFrame.Navigated -= OnNavigated;
                        _shellFrame = null;
                    }
                    break;
                case TargetFrameOption.SideFrame:
                    if (_sideFrame != null)
                    {
                        _sideFrame.NavigationFailed -= OnNavigationFailed;
                        _sideFrame.Navigated -= OnNavigated;
                        _sideFrame = null;
                    }
                    break;
            }
            ;

        }

        public void NavigateTo(Type sourcePageType, object? parameter = null, NavigationTransitionInfo infoOverride = null, TargetFrameOption option = TargetFrameOption.RootFrame)
        {
            var frame = getTargetFrame(option);
            if (frame == null)
            {
                throw new InvalidOperationException($"No frame registered for {option}. Call RegisterForFrame before navigating.");
            }

            frame.Navigate(sourcePageType, parameter, infoOverride);
        }

        public void NavigateTo(string pageId, object? parameter = null, NavigationTransitionInfo infoOverride = null, TargetFrameOption option = TargetFrameOption.RootFrame)
        {
            var frame = getTargetFrame(option);
            if (frame == null)
            {
                throw new InvalidOperationException($"No frame registered for {option}. Call RegisterForFrame before navigating.");
            }

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

        // Expose registration check to allow callers to await readiness
        public bool IsFrameRegistered(TargetFrameOption option)
        {
            return option switch
            {
                TargetFrameOption.RootFrame => _rootFrame != null,
                TargetFrameOption.ShellFrame => _shellFrame != null,
                TargetFrameOption.SideFrame => _sideFrame != null,
            };
        }
    }
}
    

