using bikeRental.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    internal class Order : BaseEntity, IAuditedEntity
    {
        public Guid CustomerID { get; set; }

        public Guid BikeID { get; set; }

        public DateTime RentalStartTime { get; set; }

        public DateTime RentalEndTime { get; set; }

        public string RentalPrice { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
