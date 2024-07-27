using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, CarsDbContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetailsDto()
        {
            using (CarsDbContext context = new CarsDbContext())
            {
                var result = from c in context.Car
                             join b in context.Brand on c.BrandId equals b.Id
                             join color in context.Color on c.ColorId equals color.Id
                             select new CarDetailDto
                             {
                                 CarId = c.Id,
                                 BrandName = b.Name,
                                 Description = c.Description,
                                 ColorName = color.Name,
                                 ModelYear = c.ModelYear,
                                 DailyPrice = c.DailyPrice,
                             };
                return result.ToList();


            }
        }
    }
}
