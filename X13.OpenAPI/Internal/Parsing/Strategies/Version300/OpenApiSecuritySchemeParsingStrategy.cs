using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiSecuritySchemeParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiSecurityScheme;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "type":
                        element.Type = (OpenApiSecuritySchemeType)Enum.Parse(typeof(OpenApiSecuritySchemeType), childNode.GetSimpleValue<string>(), true);
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "name":
                        element.Name = childNode.GetSimpleValue<string>();
                        break;
                    case "in":
                        element.In = (OpenApiParameterLocation)Enum.Parse(typeof(OpenApiParameterLocation), childNode.GetSimpleValue<string>(), true);
                        break;
                    case "scheme":
                        element.Scheme = childNode.GetSimpleValue<string>();
                        break;
                    case "bearerFormat":
                        element.BearerFormat = childNode.GetSimpleValue<string>();
                        break;
                    case "flows":
                        element.Flows = childNode.Value.ParseIntoElement<OpenApiOAuthFlows, OpenApiOAuthFlowsParsingStrategy>();
                        break;
                    case "openIdConnectUrl":
                        element.OpenIdConnectUrl = childNode.GetSimpleValue<string>();
                        break;
                }
            }
        }
    }
}