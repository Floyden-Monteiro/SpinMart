using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CustomersController : BaseApiController
    {
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public CustomersController(IGenericRepository<Customer> customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<CustomerToReturnDto>>> GetCustomers(
            [FromQuery] CustomerSpecParams customerParams)
        {
            var spec = new CustomerWithOrdersSpecification(customerParams);
            var customers = await _customerRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Customer>,
                IReadOnlyList<CustomerToReturnDto>>(customers);

            return Ok(new Pagination<CustomerToReturnDto>(customerParams.PageIndex,
                customerParams.PageSize, customers.Count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerToReturnDto>> GetCustomer(int id)
        {
            var spec = new CustomerWithOrdersSpecification(id);
            var customer = await _customerRepo.GetEntityWithSpec(spec);

            if (customer == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Customer, CustomerToReturnDto>(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerToReturnDto>> CreateCustomer(
            CustomerCreateDto customerDto)
        {
            var customer = _mapper.Map<CustomerCreateDto, Customer>(customerDto);
            _customerRepo.Add(customer);

            if (await _customerRepo.SaveAsync())
                return CreatedAtAction(nameof(GetCustomer),
                    new { id = customer.Id },
                    _mapper.Map<Customer, CustomerToReturnDto>(customer));

            return BadRequest(new ApiResponse(400, "Problem creating customer"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerToReturnDto>> UpdateCustomer(
            int id, CustomerCreateDto customerDto)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return NotFound(new ApiResponse(404));

            _mapper.Map(customerDto, customer);
            _customerRepo.Update(customer);

            if (await _customerRepo.SaveAsync())
                return Ok(_mapper.Map<Customer, CustomerToReturnDto>(customer));

            return BadRequest(new ApiResponse(400, "Problem updating customer"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return NotFound(new ApiResponse(404));

            _customerRepo.Delete(customer);

            if (await _customerRepo.SaveAsync())
                return Ok();

            return BadRequest(new ApiResponse(400, "Problem deleting customer"));
        }
    }
}