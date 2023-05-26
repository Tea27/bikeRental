using bikeRental.Core.Common;
using bikeRental.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    public class Order : BaseEntity
    {

        public DateTime RentalStartTime { get; set; }

        public DateTime RentalEndTime { get; set; }

        public decimal RentalPrice { get; set; }

        public ApplicationUser Customer { get; set; }

        public Bicycle Bicycle { get; set; }

    }
}
