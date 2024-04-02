using System;

namespace TheaterReservation.Dao
{
    public class CustomerSubscriptionDao
    {
        // simulates fetching data from Customer advantages
        public bool fetchCustomerSubscription(Int64 customerId)
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
