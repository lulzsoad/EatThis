using EatThisAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Database
{
    public class Seeder
    {
        private readonly AppDbContext context;
        public Seeder(AppDbContext context)
        {
            this.context = context;
        }

        public void Seed()
        {
            if (context.Database.CanConnect())
            {
                var data = context.Units.ToList();
                if (!context.Units.Any())
                {
                    var units = GetUnits();
                    context.Units.AddRange(units);
                    context.SaveChanges();
                }
                if (!context.Ingredients.Any())
                {
                    var ingredients = GetIngredients();
                    context.Ingredients.AddRange(ingredients);
                    context.SaveChanges();
                }
            }
        }

        #region Składniki
        private IEnumerable<Ingredient> GetIngredients()
        {
            var ingredients = new List<Ingredient>()
            {
                new Ingredient
                {
                    Name = "Schab środkowy bez kości",
                },
                new Ingredient
                {
                    Name = "Pieczarki",
                },
                new Ingredient
                {
                    Name = "Majonez Hellman's Babuni",
                },
                new Ingredient
                {
                    Name = "Ketchup Hellman's Pikantny",
                },
                new Ingredient
                {
                    Name = "Przyprawa do mięs Knorr",
                },
                new Ingredient
                {
                    Name = "Cebula",
                },
                new Ingredient
                {
                    Name = "Żółty Ser",
                },
                new Ingredient
                {
                    Name = "Ząbek czosnku",
                },
                new Ingredient
                {
                    Name = "Natka pietruszki",
                },
                new Ingredient
                {
                    Name = "Wołowina bez kości",
                },
                new Ingredient
                {
                    Name = "Kostka do mięs pieczeniowa Knorr",
                },
                new Ingredient
                {
                    Name = "Olej do smażenia",
                },
                new Ingredient
                {
                    Name = "Olej",
                },
                new Ingredient
                {
                    Name = "Śmietana 30%",
                },
                new Ingredient
                {
                    Name = "Musztarda sarepska",
                },
                new Ingredient
                {
                    Name = "Majeranek",
                },
                new Ingredient
                {
                    Name = "Suszone podgrzybki i borowiki namoczone wcześniej",
                },
                new Ingredient
                {
                    Name = "Chleb razowy",
                },
            };
            return ingredients;
        }
        #endregion

        #region Jednostki
        private IEnumerable<Unit> GetUnits()
        {
            var units = new List<Unit>()
            {
                new Unit
                {
                    Name = "Gram/ów"
                },
                new Unit
                {
                    Name = "Opakowanie/a"
                },
                new Unit
                {
                    Name = "Sztuka/i"
                },
                new Unit
                {
                    Name = "Łyżka/i"
                },
                new Unit
                {
                    Name = "Łyżeczka/i"
                },
                new Unit
                {
                    Name = "Litr/ów"
                },
                new Unit
                {
                    Name = "Mililitr/ów"
                },
                new Unit
                {
                    Name = "Szklanka/i"
                },
            };

            return units;
        }
        #endregion
    }
}
