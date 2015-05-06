using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SParse = Sprache.Parse;

namespace KVLib.KeyValues
{
    public class SpracheKVParser : IKVParser
    {
        #region TextModel
        static Parser<char> DisallowedKeyChar = SParse.Char('}').XOr(SParse.Char('{'));

        static CommentParser Comment = new CommentParser("//", "/*", "\\*", "\n"); 

        static Parser<IEnumerable<string>> AllComments =
            from first in Comment.SingleLineComment.Token().Many()
            select first;


        static Parser<string> UnquotedKVString =
            from rest in SParse.AnyChar.Except(DisallowedKeyChar).Until(SParse.WhiteSpace.AtLeastOnce())
            select new string(rest.ToArray());

        static Parser<string> QuotedKVString =
            from openquote in SParse.Char('"')
            from rest in SParse.AnyChar.Except(DisallowedKeyChar).Until(SParse.Char('"'))
            select new string(rest.ToArray());

        static Parser<string> KVString = QuotedKVString.Or(UnquotedKVString);


        static Parser<KeyValue> SubKey(string key)
        {
            return from open in SParse.Char('{').Token().Named("Start of Block")
                   from c1 in AllComments.Token().Many().Optional()
                   from value in SParse.Ref(() => Item).Many().Optional()
                   from close in SParse.Char('}').Token().Named("End of Block")
                   select (new KeyValue(key, value.Get()));
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



        public KeyValue Parse(string kvstring)
        {
            try
            {
                KeyValue i = Document.Parse(kvstring);
                return i;
            }
            catch (Sprache.ParseException e)
            {
                throw new KeyValueParsingException(e.Message, e);
            }
        }

        public KeyValue[] ParseAll(string kvstring)
        {
            IEnumerable<KeyValue> i = Document.Many().Parse(kvstring);
            return i.ToArray();
        }
    }
}
