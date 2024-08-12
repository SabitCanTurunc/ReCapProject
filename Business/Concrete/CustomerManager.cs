using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IResult Add(Customer customer)
        {
            var result = _customerDal.Get(c => c.UserId == customer.UserId);
            if (result != null) 
            {
                return new ErrorResult(Messages.AlreadyExist);
            } 

            _customerDal.Add(customer);
            return new SuccessResult(Messages.Added);
        }

        public IResult DeleteById(int id)
        {
            var result = _customerDal.Get(c => c.UserId == id);
            if (result == null)
            {
                return new ErrorDataResult<Customer>(Messages.NotFound);
            }
            _customerDal.Delete(_customerDal.Get(c=>c.UserId==id));
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<Customer>> GetAll()
        {
            var result = _customerDal.GetAll();
            if (result == null)
            {
                return new ErrorDataResult<List<Customer>>(Messages.NotFound);
            }
            return new SuccessDataResult<List<Customer>>(result,Messages.Listed);
        }

        public IDataResult<Customer> GetById(int id)
        {
            var result = _customerDal.Get(c => c.UserId == id);
            if (result == null)
            {
                return new ErrorDataResult<Customer>(Messages.NotFound);
            }
            return new SuccessDataResult<Customer>(result,Messages.Listed);
        }

        public IResult Update(Customer customer)
        {
            var result = _customerDal.Get(u => u.UserId == customer.UserId);
            if (result == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            _customerDal.Update(customer);
            return new SuccessResult(Messages.Modified);
        }
    }
}
