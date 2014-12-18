using KVLib.KeyValues;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVLib
{
    class KVTestHelper
    {
        private static string GetText(string sample)
        {
            return File.ReadAllText(string.Format("SampleKeyValues/{0}.txt", sample));
        }

        public static KeyValue Sample1(IKVParser parser)
        {
            return parser.Parse(GetText("Sample1"));
        }

        public static KeyValue Sample2(IKVParser parser)
        {
            return parser.Parse(GetText("Sample2"));
        }

        public static KeyValue Sample3(IKVParser parser)
        {
            return parser.Parse(GetText("Sample3"));
        }

        public static KeyValue Sample4(IKVParser parser)
        {
            return parser.Parse(GetText("Sample4"));
        }
        public static KeyValue Sample5(IKVParser parser)
        {
            return parser.Parse(GetText("Sample5"));
        }
        public static KeyValue Sample6(IKVParser parser)
        {
            return parser.Parse(GetText("Sample6"));
        }
        public static KeyValue Issue1(IKVParser parser)
        {
            return parser.Parse(GetText("Issue1Test"));
        }
        public static KeyValue Issue3(IKVParser parser)
        {
            return parser.Parse(GetText("Issue3Test"));
        }
               
    }
}
