using System.Collections.Generic;

namespace Elephant.Model
{
    internal interface ITDCFile
    {
        public List<TDCTag> Read();
    }
}
