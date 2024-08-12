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
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        public IResult Add(Rental rental)
        {
            var existingRental = _rentalDal.Get(r => r.CarId == rental.CarId && r.ReturnDate == null);

            if (existingRental != null)
            {
                return new ErrorResult(Messages.AlreadyRented);
            }

            _rentalDal.Add(rental);
            return new SuccessResult(Messages.Added);
        }


        public IResult DeleteById(int id)
        {
            var toDelete = _rentalDal.Get(r => r.Id == id);
            if (toDelete == null) 
            {
                return new ErrorResult(Messages.NotFound);
            }
            _rentalDal.Delete( _rentalDal.Get(r=>r.Id == id));
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(),Messages.Listed);
        }

        public IDataResult<Rental> GetById(int id)
        {
            var result = _rentalDal.Get(u => u.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<Rental>(Messages.NotFound);
            }
            return new SuccessDataResult<Rental>(_rentalDal.Get(r=>r.Id==id),Messages.Listed);
        }

        public IResult Update(Rental rental)
        {
            var result = _rentalDal.Get(u => u.Id == rental.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            _rentalDal.Update( rental);
            return new SuccessResult(Messages.Modified);
        }
    }
}
