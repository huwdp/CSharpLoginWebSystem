using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginExample.WebApp.Mapper;
using LoginExample.WebApp.Models;
using LoginExampleWebApp;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LoginExample.WebApp.Store
{
    public class UserStore : IUserStore
    {
        private DatabaseContext _databaseContext;
        private IUserMapper _userMapper;

        public UserStore(DatabaseContext databaseContext,
            IUserMapper userMapper)
        {
            _databaseContext = databaseContext;
            _userMapper = userMapper;
        }

        public void addUser(Models.UserModel userModel)
        {
            userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
            _databaseContext.Users.Add(_userMapper.Map(userModel));
            _databaseContext.SaveChanges();
        }

        public UserModel getById(int id)
        {
            return _userMapper.Map(_databaseContext.Users.Single(x => x.UserId == id));
        }

        public IEnumerable<UserModel> getByIds(IEnumerable<int> ids)
        {
            return _databaseContext.Users.Where(x => ids.Any(y => y == x.UserId)).Select(z => _userMapper.Map(z));
        }

        public UserModel loginUser(string username, string password)
        {
            DbModels.UserModel userModel = _databaseContext.Users.Single(x => x.Username == username);

            // https://cmatskas.com/-net-password-hashing-using-pbkdf2/ PBKDF2 looks a little complex so I decieded against it.
            // Rather use existing code rather than implement some algorihthm I don't fully undersatand.
            // BCrypr is fairly good. The BCRypt .NET Core system here (https://github.com/neoKushan/BCrypt.Net-Core) does salting for us.
            // Default salting is SaltRevision.Revision2B in the project.

            if (BCrypt.Net.BCrypt.Verify(password, userModel.Password))
            {
                return _userMapper.Map(userModel);
            }
            return null;
        }

        public bool registerUser(string username, string password)
        {
            if (_databaseContext.Users.Any(x => x.Username.ToLower() == username.ToLower()))
            {
                return false;
            }
            addUser(new UserModel { Username = username, Password = password });
            return true;
        }

        public void removeUser(int id)
        {
            _databaseContext.Users.Remove(_databaseContext.Users.Single(x => x.UserId == id));
            _databaseContext.SaveChanges();
        }

        public void save(UserModel userModel)
        {
            DbModels.UserModel dbUserModel = _databaseContext.Users.Single(x => x.UserId == userModel.Id);
            
            if (userModel.Password != "")
            {
                DbModels.UserModel newDbUserModel = _userMapper.Map(userModel);
                newDbUserModel.Password = dbUserModel.Password;
                dbUserModel = newDbUserModel;
            }
            else
            {
                dbUserModel = _userMapper.Map(userModel);
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
            }
            _databaseContext.SaveChanges();
        }
    }
}
