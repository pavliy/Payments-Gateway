using Newtonsoft.Json;

namespace Infrastructure.Serialization;

public class NewtonsoftJsonSerializer : IJsonSerializer
{
    public string Serialize<TValue>(
        TValue value,
        Formatting formatting = Formatting.Indented,
        JsonSerializerSettings? jsonSerializerSettings = default)
    {
        return JsonConvert.SerializeObject(value, formatting);
    }

    public TValue? Deserialize<TValue>(string input, JsonSerializerSettings? serializerSettings = default)
    {
        return JsonConvert.DeserializeObject<TValue>(input, serializerSettings);
    }

    public T? Deserialize<T>(Stream responseStream)
    {
        if (responseStream == null)
        {
            throw new ArgumentNullException(nameof(responseStream));
        }

        var serializer = new JsonSerializer();

        using var sr = new StreamReader(responseStream);
        using var jsonTextReader = new JsonTextReader(sr);

        return serializer.Deserialize<T>(jsonTextReader);
    }
}