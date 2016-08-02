using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public interface IStepPlanProcessor
    {
        void PlanElementHandler(object sender, PlanElementEventArg e);

        /// <summary>
        /// Handles the response initialization to deletion of false and evokes any spawnelements.
        /// </summary>
        /// <param name="e">PlanElementEventArg</param>
        /// <param name="s">Step</param>
        void SetCompletedStepResponses(PlanElementEventArg e, Step s);

        void HandleResponseSpawnElements(Step s, Response r, PlanElementEventArg e, string userId);
        event SpawnEventHandler SpawnEvent;
        IPlanElementUtils PEUtils { get; set; }
        PlanProcessor Successor { get; set; }
        IElementActivationStrategy ElementActivationStrategy { get; set; }
        event ProcessedElementEventHandler ProcessedElementEvent;

        void ProcessWorkflow(IPlanElement planElem, Program program, string userId, string patientId,
            Actions action, IAppDomainRequest request);

        void OnPlanElement(PlanElementEventArg e);
        void HandlePlanElementActivation(PlanElementEventArg e, SpawnElement rse);
        void HandlePlanElementActions(PlanElementEventArg e, string userId, SpawnElement rse);
    }
}