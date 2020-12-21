namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiQueryExpression : OpenApiSourceExpression
    {
        public OpenApiQueryExpression(string token) : base(token)
        {

        }

        public override string Expression => "query." + _value;
    }
}