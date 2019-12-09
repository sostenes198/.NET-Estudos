using System.Collections;
using System.Collections.Generic;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Registry
{
    public class RegistriesScanResult : IEnumerable<RegistryScanResult>, IScanResult
    {
        private readonly ICollection<RegistryScanResult> _resultRegistryScan;

        public RegistriesScanResult()
        {
            _resultRegistryScan = new HashSet<RegistryScanResult>();
        }

        public void AddRegistryScanResult(string name, bool found, bool canRead, bool canWrite)
        {
            _resultRegistryScan.Add(new RegistryScanResult(name, found, canRead, canWrite));
        }

        public IEnumerator<RegistryScanResult> GetEnumerator()
        {
            return _resultRegistryScan.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}