using System.Collections.Generic;

namespace Elephant.Model
{
    class TDCTagComparer : IEqualityComparer<TDCTag>
    {
        public bool Equals(TDCTag x, TDCTag y)
        {
            return x.Name == y.Name
                && x.Parameter == y.Parameter
                && x.Origin == y.Origin
                && x.Value == y.Value;
        }

        public int GetHashCode(TDCTag tag)
        {
            int hash = string.Concat(tag.Name, tag.Parameter, tag.Origin, tag.Value).GetHashCode();
            return hash;
        }
    }
}
