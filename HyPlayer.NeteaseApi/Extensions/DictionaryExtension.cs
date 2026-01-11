namespace HyPlayer.NeteaseApi.Extensions;

public static class DictionaryExtension
{
    public static TValue? GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default) where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public static void MergeDictionary<TKey, TValue>(this Dictionary<TKey, TValue?> dictionary,
        Dictionary<TKey, TValue?> other) where TKey : notnull
    {
        foreach (var kvp in other)
        {
            if (kvp.Value is null)
            {
                dictionary.Remove(kvp.Key);
            }
            else
            {
                dictionary[kvp.Key] = kvp.Value;
            }
        }
    }
}