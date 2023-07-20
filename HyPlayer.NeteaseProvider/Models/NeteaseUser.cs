using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseUser : PersonBase, IHasCover, IHasDescription
{
    public override string ProviderId => "ncm";
    public override string TypeId => "us";

    public int Gender { get; set; }
    public string? BackgroundUrl { get; set; }
    public bool Followed { get; set; }
    public int VipType { get; set; }

    public string? AvatarUrl { get; set; }

    public override async Task<List<ContainerBase>> GetSubContainer()
    {
        var results = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.UserPlaylistApi,
                                                    new UserPlaylistRequest
                                                    {
                                                        Uid = ActualId,
                                                        Limit = 9999,
                                                        Offset = 0
                                                    });
        return results.Match(
            success => success.Playlists?.Select(t => (ContainerBase)t.MapToNeteasePlaylist()).ToList() ?? new List<ContainerBase>(),
            error => new List<ContainerBase>()
            );
    }

    public Task<ImageResourceBase?> GetCover()
    {
        return Task.FromResult<ImageResourceBase?>(new NeteaseImageResource()
                                                   {
                                                       Url = AvatarUrl
                                                   });
    }

    public string? Description { get; set; }
}