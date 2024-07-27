using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool isSucces, string message) : base(isSucces, message)
        {
            Data = data;
        }
        public DataResult(T data, bool isSucces) : base(isSucces)
        {
            Data = data;
        }
        public T Data { get; }
    }
}
