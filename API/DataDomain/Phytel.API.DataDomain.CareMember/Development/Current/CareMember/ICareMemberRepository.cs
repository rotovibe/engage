using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember
{
    public interface ICareMemberRepository<T> : IRepository<T>
    {
        IEnumerable<object> FindByPatientId(string entityId);
    }
}
