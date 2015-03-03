using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public class NGUnitOfWork : INGUnitOfWork
    {
        private List<INGCommand> _commands = new List<INGCommand>();
        private int _current = 0;

        public void Undo()
        {
            for (int i = 0; i <= _commands.Count; i++)
            {
                if (_current > 0)
                {
                    INGCommand command = _commands[--_current] as INGCommand;
                    command.Undo();
                }
            }
        }

        public void Execute(INGCommand command)
        {
            try
            {
                command.Execute();
                _commands.Add(command);
                _current++;
            }
            catch (Exception ex)
            {
                Undo();
                throw ex;
            }
        }
    }

}
