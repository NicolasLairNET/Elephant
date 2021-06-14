using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElephantLibrary
{
    internal interface ITDCFile
    {
        public List<TDCTag> Read(string[] lines);
    }
}
