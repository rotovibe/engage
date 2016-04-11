﻿using System;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    public class CareMemberData 
    {
        public string Id { get; set; }

        public string RoleId { get; set; }
        
        public string CustomRoleName { get; set; }
       
        public DateTime? StartDate { get; set; }
      
        public DateTime? EndDate { get; set; }
        
        public bool Core { get; set; }
        
        public string Notes { get; set; }
        
        public string Frequency { get; set; }

        public int Distance { get; set; }
        
        public string ExternalRecordId { get; set; }

        public string DataSource { get; set; }
    }
}
