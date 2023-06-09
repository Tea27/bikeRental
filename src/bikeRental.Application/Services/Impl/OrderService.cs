﻿using AutoMapper;
using bikeRental.Application.Models.Bicycle;
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
        private readonly IBicycleRepository<Bicycle> _bicycleRepository;
        private readonly IBicycleService _bicycleService;
        private readonly IUserService _userService;


        public OrderService(IOrderRepository<Order> orderRepository, IBicycleRepository<Bicycle> bicycleRepository, IMapper mapper, IBicycleService bicycleService, IUserService userService)
        {
            _orderRepository = orderRepository;
            _bicycleRepository = bicycleRepository;
            _mapper = mapper;
            _bicycleService = bicycleService;
            _userService = userService;
        }

        public async Task<OrderModel> AddAsync(OrderModel orderModel, Guid customerId, Guid bicycleId)
        {
            var order = _mapper.Map<Order>(orderModel);           
            order = await _orderRepository.AddAsync(order, customerId, bicycleId);
            
            
            return _mapper.Map<OrderModel>(order);
        }

        public async Task<OrderModel> GetByIdAsync(Guid? id)
        {
            var response = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderModel>(response);
        }

        public async Task<OrderModel> GetByIdAsync(Guid? id, Guid customerId, Guid bicycleId)
        {
            var response = await _orderRepository.GetByIdAsync(id);
            var orderModel = _mapper.Map<OrderModel>(response);
            orderModel.Customer = await _userService.GetByIdAsync(customerId);
            orderModel.Bicycle = await _bicycleService.GetByIdAsync(bicycleId);
            return orderModel;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            foreach (var order in orders)
            {
                var customer = await _userService.GetByIdAsync(order.Customer.Id);
                order.Customer = _mapper.Map<ApplicationUser>(customer);
                var bicycle = await _bicycleService.GetByIdAsync(order.Bicycle.Id);
                order.Bicycle = _mapper.Map<Bicycle>(bicycle);
            }
            var orderModels = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            return orderModels;
        }

        public async Task<IEnumerable<OrderResponse>> GetByCustomer(Guid CustomerId)
        {
            var orders = await _orderRepository.GetByCustomer(CustomerId);
            var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            foreach (var order in response)
            {
                order.Customer = await _userService.GetByIdAsync(CustomerId);
            }
            return response;
        }

        public async Task<IEnumerable<OrderResponse>> GetByBicycle(Guid BicycleId)
        {
            var orders = await _orderRepository.GetByBicycle(BicycleId);
            var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            foreach (var order in response)
            {
                order.Bicycle = await _bicycleService.GetByIdAsync(BicycleId);
            }
            return response;
        }


        public async Task UpdateAsync(OrderModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);
            await _orderRepository.UpdateAsync(order, orderModel.Customer.Id, orderModel.Bicycle.Id);
        }
        public async Task Delete(Guid Id)
        {
            var order = await _orderRepository.GetByIdAsync(Id);        
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
                    return orders.OrderByDescending(o => o.RentalStartTime);
            }
        }

    }
}
