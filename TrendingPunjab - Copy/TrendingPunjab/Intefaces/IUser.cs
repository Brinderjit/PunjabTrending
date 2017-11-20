using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendingPunjab.Models;

namespace TrendingPunjab.Intefaces
{
    public interface IUser
    {
        int createUser(UserModel user);
        List<long> getAdminList();
    }
}