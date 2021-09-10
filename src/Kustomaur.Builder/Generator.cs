using System.Text.Json;
using Kustomaur.Models.Filters;
using Kustomaur.Models.Serialisation;

namespace Kustomaur.Dashboard
{
    public static class Generator
    {
        public static string Generate(Models.Dashboard dashboard)
        {
            return JsonSerializer.Serialize(dashboard, GetSerializerOptions());
        }

        public static JsonSerializerOptions GetSerializerOptions()
        {
            var serializerOptions = Models.Generator.JsonSerializerOptions;
            serializerOptions.Converters.Add(new JsonStringEnumConverterEx<MsPortalFxTimeRangeModelRelative>());
            serializerOptions.Converters.Add(new JsonStringEnumConverterEx<MsPortalFxTimeRangeModelGranularity>());
            serializerOptions.Converters.Add(new JsonStringEnumConverterEx<MsPortalFxTimeRangeModelFormat>());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());
            return serializerOptions;
        }
    }
}
