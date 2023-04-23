using System.Collections.Generic;

namespace bikeRental.Core.Common
{
    public interface IAuditedEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public static TEntity UpdateCustom<TEntity>(TEntity dbEntity)
        //where TEntity : class
        //{

        //    dbEntity.GetType().GetProperty("UpdatedAt")?.SetValue(dbEntity, DateTime.Now, null);

        //    return dbEntity;
        //}
    }

}
