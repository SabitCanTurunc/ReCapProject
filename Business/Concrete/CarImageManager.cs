using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;

        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        public IResult Add(CarImage carImage)
        {
            var result = BusinessRules.Run(CheckImageLimit(carImage.CarId));

            if (result != null)
            {
                return result;
            }
            
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.Added);
        }



        public IResult DeleteById(int id)
        {
            CarImage result = _carImageDal.Get(i => i.Id == id);
            _carImageDal.Delete(result);
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            var result= _carImageDal.GetAll();
            return new SuccessDataResult<List<CarImage>>(result, Messages.Listed);
        }

        public IDataResult<CarImage> GetById(int id)
        {
            CarImage result = _carImageDal.Get(i => i.Id == id);
            return new SuccessDataResult<CarImage>(result);
        }

        public IResult Update(CarImage carImage)
        {
            _carImageDal.Update(carImage);
            return new SuccessResult(Messages.Modified);
        }

        public IResult CheckImageLimit(int carId)
        {
            var result = _carImageDal.GetAll(i => i.CarId == carId).Count;
            if (result < 5)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.OutOfLimit);
        }
    }
}
