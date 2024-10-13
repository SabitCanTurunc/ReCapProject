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
using Core.Utilities.Results.Abstract;

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

            // Cloudinary settings
            var account = new Account(
                "dquzrdxog", // Cloudinary Cloud Name
                "342841848959963", // Cloudinary API Key
                "Sr2y9tQN7OzUy2GXq45eyxYSVwk" // Cloudinary API Secret
            );

            _cloudinary = new Cloudinary(account);
        }

        [HttpPost("getAll")]
        public IDataResult<List<CarImage>> GetCarImages([FromForm] int carId)
        {
            var result = _carImageService.GetAll();
            List<CarImage> carImages = result.Data.Where(item => item.CarId == carId).ToList();

            if (result.IsSuccess)
            {
                return new SuccessDataResult<List<CarImage>>(carImages);
            }
            return new ErrorDataResult<List<CarImage>>(result.Message);
        }

        [HttpPost("delete")]
        public async Task<Core.Utilities.Results.Abstract.IResult> DeleteImage([FromForm] int imageId)
        {
            var carImage = _carImageService.GetById(imageId);
            if (carImage == null || carImage.Data == null || string.IsNullOrEmpty(carImage.Data.ImagePath))
            {
                return new ErrorResult("Image not found.");
            }

            // Delete the image from Cloudinary
            var deletionParams = new DeletionParams(carImage.Data.ImagePath); // Cloudinary public ID
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Delete the image from the database
                var deleteResult = _carImageService.DeleteById(imageId);
                if (deleteResult.IsSuccess)
                {
                    return new SuccessResult(deleteResult.Message);
                }
            }
            return new ErrorResult("Image deletion failed.");
        }

        [HttpPost("update")]
        public async Task<Core.Utilities.Results.Abstract.IResult> UpdateImage(IFormFile file, [FromForm] int imageId, [FromForm] int carId)
        {
            if (file == null || file.Length == 0)
            {
                return new ErrorResult("No file uploaded.");
            }

            var carImageResult = _carImageService.GetById(imageId);
            if (carImageResult == null || carImageResult.Data == null)
            {
                return new ErrorResult("Image not found.");
            }

            // Delete the existing image from Cloudinary
            var imageUrl = carImageResult.Data.ImagePath;
            var publicId = GetPublicIdFromUrl(imageUrl); // Get public ID from URL

            if (publicId == null)
            {
                return new ErrorResult("Invalid image URL, public ID could not be extracted.");
            }

            var deletionParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new ErrorResult("Failed to delete existing image from Cloudinary: " + deletionResult.Error.Message);
            }

            // Upload the new image to Cloudinary
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = Guid.NewGuid().ToString(), // Generate a new PublicId
                    Folder = "CarRental"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Update the image in the database
                    carImageResult.Data.ImagePath = uploadResult.SecureUrl.ToString(); // Save the new URL in the database
                    carImageResult.Data.CarId = carId; // Update if necessary
                    var result = _carImageService.Update(carImageResult.Data);
                    if (result.IsSuccess)
                    {
                        return new SuccessResult(result.Message);
                    }
                }
            }

            return new ErrorResult("Image update failed.");
        }

        [HttpPost("add")]
        public async Task<Core.Utilities.Results.Abstract.IResult> AddImage(IFormFile file, [FromForm] int carId)
        {
            if (file == null || file.Length == 0)
            {
                return new ErrorResult("No file uploaded.");
            }

            // Send the file stream to Cloudinary
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),  // Provide file name and stream
                    PublicId = Guid.NewGuid().ToString(),  // Generate a unique PublicId for each image
                    Folder = "CarRental" // Specify the folder name here
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var carImage = new CarImage
                    {
                        CarId = carId,
                        ImagePath = uploadResult.SecureUrl.ToString(), // URL received from Cloudinary
                        AddingDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    var result = _carImageService.Add(carImage);
                    if (result.IsSuccess)
                    {
                        return new SuccessResult(result.Message);
                    }
                }
            }

            return new ErrorResult("Image upload failed.");
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;

            // Get the public ID from the URL
            if (segments.Length >= 6) // At least 6 segments (upload, v{version}, folder, public id)
            {
                // Get the last segment and remove the extension
                var publicId = segments[^1].Split('.')[0]; // Remove extension from the last segment

                // Use a fixed folder name "CarRental"
                var folderSegment = "CarRental";

                // Construct the resulting public ID
                publicId = $"{folderSegment}/{publicId}";

                Console.WriteLine("publicId: " + publicId); // Logging
                return publicId;
            }

            return null; // Invalid URL
        }
    }
}
