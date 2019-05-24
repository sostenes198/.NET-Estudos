using FluentAssertions;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Estudos.Selenium
{
    public class SeleniumTest
    {
        public SeleniumTest()
        {

        }

        [Fact]
        public void Selenium_Teste()
        {
            ChromeDriver _driver = new ChromeDriver(Environment.CurrentDirectory);
            try
            {
                //_driver.Navigate().GoToUrl("http://sqhoras:8085");
                _driver.Navigate().GoToUrl("http://www.squadra.com.br/sqhoras/");
                _driver.FindElementById("spnusuario").SendKeys(Constantes.Login);
                _driver.FindElementById("spnsenha").SendKeys(Constantes.Senha);
                _driver.FindElementById("acaoBotao").Click();

                //_driver.Navigate().GoToUrl("http://sqhoras:8085/timesheetColaborador.asp?date=5/22/2019");
                _driver.Navigate().GoToUrl("http://www.squadra.com.br/sqhoras/timesheetColaborador.asp?date=5/22/2019");
                _driver.FindElementByXPath("//td[@nomealocacao='28563-SOFTPLAN - SQUAD 1 - NÚCLEO BH .NET/ATUAÇÃO TIME 2019/ANALISTA DESENVOLVEDOR']").Click();

                var elementos = _driver.FindElementsByXPath("//td[@height='1']").Select(lnq => TimeSpan.Parse(lnq.Text)).ToList();

                if(elementos.Count % 2 != 0)
                {
                    _driver.ExecuteScript("alert('Sua régua está bugada amigão !!!')");
                    Thread.Sleep(3000);
                    throw new Exception();
                }

                var periodos = ListarPeriodos(elementos);
                ObterMaiorIntervalo(periodos);
                //var maiorPeriodo = ObterMaiorPeriodoDeTempo(periodos);



            }
            catch
            {
            }
            finally
            {
                _driver.Quit();
            }

        }

        [Theory]
        [MemberData(nameof(Cenarios_Contruir_Periodo_Almoco))]
        public void Deve_Construir_Periodo_Almoco_Valido(List<Periodo> periodos, List<Periodo> periodosValido)
        {
            var indice = 0;

            ConstruiPeriodoAlmoco(periodos, indice);

            periodos.Should().BeEquivalentTo(periodosValido);
        }

        public void Deve_Definir_Periodo_Almoco_Valido()
        {

        }

        private List<Periodo> ListarPeriodos(List<TimeSpan> elementos)
        {
            var periodos = new List<Periodo>();
            for(var i = 0; i < elementos.Count; i += 2)
                periodos.Add(new Periodo(elementos[i], elementos[i + 1]));

            return periodos;
        }

        private void ObterMaiorIntervalo(List<Periodo> periodos)
        {
            var meioDia = new TimeSpan(12, 0, 0);

            var valorIniciallMaisProximoMeioDia = periodos.Select(lnq => lnq.PeriodoInicial).First(lnq => lnq.CompareTo(meioDia) >= 0);
            var valorFinallMaisProximoMeioDia = periodos.Select(lnq => lnq.PeriodoFinal).First(lnq => meioDia.CompareTo(lnq) <= 0);

            var indiceParteManha = periodos.IndexOf(periodos.First(lnq => lnq.PeriodoFinal.CompareTo(valorFinallMaisProximoMeioDia) == 0));
            var indiceParteTarde = periodos.IndexOf(periodos.First(lnq => lnq.PeriodoInicial.CompareTo(valorIniciallMaisProximoMeioDia) == 0));

            if(indiceParteManha == indiceParteTarde)
                ConstruiPeriodoAlmoco(periodos, indiceParteManha);

            else
                DefinirPeriodoAlmoco(periodos, indiceParteManha, indiceParteTarde);

        }

        private void ConstruiPeriodoAlmoco(List<Periodo> periodoAlmoco, int indice)
        {
            var meioDia = new TimeSpan(12, 0, 0);
            if(periodoAlmoco[indice].PeriodoFinal.Subtract(periodoAlmoco[indice].PeriodoInicial).CompareTo(new TimeSpan(6, 0, 0)) <= 0)
                return;

            if(periodoAlmoco[indice].PeriodoInicial.CompareTo(meioDia) == -1 && (meioDia.Subtract(periodoAlmoco[indice].PeriodoInicial).CompareTo(new TimeSpan(2, 0, 0)) >= 0))
            {
                var periodoParteTarde = new Periodo(new TimeSpan(13, 0, 0), periodoAlmoco[indice].PeriodoFinal);
                periodoAlmoco[indice].PeriodoFinal = new TimeSpan(12, 0, 0);
                periodoAlmoco.Insert(indice + 1, periodoParteTarde);
            }
            else
            {
                var periodoParteTarde = new Periodo(periodoAlmoco[indice].PeriodoInicial.Add(new TimeSpan(3, 0, 0)), periodoAlmoco[indice].PeriodoFinal);
                periodoAlmoco[indice].PeriodoFinal = periodoParteTarde.PeriodoInicial.Subtract(new TimeSpan(1, 0, 0));
                periodoAlmoco.Insert(indice + 1, periodoParteTarde);
            }
        }

        private void DefinirPeriodoAlmoco(List<Periodo> periodos, int indiceParteManha, int indiceParteTarde)
        {
            var umaHora = new TimeSpan(1, 0, 0);

            if(periodos[indiceParteTarde].PeriodoInicial.Subtract(periodos[indiceParteManha].PeriodoFinal).CompareTo(umaHora) == -1)
                periodos[indiceParteTarde].PeriodoInicial = periodos[indiceParteManha].PeriodoFinal.Add(umaHora);

        }

        public static IEnumerable<object[]> Cenarios_Contruir_Periodo_Almoco =>
            new List<object[]>
            {
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)), new Periodo(new TimeSpan(13, 0, 0), new TimeSpan(17, 0, 0)) } },
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(15, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)), new Periodo(new TimeSpan(13, 0, 0), new TimeSpan(15, 0, 0)) } },
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0)), new Periodo(new TimeSpan(14, 0, 0), new TimeSpan(20, 0, 0)) } },
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(20, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(14, 0, 0)), new Periodo(new TimeSpan(15, 0, 0), new TimeSpan(20, 0, 0)) } },
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)) } },
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0)) } },                 
            };
    }
}
