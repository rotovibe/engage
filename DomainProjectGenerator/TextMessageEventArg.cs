using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainProjectGenerator
{
    public class TextMessageEventArg : EventArgs
    {
        public TextMessageEventArg(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

    }
}
