using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Programs.ProgramAttributes
{
    public class EligibilityAttributeRule : ProgramAttributeRule, IProgramAttributeRule
    {
        private const int _programAttributeType = 10;
        public int ProgramAttributeType { get { return _programAttributeType; } }

        public override void Execute(SpawnElement r, Program program, string userId, ProgramAttributeData progAttr)
        {
            try
            {
                if (r.Tag == null)
                    throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                progAttr.Eligibility = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SetProgramAttributes()::Eligibility" + ex.Message, ex.InnerException);
            }

            int state; // no = 1, yes = 2
            var isNum = int.TryParse(r.Tag, out state);
            if (!isNum) return;

            // program is closed due to ineligibility
            switch (state)
            {
                case 1:
                    //program.ElementState = (int) DataDomain.Program.DTO.ElementState.Completed; //5;
                    //program.StateUpdatedOn = System.DateTime.UtcNow;
                    progAttr.Eligibility = 1;
                    //program.AttrEndDate = System.DateTime.UtcNow;
                    break;
                case 2:
                    //program.ElementState = (int) DataDomain.Program.DTO.ElementState.InProgress; //4;
                    //program.StateUpdatedOn = System.DateTime.UtcNow;
                    progAttr.Eligibility = 2;
                    break;
            }
        }
    }
}
