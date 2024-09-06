using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCrud.Data;
using ApiCrud.Data.Entities;
using ApiCrud.Dtos;
using AutoMapper;

namespace ApiCrud.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly MyWorldDbContext _myWorldDbContext;
        private readonly IMapper _mapper;
        public CustomerController(MyWorldDbContext myWorldDbContext, IMapper mapper)
        {
            _myWorldDbContext = myWorldDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _myWorldDbContext.Customer
            .Include(_ => _.CustomerAddresses).ToListAsync();
            return Ok(customers);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customers = await _myWorldDbContext.Customer
            .Include(_ => _.CustomerAddresses).Where(_=>_.Id == id).FirstOrDefaultAsync();
            return Ok(customers);
        }

        // private Customer MapCustomerObject(CustomerDto payload)
        // {
        //     var result = new Customer();
        //     result.FirstName = payload.FirstName;
        //     result.LastName = payload.LastName;
        //     result.Phone = payload.Phone;
        //     result.CustomerAddresses = new List<CustomerAddresses>();
        //     payload.CustomerAddresses.ForEach(_ => {
        //         var newAddress = new CustomerAddresses();
        //         newAddress.City = _.City;
        //         newAddress.Country = _.Country;
        //         result.CustomerAddresses.Add(newAddress);
        //     });
        //     return result;
        // }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDto payloadCustomer)
        {
            // var newCustomer = MapCustomerObject(payloadCustomer);
            var newCustomer = _mapper.Map<Customer>(payloadCustomer);
            _myWorldDbContext.Customer.Add(newCustomer);
            await _myWorldDbContext.SaveChangesAsync();
            return Created($"/customer/{newCustomer.Id}", newCustomer);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CustomerDto payloadCustomer)
        {
            var updateCustomer = _mapper.Map<Customer>(payloadCustomer);
            _myWorldDbContext.Customer.Update(updateCustomer);
            await _myWorldDbContext.SaveChangesAsync();
            return Ok(updateCustomer);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customerToDelete = await _myWorldDbContext
            .Customer.Include(_ => _.CustomerAddresses).Where(_ => _.Id == id)
            .FirstOrDefaultAsync();

            if (customerToDelete == null)
            {
                return NotFound();
            }

            _myWorldDbContext.Customer.Remove(customerToDelete);
            await _myWorldDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}