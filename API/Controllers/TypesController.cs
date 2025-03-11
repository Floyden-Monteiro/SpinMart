using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TypesController : BaseApiController
    {
        private readonly IGenericRepository<ProductType> _typesRepo;

        public TypesController(IGenericRepository<ProductType> typesRepo)
        {
            _typesRepo = typesRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            return Ok(await _typesRepo.ListAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetType(int id)
        {
            var type = await _typesRepo.GetByIdAsync(id);
            return type == null ? NotFound() : Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult<ProductType>> CreateType(ProductType type)
        {
            _typesRepo.Add(type);
            var result = await _typesRepo.SaveAsync();
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(GetType), new { id = type.Id }, type);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductType>> UpdateType(int id, ProductType type)
        {
            if (id != type.Id) return BadRequest();

            var existingType = await _typesRepo.GetByIdAsync(id);
            if (existingType == null) return NotFound();

            _typesRepo.Update(type);
            var result = await _typesRepo.SaveAsync();
            if (!result) return BadRequest();
            return Ok(type);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteType(int id)
        {
            var type = await _typesRepo.GetByIdAsync(id);
            if (type == null) return NotFound();

            _typesRepo.Delete(type);
            var result = await _typesRepo.SaveAsync();
            if (!result) return BadRequest();
            return Ok();
        }
    }
}