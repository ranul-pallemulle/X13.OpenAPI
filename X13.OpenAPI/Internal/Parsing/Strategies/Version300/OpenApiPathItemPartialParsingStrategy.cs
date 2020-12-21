using System;
using System.Collections.Generic;
using System.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiPathItemPartialParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiPathItem;
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
                    case "servers":
                        element.Servers = (childNode.Value as ListParsingNode).CreateList<OpenApiServer, OpenApiServerParsingStrategy>();
                        break;
                    case "parameters":
                        element.Parameters = (childNode.Value as ListParsingNode).CreateList<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        break;
                }
            }
            element.Operations = new Dictionary<OpenApiOperationType, OpenApiOperation>();
            var operationNode = node.childNodes.Where(c => c.Name.ToLower() == _operationName.ToLower()).First();
            // parse operation fully
            switch (operationNode.Name)
            {
                case "get":
                    element.Operations.Add(OpenApiOperationType.Get, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "put":
                    element.Operations.Add(OpenApiOperationType.Put, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "post":
                    element.Operations.Add(OpenApiOperationType.Post, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "delete":
                    element.Operations.Add(OpenApiOperationType.Delete, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "options":
                    element.Operations.Add(OpenApiOperationType.Options, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "head":
                    element.Operations.Add(OpenApiOperationType.Head, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "patch":
                    element.Operations.Add(OpenApiOperationType.Patch, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
                case "trace":
                    element.Operations.Add(OpenApiOperationType.Trace, operationNode.Value.ParseIntoElement<OpenApiOperation, OpenApiOperationParsingStrategy>());
                    break;
            }
        }

        private readonly string _operationName;

        public OpenApiPathItemPartialParsingStrategy(string operationName)
        {
            _operationName = operationName;
        }
    }
}