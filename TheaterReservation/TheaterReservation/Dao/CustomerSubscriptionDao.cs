using System;

namespace TheaterReservation.Dao
{
    public class CustomerSubscriptionDao
    {
        // simulates fetching data From Customer advantages
        public bool FetchCustomerSubscription(Int64 customerId)
        {
            bool isSubscribed = false;
            if (customerId == 1L)
            {
                isSubscribed = true;
            }
            return isSubscribed;
        }
    }
}
