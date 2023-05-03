using bikeRental.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    public class Station : BaseEntity
    {
        public string Address { get; set; }

        public int NumberOfBikes { get; set; }

        public int NumberOfElectricBikes { get; set; }

        public ICollection<Bicycle> Bicycles { get; set; }

    }
}
