﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs.ProposedRecipe
{
    public class ProposedCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
