using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Enums
{
    public enum OrderStatus
    {
        PendingPayment,
        PaymentConfirmed,
        PendingRestaurantConfirmation,
        AcceptedByRestaurant,
        DeclinedByRestaurant,
        OrderPrepared,
        AssignedToDriver,
        DriverAtRestaurant,
        OrderInTransit,
        DriverAtAddress,
        OrderDelivered
    }

}