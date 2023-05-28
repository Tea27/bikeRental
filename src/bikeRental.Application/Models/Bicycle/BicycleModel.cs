using bikeRental.Application.Models.Order;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bikeRental.Application.Models.Bicycle
{
    public class BicycleModel : BaseResponseModel
    {
        [Required]
        public BikeType Type { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "The {0} exceeded maximum input value {1}")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Price")]
        [Range(0, double.MaxValue, ErrorMessage = "The {0} field must be greater than or equal to {1}.")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Status")]
        public bool IsAvailable { get; set; }

        public ICollection<OrderModel> Orders { get; set; }

        [Required]
        public StationModel Station { get; set; }
    }
}
