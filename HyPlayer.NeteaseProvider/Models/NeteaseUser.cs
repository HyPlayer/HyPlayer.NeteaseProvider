using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseUser : PersonBase, IHasCover, IHasDescription
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.User;

    public int Gender { get; set; }
    public string? BackgroundUrl { get; set; }
    public bool Followed { get; set; }
    public int VipType { get; set; }

    public string? AvatarUrl { get; set; }

    public override async Task<List<ContainerBase>> GetSubContainerAsync(CancellationToken ctk = default)
    {
        var results = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.UserPlaylistApi,
                                                    new UserPlaylistRequest
                                                    {
                                                        Uid = ActualId,
                                                        Limit = 9999,
                                                        Offset = 0
                                                    });
        return results.Match(
            success =>
            {
                var playlists = success.Playlists?.Select(t => t.MapToNeteasePlaylist()).ToList() ?? new List<NeteasePlaylist>();
                var created = playlists.Where(t => t.Creator?.ActualId == ActualId).ToList();
                var subscribed = playlists.Where(t => t.Creator?.ActualId != ActualId).ToList();
                var containers = new List<ContainerBase>();

                if (created.Count > 0)
                    containers.Add(new NeteaseUserPlaylistSubContainer
                    {
                        ActualId = $"created{ActualId}",
                        Name = "创建的歌单",
                        Playlists = created
                    });

                if (subscribed.Count > 0)
                    containers.Add(new NeteaseUserPlaylistSubContainer
                    {
                        ActualId = $"subscribed{ActualId}",
                        Name = "收藏的歌单",
                        Playlists = subscribed
                    });

                return containers;
            },
            error => new List<ContainerBase>()
            );
    }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{AvatarUrl}?{neteaseImageResourceQualityTag.ToString()}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
        else
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{AvatarUrl}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
    }

    public string? Description { get; set; }
}
