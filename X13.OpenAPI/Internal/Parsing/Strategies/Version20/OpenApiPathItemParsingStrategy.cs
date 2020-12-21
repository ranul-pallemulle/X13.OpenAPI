using System;
using System.Collections.Generic;
using System.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiPathItemParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            ClearStorage(node);
            var element = parsingElement as OpenApiPathItem;
            element.Operations = new Dictionary<OpenApiOperationType, OpenApiOperation>();
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
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
                    case "parameters":
                        var paramList = (childNode.Value as ListParsingNode).CreateList<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        node.storage.Store("pathItem.parameters", paramList);
                        break;
                }
            }

            var parameters = node.storage.Retrieve<IList<OpenApiParameter>>("pathItem.parameters");
            if (parameters != null)
            {
                var consumes = node.storage.Retrieve<IList<string>>("root.consumes");
                if (consumes == null)
                {
                    consumes = new List<string> { "application/json" };
                }
                foreach (var parameter in parameters)
                {
                    if (parameter.In == OpenApiParameterLocation.Body || parameter.In == OpenApiParameterLocation.FormData)
                    {
                        foreach (var (_,operation) in element.Operations.Select(o => (o.Key, o.Value)))
                        {
                            if (operation.RequestBody == null)
                            {
                                operation.RequestBody = new OpenApiRequestBody
                                {
                                    Content = new Dictionary<string, OpenApiMediaType>()
                                };
                            }
                            foreach (var mediaType in consumes)
                            {
                                if (!operation.RequestBody.Content.ContainsKey(mediaType))
                                {
                                    operation.RequestBody.Content.Add(mediaType, new OpenApiMediaType
                                    {
                                        Schema = parameter.Schema
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        element.Parameters.Add(parameter);
                    }
                }
            }
            ClearStorage(node);
        }

        private void ClearStorage(ParsingNode node)
        {
            node.storage.Remove("pathItem.parameters");
        }
    }
}
