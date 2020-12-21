namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiPathExpression : OpenApiSourceExpression
    {
        public OpenApiPathExpression(string token) : base(token)
        {

        }

        public override string Expression => "path." + _value;
    }
}