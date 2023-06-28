using bikeRental.Core.Entities;
using bikeRental.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.DataAccess.Repositories.Impl
{
    public class OrderRepository<TEntity> : IOrderRepository<TEntity> where TEntity : Order
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public OrderRepository(DatabaseContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity, Guid customerId, Guid bicycleId)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = Guid.NewGuid();

            var customer = await _context.Users.FindAsync(customerId);
            if (customer == null)
            {
                throw new InvalidOperationException("The associated Customer does not exist.");
            }
            entity.Customer = customer;

            var bicycle = await _context.Bicycles.FindAsync(bicycleId);
            if (bicycle == null)
            {
                throw new InvalidOperationException("The associated Bicycle does not exist.");
            }
            entity.Bicycle = bicycle;


            var addedEntity = (await DbSet.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return addedEntity;
        }



        public async Task<TEntity> GetByIdAsync(Guid? id)
        {
            return await FindByCondition(order => order.Id.Equals(id)).FirstOrDefaultAsync();

        }

        public IQueryable<TEntity> GetAll()
        {
            var orders = DbSet.Include(o => o.Customer).Include(o => o.Bicycle).ThenInclude(b => b.Station).AsQueryable();         
            return orders;
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Include(o => o.Customer).Include(o => o.Bicycle).ThenInclude(b => b.Station).Where(expression).AsNoTracking();
        }

       
        public async Task<IEnumerable<TEntity>> GetByCustomer(Guid? customerId)
        {
            return await FindByCondition(TEntity => TEntity.Customer.Id.Equals(customerId)).ToListAsync();
        }
    
        public async Task<IEnumerable<TEntity>> GetByBicycle(Guid? bicycleId)
        {
            return await FindByCondition(TEntity => TEntity.Bicycle.Id.Equals(bicycleId)).ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity, Guid customerId, Guid bicycleId)
        {
            entity.Customer = await _context.Users.FindAsync(customerId);
            entity.Bicycle = await _context.Bicycles.FindAsync(bicycleId);
            try
            {
                _context.Attach(entity).State = EntityState.Modified;
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            await _context.SaveChangesAsync();
        }             
    }
}
