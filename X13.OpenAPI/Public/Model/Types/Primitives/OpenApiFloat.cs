namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiFloat : OpenApiPrimitive<float>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Float; }}
        public OpenApiFloat(float value) : base(value)
        {
        }
    }
}