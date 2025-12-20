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

namespace Phono.ViewModels.AudioPlay
{
    public partial class PlayBarViewModel :
        ObservableRecipient ,IViewModel, ISingletonViewModel, IDisposable,
        INotificationSubscriber<CurrentSongChangedNotification>
    {
        [ObservableProperty] private bool _isPlaying;
        [ObservableProperty] private ImageResourceBase? _currentSongCover;
        [ObservableProperty] private SingleSongBase? _currentSong;
        [ObservableProperty] private ArtistBase? _currentArtist;
        [ObservableProperty] private AlbumBase? _currentAlbum;
        [ObservableProperty] private TimeSpan? _currentPosition;

        private readonly PlayCoreBase? _playCore;
        private bool _disposed;

        public PlayBarViewModel(PlayCoreBase playCore)
        {
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
