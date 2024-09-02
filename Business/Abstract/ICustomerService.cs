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
    public interface ICustomerService
    {
        IDataResult<List<Customer>> GetAll();
        IDataResult<List<CustomerDetailDto>> GetCustomerDetailsDto();
        IDataResult<Customer> GetById(int id);
        IResult Update(Customer customer);
        IResult DeleteById(int id);
        IResult Add(Customer customer);
    }
}
