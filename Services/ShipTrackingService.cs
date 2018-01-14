using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingService.DomainEvents;
using TrackingService.Models;

namespace TrackingService.Services
{
    [System.ComponentModel.DefaultEvent("ShipTracked")]
    public class ShipTrackingService
    {
        #region Event Declaration

        public delegate void ShipTrackedEventHandler(object sender, ShipTrackedEventArgs e);
        public event ShipTrackedEventHandler ShipTracked;

        #endregion Event Declaration

        #region Private Storage

        private TrackingType _trackingType = TrackingType.None;
        private Guid _trackingServiceId;
        private DateTime _recorded;
        private List<Port> _ports;
        private List<Ship> _ships;
        private Ship _trackedShip;
        private Port _currentPort;
        private Port _setPort;

        #endregion Private Storage

        #region Public Properties

        public TrackingType TrackingType
        {
            get { return _trackingType; }
            set { _trackingType = value; }
        }

        public Guid TrackingServiceId
        {
            get { return _trackingServiceId; }
            set { _trackingServiceId = value; }
        }

        public DateTime Recorded
        {
            get { return _recorded; }
            set { _recorded = value; }
        }
        
        public List<Port> Ports
        {
            get { return _ports; }
            set { _ports = value; }
        }

        public List<Ship> Ships
        {
            get { return _ships; }
            set { _ships = value; }
        }

        public Ship TrackedShip
        {
            get { return _trackedShip; }
            set { _trackedShip = value; }
        }

        public Port CurrentPort
        {
            get { return _currentPort; }
            set { _currentPort = value; }
        }

        public Port SetPort
        {
            get { return _setPort; }
            set { _setPort = value; }
        }

        #endregion Public Properties

        #region C'tor

        public ShipTrackingService()
        {
            // initialize ports
            _ports = new List<Port>()
            {

                new Port()
                {
                    PortId = 0, Name = "AT Sea"
                },
                new Port()
                {
                    PortId = 1, Name = "Port of Shangai"
                },
                new Port()
                {
                    PortId = 2, Name = "Port of Antwerp"
                },
                new Port()
                {
                    PortId = 3, Name = "Port of Singapore"
                },
                new Port()
                {
                    PortId = 4, Name = "Port of Dover"
                }

            };

            // initialize ships
            _ships = new List<Ship>()
            {
                new Ship()
                {
                    ShipId = 1, Name = "Ship_1", Location = _ports[0]
                },
                new Ship()
                {
                    ShipId = 2, Name = "Ship_2", Location = _ports[0]
                }
                ,new Ship()
                {
                    ShipId = 3, Name = "Ship_3", Location = _ports[0]
                },
                new Ship()
                {
                    ShipId = 4, Name = "Ship_4", Location = _ports[0]
                }
            };
        }

        #endregion C'tor

        #region Public Interface

        public void RecordTracking(EventProcessor<ShippingEvent> eProc)
        {
            // Create event depending on TrackingType
            Port OldLocation = TrackedShip.Location;
            ShippingEvent ev;
            if (TrackingType == TrackingType.Arrival)
            {
                ev = new ArrivalEvent(DateTime.Now, SetPort, TrackedShip,TrackingType);
            }
            else
            {
                ev = new DepartureEvent(DateTime.Now, SetPort, TrackedShip, TrackingType);
            }

            // send the event to the event handler (ship) which will update it's status on the provided event data
            eProc.ProcessEvent(ev);

            // notify the UI Tracking List so it can update itself
            ShipTrackedEventArgs args = new ShipTrackedEventArgs()
            {
                TrackingServiceId = TrackingServiceId,
                Recorded = Recorded,
                TrackingType = TrackingType,
                TrackedShip = TrackedShip,
                OldLocation = OldLocation,
                NewLocation = SetPort,

            };
            // notify subscribers ...
            OnShipTracked(args);
        }

        #endregion Public Interface

        #region Protected Interface

        /// <summary>
        /// Notify the (UI) Subscribders that a Ship has been tracked
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnShipTracked(ShipTrackedEventArgs args)
        {

            if (ShipTracked != null)
                ShipTracked(this, args);

        }

        #endregion Protected Interface
    }
}
