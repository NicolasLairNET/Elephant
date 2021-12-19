namespace Elephant.Services.JsonFileTDCTag.DTOs;
public record TagDto
{
    public int[] NamePosition { get; init; }
    public string Parameter { get; init; }
    public int[] ValuePosition { get; init; }
    public string Origin { get; init; }
}
