using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet("getAll")]

        public IActionResult GetAll()
        {
            var result = _colorService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("add")]
        public IActionResult add([FromForm] Color color)
        {
            var result = _colorService.Add(color);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest();
        }

    }
}
