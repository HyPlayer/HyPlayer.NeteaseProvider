using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseArtist : ArtistBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Artist;
    public string? Translation { get; set; }
    public string[]? Alias { get; set; }
    public string? CoverUrl { get; set; }
    public int AlbumSize { get; set; }
    public int MvSize { get; set; }

    public override Task<List<ContainerBase>> GetSubContainerAsync(CancellationToken ctk = default)
    {
        return
            Task.FromResult(new List<ContainerBase>()
            {
                new NeteaseArtistSubContainer
                {
                    Name = "热门歌曲",
                    ActualId = "hot" + ActualId
                },
                new NeteaseArtistSubContainer()
                {
                    Name = "最新歌曲",
                    ActualId = "tim" + ActualId
                },
                new NeteaseArtistSubContainer()
                {
                    Name = "专辑",
                    ActualId = "alb" + ActualId
                }
            });
    }
}
