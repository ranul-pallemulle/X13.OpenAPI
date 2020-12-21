using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiOAuthFlowParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiOAuthFlow;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "authorizationUrl":
                        element.AuthorizationUrl = childNode.GetSimpleValue<string>();
                        break;
                    case "tokenUrl":
                        element.TokenUrl = childNode.GetSimpleValue<string>();
                        break;
                    case "refreshUrl":
                        element.RefreshUrl = childNode.GetSimpleValue<string>();
                        break;
                    case "scopes":
                        element.Scopes = (childNode.Value as ObjectParsingNode).CreateObject(n => (string)(n as ValueParsingNode).Value);
                        break;
                }
            }
        }
    }
}