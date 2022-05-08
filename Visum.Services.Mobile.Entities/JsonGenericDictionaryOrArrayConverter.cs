using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace Visum.Services.Mobile.Entities
{
    public class JsonGenericDictionaryOrArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetDictionaryKeyValueTypes().Count() == 1;
        }

        public override bool CanWrite { get { return false; } }

        object ReadJsonGeneric<TKey, TValue>(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var tokenType = reader.TokenType;

            var dict = existingValue as IDictionary<TKey, TValue>;
            if (dict == null)
            {
                var contract = serializer.ContractResolver.ResolveContract(objectType);
                dict = (IDictionary<TKey, TValue>)contract.DefaultCreator();
            }

            if (tokenType == JsonToken.StartArray)
            {
                var pairs = new JsonSerializer().Deserialize<KeyValuePair<TKey, TValue>[]>(reader);
                if (pairs == null)
                    return existingValue;
                foreach (var pair in pairs)
                    dict.Add(pair);
            }
            else if (tokenType == JsonToken.StartObject)
            {
                // Using "Populate()" avoids infinite recursion.
                // https://github.com/JamesNK/Newtonsoft.Json/blob/ee170dc5510bb3ffd35fc1b0d986f34e33c51ab9/Src/Newtonsoft.Json/Converters/CustomCreationConverter.cs
                serializer.Populate(reader, dict);
            }
            return dict;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var keyValueTypes = objectType.GetDictionaryKeyValueTypes().Single(); // Throws an exception if not exactly one.

            var method = GetType().GetMethod("ReadJsonGeneric", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            var genericMethod = method.MakeGenericMethod(new[] { keyValueTypes.Key, keyValueTypes.Value });
            return genericMethod.Invoke(this, new object[] { reader, objectType, existingValue, serializer });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
