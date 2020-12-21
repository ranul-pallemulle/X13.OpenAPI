using System;
using System.Collections.Generic;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiPathItemParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiPathItem;
            element.Operations = new Dictionary<OpenApiOperationType, OpenApiOperation>();
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "summary":
                        element.Summary = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "get":
                        element.Operations.Add(OpenApiOperationType.Get, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "put":
                        element.Operations.Add(OpenApiOperationType.Put, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "post":
                        element.Operations.Add(OpenApiOperationType.Post, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "delete":
                        element.Operations.Add(OpenApiOperationType.Delete, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "options":
                        element.Operations.Add(OpenApiOperationType.Options, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "head":
                        element.Operations.Add(OpenApiOperationType.Head, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "patch":
                        element.Operations.Add(OpenApiOperationType.Patch, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "trace":
                        element.Operations.Add(OpenApiOperationType.Trace, childNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                        break;
                    case "servers":
                        element.Servers = (childNode.Value as ListParsingNode).CreateList<OpenApiServer, OpenApiServerParsingStrategy>();
                        break;
                    case "parameters":
                        element.Parameters = (childNode.Value as ListParsingNode).CreateList<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        break;
                }
            }
        }
    }
}