using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVLib.KeyValues
{
    /// <summary>
    /// An exception thrown when parsing a KV file.
    /// </summary>
    class KeyValueParsingException : Exception
    {
        /// <summary>
        /// Construct a new KeyValueParsingException
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="inner">The internal exception that caused the KVPE</param>
        public KeyValueParsingException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
