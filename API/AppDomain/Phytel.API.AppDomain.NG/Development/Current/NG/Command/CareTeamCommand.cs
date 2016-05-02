using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class CareTeamCommand : INGCommand
    {
        private PostDeletePatientRequest _request;
        private IContactEndpointUtil _contactEndpointUtil { get; set; }
        
        public CareTeamCommand(PostDeletePatientRequest req)
        {
            _request = req as PostDeletePatientRequest;           
        }
    public void Execute()
        {
            try
            {
                // We get the patient contact card
                // We get the patient care team using the patient contact's Id
                // We send a request to the data domain to delete care team by care tam Id                
            }
            catch (Exception ex)
            {
                throw new Exception("AD: CareTeamCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
