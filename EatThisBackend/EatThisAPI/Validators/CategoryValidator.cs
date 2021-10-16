using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public class CategoryValidator
    {
        public static void Validate(Category category)
        {
            if (category == null)
            {
                throw new CustomException(BackendMessage.Category.CATEGORY_NOT_FOUND);
            }
        }
    }
}
