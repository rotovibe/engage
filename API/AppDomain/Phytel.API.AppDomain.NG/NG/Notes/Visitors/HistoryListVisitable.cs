using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Note.Context;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class HistoryListVisitable
    {
        private List<PatientNote> _histList;
        private readonly string _contractNumber;
        private readonly double _version;
        private readonly string _patientId;
        private readonly int _count;
        private readonly string _userId;

        public HistoryListVisitable(IServiceContext context)
        {
            _histList = new List<PatientNote>();
            _contractNumber = context.Contract;
            _version = context.Version;
            _patientId = ((PatientNoteContext)context.Tag).PatientId;
            _count = ((PatientNoteContext)context.Tag).Count;
            _userId = context.UserId;
        }

        public void Accept(VisitorBase visitor)
        {
            visitor.ContractNumber = _contractNumber;
            visitor.Count = _count;
            visitor.PatientId = _patientId;
            visitor.UserId = _userId;
            visitor.Version = _version;
            
            visitor.Visit(ref _histList);
        }

        public void Modify(ModifierBase modifier)
        {
            _histList = modifier.Modify(ref _histList);
        }

        public List<PatientNote> GetList()
        {
            return _histList;
        }
    }
}
