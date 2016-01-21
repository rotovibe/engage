using System.Xml;
using AutoMapper;
using Funq;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.ASE.Core;
using Phytel.Engage.Integrations.Commands;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.DTO.Config;
using Phytel.Engage.Integrations.Process.Initialization;
using Phytel.Engage.Integrations.Repo.DTO;
using LogType = Phytel.Engage.Integrations.DomainEvents.LogType;

namespace Phytel.Engage.Integrations.QueueProcess
{
    public class IntegrationProcess : QueueProcessBase
    {
        private IMessageProcessor Processor { get; set; }
        public IIntegrationCommand<string, string> GetSystemIdCommand { get; set; }
        private string _contractName;
        public bool Debug { get; set; }

        public override void Execute(QueueMessage queueMessage)
        {
            AppConfigSettings.Initialize(base.Configuration.SelectNodes("//Phytel.ASE.Process/ProcessConfiguration/appSettings/add"));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(queueMessage.Body);
            var message = Mapper.Map<RegistryCompleteMessage>(doc.DocumentElement);
            _contractName = message.ContractDataBase;
            Processor.Process(message);
        }

        public IntegrationProcess()
        {
            Container container = new ContainerInitializer().Build();
            LoggerDomainEvent.Logger.EtlEvent += Logger_EtlEvent;

            GetSystemIdCommand = container.Resolve<IIntegrationCommand<string, string>>();
            Processor = container.Resolve<IMessageProcessor>();

            InitializeMappings();
        }

        private void Logger_EtlEvent(object sender, LogStatus e)
        {
            if (Debug) return;
            if (e.Type == LogType.Error)
                this.LogError("[" + _contractName + "] : " + e.Message, LogErrorCode.Error, LogErrorSeverity.Critical, string.Empty);
            else
                this.LogDebug("[" + _contractName + "] : " + e.Message);
        }

        private void InitializeMappings()
        {
            Mapper.CreateMap<XmlElement, RegistryCompleteMessage>()
                .ForMember(d => d.ContractDataBase, opt => opt.MapFrom(src => src.Attributes["contractdatabase"].Value))
                .ForMember(d => d.ContractId, opt => opt.MapFrom(src => src.Attributes["contractid"].Value))
                .ForMember(d => d.ReportDate, opt => opt.MapFrom(src => src.Attributes["reportdate"].Value))
                .ForMember(d => d.RunType, opt => opt.MapFrom(src => src.Attributes["runtype"].Value));

            Mapper.CreateMap<PatientXref, PatientSystemData>()
                .ForMember(d => d.Value,opt => opt.MapFrom(src => src.ExternalDisplayPatientId ?? src.ExternalPatientID))
                .ForMember(d => d.SystemId,opt => opt.MapFrom(src => GetSystemIdCommand.Execute(src.SendingApplication)))
                .ForMember(d => d.Primary, opt => opt.MapFrom(src => false))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => 1))
                .ForMember(d => d.DataSource, opt => opt.MapFrom(src => "P-Reg"))
                .ForMember(d => d.PatientId, opt => opt.MapFrom(src => src.PhytelPatientID))
                .ForMember(d => d.ExternalRecordId, opt => opt.MapFrom(src => src.ID))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => 1))
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(d => d.UpdatedOn, opt => opt.MapFrom(src => src.UpdateDate));
        }
    }
}
