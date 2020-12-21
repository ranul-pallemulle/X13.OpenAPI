namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiPassword : OpenApiPrimitive<string>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Password; }}
        public OpenApiPassword(string value) : base(value)
        {
        }
    }
}