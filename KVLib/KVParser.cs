using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace KVLib
{

    public class Item 
    {
        public string Key
        {
            internal set;
            get;
        }

        public string Text;

        public IEnumerable<Item> Children = null;

        public bool HasChildren
        {
            get
            {
                return Children != null;
            }
        }
    }
   

    public class KVParser
    {
        static Parser<Char> NewLine = Parse.Char((c => char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.LineSeparator), "New Line");

        static Parser<char> DisallowedKeyChar = Parse.Char('}').XOr(Parse.Char('{')
            .XOr(Parse.WhiteSpace.XOr(Parse.Char('"'))));

        public static Parser<string> Comment =           
            from first in Parse.Char('/').Repeat(2).Token()
            from rest in Parse.AnyChar.Until(Parse.Char('\n'))
            select new string(rest.ToArray());

        public static Parser<IEnumerable<string>> AllComments =
            from first in Comment.Token().Many()
            select first;       

        public static Parser<string> KVString =        
            from first in Parse.Char('"').Token().Optional()
            from rest in Parse.AnyChar.Except(DisallowedKeyChar).Token().Many()
            from last in Parse.Char('"').Token().Optional()
            select new string(rest.ToArray());


        public static Parser<Item> SubKey(string key)
        {
            return from open in Parse.Char('{').Token().Named("Start of Block")
                   from value in Parse.Ref(() => Item).Many()  
                   from close in Parse.Char('}').Token().Named("End of Block")
                   select (new Item() { Key = key, Children = value });
        }

        public static Parser<Item> Value(string key)
        {
            return from Value in KVString.Token().Named("Content Value")
                   select (new Item() { Key = key, Text = Value });

        }

        public static Parser<Item> Item =
            from c1 in AllComments.Token().Optional()
            from Key in KVString.Named("Key")
            from c in AllComments.Token().Optional()
            from nodes in SubKey(Key).Token().XOr(Value(Key).Token())
            from c2 in AllComments.Token().Optional()
            select nodes;

       
                

        public static Parser<Item> Document =
            from leading in AllComments.Token().Optional()
            from node in Item
            select node;            

    }
}
