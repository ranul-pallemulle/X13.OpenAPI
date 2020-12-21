namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiHeaderExpression : OpenApiSourceExpression
    {
        public OpenApiHeaderExpression(string token) : base(token)
        {

        }

        public override string Expression => "header." + _value;
    }
}