using bikeRental.Core.Entities;
using bikeRental.DataAccess.Persistence;

namespace bikeRental.DataAccess.Repositories.Impl;

public class TodoListRepository : BaseRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(DatabaseContext context) : base(context) { }
}
