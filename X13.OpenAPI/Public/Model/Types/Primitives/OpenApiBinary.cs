namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiBinary : OpenApiPrimitive<byte[]>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Binary; }}
        public OpenApiBinary(byte[] value) : base(value)
        {
        }
    }
}