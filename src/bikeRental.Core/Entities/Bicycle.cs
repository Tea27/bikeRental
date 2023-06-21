using bikeRental.Core.Common;
using bikeRental.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Entities
{
    public class Bicycle : BaseEntity
    {
        public BikeType Type { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public BikeStatus Status { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Station Station { get; set; } 

    }
}
