namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiInt32 : OpenApiPrimitive<int>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Int32; }}
        public OpenApiInt32(int value) : base(value)
        {
        }
    }
}