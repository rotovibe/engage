using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public interface INGUnitOfWork
    {
        void Undo();
        void Execute(INGCommand command);
    }
}
