using System.Collections.Generic;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA
{
    public class ARecordStep
    {
        public IList<string> Records { get; set; } = new List<string>();
    }
}