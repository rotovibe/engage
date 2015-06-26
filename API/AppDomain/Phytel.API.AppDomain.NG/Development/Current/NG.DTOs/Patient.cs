﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Patient
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DisplaySystemId { get; set; }
        public string DisplaySystemName { get; set; }
        public string DisplayLabel { get; set; }
        public int Priority { get; set; }
        public int Flagged { get; set; }
        public string Background { get; set; }
        public string ClinicalBackground { get; set; }
        public int? LastFourSSN { get; set; }
        public string FullSSN { get; set; }
        public string System { get; set; }

    }
}
