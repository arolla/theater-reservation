using TheaterReservation.Data;
using TheaterReservation.Domain.Topology;

namespace TheaterReservation.Domain.Allocation
{
    internal class PerformanceAllocation
    {
        private readonly TheaterTopology theaterTopology;

        private readonly List<String> freeSeats;
        private readonly int requestedSeatCount;
        private readonly string reservationCategory;


        public PerformanceAllocation(TheaterTopology theaterTopology, List<String> freeSeats, int requestedSeatCount, String reservationCategory)
        {
            this.theaterTopology = theaterTopology;
            this.freeSeats = freeSeats;
            this.requestedSeatCount = requestedSeatCount;
            this.reservationCategory = reservationCategory;
        }


        public int GetTotalSeatCount()
        {
            return theaterTopology.GetTotalSeatCount();
        }

        public int GetFreeSeatCount()
        {
            return freeSeats.Count;
        }

        public List<ReservationSeat> FindSeatsForReservation()
        {
            var reservationSeats = theaterTopology.Rows
                .Select(row => row.FindSeatsForReservation(requestedSeatCount, reservationCategory, freeSeats))
                .FirstOrDefault(seatTopologies => seatTopologies.Count != 0);

            reservationSeats ??= new List<SeatTopology>();

            var result = reservationSeats
                .Select(seatTopology =>
                    new ReservationSeat(seatTopology.SeatReference, seatTopology.category))
                .ToList();

            return result;
        }
        public PerformanceInventory GetPerformanceInventory()
        {
            return new PerformanceInventory(GetFreeSeatCount() - requestedSeatCount, GetTotalSeatCount());
        }
    }
}
