using Newtonsoft.Json;

namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public abstract class OpenApiPrimitive<T> : IOpenApiPrimitive
    {
        public abstract OpenApiPrimitiveType PrimitiveType { get; }
        public OpenApiAnyType AnyType { get; }
        [JsonProperty]
        public T Value { get; private set; }

        public OpenApiPrimitive(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
        public string GetString()
        {
            return ToString();
        }
    }
}
