using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement
{
    public class MongoProcedureFactory
    {
        public IMongoProcedure GetProcedure(GetMongoProceduresRequest request)
        {
            IMongoProcedure proc = GetInstanceFromExecutingAssembly(request);
            if(proc != null)
            {
                proc.Request = request;
            }
            return proc;
        }

        private IMongoProcedure GetInstanceFromExecutingAssembly(GetMongoProceduresRequest request)
        {
            IMongoProcedure pr = null;

            string nm = string.Empty;
            foreach (Type mytype in Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(IMongoProcedure)) && !mytype.IsAbstract))
            {
                if (GetStaticPropertyName(request.Name, mytype))
                {
                    pr = mytype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as IMongoProcedure;
                    break;
                }
            }

            return pr;
        }

        private bool GetStaticPropertyName(string name, Type mytype)
        {
            bool result = false;
            FieldInfo field = mytype.GetField( "Name");
            if (name.Equals(field.GetValue(typeof(MongoProcedure))))
            {
                result = true;
            }
            return result;
        }
    }
}
