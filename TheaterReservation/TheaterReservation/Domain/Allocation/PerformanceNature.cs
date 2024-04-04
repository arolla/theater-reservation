namespace TheaterReservation.Domain.Allocation;

public class PerformanceNature
{
    public string Value { get; }

    public PerformanceNature(string value)
    {
        Value = value;
    }
}