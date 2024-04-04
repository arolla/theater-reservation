namespace TheaterReservation.Data;

public class Zone
{
    private Row[] rows;

    private String category; // STANDARD, PREMIUM

    public Zone(Row[] rows, String category)
    {
        this.rows = rows;
        this.category = category;
    }

    public Row[] GetRows()
    {
        return rows;
    }
    
    public String GetCategory()
    {
        return category;
    }
}