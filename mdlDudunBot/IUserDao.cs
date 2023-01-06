using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdlDudunBot
{
    public interface IUserDao
    {
        bool Auth(string username, string password);
    }
}
