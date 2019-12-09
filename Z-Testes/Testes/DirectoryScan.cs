using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Testes
{
    public class DirectoryScan
    {
        public void LerDiretorio(string folderPath)
        {
            try
            {
                var x = Directory.GetFiles(folderPath);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public void CriarDiretorio(string folderPath)
        {
            try
            {
                var x = Directory.CreateDirectory(folderPath);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (Exception)
            {
            }
        }

        public bool CheckFolderPermission(string folderPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            try
            {
                DirectorySecurity dirAC = dirInfo.GetAccessControl(AccessControlSections.All);
                return true;
            }
            catch (PrivilegeNotHeldException)
            {
                return false;
            }
        }
        
        public void PermissaoDiretorio(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectorySecurity acl = di.GetAccessControl(AccessControlSections.All);
            AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(NTAccount));

            foreach (var rule in rules)
            {
                var filesystemAccessRule = (FileSystemAccessRule) rule;

                //Cast to a FileSystemAccessRule to check for access rights
                if ((filesystemAccessRule.FileSystemRights & FileSystemRights.WriteData) > 0 && filesystemAccessRule.AccessControlType != AccessControlType.Deny)
                {
                    Console.WriteLine($"{"AS"} has write access to {path}");
                }
                else
                {
                    Console.WriteLine($"{"AS"} does not have write access to {path}");
                }
                
                if ((filesystemAccessRule.FileSystemRights & FileSystemRights.ReadData) > 0 && filesystemAccessRule.AccessControlType != AccessControlType.Deny)
                {
                    Console.WriteLine($"{"AS"} has write access to {path}");
                }
                else
                {
                    Console.WriteLine($"{"AS"} does not have write access to {path}");
                }
            }
        }
    }
}