using HyPlayer.NeteaseProvider;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Implementation.AudioGraphService;
using HyPlayer.PlayCore.PlayListControllers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Phono.Contracts.Services.App;
using Phono.Views.Settings;
using Phono.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phono.Services.App
{
    public class ActivationService : IActivationService
    {
        private readonly INavigationService _navigationService;

        public ActivationService(INavigationService navigationService) 
        {
            _navigationService = navigationService;
        }

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
            

            MainWindow.Current.Activate();

            // Ensure root frame is registered before navigating
            try
            {
                // if navigation service can report registration, wait for it; otherwise wait for MainWindow's readiness
                if (_navigationService is Phono.Services.App.NavigationService navService)
                {
                    // check immediately
                    if (!navService.IsFrameRegistered(Contracts.Services.App.TargetFrameOption.RootFrame))
                    {
                        // wait for MainWindow's frame ready signal
                        await MainWindow.Current.WaitForRootFrameAsync();
                    }
                }
                else
                {
                    // Fallback - wait briefly for window to finish loading
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                // Log and continue - navigation may still fail but we shouldn't crash the activation path
                System.Diagnostics.Debug.WriteLine($"ActivationService: error while waiting for root frame: {ex}");
            }

            _navigationService.NavigateTo("ShellPage", args, new DrillInNavigationTransitionInfo(), TargetFrameOption.RootFrame);

            var signInTest = new SignInWindow();     
            signInTest.Activate();
        }
    }
}
