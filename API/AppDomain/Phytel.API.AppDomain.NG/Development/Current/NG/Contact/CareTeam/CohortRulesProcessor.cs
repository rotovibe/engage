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
        private readonly IContactEndpointUtil EndpointUtil;
        private readonly ICareMemberCohortRuleFactory CareMemberCohortRuleFactory;
        private readonly ILogger _logger;
        public CohortRulesProcessor(ICareMemberCohortRuleFactory cf,IContactEndpointUtil ceu, ICohortRuleUtil cohortRuleUtil, ILogger logger)
        {
            _cohortRuleUtil = cohortRuleUtil;
            _cohortRuleCheckDataTeamQueue = new ConcurrentQueue<CohortRuleCheckData>();
            _logger = logger;
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
            if (_exitEvent != null)
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
                        if (_cohortRuleCheckDataTeamQueue.TryDequeue(out currCohortRuleCheckData) &&
                            currCohortRuleCheckData != null) ApplyCohortRules(currCohortRuleCheckData);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                }                         
            }
        }
        private List<string> GetAllUsersIds(string contractNumber, string userId, double version)
        {
            List<string> res = null;
            var getAllCareManagersRequest = new GetAllCareManagersRequest()
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            var careManagers = EndpointUtil.GetCareManagers(getAllCareManagersRequest);

            if (careManagers != null)
            {
                res = (from c in careManagers where c.IsUser select c.Id).ToList();
            }
            return res;
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
            var userList = GetAllUsersIds(cohortRuleCheckData.ContractNumber, cohortRuleCheckData.UserId, cohortRuleCheckData.Version);
            cohortRuleCheckData.UserIds = userList;
            var rules = CareMemberCohortRuleFactory.GenerateEngageCareMemberCohortRules();
            foreach (var rule in rules)
            {
                rule.Run(careTeam, cohortRuleCheckData);
            }
        }

    }
}
