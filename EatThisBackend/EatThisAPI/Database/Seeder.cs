﻿using EatThisAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EatThisAPI.Database
{
    public class Seeder
    {
        private readonly AppDbContext context;
        public Seeder(AppDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Seed()
        {
            

            if (context.Database.CanConnect())
            {
                if (!context.Roles.Any())
                {
                    context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT Roles ON");
                    context.Database.ExecuteSqlRaw(@"
SET IDENTITY_INSERT Roles ON; 
INSERT INTO Roles(Id, Name) VALUES(1, 'Admin'), (2, 'Employee'), (3, 'User')
SET IDENTITY_INSERT Roles OFF; 
");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Roles] OFF");
                }

                if (!context.Units.Any())
                {
                    var units = GetUnits();
                    context.Units.AddRange(units);
                    context.SaveChanges();
                }

                if (!context.ReportStatuses.Any())
                {
                    context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT ReportStatuses ON");
                    context.Database.ExecuteSqlRaw(@"
SET IDENTITY_INSERT ReportStatuses ON; 
INSERT INTO ReportStatuses(Id, Name) VALUES(1, 'Zgłoszono'), (2, 'W trakcie rozpatrywania'), (3, 'Zakończono')
SET IDENTITY_INSERT ReportStatuses OFF; 
");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[ReportStatuses] OFF");

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

        #region Role
        private List<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "Employee"
                },
                new Role
                {
                    Id = 3,
                    Name = "User"
                }
            };

            return roles;
        }
        #endregion
    }
}
