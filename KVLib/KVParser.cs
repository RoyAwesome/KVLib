using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using KVLib.KeyValues;

namespace KVLib
{
    /// <summary>
    /// Parser entry point for reading Key Value strings
    /// </summary>
    public static class KVParser
    {
        public static IKVParser KV1 = new KeyValues.PenguinParser();
     

        /// <summary>
        /// Reads a line of text and returns the first Root key in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// 
        [Obsolete("Please use KVParser.KV1.Parse()")]
        public static KeyValue ParseKeyValueText(string text)
        {
            return KV1.Parse(text);
            

        }
        /// <summary>
        /// Reads a blob of text and returns an array of all root keys in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// 
        [Obsolete("Please use KVParser.KV1.ParseAll()")]
        public static KeyValue[] ParseAllKVRootNodes(string text)
        {
            return KV1.ParseAll(text);
        }


    }

    public class KeyValueParsingException : Exception
    {
        public KeyValueParsingException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
