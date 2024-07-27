using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        public IResult Add(Car car)
        {
            if (car.Description == null || car.Description.Length < 2)
            {
                return new ErrorResult(Messages.NameInvalid);
            }

            _carDal.Add(car);
            return new SuccessResult(Messages.Added);
        }


        public IDataResult<List<Car>> GetAll()
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Car>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Car>>( _carDal.GetAll(),Messages.Added);
        }

        public IDataResult<List<Car>> GetCarsByBrandId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.BrandId == id),Messages.Listed);
        }

        public IDataResult<List<Car>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(p=>p.DailyPrice>=min && p.DailyPrice<= max).ToList(),Messages.Listed);
        }
        public IResult Update(Car car) 
        {
            _carDal.Update(car);
           return new SuccessResult( Messages.Modified);

        }

        public IDataResult<List<Car>> GetCarsByBrandID(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll().Where(c => c.BrandId == id).ToList(),Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetailsDto(),Messages.Listed);
        }
    }
}
