using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal class OpenApiTraverser
    {
        public OpenApiVisitor Visitor { get; private set; }
        private readonly Stack<OpenApiSchema> _visitedSchemas;
        private readonly Stack<OpenApiPathItem> _visitedPathItems;
        public OpenApiTraverser(OpenApiVisitor visitor)
        {
            Visitor = visitor;
            _visitedSchemas = new Stack<OpenApiSchema>();
            _visitedPathItems = new Stack<OpenApiPathItem>();
        }

        public void Traverse(OpenApiDocument document)
        {
            if (document == null)
            {
                return;
            }
            _visitedSchemas.Clear();
            _visitedPathItems.Clear();

            Visitor.Visit(document);
            Traverse(document.Info);
            Traverse(document.Servers);
            Traverse(document.Paths);
            Traverse(document.Components);
            Traverse(document.Security);
            Traverse(document.ExternalDocs);
            Traverse(document.Tags);
        }

        public void Traverse(IList<OpenApiTag> tags)
        {
            if (tags == null)
            {
                return;
            }
            foreach (var tag in tags)
            {
                Traverse(tag);
            }
        }
        public void Traverse(OpenApiComponents components)
        {
            if (components == null)
            {
                return;
            }
            Visitor.Visit(components);
            if (components.Schemas != null)
            {
                foreach (var schema in components.Schemas)
                {
                    Traverse(schema.Value);
                }
            }
            if (components.Responses != null)
            {
                foreach (var response in components.Responses)
                {
                    Traverse(response.Value);
                }
            }
            if (components.Parameters != null)
            {
                foreach (var parameter in components.Parameters)
                {
                    Traverse(parameter.Value);
                }
            }
            if (components.Examples != null)
            {
                foreach (var example in components.Examples)
                {
                    Traverse(example.Value);
                }
            }
            if (components.RequestBodies != null)
            {
                foreach (var requestBody in components.RequestBodies)
                {
                    Traverse(requestBody.Value);
                }
            }
            if (components.Headers != null)
            {
                foreach (var header in components.Headers)
                {
                    Traverse(header.Value);
                }
            }
            if (components.SecuritySchemes != null)
            {
                foreach (var scheme in components.SecuritySchemes)
                {
                    Traverse(scheme.Value);
                }
            }
            if (components.Links != null)
            {
                foreach (var link in components.Links)
                {
                    Traverse(link.Value);
                }
            }
            if (components.Callbacks != null)
            {
                foreach (var callback in components.Callbacks)
                {
                    Traverse(callback.Value);
                }
            }
        }

        public void Traverse(OpenApiInfo info)
        {
            if (info == null)
            {
                return;
            }
            Visitor.Visit(info);
            Traverse(info.Contact);
            Traverse(info.License);
        }

        public void Traverse(OpenApiContact contact)
        {
            if (contact == null)
            {
                return;
            }
            Visitor.Visit(contact);
        }

        public void Traverse(OpenApiLicense license)
        {
            if (license == null)
            {
                return;
            }
            Visitor.Visit(license);
        }

        public void Traverse(IList<OpenApiServer> servers)
        {
            if (servers == null)
            {
                return;
            }
            foreach (var server in servers)
            {
                Traverse(server);
            }
        }
        
        public void Traverse(OpenApiServer server)
        {
            if (server == null)
            {
                return;
            }
            Visitor.Visit(server);
            if (server.Variables != null)
            {
                foreach (var variable in server.Variables)
                {
                    Traverse(variable.Value);
                }
            }
        }

        public void Traverse(OpenApiServerVariable variable)
        {
            if (variable == null)
            {
                return;
            }
            Visitor.Visit(variable);
        }

        public void Traverse(OpenApiPaths paths)
        {
            if (paths == null)
            {
                return;
            }
            Visitor.Visit(paths);
            foreach (var pathItem in paths)
            {
                Traverse(pathItem.Value);
            }
        }

        public void Traverse(OpenApiPathItem item)
        {
            if (item == null)
            {
                return;
            }
            if (_visitedPathItems.Contains(item))
            {
                return;
            }
            _visitedPathItems.Push(item);
            Visitor.Visit(item);
            if (item.Operations != null)
            {
                foreach (var operation in item.Operations)
                {
                    Traverse(operation.Value);
                }
            }
            Traverse(item.Servers);
            Traverse(item.Parameters);
            _visitedPathItems.Pop();
        }

        public void Traverse(OpenApiOperation operation)
        {
            if (operation == null)
            {
                return;
            }
            Visitor.Visit(operation);
            Traverse(operation.Parameters);
            Traverse(operation.RequestBody);
            Traverse(operation.Responses);
            if (operation.Callbacks != null)
            {
                foreach (var callback in operation.Callbacks)
                {
                    Traverse(callback.Value);
                }
            }
            Traverse(operation.Security);
            Traverse(operation.Servers);
        }

        public void Traverse(OpenApiCallback callback)
        {
            if (callback == null)
            {
                return;
            }
            Visitor.Visit(callback);
            if (callback.PathItems != null)
            {
                foreach (var item in callback.PathItems)
                {
                    Traverse(item.Value);
                }
            }
        }

        public void Traverse(IList<OpenApiSecurityRequirement> security)
        {
            if (security == null)
            {
                return;
            }
            foreach (var requirement in security)
            {
                Traverse(requirement);
            }
        }

        public void Traverse(OpenApiSecurityRequirement requirement)
        {
            if (requirement == null)
            {
                return;
            }
            Visitor.Visit(requirement);
        }

        public void Traverse(IList<OpenApiParameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }
            foreach (var parameter in parameters)
            {
                Traverse(parameter);
            }
        }

        public void Traverse(OpenApiParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }
            Visitor.Visit(parameter);
            if (parameter.Content != null)
            {
                foreach (var mediaType in parameter.Content)
                {
                    Traverse(mediaType.Value);
                }
            }
            if (parameter.Examples != null)
            {
                foreach (var example in parameter.Examples)
                {
                    Traverse(example.Value);
                }
            }
        }

        public void Traverse(OpenApiRequestBody requestBody)
        {
            if (requestBody == null)
            {
                return;
            }
            Visitor.Visit(requestBody);
            if (requestBody.Content != null)
            {
                foreach (var mediaType in requestBody.Content)
                {
                    Traverse(mediaType.Value);
                }
            }
        }

        public void Traverse(OpenApiResponses responses)
        {
            if (responses == null)
            {
                return;
            }
            Visitor.Visit(responses);
            foreach (var response in responses)
            {
                Traverse(response.Value);
            }
        }

        public void Traverse(OpenApiResponse response)
        {
            if (response == null)
            {
                return;
            }
            Visitor.Visit(response);
            if (response.Headers != null)
            {
                foreach (var header in response.Headers)
                {
                    Traverse(header.Value);
                }
            }
            if (response.Content != null)
            {
                foreach (var mediaType in response.Content)
                {
                    Traverse(mediaType.Value);
                }
            }
            if (response.Links != null)
            {
                foreach (var link in response.Links)
                {
                    Traverse(link.Value);
                }
            }
        }

        public void Traverse(OpenApiHeader header)
        {
            if (header == null)
            {
                return;
            }
            Visitor.Visit(header);
            Traverse(header.Schema);
            if (header.Examples != null)
            {
                foreach (var example in header.Examples)
                {
                    Traverse(example.Value);
                }
            }
            if (header.Content != null)
            {
                foreach (var mediaType in header.Content)
                {
                    Traverse(mediaType.Value);
                }
            }
        }

        public void Traverse(OpenApiMediaType mediaType)
        {
            if (mediaType == null)
            {
                return;
            }
            Visitor.Visit(mediaType);
            Traverse(mediaType.Schema);
            if (mediaType.Examples != null)
            {
                foreach (var example in mediaType.Examples)
                {
                    Traverse(example.Value);
                }
            }
            if (mediaType.Encoding != null)
            {
                foreach (var encoding in mediaType.Encoding)
                {
                    Traverse(encoding.Value);
                }
            }
        }

        public void Traverse(OpenApiLink link)
        {
            if (link != null)
            {
                return;
            }
            Visitor.Visit(link);
            Traverse(link.Server);
        }

        public void Traverse(OpenApiSchema schema)
        {
            if (schema == null)
            {
                return;
            }
            if (_visitedSchemas.Contains(schema))
            {
                return;
            }
            _visitedSchemas.Push(schema);
            Visitor.Visit(schema);
            if (schema.AllOf != null)
            {
                foreach (var subschema in schema.AllOf)
                {
                    Traverse(subschema);
                }
            }
            if (schema.OneOf != null)
            {
                foreach (var subschema in schema.OneOf)
                {
                    Traverse(subschema);
                }
            }
            if (schema.AnyOf != null)
            {
                foreach (var subschema in schema.AnyOf)
                {
                    Traverse(subschema);
                }
            }
            Traverse(schema.Not);
            Traverse(schema.Items);
            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties)
                {
                    Traverse(property.Value);
                }
            }
            Traverse(schema.ExternalDocs);
            Traverse(schema.Xml);
            _visitedSchemas.Pop();
        }
        
        public void Traverse(OpenApiExample example)
        {
            if (example == null)
            {
                return;
            }
            Visitor.Visit(example);
        }

        public void Traverse(OpenApiEncoding encoding)
        {
            if (encoding == null)
            {
                return;
            }
            Visitor.Visit(encoding);
            if (encoding.Headers != null)
            {
                foreach (var header in encoding.Headers)
                {
                    Traverse(header.Value);
                }
            }
        }

        public void Traverse(OpenApiSecurityScheme scheme)
        {
            if (scheme == null)
            {
                return;
            }
            Visitor.Visit(scheme);
            Traverse(scheme.Flows);
        }

        public void Traverse(OpenApiOAuthFlows flows)
        {
            if (flows == null)
            {
                return;
            }
            Visitor.Visit(flows);
            Traverse(flows.Implicit);
            Traverse(flows.Password);
            Traverse(flows.ClientCredentials);
            Traverse(flows.AuthorizationCode);
        }

        public void Traverse(OpenApiOAuthFlow flow)
        {
            if (flow == null)
            {
                return;
            }
            Visitor.Visit(flow);
        }

        public void Traverse(OpenApiExternalDocs docs)
        {
            if (docs == null)
            {
                return;
            }
            Visitor.Visit(docs);
        }

        public void Traverse(OpenApiXml xml)
        {
            if (xml == null)
            {
                return;
            }
            Visitor.Visit(xml);
        }

        public void Traverse(OpenApiTag tag)
        {
            if (tag == null)
            {
                return;
            }
            Visitor.Visit(tag);
            Traverse(tag.ExternalDocs);
        }
    }
}
