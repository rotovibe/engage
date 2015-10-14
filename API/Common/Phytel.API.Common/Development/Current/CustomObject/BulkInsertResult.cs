using System;
using System.Collections.Generic;

namespace Phytel.API.Common
{
    public class BulkInsertResult<T>
    {
        public List<T> Data { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}