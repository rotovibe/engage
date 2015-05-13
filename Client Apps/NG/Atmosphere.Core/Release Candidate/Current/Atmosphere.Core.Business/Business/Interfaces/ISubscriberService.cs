using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using C3.Data;
using C3.Data.Enum;

namespace C3.Business.Interfaces
{
    public interface ISubscriberService
    {
        DataTable GetSubscribersByUser(Contract contract, Guid userId, GroupEntityTypes type);
        List<Subscriber> GetSubscribersByUserAndType(Contract contract, Guid userId, GroupEntityTypes type);
        DataTable GetSubscribersByGroup(Contract contract, string groupXML);
        List<Subscriber> GetSubscribersByGroups(Contract contract, string groupXML);
    }
}
