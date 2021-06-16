
using System.Collections.Generic;

namespace ElephantLibrary
{
    internal interface ITDCFile
    {
        public List<TDCTag> Read();
    }
}
