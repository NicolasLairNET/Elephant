using System.Text.Json;

namespace Elephant.Model;
public record Tag
{
    public string Name { get; init; } = "";
    public string Parameter { get; init; } = "";
    public string Value { get; init; } = "";
    public string Origin { get; init; } = "";

    public override string ToString() => JsonSerializer.Serialize<Tag>(this);

    public List<string> ToList() => new() { Name, Parameter, Value, Origin, Environment.NewLine };
}
