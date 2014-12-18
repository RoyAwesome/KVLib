using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVLib.KeyValues
{
    public interface IKVParser
    {
        KeyValue Parse(string kvstring);

        KeyValue[] ParseAll(string kvstring);

    }
}
