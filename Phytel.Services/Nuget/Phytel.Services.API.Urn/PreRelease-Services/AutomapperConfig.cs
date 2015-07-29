using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.API.Urn
{
    public static class AutomapperConfig
    {
        public const string EnumerableSeperator = "&";
        public static void Config()
        {
            Mapper.CreateMap<IEnumerable<int>, string>().ConvertUsing((source) => {
                string rvalue = string.Empty;
                if(source != null && source.Any())
                {
                    rvalue = string.Join(EnumerableSeperator, source.Select(x => x.ToString()));
                }

                return rvalue;
            });

            Mapper.CreateMap<IEnumerable<double>, string>().ConvertUsing((source) =>
            {
                string rvalue = string.Empty;
                if (source != null && source.Any())
                {
                    rvalue = string.Join(EnumerableSeperator, source.Select(x => x.ToString()));
                }

                return rvalue;
            });

            Mapper.CreateMap<IEnumerable<decimal>, string>().ConvertUsing((source) =>
            {
                string rvalue = string.Empty;
                if (source != null && source.Any())
                {
                    rvalue = string.Join(EnumerableSeperator, source.Select(x => x.ToString()));
                }

                return rvalue;
            });

            Mapper.CreateMap<IEnumerable<string>, string>().ConvertUsing((source) =>
            {
                string rvalue = string.Empty;
                if (source != null && source.Any())
                {
                    rvalue = string.Join(EnumerableSeperator, source);
                }

                return rvalue;
            });
        }
    }
}
