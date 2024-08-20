using Business.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImageController : ControllerBase
    {
        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        private readonly ICarImageService _carImageService;

        public CarImageController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
            EnsureImageFolderExists();
        }

        private void EnsureImageFolderExists()
        {
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }


        [HttpPost("getAll")]
        public async Task<IActionResult> GetCarImages([FromForm]int carId)
        {
            var result = _carImageService.GetAll();
            List<CarImage> carImages = result.Data.Where(item => item.CarId == carId).ToList();
            var carImagesBytes = new List<byte[]>();

            foreach (var carImage in carImages) 
            {
                var filePath= carImage.ImagePath;
                try
                {
                    var imageBytes = await ReadImageFromFileSystem(filePath);
                    carImagesBytes.Add(imageBytes);
                }
                catch (FileNotFoundException)
                {
                    return NotFound($"image not found{ filePath}");
                }
            }

            if (result.IsSuccess)
            {
                return Ok(carImagesBytes);
            }return BadRequest(carImages);
        }

        [HttpPost("delete")]
        public IActionResult DeleteImage([FromForm] int imageId)
        {
            // 1. Dosyayı dosya sisteminden sil
            var deleteFileResult = DeleteImageFromFile(imageId);


            // 2. Veritabanından resmi sil
            var deleteResult = _carImageService.DeleteById(imageId);
            if (deleteResult.IsSuccess)
            {
                return Ok(deleteResult);
            }
            return BadRequest(deleteResult);
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateImage(IFormFile file, [FromForm] int imageId, [FromForm] int carId) 
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = await SaveImageToFileSystem(file);
            DeleteImageFromFile(imageId);

            var carImage = new CarImage
            {
                Id = imageId,
                CarId=carId,
                ImagePath = filePath,
                AddingDate = DateOnly.FromDateTime(DateTime.Now),


            };
            var result = _carImageService.Update(carImage);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("add")]
        public async Task<IActionResult> AddImage(IFormFile file, [FromForm] int carId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }


            var filePath= await SaveImageToFileSystem(file);

            var carImage = new CarImage
            {
                CarId = carId,
                ImagePath = filePath,
                AddingDate = DateOnly.FromDateTime(DateTime.Now),


            };

            var result = _carImageService.Add(carImage);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        private IActionResult DeleteImageFromFile(int imageId)
        {
            var carImage = _carImageService.GetById(imageId);

            if (carImage == null || carImage.Data == null || string.IsNullOrEmpty(carImage.Data.ImagePath))
            {
                return NotFound("Image not found.");
            }

            var imagePath = carImage.Data.ImagePath;

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
                return Ok("Image deleted successfully.");
            }

            return NotFound("Image file not found on the server.");
        }

        private async Task<string> SaveImageToFileSystem(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(_imageFolderPath, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
        private async Task<byte[]> ReadImageFromFileSystem(string filePath)
        {
            
            if (!System.IO.File.Exists(filePath) || filePath==null) 
            { 
                filePath= _carImageService.GetById(19).Data.ImagePath.ToString();
                return await System.IO.File.ReadAllBytesAsync(filePath);
            }
            return await System.IO.File.ReadAllBytesAsync(filePath); }
    }
}
 