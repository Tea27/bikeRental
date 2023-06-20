using bikeRental.Application.Models.Bicycle;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bikeRental.Application.Models.Station;
public class StationModel : BaseResponseModel
{
    [Required]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "The {0} exceeded maximum input value {1}")]
   // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only text is allowed")]
    public string Address { get; set; }

    [Required]
    [DisplayName("Lattitude")]
    [Range(0, double.MaxValue, ErrorMessage = "The {0} field must be greater than or equal to {1}.")]
    public double lattitude { get; set; }
    [Required]
    [DisplayName("Longitude")]
    [Range(0, double.MaxValue, ErrorMessage = "The {0} field must be greater than or equal to {1}.")]
    public double longitude { get; set; }

    public ICollection<BicycleModel> Bicycles { get; set; }
}

