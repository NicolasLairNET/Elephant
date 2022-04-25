using System.Text.Json;

namespace Elephant.Model;
public record Tag
{
    public string Name { get; init; } = "";
    public string Parameter { get; init; } = "";
    public string Value { get; init; } = "";
    public string Origin { get; init; } = "";
    public List<string> ToList() => new() { Name, Parameter, Value, Origin, Environment.NewLine };
}
