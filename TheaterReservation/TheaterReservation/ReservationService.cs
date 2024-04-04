using TheaterReservation.Dao;
using TheaterReservation.Domain.Reservation;

namespace TheaterReservation;


// ach, a good old singleton
public class ReservationService
{

    private static Int64 currentId = 123_455;
    public static String InitNewReservation()
    {
        currentId++;
        return currentId.ToString();
    }

    public static void UpdateReservation(Reservation reservation)
    {
        new ReservationDao().Update(reservation);
    }

    public static Reservation FindReservation(Int64 reservationId)
    {
        return new ReservationDao().Find(reservationId);
    }

    public static void CancelReservation(Int64 reservationId)
    {
        Reservation reservation = new ReservationDao().Find(reservationId);
        reservation.SetStatus("CANCELLED");
        reservation.SetSeats(new String[0]);
        new ReservationDao().Update(reservation);
    }
}