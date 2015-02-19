﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication
{
    public class PatientMedSuppDataManager : IPatientMedSuppDataManager
    {
        public IMongoPatientMedSuppRepository PatientMedSuppRepository { get; set; }
        
        public List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsDataRequest request)
        {
            try
            {
                List<PatientMedSuppData> result = null;
                //PatientMedSuppRepository.UserId = request.UserId;
                if (request.PatientId != null)
                {
                    var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
                    result = repo.FindByPatientId(request) as List<PatientMedSuppData>;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientMedSuppData SavePatientMedSupps(PutPatientMedSuppDataRequest request)
        {
            try
            {
                PatientMedSuppData result = null;
                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);

                if (request.PatientMedSuppData != null)
                {
                    if (request.Insert)
                    {
                        result = (PatientMedSuppData)repo.Insert(request);
                    }
                    else
                    {
                        bool status = (bool)repo.Update(request);
                        if (status)
                        {
                            result = (PatientMedSuppData)repo.FindByID(request.PatientMedSuppData.Id);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public DeleteMedSuppsByPatientIdDataResponse DeletePatientMedSupps(DeleteMedSuppsByPatientIdDataRequest request)
        {
            DeleteMedSuppsByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteMedSuppsByPatientIdDataResponse();

                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
                GetPatientMedSuppsDataRequest getAllPatientMedSuppsDataRequest = new GetPatientMedSuppsDataRequest
                {
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    UserId = request.UserId,
                    Version = request.Version
                };
                List<PatientMedSuppData> patientMedSupps = repo.FindByPatientId(getAllPatientMedSuppsDataRequest) as List<PatientMedSuppData>;
                List<string> deletedIds = null;
                if (patientMedSupps != null)
                {
                    deletedIds = new List<string>();
                    patientMedSupps.ForEach(u =>
                    {
                        DeleteMedSuppsByPatientIdDataRequest deletePMSDataRequest = new DeleteMedSuppsByPatientIdDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            Id = u.Id,
                            PatientId = request.PatientId,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        repo.Delete(deletePMSDataRequest);
                        deletedIds.Add(deletePMSDataRequest.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeletePatientMedSuppsDataResponse UndoDeletePatientMedSupps(UndoDeletePatientMedSuppsDataRequest request)
        {
            UndoDeletePatientMedSuppsDataResponse response = null;
            try
            {
                response = new UndoDeletePatientMedSuppsDataResponse();

                var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.PatientMedSupp);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientMedSuppId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
