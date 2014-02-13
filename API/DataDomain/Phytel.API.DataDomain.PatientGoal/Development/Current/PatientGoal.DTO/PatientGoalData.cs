﻿using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PatientGoalData
    {
        public string Id { get; set; }
        public List<string> FocusAreas { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public List<string> Programs { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<AttributeData> Attributes { get; set; }

        public List<PatientBarrierData> BarriersData { get; set; }
        public List<PatientTaskData> TasksData { get; set; }
        public List<PatientInterventionData> InterventionsData { get; set; }
    }
}
