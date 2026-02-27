using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class RadioChannelToNeteaseRadioChannelMapper
{
    /// <summary>
    /// 将 DjRadioChannelDto 映射到 NeteaseRadioChannel
    /// </summary>
    public static NeteaseRadioChannel MapToNeteaseRadioChannel(this DjRadioChannelDto radioChannel)
    {
        return new NeteaseRadioChannel
        {
            ActualId = radioChannel.Id!,
            Name = radioChannel.Name ?? "未知电台",
            Description = radioChannel.Description,
            ProgramCount = radioChannel.ProgramCount,
            CreateTime = radioChannel.CreateTime,
            SubscribedCount = radioChannel.SubscribedCount,
            CoverUrl = radioChannel.CoverUrl,
            CategoryId = radioChannel.CategoryId,
            Category = radioChannel.Category,
            SecondCategoryId = radioChannel.SecondCategoryId,
            SecondCategory = radioChannel.SecondCategory,
            LikedCount = radioChannel.LikedCount,
            CommentCount = radioChannel.CommentCount,
            ShareCount = radioChannel.ShareCount,
            PlayCount = radioChannel.PlayCount,
            RecommendText = radioChannel.RecommendText,
            Price = radioChannel.Price,
            Bought = radioChannel.Bought,
            LastProgramCreateTime = radioChannel.LastProgramCreateTime,
            LastProgramId = radioChannel.LastProgramId,
            LastProgramName = radioChannel.LastProgramName,
            IsHighQuality = radioChannel.IsHighQuality,
            Subscribed = radioChannel.Subscribed,
            CreatorList = new List<string>()
        };
    }

    /// <summary>
    /// 将 DjRadioChannelWithDjDto 映射到 NeteaseRadioChannel
    /// </summary>
    public static NeteaseRadioChannel MapToNeteaseRadioChannel(this DjRadioChannelWithDjDto radioChannel)
    {
        var channel = ((DjRadioChannelDto)radioChannel).MapToNeteaseRadioChannel();
        
        if (radioChannel.DjData != null)
        {
            var host = radioChannel.DjData.MapToNeteaseUser();
            channel.Host = host;
            channel.CreatorList?.Clear();
            channel.CreatorList?.Add(host.Name);
        }

        return channel;
    }
}