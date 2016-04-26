using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Common.Net30;

namespace Phytel.API.AppDomain.NG
{
    public class CohortRulesProcessor: ICohortRulesProcessor
    {
        private readonly ICohortRuleUtil _cohortRuleUtil;
        protected ConcurrentQueue<CohortRuleCheckData> _cohortRuleCheckDataTeamQueue;        
        private AutoResetEvent _queuEvent;
        private ManualResetEvent _exitEvent;
        private WaitHandle[] wHandles;
        private Thread _processQueueThread;
        private object queueLock;
        private IContactEndpointUtil EndpointUtil { get; set; }
        private ICareMemberCohortRuleFactory CareMemberCohortRuleFactory { get; set; }
        public CohortRulesProcessor(ICareMemberCohortRuleFactory cf,IContactEndpointUtil ceu, ICohortRuleUtil cohortRuleUtil)
        {
            _cohortRuleUtil = cohortRuleUtil;
            _cohortRuleCheckDataTeamQueue = new ConcurrentQueue<CohortRuleCheckData>();
            CareMemberCohortRuleFactory = cf;
            EndpointUtil = ceu;            
            _queuEvent = new AutoResetEvent(false);
            _exitEvent = new ManualResetEvent(false);
            wHandles = new WaitHandle[]
            {
                _queuEvent,
                _exitEvent
            };
            queueLock = new object();
            var threadDelegate = new ThreadStart(ProcessQueue);
            _processQueueThread = new Thread(threadDelegate) {Name = "CohortRulesThread"};
            _processQueueThread.Start();
        }

        public void EnqueueCohorRuleCheck(CohortRuleCheckData cohortRuleCheckData)
        {
            lock (queueLock)
            {
                _cohortRuleCheckDataTeamQueue.Enqueue(cohortRuleCheckData);
                _queuEvent.Set();
            }
            
        }

        public string GetCareTeamActiveCorePCMId(CohortRuleCheckData cohortRuleCheckData)
        {
            string res = null;
            var careTeamData = EndpointUtil.GetCareTeam(new GetCareTeamRequest
            {
                ContactId = cohortRuleCheckData.ContactId,
                ContractNumber = cohortRuleCheckData.ContractNumber,
                UserId = cohortRuleCheckData.UserId
            });

            if (careTeamData != null)
            {
                var careTeam = Mapper.Map<CareTeam>(careTeamData);
                var m = _cohortRuleUtil.GetCareTeamActiveCorePCM(careTeam);
                if (m != null) res = m.Id;
            }
            return res;
        }
              
        public void Stop()
        {
            if (_exitEvent!=null)
                _exitEvent.Set();            
        }

        private void ProcessQueue()
        {
            while (WaitHandle.WaitAny(wHandles)!=1)
            {
                try
                {
                    lock (queueLock)
                    {
                        CohortRuleCheckData currCohortRuleCheckData = null;
                        if (_cohortRuleCheckDataTeamQueue.TryDequeue(out currCohortRuleCheckData))
                            ApplyCohortRules(currCohortRuleCheckData);
                    }
                }
                catch (Exception ex)
                {
                    //TODO what do we do if we fail to apply a cohort rule?    
                    throw;
                }                         
            }
        }
       
        private void ApplyCohortRules(CohortRuleCheckData cohortRuleCheckData)
        {
            CareTeam careTeam = null;
            var careTeamData = EndpointUtil.GetCareTeam(new GetCareTeamRequest
            {
                ContactId = cohortRuleCheckData.ContactId,
                ContractNumber = cohortRuleCheckData.ContractNumber,
                UserId = cohortRuleCheckData.UserId
            });

            if (careTeamData == null) return;
            careTeam = Mapper.Map<CareTeam>(careTeamData);
            var rules = CareMemberCohortRuleFactory.GenerateEngageCareMemberCohortRules();
            foreach (var rule in rules)
            {
                rule.Run(careTeam, cohortRuleCheckData);
            }
        }

    }
}
