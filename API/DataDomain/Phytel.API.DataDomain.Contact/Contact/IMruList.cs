using MongoDB.Bson;
using System;
using System.Collections.Generic;
namespace Phytel.API.DataDomain.Contact
{
    public interface IMruList
    {
        void AddPatient(string patientId);
        int Limit { get; set; }
        List<string> RecentList { get; set; }
        void RemovePatient(string patientId);
    }
}
