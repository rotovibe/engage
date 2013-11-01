using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.C3User.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.C3User
{
    public interface IC3UserRepository<T> : IRepository<T>
    {
        C3UserDataResponse ProcessUserToken(string userToken);
    }
}
