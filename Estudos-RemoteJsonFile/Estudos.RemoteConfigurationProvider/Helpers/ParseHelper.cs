using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Estudos.RemoteConfigurationProvider.Helpers
{
    public class ParseHelper : SortedDictionary<string, string>
    {
        private readonly Stack<string> _context;
        private string _currentPath;

        public static IDictionary<string, string> Parse(Stream input) => new ParseHelper(input);

        public static IDictionary<string, string> Parse(JObject input) => new ParseHelper(input);

        private ParseHelper(JObject jObject) : base(StringComparer.OrdinalIgnoreCase)
        {
            _context = new Stack<string>();
            _currentPath = string.Empty;

            PopulateDictionary(jObject);
        }

        private ParseHelper(Stream input) : base(StringComparer.OrdinalIgnoreCase)
        {
            _context = new Stack<string>();
            _currentPath = string.Empty;

            if (input.Length > 0)
                PopulateDictionary(JObject.Load(new JsonTextReader(new StreamReader(input)) {DateParseHandling = DateParseHandling.None}));
        }

        private void PopulateDictionary(JObject jObject)
        {
            VisitJObject(jObject);
        }

        private void VisitJObject(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitProperty(property);
                ExitContext();
            }
        }

        private void VisitProperty(JProperty property)
        {
            VisitToken(property.Value);
        }

        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;

                case JTokenType.Array:
                    VisitArray(token.Value<JArray>());
                    break;

                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Bytes:
                case JTokenType.Raw:
                case JTokenType.Null:
                    VisitPrimitive(token.Value<JValue>());
                    break;

                default:
                    throw new FormatException("Invalid format");
            }
        }

        private void VisitArray(JArray array)
        {
            for (int index = 0; index < array.Count; index++)
            {
                EnterContext(index.ToString());
                VisitToken(array[index]);
                ExitContext();
            }
        }

        private void VisitPrimitive(JValue data)
        {
            var key = _currentPath;

            if (ContainsKey(key))
            {
                throw new FormatException($"Duplicate key: {key}");
            }

            this[key] = data.ToString(CultureInfo.InvariantCulture);
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}