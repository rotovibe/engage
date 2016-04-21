using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AppDomain.Engage.Clinical.DataDomainClient;
using AppDomain.Engage.Clinical.DTO.Context;
using AppDomain.Engage.Clinical.DTO.Medications;
using AutoMapper;
using Phytel.API.DataDomain.Medication.DTO;
using MedicationData = AppDomain.Engage.Clinical.DTO.Medications.MedicationData;


namespace AppDomain.Engage.Clinical
{
    public class ClinicalManager : IClinicalManager
    {
        private readonly IServiceContext _context;
        private readonly IMedicationDataDomainClient _client;
        public UserContext UserContext { get; set; }

        public ClinicalManager(IServiceContext context, IMedicationDataDomainClient client)
        {
            _context = context;
            _client = client;
        }

        public PostPatientMedicationsResponse SavePatientMedications(List<MedicationData> medsData)
        {
            List<PatientMedSuppData> patientMedSupp = new List<PatientMedSuppData>();
            MappMedication mapper = new MappMedication();
            medsData.ForEach(x => patientMedSupp.Add(mapper.Map(x)));


            _client.PostPatientMedications(patientMedSupp);
            System.Threading.Thread.Sleep(5000);

            return new PostPatientMedicationsResponse(); // add new dd client method call.  ie _client.SavePatientMedications.
        }
    }


    public class MappMedication : Profile
    {
        //protected override void Configure()
        //{
        //    base.Configure();
        //    //CreateMap<PatientMedSuppData, MedicationData>()
        //    //    .ForMember(x => x.category, opt => opt.MapFrom(y => y.CategoryId))
        //    //    .ForMember(x => x.startDate, opt => opt.MapFrom(y => y.StartDate.Value))
        //    //    .ForMember(x => x.endDate, opt => opt.MapFrom(y => y.EndDate.Value))
        //    //    .ForMember(x => x.externalRecordId, opt => opt.MapFrom(y => y.ExternalRecordId))
        //    //    .ForMember(x => x.dosage, opt => opt.MapFrom(y => y.FreqQuantity))
        //    //    .ForMember(x => x.strength, opt => opt.MapFrom(y => y.Strength))
        //    //    .ForMember(x => x.sig, opt => opt.MapFrom(y => y.SigCode))
        //    //    .ForMember(x => x.route, opt => opt.MapFrom(y => y.Route))
        //    //    .ForMember(x => x.form, opt => opt.MapFrom(y => y.Form))
        //    //    .ForMember(x => x.reason, opt => opt.MapFrom(y => y.Reason))
        //    //    .ForMember(x => x.medType, opt => opt.MapFrom(y => y.TypeId))
        //    //    .ForMember(x => x.medName, opt => opt.MapFrom(y => y.Name))
        //    //    .ForMember(x => x.sourceType, opt => opt.MapFrom(y => y.SourceId))
        //    //    .ForMember(x => x.patientId, opt => opt.MapFrom(y => y.PatientId))
        //    //    .ForMember(x => x.externalSystem, opt => opt.MapFrom(y => y.SystemName))
        //    //    .ForMember(x => x.dosageFreq, opt => opt.MapFrom(y => y.FrequencyId))
        //    //    .ForMember(x => x.notes, opt => opt.MapFrom(y => y.Notes))
        //    //    .ForMember(x => x.prescribedBy, opt => opt.MapFrom(y => y.PrescribedBy))
        //    //    .ForMember(x => DateTime.Today, opt => opt.MapFrom(y => y.CreatedOn))
        //    //    .ForMember(x => DateTime.Today, opt => opt.MapFrom(y => y.UpdatedOn))
        //    //    .ForMember(x => x.medCodes, opt => opt.MapFrom(y => y.NDCs))
        //    //    .ForMember(x => x.medClasses, opt => opt.MapFrom(y => y.PharmClasses));
        //}

        public PatientMedSuppData Map(MedicationData medication)
        {
            PatientMedSuppData patientMedSupp = new PatientMedSuppData
            {
                CategoryId = int.Parse(medication.category),
                CreatedOn = DateTime.Today,
                UpdatedOn = DateTime.Today,
                StartDate = medication.startDate ?? DateTime.Today,
                EndDate = medication.endDate ?? DateTime.Today,
                ExternalRecordId = medication.externalRecordId,
                FreqQuantity = medication.dosage,
                Strength = medication.strength,
                SigCode = medication.sig,
                Route = medication.route,
                Form = medication.form,
                Reason = medication.reason,
                TypeId = medication.medType,
                Name = medication.medName,
                SourceId = medication.sourceType,
                PatientId = medication.patientId,
                SystemName = medication.externalSystem,
                FrequencyId = medication.dosageFreq,
                Notes = medication.notes,
                PrescribedBy = medication.prescribedBy,
                NDCs = medication.medCodes.ToList(),
                PharmClasses = medication.medClasses.ToList()
            };
            // patientMedSupp = Mapper.Map<MedicationData, PatientMedSuppData>(medication);

            return patientMedSupp;
        }
    }
}