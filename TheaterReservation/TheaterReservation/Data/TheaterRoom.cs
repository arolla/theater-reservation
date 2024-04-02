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

    public void SetZones(Zone[] zones)
    {
        this.zones = zones;
    }
}