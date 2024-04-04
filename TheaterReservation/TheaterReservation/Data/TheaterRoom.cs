namespace TheaterReservation.Data;

public class TheaterRoom
{
    private Zone[] zones;

    public TheaterRoom(Zone[] zones)
    {
        this.zones = zones;
    }

    public Zone[] GetZones()
    {
        return zones;
    }
    public List<Seat> GetAllSeats()
    {
        return zones.SelectMany(z =>
            z.GetRows().SelectMany(r => r.GetSeats()))
            .ToList();
    }

    public List<String> GetFreeSeats()
    {
        return GetAllSeats().Where(s => s.isFree())
            .Select(s => s.GetSeatId())
            .ToList();
    }
}