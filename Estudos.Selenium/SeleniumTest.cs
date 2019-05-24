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
                _driver.Navigate().GoToUrl("http://sqhoras:8085");
                //_driver.Navigate().GoToUrl("http://www.squadra.com.br/sqhoras/");
                _driver.FindElementById("spnusuario").SendKeys(Constantes.Login);
                _driver.FindElementById("spnsenha").SendKeys(Constantes.Senha);
                _driver.FindElementById("acaoBotao").Click();

                _driver.Navigate().GoToUrl("http://sqhoras:8085/timesheetColaborador.asp?date=5/22/2019");
                //_driver.Navigate().GoToUrl("http://www.squadra.com.br/sqhoras/timesheetColaborador.asp?date=5/22/2019");
                _driver.FindElementByXPath("//td[@nomealocacao='28563-SOFTPLAN - SQUAD 1 - NÚCLEO BH .NET/ATUAÇÃO TIME 2019/ANALISTA DESENVOLVEDOR']").Click();

                var elementos = _driver.FindElementsByXPath("//td[@height='1']").Select(lnq => TimeSpan.Parse(lnq.Text)).ToList();

                if (elementos.Count % 2 != 0)
                {
                    _driver.ExecuteScript("alert('Sua régua está bugada amigão !!!')");
                    Thread.Sleep(3000);
                    throw new Exception();
                }

                var periodos = ListarPeriodos(elementos);
                ObterIntervaloAlmoco(periodos);
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
        [MemberData(nameof(Cenarios_Intervalo_Periodo_Almoco))]
        public void Deve_Obter_Intervalo_Periodo_Almoco_Valido(List<Periodo> periodos, List<Periodo> periodosValido)
        {
            ObterIntervaloAlmoco(periodos);

            periodos.Should().BeEquivalentTo(periodosValido);
        }

        private List<Periodo> ListarPeriodos(List<TimeSpan> elementos)
        {
            var periodos = new List<Periodo>();
            for (var i = 0; i < elementos.Count; i += 2)
                periodos.Add(new Periodo(elementos[i], elementos[i + 1]));

            return periodos;
        }

        private void ObterIntervaloAlmoco(List<Periodo> periodos)
        {
            var meioDia = new TimeSpan(12, 0, 0);

            var periodoParteTardeMaisProximoMeioDia = periodos.Select(lnq => lnq.PeriodoInicial).Any(lnq => lnq.CompareTo(meioDia) >= 0)
                            ? periodos.Select(lnq => lnq.PeriodoInicial).FirstOrDefault(lnq => lnq.CompareTo(meioDia) >= 0)
                            : periodos.Select(lnq => lnq.PeriodoFinal).FirstOrDefault(lnq => lnq.CompareTo(meioDia) >= 0);

            var periodoParteManhaMaisProximoMeioDia = periodos.Select(lnq => lnq.PeriodoFinal).Any(lnq => lnq.CompareTo(meioDia) <= 0)
                            ? periodos.Select(lnq => lnq.PeriodoFinal).FirstOrDefault(lnq => lnq.CompareTo(meioDia) <= 0)
                            : periodos.Select(lnq => lnq.PeriodoInicial).FirstOrDefault(lnq => lnq.CompareTo(meioDia) <= 0);

            var indiceParteManha = periodos.IndexOf(periodos.First(lnq => (lnq.PeriodoFinal.CompareTo(periodoParteManhaMaisProximoMeioDia) == 0) || (lnq.PeriodoInicial.CompareTo(periodoParteManhaMaisProximoMeioDia) == 0)));
            var indiceParteTarde = periodos.IndexOf(periodos.First(lnq => (lnq.PeriodoInicial.CompareTo(periodoParteTardeMaisProximoMeioDia) == 0) || (lnq.PeriodoFinal.CompareTo(periodoParteTardeMaisProximoMeioDia) == 0)));

            if (indiceParteManha == indiceParteTarde)
                ConstruiPeriodoAlmoco(periodos, indiceParteManha);

            else
                DefinirPeriodoAlmoco(periodos, indiceParteManha, indiceParteTarde);

        }

        private void ConstruiPeriodoAlmoco(List<Periodo> periodoAlmoco, int indice)
        {
            var meioDia = new TimeSpan(12, 0, 0);
            if (periodoAlmoco[indice].PeriodoFinal.Subtract(periodoAlmoco[indice].PeriodoInicial).CompareTo(new TimeSpan(6, 0, 0)) <= 0)
                //&& periodoAlmoco.Select(lnq => lnq.PeriodoFinal.Subtract(lnq.PeriodoInicial)).Any(lnq => lnq.CompareTo(new TimeSpan(1,0,0)) >= 0))
                return;

            var periodoParteTarde = new Periodo(periodoAlmoco[indice].PeriodoFinal.Subtract(periodoAlmoco[indice].PeriodoFinal.Subtract(periodoAlmoco[indice].PeriodoInicial).Divide(2)), periodoAlmoco[indice].PeriodoFinal);
            periodoAlmoco[indice].PeriodoFinal = periodoParteTarde.PeriodoInicial.Subtract(new TimeSpan(1, 0, 0));
            periodoAlmoco.Insert(indice + 1, periodoParteTarde);

            //if (periodoAlmoco[indice].PeriodoInicial.CompareTo(meioDia) == -1 
            //    && (meioDia.Subtract(periodoAlmoco[indice].PeriodoInicial).CompareTo(new TimeSpan(2, 0, 0)) >= 0))
            //{
            //    var periodoParteTarde = new Periodo(periodoAlmoco[indice].PeriodoFinal.Subtract(new TimeSpan(1, 0, 0)), periodoAlmoco[indice].PeriodoFinal);
            //    periodoAlmoco[indice].PeriodoFinal = periodoParteTarde.PeriodoFinal.Subtract(new TimeSpan(2, 0, 0));
            //    periodoAlmoco.Insert(indice + 1, periodoParteTarde);
            //}
            //else
            //{

            //}
        }

        private void DefinirPeriodoAlmoco(List<Periodo> periodos, int indiceParteManha, int indiceParteTarde)
        {
            var umaHora = new TimeSpan(1, 0, 0);

            if (periodos[indiceParteTarde].PeriodoInicial.Subtract(periodos[indiceParteManha].PeriodoFinal).CompareTo(umaHora) == -1)
                periodos[indiceParteTarde].PeriodoInicial = periodos[indiceParteManha].PeriodoFinal.Add(umaHora);

        }

        public static IEnumerable<object[]> Cenarios_Intervalo_Periodo_Almoco =>
            new List<object[]>
            {                
                 new object[]{new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(11, 30, 0)), new Periodo(new TimeSpan(12, 30, 0), new TimeSpan(17, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(15, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)), new Periodo(new TimeSpan(13, 0, 0), new TimeSpan(15, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0)), new Periodo(new TimeSpan(14, 0, 0), new TimeSpan(20, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(20, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(15, 0, 0)), new Periodo(new TimeSpan(16, 0, 0), new TimeSpan(20, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(14, 30, 0)), new Periodo(new TimeSpan(15, 30, 0), new TimeSpan(20, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(9, 0, 0), new TimeSpan(20, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(14, 30, 0)), new Periodo(new TimeSpan(15, 30, 0), new TimeSpan(20, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)) } },
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0)) } },                 
                 //new object[]{new List<Periodo> { new Periodo(new TimeSpan(5, 0, 0), new TimeSpan(12, 0, 0)) }, new List<Periodo> { new Periodo(new TimeSpan(5, 0, 0), new TimeSpan(10, 0, 0)), new Periodo(new TimeSpan(11,0,0), new TimeSpan(12,0,0)) } },
            };
    }
}
