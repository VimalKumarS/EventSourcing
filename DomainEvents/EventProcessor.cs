using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingService.DomainEvents
{
    /// <summary>
    /// The Event Processor Processes the Events 
    /// as received from the TrackingService
    /// </summary>
    public class EventProcessor<T>  where T : ShippingEvent
    {
        #region Private Storage

        private IList<T> _eventLogger = new List<T>();

        #endregion Private Storage


        #region Public Interface

        public void ProcessEvent(T e)
        {
            e.Process();
            _eventLogger.Add(e);
        }

        public int CountEventLogEntries()
        {
            return _eventLogger.Count;
        }

        public List<T> GetEvents()
        {
            return _eventLogger as List<T>;
        }

        #endregion Public Interface
    }
}
