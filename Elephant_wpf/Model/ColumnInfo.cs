namespace Elephant.Model;

public record ColumnInfo
{
    public string Name { get; set; } = string.Empty;
    public int StartIndex { get; set; }
    public int Length { get; set; }
}
