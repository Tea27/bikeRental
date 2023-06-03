using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Models.User;
using bikeRental.Core.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Models.Order
{
    public class OrderModel : BaseResponseModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Rental start time")]
        public DateTime RentalStartTime { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Rental end time")]
        public DateTime RentalEndTime { get; set; }

        [Required]
        [DisplayName("Rental price")]
        [Range(0, double.MaxValue, ErrorMessage = "The {0} field must be greater than or equal to {1}.")]
        public decimal RentalPrice { get; set; }

        public UserModel Customer { get; set; }

        public BicycleModel Bicycle { get; set; }
    }
}
