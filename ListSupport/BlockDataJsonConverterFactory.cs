using EPiServer.Shell.Json;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace listtest.ListSupport
{
    public class BlockDataJsonConverterFactory : JsonConverterFactory, IJsonConverter
    {
        private JsonConverter<BlockData> _defaultBlockConverter;
        private ConcurrentDictionary<Type, JsonConverter> _blockConverters = new();

        public override bool CanConvert(Type typeToConvert) => typeof(BlockData).IsAssignableFrom(typeToConvert);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return _blockConverters.GetOrAdd(typeToConvert, t =>
            {
                if (_defaultBlockConverter is null)
                {
                    _defaultBlockConverter = (JsonConverter<BlockData>)options.Converters.FirstOrDefault(c => c.CanConvert(typeToConvert) && c.GetType() != GetType());
                }
                var concreteType = typeof(SpecificBlockDataConverter<>).MakeGenericType(typeToConvert);
                return Activator.CreateInstance(concreteType, _defaultBlockConverter) as JsonConverter;
            });
        }
    }

    internal class SpecificBlockDataConverter<T> : JsonConverter<T> where T : BlockData
    {
        private readonly JsonConverter<BlockData> _defaultConverter;

        public SpecificBlockDataConverter(JsonConverter<BlockData> defaultConverter)
            :base()
        {
            _defaultConverter = defaultConverter;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => (T)_defaultConverter.Read(ref reader, typeToConvert, options);

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => _defaultConverter.Write(writer, value, options);
    }
}
