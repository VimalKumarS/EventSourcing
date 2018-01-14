using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackingService.Models;
using System.Timers;
using System.Diagnostics;
using TrackingService.Services;
using TrackingService.DomainEvents;

namespace TrackingService
{
    public partial class FormShipTrackingService : Form
    {
        #region Storage

        private TrackingService.Services.ShipTrackingService _trackingService;
        private EventProcessor<ShippingEvent> _eventProcessor;
        private int _selectedShipId = -1;
        private int _selectedPortId = -1;
        private Random _randomShip = new Random();
        private Random _randomPort = new Random();

        #endregion Storage

        public FormShipTrackingService()
        {
            InitializeComponent();
        }

        

        private void FormShipTrackingService_Load(object sender, EventArgs e)
        {
            try
            {
                // create the tracking service
                _trackingService = new Services.ShipTrackingService();

                // create the eventprocessor
                _eventProcessor = new EventProcessor<ShippingEvent>();

                // subscribe to the ship tracked event of the tracking service
                _trackingService.ShipTracked += _trackingService_ShipTracked;

                this.SetDataSource();
                SetTimer();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Update the UI after RecordTracked has been tracked in TrackingService
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _trackingService_ShipTracked(object sender, ShipTrackedEventArgs e)
        {
            this.TrackingOccuredTextBox.Text +=
            $"TrackingId: {e.TrackingServiceId}\r\n" +
            $"RecordedAt: {e.Recorded.ToLongTimeString()}\r\n" +
            $"TrackingType: {e.TrackingType}\r\n" +
            $"Ship: {e.TrackedShip.Name} Id: {e.TrackedShip.ShipId}\r\n" +
            $"Current Location : {e.OldLocation.Name}\r\n" +
            $"New Location : {e.NewLocation.Name}" +
            "\r\n\r\n";
        }

        private void SetDataSource()
        {
            _shipsBindingSource.DataSource = null;
            _shipsBindingSource.DataSource = _trackingService.Ships;
        }

        /// <summary>
        /// we simulate ship arrival/departure every 2 seconds
        /// </summary>
        private void SetTimer()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 2000;
            _timer.Tick += _timer_Tick;
            _timer.Enabled = true;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if(_trackingService != null)
            {
                // we simulate tracking events by selecting a RANDOM ship
                // next tracking type is set to Arrival or Departure depending on the 
                // current location of the selected ship
                // if port of selected ship == "AT SEA" then we set the tracking event type
                // as an ARRIVAL and will set ship port to the next port (!= 0) in the Port list
                // if port of selected ship != "AT SEA" then we set the tracking event type
                // as a DEPARTURE and set port to "AT SEA"

                int maxShip = _trackingService.Ships.Count;

                // select a random ship in the list
                _selectedShipId = _randomShip.Next(1, maxShip); 

                // set tracked ship to the current selected id
                _trackingService.TrackedShip = _trackingService.Ships[_selectedShipId];

                // set the tracking event type (Arrival or Departure) depening on the current location (PortId 0 is AT-SEA)
                _trackingService.TrackingType = _trackingService.TrackedShip.Location.PortId == 0 ? TrackingType.Arrival : TrackingType.Departure;

                // set the time of tracking recording
                _trackingService.Recorded = DateTime.Now;

                // create a unique id for the tracking
                _trackingService.TrackingServiceId = Guid.NewGuid();

                // set the new port of the tracking, this is a random port in
                // the list in case of arrival 
                // or port 0 = AT SEA in case of departure
                if(_trackingService.TrackingType == TrackingType.Arrival)
                {
                    int maxPort = _trackingService.Ports.Count;
                    _selectedPortId = _randomPort.Next(1, maxPort);
                }
                else
                {
                    _selectedPortId = 0;
                }

                _trackingService.SetPort = _trackingService.Ports[_selectedPortId];

                // handle the tracking by the tracking service
                // the tracking service will now send an Arrival event or Departure event to the Ship
                _trackingService.RecordTracking(_eventProcessor);
                

                // augment number of events
                _numberOfEventsCount.Text = _eventProcessor.CountEventLogEntries().ToString();

                // refresh
                this.SetDataSource();

            }
        }


        private void FormShipTrackingService_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
        }

        private void toolStripButtonShowEvents_Click(object sender, EventArgs e)
        {
            this._eventsTextBox.Text = string.Empty;

            // show the events for the selected ship

            try
            {
                // first get the selected ship

                Ship currentShip = (Ship)this._shipsBindingSource.Current;

                if(currentShip != null)
                {
                    // get the event logs
                    List<ShippingEvent> events = _eventProcessor.GetEvents() as List<ShippingEvent>;

                    // filter events for the current ship
                    var filterByShip = events.Where(ev => ev.Ship.ShipId == currentShip.ShipId);

                    foreach (ShippingEvent ev in filterByShip)
                    {
                        this._eventsTextBox.Text += ev.ToString() + "\r\n";
                    }
                   
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
