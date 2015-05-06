using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KVLib;
using KVLib.KeyValues;
using System.Linq;

namespace KVLib
{
    [TestClass]
    public class PenguinParserTester
    {
        [TestMethod]
        public void PSample1()
        {
            KeyValue kv = KVTestHelper.Sample1(new PenguinParser());

            Assert.AreEqual(kv.Key, "SomeTest");
            Assert.IsNotNull(kv["Key"]);
            Assert.AreEqual(kv["Key"].GetString(), "Value");

        }

        [TestMethod]
        public void PSample2()
        {
            KeyValue kv = KVTestHelper.Sample2(new PenguinParser());

            Assert.AreEqual(kv.Key, "Sample2");
            Assert.IsNotNull(kv["Key"]);
            Assert.AreEqual(kv["Key"].GetString(), "Value");

        }

        [TestMethod]
        public void PSample3()
        {
            KeyValue kv = KVTestHelper.Sample3(new PenguinParser());

            Assert.AreEqual(kv.Key, "Sample3");
            Assert.IsNotNull(kv["Key"]);
            Assert.AreEqual(kv["Key"].GetString(), "Value");
            Assert.IsNotNull(kv["Subkey"]);
            Assert.AreEqual(kv["Subkey"].Key, "Subkey");
            Assert.IsNotNull(kv["Subkey"]["Key"]);
            Assert.AreEqual(kv["Subkey"]["Key"].GetString(), "Value");
        }

        [TestMethod]
        public void PSample4()
        {
            /* This sample was taken from the Dota 2 project sample for Holdout
             * It had an issue because on line 6 there is a key/value pair with a comment at the end, but not whitespace 
             * betwen the comment and the end of the value.  
             */
            KeyValue kv = KVTestHelper.Sample4(new PenguinParser());

            Assert.AreEqual(kv.Key, "npc_dota_holdout_tower");

            Assert.IsNotNull(kv["BaseClass"]);

            Assert.IsNotNull(kv["Model"]);
            Assert.AreEqual(kv["Model"].GetString(), "models/props_structures/tower_good.vmdl");

            Assert.IsNotNull(kv["SoundSet"]);
            Assert.AreEqual(kv["SoundSet"].GetString(), "Tower.Water");
        }

        [TestMethod]
        public void PSample5()
        {
            //In this sample on line 128 there is a KeyValue pair without a space
            //between the key and the value.  

            //Check that value and make sure it returns 50.

            KeyValue kv = KVTestHelper.Sample5(new PenguinParser());

            Assert.AreEqual("DOTAUnits", kv.Key);

            KeyValue SpecialKey = kv["npc_dota_creature_berserk_zombie"]["Creature"]["DefensiveAbilities"]["Ability1"];
            Assert.AreEqual(50, SpecialKey["UseAtHealthPercent"].GetInt());
        }

        [TestMethod]
        public void PSample6()
        {
            KeyValue kv = KVTestHelper.Sample6(new PenguinParser());

            Assert.AreEqual("DOTAAbilities", kv.Key);
        }

        [TestMethod]
        public void PIssue1()
        {
            KeyValue kv = KVTestHelper.Issue1(new PenguinParser());
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
        public void PIssue3()
        {
            KeyValue kv = KVTestHelper.Issue3(new PenguinParser());
            Assert.AreEqual(kv.Key, "Issue3Test");

            Assert.IsNotNull(kv["Test"]);
            Assert.AreEqual(kv["Test"].GetString(), "Value");
        }

        [TestMethod]
        public void PSample7()
        {
            KeyValue kv = KVTestHelper.Sample7(new PenguinParser());

            Assert.AreEqual(kv.Key, "DOTAAbilities");
            Assert.IsNotNull(kv["ninja_dash_fire"]);
            Assert.IsNotNull(kv["ninja_dash_fire"]["precache"]);
            Assert.AreEqual(kv["ninja_dash_fire"]["precache"].Children.Count(), 0);
        }
    }
}
