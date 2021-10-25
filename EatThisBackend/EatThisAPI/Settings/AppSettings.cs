using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Settings
{
    public static class AppSettings
    {
        public static string URL_BACKEND = "https://localhost:44331/";
        public static string URL_FRONTEND = "http://localhost:4200/";
        public static string URL_ACCOUNT_ACTIVATION = $"{URL_FRONTEND}account/activate?activationCode=";
    }
}
