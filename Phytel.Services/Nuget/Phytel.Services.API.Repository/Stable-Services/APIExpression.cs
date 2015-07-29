using System.Collections.Generic;

namespace Phytel.Services.API.Repository
{
    public class APIExpression
    {
        public string ExpressionID { get; set; }

        public ICollection<SelectExpression> Expressions { get; set; }

        public int skip { get; set; }

        public int take { get; set; }
    }
}