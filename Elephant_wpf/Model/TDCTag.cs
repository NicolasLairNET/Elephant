

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Elephant.Model
{
    public class TDCTag
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
        public string Origin { get; set; }

        public override string ToString() => JsonSerializer.Serialize<TDCTag>(this);

        public List<string> ToList() => new() { Name, Parameter, Value, Origin, Environment.NewLine };
    }
}
