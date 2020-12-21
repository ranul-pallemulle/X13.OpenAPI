using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiOAuthFlowsParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiOAuthFlows;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "implicit":
                        element.Implicit = childNode.Value.ParseIntoElement<OpenApiOAuthFlow, OpenApiOAuthFlowParsingStrategy>();
                        break;
                    case "password":
                        element.Password = childNode.Value.ParseIntoElement<OpenApiOAuthFlow, OpenApiOAuthFlowParsingStrategy>();
                        break;
                    case "clientCredentials":
                        element.ClientCredentials = childNode.Value.ParseIntoElement<OpenApiOAuthFlow, OpenApiOAuthFlowParsingStrategy>();
                        break;
                    case "authorizationCode":
                        element.AuthorizationCode = childNode.Value.ParseIntoElement<OpenApiOAuthFlow, OpenApiOAuthFlowParsingStrategy>();
                        break;
                }
            }
        }
    }
}