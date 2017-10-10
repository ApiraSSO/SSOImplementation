using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Kernel.Serialisation;
using Newtonsoft.Json;

namespace Serialisation.JSON
{
    public class NSJsonSerializer : IJsonSerialiser
    {
        #region fields

        private JsonSerializerSettings jsonSerializerSettings;
        
        #endregion

        #region Constructors

        public NSJsonSerializer(ISerialisationSettingsProvider<JsonSerializerSettings> jsonSerializerSettings)
        {
            this.jsonSerializerSettings = jsonSerializerSettings.GetSettings().Settings;
        }

        #endregion

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="o"></param>
        void ISerializer.Serialize(Stream stream, object[] o)
        {
            var writer = new StreamWriter(stream);
            var ser = JsonSerializer.Create(this.jsonSerializerSettings);
            o.Aggregate(ser, (s, next) => { s.Serialize(writer, next); return s; });
            writer.Flush();
        }

        /// <summary>
        /// Not imlemented.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="messageTypes"></param>
        /// <returns></returns>
        object[] ISerializer.Deserialize(Stream stream, IList<Type> messageTypes)
        {
            using (var jsonReader = new JsonTextReader(new StreamReader(stream)))
            {
                var ser = JsonSerializer.Create(this.jsonSerializerSettings);
                var result = ser.Deserialize(jsonReader);
                return new[] { result };
            }
        }

        /// <summary>
        /// Deserializes data to type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, jsonSerializerSettings);
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public object Deserialize(string data)
        {
            return JsonConvert.DeserializeObject(data, jsonSerializerSettings);
        }

        /// <summary>
        /// Serialise an object to a string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o, jsonSerializerSettings);
        }
    }
}