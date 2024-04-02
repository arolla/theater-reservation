namespace TheaterReservation.Dao;

public class PerformancePriceDao
{
    // simulates a performance pricing repository
    public decimal fetchPerformancePrice(Int64 performanceId)
    {
        if (performanceId == 1L)
        {
            return 35.00m;
        }
        else
        {
            return 28.50m;
        }
    }
}