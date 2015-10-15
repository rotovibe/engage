using System;
using System.Collections.Generic;

namespace Phytel.API.Common
{
    public class BulkInsertResult
    {
        public List<string> ProcessedIds { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}