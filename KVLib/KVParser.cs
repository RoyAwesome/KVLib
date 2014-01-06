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
    }

    public class Content : Item
    {
        public string Text;
    }

    public class Node : Item
    {
        public IEnumerable<Item> Children;
    }

    public class KVParser
    {
        static Parser<Char> NewLine = Parse.Char((c => char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.LineSeparator), "New Line");

        public static Parser<string> Comment =           
            from first in Parse.Char('/').Repeat(2).Token()
            from rest in Parse.AnyChar.Until(Parse.Char('\n'))
            select new string(rest.ToArray());

        public static Parser<IEnumerable<string>> AllComments =
            from first in Comment.Token().Many()
            select first;

        public static Parser<string> BunchaWhitespace =
            from ws in Parse.WhiteSpace.Many()
            select string.Join("", ws);

       

        public static Parser<string> KVString =        
             from first in Parse.Char('"').Token().Optional()
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Or(Parse.Char('-')).Until(Parse.Char('"').Or(Parse.WhiteSpace))
            select new string(rest.ToArray()).Trim();

        public static Parser<Content> Content =
            from Key in KVString.Token().Named("Content Key")
            from Value in KVString.Token().Named("Content Value")
            from comment in AllComments.Optional()
            select new Content() { Key = Key, Text = Value };

        static Parser<T> BlockT<T>(Parser<T> content)
        {
            return from open in Parse.Char('{').Named("Start of Block")
            from t in content
            from close in Parse.Char('}').Token().Named("End of Block")
            select t;
        }


        public static Parser<IEnumerable<Item>> Block = BlockT(         
            from nodes in Parse.Ref(() => Item).Many().Named("Nodes")            
                select nodes);
            

        public static Parser<Node> Node =             
            from Key in KVString.Token().Named("Node Key")
            from comment in AllComments.Optional()
            from Value in Block.Token().Named("Node Value")
            select new Node() { Key = Key, Children = Value };

        public static Parser<Item> Item =
            from comment in AllComments.Optional()
            from nodes in Node.Select(n => (Item)n).Or(Content).Named("Node Or Content")
            from comment2 in Comment.Optional()
            select nodes;

       
                

        public static Parser<Node> Document =
            from leading in AllComments.Token().Optional()
            from node in Node
            select node;


      
     

    }
}
