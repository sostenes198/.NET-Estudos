using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Testes
{
    public static class RegisterWindows
    {
        public static void Register()
        {
            var read = CanReadRegister(@"SYSTEM\WPA\8DEC0AF1-0341-4b93-85CD-72606C2DF94C-7P-101");
            var write = CanWriteRegister(@"SYSTEM\WPA\8DEC0AF1-0341-4b93-85CD-72606C2DF94C-7P-101");
        }

        public static bool CanReadRegister(string key)
        {
            try
            {
                Registry.LocalMachine.OpenSubKey(key, true).Close();
                var registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, key);
                registryPermission.Demand();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        public static bool CanWriteRegister(string key)
        {
            try
            {
                var registryPermission = new RegistryPermission(RegistryPermissionAccess.Write, key);
                registryPermission.Demand();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }
    }
}