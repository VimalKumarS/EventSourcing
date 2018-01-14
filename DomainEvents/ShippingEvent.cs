using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Models;
using TrackingService.Services;
namespace TrackingService.DomainEvents
{
    // The ShippingEvent enherits from the base DomainEvent
    // And adds logic to keep track of the Ship, Port and Trackingtype (Arrival,Departure)
    public abstract class ShippingEvent : DomainEvent
    {

        #region Private Storage

        private Port _port;
        private Ship _ship;
        private TrackingType _trackingType;

        #endregion Private Storage

        #region Public Properties

        public Port Port
        {
            get { return _port; }
            set { _port = value; }
        }
        
        public Ship Ship
        {
            get { return _ship; }
            set { _ship = value; }
        }
        
        public TrackingType TrackingType
        {
            get { return _trackingType; }
            set { _trackingType = value; }
        }

        #endregion Public Properties

        #region Internal Interface

        internal ShippingEvent(DateTime occured, Port port, Ship ship,TrackingType trackingType) : base(occured)
        {
            this._port = port;
            this._ship = ship;
            this._trackingType = trackingType;
        }

        #endregion Internal Interface

        #region Public Interface

        public override string ToString()
        {
            return $"TrackingType: {this.TrackingType} Ship: {this.Ship.Name} Port: {this.Port.Name}";
        }

        #endregion Public Interface

    }
}
