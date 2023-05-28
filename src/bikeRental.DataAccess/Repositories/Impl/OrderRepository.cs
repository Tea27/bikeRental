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
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            entity.Id = Guid.NewGuid();

            var addedEntity = (await DbSet.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return addedEntity;
        }
        public async Task<TEntity> GetByIdAsync(Guid? id)
        {
            return await FindByCondition(order => order.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression).AsNoTracking();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();

        }

        public async Task UpdateAsync(TEntity entity)
        {
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

        public async Task DeleteAsync(Guid id)
        {
            var order = new Order() { Id = id };
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
