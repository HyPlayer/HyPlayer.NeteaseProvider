using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseAlbum : AlbumBase, IProgressiveLoadingContainer, IHasCover, IHasTranslation, IHasDescription, IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Album;
    public string? PictureUrl { get; set; }

    public List<string>? Alias { get; set; }
    public List<string>? Translations { get; set; }
    public string? Company { get; set; }
    public string? BriefDescription { get; set; }
    public string? SubType { get; set; }
    public string? AlbumType { get; set; }
    public bool IsSubscribed { get; set; }
    public List<NeteaseArtist>? Artists { get; set; }


    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        if(qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{PictureUrl}?{neteaseImageResourceQualityTag.ToString()}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
        else
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{PictureUrl}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
    }

    public string? Translation { get; set; }
    public string? Description { get; set; }
    public Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = default)
    {
        if (Artists is null) return Task.FromResult<List<PersonBase>?>(null);
        return Task.FromResult(Artists?.Select(ar => (PersonBase)ar).ToList());
    }

    public async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        return (await GetProgressiveItemsListAsync(0, MaxProgressiveCount, ctk)).Item2;
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start, int count, CancellationToken ctk = default)
    {
        if (ActualId is null) return (false, new List<ProvidableItemBase>());

        var result = await NeteaseProvider.Instance.RequestAsync(
            NeteaseApis.AlbumApi,
            new AlbumRequest
            {
                Id = ActualId
            }, ctk);

        return result.Match(
            success =>
            {
                var songs = success.Songs?.Skip(start).Take(count).Select(t => (ProvidableItemBase)t.MapToNeteaseMusic()).ToList()
                            ?? new List<ProvidableItemBase>();
                return (success.Songs?.Length > start + count, songs);
            },
            _ => (false, new List<ProvidableItemBase>()));
    }

    public int MaxProgressiveCount => 100;

    public List<string>? CreatorList { get; init; }
}
