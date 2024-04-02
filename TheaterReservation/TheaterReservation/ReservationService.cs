using TheaterReservation.Dao;
using TheaterReservation.Data;

namespace TheaterReservation;


// ach, a good old singleton
public class ReservationService
{

    private static Int64 currentId = 123_455;
    public static String initNewReservation()
    {
        currentId++;
        return currentId.ToString();
    }

    public static void updateReservation(Reservation reservation)
    {
        new ReservationDao().update(reservation);
    }

    public static Reservation findReservation(Int64 reservationId)
    {
        return new ReservationDao().find(reservationId);
    }

    public static void cancelReservation(Int64 reservationId)
    {
        Reservation reservation = new ReservationDao().find(reservationId);
        reservation.setStatus("CANCELLED");
        reservation.setSeats(new String[0]);
        new ReservationDao().update(reservation);
    }
}