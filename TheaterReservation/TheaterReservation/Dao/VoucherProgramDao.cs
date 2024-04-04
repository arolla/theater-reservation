namespace TheaterReservation.Dao;

public class VoucherProgramDao
{
    public static decimal FetchVoucherProgram(DateTime now)
    {
        // applies From reservation date, not performance date
        decimal voucher = 0m;
        if (now < new DateTime(2023, 04, 30))
        {
            voucher = Convert.ToDecimal("0.20");
        }

        return voucher;
    }
}