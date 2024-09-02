using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, CarsDbContext>, ICustomerDal
    {
       
        public List<CustomerDetailDto> GetCustomerDetaisDto()
        {
            using (CarsDbContext context = new CarsDbContext())
            {
                var result = from c in context.Customers
                             join u in context.Users on c.UserId equals u.Id
                             select new CustomerDetailDto
                             {
                                 Id = u.Id,
                                 FirsName = u.FirstName,
                                 LastName = u.LastName,
                                 Email = u.Email,
                                 CompanyName = c.CompanyName,

                             };
                return result.ToList();
            }
        }
    }
}
