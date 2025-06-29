using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Dtos
{
    public class OperationResut<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
        public static OperationResut<T> SuccessResult(T Data)
            => new OperationResut<T>() { Success = true, Data = Data };
        public static OperationResut<T> FailureResult(string error)
            => new OperationResut<T> { Success = false, Error = error }; 
    }
}
