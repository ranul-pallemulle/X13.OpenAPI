using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiServerParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiServer;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "url":
                        element.Url = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "variables":
                        element.Variables = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiServerVariable, OpenApiServerVariableParsingStrategy>();
                        break;
                }
            }
        }
    }
}