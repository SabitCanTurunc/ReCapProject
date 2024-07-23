using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICarService
    {
        List<Car> GetAll();
        List<Car> GetCarsByBrandId(int id);
        List<Car> GetByUnitPrice(decimal min, decimal max);
        List<Car> GetCarsByBrandID(int id);
        void Update(Car car);  
        void Add(Car car);
    }
}
