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

        public class Account
        {
            public static string USER_EMAIL_ALEADY_TAKEN = "Istnieje już konto z takim adresem E-mail";
            public static string USER_INVALID_EMAIL = "Nieprawidłowy adres E-mail";
            public static string USER_INVALID_PASSWORD = "Hasło powinno zawierać mnimum 6 znaków";
            public static string USER_FIRST_NAME_REQUIRED = "Imię jest wymagane";
            public static string USER_LAST_NAME_REQUIRED = "Nazwisko jest wymagane";
            public static string USER_INVALID_EMAIL_OR_PASSWORD = "Nieprawidłowe dane logowania";
            public static string USER_EMAIL_NOT_FOUND = "Nie znaleziono adresu email";

            public static string PASSWORD_RESET_CODE_INVALID = "Nieprawidłowy kod";
            public static string PASSWORD_RESET_CODE_ERROR = "Niezgodne dane";
        }

        public class EmailMessages
        {
            public static string EMAILMESSAGE_ACTIVATIONLINK = "Dziękujemy za rejestrację w naszym serwisie! Kliknij poniższy link, aby aktywować swoje konto i korzystać z pełnej funkcjonalności naszej aplikacji.";
            public static string EMAILMESSAGE_ACTIVATE_YOUR_ACCOUNT = "Aktywuj swoje konto";
            public static string EMAILMESSAGE_RESET_YOUR_PASSWORD = "Prośba o zresetowanie hasła";
            public static string EMAILMESSAGE_RESET_YOUR_PASSWORD_MESSAGE = "Otrzymaliśmy informację o tym, że chcesz zresetować swoje hasło. Wpisz poniższy kod na naszej stronie, aby kontynuować. Jeżeli to nie Ty wysłałeś/aś zapytanie o zresetowaniu hasła, zalecamy je zmienić w razie próby kradzieży konta.";
        }

        public class Transaction
        {
            public static string TRANSACTION_ERROR = "Błąd wykonania transakcji EF";
        }

        public static class User
        {
            public static string USER_NOT_FOUND = "Nie znaleziono użytkownika";
        }

        public static class Recipe
        {
            public static string RECIPE_NOT_FOUND = "Nie znaleziono przepisu";
        }
    }
}
