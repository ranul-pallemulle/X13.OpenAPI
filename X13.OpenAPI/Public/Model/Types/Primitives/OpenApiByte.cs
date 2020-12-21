namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiByte : OpenApiPrimitive<byte[]>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Byte; }}
        public OpenApiByte(byte[] value) : base(value)
        {
        }
        public OpenApiByte(byte value): this(new byte[] { value })
        {
        }
    }
}