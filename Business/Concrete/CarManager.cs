using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
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

        public void Add(Car car)
        {
            if (car.Description == null || car.Description.Length < 2)
            {
                throw new Exception("Car description length should be at least 2 characters.");
            }

            if (car.DailyPrice==null || car.DailyPrice <= 0)
            {
                throw new Exception("Car daily price must be greater than zero.");
            }

            _carDal.Add(car);
        }


        public List<Car> GetAll()
        {
            return _carDal.GetAll();
        }

        public List<Car> GetCarsByBrandId(int id)
        {
            return _carDal.GetAll(c => c.BrandId == id);
        }

        public List<Car> GetByUnitPrice(decimal min, decimal max)
        {
            return _carDal.GetAll(p=>p.DailyPrice>=min && p.DailyPrice<= max).ToList();
        }
        public void Update(Car car) 
        {
            _carDal.Update(car);
        }

        public List<Car> GetCarsByBrandID(int id)
        {
            return _carDal.GetAll().Where(c => c.BrandId == id).ToList();
        }
    }
}
