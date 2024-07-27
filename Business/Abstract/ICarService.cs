using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICarService
    {
        IDataResult<List<Car>> GetAll();
        IDataResult<List<Car>> GetCarsByBrandId(int id);
        IDataResult<List<Car>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<Car>> GetCarsByBrandID(int id);
        IDataResult<List<CarDetailDto>> GetCarDetails();
        IResult Update(Car car);
        IResult Add(Car car);
    }
}
      