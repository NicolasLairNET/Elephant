using Elephant.Model;
using System.Collections.Generic;

namespace Elephant.Services
{
    internal interface ITDCFile
    {
        public List<TDCTag> Read();
    }
}
