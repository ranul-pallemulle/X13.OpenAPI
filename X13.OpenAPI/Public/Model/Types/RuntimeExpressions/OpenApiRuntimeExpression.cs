using System;

namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public abstract class OpenApiRuntimeExpression
    {
        public const string Prefix = "$";
        public abstract string Expression { get; }

        public override string ToString()
        {
            return Expression;
        }

        public static OpenApiRuntimeExpression Build(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException();
            }
            if (expression == "$url")
            {
                return new OpenApiUrlExpression();
            }
            if (expression == "$method")
            {
                return new OpenApiMethodExpression();
            }
            if (expression == "$statusCode")
            {
                return new OpenApiStatusCodeExpression();
            }
            if (expression.StartsWith("$request."))
            {
                var sourceExp = OpenApiSourceExpression.Build(expression.Split(new [] {"$request."}, 2, StringSplitOptions.None)[1]);
                return new OpenApiRequestExpression(sourceExp);
            }
            if (expression.StartsWith("$response."))
            {
                var sourceExp = OpenApiSourceExpression.Build(expression.Split(new [] {"$response."}, 2, StringSplitOptions.None)[1]);
                return new OpenApiResponseExpression(sourceExp);
            }
            else // compound expression
            {
                return new OpenApiCompoundExpression(expression);
            }
        }
    }
}