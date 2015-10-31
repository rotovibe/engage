using System;
using System.Xml;

namespace Phytel.Engage.Integrations.DTO.Config
{
    public static class AppConfigSettings
    {
        public static void Initialize(XmlNodeList list)
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
            //<add key="DdPatientToDoServiceUrl" value="http://azurePhytelDev.cloudapp.net:59901/Scheduling" /> 
            //</appSettings> 
            //</ProcessConfiguration>
            //</Phytel.ASE.Process>

            if (list == null) return;
            foreach (XmlNode n in list)
            {
                if (n.Attributes == null) continue;
                switch (n.Attributes.GetNamedItem("key").Value)
                {
                    case "Contracts":
                        DTO.ProcConstants.Contracts = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "TakeCount":
                        DTO.ProcConstants.TakeCount = Convert.ToInt32(n.Attributes.GetNamedItem("value").Value);
                        break;
                    case "DDPatientServiceUrl":
                        DTO.ProcConstants.DdPatientServiceUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DDPatientSystemUrl":
                        DTO.ProcConstants.DdPatientSystemUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DDPatientNoteUrl":
                        DTO.ProcConstants.DdPatientNoteUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DDContactServiceUrl":
                        DTO.ProcConstants.DdContactServiceUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                    case "DdPatientToDoServiceUrl":
                        DTO.ProcConstants.DdPatientToDoServiceUrl = n.Attributes.GetNamedItem("value").Value;
                        break;
                }
            }
        }
    }
}
