using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubPatientSystemEndpointUtil : IPatientSystemEndpointUtil
    {
        private List<PatientSystemData> _plData;
        private string _patientId;


        public StubPatientSystemEndpointUtil()
        {
            _patientId = ObjectId.GenerateNewId().ToString();

            _plData = new List<PatientSystemData>
            {
                new PatientSystemData
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PatientId = _patientId,
                    Value = "66789",
                    Primary = true
                },
                new PatientSystemData
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PatientId = _patientId,
                    Value = "4rerty",
                    Primary = false
                },
                new PatientSystemData
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PatientId = _patientId,
                    Value = "uieu39427",
                    Primary = false
                }
            };
        }

        public List<SystemData> GetSystems(IServiceContext context)
        {
            throw new NotImplementedException();
        }

        public List<PatientSystemData> GetPatientSystems(IServiceContext context, string patientId)
        {
            var list = _plData;
            return list;
        }

        public List<PatientSystemOldData> GetAllPatientSystems(IServiceContext context)
        {
            throw new NotImplementedException();
        }

        public List<PatientData> GetAllPatients(IServiceContext request)
        {
            throw new NotImplementedException();
        }

        public List<PatientSystemData> InsertPatientSystems(IServiceContext context, string patientId)
        {
            throw new NotImplementedException();
        }

        public List<PatientSystemData> UpdatePatientSystems(IServiceContext context, string patientId)
        {
            var list = _plData;
            return list;
        }

        public void DeletePatientSystems(DeletePatientSystemsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
