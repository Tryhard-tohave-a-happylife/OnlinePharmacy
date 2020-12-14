using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopCNPM.Common
{
    public static class CommonConstants
    {
        public static string USER_SESSION = "USER_SESSION";
        public static string LISTPRODUCT_SESSION = "LISTPRODUCT_SESSION";
        public static string RESTORE_ITEM = "RESTORE_ITEM";
        public static string ADD_ITEM = "ADD_ITEM";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CartSession = "CartSession";
        public static string CurrentCulture { set; get; }
    }
}