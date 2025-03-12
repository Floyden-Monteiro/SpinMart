using API.DTOs;
using API.Errors;
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
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IInventoryService _inventoryService;

        private readonly IMapper _mapper;

        public OrdersController(IGenericRepository<Order> orderRepo,
            IGenericRepository<Product> productRepo, IOrderService orderService, IMapper mapper, IGenericRepository<Customer> customerRepo, IInventoryService inventoryService)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _orderService = orderService;
            _customerRepo = customerRepo;
            _inventoryService = inventoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrders()
        {
            var orders = await _orderRepo.ListAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(CreateOrderDto orderDto)
        {

            var customer = await _customerRepo.GetByIdAsync(orderDto.CustomerId);
            if (customer == null)
                return BadRequest(new ApiResponse(400, "Invalid customer"));

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;


            foreach (var item in orderDto.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    return BadRequest(new ApiResponse(400, $"Product {item.ProductId} not found"));

                if (!await _inventoryService.IsInStockAsync(item.ProductId, item.Quantity))
                    return BadRequest(new ApiResponse(400, $"Product {product.Name} has insufficient stock"));

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                totalAmount += orderItem.Price * orderItem.Quantity;
                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                ShippingAddress = orderDto.ShippingAddress,
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };


            _orderRepo.Add(order);
            await _orderRepo.SaveAsync();


            foreach (var item in orderItems)
            {
                await _inventoryService.UpdateStockQuantityAsync(item.ProductId, -item.Quantity);
            }

            var orderToReturn = _mapper.Map<Order, OrderToReturnDto>(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderToReturn);
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
            return Ok(new ApiResponse(200, $"Order status updated to {status}"));
        }
    }
}