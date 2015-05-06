using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVLib.KeyValues
{
    public interface IKVParser
    {
        /// <summary>
        /// Parse a single KeyValue from a string.
        /// </summary>
        /// <param name="contents">The string representation of the KeyValue object</param>
        /// <returns>The parsed KeyValue, or null if no keyvalues were found while parsing.</returns>
        /// <exception cref="KVLib.KeyValues.KeyValueParsingException">Throws one of these if parsing fails</exception>
        KeyValue Parse(string kvstring);

        /// <summary>
        /// Grab all of the keyvalues from a string.
        /// </summary>
        /// <param name="contents">The string containing keyvalues</param>
        /// <returns>An array containing all root-level KeyValues in the string</returns>
        /// <exception cref="KVLib.KeyValues.KeyValueParsingException">Throws one of these if parsing fails</exception>
        KeyValue[] ParseAll(string kvstring);

    }
}
