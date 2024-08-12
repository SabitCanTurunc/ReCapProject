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
    public class ColorManager : IColorService
    {
        IColorDal _colorDal;

        public ColorManager(IColorDal colorDal)
        {
            _colorDal = colorDal;
        }

        public IResult Add(Color color)
        {
            var result = _colorDal.Get(c => c.Id == color.Id);
            if (result != null)
            {
                return new ErrorResult(Messages.AlreadyExist);
            }
            _colorDal.Add(color);
            return new SuccessResult(Messages.Added);
        }

        public IResult Delete(int id)
        {
            Color colorToDelete = _colorDal.Get(c=>c.Id == id);
            if (colorToDelete == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            _colorDal.Delete(colorToDelete);
            return new SuccessResult(Messages.Deleted);
           
        }

        public IDataResult<List<Color>> GetAll()
        {
            var result = _colorDal.GetAll();
            if (result == null)
            {
                return new ErrorDataResult<List<Color>>(Messages.NotFound);
            }
            return new SuccessDataResult<List<Color>>( result,Messages.Listed);
        }

        public IDataResult< Color> GetById(int id)
        {
            var result = _colorDal.Get(c => c.Id == id);
            if (result == null)
            { 
                return new ErrorDataResult<Color>(Messages.NotFound); 
            }
            return new SuccessDataResult<Color>( result,Messages.Listed);
        }

        public IResult Update(Color color)
        {
            var result = _colorDal.Get(u => u.Id == color.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            _colorDal.Update(color);
            return new SuccessResult(Messages.Modified); 
        }
    }
}
