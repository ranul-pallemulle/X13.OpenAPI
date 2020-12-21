using System;
using System.Collections.Generic;
using System.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiDocumentPartialParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }

            var element = parsingElement as OpenApiDocument;

            var pathsNode = node.childNodes.Where(c => c.Name == "paths").First().Value;
            pathsNode.strategy = new OpenApiPathsPartialParsingStrategy(_pathName, _operationName);
            element.Paths = new OpenApiPaths();
            pathsNode.Parse(element.Paths);

            // collect unresolved references
            _unresolvedReferences = new List<OpenApiReference>();
            var path = element.Paths.First().Value; // parsed only a single path
            var operation = path.Operations.First().Value; // parsed only a single operation
            CollectOperationReferences(operation);
            foreach (var parameter in path.Parameters)
            {
                CollectParameterReferences(parameter); // path parameters are referenceable
            }

            if (_unresolvedReferences.Count > 0)
            {
                // build components to contain only references from parsed pathItem/operation
                var componentsNode = node.childNodes.Where(c => c.Name == "components").First().Value;
                componentsNode.strategy = new OpenApiComponentsPartialParsingStrategy(_unresolvedReferences);
                element.Components = new OpenApiComponents();
                componentsNode.Parse(element.Components);
            }
        }

        private void CollectOperationReferences(OpenApiOperation operation)
        {
            foreach (var parameter in operation.Parameters)
            {
                CollectParameterReferences(parameter);
            }
            CollectRequestBodyReferences(operation.RequestBody);
            foreach (var response in operation.Responses)
            {
                CollectResponseReferences(response.Value);
            }
            foreach (var callback in operation.Callbacks)
            {
                CollectCallbackReferences(callback.Value);
            }
        }

        private void CollectMediaTypeReferences(OpenApiMediaType mediaType)
        {
            if (mediaType == null)
            {
                return;
            }
            CollectSchemaReferences(mediaType.Schema);
            foreach (var example in mediaType.Examples)
            {
                if (example.Value.UnresolvedReference && !_unresolvedReferences.Contains(example.Value.Reference))
                {
                    _unresolvedReferences.Add(example.Value.Reference);
                }
            }
        }

        private void CollectParameterReferences(OpenApiParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }
            if (parameter.UnresolvedReference)
            {
                if (!_unresolvedReferences.Contains(parameter.Reference))
                {
                    _unresolvedReferences.Add(parameter.Reference);
                }
                return;
            }
            CollectSchemaReferences(parameter.Schema);
            foreach (var content in parameter.Content)
            {
                CollectMediaTypeReferences(content.Value);
            }
            foreach (var example in parameter.Examples)
            {
                if (example.Value.UnresolvedReference && !_unresolvedReferences.Contains(example.Value.Reference))
                {
                    _unresolvedReferences.Add(example.Value.Reference);
                }
            }
        }

        private void CollectRequestBodyReferences(OpenApiRequestBody requestBody)
        {
            if (requestBody == null)
            {
                return;
            }
            if (requestBody.UnresolvedReference)
            {
                if (!_unresolvedReferences.Contains(requestBody.Reference))
                {
                    _unresolvedReferences.Add(requestBody.Reference);
                }
                return;
            }
            foreach (var content in requestBody.Content)
            {
                CollectMediaTypeReferences(content.Value);
            }
        }

        private void CollectResponseReferences(OpenApiResponse response)
        {
            if (response == null)
            {
                return;
            }
            if (response.UnresolvedReference)
            {
                if (!_unresolvedReferences.Contains(response.Reference))
                {
                    _unresolvedReferences.Add(response.Reference);
                }
                return;
            }
            foreach (var header in response.Headers)
            {
                CollectHeaderReferences(header.Value);
            }
            foreach (var content in response.Content)
            {
                CollectMediaTypeReferences(content.Value);
            }
            foreach (var link in response.Links)
            {
                if (link.Value.UnresolvedReference && !_unresolvedReferences.Contains(link.Value.Reference))
                {
                    _unresolvedReferences.Add(link.Value.Reference);
                }
            }
        }

        private void CollectCallbackReferences(OpenApiCallback callback)
        {
            if (callback == null)
            {
                return;
            }
            if (callback.UnresolvedReference)
            {
                if (!_unresolvedReferences.Contains(callback.Reference))
                {
                    _unresolvedReferences.Add(callback.Reference);
                }
                return;
            }
            foreach (var pathItem in callback.PathItems)
            {
                foreach (var operation in pathItem.Value.Operations)
                {
                    CollectOperationReferences(operation.Value);
                }
                foreach (var parameter in pathItem.Value.Parameters)
                {
                    CollectParameterReferences(parameter);
                }
            }
        }

        private void CollectHeaderReferences(OpenApiHeader header)
        {
            if (header == null)
            {
                return;
            }
            if (header.UnresolvedReference)
            {
                if (!_unresolvedReferences.Contains(header.Reference))
                {
                    _unresolvedReferences.Add(header.Reference);
                }
                return;
            }
            CollectSchemaReferences(header.Schema);
            foreach (var content in header.Content)
            {
                CollectMediaTypeReferences(content.Value);
            }
            foreach (var example in header.Examples)
            {
                if (example.Value.UnresolvedReference && !_unresolvedReferences.Contains(example.Value.Reference))
                {
                    _unresolvedReferences.Add(example.Value.Reference);
                }
            }
        }

        private void CollectSchemaReferences(OpenApiSchema schema)
        {
            if (schema == null)
            {
                return;
            }
            if (schema.UnresolvedReference)
            {
                if (!_unresolvedReferences.Contains(schema.Reference))
                {
                    _unresolvedReferences.Add(schema.Reference);
                }
                return;
            }
            foreach (var subSchema in schema.AllOf)
            {
                CollectSchemaReferences(subSchema);
            }
            foreach (var subSchema in schema.OneOf)
            {
                CollectSchemaReferences(subSchema);
            }
            foreach (var subSchema in schema.AnyOf)
            {
                CollectSchemaReferences(subSchema);
            }
            CollectSchemaReferences(schema.Not);
            CollectSchemaReferences(schema.Items);
            foreach (var property in schema.Properties)
            {
                CollectSchemaReferences(property.Value);
            }
        }

        private readonly string _pathName;
        private readonly string _operationName;
        private List<OpenApiReference> _unresolvedReferences;
        public OpenApiDocumentPartialParsingStrategy(string pathName, string operationName)
        {
            _pathName = pathName;
            _operationName = operationName;
        }
    }
}