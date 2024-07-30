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
            User userTodelete= _userDal.Get(u=>u.Id == id);
            _userDal.Delete(userTodelete);
            throw new NotImplementedException();
        }

        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.Listed);
        }

        public IDataResult<User> GetById(int id)
        {

            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id), Messages.Listed);
        }

        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult(Messages.Modified);
        }
    }
}
