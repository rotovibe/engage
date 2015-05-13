using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atmosphere.Core.Data.Services
{
    public class ServiceUriFormats
    {
        public const string Application = ""; // compiler doesn't allow string.Empty;

        // TODO Does not appear to be used ... let's see in regression
        ////// ContractsController
        ////public const string Contracts = "Contracts";

        ////public const string SingleContract = "Contracts/{0}";

        #region GroupsController

        public const string UserGroupsPerProduct = "Contracts/{0}/Groups/{1}/{2}";

        public const string UserGroups = "Contracts/{0}/Groups/{1}";

        public const string UserGroupsSubsribersPerProduct = "Contracts/{0}/GroupsSubscribers/{1}/{2}";

        public const string ContractGroups = "Contracts/{0}/Groups";

        public const string ContractGroup = "Contracts/{0}/Groups/{1}";

        #endregion

        #region SchedulesController

        public const string GroupsSchedules = "Contracts/{0}/Schedules/{1}";

        #endregion

        #region SubscribersController

        public const string UserSubscribersPerProduct = "Contracts/{0}/Subscribers/{1}/{2}";

        public const string UserSubscribers = "Contracts/{0}/Subscribers/{1}";

        public const string GroupsSubscribers = "Contracts/{0}/Subscribers/";

        #endregion

        #region PageViewController

        public const string PageViewSave = "Contracts/{0}/Users/PageViews/{1}/Save/";

        public const string PageViewDelete = "Contracts/{0}/Users/PageViews/{1}/Delete/";

        public const string PageViewList = "Contracts/{0}/Users/PageViews/{1}/{2}";

        public const string DefaultPageViewList = "Contracts/{0}/Users/PageViews/{1}";

        #endregion

		#region Search

		public const string PatientSearch = "search/patient/";

		#endregion
	}
}
