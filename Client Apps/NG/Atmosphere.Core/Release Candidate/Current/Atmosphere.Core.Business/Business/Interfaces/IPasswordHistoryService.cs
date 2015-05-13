using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using C3.Data;

namespace C3.Business.Interfaces
{
    public interface IPasswordHistoryService
    {
        List<PasswordHistory> GetByUser(Guid userId);
        User SetByUser(User user, string newPassword);
    }
}
