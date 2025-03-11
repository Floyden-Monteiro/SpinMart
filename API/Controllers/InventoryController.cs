using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class InventoryController : BaseApiController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;

        public InventoryController(IInventoryService inventoryService, IMapper mapper)
        {
            _inventoryService = inventoryService;
            _mapper = mapper;
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetLowStockProducts(
            [FromQuery] int threshold = 5)
        {
            var products = await _inventoryService.GetLowStockProductsAsync(threshold);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
        }

        [HttpPut("update-stock/{id}")]
        public async Task<ActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            var result = await _inventoryService.UpdateStockQuantityAsync(id, quantity);
            if (!result) return BadRequest("Failed to update stock");
            return Ok();
        }

        [HttpGet("check-stock/{id}")]
        public async Task<ActionResult<bool>> CheckStock(int id, [FromQuery] int quantity)
        {
            return Ok(await _inventoryService.IsInStockAsync(id, quantity));
        }
    }
}