using System;
using System.Collections.Generic;
using System.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiComponentsPartialParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiComponents;
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Schema))
            {
                AddSchema(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Response))
            {
                AddResponse(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Parameter))
            {
                AddParameter(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Example))
            {
                AddExample(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.RequestBody))
            {
                AddRequestBody(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Header))
            {
                AddHeader(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.SecurityScheme))
            {
                AddSecurityScheme(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Link))
            {
                AddLink(node.childNodes, element, reference);
            }
            foreach (var reference in _unresolvedReferences.Where(r => r.Type == OpenApiReferenceType.Callback))
            {
                AddCallback(node.childNodes, element, reference);
            }
        }

        private void AddSchema(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiSchema schema)
        {
            if (schema == null)
            {
                return;
            }
            if (schema.UnresolvedReference)
            {
                AddSchema(nodes, components, schema.Reference);
                return;
            }
            foreach (var allOf in schema.AllOf)
            {
                AddSchema(nodes, components, allOf);
            }
            foreach (var oneOf in schema.OneOf)
            {
                AddSchema(nodes, components, oneOf);
            }
            foreach (var anyOf in schema.AnyOf)
            {
                AddSchema(nodes, components, anyOf);
            }
            AddSchema(nodes, components, schema.Not);
            AddSchema(nodes, components, schema.Items);
            foreach (var property in schema.Properties)
            {
                AddSchema(nodes, components, property.Value);
            }

        }

        private void AddSchema(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var schemaNodes = nodes.Where(c => c.Name == "schemas").First().Value as ObjectParsingNode;
            var schemaRef = schemaNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Schemas.ContainsKey(schemaRef.Name))
            {
                return;
            }
            var resolvedSchema = schemaRef.Value.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
            components.Schemas.Add(schemaRef.Name, resolvedSchema);
            AddSchema(nodes, components, resolvedSchema); // add all subschemas
        }

        private void AddResponse(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiResponse response)
        {
            if (response == null)
            {
                return;
            }
            if (response.UnresolvedReference)
            {
                AddResponse(nodes, components, response.Reference);
                return;
            }
            foreach (var header in response.Headers)
            {
                AddHeader(nodes, components, header.Value);
            }
            foreach (var content in response.Content)
            {
                AddSchema(nodes, components, content.Value.Schema);
                foreach (var example in content.Value.Examples)
                {
                    if (example.Value.UnresolvedReference)
                    {
                        AddExample(nodes, components, example.Value.Reference);
                    }
                }
            }
            foreach (var link in response.Links)
            {
                if (link.Value.UnresolvedReference)
                {
                    AddLink(nodes, components, link.Value.Reference);
                }
            }
        }

        private void AddResponse(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var responseNodes = nodes.Where(c => c.Name == "responses").First().Value as ObjectParsingNode;
            var responseRef = responseNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Responses.ContainsKey(responseRef.Name))
            {
                return;
            }
            var resolvedResponse = responseRef.Value.ParseIntoElement<OpenApiResponse, OpenApiResponseParsingStrategy>();
            components.Responses.Add(responseRef.Name, resolvedResponse);
            AddResponse(nodes, components, resolvedResponse); // add resolvedResponse.Headers, resolvedResponse.Content, etc

        }

        private void AddParameter(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }
            if (parameter.UnresolvedReference)
            {
                AddParameter(nodes, components, parameter.Reference);
                return;
            }
            AddSchema(nodes, components, parameter.Schema);
            foreach (var content in parameter.Content)
            {
                AddSchema(nodes, components, content.Value.Schema);
                foreach (var example in content.Value.Examples)
                {
                    if (example.Value.UnresolvedReference)
                    {
                        AddExample(nodes, components, example.Value.Reference);
                    }
                }
            }
            foreach (var example in parameter.Examples)
            {
                if (example.Value.UnresolvedReference)
                {
                    AddExample(nodes, components, example.Value.Reference);
                }
            }
        }

        private void AddParameter(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var parameterNodes = nodes.Where(c => c.Name == "parameters").First().Value as ObjectParsingNode;
            var parameterRef = parameterNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Parameters.ContainsKey(parameterRef.Name))
            {
                return;
            }
            var resolvedParameter = parameterRef.Value.ParseIntoElement<OpenApiParameter, OpenApiParameterParsingStrategy>();
            components.Parameters.Add(parameterRef.Name, resolvedParameter);
            AddParameter(nodes, components, resolvedParameter); // add resolvedParameter.Schema, resolvedParameter.Examples, etc
        }

        private void AddExample(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var exampleNodes = nodes.Where(c => c.Name == "examples").First().Value as ObjectParsingNode;
            var exampleRef = exampleNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Examples.ContainsKey(exampleRef.Name))
            {
                return;
            }
            var resolvedExample = exampleRef.Value.ParseIntoElement<OpenApiExample, OpenApiExampleParsingStrategy>();
            components.Examples.Add(exampleRef.Name, resolvedExample);
        }

        private void AddRequestBody(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiRequestBody requestBody)
        {
            if (requestBody == null)
            {
                return;
            }
            if (requestBody.UnresolvedReference)
            {
                AddRequestBody(nodes, components, requestBody.Reference);
                return;
            }
            foreach (var content in requestBody.Content)
            {
                AddSchema(nodes, components, content.Value.Schema);
                foreach (var example in content.Value.Examples)
                {
                    if (example.Value.UnresolvedReference)
                    {
                        AddExample(nodes, components, example.Value.Reference);
                    }
                }
            }
        }

        private void AddRequestBody(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var requestBodyNodes = nodes.Where(c => c.Name == "requestBodies").First().Value as ObjectParsingNode;
            var requestBodyRef = requestBodyNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.RequestBodies.ContainsKey(requestBodyRef.Name))
            {
                return;
            }
            var resolvedRequestBody = requestBodyRef.Value.ParseIntoElement<OpenApiRequestBody, OpenApiRequestBodyParsingStrategy>();
            components.RequestBodies.Add(requestBodyRef.Name, resolvedRequestBody);
            AddRequestBody(nodes, components, resolvedRequestBody); // add resolvedRequestBody.Content, etc
        }

        private void AddHeader(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiHeader header)
        {
            if (header == null)
            {
                return;
            }
            if (header.UnresolvedReference)
            {
                AddHeader(nodes, components, header.Reference);
                return;
            }
            AddSchema(nodes, components, header.Schema);
            foreach (var example in header.Examples)
            {
                if (example.Value.UnresolvedReference)
                {
                    AddExample(nodes, components, example.Value.Reference);
                }
            }
            foreach (var content in header.Content)
            {
                AddSchema(nodes, components, content.Value.Schema);
                foreach (var example in content.Value.Examples)
                {
                    if (example.Value.UnresolvedReference)
                    {
                        AddExample(nodes, components, example.Value.Reference);
                    }
                }
            }
        }

        private void AddHeader(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var headerNodes = nodes.Where(c => c.Name == "headers").First().Value as ObjectParsingNode;
            var headerRef = headerNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Headers.ContainsKey(headerRef.Name))
            {
                return;
            }
            var resolvedHeader = headerRef.Value.ParseIntoElement<OpenApiHeader, OpenApiHeaderParsingStrategy>();
            components.Headers.Add(headerRef.Name, resolvedHeader);
            AddHeader(nodes, components, resolvedHeader); // add resolvedHeader.schema, resolvedHeader.Examples, etc
        }

        private void AddSecurityScheme(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var securitySchemeNodes = nodes.Where(c => c.Name == "securitySchemes").First().Value as ObjectParsingNode;
            var securitySchemaRef = securitySchemeNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.SecuritySchemes.ContainsKey(securitySchemaRef.Name))
            {
                return;
            }
            var resolvedSecurityScheme = securitySchemaRef.Value.ParseIntoElement<OpenApiSecurityScheme, OpenApiSecuritySchemeParsingStrategy>();
            components.SecuritySchemes.Add(securitySchemaRef.Name, resolvedSecurityScheme);
        }

        private void AddLink(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var linkNodes = nodes.Where(c => c.Name == "links").First().Value as ObjectParsingNode;
            var linkRef = linkNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Links.ContainsKey(linkRef.Name))
            {
                return;
            }
            var resolvedLink = linkRef.Value.ParseIntoElement<OpenApiLink, OpenApiLinkParsingStrategy>();
            components.Links.Add(linkRef.Name, resolvedLink);
        }

        private void AddCallback(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiCallback callback)
        {
            if (callback == null)
            {
                return;
            }
            if (callback.UnresolvedReference)
            {
                AddCallback(nodes, components, callback.Reference);
                return;
            }
            foreach (var pathItem in callback.PathItems)
            {
                foreach (var operation in pathItem.Value.Operations)
                {
                    foreach (var parameter in operation.Value.Parameters)
                    {
                        AddParameter(nodes, components, parameter.Reference);
                    }
                    AddRequestBody(nodes, components, operation.Value.RequestBody.Reference);
                    foreach (var response in operation.Value.Responses)
                    {
                        AddResponse(nodes, components, response.Value.Reference);
                    }
                }
                foreach (var parameter in pathItem.Value.Parameters)
                {
                    AddParameter(nodes, components, parameter.Reference);
                }
            }
        }

        private void AddCallback(IList<PropertyParsingNode> nodes, OpenApiComponents components, OpenApiReference unresolved)
        {
            var callbackNodes = nodes.Where(c => c.Name == "callbacks").First().Value as ObjectParsingNode;
            var callbackRef = callbackNodes.childNodes.Where(c => c.Name == unresolved.Id).First();
            if (components.Callbacks.ContainsKey(callbackRef.Name))
            {
                return;
            }
            var resolvedCallback = callbackRef.Value.ParseIntoElement<OpenApiCallback, OpenApiCallbackParsingStrategy>();
            components.Callbacks.Add(callbackRef.Name, resolvedCallback);
            AddCallback(nodes, components, resolvedCallback);

        }

        private readonly List<OpenApiReference> _unresolvedReferences;
        public OpenApiComponentsPartialParsingStrategy(List<OpenApiReference> unresolvedReferences)
        {
            _unresolvedReferences = unresolvedReferences;
        }
    }
}