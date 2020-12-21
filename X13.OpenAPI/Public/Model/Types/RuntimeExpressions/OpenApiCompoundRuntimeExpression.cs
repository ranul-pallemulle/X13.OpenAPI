namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiCompoundExpression : OpenApiRuntimeExpression
    {
        private string _expression;
        public OpenApiCompoundExpression(string expression)
        {
            _expression = expression;
        }
        public override string Expression => _expression;
    }
}