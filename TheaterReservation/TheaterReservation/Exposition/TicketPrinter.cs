using System.Net.Http.Headers;
using System.Text;
using TheaterReservation.Data;
using TheaterReservation.Domain.Reservation;

namespace TheaterReservation.Exposition
{
    public class TicketPrinter
    {
        private readonly ReservationAgent _reservationAgent;

        public TicketPrinter(ReservationAgent reservationAgent)
        {
            this._reservationAgent = reservationAgent;
        }
        public String Reservation(Int64 customerId, int reservationCount, String reservationCategory, Performance performance)
        {
            var reservationRequest = this._reservationAgent.Reserve(customerId, reservationCount, reservationCategory, performance);
            return ToXml(reservationRequest);
        }
        private static string ToXml(ReservationRequest reservationRequest)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<reservation>\n");
            sb.Append("\t<performance>\n");
            sb.Append("\t\t<play>").Append(reservationRequest.GetPerformanceTitle()).Append("</play>\n");
            sb.Append("\t\t<date>").Append(reservationRequest.GetStartDate()).Append("</date>\n");
            sb.Append("\t</performance>\n");
            sb.Append("\t<reservationId>").Append(reservationRequest.ReservationId).Append("</reservationId>\n");
            if (reservationRequest.IsFulfillable())
            {
                sb.Append("\t<reservationStatus>FULFILLABLE</reservationStatus>\n");
                sb.Append("\t\t<seats>\n");
                foreach (var reservedSeat in reservationRequest.ReservedSeats)
                {
                    sb.Append("\t\t\t<seat>\n");
                    sb.Append("\t\t\t\t<id>").Append(reservedSeat.Seat).Append("</id>\n");
                    sb.Append("\t\t\t\t<category>").Append(reservedSeat.Category).Append("</category>\n");
                    sb.Append("\t\t\t</seat>\n");
                }

                sb.Append("\t\t</seats>\n");
            }
            else
            {
                sb.Append("\t<reservationStatus>ABORTED</reservationStatus>\n");
            }

            sb.Append("\t<seatCategory>").Append(reservationRequest.ReservationCategory).Append("</seatCategory>\n");
            sb.Append("\t<totalAmountDue>").Append(reservationRequest.TotalBilling).Append("</totalAmountDue>\n");
            sb.Append("</reservation>\n");
            return sb.ToString();
        }
    }
}