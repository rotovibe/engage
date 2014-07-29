﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Phone
    {
        public string Id { get; set; }
        public long Number { get; set; }
        public string TypeId { get; set; }
        public bool IsText { get; set; }
        public bool PhonePreferred { get; set; }
        public bool TextPreferred { get; set; }
        public bool OptOut { get; set; }
    }
}
