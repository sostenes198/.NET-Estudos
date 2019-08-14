namespace Wrapper.Interacao_Proxy
{
    public interface IInteractor
    {
        void Percieve(string percievedThing);
        void Change(ref string perceivedThing);
    }
}