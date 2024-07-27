using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryCarDAl : ICarDal
    {
        List<Car> _cars;
        public InMemoryCarDAl()
        {
            _cars = new List<Car>
            {
                new Car{Id=1, BrandId=1,ColorId=1,DailyPrice=90000,ModelYear=2022,Description="toyota corolla"},
                new Car{Id=2, BrandId=1,ColorId=5,DailyPrice=1200000,ModelYear=2022,Description="toyota hilux"},
                new Car{Id=3, BrandId=2,ColorId=3,DailyPrice=1000000,ModelYear=2020,Description="volkswagen golf"},
                new Car{Id=4, BrandId=2,ColorId=3,DailyPrice=1400000,ModelYear=2019,Description="volkswagen passat"},
                new Car{Id=5, BrandId=3,ColorId=4,DailyPrice=3260000,ModelYear=2016,Description="audi a7"},
                new Car{Id=6, BrandId=4,ColorId=1,DailyPrice=800000,ModelYear=2002,Description="mercedes w240"}


            };
        }
        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Delete(Car car)
        {
            //Car carToDelete=null;
            //foreach (var c in _cars) {
            //    if(c.CarId == car.CarId)
            //    {
            //        carToDelete = c;
            //    }
            //}
            Car carToDelete = _cars.SingleOrDefault(c => c.Id == car.Id);
            _cars.Remove(carToDelete);
        }

        public Car Get(Expression<Func<Car, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAll()
        {
            return _cars;
        }

        public List<Car> GetAll(Expression<Func<Car, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Car GetById(int id)
        {
            return _cars.FirstOrDefault(c => c.Id == id);
        }

        public List<CarDetailDto> GetCarDetailsDto()
        {
            throw new NotImplementedException();
        }

        public void Update(Car car)
        {
            Car carToUpdate = _cars.SingleOrDefault(c => c.Id == car.Id);
            carToUpdate.Id = car.Id;
            carToUpdate.BrandId = car.BrandId;
            carToUpdate.ColorId = car.ColorId;
            carToUpdate.DailyPrice = car.DailyPrice;
            carToUpdate.Description = car.Description;
            carToUpdate.ModelYear = car.ModelYear;
        }
    }
}
