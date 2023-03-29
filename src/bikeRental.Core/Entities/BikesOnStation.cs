using bikeRental.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    internal class BikesOnStation : IAuditedEntity
    {
        public Guid BikeID { get; set; }

        public Guid StationID { get; set; }

        public Bicycle Bicycle { get; set; }

        public Station Station { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }
}
