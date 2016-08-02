using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Commands;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.QueueProcess;
using Phytel.Engage.Integrations.Repo.DTO;

namespace Phytel.Engage.Integrations.ProcessTests
{
    [TestClass()]
    public class IntegrationProcessTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            //<RegistryComplete contractid="465" contractdatabase="ORLANDOHEALTH001" runtype="Daily" reportdate="09/11/1012"/>
            SetMappings();
            var watch = Stopwatch.StartNew();
            const string body = @"<RegistryComplete contractid=""465"" contractdatabase=""HILLCREST001"" runtype=""Daily"" reportdate=""09/11/1012""/>";
            var queMessage = new QueueMessage {Body = body};
            var xmldoc = LoadTextXmlDoc();
            //var nList = xmldoc.DocumentElement.ChildNodes;
            //var proc = new IntegrationProcess(nList);
            var proc = new IntegrationProcess{ Debug = true};
            proc.Configuration = xmldoc.DocumentElement;
            proc.Execute(queMessage);

            watch.Stop();
            var elapsed = TimeSpan.FromMilliseconds( watch.ElapsedMilliseconds).TotalMinutes;
        }

        private void SetMappings()
        {
            Mapper.CreateMap<XmlElement, RegistryCompleteMessage>()
                .ForMember(d => d.ContractDataBase, opt => opt.MapFrom(src => src.Attributes["contractdatabase"].Value))
                .ForMember(d => d.ContractId, opt => opt.MapFrom(src => src.Attributes["contractid"].Value))
                .ForMember(d => d.ReportDate, opt => opt.MapFrom(src => src.Attributes["reportdate"].Value))
                .ForMember(d => d.RunType, opt => opt.MapFrom(src => src.Attributes["runtype"].Value));

            IIntegrationCommand<string, string> getSystemIdCommand = new GetSendingApplicationId();
            Mapper.CreateMap<PatientXref, PatientSystemData>()
                .ForMember(d => d.Value, opt => opt.MapFrom(src => src.ExternalDisplayPatientId ?? src.ExternalPatientID))
                .ForMember(d => d.SystemId, opt => opt.MapFrom(src => getSystemIdCommand.Execute(src.SendingApplication)))
                .ForMember(d => d.Primary, opt => opt.MapFrom(src => false))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => 1))
                .ForMember(d => d.DataSource, opt => opt.MapFrom(src => "P-Reg"))
                .ForMember(d => d.PatientId, opt => opt.MapFrom(src => src.PhytelPatientID))
                .ForMember(d => d.ExternalRecordId, opt => opt.MapFrom(src => src.ID))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => 1));
        }

        public static XmlDocument LoadTextXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();

            //<Phytel.ASE.Process>
            var aseProcess = xmlDoc.CreateElement("Phytel.ASE.Process");
            var rootNode = xmlDoc.AppendChild(aseProcess);

            //<ProcessConfiguration>
            var procConfiguration = xmlDoc.CreateElement("ProcessConfiguration");
            var procConfig = rootNode.AppendChild(procConfiguration);

            var root = xmlDoc.CreateElement("appSettings");
            var appConfig = procConfig.AppendChild(root);

            //<add key="TakeCount" value="5000" />
            var takeCount = xmlDoc.CreateElement("add");
            takeCount.SetAttribute("key", "TakeCount");
            takeCount.SetAttribute("value", "5000");
            appConfig.AppendChild(takeCount);

            //<add key="PhytelServicesConnName" value="Phytel" />
            var phytelServicesConnName = xmlDoc.CreateElement("add");
            phytelServicesConnName.SetAttribute("key", "PhytelServicesConnName");
            phytelServicesConnName.SetAttribute("value", "Phytel");
            appConfig.AppendChild(phytelServicesConnName);

            //<add key="Contracts" value="ORLANDOHEALTH001" />
            var contracts = xmlDoc.CreateElement("add");
            contracts.SetAttribute("key", "Contracts");
            contracts.SetAttribute("value", "HILLCREST001");
            appConfig.AppendChild(contracts);
            
            //<add key="DDPatientServiceUrl" value="http://azurePhytelDev.cloudapp.net:59901/Patient" />
            var dDPatientServiceUrl = xmlDoc.CreateElement("add");
            dDPatientServiceUrl.SetAttribute("key", "DDPatientServiceUrl");
            dDPatientServiceUrl.SetAttribute("value", "http://azurePhytel.cloudapp.net:59901/Patient");
            appConfig.AppendChild(dDPatientServiceUrl);

            //<add key="DDPatientSystemUrl" value="http://azurePhytel.cloudapp.net:59901/PatientSystem" />
            var dDPatientSystemUrl = xmlDoc.CreateElement("add");
            dDPatientSystemUrl.SetAttribute("key", "DDPatientSystemUrl");
            dDPatientSystemUrl.SetAttribute("value", "http://azurePhytel.cloudapp.net:59901/PatientSystem");
            appConfig.AppendChild(dDPatientSystemUrl);

            //<add key="DDPatientNoteUrl" value="http://azurePhytel.cloudapp.net:59901/PatientNote" />
            var dPatientSystemUrl = xmlDoc.CreateElement("add");
            dPatientSystemUrl.SetAttribute("key", "DDPatientNoteUrl");
            dPatientSystemUrl.SetAttribute("value", "http://azurePhytel.cloudapp.net:59901/PatientNote");
            appConfig.AppendChild(dPatientSystemUrl);

            //<add key="DDContactServiceUrl" value="http://azurePhytel.cloudapp.net:59901/Contact" />   
            var dDContactServiceUrl = xmlDoc.CreateElement("add");
            dDContactServiceUrl.SetAttribute("key", "DDContactServiceUrl");
            dDContactServiceUrl.SetAttribute("value", "http://azurePhytel.cloudapp.net:59901/Contact");
            appConfig.AppendChild(dDContactServiceUrl);

            //<add key="DdPatientToDoServiceUrl" value="http://azurePhytel.cloudapp.net:59901/Scheduling" /> 
            var ddPatientToDoServiceUrl = xmlDoc.CreateElement("add");
            ddPatientToDoServiceUrl.SetAttribute("key", "DdPatientToDoServiceUrl");
            ddPatientToDoServiceUrl.SetAttribute("value", "http://azurePhytel.cloudapp.net:59901/Scheduling");
            appConfig.AppendChild(ddPatientToDoServiceUrl); 

            return xmlDoc;
        }
    }
}