using System.Collections.Generic;

namespace X13.OpenAPI.Internal.Parsing.Context
{
    internal class ContextStorage : Dictionary<string, object>
    {
        public T Retrieve<T>(string key)
        {
            this.TryGetValue(key, out var res);
            if (res != null)
            {
                return (T)res;
            }
            return default(T);
        }
        public void Store<T>(string key, T value)
        {
            // replace existing values
            this[key] = value;
        }
    }
}