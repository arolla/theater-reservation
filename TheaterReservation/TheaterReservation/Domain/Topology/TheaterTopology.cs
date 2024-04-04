using TheaterReservation.Data;

namespace TheaterReservation.Domain.Topology
{
    public class TheaterTopology
    {
        public List<RowTopology> Rows { get; }

        public TheaterTopology(List<RowTopology> rows)
        {
            this.Rows = rows;
        }

        public List<SeatTopology> GetAllSeats()
        {
            return Rows.SelectMany(r => r.seats).ToList();
        }

        public int GetTotalSeatCount()
        {
            return GetAllSeats().Count;
        }

        public static TheaterTopology From(TheaterRoom theaterRoom)
        {
            var rows = theaterRoom.GetZones()
                .SelectMany(zone =>
                    zone.GetRows()
                    .Select(row =>
                        new RowTopology(
                            row.GetSeats()
                                .Select(seat =>
                                    new SeatTopology(seat.GetSeatId(), zone.GetCategory()))
                                .ToList()
                        )))
                .ToList();

            return new TheaterTopology(rows);
        }
    }
}
