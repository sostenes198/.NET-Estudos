using System.Collections;
using System.Collections.Generic;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Directory
{
    public class DirectoriesScanResult : IEnumerable<DirectoryScanResult>, IScanResult
    {
        private readonly ICollection<DirectoryScanResult> _resultRegistryScan;

        public DirectoriesScanResult()
        {
            _resultRegistryScan = new HashSet<DirectoryScanResult>();
        }

        public void AddRegistryScanResult(string name, bool found, bool canRead, bool canWrite)
        {
            _resultRegistryScan.Add(new DirectoryScanResult(name, found, canRead, canWrite));
        }

        public IEnumerator<DirectoryScanResult> GetEnumerator()
        {
            return _resultRegistryScan.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}