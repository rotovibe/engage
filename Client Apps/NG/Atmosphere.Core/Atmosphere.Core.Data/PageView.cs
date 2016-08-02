using Phytel.Framework.SQL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C3.Data
{

    [Serializable]
    public class PageView
    {
        public int ViewId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? UserId { get; set; }

        public int ControlId { get; set; }

        public string PagePath { get; set; }

        public string PageName { get; set; }

        public int ContractId { get; set; }

        public string ViewContainer { get; set; }

        public bool IsPageDefaultView { get; set; }

        public bool IsUserDefaultView { get; set; }

        public static PageView Build(ITypeReader reader)
        {
            var pageView = new PageView
                               {
                                   ContractId = reader.GetInt("ContractId"),
                                   Name = reader.GetString("ViewName"),
                                   Description = reader.GetString("Description"),
                                   IsPageDefaultView = reader.GetBool("IsPageDefaultView"),
                                   IsUserDefaultView = reader.GetBool("IsUserDefaultView"),
                                   ControlId = reader.GetInt("ControlId"),
                                   PageName = reader.GetString("PageName"),
                                   PagePath = reader.GetString("PagePath"),
                                   UserId = reader.GetGuid("UserId"),
                                   ViewContainer = reader.GetString("ViewContainer"),
                                   ViewId = reader.GetInt("ViewId")
                               };
            return pageView;

        }

    }
}
