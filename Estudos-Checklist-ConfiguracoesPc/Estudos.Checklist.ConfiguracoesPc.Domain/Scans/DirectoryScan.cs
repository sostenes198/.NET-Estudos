using System;
using System.IO;
using System.Threading.Tasks;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Directory;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.Scans
{
    public class DirectoryScan : IScan
    {
        private readonly string[] _directories;
        private readonly DirectoriesScanResult _directoriesScanResult;

        public DirectoryScan(params string[] directories)
        {
            _directories = directories;
            _directoriesScanResult = new DirectoriesScanResult();
        }

        public Task<IScanResult> ScanAsync()
        {
            foreach (var directory in _directories)
            {
                if (ExisteDirectory(directory))
                    _directoriesScanResult.AddRegistryScanResult(directory, true, CanReadDirectory(directory), CanWriteDirectory(directory));
                else
                    _directoriesScanResult.AddRegistryScanResult(directory, false, false, false);
            }

            return Task.FromResult<IScanResult>(_directoriesScanResult);
        }

        private bool ExisteDirectory(string pathDirectory)
        {
            return Directory.Exists(pathDirectory);
        }

        private bool CanReadDirectory(string pathDirectory)
        {
            try
            {
                Directory.GetDirectories(pathDirectory);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private bool CanWriteDirectory(string pathDirectory)
        {
            var fileCreate = $@"{pathDirectory}\file-open";
            try
            {
                var file = File.Create(fileCreate);
                file.Close();
                File.Delete(fileCreate);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}