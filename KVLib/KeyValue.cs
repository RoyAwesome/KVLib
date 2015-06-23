using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KVLib
{
    /// <summary>
    /// Represnetation of Valve's Key Value format
    /// 
    /// A KeyValue contains a string key, and either a value or a list of children. 
    /// 
    /// Example Key-Value tree:
    /// 
    /// "Example"
    /// {
    ///     "ExampleParent"
    ///     {
    ///         "ExampleValue1" "A String"
    ///         "ExampleValueInt" "12"
    ///     }
    ///     
    ///     "ExampleValue" "3.14 12 15" 
    /// }
    /// </summary>
    public class KeyValue : ICloneable
    {
        /// <summary>
        /// The key of the KV Object.  Read-only
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        /// <summary>
        /// List of children leafs
        /// </summary>
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

        /// <summary>
        /// True if the object has children leafs. 
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return children != null;
            }
        }

        /// <summary>
        /// The parent node for this KeyValue.
        /// </summary>
        public KeyValue Parent
        {
            get;
            private set;
        }


        /// <summary>
        /// The internal value string.
        /// </summary>
        private string _value;

        /// <summary>
        /// Get the value of the keyvalue as a string.
        /// </summary>
        internal string Value
        {
            get
            {
                return GetString();
            }
            set
            {
                Set(value);
            }
        }
       
        /// <summary>
        /// The children of this KeyValue.
        /// </summary>
        List<KeyValue> children = null;

        /// <summary>
        /// Access the KeyValue tree by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Key Value Constructor
        /// </summary>
        /// <param name="key">the key of the Key-Value pair</param>
        public KeyValue(string key)
        {          
            Key = key;
           
        }

        /// <summary>
        /// Construct a keyvalue with a given key and set of children.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="children">The enumerable set of children.</param>
        internal KeyValue(string key, IEnumerable<KeyValue> children)
            : this(key)
        {
            AddChildren(children);
        }

        #region Getters
        public bool TryGet(out int value)
        {
            return int.TryParse(_value, out value);
        }
        public bool TryGet(out float value)
        {
            return float.TryParse(_value, out value);
        }

        public bool TryGet(out bool value)
        {
            value = default(bool);
            int a;
            if (!int.TryParse(_value, out a)) return false;
            value = (a != 0);
            return true;

        }

        public int GetInt()
        {
            int v;
            bool success = int.TryParse(_value, out v);
            return success ? v : 0;
        }
        public float GetFloat()
        {
            float v;
            bool success = float.TryParse(_value, out v);
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
            return _value;
        }       
        #endregion

        #region Setters
        /// <summary>
        /// Set the value of the Key-Value leaf
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KeyValue Set(int value)
        {
            children = null;
            _value = value.ToString();

            return this;
        }
        /// <summary>
        /// Set the value of the Key-Value leaf
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KeyValue Set(float value)
        {
            children = null;
            _value = value.ToString();

            return this;
        }
        /// <summary>
        /// Set the value of the Key-Value leaf
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KeyValue Set(string value)
        {
            
#if CHECKED
            if(value == null) throw new ArgumentNullException("value");     
#endif
            value = value ?? ""; 
            children = null;           
            _value = value;

            return this;
        }
        /// <summary>
        /// Set the value of the Key-Value leaf
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KeyValue Set(bool value)
        {
            children = null;
            _value = value ? "1" : "0";

            return this;
        }

        /// <summary>
        /// Add a single child to the list of children
        /// </summary>
        /// <param name="value"></param>
        public void AddChild(KeyValue value)
        {
            if(children == null)
            {
                _value = "";
                children = new List<KeyValue>();
            }
            value.Parent = this;
            children.Add(value);
        }

        /// <summary>
        /// Add a collection of KeyValue objects as children
        /// </summary>
        /// <param name="KVList"></param>
        public void AddChildren(IEnumerable<KeyValue> KVList)
        {
            if(children == null)
            {
                _value = "";
                children = KVList.ToList();
                foreach(KeyValue kv in children)
                {
                    kv.Parent = this;
                }
                return;
            }
            else
            {
                children = children.Select(x => { x.Parent = this; return x; }).Union(KVList).ToList();
            }
        }

        /// <summary>
        /// Removes a child keyvalue
        /// </summary>
        /// <param name="child"></param>
        public bool RemoveChild(KeyValue child)
        {
            if (children == null) return false;
            if (!children.Contains(child)) return false;
            
            child.Parent = null;
            return children.Remove(child);
        }

        public void RemoveChildAt(int index)
        {
            if (!HasChildren) throw new InvalidOperationException("Keyvalue doesn't have children!");
            var c = children[index];
            c.Parent = null;

            children.RemoveAt(index);
        }

        /// <summary>
        /// Converts this Keyvalue object into a Keyvalue object without subkeys
        /// </summary>
        /// <returns></returns>
        public KeyValue MakeEmptyParent()
        {
            if (HasChildren)
            {
                return this;
            }

            children = new List<KeyValue>();
            return this;
        }

        public void ReplaceChildAtIndex(KeyValue value, int Index)
        {
            if (!HasChildren) throw new InvalidOperationException("Keyvalue doesn't have children!");
            children[Index] = value;
        }

        public int IndexOfChild(KeyValue kv)
        {
            if (!HasChildren) throw new InvalidOperationException("Keyvalue doesn't have children!");
            return children.IndexOf(kv);
        }

        public bool ContainsChild(KeyValue kv)
        {
            if (!HasChildren) throw new InvalidOperationException("Keyvalue doesn't have children!");
            return children.Contains(kv);
        }
        #endregion

        /// <summary>
        /// Adds two Key-Value objects together.  The left hand KVObject becomes a child of the right hand object
        /// </summary>
        /// <param name="rhs">The KeyValue which will be set.</param>
        /// <param name="lhs">The new value for the KeyValue.</param>
        /// <returns></returns>
        public static KeyValue operator+(KeyValue rhs, KeyValue lhs)
        {
            rhs.AddChild(lhs);
            return rhs;
        } 
        /// <summary>
        /// Sets the value of the right hand kv object to be the left hand int
        /// </summary>
        /// <param name="rhs">The KeyValue which will be set.</param>
        /// <param name="lhs">The new value for the KeyValue.</param>
        /// <returns></returns>
        public static KeyValue operator+(KeyValue rhs, int lhs)
        {
            return rhs.Set(lhs);
        }

        /// <summary>
        /// Sets the value of the right hand kv object to be the left hand int
        /// </summary>
        /// <param name="rhs">The KeyValue which will be set.</param>
        /// <param name="lhs">The new value for the KeyValue.</param>
        /// <returns></returns>
        public static KeyValue operator +(KeyValue rhs, float lhs)
        {
            return rhs.Set(lhs);
        }

        /// <summary>
        /// Sets the value of the right hand kv object to be the left hand int
        /// </summary>
        /// <param name="rhs">The KeyValue which will be set.</param>
        /// <param name="lhs">The new value for the KeyValue.</param>
        /// <returns></returns>
        public static KeyValue operator +(KeyValue rhs, string lhs)
        {
            return rhs.Set(lhs);
        }

        /// <summary>
        /// Sets the value of the right hand kv object to be the left hand int
        /// </summary>
        /// <param name="rhs">The KeyValue which will be set.</param>
        /// <param name="lhs">The new value for the KeyValue.</param>
        /// <returns></returns>
        public static KeyValue operator +(KeyValue rhs, bool lhs)
        {
            return rhs.Set(lhs);
        }

        /// <summary>
        /// Null out the parent entry of all children.
        /// </summary>
        public void ClearChildParents()
        {
            foreach (KeyValue child in children)
            {
                child.Parent = null;
            }
        }

        /// <summary>
        /// Get a string representation of the KeyValue.
        /// </summary>
        /// <returns>A string representation of the KeyValue.</returns>
        public string ToString(int indent)
        {
            if (children == null)
            {               
                return string.Format("{0}\"{1}\"\t\"{2}\"",indent > 0 ? "\t" : "", Key, _value);
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
                if(!child.HasChildren)
                {
                    for (int i = 0; i < indent; i++)
                    {
                        builder.Append("\t");
                    }
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

        /// <summary>
        /// Get a string representation of the KeyValue.
        /// </summary>
        /// <returns>A string representation of the KeyValue.</returns>
        public override string ToString()
        {
            return ToString(0);
        }     

        /// <summary>
        /// Clears the children of this Key-Value object
        /// </summary>
        public void ClearChildren()
        {
            this.children.Clear();
            this.children = null;
        }
        
        /// <summary>
        /// Create a deep copy of the KeyValue.
        /// </summary>
        /// <returns>The copy.</returns>
        public object Clone()
        {
            KeyValue kv;
            DeepCopy(this, out kv);

            return kv;
        }

        private void DeepCopy(KeyValue original, out KeyValue Copy)
        {
            Copy = new KeyValue(original.Key);
            if(original.HasChildren)
            {
                foreach(KeyValue child in original.Children)
                {
                    //Copy the child
                    KeyValue kv = new KeyValue(child.Key);
                    DeepCopy(child, out kv);
                    Copy += kv;                    
                }
            }
            else
            {
                Copy += original.GetString();
            }
                    
        }
    }
}
