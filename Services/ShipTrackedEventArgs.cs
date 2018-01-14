using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Models;
namespace TrackingService.Services
{
    /// <summary>
    /// Holds the properties of the Ship TrackingService
    /// that are exposed through event-handeling
    /// </summary>
    public class ShipTrackedEventArgs : EventArgs
    {
        #region Public Properties

        public Guid TrackingServiceId { get; set; }
        public DateTime Recorded { get; set; }
        public TrackingType TrackingType { get; set; }
        public Ship TrackedShip { get; set; }
        public Port OldLocation { get; set; }
        public Port NewLocation { get; set; }

        #endregion Public Properties
    }
}
