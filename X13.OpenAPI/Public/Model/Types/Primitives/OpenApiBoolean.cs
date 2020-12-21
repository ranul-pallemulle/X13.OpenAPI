namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiBoolean : OpenApiPrimitive<bool>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Boolean; }}
        public OpenApiBoolean(bool value) : base(value)
        {
        }
    }
}