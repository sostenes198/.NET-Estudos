using System;

namespace Estudos.Selenium
{
    public class Periodo
    {
        public Periodo(TimeSpan periodoInicial, TimeSpan periodoFinal)
        {
            PeriodoInicial = periodoInicial;
            PeriodoFinal = periodoFinal;
        }

        public Periodo()
        {
        }

        public TimeSpan PeriodoInicial { get; set; }
        public TimeSpan PeriodoFinal { get; set; }
    }
}