using AutoMapper;
using bikeRental.Application.Models.Order;
using bikeRental.Application.Models.Station;
using bikeRental.Core.Entities;
using bikeRental.Core.Identity;
using bikeRental.DataAccess.Repositories;
using bikeRental.DataAccess.Repositories.Impl;
using MailKit.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Application.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository<Order> _orderRepository;

        public OrderService(IOrderRepository<Order> orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<OrderModel> AddAsync(OrderModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);
            order = await _orderRepository.AddAsync(order);
            return _mapper.Map<OrderModel>(order);
        }

        public async Task<OrderModel> GetByIdAsync(Guid? id)
        {
            var response = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderModel>(response);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var response = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(response);
        }


        public async Task UpdateAsync(OrderModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);
            await _orderRepository.UpdateAsync(order);
        }
        public async Task Delete(Guid Id)
        {
            await _orderRepository.DeleteAsync(Id);

        }

        /** 
         * Search orders for specific date
         * */
        public IEnumerable<OrderResponse> SearchSelectionAsync(IEnumerable<OrderResponse> orders, DateTime dateSearch)
        {
            IEnumerable<OrderResponse> ordersSearched = orders.ToList();           
            if (!dateSearch.Equals(new DateTime(0001, 01, 01, 00, 00, 00)) && dateSearch < DateTime.Now)
            {
                ordersSearched = orders.Where(o =>
                    (o.RentalStartTime.Ticks <= dateSearch.Ticks && o.RentalEndTime.Ticks >=dateSearch.Ticks));
            }
            return ordersSearched;
        }
        public IEnumerable<OrderResponse> SortingSelection(IEnumerable<OrderResponse> orders, string sortOrder)
        {
            switch (sortOrder)
            {
                case "RentalStartTime":
                    return orders.OrderBy(o => o.RentalStartTime);
                case "RentalStartTimeDesc":
                    return orders.OrderByDescending(o => o.RentalStartTime);
                case "RentalEndTime":
                    return orders.OrderBy(o => o.RentalEndTime);
                case "RentalEndTimeDesc":
                    return orders.OrderByDescending(o => o.RentalEndTime);
                case "RentalPrice":
                    return orders.OrderBy(o => o.RentalPrice);
                case "RentalPriceDesc":
                    return orders.OrderByDescending(o => o.RentalPrice);
                default:
                    return orders.OrderBy(o => o.RentalStartTime);
            }
        }

    }
}
