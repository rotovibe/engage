using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public class UpdatePatientProgramAssignedAttributes : MongoProcedure, IMongoProcedure
    {
        public const string Name = "mp_UpdatePatientProgramAssignedAttributes";
        public const string Description = "Procedure to update the existing fields like AssignedBy(NIGHT-832), AssignedDate(NIGHT-831) and AssignedTo(NIGHT-833) in PatientProgram collection.";

        public override void Implementation()
        {
            try
            {
                Results = new List<Result>();
                ObjectId systemObjectId = ObjectId.Parse(Constants.SystemContactId);
                IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(Request, RepositoryType.PatientProgram);

                List<MEPatientProgram> programs = (List<MEPatientProgram>)repo.SelectAll();

                foreach (MEPatientProgram mePP in programs)
                {
                    mePP.AssignedBy = systemObjectId;
                    mePP.AssignedOn = mePP.RecordCreatedOn;
                    mePP.LastUpdatedOn = DateTime.UtcNow;
                    mePP.UpdatedBy = systemObjectId;
                    MEPatientProgram updatedProgram = mePP;
                    bool success = repo.Save(updatedProgram);
                    if (success)
                    {
                        Results.Add(new Result { Message = "AssignedBy(aby), AssignedDate(aon) for Program Id [" + updatedProgram.Id.ToString() + "] are updated in PatientProgram collection." });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
