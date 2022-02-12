namespace Elephant.Services.JsonFileTDCTag.DTOs;
public record TagInfo
{
    public int[] NamePosition { get; init; } = new int[2];
    public string Parameter { get; init; } = "";
    public int[] ValuePosition { get; init; } = new int[2];
    public string Origin { get; init; } = "";
}
