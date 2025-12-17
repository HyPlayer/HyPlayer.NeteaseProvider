using CommunityToolkit.Mvvm.ComponentModel;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using Phono.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Phono.ViewModels.AudioPlay
{
    public partial class PlayBarViewModel :
        ObservableRecipient ,IViewModel, ISingletonViewModel
    {
        [ObservableProperty] private bool _isPlaying;
        [ObservableProperty] private ImageResourceBase? _currentSongCover;
        [ObservableProperty] private SingleSongBase _currentSong;
        [ObservableProperty] private ArtistBase? _currentArtist;
        [ObservableProperty] private AlbumBase? _currentAlbum;
        [ObservableProperty] private TimeSpan? _currentPosition;
    }
}
