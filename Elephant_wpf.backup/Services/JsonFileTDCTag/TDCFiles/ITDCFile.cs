using Elephant.Model;
using System.Collections.Generic;

namespace Elephant.Services
{
    public interface ITDCFile
    {
        public List<TDCTag> GetTagsList();
    }
}
