using bikeRental.Application.Models.Bicycle;
using bikeRental.Core.Common;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Models.Station;
public class StationResponse : BaseResponseModel
{
    public string Address { get; set; }

    public ICollection<BicycleModel> Bicycles { get; set; }
}



