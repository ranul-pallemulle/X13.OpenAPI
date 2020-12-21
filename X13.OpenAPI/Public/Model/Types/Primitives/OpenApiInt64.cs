namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiInt64 : OpenApiPrimitive<long>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Int64; }}
        public OpenApiInt64(long value) : base(value)
        {
        }
    }
}