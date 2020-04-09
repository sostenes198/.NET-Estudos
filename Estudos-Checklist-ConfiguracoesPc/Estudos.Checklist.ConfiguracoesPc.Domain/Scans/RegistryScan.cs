using System;
using System.Security;
using System.Threading.Tasks;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Registry;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.Scans
{
    public class RegistryScan : IScan
    {
        private readonly string[] _registries;
        private readonly RegistriesScanResult _registriesScanResult;

        public RegistryScan(params string[] registries)
        {
            _registries = registries;
            _registriesScanResult = new RegistriesScanResult();
        }
        
        
        public Task<IScanResult> ScanAsync()
        {
            foreach (var registry in _registries)
            {
                if (ExistRegistry(registry))
                    _registriesScanResult.AddRegistryScanResult(registry, true, CanReadRegister(registry), CanWriteRegister(registry));
                else
                    _registriesScanResult.AddRegistryScanResult(registry, false, false, false);
            }

            return Task.FromResult<IScanResult>(_registriesScanResult);
        }

        private bool ExistRegistry(string registryKey)
        {
            try
            {
                //Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey).Close();
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        private bool CanReadRegister(string registryKey)
        {
            try
            {
                // Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey).Close();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        private bool CanWriteRegister(string registryKey)
        {
            try
            {
                // Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey, true).Close();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }
    }
}