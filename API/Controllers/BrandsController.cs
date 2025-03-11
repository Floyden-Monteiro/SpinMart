using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Core.Specification;

namespace API.Controllers
{
    public class BrandsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IMapper _mapper;

        public BrandsController(IGenericRepository<ProductBrand> brandsRepo, IMapper mapper)
        {
            _brandsRepo = brandsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<BrandToReturnDto>>> GetBrands([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 6)
        {
            var spec = new BrandSpecification(pageIndex, pageSize);
            var countSpec = new BrandWithFiltersForCountSpecification();

            var totalItems = await _brandsRepo.CountAsync(countSpec);
            var brands = await _brandsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<BrandToReturnDto>>(brands);

            return Ok(new Pagination<BrandToReturnDto>(pageIndex, pageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandToReturnDto>> GetBrand(int id)
        {
            var brand = await _brandsRepo.GetByIdAsync(id);
            if (brand == null) return NotFound(new ApiResponse(404, "Brand not found"));

            return Ok(_mapper.Map<ProductBrand, BrandToReturnDto>(brand));
        }

        [HttpPost]
        public async Task<ActionResult<BrandToReturnDto>> CreateBrand(BrandCreateDto brandDto)
        {

            var brand = new ProductBrand
            {
                Name = brandDto.Name
            };

            _brandsRepo.Add(brand);

            var result = await _brandsRepo.SaveAsync();
            if (!result) return BadRequest(new ApiResponse(400, "Problem creating brand"));

            var brandToReturn = _mapper.Map<ProductBrand, BrandToReturnDto>(brand);
            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, brandToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandToReturnDto>> UpdateBrand(int id, BrandUpdateDto brandDto)
        {
            var brand = await _brandsRepo.GetByIdAsync(id);
            if (brand == null) return NotFound(new ApiResponse(404, "Brand not found"));

            _mapper.Map(brandDto, brand);
            _brandsRepo.Update(brand);

            var result = await _brandsRepo.SaveAsync();
            if (!result) return BadRequest(new ApiResponse(400, "Problem updating brand"));

            return Ok(_mapper.Map<ProductBrand, BrandToReturnDto>(brand));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await _brandsRepo.GetByIdAsync(id);
            if (brand == null) return NotFound(new ApiResponse(404, "Brand not found"));

            _brandsRepo.Delete(brand);
            var result = await _brandsRepo.SaveAsync();

            if (!result) return BadRequest(new ApiResponse(400, "Problem deleting brand"));
            return Ok(new ApiResponse(200, $"Brand '{brand.Name}' deleted successfully"));
        }
    }
}