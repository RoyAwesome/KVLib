using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KVLib
{
    public class KeyValue
    {
        public string Key
        {
            get;
            private set;
        }

        public IEnumerable<KeyValue> Children
        {
            get
            {
                if (children == null) return Enumerable.Empty<KeyValue>();
                else return Children.AsEnumerable();
            }
        }

        string Value = "";
        KeyValue[] children = null;

        public KeyValue this[string key]
        {
            get
            {
                if (children == null) return null;
                return children.FirstOrDefault(x => x.Key == key);
            }

            set
            {
                AddChild(value);
            }

        }


        public KeyValue(string key)
        {
            Key = key;
        }

        #region Getters
        public bool TryGet(out int value)
        {
            return int.TryParse(Value, out value);
        }
        public bool TryGet(out float value)
        {
            return float.TryParse(Value, out value);
        }

        public int GetInt()
        {
            int v;
            bool success = int.TryParse(Value, out v);
            return success ? v : 0;
        }
        public float GetFloat()
        {
            float v;
            bool success = float.TryParse(Value, out v);
            return success ? v : 0;
        }
        public string GetString()
        {
            return Value;
        }       
        #endregion

        #region Setters
        public void Set(int value)
        {
            children = null;
            Value = value.ToString();
        }
        public void Set(float value)
        {
            children = null;
            Value = value.ToString();
        }
        public void Set(string value)
        {
            children = null;
            Value = value;
        }

        public void AddChild(KeyValue value)
        {
            if(children == null)
            {
                Value = "";
                children = new KeyValue[] { };
            }

            KeyValue[] old = children;
            children = new KeyValue[old.Length + 1];
            for (int i = 0; i < old.Length; i++ )
            {
                children[i] = old[i];
            }
            children[children.Length - 1] = value;            
        }
        #endregion

       /* public KeyValue operator+(KeyValue ob)
        {
            AddChild(ob);
            return this;
        } */

        public string ToString(int indent)
        {
            if (children == null)
            {
                return string.Format("\"{0}\"\t\"{1}\"", Key, Value);
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < indent; i++)
            {
                builder.Append("\t");
            }
            builder.AppendLine(string.Format("\"{0}\"", Key));
            for (int i = 0; i < indent; i++)
            {
                builder.Append("\t");
            }
            builder.AppendLine("{");
            foreach (KeyValue child in Children)
            {
                for (int i = 0; i < indent; i++)
                {
                    builder.Append("\t");
                }
                builder.AppendLine("\t" + child.ToString(indent + 1));
            }
            for (int i = 0; i < indent; i++)
            {
                builder.Append("\t");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }

        public override string ToString()
        {
            return ToString(0);
        }
        /*
        public static KeyValue Parse(string text)
        {

            



           

            return ob;
        }*/
    }
}
