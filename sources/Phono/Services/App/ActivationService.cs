using HyPlayer.NeteaseProvider;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Implementation.AudioGraphService;
using HyPlayer.PlayCore.PlayListControllers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.Services.App;
using Phono.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phono.Services.App
{
    public class ActivationService : IActivationService
    {
        public async void OnActivated(object args)
        {
            var playCore = Locator.Instance?.GetService<PlayCoreBase>();
            if (playCore != null)
            {
                await playCore.RegisterMusicProviderAsync(typeof(NeteaseProvider));
                await playCore.RegisterAudioServiceAsync(typeof(AudioGraphService));
                await playCore.RegisterPlayListControllerAsync(typeof(DefaultPlayListManager));
                await playCore.RegisterPlayListControllerAsync(typeof(OrderedRollPlayController));
            }

            var rootFrame = MainWindow.Current.Content as Frame;

            if (rootFrame is null)
            {
                rootFrame = new Frame();
                MainWindow.Current.Content = rootFrame;
            }

            MainWindow.Current.Activate();
        }
    }
}
