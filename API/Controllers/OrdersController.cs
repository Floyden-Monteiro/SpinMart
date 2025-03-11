using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IGenericRepository<Order> orderRepo,
            IGenericRepository<Product> productRepo, IOrderService orderService, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrders()
        {
            var orders = await _orderRepo.ListAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _orderRepo.Add(order);
            var result = await _orderRepo.SaveAsync();
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            _orderRepo.Update(order);
            var result = await _orderRepo.SaveAsync();

            if (!result) return BadRequest();
            return Ok();
        }
    }
}