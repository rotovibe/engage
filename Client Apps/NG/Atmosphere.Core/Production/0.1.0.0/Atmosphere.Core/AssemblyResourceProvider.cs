using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Hosting;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Atmosphere
{
    public class AssemblyResourceProvider : System.Web.Hosting.VirtualPathProvider
    {
        public const string APP_RESOURCE = "/App_Resource/";
        private const string ASSET = "{asset_root}";

        public AssemblyResourceProvider() { }

        private bool IsAppResourcePath(string virtualPath)
        {
            String checkPath =
               VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.IndexOf(APP_RESOURCE, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool IsAssetPath(string virtualPath)
        {
            String checkPath =
                          VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(ASSET, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsAppResourcePath(virtualPath) || base.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
                return new AssemblyResourceVirtualFile(virtualPath);
            else
                return base.GetFile(virtualPath);
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return null;
            }
            else
            {
                if (IsAssetPath(virtualPath))
                {
                    virtualPath = virtualPath.Replace(ASSET, ConfigurationManager.AppSettings["Asset_Root"]);
                }

                List<string> vpDependencies = new List<string>(virtualPathDependencies.Cast<string>());

                if (vpDependencies.Any(vp => vp.IndexOf(APP_RESOURCE, StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    List<string> tempPathList = new List<string>();

                    for (int i = 0; i < vpDependencies.Count; i++)
                    {
                        if (!(vpDependencies[i].IndexOf(APP_RESOURCE, StringComparison.InvariantCultureIgnoreCase) >= 0))
                        {
                            tempPathList.Add(vpDependencies[i]);
                        }
                    }

                    vpDependencies = tempPathList;
                }

                return base.GetCacheDependency(virtualPath, vpDependencies, utcStart);
            }
        }
    }

    internal class AssemblyResourceVirtualFile : VirtualFile
    {
        string path;

        public AssemblyResourceVirtualFile(string virtualPath) : base(virtualPath)
        {
            string vp = "~" + virtualPath.Substring(virtualPath.IndexOf(AssemblyResourceProvider.APP_RESOURCE));
            path = VirtualPathUtility.ToAppRelative(vp);
        }

        public override System.IO.Stream Open()
        {
            string[] parts = path.Split('/');
            string assemblyName = parts[2];
            string resourceName = parts[3];

            assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(assemblyName);

            if (assembly != null)
            {
                string fixedName = FixResourceCase(resourceName);

                Stream resource = assembly.GetManifestResourceStream(fixedName);
                return resource;
            }

            return null;
        }

        public string FixResourceCase(string resourceName)
        {
            var splitName = resourceName.Split(new char[] { '.' });

            StringBuilder fixedName = new StringBuilder();

            for(int i = 0; i < splitName.Length - 1; i++)
            {
                string s = splitName[i];
                fixedName.Append(char.ToUpper(s[0]));
                fixedName.Append(s.Substring(1));
                fixedName.Append(".");
            }

            string extension = splitName[splitName.Length - 1].ToLower();
            fixedName.Append(extension[0]);
            fixedName.Append(extension.Substring(1));

            return fixedName.ToString();
        }
    }

}
