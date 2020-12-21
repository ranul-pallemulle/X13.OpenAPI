using System;
using System.Collections.Generic;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiSecurityRequirementParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiSecurityRequirement;
            foreach (var childNode in node.childNodes)
            {
                var requirements = new List<string>();
                foreach (var name in (childNode.Value as ListParsingNode).childNodes)
                {
                    requirements.Add((string)(name as ValueParsingNode).Value);
                }
                element.Add(childNode.Name, requirements);
            }
        }
    }
}