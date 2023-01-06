using daoInSQLDudunBot;
using mdlDudunBot;
using svcDudunBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bosvcDudunBot
{
    public class UserService : IUserService
    {
        private readonly IUserDao _userDao;
        public UserService()
        {
            _userDao = new InSQLUserDao();
        }
        public bool Auth(string username, string password)
        {
            return _userDao.Auth(username, password);
        }
    }
}
