using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserDao
    {
        OnlShopCNPM db = null;
        public UserDao()
        {
            db = new OnlShopCNPM();
        }
        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == userName);
            if (result == null)
            {
                return 0;
            }
            else { 
                if(result.TypeOfAccount == 1)
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }
                    }
                }
            }
            return 0;
        }
        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.Username == userName);
        }
        public bool CheckUserName(string username)
        {
            var count = db.Users.Count(x => x.Username.Equals(username));
            if (count > 0) return true;
            return false;
        }
        public Guid Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }
        public bool ModifyMissing(Guid userID, int type)
        {
            var user = db.Users.Find(userID);
            if (type == 1)
            {
                user.AmountMissOrder += 1;
            }
            else
            {
                user.AmountMissOrder -= 1;
            }
            db.SaveChanges();
            return true;
        }
        public int returnAmountMiss(Guid userID)
        {
            var model = db.Users.Find(userID);
            return model.AmountMissOrder.Value;
        }
        public string ReturnUserFullName(Guid userID)
        {
            return db.Users.Find(userID).Name;
        }
    }
}
