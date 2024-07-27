﻿using Business.Abstract;
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
    public class ColorManager : IColorService
    {
        IColorDal _colorDal;

        public ColorManager(IColorDal colorDal)
        {
            _colorDal = colorDal;
        }

        public IResult Add(Color color)
        {
            _colorDal.Add(color);
            return new SuccessResult(Messages.Added);
        }

        public IResult Delete(int id)
        {
            Color colorToDelete = _colorDal.Get(c=>c.Id == id);
            _colorDal.Delete(colorToDelete);
            return new SuccessResult(Messages.Deleted);
           
        }

        public IDataResult<List<Color>> GetAll()
        {
            return new SuccessDataResult<List<Color>>( _colorDal.GetAll(),Messages.Listed);
        }

        public IDataResult< Color> GetById(int id)
        {
            return new SuccessDataResult<Color>( _colorDal.Get(c=> c.Id == id),Messages.Listed);
        }

        public IResult Update(Color color)
        {
            _colorDal.Update(color);
            return new SuccessResult(Messages.Modified); 
        }
    }
}
