using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    /// <summary>
    /// This is the Lucene Document used for Allergy registration into the Index.
    /// </summary>
    public class AllergySearchDoc
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
