using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Writers;

namespace NetCore.WebHost.Models
{
    /// <summary>
    /// swagger Date sample provider
    /// </summary>
    public class DateTimeSampleProvider : Microsoft.OpenApi.Any.IOpenApiPrimitive
    {
        public DateTimeSampleProvider(DateTime value)
        {
            this.Value = value;
        }

        public DateTime Value { get; set; }
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.DateTime;

        public AnyType AnyType { get; } = AnyType.Primitive;

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteValue(Value.ToString("yyyy'/'MM'/'dd'T'HH':'mm':'ss"));
        }
    }
}

