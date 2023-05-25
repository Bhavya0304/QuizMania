using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuizMania.DBContext;

namespace QuizMania.Models
{
    public class UserMemberShip
    {
        public bool verifyLogin(string username,string password)
        {
            try
            {
                using(BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    return _db.Users.Any(x => x.Username == username && x.Password == password);
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public int RegisterUser(User user)
        {
            try
            {
                using (BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    _db.Users.Add(user);
                    _db.SaveChanges();
                }
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int GetId(string username)
        {
            try
            {
                using (BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    var data = _db.Users.Where(x=>x.Username == username).FirstOrDefault();
                    if (data != null)
                    {
                        return data.Id;
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public User getUser(string Username)
        {
            try
            {
                using (BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    var data = _db.Users.Where(x => x.Username == Username).FirstOrDefault();
                    data.ProfilePic = "/Theme/assets/images/profile/" + data.ProfilePic;
                    if (data != null)
                    {
                        return data;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int UpdateUser(User usr)
        {
            try
            {
                using (BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    _db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}