using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Contact
{
    public class MruList : IMruList
    {
        public int Limit { get; set; }
        public List<string> RecentList {get; set;}

        // Remove a patientid's info from the list.
        private void RemoveFromRecentList(string patientId)
        {
            for (int i = RecentList.Count - 1; i >= 0; i--)
            {
                if (RecentList[i] == patientId) RecentList.RemoveAt(i);
            }
        }

        // Add a patient to the list, rearranging if necessary.
        public void AddPatient(string patientId)
        {
            // 1) Remove the patient from the list.
            RemoveFromRecentList(patientId);

            // 2) Add the patient to the beginning of the list.
            RecentList.Insert(0, patientId);

            // 3) If we have too many patientids, remove the last one.
            if (RecentList.Count > Limit) RecentList.RemoveAt(Limit);
        }

        public void RemovePatient(string patientId)
        {
            RemoveFromRecentList(patientId);
        }

    }
}

