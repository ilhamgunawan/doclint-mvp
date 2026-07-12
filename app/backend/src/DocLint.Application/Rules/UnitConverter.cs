namespace DocLint.Application.Rules;

public static class UnitConverter
{
    public static double ToPoints(double value, string unit)
    {
        return unit.ToLowerInvariant() switch
        {
            "pt" => value,
            "in" => value * 72,
            "cm" => value * 28.3465,
            "mm" => value * 2.83465,
            _ => value
        };
    }
}
