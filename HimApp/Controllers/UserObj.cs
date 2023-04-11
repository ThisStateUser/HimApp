using HimApp.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimApp.Controllers
{
    public class UserObj
    {
        public static Users FindUser(string login, string password)
        {
            Users user = HimBDEntities.GetContext().Users.FirstOrDefault
                            (x =>
                                x.login == login.Trim() &&
                                x.password == password.Trim()
                            );
            return user;
        }

        public static Users UserAcc;
    }
}
