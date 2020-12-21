namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiDouble : OpenApiPrimitive<double>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.Double; }}
        public OpenApiDouble(double value) : base(value)
        {
        }
    }
}