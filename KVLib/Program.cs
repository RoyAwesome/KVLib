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

            string doc = File.ReadAllText("npc_abilities_custom.txt");
            string test = " \"CustomHeroList\" { \"npc_dota_hero_axe\"		\"1\" \r\n } ";
            string itemtest = " //test \n\"BlockTestTester\" { //=====\n //test \n \"npc_dota_hero_axe\"  \t\"0\" //========\n//======\n\"test2\" { test3 0 } \r\n}";
            string commentTest = " //test\n //test2 //test3\n";

            var c = KVParser.Document.Parse(itemtest);

            PrintNode(c, 0);




            c = KVParser.Document.Parse(doc);

            PrintNode(c, 0);


            //string comment = KVParser.Document.Parse();

            //Console.WriteLine(comment);

            Console.ReadKey();

        }

        public static void PrintNode(Node n, int indent)
        {
            for (int i = 0; i < indent; i++)
                Console.Write("\t");
            Console.WriteLine(n.Key);
            foreach (Item child in n.Children)
            {
                if (child is Content)
                {
                    for (int i = 0; i <= indent; i++)
                        Console.Write("\t");
                    Console.WriteLine(string.Format("{0} {1}", child.Key, (child as Content).Text));
                }
                if (child is Node)
                {                   
                    PrintNode(child as Node, indent+1);
                }
            }
        }
    }
}
