using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingService.Models
{
    public class Port
    {
        #region Properties

        public int PortId { get; set; }
        public string Name { get; set; }

        #endregion Properties
        
        #region Public Interface

        public override string ToString()
        {
            return Name;
        }
        
        #endregion Public Interface
    }
}
