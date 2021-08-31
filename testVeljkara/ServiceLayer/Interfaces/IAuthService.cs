using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.ServiceLayer.Interfaces
{
    public interface IAuthService
    {
        public string GetToken(int userId, AppSettings appSettings);
    }
}
