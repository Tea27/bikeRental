using bikeRental.Core.Common;
using bikeRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Models.Station;
public class StationResponse : BaseResponseModel
{
    public string Address { get; set; }

    public int NumberOfBikes { get; set; }

    public int NumberOfElectricBikes { get; set; }

    public ICollection<Bicycle> Bicycles { get; set; }

}
