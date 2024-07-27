using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBrandService


    {
        IDataResult<List<Brand>> GetAll();
        IDataResult<Brand> GetById(int id);
        IResult Update(Brand brand);
        IResult Delete(int id);
        IResult Add(Brand brand); 
        

    }
}
