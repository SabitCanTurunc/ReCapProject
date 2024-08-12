using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult Add(Car car)
        {

            _carDal.Add(car);
            return new SuccessResult(Messages.Added);
        }


        public IDataResult<List<Car>> GetAll()
        {
            if (DateTime.Now.Hour == 20)
            {
                return new ErrorDataResult<List<Car>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Car>>( _carDal.GetAll(),Messages.Added);
        }

        public IDataResult<List<Car>> GetCarsByBrandId(int id)
        {
            var result = _carDal.GetAll(c => c.BrandId == id);
            if (result == null)
            {
                return new ErrorDataResult<List<Car>>(Messages.NotFound);
            }

            return new SuccessDataResult<List<Car>>(result,Messages.Listed);
        }

        public IDataResult<List<Car>> GetByUnitPrice(decimal min, decimal max)
        {
            var result = _carDal.GetAll(p => p.DailyPrice >= min && p.DailyPrice <= max).ToList();
            if (result == null)
            {
                return new ErrorDataResult<List<Car>>(Messages.NotFound);
            }

            return new SuccessDataResult<List<Car>>(result,Messages.Listed);
        }
        public IResult Update(Car car) 
        {
            var result = _carDal.Get(u => u.Id == car.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            _carDal.Update(car);
           return new SuccessResult( Messages.Modified);

        }

        public IDataResult<List<Car>> GetCarsByBrandID(int id)
        {
            var result = _carDal.GetAll().Where(c => c.BrandId == id).ToList();
            if (result == null)
            {
                return new ErrorDataResult<List<Car>>(Messages.NotFound);
            }

            return new SuccessDataResult<List<Car>>(result,Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            var result = _carDal.GetCarDetailsDto();
            if (result == null)
            {
                return new ErrorDataResult<List<CarDetailDto>>(Messages.NotFound);
            }
            return new SuccessDataResult<List<CarDetailDto>>(result,Messages.Listed);
        }
    }
}
