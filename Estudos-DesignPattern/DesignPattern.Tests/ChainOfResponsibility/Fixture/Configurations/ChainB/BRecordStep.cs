using System.Collections.Generic;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB
{
    public class BRecordStep
    {
        public IList<string> Records { get; set; } = new List<string>();
    }
}