using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVLib.KeyValues
{
    public class AderumParser : IKVParser
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AderumParser()
        {

        }
        #region TextModel
        static Parser<char> DisallowedKeyChar = Sprache.Parse.Char('}').XOr(Sprache.Parse.Char('{'));

        static Parser<string> Comment = Sprache.Parse.EndOfLineComment("//");

        static Parser<IEnumerable<string>> AllComments =
            from first in Comment.Token().Many()
            select first;


        static Parser<string> UnquotedKVString =
            from rest in Sprache.Parse.AnyChar.Except(DisallowedKeyChar).Until(Sprache.Parse.WhiteSpace.AtLeastOnce())
            select new string(rest.ToArray());

        static Parser<string> QuotedKVString =
            from openquote in Sprache.Parse.Char('"')
            from rest in Sprache.Parse.AnyChar.Except(DisallowedKeyChar).Until(Sprache.Parse.Char('"'))
            select new string(rest.ToArray());

        static Parser<string> KVString = QuotedKVString.Or(UnquotedKVString);


        static Parser<KeyValue> SubKey(string key)
        {
            return from open in Sprache.Parse.Char('{').Token().Named("Start of Block")
                   from value in Sprache.Parse.Ref(() => Item).Many()
                   from close in Sprache.Parse.Char('}').Token().Named("End of Block")
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

        #region ManualParse 

        enum parseEnum {foundFirstKey,foundSecondKey,foundFirstValue,foundSecondvalue, nil};
        static parseEnum parseState = parseEnum.nil;

        public static KeyValue ParseKeyValueFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines("SampleKeyValues/" + path);
            KeyValue kv = new KeyValue(""); 
            foreach(string str in lines)
            {
                if(str.StartsWith("{") || str.StartsWith("//")){ continue; }
                if (str.Contains("}")) 
                { 
                    kv = kv.Parent ?? kv; //we reached the end of a parent, go up one level 
                    continue;
                }

                string key = null;
                string value = null; 
                for (int i = 0; i < str.Length;i++ )
                {
                    if (str.ElementAt(i) == '/' && str.ElementAt(i + 1) == '/') 
                    {
                        break;
                    }
                    switch(parseState)
                    {
                        case parseEnum.nil:
                            if(str.ElementAt(i) == '"')
                            {
                                parseState = parseEnum.foundFirstKey;
                                key = "";
                                continue;
                            }
                            break;
                        case parseEnum.foundFirstKey:
                            if(str.ElementAt(i) != '"')
                            {
                                key = key + str.ElementAt(i).ToString();
                            }
                            else
                            {
                                parseState = parseEnum.foundSecondKey;
                                continue;
                            }
                            break;
                        case parseEnum.foundSecondKey:
                            if(str.ElementAt(i) == '"')
                            {
                                parseState = parseEnum.foundFirstValue;
                                value = "";
                                continue;
                            }
                            break;
                        case parseEnum.foundFirstValue:
                            if(str.ElementAt(i) != '"')
                            {
                                value = value + str.ElementAt(i).ToString();
                            }
                            else
                            {
                                parseState = parseEnum.foundSecondvalue;
                                continue;
                            }
                            break;
                        case parseEnum.foundSecondvalue:
                            //
                            break;
                    }


                }
                parseState = parseEnum.nil;

                if (key == null && value == null) { continue; }

                if(value == null)
                {
                    if (kv.Key == "")
                    {
                        kv = new KeyValue(key);
                    }
                    else
                    {
                        KeyValue tmpKv = new KeyValue(key);
                        kv.AddChild(tmpKv);
                        kv = tmpKv; //we found a parent, set kv to be the parent we found
                    }
                }
                else
                {
                    KeyValue tmpKv = new KeyValue(key);
                    tmpKv.Value = value;
                    kv.AddChild(tmpKv);
                }
                
            }

            return kv;
        }
        #endregion

        /// <summary>
        /// Reads a line of text and returns the first Root key in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public KeyValue Parse(string text)
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
        public KeyValue[] ParseAll(string text)
        {
            IEnumerable<KeyValue> i = Document.Many().Parse(text);
            return i.ToArray();
        }


    }
}
