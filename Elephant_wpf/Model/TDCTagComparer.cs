namespace Elephant.Model;
class TDCTagComparer : IEqualityComparer<TDCTag>
{
    public bool Equals(TDCTag? x, TDCTag? y)
    {
        if (x is null || y is null) return false;

        return x.Name == y.Name
            && x.Parameter == y.Parameter
            && x.Origin == y.Origin
            && x.Value == y.Value;
    }

    public int GetHashCode(TDCTag tag)
    {
        return string.Concat(tag.Name, tag.Parameter, tag.Origin, tag.Value).GetHashCode();
    }
}
