using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Models;
using TrackingService.Services;
namespace TrackingService.DomainEvents
{
    /// <summary>
    /// The arrival Event simply captures the data and has a process method that simply
    /// forwards the event to an appropriate domain object (ship in this case)
    /// </summary>
    public class ArrivalEvent : ShippingEvent
    {

        #region Internal Interface
        
        internal ArrivalEvent(DateTime arrivalTime, Port port, Ship ship, TrackingType trackingType) : base(arrivalTime, port, ship,trackingType)
        {
        }

        internal override void Process()
        {
            Ship.HandleArrival(this);
        }

        #endregion Internal Interface
    }
}
