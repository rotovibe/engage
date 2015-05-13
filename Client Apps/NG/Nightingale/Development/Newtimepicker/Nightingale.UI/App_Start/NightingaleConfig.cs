using System;
using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(
    typeof(Nightingale.Site.App_Start.NightingaleConfig), "PreStart")]

namespace Nightingale.Site.App_Start
{
    public static class NightingaleConfig
    {
        public static void PreStart()
        {
            // Add your start logic here
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}