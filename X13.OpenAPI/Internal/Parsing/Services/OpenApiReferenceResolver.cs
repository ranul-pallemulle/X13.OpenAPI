using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal class OpenApiReferenceResolver : OpenApiVisitor
    {
        private OpenApiDocument Document { get; set; }
        public OpenApiReferenceResolver(OpenApiDocument document)
        {
            Document = document;
        }
        public override void Visit(OpenApiDocument document)
        {
            if (document.Tags != null)
            {
                ResolveTags(document.Tags);
            }
        }
        public override void Visit(OpenApiPathItem pathItem)
        {
            ResolveList(pathItem.Parameters);
        }

        public override void Visit(OpenApiOperation operation)
        {
            ResolveList(operation.Parameters);
            var requestBody = ResolveElement(operation.RequestBody);
            if (requestBody != null)
            {
                operation.RequestBody = requestBody as OpenApiRequestBody;
            }
            ResolveDict(operation.Responses);
            ResolveDict(operation.Callbacks);
        }


        public override void Visit(OpenApiParameter parameter)
        {
            if (parameter.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
            var schema = ResolveElement(parameter.Schema);
            if (schema != null)
            {
                parameter.Schema = schema as OpenApiSchema;
            }
            ResolveDict(parameter.Examples);
        }

        public override void Visit(OpenApiRequestBody requestBody)
        {
            if (requestBody.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
        }

        public override void Visit(OpenApiMediaType mediaType)
        {
            var schema = ResolveElement(mediaType.Schema);
            if (schema != null)
            {
                mediaType.Schema = schema as OpenApiSchema;
            }
            ResolveDict(mediaType.Examples);
        }

        public override void Visit(OpenApiResponses responses)
        {
            ResolveDict(responses);
        }

        public override void Visit(OpenApiResponse response)
        {
            if (response.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
            ResolveDict(response.Headers);
            ResolveDict(response.Links);
        }

        public override void Visit(OpenApiHeader header)
        {
            if (header.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
            var schema = ResolveElement(header.Schema);
            if (schema != null)
            {
                header.Schema = schema as OpenApiSchema;
            }
            ResolveDict(header.Examples);
        }
        
        public override void Visit(OpenApiLink link)
        {
            if (link.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
        }

        public override void Visit(OpenApiCallback callback)
        {
            if (callback.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
        }

        public override void Visit(OpenApiComponents components)
        {
            ResolveDict(components.Callbacks);
            ResolveDict(components.Examples);
            ResolveDict(components.Headers);
            ResolveDict(components.Links);
            ResolveDict(components.Parameters);
            ResolveDict(components.RequestBodies);
            ResolveDict(components.Responses);
            ResolveDict(components.Schemas);
            ResolveDict(components.SecuritySchemes);
        }

        public override void Visit(OpenApiSchema schema)
        {
            if (schema.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
            ResolveList(schema.AllOf);
            ResolveList(schema.OneOf);
            ResolveList(schema.AnyOf);
            var not = ResolveElement(schema.Not);
            if (not != null)
            {
                schema.Not = not as OpenApiSchema;
            }
            var items = ResolveElement(schema.Items);
            if (items != null)
            {
                schema.Items = items as OpenApiSchema;
            }
            ResolveDict(schema.Properties);
        }

        public override void Visit(OpenApiExample example)
        {
            if (example.UnresolvedReference)
            {
                throw new ArgumentException("Single pass reference resolution expects this element to already be resolved");
            }
        }

        private T ResolveElement<T>(T obj) where T : class, IOpenApiReferenceable, new()
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.UnresolvedReference)
            {
                var resolved = Document.ResolveReference(obj.Reference);
                return resolved as T;
            }
            return null;
        }

        private void ResolveDict<T>(IDictionary<string, T> obj) where T : class, IOpenApiReferenceable, new()
        {
            if (obj == null)
            {
                return;
            }
            foreach (var key in obj.Keys.ToList())
            {
                var resolved = ResolveElement(obj[key]);
                if (resolved != null)
                {
                    obj[key] = resolved as T;
                }
            }
        }

        private void ResolveList<T>(IList<T> list) where T : class, IOpenApiReferenceable, new()
        {
            if (list == null)
            {
                return;
            }
            for (int i = 0; i < list.Count; ++i)
            {
                var resolved = ResolveElement(list[i]);
                if (resolved != null)
                {
                    list[i] = resolved as T;
                }
            }
        }

        private void ResolveTags(IList<OpenApiTag> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                var resolved = ResolveElement(tags[i]);
                if (resolved != null)
                {
                    tags[i] = resolved as OpenApiTag;
                }
            }
        }
    }
}
