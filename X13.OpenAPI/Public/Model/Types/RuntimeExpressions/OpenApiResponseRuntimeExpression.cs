namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiResponseExpression : OpenApiRuntimeExpression
    {
        public OpenApiSourceExpression Source;
        public OpenApiResponseExpression(OpenApiSourceExpression source)
        {
            Source = source;
        }
        public override string Expression => "$response." + Source.Expression;
    }
}