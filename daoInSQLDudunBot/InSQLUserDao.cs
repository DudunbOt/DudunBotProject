using mdlDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace daoInSQLDudunBot
{
    public class InSQLUserDao : IUserDao
    {
        DudunBotEntities Db = new DudunBotEntities();
        public bool Auth(string username, string password)
        {
            if (!Db.Users.Any(u => u.Username == username) && !Db.Users.Any(u => u.Password == password))
                return false;

            return true;
        }
    }
}
