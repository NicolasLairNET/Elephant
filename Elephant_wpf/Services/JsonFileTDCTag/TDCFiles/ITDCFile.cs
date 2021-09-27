using Elephant.Model;
using System.Collections.Generic;

namespace Elephant.Services.JsonFileTDCTag
{
    public interface ITDCFile
    {
        public List<TDCTag> GetTagsList();
    }
}
