using CommunityToolkit.Mvvm.ComponentModel;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using Phono.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Models.Notifications;
using Depository.Abstraction.Interfaces.NotificationHub;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using AsyncAwaitBestPractices;

namespace Phono.ViewModels.AudioPlay
{
    public partial class PlayBarViewModel :
        ObservableRecipient ,IViewModel, ISingletonViewModel, IDisposable,
        INotificationSubscriber<CurrentSongChangedNotification>,
        INotificationSubscriber<PlaybackPositionChangedNotification>
    {
        [ObservableProperty] private bool _isPlaying;
        [ObservableProperty] private ImageResourceBase? _currentSongCover;
        [ObservableProperty] private SingleSongBase? _currentSong;
        [ObservableProperty] private ArtistBase? _currentArtist;
        [ObservableProperty] private AlbumBase? _currentAlbum;
        [ObservableProperty] private TimeSpan? _currentPosition;

        // Expose seconds-based values for easy binding to ProgressBar
        [ObservableProperty] private double _currentPositionSeconds;
        [ObservableProperty] private double _currentSongDurationSeconds;

        private readonly PlayCoreBase? _playCore;
        private bool _disposed;

        public PlayBarViewModel(PlayCoreBase playCore)
        {
            IsPlaying = false;

            _playCore = playCore;

            // Initialize from current state if available
            var coreSong = _playCore?.CurrentSong;
            if (!EqualityComparer<SingleSongBase?>.Default.Equals(coreSong, CurrentSong))
            {
                CurrentSong = coreSong;
            }

            var coreAlbum = coreSong?.Album;
            if (!EqualityComparer<AlbumBase?>.Default.Equals(coreAlbum, CurrentAlbum))
            {
                CurrentAlbum = coreAlbum;
            }

            UpdatePlayingStateFromPlayCore();
        }

        // Called when CurrentSong property changes by CommunityToolkit source generator
        partial void OnCurrentSongChanged(SingleSongBase? value)
        {
            // Reset position
            CurrentPositionSeconds = 0;
            CurrentPosition = TimeSpan.Zero;

            if (value != null && value.Duration > 0)
            {
                // Duration stored as long in the model. Treat it as milliseconds and convert to seconds for the progress bar.
                CurrentSongDurationSeconds = value.Duration / 1000.0;
            }
            else
            {
                CurrentSongDurationSeconds = 0;
            }
        }

        public Task HandleNotificationAsync(CurrentSongChangedNotification notification, CancellationToken ctk = new())
        {
            ctk.ThrowIfCancellationRequested();

            var song = notification.CurrentPlayingSong;
            if (!EqualityComparer<SingleSongBase?>.Default.Equals(song, CurrentSong))
            {
                CurrentSong = song;
            }

            var album = song?.Album;
            if (!EqualityComparer<AlbumBase?>.Default.Equals(album, CurrentAlbum))
            {
                CurrentAlbum = album;
            }

            // Try infer playing state from play core's current ticket
            UpdatePlayingStateFromPlayCore();

            // Position and cover are left to be provided by specific audio service implementations or additional notifications

            return Task.CompletedTask;
        }

        public Task HandleNotificationAsync(PlaybackPositionChangedNotification notification, CancellationToken ctk = new())
        {
            ctk.ThrowIfCancellationRequested();

            try
            {
                // Notification value represents current playback position in seconds (double)
                var seconds = notification.CurrentPlaybackPosition;
                CurrentPositionSeconds = seconds;
                CurrentPosition = TimeSpan.FromSeconds(seconds);
            }
            catch (Exception)
            {
                // ignore non-fatal errors updating UI state
            }

            return Task.CompletedTask;
        }

        private void UpdatePlayingStateFromPlayCore()
        {
            try
            {
                var ticket = _playCore?.CurrentPlayingTicket;
                bool isPlaying = ticket is not null && ticket.Status == HyPlayer.PlayCore.Abstraction.Models.AudioServiceComponents.AudioTicketStatus.Playing;
                if (IsPlaying != isPlaying) IsPlaying = isPlaying;
            }
            catch
            {
                // ignore
            }
        }

        [RelayCommand]
        private void TogglePlayPause()
        {
            
            if (_playCore == null) return;
            var ticket = _playCore.CurrentPlayingTicket;
            if (ticket == null) return;
            if (ticket.Status == HyPlayer.PlayCore.Abstraction.Models.AudioServiceComponents.AudioTicketStatus.Playing)
            {
                _playCore.PauseAsync().SafeFireAndForget();
                IsPlaying = false;
            }
            else
            {
                _playCore.PlayAsync().SafeFireAndForget();
                IsPlaying = true;
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[PlayBarViewModel] TogglePlayPause executed. New IsPlaying: {IsPlaying}");
#endif
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                // Notification subscription lifecycle is managed by Depository/NotificationHub.
                // If explicit unregistration API is required, it can be called here.
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
