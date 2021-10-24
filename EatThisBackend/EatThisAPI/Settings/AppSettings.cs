using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Settings
{
    public static class AppSettings
    {
        public static string URL = "https://localhost:44331/";
        public static string URL_ACTIVATION_ENDPOINT = $"{URL}api/account/activate/";
    }
}
