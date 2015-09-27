using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Commands
{
    public abstract class Command<TOut,TParameter> : IIntegrationCommand<TOut, TParameter>
    {
        public abstract TOut Execute(TParameter val);
    }
}
