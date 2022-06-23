using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartCities.API.Gira.Data
{
    public partial class GiraStations
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("totalFeatures", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalFeatures { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<Feature> Features { get; set; }

        [JsonProperty("crs", NullValueHandling = NullValueHandling.Ignore)]
        public Crs Crs { get; set; }

        [JsonProperty("bbox", NullValueHandling = NullValueHandling.Ignore)]
        public List<float> Bbox { get; set; }
    }

    public partial class Crs
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public CrsProperties Properties { get; set; }
    }

    public partial class CrsProperties
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public FeatureType? Type { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public Geometry Geometry { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public FeatureProperties Properties { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public GeometryType? Type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<float>> Coordinates { get; set; }
    }

    public partial class FeatureProperties
    {
        [JsonProperty("id_expl", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? IdExpl { get; set; }

        [JsonProperty("id_planeamento", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? IdPlaneamento { get; set; }

        [JsonProperty("desig_comercial", NullValueHandling = NullValueHandling.Ignore)]
        public string DesigComercial { get; set; }

        [JsonProperty("tipo_servico_niveis", NullValueHandling = NullValueHandling.Ignore)]
        public TipoServicoNiveis? TipoServicoNiveis { get; set; }

        [JsonProperty("num_bicicletas", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumBicicletas { get; set; }

        [JsonProperty("num_docas", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumDocas { get; set; }

        [JsonProperty("racio", NullValueHandling = NullValueHandling.Ignore)]
        public float? Racio { get; set; }

        [JsonProperty("estado", NullValueHandling = NullValueHandling.Ignore)]
        public Estado? Estado { get; set; }

        [JsonProperty("update_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? UpdateDate { get; set; }

        [JsonProperty("bbox", NullValueHandling = NullValueHandling.Ignore)]
        public List<float> Bbox { get; set; }
    }

    public enum GeometryType { MultiPoint };

    public enum Estado { Active, Repair };

    public enum TipoServicoNiveis { A, B, Empty };

    public enum FeatureType { Feature };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                GeometryTypeConverter.Singleton,
                EstadoConverter.Singleton,
                TipoServicoNiveisConverter.Singleton,
                FeatureTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class GeometryTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(GeometryType) || t == typeof(GeometryType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "MultiPoint")
            {
                return GeometryType.MultiPoint;
            }
            throw new Exception("Cannot unmarshal type GeometryType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (GeometryType)untypedValue;
            if (value == GeometryType.MultiPoint)
            {
                serializer.Serialize(writer, "MultiPoint");
                return;
            }
            throw new Exception("Cannot marshal type GeometryType");
        }

        public static readonly GeometryTypeConverter Singleton = new GeometryTypeConverter();
    }

    internal class EstadoConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Estado) || t == typeof(Estado?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "active":
                    return Estado.Active;
                case "repair":
                    return Estado.Repair;
            }
            throw new Exception("Cannot unmarshal type Estado");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Estado)untypedValue;
            switch (value)
            {
                case Estado.Active:
                    serializer.Serialize(writer, "active");
                    return;
                case Estado.Repair:
                    serializer.Serialize(writer, "repair");
                    return;
            }
            throw new Exception("Cannot marshal type Estado");
        }

        public static readonly EstadoConverter Singleton = new EstadoConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class TipoServicoNiveisConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TipoServicoNiveis) || t == typeof(TipoServicoNiveis?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return TipoServicoNiveis.Empty;
                case "A":
                    return TipoServicoNiveis.A;
                case "B":
                    return TipoServicoNiveis.B;
            }
            throw new Exception("Cannot unmarshal type TipoServicoNiveis");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TipoServicoNiveis)untypedValue;
            switch (value)
            {
                case TipoServicoNiveis.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case TipoServicoNiveis.A:
                    serializer.Serialize(writer, "A");
                    return;
                case TipoServicoNiveis.B:
                    serializer.Serialize(writer, "B");
                    return;
            }
            throw new Exception("Cannot marshal type TipoServicoNiveis");
        }

        public static readonly TipoServicoNiveisConverter Singleton = new TipoServicoNiveisConverter();
    }

    internal class FeatureTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FeatureType) || t == typeof(FeatureType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Feature")
            {
                return FeatureType.Feature;
            }
            throw new Exception("Cannot unmarshal type FeatureType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FeatureType)untypedValue;
            if (value == FeatureType.Feature)
            {
                serializer.Serialize(writer, "Feature");
                return;
            }
            throw new Exception("Cannot marshal type FeatureType");
        }

        public static readonly FeatureTypeConverter Singleton = new FeatureTypeConverter();
    }
}
