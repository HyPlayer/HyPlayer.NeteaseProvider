using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseRadioProgram : SingleSongBase, IHasCover, IHasDescription, IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.RadioProgram;

    /// <summary>
    /// 节目封面 URL
    /// </summary>
    public string? PictureUrl { get; set; }

    /// <summary>
    /// 节目封面 URL (备用)
    /// </summary>
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 节目时长
    /// </summary>
    public long ProgramDuration { get; set; }

    /// <summary>
    /// 节目所属电台
    /// </summary>
    public NeteaseRadioChannel? RadioChannel { get; set; }

    /// <summary>
    /// 节目主歌曲
    /// </summary>
    public NeteaseSong? MainSong { get; set; }

    /// <summary>
    /// 是否已购买
    /// </summary>
    public bool Bought { get; set; }

    /// <summary>
    /// 监听人数
    /// </summary>
    public long ListenerCount { get; set; }

    /// <summary>
    /// 订阅人数
    /// </summary>
    public long SubscribedCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    public long CommentCount { get; set; }

    /// <summary>
    /// 分享数
    /// </summary>
    public long ShareCount { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public long LikedCount { get; set; }

    /// <summary>
    /// 创建时间戳 (毫秒)
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 期数
    /// </summary>
    public int SerialNum { get; set; }

    /// <summary>
    /// 节目描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 主播信息
    /// </summary>
    public NeteaseUser? Host { get; set; }

    public override Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = default)
    {
        if (Host != null)
            return Task.FromResult<List<PersonBase>?>(new List<PersonBase> { Host });

        return Task.FromResult<List<PersonBase>?>(null);
    }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        var coverUrl = CoverUrl ?? PictureUrl ?? "";
        if (string.IsNullOrEmpty(coverUrl))
        {
            return Task.FromResult<ResourceResultBase>(new NeteaseImageResourceResult()
            {
                ExternalException = new Exception("No cover URL available"),
                ResourceStatus = ResourceStatus.Success,
                Uri = null!
            });
        }

        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{coverUrl}?{neteaseImageResourceQualityTag}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }

        return Task.FromResult(new NeteaseImageResourceResult()
        {
            ExternalException = null,
            ResourceStatus = ResourceStatus.Success,
            Uri = new Uri(coverUrl)
        } as ResourceResultBase);
    }
}
