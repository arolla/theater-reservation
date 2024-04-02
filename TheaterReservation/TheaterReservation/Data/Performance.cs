using String = System.String;

namespace TheaterReservation.Data
{
    public class Performance
    {
        public Int64 id;
     
        public String play; // "The CICD - Corneille", "Les fourberies de Scala - Molière"

        public DateTime startTime;

        public DateTime endTime;

        public String performanceNature; // can be "PREVIEW", "PREMIERE", etc
    }
}
