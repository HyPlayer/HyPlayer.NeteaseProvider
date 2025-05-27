namespace HyPlayer.NeteaseApi.Bases;

public interface IBatchableApi
{
    public string GetRequestJson(ApiHandlerOption option);
    public string GetRequestJson<TActualRequestMessageModel>(TActualRequestMessageModel actualRequest, ApiHandlerOption option);
    public ResponseBase? GetResponseModel(string json, ApiHandlerOption option);
    public TResponseModel? GetResponseModel<TResponseModel>(string json, ApiHandlerOption option) where TResponseModel : ResponseBase;
}