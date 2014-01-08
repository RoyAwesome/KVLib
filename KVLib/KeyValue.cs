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
                else return children.AsEnumerable();
            }
            internal set
            {
                children = value.ToList();
            }
        }

        public bool HasChildren
        {
            get
            {
                return children != null;
            }
        }

        internal string Value = "";
        List<KeyValue> children = null;

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

        public bool TryGet(out bool value)
        {
            value = default(bool);
            int a;
            if (!int.TryParse(Value, out a)) return false;
            value = (a != 0);
            return true;

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

        public bool GetBool()
        {
            bool v;
            bool success = TryGet(out v);
            return success ? v : false;
        }

        public string GetString()
        {
            return Value;
        }       
        #endregion

        #region Setters
        public KeyValue Set(int value)
        {
            children = null;
            Value = value.ToString();

            return this;
        }
        public KeyValue Set(float value)
        {
            children = null;
            Value = value.ToString();

            return this;
        }
        public KeyValue Set(string value)
        {
            children = null;
            Value = value;

            return this;
        }
        public KeyValue Set(bool value)
        {
            children = null;
            Value = value ? "1" : "0";

            return this;
        }

        public void AddChild(KeyValue value)
        {
            if(children == null)
            {
                Value = "";
                children = new List<KeyValue>();
            }      

            children.Add(value);
        }
        #endregion

        public static KeyValue operator+(KeyValue rhs, KeyValue lhs)
        {
            rhs.AddChild(lhs);
            return rhs;
        } 

        public static KeyValue operator+(KeyValue rhs, int lhs)
        {
            return rhs.Set(lhs);
        }

        public static KeyValue operator +(KeyValue rhs, float lhs)
        {
            return rhs.Set(lhs);
        }

        public static KeyValue operator +(KeyValue rhs, string lhs)
        {
            return rhs.Set(lhs);
        }

        public static KeyValue operator +(KeyValue rhs, bool lhs)
        {
            return rhs.Set(lhs);
        }


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
                builder.AppendLine(child.ToString(indent + 1));
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
    }
}
