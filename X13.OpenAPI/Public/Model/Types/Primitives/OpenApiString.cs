namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public class OpenApiString : OpenApiPrimitive<string>
    {
        public override OpenApiPrimitiveType PrimitiveType { get { return OpenApiPrimitiveType.String; }}
        public bool Quoted { get; }
        public OpenApiString(string value, bool quoted = false) : base(value)
        {
            Quoted = quoted;
        }
    }
}