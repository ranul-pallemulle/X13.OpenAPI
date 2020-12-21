using System;

namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiSourceExpression : OpenApiRuntimeExpression
    {
        public string _value;
        public OpenApiSourceExpression(string value)
        {
            _value = value;
        }
        public override string Expression => throw new NotSupportedException();
        public static new OpenApiSourceExpression Build(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException();
            }
            var subExps = expression.Split('.');
            if (expression.StartsWith("header."))
            {
                return new OpenApiHeaderExpression(subExps[1]);
            }
            if (expression.StartsWith("query."))
            {
                return new OpenApiQueryExpression(subExps[1]);
            }
            if (expression.StartsWith("path."))
            {
                return new OpenApiPathExpression(subExps[1]);
            }
            if (expression.StartsWith("body"))
            {
                var jsonPointer = expression.Split(new [] {"body"}, 2, StringSplitOptions.None)[1] ?? default(string);
                return new OpenApiBodyExpression(new OpenApiJsonPointer(jsonPointer));
            }
            throw new ArgumentException();
        }
    }
}