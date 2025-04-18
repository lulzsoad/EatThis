﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs
{
    public class IngredientDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public IngredientCategoryDto IngredientCategory { get; set; }
    }
}
