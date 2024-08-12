using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IResult Add(User user)
        {
            if (_userDal.Get(u => u.Email == user.Email) != null)
            {
                return new ErrorResult(Messages.MailAlreadyExist);
            }

            _userDal.Add(user);
            return new SuccessResult(Messages.Added);
        }

        public IResult DeleteById(int id)
        {
            User userTodelete = _userDal.Get(u => u.Id == id);
            if (userTodelete == null)
            {
                return new ErrorResult(Messages.NotFound); // Veya uygun bir hata mesajı döndürün
            }
            _userDal.Delete(userTodelete);
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.Listed);
        }

        public IDataResult<User> GetById(int id)
        {
            var result = _userDal.Get(u => u.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<User>(Messages.NotFound);
            }

            return new SuccessDataResult<User>(result, Messages.Listed);
        }

        public IResult Update(User user)
        {
            var result = _userDal.Get(u => u.Id == user.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.NotFound);
            }

            _userDal.Update(user);
            return new SuccessResult(Messages.Modified);
        }
    }
}
