using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Business.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImageController : ControllerBase
    {
        private readonly ICarImageService _carImageService;
        private readonly Cloudinary _cloudinary;

        public CarImageController(ICarImageService carImageService)
        {
            _carImageService = carImageService;

            // Cloudinary ayarları
            var account = new Account(
                "dquzrdxog", // Cloudinary Cloud Name
                "342841848959963",    // Cloudinary API Key
                "Sr2y9tQN7OzUy2GXq45eyxYSVwk"  // Cloudinary API Secret
            );

            _cloudinary = new Cloudinary(account);
        }

        [HttpPost("getAll")]
        public IActionResult GetCarImages([FromForm] int carId)
        {
            var result = _carImageService.GetAll();
            List<CarImage> carImages = result.Data.Where(item => item.CarId == carId).ToList();

            if (result.IsSuccess)
            {
                return Ok(carImages);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteImage([FromForm] int imageId)
        {
            var carImage = _carImageService.GetById(imageId);
            if (carImage == null || carImage.Data == null || string.IsNullOrEmpty(carImage.Data.ImagePath))
            {
                return NotFound("Image not found.");
            }

            // Cloudinary'den resmi sil
            var deletionParams = new DeletionParams(carImage.Data.ImagePath); // Cloudinary'deki public ID
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Veritabanından resmi sil
                var deleteResult = _carImageService.DeleteById(imageId);
                if (deleteResult.IsSuccess)
                {
                    return Ok(deleteResult);
                }
            }
            return BadRequest("Image deletion failed.");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateImage(IFormFile file, [FromForm] int imageId, [FromForm] int carId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var carImage = _carImageService.GetById(imageId);
            if (carImage == null || carImage.Data == null)
            {
                return NotFound("Image not found.");
            }

            // Cloudinary'de mevcut resmi sil
            var deletionParams = new DeletionParams(carImage.Data.ImagePath);
            await _cloudinary.DestroyAsync(deletionParams);

            // Yeni resmi Cloudinary'ye yükle
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),  // Dosya adını ve stream'i veriyoruz
                    PublicId = Guid.NewGuid().ToString() // Yeni PublicId oluştur
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    carImage.Data.ImagePath = uploadResult.SecureUrl.ToString(); // Yeni URL'yi veritabanına kaydet
                    carImage.Data.CarId = carId; // Gerekirse güncelle
                    var result = _carImageService.Update(carImage.Data);
                    if (result.IsSuccess)
                    {
                        return Ok(result);
                    }
                }
            }

            return BadRequest("Image update failed.");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddImage(IFormFile file, [FromForm] int carId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Dosyayı Stream olarak Cloudinary'ye gönderme
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),  // Dosya adını ve stream'i veriyoruz
                    PublicId = Guid.NewGuid().ToString()  // Her resim için benzersiz bir PublicId oluştur
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var carImage = new CarImage
                    {
                        CarId = carId,
                        ImagePath = uploadResult.SecureUrl.ToString(), // Cloudinary'den alınan URL
                        AddingDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    var result = _carImageService.Add(carImage);
                    if (result.IsSuccess)
                    {
                        return Ok(result);
                    }
                }
            }

            return BadRequest("Image upload failed.");
        }
    }
    }
