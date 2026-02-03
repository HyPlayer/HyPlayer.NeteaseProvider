using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseRadioChannel : LinerContainerBase, IProgressiveLoadingContainer, IHasCover, IHasDescription,
IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.RadioChannel;

    /// <summary>
    /// 电台节目数量
    /// </summary>
    public int ProgramCount { get; set; }

    /// <summary>
    /// 创建时间戳 (毫秒)
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 订阅人数
    /// </summary>
    public long SubscribedCount { get; set; }

    /// <summary>
    /// 电台封面 URL
    /// </summary>
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 类别ID
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 类别名称
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// 子类别ID
    /// </summary>
    public int SecondCategoryId { get; set; }

    /// <summary>
    /// 子类别名称
    /// </summary>
    public string? SecondCategory { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public long LikedCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    public long CommentCount { get; set; }

    /// <summary>
    /// 分享数
    /// </summary>
    public long ShareCount { get; set; }

    /// <summary>
    /// 播放数
    /// </summary>
    public long PlayCount { get; set; }

    /// <summary>
    /// 推荐文本
    /// </summary>
    public string? RecommendText { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public float Price { get; set; }

    /// <summary>
    /// 是否已购买
    /// </summary>
    public bool Bought { get; set; }

    /// <summary>
    /// 最后一期节目创建时间
    /// </summary>
    public long LastProgramCreateTime { get; set; }

    /// <summary>
    /// 最后一期节目ID
    /// </summary>
    public string? LastProgramId { get; set; }

    /// <summary>
    /// 最后一期节目名称
    /// </summary>
    public string? LastProgramName { get; set; }

    /// <summary>
    /// 是否为高质量电台
    /// </summary>
    public bool IsHighQuality { get; set; }

    /// <summary>
    /// 是否已订阅
    /// </summary>
    public bool Subscribed { get; set; }

    /// <summary>
    /// 电台主播
    /// </summary>
    public NeteaseUser? Host { get; set; }

    private NeteaseRadioProgram[]? _cachedPrograms;

    /// <summary>
    /// 获取所有节目
    /// </summary>
    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = new CancellationToken())
    {
        var programs = new List<NeteaseRadioProgram>();
        int offset = 0;
        const int pageSize = 100;
        bool hasMore = true;

        while (hasMore)
        {
            var (more, items) = await GetProgressiveItemsListAsync(offset, pageSize, ctk);
            programs.AddRange(items.Cast<NeteaseRadioProgram>());
            hasMore = more;
            offset += pageSize;
        }

        return programs.Cast<ProvidableItemBase>().ToList();
    }

    /// <summary>
    /// 获取分页节目列表
    /// </summary>
    public int MaxProgressiveCount => 100;

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start, int count, CancellationToken ctk = new CancellationToken())
    {
        var result = await NeteaseProvider.Instance.RequestAsync(
            NeteaseApis.DjChannelProgramsApi,
            new DjChannelProgramsRequest
            {
                RadioId = ActualId!,
                Limit = count,
                Offset = start,
                Asc = false
            }, ctk);

        return result.Match(
            success =>
            {
                var programs = success.Data?.Programs?
                    .Select(p => (ProvidableItemBase)p.MapToNeteaseRadioProgram())
                    .ToList() ?? new List<ProvidableItemBase>();
                bool hasMore = success.Data?.More ?? false;
                return (hasMore, programs);
            },
            error => (false, new List<ProvidableItemBase>()));
    }

    /// <summary>
    /// 获取电台封面
    /// </summary>
    public async Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = new CancellationToken())
    {
        var coverUrl = CoverUrl ?? "";
        if (string.IsNullOrEmpty(coverUrl))
        {
            return new NeteaseImageResourceResult()
            {
                ExternalException = new Exception("No cover URL available"),
                ResourceStatus = ResourceStatus.Success,
                Uri = null!
            };
        }

        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            return new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{coverUrl}?{neteaseImageResourceQualityTag}")
            };
        }

        return new NeteaseImageResourceResult()
        {
            ExternalException = null,
            ResourceStatus = ResourceStatus.Success,
            Uri = new Uri(coverUrl)
        };
    }

    /// <summary>
    /// 电台描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 主播名称列表
    /// </summary>
    public List<string>? CreatorList { get; init; }

    /// <summary>
    /// 获取电台主播列表
    /// </summary>
    public Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = new CancellationToken())
    {
        if (Host != null)
            return Task.FromResult<List<PersonBase>?>(new List<PersonBase> { Host });

        return Task.FromResult<List<PersonBase>?>(null);
    }
}