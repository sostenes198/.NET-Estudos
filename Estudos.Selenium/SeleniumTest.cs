using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Estudos.Selenium
{
   public class SeleniumTest : IDisposable
    {
        private readonly ChromeDriver _driver;
        public SeleniumTest()
        {
            _driver = new ChromeDriver(Environment.CurrentDirectory);
        }

        [Fact]
        public void Selenium_Teste()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://sqhoras:8085");
                _driver.FindElementById("spnusuario").SendKeys(Constantes.Login);
                _driver.FindElementById("spnsenha").SendKeys(Constantes.Senha);
                _driver.FindElementById("acaoBotao").Click();

                _driver.Navigate().GoToUrl("http://sqhoras:8085/timesheetColaborador.asp?date=5/22/2019");
                _driver.FindElementByXPath("//td[@nomealocacao='28563-SOFTPLAN - SQUAD 1 - NÚCLEO BH .NET/ATUAÇÃO TIME 2019/ANALISTA DESENVOLVEDOR']").Click();

                var elementos = _driver.FindElementsByXPath("//td[@height='1']").Select(lnq => TimeSpan.Parse(lnq.Text)).ToList();

                if (elementos.Count % 2 != 0)
                {
                    _driver.ExecuteScript("alert('Sua régua está bugada amigão !!!')");
                    Thread.Sleep(3000);
                    throw new Exception();
                }

                var periodos = ListarPeriodos(elementos);
                var maiorPeriodo = ObterMaiorPeriodoDeTempo(periodos);



            }
            catch
            {
                _driver.Quit();
            }

        }

        public void Dispose()
        {
            _driver.Quit();
        }

        private List<Periodo> ListarPeriodos(List<TimeSpan> elementos)
        {
            var periodos = new List<Periodo>();
            for (var i = 0; i < elementos.Count; i += 2)
                periodos.Add(new Periodo(elementos[i], elementos[i + 1]));

            return periodos;
        }

        private (int indice, TimeSpan periodo) ObterMaiorPeriodoDeTempo(List<Periodo> periodos)
        {
            (int indice, TimeSpan periodo) periodo = (0, periodos[0].PeriodoFinal.Subtract(periodos[0].PeriodoInicial));
            for(var i = 1; i < periodos.Count; i++)
            {
                var periodoAtual = periodos[i].PeriodoFinal.Subtract(periodos[i].PeriodoInicial);
                if (periodoAtual.CompareTo(periodo.periodo) == 1)
                    periodo = (i, periodoAtual);
            }            

            return periodo;
        }
    }
}
