using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model.Types;
using X13.OpenAPI.Public.Model.Types.Primitives;

namespace X13.OpenAPI.Internal.Parsing.Context
{
    internal abstract class ParsingNode
    {
        public IOpenApiParsingStrategy strategy { get; set; } // strategy to use for parsing this.json into the OpenAPI model
        public IOpenApiVersionParser versionParser { get; set; } // OpenAPI version-specific service
        public ContextStorage storage { get; set; } // temporary storage for the tree - for use during parsing
        public JToken json { get; set; } // JToken containing the json for this node

        protected ParsingNode(JToken json, IOpenApiVersionParser versionParser, ContextStorage storage)
        {
            this.json = json; 
            this.versionParser = versionParser; 
            this.storage = storage; 
        }

        /// <summary>
        /// Parse the contents of this node into an OpenApiElement
        /// </summary>
        /// <param name="openApiElement"> Element to parse content into. </param>
        public virtual void Parse(IOpenApiElement openApiElement)
        {
            if (strategy == null)
            {
                throw new Exception("Strategy to parse node unspecified.");
            }
            strategy.Parse(this, openApiElement);
        }

        /// <summary>
        /// Generate a tree of nodes. To be called by a OpenAPI version-specific parsing service.
        /// </summary>
        /// <param name="json"> JToken representing the OpenAPI json. </param>
        /// <param name="versionParser"> Version-specific parsing service. </param>
        /// <param name="storage"> Temporary storage to use in the parsing of the tree. </param>
        /// <returns></returns>
        public static ParsingNode Create(JToken json, IOpenApiVersionParser versionParser, ContextStorage storage = null)
        {
            if (json is JObject)
            {
                return new ObjectParsingNode(json as JObject, versionParser, storage);
            }
            else if (json is JArray)
            {
                return new ListParsingNode(json as JArray, versionParser, storage);
            }
            else if (json is JProperty)
            {
                return new PropertyParsingNode(json as JProperty, versionParser, storage);
            }
            else if (json is JValue)
            {
                return new ValueParsingNode(json as JValue, versionParser, storage);
            }
            throw new ArgumentException("Invalid OpenAPI/Swagger file - unsupported JToken type found.");
        }

        public T ParseIntoElement<T, X>() 
            where T : IOpenApiElement, new()
            where X : IOpenApiParsingStrategy, new()
        {
            var element = new T();
            this.strategy = new X();
            this.Parse(element);
            return element;
        }

        public T ParseIntoExtension<T, X>()
            where T : IOpenApiExtension, new()
            where X : IOpenApiExtensionParsingStrategy, new()
        {
            var extension = new T();
            var strategy = new X();
            strategy.Parse(this, extension);
            return extension;
        }

        public abstract IOpenApiAny ParseIntoAny();
    }

    internal class ObjectParsingNode : ParsingNode
    {
        public IList<PropertyParsingNode> childNodes { get; set; } = new List<PropertyParsingNode>(); // child properties

        public ObjectParsingNode(JObject json, IOpenApiVersionParser versionParser, ContextStorage storage) : base(json, versionParser, storage)
        {
            foreach(var property in json.Properties())
            {
                childNodes.Add(Create(property, versionParser, storage) as PropertyParsingNode); // add child elements to the node tree
            }
        }
        /// <summary>
        /// If element to parse into has a reference link, find the reference before parsing it.
        /// </summary>
        /// <param name="openApiElement"> Element to parse into. </param>
        public override void Parse(IOpenApiElement openApiElement)
        {
            if (openApiElement is IOpenApiReferenceable referenceable)
            {
                var refLink = GetRefLink();
                if (refLink != null)
                {
                    referenceable.UnresolvedReference = true;
                    referenceable.Reference = this.versionParser.CreateReference(referenceable.ReferenceType, refLink);
                }
            }
            base.Parse(openApiElement);
        }

        /// <summary>
        /// Parse child nodes of this node (must be of the same type) into a dictionary of elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse"> Function that takes a child node, parses it using some strategy and returns the parsed element. </param>
        /// <returns> Dictionary of elements constructed from children of this node. </returns>
        public IDictionary<string, T> CreateObject<T>(Func<ParsingNode, T> parse)
        {
            var result = new Dictionary<string, T>();
            foreach (var childNode in childNodes)
            {
                result.Add(childNode.Name, parse(childNode.Value));
            }
            return result;
        }

        public IDictionary<string, T> CreateObject<T, X>() 
            where T : IOpenApiElement, new() 
            where X : IOpenApiParsingStrategy, new()
        {
            var result = new Dictionary<string, T>();
            foreach (var childNode in childNodes)
            {
                result.Add(childNode.Name, childNode.Value.ParseIntoElement<T, X>());
            }
            return result;
        }

        public ParsingNode GetChildByName(string name)
        {
            foreach (var childNode in childNodes)
            {
                if (childNode.Name == name)
                {
                    return childNode.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Check if this node contains a json reference and return it.
        /// </summary>
        /// <returns> json pointer if $ref exists, otherwise null. </returns>
        public string GetRefLink()
        {
            var refToken = this.json.SelectToken("$ref");
            if (refToken != null)
            {
                return (string)(refToken as JValue).Value;
            }
            return null;
        }

        public override IOpenApiAny ParseIntoAny()
        {
            var obj = new OpenApiObject();
            foreach (var node in childNodes)
            {
                obj.Add(node.Name, node.Value.ParseIntoAny());
            }
            return obj;
        }
    }

    /// <summary>
    /// A list node in the OpenAPI/swagger structure.
    /// </summary>
    internal class ListParsingNode : ParsingNode
    {
        public IList<ParsingNode> childNodes { get; set; } = new List<ParsingNode>(); // child tokens
        public ListParsingNode(JArray json, IOpenApiVersionParser versionParser, ContextStorage storage) : base(json, versionParser, storage)
        {
            foreach (var token in json)
            {
                childNodes.Add(Create(token, versionParser, storage)); // add child elements to the node tree
            }
        }

        /// <summary>
        /// Parse child nodes of this node (must be of the same type) into a list of elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse"> Function that takes a child node, parses it using some strategy and returns the parsed element. </param>
        /// <returns> List of elements constructed from the child elements of this node. </returns>
        public IList<T> CreateList<T>(Func<ParsingNode,T> parse)
        {
            var result = new List<T>();
            foreach (var childNode in childNodes)
            {
                result.Add(parse(childNode));
            }
            return result;
        }

        public IList<T> CreateList<T, X>()
            where T : IOpenApiElement, new()
            where X : IOpenApiParsingStrategy, new()
        {
            var result = new List<T>();
            foreach (var childNode in childNodes)
            {
                result.Add(childNode.ParseIntoElement<T, X>());
            }
            return result;
        }

        public override IOpenApiAny ParseIntoAny()
        {
            var array = new OpenApiArray();
            foreach (var node in childNodes)
            {
                array.Add(node.ParseIntoAny());
            }
            return array;
        }
    }

    /// <summary>
    /// A property node in the OpenAPI/swagger structure.
    /// </summary>
    internal class PropertyParsingNode : ParsingNode
    {
        public string Name { get; set; } // property key
        public ParsingNode Value { get; set; } // property value
        public PropertyParsingNode(JProperty json, IOpenApiVersionParser versionParser, ContextStorage storage) : base(json, versionParser, storage)
        {
            Name = json.Name;
            Value = Create(json.Value, versionParser, storage); // add child elements to the node tree
        }
        public T GetSimpleValue<T>()
        {
            return (T)(this.Value as ValueParsingNode).Value;
        }

        public override IOpenApiAny ParseIntoAny()
        {
            throw new Exception("Cannot parse a property node into OpenApiAny");
        }
    }

    /// <summary>
    /// A value node in the OpenAPI/swagger structure.
    /// </summary>
    internal class ValueParsingNode : ParsingNode
    {
        public object Value { get; set; }

        public ValueParsingNode(JValue json, IOpenApiVersionParser versionParser, ContextStorage storage) : base(json, versionParser, storage)
        {
            Value = json.Value;
        }

        public override IOpenApiAny ParseIntoAny()
        {
            if (Value == null)
            {
                return new OpenApiString(null, false); // null
            }
            if (Value.GetType() == typeof(string)) // value is quoted when specified in json
            {
                return new OpenApiString(Value.ToString(), true); // an actual string
            }
            return new OpenApiString(Value.ToString(), false); // something else - int32, double, bool, etc
        }
    }
}