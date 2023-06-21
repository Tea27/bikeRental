using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace bikeRental.Core.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum BikeStatus
{
    [Display(Name = "Available")]
    Available,
    [Display(Name = "InUse")]
    InUse,
    [Display(Name = "Disabled")]
    Disabled
}
