using System.Text.Json.Serialization;

namespace DocLint.Application.Models;

public class RuleCollection
{
    [JsonPropertyName("rules")]
    public List<RuleDefinition> Rules { get; set; } = new();
}

public class RuleDefinition
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("constraints")]
    public RuleConstraints Constraints { get; set; } = null!;

    [JsonPropertyName("pageSelector")]
    public PageSelectorModel PageSelector { get; set; } = null!;

    [JsonPropertyName("severity")]
    public string Severity { get; set; } = string.Empty;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}

public class RuleConstraints
{
    [JsonPropertyName("fontFamily")]
    public string? FontFamily { get; set; }

    [JsonPropertyName("fontSize")]
    public ConstraintValue? FontSize { get; set; }

    [JsonPropertyName("top")]
    public ConstraintValue? Top { get; set; }

    [JsonPropertyName("bottom")]
    public ConstraintValue? Bottom { get; set; }

    [JsonPropertyName("left")]
    public ConstraintValue? Left { get; set; }

    [JsonPropertyName("right")]
    public ConstraintValue? Right { get; set; }

    [JsonPropertyName("width")]
    public ConstraintValue? Width { get; set; }

    [JsonPropertyName("height")]
    public ConstraintValue? Height { get; set; }

    [JsonPropertyName("orientation")]
    public string? Orientation { get; set; }
}

public class ConstraintValue
{
    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("unit")]
    public string Unit { get; set; } = string.Empty;
}

public class PageSelectorModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("start")]
    public int? Start { get; set; }

    [JsonPropertyName("end")]
    public int? End { get; set; }
}
