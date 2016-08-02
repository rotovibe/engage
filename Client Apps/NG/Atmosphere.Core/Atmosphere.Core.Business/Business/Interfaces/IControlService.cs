using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using C3.Data;
using C3.Data.Enum;

namespace C3.Business.Interfaces
{
    public interface IControlService
    {
        List<Control> GetAllControls();
        Control GetControlByName(string name);
        Control GetTabByPath(string path);
        List<Control> GetTabControls();
        List<Control> GetTabControlsByUser(Guid UserId);
        List<Control> GetSubTabControls();
        List<Control> GetSubTabControls(int parentControlId);
        List<Control> GetSubTabControlsByName(string tabName);
        List<ControlPermission> GetAllControlsPermissions();
        ControlPermission GetCacheControlPermission(int controlId);
        List<ControlPermission> GetControlCachedPermissions(int controlId, PermissionTypes type);
    }
}
