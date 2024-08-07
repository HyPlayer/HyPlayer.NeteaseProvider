using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseAlbum : AlbumBase, IHasCover, IHasTranslation, IHasDescription, IHasCreators
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
    public Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = new())
    {
        if (Artists is null) return Task.FromResult<List<PersonBase>?>(null);
        return Task.FromResult(Artists?.Select(ar => (PersonBase)ar).ToList());
    }

    public List<string>? CreatorList { get; init; }
}