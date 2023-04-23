using bikeRental.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    public class Station : BaseEntity, IAuditedEntity
    {
        public string Address { get; set; }

        public int NumberOfBikes { get; set; }

        public int NumberOfElectricBikes { get; set; }

        public ICollection<Bicycle> Bicycles { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
