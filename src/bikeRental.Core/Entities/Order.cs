using bikeRental.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    public class Order : BaseEntity, IAuditedEntity
    {

        public DateTime RentalStartTime { get; set; }

        public DateTime RentalEndTime { get; set; }

        public string RentalPrice { get; set; }

        public Customer Customer { get; set; }

        public Bicycle Bicycle { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; } 

        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
