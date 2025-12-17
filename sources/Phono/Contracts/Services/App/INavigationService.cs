using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phono.Contracts.Services.App
{
    public interface INavigationService
    {
        void RegisterForFrame(Frame targetFrame, TargetFrameOption option);

        void UnregisterForFrame(TargetFrameOption option);

        void NavigateTo(Type sourcePageType, object? parameter = null, NavigationTransitionInfo infoOverride = null, TargetFrameOption option = TargetFrameOption.RootFrame);

        void NavigateTo(string pageId, object? parameter = null, NavigationTransitionInfo infoOverride = null, TargetFrameOption option = TargetFrameOption.RootFrame);

    }

    public enum TargetFrameOption
    {
        RootFrame,
        ShellFrame,
        SideFrame
    }
}
