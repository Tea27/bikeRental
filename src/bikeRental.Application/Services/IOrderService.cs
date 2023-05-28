using bikeRental.Application.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Services
{
    public interface IOrderService
    {
        Task<OrderModel> AddAsync(OrderModel entity);
        Task<OrderModel> GetByIdAsync(Guid? id);
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        IEnumerable<OrderResponse> SearchSelectionAsync(IEnumerable<OrderResponse> orders, DateTime dateSearch);
        Task UpdateAsync(OrderModel orderModel);
        IEnumerable<OrderResponse> SortingSelection(IEnumerable<OrderResponse> orders, string sortOrder);
        Task Delete(Guid Id);

    }
}
