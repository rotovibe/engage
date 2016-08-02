using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.Repositories;
using Phytel.Engage.Integrations.UOW;


namespace Phytel.Engage.Fixes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Engage Data fix initializing...");
            InitializeMappings();
            Console.WriteLine("complete!");
            var pi = new PatientsImportUow { RepositoryFactory = new RepositoryFactory() };
            Console.WriteLine("getting repository...");
            //pi.Initialize("HILLCREST001");
            Console.WriteLine("Executing Data modification process...");
            var patientUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
            pi.PatientBackgroundModification("HILLCREST001", patientUrl);
            Console.WriteLine("Complete!");
            Console.ReadLine();
        }

        public static string Execute(string val)
        {
            var result = string.Empty;
            //AS-OH
            //MYSIS
            //OHMRN

            switch (val)
            {
                case "AS-OH":
                {
                    result = "55e47fb5d433232058923e86";
                    break;
                }
                case "MYSIS":
                {
                    result = "55e47fb3d433232058923e85";
                    break;
                }
                case "OHMRN":
                {
                    result = "55e47fb0d433232058923e84";
                    break;
                }
                case "Atmosphere":
                {
                    result = "55e47fb9d433232058923e87";
                    break;
                }
                case "Insurer":
                {
                    result = "55e47fbcd433232058923e88";
                    break;
                }
                case "Engage":
                {
                    result = "559d8453d433232ca04b3131";
                    break;
                }
                default:
                {
                    result = val;
                    break;
                }
            }

            return result;
        }

        private static void InitializeMappings()
        {
            //Mapper.CreateMap<XmlElement, RegistryCompleteMessage>()
            //    .ForMember(d => d.ContractDataBase, opt => opt.MapFrom(src => src.Attributes["contractdatabase"].Value))
            //    .ForMember(d => d.ContractId, opt => opt.MapFrom(src => src.Attributes["contractid"].Value))
            //    .ForMember(d => d.ReportDate, opt => opt.MapFrom(src => src.Attributes["reportdate"].Value))
            //    .ForMember(d => d.RunType, opt => opt.MapFrom(src => src.Attributes["runtype"].Value));

            Mapper.CreateMap<PatientXref, PatientSystemData>()
                .ForMember(d => d.Value, opt => opt.MapFrom(src => src.ExternalDisplayPatientId ?? src.ExternalPatientID))
                .ForMember(d => d.SystemId, opt => opt.MapFrom(src => Execute(src.SendingApplication)))
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
