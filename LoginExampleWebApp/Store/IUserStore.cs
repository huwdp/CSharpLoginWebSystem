using LoginExample.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginExample.WebApp.Store
{
    public interface IUserStore
    {
        void addUser(UserModel userModel);
        void removeUser(int id);
        bool registerUser(string username, string password);
        UserModel getById(int id);
        IEnumerable<UserModel> getByIds(IEnumerable<int> ids);
        UserModel loginUser(string username, string password);

        void save(UserModel userModel);

    }
}
