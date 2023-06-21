using bikeRental.Application.Models.Order;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bikeRental.Application.Models.Bicycle
{
    public class BicycleModel : BaseResponseModel
    {
        [Required]
        public StationModel Station { get; set; }

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
        public BikeStatus Status { get; set; }

        public ICollection<OrderModel> Orders { get; set; }
    }
}
