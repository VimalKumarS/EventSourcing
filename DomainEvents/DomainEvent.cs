using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingService.DomainEvents
{
    /// <summary>
    /// Domain event is the base event class
    /// it simply registers when the according 
    /// event occured and is recorded
    /// </summary>
    public abstract class DomainEvent
    {
        #region Private Storage

        private DateTime _recorded, _occured;

        #endregion Private Storage

        #region Internal Interface

        internal DomainEvent(DateTime occured)
        {
            this._occured = occured;
            this._recorded = DateTime.Now;
        }

        abstract internal void Process();

        #endregion Internal Interface
    }
}
