using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact
{
    public interface IContactRepository<T> : IRepository<T>
    {
        object FindContactByPatientId(GetContactDataRequest request);
    }
}
