using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Models.Station;
public class StationModel : BaseResponseModel
{
    [Required]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "The {0} exceeded maximum input value {1}")]
   // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only text is allowed")]
    public string Address { get; set; }

    [Required]
    [DisplayName("Number of Acoustic")]
    [Range(0, int.MaxValue, ErrorMessage = "The {0} field must be greater than or equal to {1}.")]
    //  [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
    public int NumberOfBikes { get; set; }

    [Required]
    //[RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed.")]
    [DisplayName("Number of Electric")]
    [Range(0, int.MaxValue, ErrorMessage = "The {0} field must be greater than or equal to {1}.")]
    public int NumberOfElectricBikes { get; set; }
}

