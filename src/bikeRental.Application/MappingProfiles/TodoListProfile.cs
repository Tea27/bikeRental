using AutoMapper;
using bikeRental.Application.Models.TodoList;
using bikeRental.Core.Entities;

namespace bikeRental.Application.MappingProfiles;

public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap<CreateTodoListModel, TodoList>();

        CreateMap<TodoList, TodoListResponseModel>();
    }
}
