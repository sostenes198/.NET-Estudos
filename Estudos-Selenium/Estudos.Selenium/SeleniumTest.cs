using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Estudos.Selenium
{
    public class SeleniumTest
    {
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
            if (periodos.Select(lnq => lnq.PeriodoFinal.Subtract(lnq.PeriodoInicial)).Aggregate(TimeSpan.Zero, (acc, nextObj) => acc.Add(nextObj)).CompareTo(new TimeSpan(6, 0, 0)) <= 0)
                return;

            var meioDia = new TimeSpan(12, 0, 0);


            (int indicePeriodo, TimeSpan valorPeriodo) valorPeriodoTarde = (0, new TimeSpan(Math.Abs(periodos[0].PeriodoInicial.Ticks - meioDia.Ticks)));
            (int indicePeriodo, TimeSpan valorPeriodo) valorPeriodoManha = (0, new TimeSpan(Math.Abs(periodos[0].PeriodoFinal.Ticks - meioDia.Ticks)));
            for (var i = 1; i < periodos.Count; i++)
            {
                var periodoTarde = new TimeSpan(Math.Abs(periodos[i].PeriodoInicial.Ticks - meioDia.Ticks));
                var periodoManha = new TimeSpan(Math.Abs(periodos[i].PeriodoFinal.Ticks - meioDia.Ticks));

                if (periodoTarde.CompareTo(valorPeriodoTarde.valorPeriodo) == -1)
                    valorPeriodoTarde = (i, periodoTarde);

                if (periodoManha.CompareTo(valorPeriodoManha.valorPeriodo) == -1)
                    valorPeriodoManha = (i, periodoManha);
            }

            if (valorPeriodoTarde.indicePeriodo == valorPeriodoManha.indicePeriodo)
                ConstruiPeriodoAlmoco(periodos, valorPeriodoTarde.indicePeriodo);
            else
                DefinirPeriodoAlmoco(periodos, valorPeriodoManha.indicePeriodo, valorPeriodoTarde.indicePeriodo);
        }

        private void ObterPeriodoTrabalhado(List<Periodo> periodos)
        {
            var periodoTrabalhado = periodos.Select(lnq => lnq.PeriodoFinal.Subtract(lnq.PeriodoInicial))
                .Aggregate(new TimeSpan(0, 0, 0), (acc, periodo) => acc.Add(periodo));

            //if(periodoTrabalhado.CompareTo(previsaoATrabalhar) == 1)
            //{
            //    periodoTrabalhado.Subtract()
            //}
        }

        private void ConstruiPeriodoAlmoco(List<Periodo> periodoAlmoco, int indice)
        {
            var meioDia = new TimeSpan(12, 0, 0);

            var periodoParteTarde = new Periodo(periodoAlmoco[indice].PeriodoFinal.Subtract(periodoAlmoco[indice].PeriodoFinal.Subtract(periodoAlmoco[indice].PeriodoInicial).Divide(2)), periodoAlmoco[indice].PeriodoFinal);
            periodoAlmoco[indice].PeriodoFinal = periodoParteTarde.PeriodoInicial.Subtract(new TimeSpan(1, 0, 0));
            periodoAlmoco.Insert(indice + 1, periodoParteTarde);
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
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 14, 0), new TimeSpan(9, 4, 0)), new Periodo(new TimeSpan(9, 7, 0), new TimeSpan(12, 1, 0)), new Periodo(new TimeSpan(12, 23, 0), new TimeSpan(16, 29, 0)), new Periodo(new TimeSpan(17, 9, 0), new TimeSpan(19, 3, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 14, 0), new TimeSpan(9, 4, 0)), new Periodo(new TimeSpan(9, 7, 0), new TimeSpan(12, 1, 0)), new Periodo(new TimeSpan(13, 1, 0), new TimeSpan(16, 29, 0)), new Periodo(new TimeSpan(17, 9, 0), new TimeSpan(19, 3, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 9, 0), new TimeSpan(9, 14, 0)), new Periodo(new TimeSpan(9, 16, 0), new TimeSpan(12, 18, 0)), new Periodo(new TimeSpan(12, 58, 0), new TimeSpan(18, 39, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 9, 0), new TimeSpan(9, 14, 0)), new Periodo(new TimeSpan(9, 16, 0), new TimeSpan(12, 18, 0)), new Periodo(new TimeSpan(13, 18, 0), new TimeSpan(18, 39, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 6, 0), new TimeSpan(12, 28, 0)), new Periodo(new TimeSpan(13, 46, 0), new TimeSpan(17, 20, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 6, 0), new TimeSpan(12, 28, 0)), new Periodo(new TimeSpan(13, 46, 0), new TimeSpan(17, 20, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)), new Periodo(new TimeSpan(9, 15, 0), new TimeSpan(13, 0, 0)), new Periodo(new TimeSpan(13, 20, 0), new TimeSpan(18, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)), new Periodo(new TimeSpan(9, 15, 0), new TimeSpan(13, 0, 0)), new Periodo(new TimeSpan(14, 0, 0), new TimeSpan(18, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(11, 30, 0)), new Periodo(new TimeSpan(12, 30, 0), new TimeSpan(17, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(15, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(10, 30, 0)), new Periodo(new TimeSpan(11, 30, 0), new TimeSpan(15, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(14, 30, 0)), new Periodo(new TimeSpan(15, 30, 0), new TimeSpan(20, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(20, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(15, 0, 0)), new Periodo(new TimeSpan(16, 0, 0), new TimeSpan(20, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(11, 0, 0), new TimeSpan(14, 30, 0)), new Periodo(new TimeSpan(15, 30, 0), new TimeSpan(20, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(9, 0, 0), new TimeSpan(20, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(9, 0, 0), new TimeSpan(13, 30, 0)), new Periodo(new TimeSpan(14, 30, 0), new TimeSpan(20, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0))}},
                new object[] {new List<Periodo> {new Periodo(new TimeSpan(5, 0, 0), new TimeSpan(12, 0, 0))}, new List<Periodo> {new Periodo(new TimeSpan(5, 0, 0), new TimeSpan(7, 30, 0)), new Periodo(new TimeSpan(8, 30, 0), new TimeSpan(12, 0, 0))}}
            };

        [Fact]
        public void Selenium_Teste()
        {
            //new WebDriverWait(_driver, new TimeSpan(0,0 ).Until()
            var _driver = new ChromeDriver(Environment.CurrentDirectory);
            try
            {
                //_driver.Navigate().GoToUrl("http://sqhoras:8085");
                _driver.Navigate().GoToUrl("http://www.squadra.com.br/sqhoras/");
                _driver.FindElementById("spnusuario").SendKeys(Constantes.Login);
                _driver.FindElementById("spnsenha").SendKeys(Constantes.Senha);
                _driver.FindElementById("acaoBotao").Click();

                //_driver.Navigate().GoToUrl("http://sqhoras:8085/timesheetColaborador.asp?date=5/22/2019");
                _driver.Navigate().GoToUrl("http://www.squadra.com.br/sqhoras/timesheetColaborador.asp?date=5/22/2019");

                var periodosApropriados = _driver.FindElementsByXPath("(//td[@cor='#0000ff']/..)//td[@height='1']").Select(lnq => TimeSpan.Parse(lnq.Text)).ToList();
                if (periodosApropriados.Any())
                    throw new Exception();

                _driver.FindElementByXPath("//td[@nomealocacao='28563-SOFTPLAN - SQUAD 1 - NÚCLEO BH .NET/ATUAÇÃO TIME 2019/ANALISTA DESENVOLVEDOR']").Click();

                var periodosAcessoAEmpresa = _driver.FindElementsByXPath("(//td[@cor='#000000']/..)//td[@height='1']").Select(lnq => TimeSpan.Parse(lnq.Text)).ToList();

                if (periodosAcessoAEmpresa.Count % 2 != 0)
                {
                    _driver.ExecuteScript("alert('Sua régua está bugada amigão !!!')");

                    Console.WriteLine("Sua Regua ta bugadassa !!!");

                    Thread.Sleep(3000);
                    throw new Exception();
                }

                var previsaoATrabalhar = TimeSpan.Parse(_driver.FindElementByXPath("//td/b/font[@color='RED'][@class='texto2']").Text.Split(" ")[0]);

                var periodos = ListarPeriodos(periodosAcessoAEmpresa);
                ObterIntervaloAlmoco(periodos);
                ObterPeriodoTrabalhado(periodos);


                _driver.Quit();
            }
            catch (Exception ex)
            {
                _driver.Quit();
                true.Should().BeFalse(ex.ToString());
            }
        }
    }
}