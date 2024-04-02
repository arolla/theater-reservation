namespace TheaterReservation.Dao;

public class PerformancePriceDao
{
    // simulates a performance pricing repository
    public decimal FetchPerformancePrice(Int64 performanceId)
    {
        return performanceId == 1L
            ? 35.00m
            : 28.50m;
    }
}