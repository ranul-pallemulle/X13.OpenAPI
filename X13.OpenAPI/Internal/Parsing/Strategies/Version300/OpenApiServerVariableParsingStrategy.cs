using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiServerVariableParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiServerVariable;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "Enum":
                        element.Enum = (childNode.Value as ListParsingNode).CreateList(n => (string)(n as ValueParsingNode).Value);
                        break;
                    case "default":
                        element.Default = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                }
            }
        }
    }
}