using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KVLib
{
    [TestClass]
    public class KeyValueTester
    {
        [TestMethod]
        public void TestDeepCopy()
        {
            KeyValue kv = new KeyValue("Test");
            kv += new KeyValue("Key1") + "Some Value";
            kv += new KeyValue("Key2") + 3.14f;
            KeyValue child = new KeyValue("Child");
            child += new KeyValue("ChildKey") + true;
            kv += child;

            KeyValue clone = kv.Clone() as KeyValue;

            Assert.AreEqual("Test", clone.Key);
            Assert.AreEqual("Some Value", clone["Key1"].GetString());
            Assert.AreEqual("Child", clone["Child"].Key);
            Assert.AreEqual(true, clone["Child"]["ChildKey"].GetBool());

        }
    }
}
