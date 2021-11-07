using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public interface IValidator
    {
        void IsObjectNull(object obj);
    }
    public class Validator: IValidator
    {
        public void IsObjectNull(object obj)
        {
            if(obj == null)
            {
                throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }
    }
}
