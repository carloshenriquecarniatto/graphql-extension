namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class 
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("tokenGeneratorQuery")]
        public TokenGeneratorQuery TokenGeneratorQuery { get; set; }
    }

    public partial class TokenGeneratorQuery
    {
        [JsonProperty("errorMessages")]
        public object ErrorMessages { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("configurationDetails")]
        public ConfigurationDetails ConfigurationDetails { get; set; }
    }

    public partial class ConfigurationDetails
    {
        [JsonProperty("techincalData")]
        public TechincalData TechincalData { get; set; }
    }

    public partial class TechincalData
    {
        [JsonProperty("fields")]
        public Field[] Fields { get; set; }
    }

    public partial class Field
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public partial class State
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("short")]
        public string Short { get; set; }
    }

    public enum Source { Reda, Scng };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                SourceConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class SourceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Source) || t == typeof(Source?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "REDA":
                    return Source.Reda;
                case "SCNG":
                    return Source.Scng;
            }
            throw new Exception("Cannot unmarshal type Source");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Source)untypedValue;
            switch (value)
            {
                case Source.Reda:
                    serializer.Serialize(writer, "REDA");
                    return;
                case Source.Scng:
                    serializer.Serialize(writer, "SCNG");
                    return;
            }
            throw new Exception("Cannot marshal type Source");
        }

        public static readonly SourceConverter Singleton = new SourceConverter();
    }
}
