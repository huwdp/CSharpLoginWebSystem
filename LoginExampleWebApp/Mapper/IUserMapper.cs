using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginExample.WebApp.Mapper
{
    public interface IUserMapper : IMapper<LoginExample.WebApp.DbModels.UserModel, Models.UserModel>,
        IMapper<Models.UserModel, LoginExample.WebApp.DbModels.UserModel>
    {
    }
}
