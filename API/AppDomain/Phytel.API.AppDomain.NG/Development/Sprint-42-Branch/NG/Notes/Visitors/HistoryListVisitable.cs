using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

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

        public HistoryListVisitable(GetAllPatientNotesRequest request)
        {
            _histList = new List<PatientNote>();
            _contractNumber = request.ContractNumber;
            _version = request.Version;
            _patientId = request.PatientId;
            _count = request.Count;
            _userId = request.UserId;
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
            _histList = modifier.Modify(_histList);
        }

        public List<PatientNote> GetList()
        {
            return _histList;
        }
    }
}
