using System;
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

        public override void Execute(QueueMessage queueMessage)
        {
            LoggerDomainEvent.Raise(LogStatus.Create("*** Atmosphere Import Start ***" + " contract", true));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(queueMessage.Body);
            InitializeContractSettings();

            var message = Mapper.Map<RegistryCompleteMessage>(doc.DocumentElement);
            _contractName = message.ContractDataBase;

            LoggerDomainEvent.Raise(LogStatus.Create("Initializing Integration process for : " + message.ContractDataBase  +" contract", true));
            Processor.Process(message);
            LoggerDomainEvent.Raise(LogStatus.Create("Atmosphere Patient Import completed.", true));
        }

        public void InitializeContractSettings()
        {
            //<Phytel.ASE.Process>
            //<ProcessConfiguration>
            //<appSettings>
            //<add key="TakeCount" value="5000" />
            //<add key="PhytelServicesConnName" value="Phytel" />
            //<add key="Contracts" value="ORLANDOHEALTH001" />
            //<add key="DDPatientServiceUrl" value="http://azurePhytelDev.cloudapp.net:59901/Patient" />
            //<add key="DDPatientSystemUrl" value="http://azurePhytelDev.cloudapp.net:59901/PatientSystem" />
            //<add key="DDPatientNoteUrl" value="http://azurePhytelDev.cloudapp.net:59901/PatientNote" />
            //<add key="DDContactServiceUrl" value="http://azurePhytelDev.cloudapp.net:59901/Contact" />    
            //</appSettings> 
            //</ProcessConfiguration>
            //</Phytel.ASE.Process>

            var list = base.Configuration.SelectNodes("//Phytel.ASE.Process/ProcessConfiguration/appSettings/add");

            if (list == null) return;
            LoggerDomainEvent.Raise(LogStatus.Create("loop through node list.", true));
            foreach (XmlNode n in list)
            {
                LoggerDomainEvent.Raise(LogStatus.Create(n.Name, true));
                LoggerDomainEvent.Raise(LogStatus.Create(n.Attributes.GetNamedItem("key").Value, true));
                switch (n.Attributes.GetNamedItem("key").Value)
                {
                    case "Contracts":
                        ProcConstants.Contracts = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "TakeCount":
                        ProcConstants.TakeCount = Convert.ToInt32(n.Attributes.GetNamedItem("value").Value);
                        LoggerDomainEvent.Raise(LogStatus.Create("takecount: " + ProcConstants.TakeCount, true));
                        break;
                    case "DDPatientServiceUrl":
                        ProcConstants.DdPatientServiceUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DDPatientSystemUrl":
                        ProcConstants.DdPatientSystemUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DDPatientNoteUrl":
                        ProcConstants.DdPatientNoteUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DDContactServiceUrl":
                        ProcConstants.DdContactServiceUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                }
            }
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
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => 1));
        }
    }
}
