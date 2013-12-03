using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainProjectGenerator
{
    public delegate void RaiseMessageEventHandler(TextMessageEventArg e);

    public static class DirectoryService
    {
        public static event RaiseMessageEventHandler RaiseMessageEvent;

        static void RaiseSampleEvent(string message)
        {
            if (RaiseMessageEvent != null)
                RaiseMessageEvent(new TextMessageEventArg(message));
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, string className, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string newFileName = InsertNewFileName(file.Name, className);
                string temppath = Path.Combine(destDirName, newFileName);
                file.CopyTo(temppath, false);
                RaiseSampleEvent("Created File : " + newFileName);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (!subdir.Name.ToLower().Equals("bin") && !subdir.Name.ToLower().Equals("obj") && !subdir.Name.ToLower().Equals("packages"))
                    {
                        string newDirName = InsertNewFileName(subdir.Name, className);
                        string temppath = Path.Combine(destDirName, newDirName);
                        DirectoryCopy(subdir.FullName, temppath, className, copySubDirs);
                        RaiseSampleEvent("Created directory : " + newDirName);
                    }
                }
            }
        }

        private static string InsertNewFileName(string p, string className)
        {
            string result = string.Empty;
            result = p.Replace("Template", className);
            return result;
        }

        internal static void ChangeStringsInFiles(string className, string folderName)
        {
            DirectoryInfo di = new DirectoryInfo(folderName);
            DirectoryInfo[] dirs = di.GetDirectories();

            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                if (!Path.GetExtension(file.Name).ToLower().Equals(".suo")
                    && !Path.GetExtension(file.Name).ToLower().Equals(".dll")
                    && !Path.GetExtension(file.Name).ToLower().Equals(".pubxml")
                    && !Path.GetExtension(file.Name).ToLower().Equals(".pubxml.user"))
                {
                    ReplaceStringInFile(className, file, "Template");
                    ReplaceStringInFile(className, file, "templatenoun");
                    RaiseSampleEvent("Looked at File : " + file.FullName);
                }
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                ChangeStringsInFiles(className, subdir.FullName);
            }
        }

        private static void ReplaceStringInFile(string className, FileInfo file, string name)
        {
            string str = File.ReadAllText(file.FullName);
            str = str.Replace(name, className);
            File.WriteAllText(file.FullName, str);
        }
    }
}
