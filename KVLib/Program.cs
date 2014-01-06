using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Sprache;

namespace KVLib
{
    class Program
    {

        public static void Main(string[] args)
        {

            string doc = File.ReadAllText("frostivus_english.txt");
            string test = "{ \"npc_dota_hero_axe\"		\"1\" \n  } ";
            string itemtest = " //test \n\"BlockTestTester\" {\r\n\t\"npc_dota_hero_axe\"  \t\"0\"\r\n //test\r\n\"test2\" { \"test3\" \"0\" } \r\n}";
            string commentTest = " //test\n //test2 //test3\n";

           

            var c = KVParser.Document.Parse(doc);

            PrintNode(c, 0);


            //string comment = KVParser.Document.Parse();

            //Console.WriteLine(comment);

            Console.ReadKey();

        }

        public static void PrintNode(Item n, int indent)
        {
            for (int i = 0; i < indent; i++)
                Console.Write("\t");
            Console.WriteLine(n.Key);
            if (n.HasChildren)
            {
                foreach (Item child in n.Children)
                {
                    if (!child.HasChildren)
                    {
                        for (int i = 0; i <= indent; i++)
                            Console.Write("\t");
                        Console.WriteLine(string.Format("{0} {1}", child.Key, child.Text));
                    }
                    else
                    {
                        PrintNode(child, indent + 1);
                    }
                }
            }
        }
    }
}
