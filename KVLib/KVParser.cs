using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using System.Text.RegularExpressions;

namespace KVLib
{
    /// <summary>
    /// Parser entry point for reading Key Value strings
    /// </summary>
    public static class KVParser
    {    

        #region TextModel
        static Parser<char> DisallowedKeyChar = Parse.Char('}').XOr(Parse.Char('{'));

        static Parser<string> Comment = Parse.EndOfLineComment("//");

        static Parser<IEnumerable<string>> AllComments =
            from first in Comment.Token().Many()
            select first;


        static Parser<string> UnquotedKVString =
            from rest in Parse.AnyChar.Except(DisallowedKeyChar).Until(Parse.WhiteSpace.AtLeastOnce())
            select new string(rest.ToArray());

        static Parser<string> QuotedKVString =
            from openquote in Parse.Char('"')
            from rest in Parse.AnyChar.Except(DisallowedKeyChar).Until(Parse.Char('"'))
            select new string(rest.ToArray());

        static Parser<string> KVString = QuotedKVString.Or(UnquotedKVString);


        static Parser<KeyValue> SubKey(string key)
        {
            return from open in Parse.Char('{').Token().Named("Start of Block")
                   from value in Parse.Ref(() => Item).Many()
                   from close in Parse.Char('}').Token().Named("End of Block")
                   select (new KeyValue(key, value));
        }

        static Parser<KeyValue> Value(string key)
        {
            return from Value in KVString.Token().Named("Content Value")
                   select (new KeyValue(key) { Value = Value });

        }

        static Parser<KeyValue> Item =
            from c1 in AllComments.Token().Optional()
            from Key in KVString.Named("Key").Token()
            from c in AllComments.Token().Optional()
            from nodes in SubKey(Key).Token().XOr(Value(Key).Named("Value"))
            from c2 in AllComments.Token().Optional()
            select nodes;




        static Parser<KeyValue> Document =
            from leading in AllComments.Token().Optional()
            from node in Item
            select node;

        #endregion

        /// <summary>
        /// Reads a line of text and returns the first Root key in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static KeyValue ParseKeyValueText(string text)
        {

            try
            {
                KeyValue i = Document.Parse(text);
                return i;
            }
            catch (Sprache.ParseException e)
            {
                throw new KeyValueParsingException(e.Message, e);
            }

        }
        /// <summary>
        /// Reads a blob of text and returns an array of all root keys in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static KeyValue[] ParseAllKVRootNodes(string text)
        {
            IEnumerable<KeyValue> i = Document.Many().Parse(text);
            return i.ToArray();
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
