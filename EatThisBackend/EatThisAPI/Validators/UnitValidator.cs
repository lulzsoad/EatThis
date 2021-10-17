using EatThisAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public class UnitValidator
    {
        public static void Validate(Unit unit)
        {
            if (unit == null)
            {
                throw new Exception();
            }
        }
    }
}
