using LoginExample.WebApp.DbModels;
using LoginExample.WebApp.Models;
using LoginExampleWebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginExample.WebApp.Mapper
{
    public class UserMapper : IUserMapper
    {
        public DbModels.UserModel Map(Models.UserModel obj)
        {
            return new DbModels.UserModel { UserId = obj.Id, Username = obj.Username, Password = obj.Password };
        }

        public Models.UserModel Map(DbModels.UserModel obj)
        {
            return new Models.UserModel { Id = obj.UserId, Username = obj.Username, Password = obj.Password };
        }
    }
}
