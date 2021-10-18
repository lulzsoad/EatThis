using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Helpers
{
    public static class BackendMessage
    {
        public class General
        {
            public static string GENERAL_ERROR = "Wystąpił błąd";
            public static string INVALID_OBJECT_MODEL = "Nieprawidłowy model obiektu";
        }
        public class Ingredient
        {
            public static string INGREDIENT_NOT_FOUND = "Nie znaleziono składnika";
            public static string INGREDIENT_ALREADY_EXISTS = "Składnik już istnieje";
        }

        public class Category
        {
            public static string CATEGORY_NOT_FOUND = "Nie znaleziono kategorii";
            public static string CATEGORY_ALREADY_EXISTS = "Kategoria już istnieje";
        }

        public class Unit
        {
            public static string UNIT_NOT_FOUND = "Nie znaleziono jednostki";
            public static string UNIT_ALREADY_EXISTS = "Jednostka już istnieje";
        }
    }
}
