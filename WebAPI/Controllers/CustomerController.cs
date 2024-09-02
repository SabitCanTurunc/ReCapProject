using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var result = _customerService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest();


        }

        [HttpGet("getCustomerDetailDto")]
        public IActionResult GetCustomerDetailDto()
        {
            var result = _customerService.GetCustomerDetailsDto();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest();


        }

    }
}
