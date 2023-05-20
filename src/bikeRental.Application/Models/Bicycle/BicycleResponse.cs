using System.Collections.Generic;
using System.ComponentModel;

namespace bikeRental.Application.Models.Bicycle
{
    public class BicycleResponse : BaseResponseModel
    {
        public string Address { get; set; }

        [DisplayName("Number of Bicycles")]
        public int NumberOfBicycles { get; set; }

        [DisplayName("Number of Electric Bicycles")]
        public int NumberOfElectricBicycles { get; set; }

    }
}
