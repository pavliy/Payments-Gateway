using Newtonsoft.Json;

namespace Infrastructure.Serialization;

public interface IJsonSerializer
{
    T? Deserialize<T>(Stream responseStream);

    TValue? Deserialize<TValue>(string input, JsonSerializerSettings? serializerSettings = default);

    string Serialize<TValue>(
        TValue value,
        Formatting formatting = Formatting.None,
        JsonSerializerSettings? serializerSettings = default);
}