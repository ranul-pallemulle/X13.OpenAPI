namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiRequestExpression : OpenApiRuntimeExpression
    {
        public OpenApiSourceExpression Source;
        public OpenApiRequestExpression(OpenApiSourceExpression source)
        {
            Source = source;
        }
        public override string Expression => "$request." + Source.Expression;
    }
}