using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiResponseParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiResponse;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "schema":
                        element.Content = new Dictionary<string, OpenApiMediaType>();
                        var produces = node.storage.Retrieve<IList<string>>("operation.produces");
                        if (produces == null)
                        {
                            produces = node.storage.Retrieve<IList<string>>("root.produces");
                        }
                        if (produces == null)
                        {
                            produces = new List<string> { "application/json" };
                        }
                        var schema = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        foreach (var mediaType in produces)
                        {
                            element.Content.Add(mediaType, new OpenApiMediaType
                            {
                                Schema = schema
                            });
                        }
                        break;
                    case "headers":
                        element.Headers = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiHeader, OpenApiHeaderParsingStrategy>();
                        break;
                    case "examples":
                        break;
                }
            }
        }
    }
}
