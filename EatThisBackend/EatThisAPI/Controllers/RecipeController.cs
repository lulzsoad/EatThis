﻿using EatThisAPI.Models.DTOs;
using EatThisAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] RecipeDto recipeDto)
        {
            return Ok(await recipeService.AddRecipe(recipeDto));
        }
    }
}
