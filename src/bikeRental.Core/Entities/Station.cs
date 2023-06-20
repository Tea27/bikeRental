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
        public double lattitude { get; set; }
        public double longitude { get; set; }
        public ICollection<Bicycle> Bicycles { get; set; }

    }
}
