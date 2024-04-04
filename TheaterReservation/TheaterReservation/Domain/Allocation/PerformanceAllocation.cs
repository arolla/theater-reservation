using TheaterReservation.Data;
using TheaterReservation.Domain.Topology;

namespace TheaterReservation.Domain.Allocation
{
    internal class PerformanceAllocation
    {
        private readonly TheaterTopology theaterTopology;

        private readonly List<String> freeSeats;


        public PerformanceAllocation(TheaterTopology theaterTopology, List<String> freeSeats)
        {
            this.theaterTopology = theaterTopology;
            this.freeSeats = freeSeats;
        }


        public int GetTotalSeatCount()
        {
            return theaterTopology.GetTotalSeatCount();
        }

        public int GetFreeSeatCount()
        {
            return freeSeats.Count;
        }

        public List<ReservationSeat> FindSeatsForReservation(int reservationCount, String reservationCategory)
        {
            var reservationSeats = theaterTopology.Rows
                .Select(row => row.FindSeatsForReservation(reservationCount, reservationCategory, freeSeats))
                .FirstOrDefault(seatTopologies => seatTopologies.Count != 0);
            
            reservationSeats ??= new List<SeatTopology>();
            
            var result = reservationSeats
                .Select(seatTopology =>
                    new ReservationSeat(seatTopology.SeatReference, seatTopology.category))
                .ToList();

            return result;
        }
    }
}
