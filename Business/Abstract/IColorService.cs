﻿using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IColorService
    {
        IDataResult<List<Color>> GetAll();
        IDataResult< Color> GetById(int id);
        IResult Update(Color color);
        IResult Delete(int id);
        IResult Add(Color color);
        
    }
    
    
}
