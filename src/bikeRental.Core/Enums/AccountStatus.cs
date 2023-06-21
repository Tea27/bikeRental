using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace bikeRental.Core.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum AccountStatus
{
    [Display(Name = "Active")]
    Active,
    [Display(Name = "Disabled")]
    Disabled
}
