using System;
using System.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiPathsPartialParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiPaths;

            var pathNode = node.childNodes.Where(c => c.Name == _pathName).First().Value; // find correct path in JSON
            pathNode.strategy = new OpenApiPathItemPartialParsingStrategy(_operationName);
            var pathItem = new OpenApiPathItem();
            pathNode.Parse(pathItem); // parse JSON into new PathItem object
            element.Add(_pathName, pathItem);
        }

        private readonly string _pathName;
        private readonly string _operationName;

        public OpenApiPathsPartialParsingStrategy(string pathName, string operationName)
        {
            _pathName = pathName;
            _operationName = operationName;
        }
    }
}