namespace HyPlayer.NeteaseApi.Bases;

public class IdOrIdListListRequest : RequestBase
{
    /// <summary>
    /// 资源 ID 列表
    /// </summary>
    public List<string>? IdList { get; set; }

    /// <summary>
    /// 资源 ID
    /// </summary>
    public string? Id { get; set; }

    public string ConvertToQuotedIdStringList()
    {
        return string.IsNullOrWhiteSpace(Id)
            ? $"[\"{string.Join("\",\"", IdList ?? [])}]\""
            : $"[\"{Id}\"]";
    }

    public string ConvertToIdStringList()
    {
        return string.IsNullOrWhiteSpace(Id)
            ? $"[{string.Join(",", IdList ?? [])}]"
            : $"[{Id}]";
    }

    public string ParseToIdObjects()
    {
        return string.IsNullOrWhiteSpace(Id)
            ? $"[{string.Join(",", IdList?.Select(id => $$"""{"id":'{{id}}'}""") ?? [])}]"
            : $$"""[{"id": '{{Id}}'}]""";
    }
}