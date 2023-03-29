using bikeRental.Core.Entities;
using bikeRental.DataAccess.Persistence;

namespace bikeRental.DataAccess.Repositories.Impl;

public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(DatabaseContext context) : base(context) { }
}
