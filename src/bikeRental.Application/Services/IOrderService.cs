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
        Task<OrderModel> AddAsync(OrderModel orderModel, Guid customerId, Guid bicycleId);
        Task<OrderModel> GetByIdAsync(Guid? id);
        Task<OrderModel> GetByIdAsync(Guid? id, Guid customerId, Guid bicycleId);
        IEnumerable<OrderModel> GetAll();
        IEnumerable<OrderModel> SearchSelection(IEnumerable<OrderModel> orders, DateTime dateSearchFrom, DateTime dateSearchTo );
        Task UpdateAsync(OrderModel orderModel);
        IEnumerable<OrderModel> SortingSelection(IEnumerable<OrderModel> orders, string sortOrder);
        Task<IEnumerable<OrderModel>> GetByCustomer(Guid CustomerId);
        Task<IEnumerable<OrderModel>> GetByBicycle(Guid BicycleId);
        decimal GetRentalPrice(DateTime rentalStartTime, DateTime rentalEndTime, decimal price);
        Task FinishOrder(OrderModel orderModel, Guid stationId);

    }
}
