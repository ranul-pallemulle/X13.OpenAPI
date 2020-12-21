using System;

namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiDateTime : OpenApiPrimitive<DateTimeOffset>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.DateTime; }}
        public OpenApiDateTime(DateTimeOffset value) : base(value)
        {
        }
    }
}