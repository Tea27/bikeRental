using bikeRental.Application.Models;
using bikeRental.Application.Models.TodoList;

namespace bikeRental.Application.Services;

public interface ITodoListService
{
    Task<CreateTodoListResponseModel> CreateAsync(CreateTodoListModel createTodoListModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<TodoListResponseModel>> GetAllAsync();

    Task<UpdateTodoListResponseModel> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel);
}
