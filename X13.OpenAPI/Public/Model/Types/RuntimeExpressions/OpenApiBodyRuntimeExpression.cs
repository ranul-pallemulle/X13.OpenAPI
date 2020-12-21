namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiBodyExpression : OpenApiSourceExpression
    {
        public OpenApiBodyExpression(OpenApiJsonPointer jsonPointer) : base(jsonPointer?.ToString())
        {
            JPointer = jsonPointer;
        }

        public override string Expression => "body#" + _value;
        public string Path => _value;
        public OpenApiJsonPointer JPointer { get; set; }
    }
}