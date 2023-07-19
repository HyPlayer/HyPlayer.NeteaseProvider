using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseArtist : ArtistBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => "ar";

    public override async Task<List<ContainerBase>> GetSubContainer()
    {
        return 
            new List<ContainerBase>()
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
            };
    }
}