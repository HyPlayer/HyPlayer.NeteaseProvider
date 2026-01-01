using CommunityToolkit.Mvvm.ComponentModel;
using HyPlayer.NeteaseProvider;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models;
using Phono.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playlists;

namespace Phono.ViewModels.Netease
{
    public partial class HomeViewModel : ObservableRecipient, IScopedViewModel
    {
        public ObservableCollection<NeteaseSong> RecommendedSongs { get; } = new ObservableCollection<NeteaseSong>();
        public ObservableCollection<NeteasePlaylist> RecommendedPlaylists { get; } = new ObservableCollection<NeteasePlaylist>();
        public ObservableCollection<NeteasePlaylist> TopPlaylists { get; } = new ObservableCollection<NeteasePlaylist>();

        private readonly NeteaseProvider _neteaseProvider;
        
        public HomeViewModel(NeteaseProvider neteaseProvider)
        {
            _neteaseProvider = neteaseProvider;
        }

        public async Task LoadDataAsync()
        {
            if (_neteaseProvider.LoginedUser != null)
            {
                var recommendedSongs = await _neteaseProvider.GetRecommendationAsync("sg") as NeteaseActionGettableContainer;
                RecommendedSongs.Clear();
        
                var recommendedPlaylists = await _neteaseProvider.GetRecommendationAsync("pl") as NeteaseActionGettableContainer;
                RecommendedPlaylists.Clear();

                if (recommendedPlaylists != null)
                {
                    if (await recommendedPlaylists.GetAllItemsAsync() is List<ProvidableItemBase> providableItems)
                    {
                        // 推荐歌单
                        var lists = providableItems.Select(t => (NeteasePlaylist)t).ToList();

                        foreach (var item in lists)
                        {
                            RecommendedPlaylists.Add(item);
                        }

                    }
                }

                if (recommendedSongs != null)
                {
                    if (await recommendedSongs.GetAllItemsAsync() is List<ProvidableItemBase> providableItems)
                    {
                        // 推荐歌单
                        var lists = providableItems.Select(t => (NeteaseSong)t).ToList();

                        foreach (var item in lists)
                        {
                            RecommendedSongs.Add(item);
                        }

                    }
                }
            }

            var topPlaylists = await _neteaseProvider.GetRecommendationAsync("ct") as NeteaseActionGettableContainer;
            TopPlaylists.Clear();

            if (topPlaylists != null)
            {
                if (await topPlaylists.GetAllItemsAsync() is List<ProvidableItemBase> providableItems)
                {
                    // 推荐歌单
                    var lists = providableItems.Select(t => (NeteasePlaylist)t).ToList();

                    foreach (var item in lists)
                    {
                        TopPlaylists.Add(item);
                    }

                }
            }

        }
    }
}
