using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopCNPM
{
    [Serializable]
    public class UserLogin
    {
        public Guid UserID { set; get; }
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string Name { set; get; }
        public string NameFirst { set; get; }
        public string GroupID { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
    }
}