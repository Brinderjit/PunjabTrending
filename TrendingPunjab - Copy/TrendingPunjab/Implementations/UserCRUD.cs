using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendingPunjab.DataLayer;
using TrendingPunjab.Intefaces;
using TrendingPunjab.Models;

namespace TrendingPunjab.Implementations
{
    public class UserCRUD:IUser
    {
        private readonly DataLogic _data;

        public UserCRUD()
        {
            _data = new DataLogic();
        }
        public int createUser(UserModel user)
        {
            return _data.createUser(user);
        }
        public List<long> getAdminList()
        {
            return _data.getAdminList();
        }

    }
}