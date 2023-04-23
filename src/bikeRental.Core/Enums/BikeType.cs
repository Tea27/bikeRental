

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using System.Xml.Serialization;

namespace bikeRental.Core.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum BikeType
{
    Electric,
    Acoustic
}
