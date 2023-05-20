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

    [DisplayName("Number of acoustic bikes")]
    public int NumberOfBikes { get; set; }

    [DisplayName("Number of electric bikes")]
    public int NumberOfElectricBikes { get; set; }


}
