using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.Core.Common;
using bikeRental.Core.Identity;

namespace bikeRental.Core.Entities
{
    public class Customer : ApplicationUser, IAuditedEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public string Password { get; set; }

        public ICollection<Order> Orders { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
