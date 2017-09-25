using System;
using System.Collections.Generic;
using System.IO;

namespace Kernel.Serialisation
{
    public interface ISerializer
    {
        /// <summary>
        /// Serializes objects to stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="o"></param>
        void Serialize(Stream stream, object[] o);

        /// <summary>
        /// Serialise an object to string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        string Serialize(object o);

        /// <summary>
        /// Deserializes objects from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="messageTypes"></param>
        /// <returns></returns>
        object[] Deserialize(Stream stream, IList<Type> messageTypes);

        /// <summary>
        /// Deserializes string to type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        T Deserialize<T>(string data);

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        object Deserialize(string data);
    }
}