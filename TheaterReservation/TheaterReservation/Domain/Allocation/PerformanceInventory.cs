namespace TheaterReservation.Domain.Allocation;

public class PerformanceInventory
{
    public PerformanceInventory(double availableSeatsCount, double totalSeatsCount)
    {
        AvailableSeatsCount = availableSeatsCount;
        TotalSeatsCount = totalSeatsCount;
    }

    public double TotalSeatsCount { get; }
    public double AvailableSeatsCount { get; }
}