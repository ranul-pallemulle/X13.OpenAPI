using System;
using System.Linq;

namespace X13.OpenAPI.Public.Model.Types.RuntimeExpressions
{
    public class OpenApiJsonPointer
    {
        public string[] Tokens { get; }
        public OpenApiJsonPointer(string jsonPointer)
        {
            if (jsonPointer == null)
            {
                throw new ArgumentNullException();
            }
            Tokens = jsonPointer.Split('/').Skip(1).Select(Decode).ToArray();
        }
        [Newtonsoft.Json.JsonConstructor]
        public OpenApiJsonPointer(string[] tokens)
        {
            Tokens = tokens;
        }
        private string Decode(string jsonPointer)
        {
            return Uri.UnescapeDataString(jsonPointer).Replace("~1", "/").Replace("~0", "~");
        }

        public OpenApiJsonPointer ParentPointer
        {
            get
            {
                if (Tokens.Length == 0)
                {
                    return null;
                }
                return new OpenApiJsonPointer(Tokens.Take(Tokens.Length - 1).ToArray());
            }
        }

        public override string ToString()
        {
            return "/" + string.Join("/", Tokens);
        }

        public string ToJsonPath()
        {
            if (ParentPointer == null)
            {
                return string.Empty;
            }
            var prefix = ParentPointer.ToJsonPath();
            var last = Tokens[Tokens.Length - 1];
            if (int.TryParse(last, out _))
            {
                return prefix + $"[{last}]";
            }
            if (last.Contains('.'))
            {
                last = $"['{last}']";
            }
            return prefix + '.' + last;
        }
    }
}