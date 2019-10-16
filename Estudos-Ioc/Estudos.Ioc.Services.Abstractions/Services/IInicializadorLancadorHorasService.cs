namespace Estudos.Ioc.Services.Abstractions.Services
{
    public interface IInicializadorLancadorHorasService
    {
        bool InicializarAplicacao(out string mensagemValidacao);
    }
}