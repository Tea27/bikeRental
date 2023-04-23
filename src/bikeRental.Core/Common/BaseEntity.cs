using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bikeRental.Core.Common
{
    public abstract class BaseEntity
    {
       // [Key]
       //DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
    }
}
