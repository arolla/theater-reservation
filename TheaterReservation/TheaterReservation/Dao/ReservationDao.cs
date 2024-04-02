using TheaterReservation.Data;

namespace TheaterReservation.Dao;

public class ReservationDao
{

    private static Dictionary<Int64, Reservation> reservationMap = new Dictionary<Int64, Reservation>();
    public void Update(Reservation reservation)
    {
        if (reservationMap.ContainsKey(reservation.GetReservationId()))
        {
            reservationMap.Remove(reservation.GetReservationId());
        }
        reservationMap.Add(reservation.GetReservationId(), reservation);
    }

    public Reservation Find(Int64 reservationId)
    {
        return reservationMap[reservationId];
    }
}