using System.Text;

namespace TheaterReservation.Domain
{
    public class TheaterService
    {
        // pattern sandwich ?
        // agrégats : TheatreTopology, Reservation
        // bounded contexts différents : Seat (topology, seat contains category)
        // vs Seat (reservation aka "Location", associated with a performance)

        public String reservation(int reservationCount, String reservationCategory)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<reservation>\n");

            // get reservation id
            callDatabaseOrApi("getReservationId");
            String res_id = "123456";
            sb.Append("\t<reservationId>").Append(res_id).Append("</reservationId>\n");

            // get seats (based on avaialble, count and category)
            callDatabaseOrApi("availablesSeats");

            // algo
            List<String> seatNames = new List<string>{ "1A", "2A", "3A", "4A" };

            sb.Append("\t<seats>\n");
            foreach (String s in seatNames)
            {
                sb.Append("\t\t<seat>").Append(s).Append("</seat>\n");
            }

            // calculate raw price

            decimal intialprice = 150.00m;

            // check and apply discounts and fidelity program
            callDatabaseOrApi("checkDiscountForDate");
            callDatabaseOrApi("checkCustomerFidelityProgram");

            // est-ce qu'il a un abonnement ou pas ?
            decimal removePercent = 0.175m;
            decimal one =1.00m;
            decimal totalBilling = (one- removePercent) * intialprice;
            String total = totalBilling + "€";

            // emit reservation summary
            sb.Append("\t<seatCategory>").Append(reservationCategory).Append("</seatCategory>\n");
            sb.Append("\t</seats>\n");
            sb.Append("\t<totalAmountDue>").Append(total).Append("</totalAmountDue>\n");
            sb.Append("</reservation>\n");
            return sb.ToString();
        }

        private Object callDatabaseOrApi(String usecase)
        {
            return null;
        }

        public static void main(String[] args)
        {
            Console.WriteLine(new TheaterService().reservation(4, "STANDARD"));
        }
    }
}