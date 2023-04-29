using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bikeRental.Core.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
