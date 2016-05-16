using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.UOW.Notes;

namespace Phytel.Engage.Integrations.UOW.ObjectMappers
{
    public static class MapperFactory
    {
        public static INoteMapper NoteMapper(string contract)
        {
            INoteMapper mapper = null;

            var typeInfos = Assembly.GetExecutingAssembly().DefinedTypes.Where(r => r.ImplementedInterfaces.Any(i => i == typeof (INoteMapper))).ToList();

            typeInfos.ForEach(r =>
            {
                var str = r.Name;
                var split = str.Split('_');
                if (String.Equals(contract, split[0], StringComparison.CurrentCultureIgnoreCase))
                {
                    mapper = (INoteMapper)Activator.CreateInstance(r);
                }
            });

            return mapper;
        }
    }
}
