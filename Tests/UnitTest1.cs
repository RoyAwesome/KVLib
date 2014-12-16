using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KVLib;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Sample1()
        {
            KeyValue kv = ParseKeyvalueFile("Sample1.txt");

            Assert.AreEqual(kv.Key, "SomeTest");
            Assert.IsNotNull(kv["Key"]);
            Assert.AreEqual(kv["Key"].GetString(), "Value");

        }

        [TestMethod]
        public void Sample2()
        {
            KeyValue kv = ParseKeyvalueFile("Sample2.txt");
            Assert.AreEqual(kv.Key, "Sample2");
            Assert.IsNotNull(kv["Key"]);
            Assert.AreEqual(kv["Key"].GetString(), "Value");

        }

        [TestMethod]
        public void Sample3()
        {
            KeyValue kv = ParseKeyvalueFile("Sample3.txt");
            Assert.AreEqual(kv.Key, "Sample3");
            Assert.IsNotNull(kv["Key"]);
            Assert.AreEqual(kv["Key"].GetString(), "Value");
            Assert.IsNotNull(kv["Subkey"]);
            Assert.AreEqual(kv["Subkey"].Key, "Subkey");
            Assert.IsNotNull(kv["Subkey"]["Key"]);
            Assert.AreEqual(kv["Subkey"]["Key"].GetString(), "Value");
        }

        [TestMethod]
        public void Sample4()
        {
            /* This sample was taken from the Dota 2 project sample for Holdout
             * It had an issue because on line 6 there is a key/value pair with a comment at the end, but not whitespace 
             * betwen the comment and the end of the value.  
             */
            KeyValue kv = ParseKeyvalueFile("Sample4.txt");

            Assert.AreEqual(kv.Key, "npc_dota_holdout_tower");

            Assert.IsNotNull(kv["BaseClass"]);

            Assert.IsNotNull(kv["Model"]);
            Assert.AreEqual(kv["Model"].GetString(), "models/props_structures/tower_good.vmdl");

            Assert.IsNotNull(kv["SoundSet"]);
            Assert.AreEqual(kv["SoundSet"].GetString(), "Tower.Water");
        }

        [TestMethod]
        public void Issue1()
        {
            KeyValue kv = ParseKeyvalueFile("Issue1Test.txt");
            Assert.AreEqual(kv.Key, "Issue1Test");
            Assert.IsNotNull(kv["Test"]);

            Assert.AreEqual(kv["Test"].GetString(), "5");
            Assert.AreEqual(kv["Test"].GetInt(), 5);

            Assert.IsNotNull(kv["Something"]);
            Assert.AreEqual(kv["Something"].GetString(), "asdf");

            Assert.IsNotNull(kv["Subkey"]);
            Assert.AreEqual(kv["Subkey"].Key, "Subkey");
            Assert.IsNotNull(kv["Subkey"]["Key"]);
            Assert.AreEqual(kv["Subkey"]["Key"].GetString(), "Value");

                        
        }

        [TestMethod]
        public void Issue3()
        {
            KeyValue kv = ParseKeyvalueFile("Issue3Test.txt");
            Assert.AreEqual(kv.Key, "Issue3Test");

            Assert.IsNotNull(kv["Test"]);
            Assert.AreEqual(kv["Test"].GetString(), "Value");
        }



        private KeyValue ParseKeyvalueFile(string file)
        {
            string KeyValueText = System.IO.File.ReadAllText("SampleKeyValues/" + file);

            KeyValue kv = KVParser.ParseKeyValueText(KeyValueText);
            return kv;

        }
    }
}
