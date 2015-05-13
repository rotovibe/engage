using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;

using C3.Business.Interfaces;
using C3.Data;
using C3.Data.Enum;
using Phytel.Framework.SQL.Data;


namespace C3.Business
{
    public class ControlService : SqlDataAccessor, IControlService
    {
        #region Private Variables
        
        private static volatile ControlService _instance;
        private static object _syncRoot = new Object();
        #endregion

        #region Constructors
        
        private string _dbConnName = "Phytel";

        public ControlService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        #endregion

        #region Public Methods
        
        public static ControlService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if ( _instance == null)
                            _instance = new ControlService();
                    }
                }
                return _instance;
            }
        }

        public List<Control> GetAllControls()
        {
            return CachedQueryAll<Control>(null, _dbConnName, StoredProcedure.GetAllControls, Control.Build, new CacheAccessor("C3Cache", "Controls"));
        }

        public Control GetControlByName(string name)
        {
            Control _control;
            Predicate<Control> c = delegate(Control c2) { return c2.Name == name; };
            _control = GetAllControls().Find(c);

            return _control;
        }

        public Control GetTabByPath(string path)
        {
            Control _control;
            //Predicate<Control> c = delegate(Control c2) { return c2.Path.Replace("~", "") == path; };
            _control = GetAllControls().Find((c3) => "/" + c3.Path == path);

            return _control;
        }
        public List<Control> GetTabControls()
        {
            return QueryAll<Control>(null, _dbConnName, StoredProcedure.GetTabs, Control.Build);
        }

        public List<Control> GetTabControlsByUser(Guid UserId)
        {
            return QueryAll<Control>(null, _dbConnName, StoredProcedure.GetTabsByUser, Control.Build, UserId);
        }

        public List<Control> GetSubTabControls()
        {
            return QueryAll<Control>(null, _dbConnName, StoredProcedure.GetSubTabs, Control.Build);
        }

        public List<Control> GetSubTabControls(int parentControlId)
        {
            List<Control> _subTabs;
            Predicate<Control> c = delegate(Control c2) { return c2.ParentControlId == parentControlId; };
            _subTabs = GetAllControls().FindAll(c);
            return _subTabs;
        }

        public List<Control> GetSubTabControlsByName(string tabName)
        {
            List<Control> _subTabs;

            Predicate<Control> c = delegate(Control c2) { return c2.ParentControlName == tabName && c2.IsSubTab == true; };
            _subTabs = GetAllControls().FindAll(c);
            //_subTabs.OrderByDescending(x => x.DisplayOrder).ToList();
            return _subTabs.OrderBy(x => x.DisplayOrder).ToList(); ;
        }

        public List<ControlPermission> GetAllControlsPermissions()
        {
            return CachedQueryAll<ControlPermission>(null, _dbConnName, StoredProcedure.GetAllControlPermissions, ControlPermission.Build, new CacheAccessor("C3Cache", "ControlPermissions"));
        }

        public ControlPermission GetCacheControlPermission(int controlId)
        {
            ControlPermission _permission = new ControlPermission();
            Predicate<ControlPermission> p = delegate(ControlPermission p2) { return p2.ControlId == controlId; };
            _permission = GetAllControlsPermissions().Find(p);
            return _permission;
        }

        public List<ControlPermission> GetControlCachedPermissions(int controlId, PermissionTypes type)
        {
            List<ControlPermission> _permissions = new List<ControlPermission>();
            Predicate<ControlPermission> p = delegate(ControlPermission p2) { return p2.ControlId == controlId && p2.PermissionTypeId == (int)type; };
            _permissions = GetAllControlsPermissions().FindAll(p);
            return _permissions;
        }

        #endregion

    }
}
