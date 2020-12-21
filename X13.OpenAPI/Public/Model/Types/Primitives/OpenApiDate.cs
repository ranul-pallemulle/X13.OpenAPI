using System;

namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiDate : OpenApiPrimitive<DateTime>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Date; }}
        public OpenApiDate(DateTime value) : base(value)
        {
        }
    }
}