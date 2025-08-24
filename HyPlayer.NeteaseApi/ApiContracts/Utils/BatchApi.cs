using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Extensions.JsonSerializer;
using System.Collections;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static BatchApi BatchApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Utils
{
    public class BatchApi : EApiContractBase<BatchRequest, BatchResponse, ErrorResultBase, BatchActualRequest>
    {
        public override string IdentifyRoute => "/batch";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/batch";
        public override HttpMethod Method => HttpMethod.Post;

        public override async Task MapRequest(ApiHandlerOption option)
        {
            if (Request is null) return;
            var actualRequest = new BatchActualRequest();
            foreach (var kvp in Request.Apis)
            {
                var api = kvp.Value;
                if (api is IApiMapRequest apiMapRequest)
                    await apiMapRequest.MapRequest(option);
                var request = api.GetRequestJson(option);
                actualRequest[kvp.Key] = request;
            }

            ActualRequest = actualRequest;
        }

        public override async Task<Results<BatchResponse, ErrorResultBase>> ProcessResponseAsync(
            HttpResponseMessage response, ApiHandlerOption option,
            CancellationToken cancellationToken = default)
        {
            var ret = new BatchResponse();
            var resp = await base.ProcessResponseAsync<BatchActualResponse>(response, option, cancellationToken);
            if (resp.IsSuccess)
            {
                foreach (var kvp in resp.Value ?? [])
                {
                    if (kvp.Key == "code")
                    {
                        if (kvp.Value?.Value != "200")
                        {
                            return Results<BatchResponse, ErrorResultBase>.CreateError(
                                new ErrorResultBase(int.Parse(kvp.Value?.Value ?? "500"), "返回值不为 200"));
                        }
                        continue;
                    }

                    var api = Request?.Apis[kvp.Key];
                    var singleResponse = api?.GetResponseModel(kvp.Value?.Value ?? "", option);
                    ret.Results[kvp.Key] = singleResponse;
                }
            }

            return ret;
        }

        public override string ApiPath { get; protected set; } = "/api/batch";
    }

    public class BatchRequest : RequestBase
    {
        public Dictionary<string, IBatchableApi> Apis { get; set; } = [];
    }

    public class BatchActualResponse : ResponseBase, IDictionary<string, JsonObjectStringWrapper>
    {
        private readonly Dictionary<string, JsonObjectStringWrapper> _backingDictionary = new();

        public IEnumerator<KeyValuePair<string, JsonObjectStringWrapper>> GetEnumerator()
        {
            return _backingDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_backingDictionary).GetEnumerator();
        }

        public void Add(KeyValuePair<string, JsonObjectStringWrapper> item)
        {
            _backingDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, JsonObjectStringWrapper> item)
        {
            return _backingDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, JsonObjectStringWrapper>[] array, int arrayIndex)
        {
            ((ICollection)_backingDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, JsonObjectStringWrapper> item)
        {
            return _backingDictionary.Remove(item.Key);
        }

        public int Count => _backingDictionary.Count;

        public bool IsReadOnly =>
            ((ICollection<KeyValuePair<string, JsonObjectStringWrapper>>)_backingDictionary).IsReadOnly;

        public void Add(string key, JsonObjectStringWrapper value)
        {
            _backingDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _backingDictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _backingDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out JsonObjectStringWrapper value)
        {
            return _backingDictionary.TryGetValue(key, out value);
        }

        public JsonObjectStringWrapper this[string key]
        {
            get => _backingDictionary[key];
            set => _backingDictionary[key] = value;
        }

        public ICollection<string> Keys => ((IDictionary<string, JsonObjectStringWrapper>)_backingDictionary).Keys;

        public ICollection<JsonObjectStringWrapper> Values =>
            ((IDictionary<string, JsonObjectStringWrapper>)_backingDictionary).Values;
    }

    public class BatchResponse : ResponseBase
    {
        public Dictionary<string, ResponseBase?> Results { get; set; } = [];
    }

    public class BatchActualRequest : EApiActualRequestBase, IDictionary<string, string>
    {
        private readonly Dictionary<string, string> _backingDictionary = new();

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _backingDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_backingDictionary).GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _backingDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _backingDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection)_backingDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _backingDictionary.Remove(item.Key);
        }

        public int Count => _backingDictionary.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, string>>)_backingDictionary).IsReadOnly;

        public void Add(string key, string value)
        {
            _backingDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _backingDictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _backingDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _backingDictionary.TryGetValue(key, out value);
        }

        public string this[string key]
        {
            get => _backingDictionary[key];
            set => _backingDictionary[key] = value;
        }

        public ICollection<string> Keys => ((IDictionary<string, string>)_backingDictionary).Keys;

        public ICollection<string> Values => ((IDictionary<string, string>)_backingDictionary).Values;
    }
}