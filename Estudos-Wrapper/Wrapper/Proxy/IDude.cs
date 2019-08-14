namespace Wrapper.Proxy
{
    public interface IDude
    {
        string Name { get; set; }
        void GotShot(string typeOfGun);
    }
}