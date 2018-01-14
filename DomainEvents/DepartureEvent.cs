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
    /// The departure Event simply captures the data and has a process method that simply
    /// forwards the event to an appropriate domain object (ship in this case)
    /// </summary>
    public class DepartureEvent : ShippingEvent
    {
        #region Internal Interface

        internal DepartureEvent(DateTime departureTime, Port port, Ship ship,TrackingType trackingType) : base(departureTime,port,ship,trackingType)
        {

        }

        internal override void Process()
        {
            Ship.HandleDeparture(this);
        }

        #endregion Internal Interface
    }
}
