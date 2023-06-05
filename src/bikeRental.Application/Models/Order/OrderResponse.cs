using bikeRental.Application.Models.Bicycle;
using bikeRental.Application.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bikeRental.Core.Identity;

namespace bikeRental.Application.Models.Order
{
    public class OrderResponse : BaseResponseModel
    {
        [DisplayName("Rental start time")]
        public DateTime RentalStartTime { get; set; }

        [DisplayName("Rental end time")]
        public DateTime RentalEndTime { get; set; }

        [DisplayName("Rental price")]
        public decimal RentalPrice { get; set; }

        public UserModel Customer{ get; set; }

        public BicycleModel Bicycle { get; set; }
    }
}

