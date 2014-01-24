using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Email
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string TypeId { get; set; }
        public bool Preferred { get; set; }
        public bool OptOut { get; set; }
    }
}
