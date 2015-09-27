using System;
using System.Xml;
using Funq;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.ASE.Core;
using Phytel.Engage.Integrations.Configurations;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.Specifications;
using Phytel.Engage.Integrations.UOW;
using System.Collections.Generic;
using AutoMapper;
using Phytel.Engage.Integrations.Commands;
using Phytel.Engage.Integrations.Process.Initialization;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.QueueProcess
{
    public class IntegrationProcess : QueueProcessBase
    {
        private IMessageProcessor Processor { get; set; }
        public IIntegrationCommand<string, string> GetSystemIdCommand { get; set; }

        public override void Execute(QueueMessage queueMessage)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(queueMessage.Body);
            Processor.Process(Mapper.Map<RegistryCompleteMessage>(doc.DocumentElement));
        }

        public IntegrationProcess()
        {
            Container container = new ContainerInitializer().Build();
            GetSystemIdCommand = container.Resolve<IIntegrationCommand<string, string>>();
            Processor = container.Resolve<IMessageProcessor>();

            InitializeMappings();
        }

        private void InitializeMappings()
        {
            Mapper.CreateMap<XmlElement, RegistryCompleteMessage>()
                .ForMember(d => d.ContractDataBase, opt => opt.MapFrom(src => src.Attributes["contractdatabase"].Value))
                .ForMember(d => d.ContractId, opt => opt.MapFrom(src => src.Attributes["contractid"].Value))
                .ForMember(d => d.ReportDate, opt => opt.MapFrom(src => src.Attributes["reportdate"].Value))
                .ForMember(d => d.RunType, opt => opt.MapFrom(src => src.Attributes["runtype"].Value));

            Mapper.CreateMap<PatientInfo, PatientData>()
                .ForMember(d => d.AtmosphereId, opt => opt.MapFrom(src => src.PatientId.ToString()))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.MiddleName, opt => opt.MapFrom(src => src.MiddleInitial))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(d => d.Suffix, opt => opt.MapFrom(src => src.Suffix))
                .ForMember(d => d.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(d => d.DOB, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(d => d.FullSSN, opt => opt.MapFrom(src => src.Ssn))
                .ForMember(d => d.RecordCreatedOn, opt => opt.MapFrom(src => src.CreateDate.GetValueOrDefault()))
                .ForMember(d => d.LastUpdatedOn, opt => opt.MapFrom(src => src.UpdateDate.GetValueOrDefault()))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => PatientInfoUtils.GetStatus(src.Status)))
                .ForMember(d => d.DataSource, opt => opt.MapFrom(src => "P-Reg"))
                .ForMember(d => d.DeceasedId, opt => opt.MapFrom(src => PatientInfoUtils.GetDeceased(src.Status)))
                .ForMember(d => d.PriorityData, opt => opt.MapFrom(src => Convert.ToInt32(src.Priority)));

            Mapper.CreateMap<PatientXref, PatientSystemData>()
                .ForMember(d => d.Value, opt => opt.MapFrom(src => src.ExternalDisplayPatientId ?? src.ExternalPatientID))
                .ForMember(d => d.SystemId, opt => opt.MapFrom(src => GetSystemIdCommand.Execute(src.SendingApplication)))
                .ForMember(d => d.Primary, opt => opt.MapFrom(src => false))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => 1))
                .ForMember(d => d.DataSource, opt => opt.MapFrom(src => "P-Reg"));
        }
    }
}
