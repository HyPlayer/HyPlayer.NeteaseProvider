using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class RadioChannelToNeteaseRadioChannelMapper
{
    public static NeteaseRadioChannel MapToNeteaseRadioChannel(this DjRadioChannelDto radioChannel)
    {
        // TODO: Implement mapping
        return new NeteaseRadioChannel
        {
            ActualId = radioChannel.Id,
            Name = radioChannel.Name ?? "",
            Description = radioChannel.Description,
        };
    }
}