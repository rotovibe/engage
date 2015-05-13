using System;
using System.Web.Optimization;

namespace Nightingale.Site
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            /** Tell Bundling to use a CDN if it is available **/
            bundles.UseCdn = true;

            var jqueryCDNPath = "//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js";
            var jqueryuiCDNPath = "//ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js";
            var knockoutCDNPath = "//cdnjs.cloudflare.com/ajax/libs/knockout/3.0.0/knockout-min.js";
            var momentCDNPath = "//cdnjs.cloudflare.com/ajax/libs/moment.js/2.5.1/moment.min.js";

            /** JavaScript Bundles */
            
            /** Vendor scripts first from template*/
            bundles.Add(new ScriptBundle("~/bundles/jquery.js")
                        .Include("~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen.js")
                        .Include("~/Scripts/chosen/chosen.jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui.js").Include(
                        "~/Scripts/jqueryui/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout.js", knockoutCDNPath).Include(
                        "~/Scripts/knockout-{version}.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/moment.js", momentCDNPath).Include(
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor.js")
                        .Include("~/Scripts/accordion.js")
                        .Include("~/Scripts/Q.js")
                        .Include("~/Scripts/breeze.debug.js")
                        .Include("~/Scripts/breeze.ajaxpost2.js")
                        .Include("~/Scripts/typeahead.js"));

            /** Testing Cody's Venn Diagram stuff **/
            bundles.Add(new ScriptBundle("~/bundles/venn.js")
                        .Include("~/Scripts/venn/venn.js")
                        .Include("~/Scripts/venn/color.js")
                        .Include("~/Scripts/venn/mds.js")
                        .Include("~/Scripts/venn/underscore.js"));

            /** jQuery full calendar plug-in **/
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar.js")
                        .Include("~/Scripts/fullcalendar.js"));
            
            /** Vendor scripts first*/

            /** JavaScript Bundles */


            // TODO : Do we need this????
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
            /** Styling Bundles */

            /** ie10mobile style */
            bundles.Add(new StyleBundle("~/Content/ie10mobile").Include(
                        "~/Content/ie10mobile.css"));

            /** Vendor styles */
            bundles.Add(new StyleBundle("~/Content/nightingale/vendor").Include(
                        "~/Content/nightingale/fontcustom.css"));

            /** Normalize styles */
            bundles.Add(new StyleBundle("~/Content/normalize").Include(
                        "~/Content/normalize.css"));

            /** Nightingale styles */
            bundles.Add(new StyleBundle("~/Content/nightingale/Nightingale")
                        .Include("~/Content/nightingale/helveticaneue.new.css",
                                "~/Content/nightingale/application.css",
                                "~/Content/nightingale/overrides.css",
                                "~/Content/nightingale/controls.css"));

            /** Styling Bundles */
        }

    }
}