using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class PatientNotesRepository : IRepository
    {
        private string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }

        public PatientNotesRepository(string contract, ISQLConnectionProvider conProvider)
        {
            _contract = contract;
            ConnStr = conProvider;
        }

        public object SelectAll()
        {
            try
            {
                List<PatientNote> ptInfo = null;
                using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
                {
                    var query = (from pn in ct.C3NotePatient
                        join na in ct.C3NoteAction on pn.ActionID equals na.ActionID
                        join nc in ct.C3NoteCategory on pn.CategoryId equals nc.CategoryId
                        select new PatientNote
                        {
                            NoteId = pn.NoteId,
                            Note = pn.Note,
                            ActionID = na.ActionID,
                            ActionName = na.ActionName,
                            CategoryId = nc.CategoryId,
                            CategoryName = nc.CategoryName,
                            PatientId = pn.PatientId,
                            Enabled = pn.Enabled.ToString(),
                            CreatedDate = pn.CreatedDate,
                            UpdatedDate = pn.UpdatedDate,
                            CreatedBy = pn.CreatedBy,
                            CreatedById = pn.CreatedById.ToString(),
                            DeletedBy = pn.DeletedBy,
                            DeletedDate = pn.DeletedDate,
                            DeletedStatus = pn.DeletedStatus.ToString()
                        });

                    ptInfo = query.ToList();
                }

                return ptInfo;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("PatientsNotesRepository:SelectAll(): " + ex.Message, false));
                throw;
            }
        }



        public object Insert(object list)
        {
            return null;
        }
    }
}
