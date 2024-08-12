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
    public class BrandManager : IBrandService
    {
        IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public IResult DeleteById(int id)
        {
            Brand brandToDelete = _brandDal.Get(b => b.Id == id);
            _brandDal.Delete(brandToDelete);
            return new SuccessResult(Messages.Deleted);
        }



        public IDataResult<List<Brand>> GetAll()
        {
            var result = _brandDal.GetAll();
            if (result == null)
            {
                return new ErrorDataResult<List<Brand>>(Messages.NotFound);
            }
            return new SuccessDataResult<List<Brand>>(result, Messages.Listed);
        }

        public IResult Update(Brand brand)
        {
            var result = _brandDal.Get(u => u.Id == brand.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            _brandDal.Update(brand);
            return new SuccessResult(Messages.Modified);
        }



        public IDataResult<Brand> GetById(int id)
        {
            var result = _brandDal.Get(b => b.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<Brand>(Messages.NotFound);
            }

            return new SuccessDataResult<Brand>(result, Messages.Listed);
        }

        public IResult Add(Brand brand)
        {
            var result = _brandDal.Get(b => b == brand);
            if (result != null)
            {
                return new ErrorResult(Messages.AlreadyExist);
            }
            _brandDal.Add(brand);
            return new SuccessResult(Messages.Added);
        }
    }
}
