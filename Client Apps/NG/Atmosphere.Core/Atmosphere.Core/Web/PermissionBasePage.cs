using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using C3.Business;
using C3.Data;
using C3.Data.Enum;

namespace C3.Web
{
    public class PermissionBasePage : BasePage
    {
        #region Private Variables
        bool hasViewPermission = false;
        bool hasEditPermission = false;
        #endregion

        #region Protected Override

        protected override void OnLoad(EventArgs e)
        {
            if (CurrentUser != null)
            {
                //Now we are checking the view permissions
                if (ViewPermissions.Count > 0)
                {
                    //Check to see if user has the ViewPermissions required
                    foreach (ControlPermission permission in ViewPermissions)
                    {
                        Predicate<Permission> p = delegate(Permission p2) { return p2.PermissionId == permission.PermissionId; };
                        if (CurrentUser.ControlPermissions.Exists(p))
                        {
                            hasViewPermission = true;
                            break;
                        }
                        else
                        {
                            hasViewPermission = false;
                        }
                    }
                }

                if (EditPermissions.Count > 0)
                {
                    foreach (ControlPermission permission in EditPermissions)
                    {
                        Predicate<Permission> p = delegate(Permission p2) { return p2.PermissionId == permission.PermissionId; };
                        if (CurrentUser.ControlPermissions.Exists(p))
                        {
                            hasEditPermission = true;
                            break;
                        }
                        else
                        {
                            hasEditPermission = false;
                        }
                    }
                }
            }
            else
            {
                hasViewPermission = false;
                hasEditPermission = false;
            }

            if (hasViewPermission == false && hasEditPermission == false)
            {
                Response.Redirect(GlobalSiteRoot + "AccessDenied.aspx?Page=" + PhytelPageName);
            }

            base.OnLoad(e);
        }
        
        #endregion

        #region Public Properties
        public List<ControlPermission> ViewPermissions
        {
            get
            {
                List<ControlPermission> _viewPermissions = new List<ControlPermission>();
                _viewPermissions = ControlService.Instance.GetControlCachedPermissions(Control.ControlId, PermissionTypes.View);
                return _viewPermissions;
            }
        }

        public List<ControlPermission> EditPermissions
        {
            get
            {
                List<ControlPermission> _editPermissions = new List<ControlPermission>();
                _editPermissions = ControlService.Instance.GetControlCachedPermissions(Control.ControlId, PermissionTypes.Edit);
                return _editPermissions;
            }
        }

        public List<ControlPermission> DeletePermissions
        {
            get
            {
                List<ControlPermission> _deletePermissions = new List<ControlPermission>();
                _deletePermissions = ControlService.Instance.GetControlCachedPermissions(Control.ControlId, PermissionTypes.Delete);
                return _deletePermissions;
            }
        }

        public List<ControlPermission> PrintPermissions
        {
            get
            {
                List<ControlPermission> _printPermissions = new List<ControlPermission>();
                _printPermissions = ControlService.Instance.GetControlCachedPermissions(Control.ControlId, PermissionTypes.Print);
                return _printPermissions;
            }
        }

        public List<ControlPermission> ExportPermissions
        {
            get
            {
                List<ControlPermission> _exportPermissions = new List<ControlPermission>();
                _exportPermissions = ControlService.Instance.GetControlCachedPermissions(Control.ControlId, PermissionTypes.Export);
                return _exportPermissions;
            }
        }

        #endregion
    }
}