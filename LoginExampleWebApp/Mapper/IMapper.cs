using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginExample.WebApp.Mapper
{
    public interface IMapper<T, R>
    {
        T Map(R obj);
    }
}
